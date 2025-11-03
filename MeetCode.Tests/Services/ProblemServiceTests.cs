using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using MeetCode.Domain.Exceptions;
using MeetCode.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace MeetCode.Tests.Services
{
    public class ProblemServiceTests
    {
        private static readonly string TITLE_DUMMY = "Two sum";
        private static readonly string STATEMENTMD_DUMMY = "Markdown body";
        private static readonly string DIFFICULTY_DUMMY = "easy";
        private static readonly int TIMELIMIT_DUMMY = 1000;
        private static readonly int MEMORYLIMIT_DUMMY = 256;
        private static readonly Guid CREATEDBY_DUMMY = Guid.NewGuid();
        private static readonly List<Guid> TAGIDS_DUMMY = new List<Guid> { Guid.NewGuid() };


        private readonly Mock<IProblemRepository> _problemRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<ProblemService>> _loggerMock;
        private readonly IProblemService _problemService;

        public ProblemServiceTests()
        {
            _problemRepositoryMock = new();
            _tagRepositoryMock = new();
            _unitOfWorkMock = new();
            _loggerMock = new();
            _problemService = new ProblemService(
                _problemRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _tagRepositoryMock.Object,
                _loggerMock.Object
                );
        }

        [Fact]
        public async Task CreateProblemAsync_ShouldCreate_WhenValid()
        {
            var tags = new List<ProblemTag> { new ProblemTag { TagId = TAGIDS_DUMMY[0] } };

            _problemRepositoryMock
                .Setup(r => r.GetBySlugAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Problem?)null);

            _tagRepositoryMock
                .Setup(r => r.GetByIdsAsync(TAGIDS_DUMMY, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tags);

            _problemRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Problem>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _problemService.CreateProblemAsync(
                TITLE_DUMMY,
                STATEMENTMD_DUMMY,
                DIFFICULTY_DUMMY,
                TIMELIMIT_DUMMY,
                MEMORYLIMIT_DUMMY,
                CREATEDBY_DUMMY,
                TAGIDS_DUMMY,
                CancellationToken.None
                );

            Assert.NotNull(result);
            Assert.Equal(TITLE_DUMMY, result.Title);
            Assert.Equal(STATEMENTMD_DUMMY, result.StatementMd);
            Assert.Equal(DIFFICULTY_DUMMY, result.Difficulty);
            Assert.NotEmpty(result.Tags);

            _problemRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Problem>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateProblemAsync_ShouldThrowDuplicateException_WhenSlugExist()
        {
            var existingProblem = new Problem { Title = "existed" };
            _problemRepositoryMock
                .Setup(r => r.GetBySlugAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProblem);

            await Assert.ThrowsAsync<DuplicateEntityException<Problem>>(() =>
                _problemService.CreateProblemAsync(
                    "existed",
                    STATEMENTMD_DUMMY,
                    DIFFICULTY_DUMMY,
                    TIMELIMIT_DUMMY,
                    MEMORYLIMIT_DUMMY,
                    CREATEDBY_DUMMY,
                    TAGIDS_DUMMY,
                    CancellationToken.None
                ));

            _problemRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Problem>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateProblemAsync_ShouldThrowDbUpdateException_WhenSaveFails()
        {
            var tags = new List<ProblemTag> { new ProblemTag { TagId = TAGIDS_DUMMY[0] } };
            _problemRepositoryMock
                .Setup(r => r.GetBySlugAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Problem?)null);
            _tagRepositoryMock
                .Setup(r => r.GetByIdsAsync(TAGIDS_DUMMY, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tags);
            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            await Assert.ThrowsAsync<DbUpdateException>(() =>
            _problemService.CreateProblemAsync(
                TITLE_DUMMY,
                STATEMENTMD_DUMMY,
                DIFFICULTY_DUMMY,
                TIMELIMIT_DUMMY,
                MEMORYLIMIT_DUMMY,
                CREATEDBY_DUMMY,
                TAGIDS_DUMMY,
                CancellationToken.None
            ));
        }

        [Fact]
        public async Task ReadAllProblemsAsync_ShouldReturn_WhenValid()
        {
            var problems = new List<Problem>
            {
                new Problem {ProblemId = Guid.NewGuid(), Title = "two sum"},
                new Problem {ProblemId = Guid.NewGuid(), Title = "three sum"}
            };

            _problemRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(problems);

            var result = await _problemService.ReadAllProblemsAsync(It.IsAny<CancellationToken>());

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            _problemRepositoryMock
                .Verify(r => r.GetAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task FindProblemById_ShouldReturn_WhenValid()
        {
            var problem = new Problem
            {
                ProblemId = Guid.NewGuid(),
                Title = TITLE_DUMMY
            };

            _problemRepositoryMock
                .Setup(r => r.GetByIdAsync(problem.ProblemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(problem);

            var result = await _problemService.FindProblemByIdAsync(problem.ProblemId, It.IsAny<CancellationToken>());

            Assert.NotNull(result);
            Assert.Equal(problem.ProblemId, result.ProblemId);
            Assert.Equal(problem.Title, result.Title);

            _problemRepositoryMock
                .Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        }

        [Fact]
        public async Task FindProblemById_ShouldReturnNull_WhenNotFound()
        {
            var problem = new Problem
            {
                ProblemId = Guid.NewGuid()
            };

            _problemRepositoryMock
                .Setup(r => r.GetByIdAsync(problem.ProblemId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Problem?)null);

            var result = await _problemService.FindProblemByIdAsync(problem.ProblemId, It.IsAny<CancellationToken>());

            Assert.Null(result);

            _problemRepositoryMock
                .Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task FindProblemBySlugAsync_ShouldReturn_WhenValid()
        {
            var problem = new Problem
            {
                ProblemId = Guid.NewGuid(),
                Title = TITLE_DUMMY
            };
            problem.GenerateSlug();
            _problemRepositoryMock
                .Setup(r => r.GetBySlugAsync(problem.Slug, It.IsAny<CancellationToken>()))
                .ReturnsAsync(problem);

            var result = await _problemService.FindProblemBySlugAsync(problem.Slug, It.IsAny<CancellationToken>());

            Assert.NotNull(result);
            Assert.Equal(problem.Title, result.Title);
            Assert.Equal(problem.Slug, result.Slug);

            _problemRepositoryMock
                .Verify(r => r.GetBySlugAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task FindProblemBySlug_ShouldReturnNull_WhenNotFound()
        {
            var problem = new Problem
            {
                ProblemId = Guid.NewGuid(),
                Title = TITLE_DUMMY
            };
            problem.GenerateSlug();

            _problemRepositoryMock
                .Setup(r => r.GetBySlugAsync(problem.Slug, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Problem?)null);

            var result = await _problemService.FindProblemBySlugAsync(problem.Slug, It.IsAny<CancellationToken>());

            Assert.Null(result);
            _problemRepositoryMock
                .Verify(r => r.GetBySlugAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ReadAllProblemsBySlugAsync_ShouldReturn_WhenValid()
        {
            IEnumerable<Problem> problems = new List<Problem>
            {
                new Problem
                {
                    ProblemId = Guid.NewGuid(),
                    Title = "two sum",
                    Slug = "two-sum"
                },
                new Problem
                {
                    ProblemId = Guid.NewGuid(),
                    Title = "three sum",
                    Slug = "three-sum"
                }
            };

            _problemRepositoryMock
                .Setup(r => r.GetAllBySlugAsync("sum", It.IsAny<CancellationToken>()))
                .ReturnsAsync(problems);

            var result = await _problemService.ReadAllProblemsBySlugAsync("sum", It.IsAny<CancellationToken>());

            Assert.Equal(2, result.Count());

            _problemRepositoryMock
                .Verify(r => r.GetAllBySlugAsync("sum", It.IsAny<CancellationToken>()));
        }
    }
}
