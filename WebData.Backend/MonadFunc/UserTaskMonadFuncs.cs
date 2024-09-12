using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Monad;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend.MonadFunc
{

    /// <summary>
    /// Kapselt alle Monad (Wrapper) -Funktionen die beim Verwalten von benutzerbezogenen Aufgaben benötigt werden 
    /// </summary>
    internal class UserTaskMonadFuncs : CommonMonadFuncs
    {
        public UserTaskMonadFuncs(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Läd alle Benutzeraufgaben
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<UserTasks>>> GetAllUserTasks()
        {
            var allTasksList = await _context.UserTasks.ToListAsync();
            return Result<List<UserTasks>>.Success(allTasksList);
        }

        /// <summary>
        /// Läd alle Benutzeraufgaben die einem bestimmten Benutzer zugewießen wurden
        /// </summary>
        public async Task<Result<List<UserTasks>>> GetTasksForUser(int userId)
        {
            var userList = await _context.ZugewieseneAufgaben
             .Where(ua => ua.UserId == userId)
             .Select(ua => ua.UserTask)
             .ToListAsync();

            return Result<List<UserTasks>>.Success(userList);
        }

        /// <summary>
        /// Sucht nach einer bestehenden Benutzeraufgabe aufgrund der ID
        /// </summary>
        public async Task<Result<UserTasks>> FindUserTasksById(int taskid)
        {
            var task = await _context.UserTasks.FindAsync(taskid);
            return task == null
                ? Result<UserTasks>.Failure($"Aufgabe mit der ID {taskid} wurde nicht gefunden.")
                : Result<UserTasks>.Success(task);
        }

        /// <summary>
        /// Überprüft ob der Benutzer eine Aufgabe mit der angegeben ID besitzt
        /// </summary>
        public async Task<Result<UserTasks>> UserTaskExistCheckById(UserObject user, int taskid)
        {
            bool exists = await _context.ZugewieseneAufgaben
                  .AnyAsync(ua => ua.UserId == user.Id && ua.UserTaskId == taskid);
            if (exists)
            {
                Result<UserTasks>.Failure("Diese Aufgabe ist bereits dem Benutzer zugewiesen.");
            }

            return Result<UserTasks>.Success(FindUserTasksById(taskid).Result.Value);

        }

        /// <summary>
        /// Erstellt eine neue Benutzeraufgabe
        /// </summary>
        public async Task<Result<ZugewieseneAufgabe>> CreateUserTask(int userid,int taskid)
        {
            ZugewieseneAufgabe userAufgabe = new ZugewieseneAufgabe
            {
                UserId = userid,
                UserTaskId = taskid
            };

            _context.ZugewieseneAufgaben.Add(userAufgabe);
            await _context.SaveChangesAsync();
            return Result<ZugewieseneAufgabe>.Success(userAufgabe);
        }

        /// <summary>
        /// Löscht alle Zuweisungen an Aufgaben von einem Benutzer
        /// </summary>
        public Task<Result<List<ZugewieseneAufgabe>>> RemoveConnectedTasksFromUser(UserObject user)
        {
            // Alle bestehenden Zuordnungen des Benutzers löschen
            var existingAssignments = _context.ZugewieseneAufgaben.Where(ua => ua.UserId == user.Id);
            _context.ZugewieseneAufgaben.RemoveRange(existingAssignments);

            return Task.FromResult(Result<List<ZugewieseneAufgabe>>.Success(existingAssignments.ToList()));
        }

    }
}
