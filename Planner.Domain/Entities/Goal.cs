using Planner.Domain.Utilities;

namespace Planner.Domain.Entities;

public class Goal
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime DueDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public ICollection<YearlyPlan> YearlyPlans { get; private set; } = new List<YearlyPlan>();

    private Goal() { }

    public Goal(string Title, string Description, DateTime DueDate)
    {
        Id = Guid.NewGuid();
        this.Title = Title;
        this.Description = Description;
        CreatedAt = DateTime.UtcNow;
        this.DueDate = UtcDateTime.Normalize(DueDate);
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
