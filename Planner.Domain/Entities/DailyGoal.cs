namespace Planner.Domain.Entities;

public class DailyGoal
{
    public Guid Id { get; private set; }
    public Guid WeeklyGoalId { get; set; }
    public WeeklyGoal WeeklyGoal { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime DueDate { get; private set; }
    public bool IsCompleted { get; private set; }

    private DailyGoal() { }

    public DailyGoal(Guid weeklyGoalId, string title, string description, DateTime dueDate)
    {
        Id = Guid.NewGuid();
        WeeklyGoalId = weeklyGoalId;
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        DueDate = dueDate.ToUniversalTime();
        IsCompleted = false;
    }

    public void MarkAsCompleted() => IsCompleted = true;

    public void Update(string title, string description, DateTime dueDate, bool isCompleted)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        Title = title;
        Description = description;
        DueDate = dueDate;
        IsCompleted = isCompleted;
    }
}