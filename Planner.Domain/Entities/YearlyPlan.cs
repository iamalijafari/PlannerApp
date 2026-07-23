using Planner.Domain.Utilities;

namespace Planner.Domain.Entities;

public class YearlyPlan
{
    public Guid Id { get; private set; }
    public Guid GoalId { get; set; }
    public Goal Goal { get; private set; } = null!;
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime DueDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public ICollection<MonthlyPlan> MonthlyPlans { get; private set; } = new List<MonthlyPlan>();

    private YearlyPlan() { }

    public YearlyPlan(Guid goalId, string title, string description, DateTime dueDate)
    {
        Id = Guid.NewGuid();
        GoalId = goalId;
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        DueDate = UtcDateTime.Normalize(dueDate);
        IsCompleted = false;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }

    public void Update(string title, string description, DateTime dueDate, bool isCompleted)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        }

        Title = title;
        Description = description;
        DueDate = UtcDateTime.Normalize(dueDate);
        IsCompleted = isCompleted;
    }
}
