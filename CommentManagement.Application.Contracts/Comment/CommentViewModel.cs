namespace CommentManagement.Application.Contracts.Comment;

public class CommentViewModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string Message { get; set; }
    public long OwnerRecordId { set; get; }
    public string OwnerName { get; set; }
    public int Type { get; set; }
    public bool IsConfirmed { get; set; }
    public bool IsCancelled { get; set; }
    public string CommentDate { get; set; }
}