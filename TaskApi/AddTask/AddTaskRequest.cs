namespace TaskApi.AddTask
{
    public record AddTaskRequest(string Title, string? Description, DateOnly? DueDate);
}
