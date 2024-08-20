using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserTasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserTasksController> _logger;

        // Konstruktor für AufgabenController, initialisiert Logger und DbContext
        public UserTasksController(ILogger<UserTasksController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Gibt alle Aufgaben zurück
        [HttpGet(Name = "GetAllTasks")]
        public async Task<List<UserTasks>> GetAllTasks(UserObject user)
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

            return await _context.UserTasks.ToListAsync();
        }

        // Gibt alle Aufgaben für einen bestimmten Benutzer zurück
        [HttpPost("GetTasksForUser", Name = "GetAllTasksForUser")]
        public async Task<List<UserTasks>> GetAllTasksForUser(UserObject user)
        {
            UserObject? foundUser = await _context.Users.FindAsync(user.Id);
            if (foundUser == null)
            {
                BadRequest($"Benutzer mit der ID {user.Id} wurde nicht gefunden.");
                return null;
            }
            if (foundUser.Password != user.Password)
            {
                BadRequest("Benutzerauthentifizierung fehlgeschlagen.");
                return null;
            }

            return await _context.ZugewieseneAufgaben
               .Where(ua => ua.UserId == user.Id)
               .Select(ua => ua.UserTask)
               .ToListAsync();


        }

        // Weist eine Aufgabe einem Benutzer zu
        [HttpPost("AssignTaskToUser", Name = "SetTaskForUser")]
        public async Task<IActionResult> SetTaskForUser(int userid, int taskid)
        {
            // Überprüfen, ob der Benutzer existiert
            UserObject? foundUser = await _context.Users.FindAsync(userid);
            if (foundUser == null)
            {
                BadRequest($"Benutzer mit der ID {userid} wurde nicht gefunden.");
                return null;
            }

            // Überprüfen, ob die Aufgabe existiert
            UserTasks? task = await _context.UserTasks.FindAsync(taskid);
            if (task == null)
            {
                return NotFound($"Aufgabe mit der ID {taskid} wurde nicht gefunden.");
            }

            // Überprüfen, ob die Beziehung bereits existiert
            bool userTaskExists = await _context.ZugewieseneAufgaben
                .AnyAsync(ua => ua.UserId == userid && ua.UserTaskId == taskid);

            if (userTaskExists)
            {
                return BadRequest("Diese Aufgabe ist bereits dem Benutzer zugewiesen.");
            }

            // Beziehung zwischen Benutzer und Aufgabe erstellen
            ZugewieseneAufgabe userAufgabe = new ZugewieseneAufgabe
            {
                UserId = userid,
                UserTaskId = taskid
            };

            _context.ZugewieseneAufgaben.Add(userAufgabe);
            await _context.SaveChangesAsync();

            return Ok("Aufgabe erfolgreich dem Benutzer zugewiesen.");
        }

        // Aktualisiert die Zuordnung einer Aufgabe zu einem Benutzer
        [HttpPost("UpdateUserTask", Name = "UpdateTaskToUser")]
        public async Task<IActionResult> UpdateTaskToUser(UserObject user, int taskid)
        {
            // Überprüfen, ob der Benutzer existiert
            UserObject? foundUser = await _context.Users.FindAsync(user);
            if (foundUser == null)
            {
                return NotFound($"Benutzer mit der ID {user} wurde nicht gefunden.");
            }
            if (foundUser.Password != user.Password)
            {
                return BadRequest("Benutzerauthentifizierung fehlgeschlagen.");

            }

            // Überprüfen, ob die Aufgabe existiert
            UserTasks? task = await _context.UserTasks.FindAsync(taskid);
            if (task == null)
            {
                return NotFound($"Aufgabe mit der ID {taskid} wurde nicht gefunden.");
            }

            // Überprüfen, ob die Aufgabe dem Benutzer bereits zugewiesen ist
            bool userTaskExists = await _context.ZugewieseneAufgaben
                .AnyAsync(ua => ua.UserId == user.Id && ua.UserTaskId == taskid);

            if (userTaskExists)
            {
                return BadRequest("Diese Aufgabe ist bereits dem Benutzer zugewiesen.");
            }

            // Alle bestehenden Zuordnungen des Benutzers löschen
            var existingAssignments = _context.ZugewieseneAufgaben.Where(ua => ua.UserId == user.Id);
            _context.ZugewieseneAufgaben.RemoveRange(existingAssignments);

            // Neue Aufgabe dem Benutzer zuweisen
            var userAufgabe = new ZugewieseneAufgabe
            {
                UserId = user.Id,
                UserTaskId = taskid
            };

            _context.ZugewieseneAufgaben.Add(userAufgabe);
            await _context.SaveChangesAsync();

            return Ok("Benutzeraufgabe erfolgreich aktualisiert.");
        }
    }
}
