using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController ]
    public class CommentController:ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;
        public CommentController(ICommentRepository commentRepository, 
                                IStockRepository stockRepository,
                                UserManager<AppUser> userManager,
                                IFMPService fmpService)
        {
            _commentRepository= commentRepository;
            _stockRepository= stockRepository;
            _userManager= userManager;
            _fmpService= fmpService;
        }
/// <summary>
/// User has submitted a new comment
/// </summary>
/// <param name="symbol">The stock/crypto ticker to comment on</param>
/// <param name="commentDto">subject and body</param>
/// <returns>CommentDto</returns>
         [HttpPost]
        [Route("{symbol:alpha}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute]string symbol, [FromBody] CreateCommentRequestDto commentDto){
            // Console.WriteLine($"Commment create 1");
            // if(!await _stockRepository.StockExists(stockId))return BadRequest("Stock does not exist");
            if(!ModelState.IsValid)return BadRequest(ModelState);
            var stock=await _stockRepository.GetBySymbolAsync(symbol,commentDto.IsCrypto);
            if(stock==null){
                return BadRequest("Stock does not exist");
            }
            var commentModel=commentDto.ToCommentFromCreateDTO(stock.Id);            
            var username=User.GetUsername();
            var appUser=await _userManager.FindByNameAsync(username);
            commentModel.AppUserId=appUser.Id;
            await _commentRepository.CreateComment(commentModel);
            // Console.WriteLine($"Commment create 2");
            // CreatedAtActionResult result=
            return CreatedAtAction(nameof(GetById),new {id=commentModel.Id},commentModel.ToCommentDto());
        }
/// <summary>
/// Retrieve all comments for a given symbol
/// </summary>
/// <param name="queryObject">symbol and isDescending</param>
/// <returns>CommentDto</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery]CommentQueryObject queryObject)
        {
            if(!ModelState.IsValid)return BadRequest(ModelState);
            var comments = await _commentRepository.GetAllCommentsAsync(queryObject);
            var commentDto=comments.Select(x=>x.ToCommentDto());
            return Ok(commentDto);
        }

/// <summary>
/// Update a specific comment 
/// </summary>
/// <param name="id">Id of the comment to be edited</param>
/// <param name="updateDto">new comment data from form</param>
/// <returns>CommentDto</returns>
        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto){
            
            if(!ModelState.IsValid)return BadRequest(ModelState);
            var commentModel=await _commentRepository.UpdateComment(id,updateDto.ToCommentFromCreateDTO(id));
            if(commentModel==null){
                return NotFound();
            }
            return Ok(commentModel.ToCommentDto());
        }
        /// <summary>
        /// Get a specific comment by id
        /// </summary>
        /// <param name="id">id of specific comment</param>
        /// <returns>CommentDto</returns>
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id){
            
            if(!ModelState.IsValid)return BadRequest(ModelState);
            var comment=await _commentRepository.GetCommentByIdAsync(id);
            if(comment==null){
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }
       
/// <summary>
/// Delete a specific comment by Id
/// </summary>
/// <param name="id">id of comment</param>
/// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id){
            
            if(!ModelState.IsValid)return BadRequest(ModelState);
            var commentModel=await _commentRepository.DeleteCommentByIdAsync(id);
            if(commentModel==null){
                return NotFound();
            }
            return NoContent();
        }
    }

    
}