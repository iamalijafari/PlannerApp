using Planner.Domain.Utilities;

namespace Planner.Domain.Entities;

public class WeeklyGoal
{
    public Guid Id { get; private set; }
    public Guid MonthlyGoalId { get; set; }
    public MonthlyGoal MonthlyGoal { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime DueDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public ICollection<DailyGoal> DailyGoals { get; private set; } = new List<DailyGoal>();

    private WeeklyGoal() { }

    public WeeklyGoal(Guid monthlyGoalId, string title, string description, DateTime dueDate)
    {
        Id = Guid.NewGuid();
        MonthlyGoalId = monthlyGoalId;
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        DueDate = UtcDateTime.Normalize(dueDate);
        IsCompleted = false;
    }

    public void MarkAsCompleted() => IsCompleted = true;

    public void Update(string title, string description, DateTime dueDate, bool isCompleted)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));

        Title = title;
        Description = description;
        DueDate = UtcDateTime.Normalize(dueDate);
        IsCompleted = isCompleted;
    }
}
