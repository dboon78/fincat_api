using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto ToCommentDto(this Comment comment){
            //Console.WriteLine($"CommentDto for {comment.Id} - {comment.Title}");
            return new CommentDto{
                Id=comment.Id,
                Title=comment.Title,
                Content=comment.Content,
                CreatedOn=comment.CreatedOn,
                StockId=comment.StockId,
                CreatedBy=comment.AppUser.UserName

            };
        }
        public static Comment ToCommentFromCreateDTO(this CreateCommentRequestDto requestDto, int stockId){
            return new Comment(){
                StockId=stockId,
                Title=requestDto.Title,
                Content=requestDto.Content
            };   
        }
        public static Comment ToCommentFromCreateDTO(this UpdateCommentRequestDto requestDto, int stockId){
            return new Comment(){
                Title=requestDto.Title,
                Content=requestDto.Content
            };   
        }
    }
}