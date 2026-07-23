using Planner.Domain.Utilities;

namespace Planner.Domain.Entities;

public class DailyPlan
{
    public Guid Id { get; private set; }
    public Guid WeeklyPlanId { get; set; }
    public WeeklyPlan WeeklyPlan { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime DueDate { get; private set; }
    public bool IsCompleted { get; private set; }

    private DailyPlan() { }

    public DailyPlan(Guid weeklyPlanId, string title, string description, DateTime dueDate)
    {
        Id = Guid.NewGuid();
        WeeklyPlanId = weeklyPlanId;
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
