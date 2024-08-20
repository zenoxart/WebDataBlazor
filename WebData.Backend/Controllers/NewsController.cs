using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NewsController> _logger;

        // Konstruktor für AufgabenController, initialisiert Logger und DbContext
        public NewsController(ILogger<NewsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Gibt alle News zurück
        [HttpGet(Name = "GetAllNews")]
        public async Task<List<NewsObject>> GetAllTasks(UserObject user)
        {
            UserObject? foundUser = await _context.Users.FindAsync(user.Id);
            if (foundUser == null)
            {
                BadRequest($"Benutzer mit der ID {user.Id} wurde nicht gefunden.");
                return null;
            }
            if (foundUser.Password != user.Password && foundUser.Role != UserRoles.Moderator)
            {
                BadRequest("Benutzerauthentifizierung fehlgeschlagen.");
                return null;
            }

            return await _context.News.ToListAsync();
        }

        // Legt einen neuen News-Artikel an
        [HttpPost("CreateNews", Name = "CreateNews")]
        public async Task<IActionResult> CreateNews(UserObject user, NewsObject artikle)
        {
            UserObject? foundUser = await _context.Users.FindAsync(user.Id);
            if (foundUser == null)
            {
                return BadRequest($"Benutzer mit der ID {user.Id} wurde nicht gefunden.");
            }
            if (foundUser.Password != user.Password && foundUser.Role != UserRoles.Admin)
            {
                return BadRequest("Benutzerauthentifizierung fehlgeschlagen.");
            }

            await _context.News.AddAsync(artikle);
            await _context.SaveChangesAsync();

            return Ok("Newsartikel erfolgreich hinzugefügt.");
        }

        // Updated einen bestehenden News-Artikel
        [HttpPost("UpdateNews", Name = "UpdateNews")]
        public async Task<IActionResult> UpdateNews(UserObject user, NewsObject artikle)
        {
            UserObject? foundUser = await _context.Users.FindAsync(user.Id);
            if (foundUser == null)
            {
                return BadRequest($"Benutzer mit der ID {user.Id} wurde nicht gefunden.");
            }
            if (foundUser.Password != user.Password && (foundUser.Role != UserRoles.Admin || foundUser.Role != UserRoles.Moderator))
            {
                return BadRequest("Benutzerauthentifizierung fehlgeschlagen.");
            }
            if (_context.News.Find(artikle) == null)
            {
                return BadRequest("News-Artikel wurde nicht gefunden.");
            }

            _context.News.Update(artikle);
            await _context.SaveChangesAsync();

            return Ok("Newsartikel erfolgreich aktuallisiert.");
        }

        // Löscht einen bestehenden News-Artikel
        [HttpDelete("DeleteNews", Name = "DeleteNews")]
        public async Task<IActionResult> DeleteNews(UserObject user, NewsObject artikle)
        {
            UserObject? foundUser = await _context.Users.FindAsync(user.Id);
            if (foundUser == null)
            {
                return BadRequest($"Benutzer mit der ID {user.Id} wurde nicht gefunden.");
            }
            if (foundUser.Password != user.Password && (foundUser.Role != UserRoles.Admin || foundUser.Role != UserRoles.Moderator))
            {
                return BadRequest("Benutzerauthentifizierung fehlgeschlagen.");
            }
            if (_context.News.Find(artikle) == null)
            {
                return BadRequest("News-Artikel wurde nicht gefunden.");
            }

            _context.News.Remove(artikle);
            await _context.SaveChangesAsync();

            return Ok("Newsartikel erfolgreich aktuallisiert.");
        }

    }
}
