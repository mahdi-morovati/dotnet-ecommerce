using _0_framework.Application;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Domain.CommentAgg;

namespace CommentManagement.Application;

public class CommentApplication : ICommentApplication
{
    private readonly ICommentRepository _commentRepository;

    public CommentApplication(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public OperationResult Add(AddComment command)
    {
        var OperationResult = new OperationResult();
        var comment = new Comment(command.Name, command.Email, command.Website, command.Message,
            command.OwnerRecordId, command.Type, command.ParentId);

        _commentRepository.Create(comment);
        _commentRepository.SaveChanges();
        return OperationResult.Succedded();
    }

    public OperationResult Confirm(long id)
    {
        var OperationResult = new OperationResult();
        var comment = _commentRepository.Get(id);
        if (comment == null)
            return OperationResult.Failed(ApplicationMessages.RecordNotFound);

        comment.Confirm();
        _commentRepository.SaveChanges();
        return OperationResult.Succedded();
    }

    public OperationResult Cancel(long id)
    {
        var OperationResult = new OperationResult();
        var comment = _commentRepository.Get(id);
        if (comment == null)
            return OperationResult.Failed(ApplicationMessages.RecordNotFound);

        comment.Cancel();
        _commentRepository.SaveChanges();
        return OperationResult.Succedded();
    }

    public List<CommentViewModel> Search(CommentSearchModel searchModel)
    {
        return _commentRepository.Search(searchModel);
    }
}