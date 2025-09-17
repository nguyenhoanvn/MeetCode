-- =====================
-- Users & Auth
-- =====================
CREATE TABLE Users (                                                -- Define the Users table (accounts)
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),  -- Primary key as GUID; NEWSEQUENTIALID() = index-friendly GUIDs
    Email NVARCHAR(255) NOT NULL UNIQUE,                            -- User email; required; must be unique
    DisplayName NVARCHAR(100) NOT NULL,                             -- Public display name; required
    [Role] NVARCHAR(50) NOT NULL CHECK (Role IN ('admin','moderator','user')), -- Simple role string validated by CHECK
    PasswordHash VARCHAR(512) NOT NULL,                           -- Hashed password bytes (e.g., PBKDF2/Argon2 output)
    IsActive BIT NOT NULL DEFAULT 1,                                -- Soft-disable flag (1=active, 0=banned/deactivated)
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),          -- Creation timestamp in UTC
    LastLoginAt DATETIME2 NULL,                                     -- Last successful login time (nullable)
    AuthProvider NVARCHAR(50)                                       -- Login method (Google, Github,...)
);

-- =====================
-- Problems & Metadata
-- =====================
CREATE TABLE Problems (                                             -- Problem catalog (statements + limits)
    ProblemId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(), -- PK as sequential GUID
    Slug NVARCHAR(200) NOT NULL UNIQUE,                             -- URL-friendly unique identifier
    Title NVARCHAR(255) NOT NULL,                                   -- Problem title
    StatementMd NVARCHAR(MAX) NOT NULL,                             -- Problem statement in Markdown
    Difficulty NVARCHAR(20) NOT NULL CHECK (Difficulty IN ('easy','medium','hard')), -- Difficulty enum via CHECK
    TimeLimitMs INT NOT NULL CHECK (TimeLimitMs > 0),               -- Per-test CPU time limit (ms)
    MemoryLimitMb INT NOT NULL CHECK (MemoryLimitMb > 0),           -- Per-test memory limit (MB)
    CreatedBy UNIQUEIDENTIFIER NOT NULL,                            -- Author (FK to Users)
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),          -- Creation timestamp
    UpdatedAt DATETIME2 NULL,                                       -- Last edit time
    TotalSubmissionCount INT NOT NULL DEFAULT 0,                    -- Denominator for acceptance rate
    ScoreAcceptedCount INT NOT NULL DEFAULT 0,                      -- Numerator (count of AC submissions)
    AcceptanceRate AS (CAST(ScoreAcceptedCount AS FLOAT) / NULLIF(TotalSubmissionCount,0)) PERSISTED, 
                                                                    -- Computed column = AC/Total; guarded against divide-by-zero; PERSISTED for indexability
    IsActive BIT NOT NULL DEFAULT 1,                                -- Soft-hide problems without deleting
    CONSTRAINT FK_Problems_Users FOREIGN KEY (CreatedBy) REFERENCES Users(UserId) -- Enforce author exists
);

CREATE TABLE RefreshTokens (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    TokenHash NVARCHAR(200) NOT NULL,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL,
    IsRevoked BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE ProblemTags (                                          -- Master list of tags (e.g., Graph, DP)
    TagId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),   -- Tag PK
    Name NVARCHAR(100) NOT NULL UNIQUE                              -- Unique tag name
);

CREATE TABLE ProblemTagMap (                                        -- Many-to-many Problem↔Tag
    ProblemId UNIQUEIDENTIFIER NOT NULL,                            -- FK to Problems
    TagId UNIQUEIDENTIFIER NOT NULL,                                -- FK to ProblemTags
    PRIMARY KEY (ProblemId, TagId),                                 -- Composite PK prevents duplicates
    CONSTRAINT FK_ProblemTagMap_Problems 
        FOREIGN KEY (ProblemId) REFERENCES Problems(ProblemId) ON DELETE CASCADE, -- Remove mappings with problem
    CONSTRAINT FK_ProblemTagMap_Tags 
        FOREIGN KEY (TagId) REFERENCES ProblemTags(TagId) ON DELETE CASCADE       -- Remove mappings with tag
);

