using CommentService.Data;
using CommentService.DTOs;
using CommentService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Controllers;
[ApiController]
[Route("api/comments")]
public class CommentController : ControllerBase
{
    private readonly CommentDbContext _context;
    public CommentController(CommentDbContext context) => _context = context;

    [HttpGet]
    public async Task<IEnumerable<Comment>> Get() => await _context.Comments.ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CommentDto comment)
    {
        var existingComment = await _context.Comments
        .FirstOrDefaultAsync(x => x.Text.ToLower() == comment.Text.ToLower());

        if (existingComment != null)
        {
            return BadRequest("Comment already exists");
        }

        var newComment = new Comment
        {
            Text = comment.Text
        };

        await _context.Comments.AddAsync(newComment);
        await _context.SaveChangesAsync();
        return Ok("Comment added");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var comment = await _context.Comments
        .FirstOrDefaultAsync(x => x.Id == id);
        if (comment == null)
        {
            return NotFound();
        }
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return Ok("Comment deleted");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, [FromBody] CommentDto comment)
    {
        var existingComment = await _context.Comments
        .FirstOrDefaultAsync(x => x.Id == id);
        if (existingComment == null)
        {
            return NotFound();
        }
        existingComment.Text = comment.Text;
        await _context.SaveChangesAsync();
        return Ok("Comment updated");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var comment = await _context.Comments
        .FirstOrDefaultAsync(x => x.Id == id);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment);
    }
}
