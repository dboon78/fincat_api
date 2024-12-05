using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Extensions;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllCommentsAsync(CommentQueryObject queryObject);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<Comment> CreateComment(Comment comment);
        Task<Comment> UpdateComment(int id, Comment commentModel);
        Task<Comment?> DeleteCommentByIdAsync(int id);
    }
}