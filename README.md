<div align="center">

<h1>MeetCode</h1>
An web-based coding challenge platform

</div>

## Description
MeetCode is a web-based platform where users can "meet" coding problems, submit solutions, track personal progress, and compete against other people. It supports multiple programming languages, real-time problem evaluation, personalized recommendations of problems for practices and ranking. The platform is designed for those who want to practice their DSA, System design skills, and those who want to demonstrate themselves by climbing to the top of the platform.

## Features:
> Let's just skip the lame backend Moderators/Admins CRUD and Analytics
+ Secured authenciation with JWT, and support multiple auth providers
+ Problem view with detailed informaton from difficulty, acceptance rate to tag and hints
+ Enables coding in various programming languages
+ Thoroughly designed with coverage for edge cases to ensure accurate evaluation
+ Tailored daily problems to match user skill levels and progress
+ Integrated AI Agent that help guide users to solution without spoiling the satisfaction of solving problem
+ Flexible setup options for profiles, including preferences and avatars
+ Various type of quizzes (multiple choices, drag and drop,...) to test users knowledge
+ Competitive rank climbing system

## Purposes:
> I just want to do it
+ Provides a hands-on environment for developers to practice DSA, test knowledges about system design, same with LeetCode
+ Leverages .NET ecosystem (ASP.NET Core) for robust APIs and React for dynamic UI—ideal for full-stack devs in enterprise settings
+ Online coding platforms (such as LeetCode and Codeforces) do not have ranking system like online games
+ Implements modern patterns (CQRS, MediatR, EF Core tracking handling) to demonstrate best practices

## Drawbacks:
+ Complexity in setup for non-IT, requires Docker for full deployment, initial configuration such as seeding data and EF Migrations might be time consuming.
+ This project may have many many security-related vulnerabilities that are not handled, you can use at your own risk
+ Not yet demonstrated in scalability, in theory, this project should have an acceptable scalability
+ Performance might also be an issue in some certain module
+ Seeding data might be small to some people as this project is made and test on a single computer, not a dedicated server
+ This project is a solo project

## Architecture
The system follow clean, layered architecture with CQRS separation for scalability and maintainability (though it might not neccessary as no one will use this but the author)


