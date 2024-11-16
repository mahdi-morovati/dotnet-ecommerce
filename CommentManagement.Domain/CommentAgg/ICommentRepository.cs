using _0_framework.Domain;

namespace CommentManagement.Domain.CommentAgg;

public interface ICommentRepository : IRepository<long, Comment>
{
}