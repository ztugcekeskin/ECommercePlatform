using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services;
using System.Text.Json;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase 
{
     private readonly IChatMessageRepository _chatRepository; 
     public ChatController(
        IChatMessageRepository chatRepository) 
         {
         _chatRepository = chatRepository; } 
    
         [HttpGet] 
         public async Task<IActionResult> GetConversation
             ( 
                int userId, 
                int otherUserId, 
                int productId) 
             { var messages = await _chatRepository.GetConversationAsync
             (  userId, 
                otherUserId, 
                productId 
            ); 
            return Ok(messages); 
        } 
    
        [HttpGet("user/{userId}")] 
        public async Task<IActionResult> GetUserMessages(int userId)
         { var messages = await _chatRepository.GetMessagesForUserAsync(userId);
        return Ok(messages); 
    }
 }