-- =====================
-- Execution Environment
-- =====================
CREATE TABLE Languages (                                            -- Supported languages/runtimes
    [LangId] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),  -- PK
    [Name] NVARCHAR(100) NOT NULL,                                    -- Human-readable name (e.g., C#, Python)
    [Version] NVARCHAR(50) NOT NULL,                                  -- Version string (e.g., 8.0, 3.12)
    RuntimeImage NVARCHAR(200) NOT NULL,                            -- Docker image reference for sandbox
    CompileCommand NVARCHAR(500) NULL,                              -- Optional template for compile step
    RunCommand NVARCHAR(500) NULL,                                  -- Optional template for run step
    IsEnabled BIT NOT NULL DEFAULT 1                                -- Toggle availability without data delete
);

-- =====================
-- Test Cases
-- =====================
CREATE TABLE TestCases (                                            -- Test cases per problem
    TestId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),  -- PK
    ProblemId UNIQUEIDENTIFIER NOT NULL,                            -- FK to Problems
    Visibility NVARCHAR(20) NOT NULL CHECK (Visibility IN ('sample','hidden','public')), 
                                                                    -- Visibility: shown to user or hidden
    InputText NVARCHAR(MAX) NOT NULL,                               -- Raw stdin content
    ExpectedOutputText NVARCHAR(MAX) NOT NULL,                      -- Expected stdout (exact/trim rules handled in code)
    [Weight] INT NOT NULL DEFAULT 1 CHECK (Weight > 0),             -- Score weight for this test
    CONSTRAINT FK_TestCases_Problems 
        FOREIGN KEY (ProblemId) REFERENCES Problems(ProblemId) ON DELETE CASCADE -- Delete tests when problem deleted
);

-- =====================
-- Submissions & Execution
-- =====================
CREATE TABLE Submissions (                                          -- Each user code submission
    SubmissionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(), -- PK
    UserId UNIQUEIDENTIFIER NOT NULL,                               -- Author of submission
    ProblemId UNIQUEIDENTIFIER NOT NULL,                            -- Target problem
    [LangId] UNIQUEIDENTIFIER NOT NULL,                             -- Language used
    Verdict NVARCHAR(30) NOT NULL CHECK (Verdict IN (               -- Lifecycle/verdict values
        'queued','running','accepted','wrong_answer',
        'tle','mle','runtime_error','compile_error','internal_error'
    )),
    SourceCode NVARCHAR(MAX) NOT NULL,                              -- Stored source (size limited at app layer)
    EnqueuedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),         -- Time job queued
    StartedAt DATETIME2 NULL,                                       -- When executor began running it
    FinishedAt DATETIME2 NULL,                                      -- When executor finished
    ExecTimeMs INT NULL CHECK (ExecTimeMs >= 0),                    -- Aggregate execution time (best/total per policy)
    MemoryKb INT NULL CHECK (MemoryKb >= 0),                        -- Peak memory observed (KB)
    Score FLOAT NULL CHECK (Score >= 0),                            -- Score computed from test weights
    ErrorLogRef NVARCHAR(500) NULL,                                 -- Pointer to detailed logs (blob path)
    Retries INT NOT NULL DEFAULT 0,                                 -- How many times rejudged/retried
    CONSTRAINT FK_Submissions_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),     -- User must exist
    CONSTRAINT FK_Submissions_Problems FOREIGN KEY (ProblemId) REFERENCES Problems(ProblemId), -- Problem must exist
    CONSTRAINT FK_Submissions_Languages FOREIGN KEY (LangId) REFERENCES Languages(LangId)      -- Language must exist
);

-- =====================
-- User Stats & Progress
-- =====================
CREATE TABLE UserStats (                                            -- Cached/aggregated user stats
    UserId UNIQUEIDENTIFIER PRIMARY KEY,                            -- 1:1 with Users(UserId)
    TotalSolved INT NOT NULL DEFAULT 0 CHECK (TotalSolved >= 0),    -- Count of distinct problems accepted
    TotalScore FLOAT NOT NULL DEFAULT 0 CHECK (TotalScore >= 0),    -- Sum of scores (if partial scoring)
    StreakDays INT NOT NULL DEFAULT 0 CHECK (StreakDays >= 0),      -- Consecutive days with ≥1 AC
    LastSubmissionAt DATETIME2 NULL,                                -- Last submit time
    Rating INT NOT NULL DEFAULT 1200,                               -- Optional ELO-style rating baseline
    CONSTRAINT FK_UserStats_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) -- Enforce 1:1 relation
);

