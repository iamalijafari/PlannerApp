using Planner.Domain.Entities;

namespace Planner.UnitTests.Domain;

public class GoalTests
{
    [Fact]
    public void Constructor_NormalizesUnspecifiedDueDateToUtc()
    {
        DateTime dueDate = new(2027, 4, 15, 0, 0, 0, DateTimeKind.Unspecified);

        Goal goal = new("Relocate to Ireland", "Complete the relocation plan.", dueDate);

        Assert.Equal(DateTimeKind.Utc, goal.DueDate.Kind);
        Assert.Equal(dueDate, goal.DueDate);
    }

    [Fact]
    public void Update_WithBlankTitle_ThrowsAndPreservesExistingState()
    {
        DateTime originalDueDate = new(2027, 4, 15, 0, 0, 0, DateTimeKind.Utc);
        Goal goal = new("Original title", "Original description", originalDueDate);

        ArgumentException exception = Assert.Throws<ArgumentException>(
            () => goal.Update("   ", "Changed description", originalDueDate.AddDays(1), true));

        Assert.Equal("title", exception.ParamName);
        Assert.Equal("Original title", goal.Title);
        Assert.Equal("Original description", goal.Description);
        Assert.Equal(originalDueDate, goal.DueDate);
        Assert.False(goal.IsCompleted);
    }
}
