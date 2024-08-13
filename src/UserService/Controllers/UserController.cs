using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTOs;
using UserService.Entities;

namespace UserService.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserDbContext _context;
    public UserController(UserDbContext context) => _context = context;

    [HttpGet]
    public IEnumerable<User> Get() => _context.Users;

    [HttpPost]
    public async Task<IActionResult> Post(UserDto user)
    {
        var existinUser = await _context.Users.
        FirstOrDefaultAsync(x => x.Email.ToLower() == user.Email.ToLower());

        if (existinUser != null)
        {
            return BadRequest("User already exists");
        }

        var newUser = new User
        {
           FirstName = user.FirstName,
           LastName = user.LastName,
           Email = user.Email,
           Password = user.Password
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return Ok(newUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(long id, UserDto user)
    {
        var existinUser = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == id);

        if (existinUser == null)
        {
            return NotFound();
        }

        existinUser.FirstName = user.FirstName;
        existinUser.LastName = user.LastName;
        existinUser.Email = user.Email;
        existinUser.Password = user.Password;

        await _context.SaveChangesAsync();
        return Ok(existinUser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var existinUser = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == id);

        if (existinUser == null)
        {
            return NotFound();
        }

        _context.Users.Remove(existinUser);
        await _context.SaveChangesAsync();
        return Ok(existinUser);
    }
}