-- =====================
-- Badges & Rewards
-- =====================
CREATE TABLE Badges (                                               -- Badge definitions/rules
    BadgeId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(), -- PK
    [Name] NVARCHAR(100) NOT NULL UNIQUE,                           -- Unique badge name
    RuleJson NVARCHAR(MAX) NOT NULL,                                -- JSON rule for awarding logic
    IconUrl NVARCHAR(500) NULL,                                     -- Optional icon URL/path
    Description NVARCHAR(MAX) DEFAULT 'Very long badge description' -- Description of badge
);

CREATE TABLE UserBadges (                                           -- Many-to-many User↔Badge awards
    UserId UNIQUEIDENTIFIER NOT NULL,                               -- FK to Users
    BadgeId UNIQUEIDENTIFIER NOT NULL,                              -- FK to Badges
    AwardedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),          -- When badge was granted
    PRIMARY KEY (UserId, BadgeId),                                  -- Prevent duplicate awards
    CONSTRAINT FK_UserBadges_Users  FOREIGN KEY (UserId)  REFERENCES Users(UserId)  ON DELETE CASCADE, -- Clean up on user delete
    CONSTRAINT FK_UserBadges_Badges FOREIGN KEY (BadgeId) REFERENCES Badges(BadgeId) ON DELETE CASCADE  -- Clean up on badge delete
);

-- =====================
-- Leaderboards & Logs
-- =====================
CREATE TABLE Leaderboards (                                         -- Periodic leaderboard snapshots
    LeaderboardId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(), -- PK
    [Period] NVARCHAR(20) NOT NULL CHECK (Period IN ('global','weekly','monthly')), -- Snapshot period
    SnapshotAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),         -- Timestamp of snapshot
    PayloadJson NVARCHAR(MAX) NOT NULL                              -- Cached/denormalized leaderboard data
);

CREATE TABLE AuditLogs (                                            -- Audit trail of actions
    AuditId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(), -- PK
    ActorUserId UNIQUEIDENTIFIER NOT NULL,                          -- Who performed the action
    Action NVARCHAR(100) NOT NULL,                                  -- Verb (e.g., 'CreateProblem', 'Login')
    Entity NVARCHAR(100) NOT NULL,                                  -- Entity type (e.g., 'Problem', 'Submission')
    EntityId UNIQUEIDENTIFIER NULL,                                 -- Specific entity key (nullable for global actions)
    Timestamp DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),          -- When it happened
    MetadataJson NVARCHAR(MAX) NULL,                                -- Extra context as JSON (IP, UA, payload diffs)
    CONSTRAINT FK_AuditLogs_Users FOREIGN KEY (ActorUserId) REFERENCES Users(UserId) -- Enforce actor exists
);

-- =====================
-- Indexes (performance)
-- =====================
CREATE INDEX IX_Problems_Difficulty_CreatedAt 
    ON Problems(Difficulty, CreatedAt DESC);                        -- Filter/sort problems by difficulty & recency

CREATE INDEX IX_Problems_Slug 
    ON Problems(Slug);                                              -- Speeds up slug lookups (even though UNIQUE)

CREATE INDEX IX_Submissions_Problem_User_FinishedAt 
    ON Submissions(ProblemId, UserId, FinishedAt DESC);             -- Fast per-user per-problem history (latest first)

CREATE INDEX IX_Users_Email 
    ON Users(Email);                                                -- Speeds up login & user search by email

CREATE INDEX IX_AuditLogs_Entity_Timestamp 
    ON AuditLogs(Entity, Timestamp DESC);                           -- Retrieve recent actions for an entity quickly

CREATE INDEX IX_Submissions_Verdict 
    ON Submissions(Verdict);                                        -- Useful for dashboards/metrics by verdict
