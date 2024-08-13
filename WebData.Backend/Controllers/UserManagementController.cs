using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Objs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebData.Objects.PageContext.Utilities;

namespace WebData.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserManagementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(ILogger<UserManagementController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: /PersonManagement
        [HttpGet(Name = "GetAllUsers")]
        public async Task<List<BenutzerObject>> GetAsync()
        {
            return await _context.User.ToListAsync();
        }

        // POST: /PersonManagement/UpdateUser
        [HttpPost("UpdateUser", Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] BenutzerObject benutzer)
        {
            var existingUser = await _context.User.FirstOrDefaultAsync(x => x.Id == benutzer.Id);

            if (existingUser != null)
            {
                existingUser.Name = benutzer.Name != existingUser.Name ? benutzer.Name : existingUser.Name;
                existingUser.Email = benutzer.Email != existingUser.Email ? benutzer.Email : existingUser.Email;

                _context.User.Update(existingUser);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            else
            {
                await _context.User.AddAsync(benutzer);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
        }

        // DELETE: /PersonManagement/DeleteUser
        [HttpDelete("DeleteUser", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] BenutzerObject benutzer)
        {
            var existingUser = await _context.User.FirstOrDefaultAsync(x => x.Id == benutzer.Id);

            if (existingUser != null)
            {
                _context.User.Remove(existingUser);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }

        // POST: /PersonManagement/RegisterUser
        [HttpPost("RegisterUser", Name = "RegisterUser")]
        public async Task<bool> Register([FromBody] BenutzerObject model)
        {
            if (_context.User != null)
            {
                BenutzerObject? existingUser = _context.User.FirstOrDefault(x => x.Email == model.Email);
                if (existingUser == null)
                {
                    var userEntity = new BenutzerObject
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        PasswordIV = model.PasswordIV
                    };
                    await _context.User.AddAsync(userEntity);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        // POST: /PersonManagement/LoginUser
        [HttpPost("LoginUser", Name = "LoginUser")]
        public async Task<BenutzerObject> Login([FromBody] BenutzerObject model)
        {
            BenutzerObject? user = _context.User.FirstOrDefault(x => x.Email == model.Email);

            if (user != null)
            {

                if (model.Password == user.Password)
                {
                    // Generate and return JWT or session token
                    return new BenutzerObject() { Id = user.Id, Name = user.Name, Email = user.Email };
                }
            }

            return null;
        }

        [HttpPost("GetUserIV", Name = "GetUserIV")]
        public async Task<BenutzerObject> GetUserIV([FromBody] BenutzerObject model)
        {
            BenutzerObject? user = _context.User.FirstOrDefault(x => x.Email == model.Email);

            if (user != null)
            {
                var newUser = new BenutzerObject()
                {
                    Email = user.Email,
                    PasswordIV = user.PasswordIV,
                    Name = user.Name,
                    Id = user.Id
                };
                // Generate and return JWT or session token
                return newUser;
            }

            return null;
        }

    }
}
