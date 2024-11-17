using _0_framework.Domain;
using CommentManagement.Application.Contracts.Comment;

namespace CommentManagement.Domain.CommentAgg;

public interface ICommentRepository : IRepository<long, Comment>
{
    List<CommentViewModel> Search(CommentSearchModel searchModel);
}