namespace Planner.Domain.Entities;

public class Goal
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime DueDate { get; private set; }
    public bool IsCompleted { get; private set; }

    private Goal() { }

    public Goal(string Title, string Description, DateTime DueDate)
    {
        Id = Guid.NewGuid();
        this.Title = Title;
        this.Description = Description;
        CreatedAt = DateTime.UtcNow;
        this.DueDate = DueDate.ToUniversalTime();
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
        DueDate = dueDate;
        IsCompleted = isCompleted;
    }
}