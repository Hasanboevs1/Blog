using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostService.Data;
using PostService.DTOs;
using PostService.Entities;

namespace PostService.Controllers;

[ApiController]
[Route("api/posts")]
public class PosController : ControllerBase
{
    private readonly PostDbContext _context;
    public PosController(PostDbContext context) => _context = context;

    [HttpGet]
    public async Task<IEnumerable<Post>> Get() => await _context.Posts.ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Post(PostDto post)
    {
        var existingPost = await _context.Posts.FirstOrDefaultAsync(p => p.Title.ToLower() == post.Title.ToLower());
        if (existingPost != null)
        {
            return BadRequest("Post already exists");
        }

        var newPost = new Post
        {
            Title = post.Title,
            Content = post.Content
        };
        newPost.AuthorId += 1;
        newPost.CreatedAt = DateTime.UtcNow;
        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();
        return Ok(newPost);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            return NotFound();
        }
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return Ok("Deleted Successfully");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, PostDto post)
    {
        var existingPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (existingPost == null)
        {
            return NotFound();
        }
        existingPost.Title = post.Title;
        existingPost.Content = post.Content;
        existingPost.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok("Updated Successfully");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }
}
