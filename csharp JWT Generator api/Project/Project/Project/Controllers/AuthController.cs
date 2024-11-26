using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Project.Auth;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ProjectContext _context;

        private readonly TokenGenerator _tokenGenerator;

        private readonly IWebHostEnvironment _env;

        public AuthController(ProjectContext context, TokenGenerator tokenGenerator, IWebHostEnvironment env)
        {
            _context = context;
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            _env = env;
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File not provided" });

            try
            {
                if (!file.ContentType.StartsWith("image/"))
                {
                    return BadRequest(new { message = "Only image files are allowed." });
                }

                var webRootPath = _env.ContentRootPath;
                if (string.IsNullOrEmpty(webRootPath))
                {
                    return StatusCode(500, new { message = "WebRootPath is not set." });
                }

                var uploadsDirectory = Path.Combine(webRootPath, "uploads");

                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsDirectory, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { message = "Image uploaded successfully", fileName = uniqueFileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error uploading image: {ex.Message}" });
            }
        }


        // GET: api/Auth
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // POST: api/Auth
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<ActionResult<User>> register(User user)
        {
            var existingUser = await _context.User.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (existingUser != null)
            {
                return Conflict(new { message = "A user with this email already exists." });
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.User.Add(user);

            await _context.SaveChangesAsync();

            var token = _tokenGenerator.GenerateToken(user.Email);

            return Ok(new {user,token});
        }


        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(User user)
        {
            var existingUser = await _context.User.FirstOrDefaultAsync(x => x.Email == user.Email);
        
            if (existingUser == null)
            {
                return Conflict(new { message = "Account doesn't exist" });
            }
           
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password);
        
            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "Invalid password" });
            }


            var token = _tokenGenerator.GenerateToken(existingUser.Email);

            return Ok(new { existingUser.Email, token });
        }

      
    }
}
