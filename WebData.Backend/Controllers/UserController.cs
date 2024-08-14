using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Objs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebData.Objects.PageContext.Utilities;
using WebData.Objects.PageContext.CModel;

namespace WebData.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        // Konstruktor für UserController, initialisiert Logger und DbContext
        public UserController(ILogger<UserController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Gibt alle Benutzer zurück
        [HttpGet(Name = "GetAllUsers")]
        public async Task<List<UserObject>> GetAllUsers( UserObject admin)
        {
            UserObject? foundUser = await _context.Users.FindAsync(admin.Id);

            if (foundUser == null)
            {
                NotFound($"Benutzer mit der ID {admin.Id} wurde nicht gefunden.");

                return null;
            }
            if (foundUser.Password != admin.Password)
            {
                BadRequest("Benutzerauthentifizierung fehlgeschlagen.");
                return null;

            }
            return await _context.Users.ToListAsync();
        }

        // Aktualisiert einen Benutzer oder erstellt einen neuen, wenn er nicht existiert
        [HttpPost("UpdateUser", Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser( UserObject user)
        {
            // Benutzer anhand der ID suchen
            UserObject? foundUser = await _context.Users.FindAsync(user.Id);

            if (foundUser != null)
            {
                // Felder aktualisieren, wenn sie sich geändert haben
                foundUser.Name = user.Name != foundUser.Name ? user.Name : foundUser.Name;
                foundUser.Email = user.Email != foundUser.Email ? user.Email : foundUser.Email;

                _context.Users.Update(foundUser);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            else
            {
                // Benutzer hinzufügen, wenn er nicht existiert
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
        }

        // Löscht einen Benutzer, wenn er existiert
        [HttpDelete("DeleteUser", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser( UserObject user)
        {
            // Benutzer anhand der ID suchen
            UserObject? foundUser = await _context.Users.FindAsync(user.Id);

            if (foundUser.Password != user.Password)
            {
                return BadRequest("Benutzerauthentifizierung fehlgeschlagen.");
                

            }
            if (foundUser != null)
            {
                // Benutzer löschen
                _context.Users.Remove(foundUser);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }

        // Registriert einen neuen Benutzer, wenn die E-Mail nicht bereits existiert
        [HttpPost("RegisterUser", Name = "RegisterUser")]
        public async Task<bool> Register( UserObject user)
        {
            if (_context.Users != null)
            {
                // Überprüfen, ob die E-Mail bereits existiert
                UserObject? foundUser = _context.Users.FirstOrDefault(x => x.Email == user.Email);
                if (foundUser == null)
                {
                    // Neuen Benutzer erstellen und speichern
                    var userEntity = new UserObject
                    {
                        Name = user.Name,
                        Email = user.Email,
                        Password = user.Password,
                        PasswordIV = user.PasswordIV,
                        Role = UserRoles.Default
                    };
                    await _context.Users.AddAsync(userEntity);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        // Führt den Benutzerlogin durch, wenn die E-Mail und das Passwort übereinstimmen
        [HttpPost("LoginUser", Name = "LoginUser")]
        public async Task<UserObject> Login(UserObject user)
        {
            // Benutzer anhand der E-Mail suchen
            UserObject? foundUser = _context.Users.FirstOrDefault(x => x.Email == user.Email);

            if (foundUser != null)
            {
                // Passwort überprüfen
                if (user.Password == foundUser.Password)
                {
                    // Benutzerinformationen zurückgeben, wenn das Passwort stimmt
                    return new UserObject()
                    {
                        Id = foundUser.Id,
                        Name = foundUser.Name,
                        Email = foundUser.Email,
                        Role = foundUser.Role
                    };
                }
            }

            // Null zurückgeben, wenn der Benutzer nicht gefunden wurde oder das Passwort falsch ist
            return null;
        }

        // Gibt die Initialisierungsvektordaten eines Benutzers zurück, wenn die E-Mail übereinstimmt
        [HttpPost("GetUserIV", Name = "GetUserIV")]
        public async Task<UserObject> GetUserIV(UserObject user)
        {
            // Benutzer anhand der E-Mail suchen
            UserObject? foundUser = _context.Users.FirstOrDefault(x => x.Email == user.Email);

            if (foundUser != null)
            {
                // Benutzerinformationen einschließlich des IV zurückgeben
                var newUser = new UserObject()
                {
                    Email = foundUser.Email,
                    PasswordIV = foundUser.PasswordIV,
                    Name = foundUser.Name,
                    Id = foundUser.Id
                };
                // Hier könnte ein JWT oder ein Session-Token generiert und zurückgegeben werden
                return newUser;
            }

            // Null zurückgeben, wenn der Benutzer nicht gefunden wurde
            return null;
        }
        

        [HttpPost("UpdateUserRole", Name = "UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleRequest users)
        {
            // Überprüfen, ob der Admin-Benutzer existiert
            UserObject? foundAdminUser = await _context.Users.FindAsync(users.AdminUser.Id);
            if (foundAdminUser == null)
            {
                return NotFound("Admin-Benutzer nicht gefunden.");
            }

            // Überprüfen, ob der Admin-Benutzer die notwendigen Berechtigungen hat
            if (foundAdminUser.Role != UserRoles.Admin && foundAdminUser.Password != users.AdminUser.Password)
            {
                return Forbid("Nur Administratoren können Benutzerrollen aktualisieren.");
            }

            // Überprüfen, ob der zu ändernde Benutzer existiert
            UserObject? foundChangedUser = await _context.Users.FindAsync(users.ChangedUser.Id);
            if (foundChangedUser == null)
            {
                return NotFound("Zu ändernder Benutzer nicht gefunden.");
            }

            // Aktualisieren der Benutzerrolle
            foundChangedUser.Role = users.ChangedUser.Role;

            // Änderungen speichern
            _context.Users.Update(foundChangedUser);
            await _context.SaveChangesAsync();

            return Ok("Benutzerrolle erfolgreich aktualisiert.");


        }

    }
}
