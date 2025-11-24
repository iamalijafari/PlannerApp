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
        CreatedAt = DateTime.Now;
        this.DueDate = DueDate;
        IsCompleted = false;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }
}