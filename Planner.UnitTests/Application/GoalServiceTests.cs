using Microsoft.Extensions.Logging;
using NSubstitute;
using Planner.Application.DTOs.Goal;
using Planner.Application.Enumerations;
using Planner.Application.Interfaces.Repositories;
using Planner.Domain.Entities;

namespace Planner.UnitTests.Application;

public class GoalServiceTests
{
    private readonly IGoalRepository goalRepository = Substitute.For<IGoalRepository>();
    private readonly ILogger<GoalService> logger = Substitute.For<ILogger<GoalService>>();

    [Fact]
    public async Task CreateAsync_WithBlankTitle_ReturnsValidationErrorWithoutPersisting()
    {
        GoalService service = new(goalRepository, logger);
        CreateGoalDto request = new("   ", "Description", DateTime.UtcNow.AddDays(30));

        var result = await service.CreateAsync(request);

        Assert.False(result.Success);
        Assert.Equal(MessageKey.Invalid_Input, result.MessageKey);
        await goalRepository.DidNotReceive().AddAsync(Arg.Any<Goal>());
        await goalRepository.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_PersistsAndReturnsMappedGoal()
    {
        GoalService service = new(goalRepository, logger);
        DateTime dueDate = new(2027, 6, 1, 0, 0, 0, DateTimeKind.Unspecified);
        CreateGoalDto request = new("Earn cloud certification", "Prepare and sit the exam.", dueDate);

        var result = await service.CreateAsync(request);

        Assert.True(result.Success);
        Assert.NotNull(result.Result);
        Assert.Equal(request.Title, result.Result.Title);
        Assert.Equal(DateTimeKind.Utc, result.Result.DueDate.Kind);
        await goalRepository.Received(1).AddAsync(
            Arg.Is<Goal>(goal =>
                goal != null &&
                goal.Title == request.Title &&
                goal.DueDate.Kind == DateTimeKind.Utc));
        await goalRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task CompleteAsync_WhenGoalExists_MarksItCompleteAndSaves()
    {
        Goal goal = new("Publish portfolio", "Prepare a public project.", DateTime.UtcNow.AddDays(14));
        goalRepository.GetByIdAsync(goal.Id).Returns(goal);
        GoalService service = new(goalRepository, logger);

        var result = await service.CompleteAsync(goal.Id);

        Assert.True(result.Success);
        Assert.True(result.Result);
        Assert.True(goal.IsCompleted);
        await goalRepository.Received(1).UpdateAsync(goal);
        await goalRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryFails_ReturnsServerError()
    {
        goalRepository.GetAllAsync().Returns(
            Task.FromException<IEnumerable<Goal>>(new InvalidOperationException("Database unavailable")));
        GoalService service = new(goalRepository, logger);

        var result = await service.GetAllAsync();

        Assert.False(result.Success);
        Assert.Equal(MessageKey.ServerError, result.MessageKey);
    }
}
