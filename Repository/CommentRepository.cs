using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateComment(Comment comment)
        {
            // var existing=await _context.Stocks.FirstOrDefaultAsync(x => x.Id == comment.Id);
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
        
        public async Task<Comment?> DeleteCommentByIdAsync(int id)
        {            
            var commentModel=await _context.Comments.FirstOrDefaultAsync(x=>x.Id==id);
            if(commentModel==null)
            {
                return null;
            }
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllCommentsAsync(CommentQueryObject queryObject)
        {
            var comments=_context.Comments.Include(c=>c.AppUser).AsQueryable();
            if(!string.IsNullOrWhiteSpace(queryObject.Symbol)){
                comments=comments.Where(x=>x.Stock.Symbol==queryObject.Symbol);
            }
            if(queryObject.IsDescending){
                comments=comments.OrderByDescending(c=>c.CreatedOn);
            }else{
                comments=comments.OrderBy(c=>c.CreatedOn);
            }
            return await comments.ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.Include(c=>c.AppUser).FirstOrDefaultAsync(c => c.Id == id);
            
        }

        public async Task<Comment?> UpdateComment(int id, Comment commentModel)
        {            
            var existing=await _context.Comments.FirstOrDefaultAsync(x=>x.Id == id);
            if(existing==null)
            {return null;}
            existing.Title=commentModel.Title;
            existing.Content=commentModel.Content;
            await _context.SaveChangesAsync();
            return existing;
        }

    }
}