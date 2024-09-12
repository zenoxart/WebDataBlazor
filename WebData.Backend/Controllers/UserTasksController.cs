using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebData.Backend.MonadFunc;
using WebData.Objects.PageContext.Objs;
using WebData.Objects.PageContext.Monad;

namespace WebData.Backend.Controllers
{
    /// <summary>
    /// API-Controller zum Verwalten von Benutzeraufgaben
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserTasksController : ControllerBase
    {
        private readonly UserTaskMonadFuncs _userTasksMonadFuncs;

        private readonly ILogger<UserTasksController> _logger;

        /// <summary>
        /// Konstruktor für AufgabenController, initialisiert Logger und DbContext
        /// </summary>
        public UserTasksController(ILogger<UserTasksController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _userTasksMonadFuncs = new UserTaskMonadFuncs(context);
        }

        /// <summary>
        /// Gibt alle Aufgaben zurück
        /// </summary>
        [HttpGet(Name = "GetAllTasks")]
        public async Task<IActionResult> GetAllTasks(UserObject user)
        {
            return await _userTasksMonadFuncs.FindUser(user.Id)
               .Bind(foundUser => Task.FromResult(_userTasksMonadFuncs.AuthenticateUser(foundUser, user.Password, UserRoles.Moderator)))
               .Bind(_ => _userTasksMonadFuncs.GetAllUserTasks())
               .OnFailure(error => BadRequest(error))
               .Map(result => Ok(result));
        }

        /// <summary>
        /// Gibt alle Aufgaben für einen bestimmten Benutzer zurück
        /// </summary>
        [HttpPost("GetTasksForUser", Name = "GetAllTasksForUser")]
        public async Task<IActionResult> GetAllTasksForUser(UserObject user)
        {
            return await _userTasksMonadFuncs.FindUser(user.Id)
               .Bind(foundUser => Task.FromResult(_userTasksMonadFuncs.AuthenticateUser(foundUser, user.Password, UserRoles.Moderator)))
               .Bind(_ => _userTasksMonadFuncs.GetTasksForUser(user.Id))
               .OnFailure(error => BadRequest(error))
               .Map(result => Ok(result));
        }

        /// <summary>
        /// Weist eine Aufgabe einem Benutzer zu
        /// </summary>
        [HttpPost("AssignTaskToUser", Name = "SetTaskForUser")]
        public async Task<IActionResult> SetTaskForUser(UserObject user, int taskid)
        {
            return await _userTasksMonadFuncs.FindUser(user.Id)
               .Bind(foundUser => Task.FromResult(_userTasksMonadFuncs.AuthenticateUser(foundUser, user.Password, UserRoles.Moderator)))
               .Bind(_ => _userTasksMonadFuncs.FindUserTasksById(taskid))
               .Bind(_ => _userTasksMonadFuncs.UserTaskExistCheckById(user, taskid))
               .Bind(_ => _userTasksMonadFuncs.CreateUserTask(user.Id, taskid))
               .OnFailure(error => BadRequest(error))
               .Map(result => Ok(result));
        }

        /// <summary>
        /// Aktualisiert die Zuordnung einer Aufgabe zu einem Benutzer
        /// </summary>
        [HttpPost("UpdateUserTask", Name = "UpdateTaskToUser")]
        public async Task<IActionResult> UpdateTaskToUser(UserObject user, int taskid)
        {
            return await _userTasksMonadFuncs.FindUser(user.Id)
               .Bind(foundUser => Task.FromResult(_userTasksMonadFuncs.AuthenticateUser(foundUser, user.Password, UserRoles.Moderator)))
               .Bind(_ => _userTasksMonadFuncs.FindUserTasksById(taskid))
               .Bind(_ => _userTasksMonadFuncs.UserTaskExistCheckById(user, taskid))
               .Bind(_ => _userTasksMonadFuncs.RemoveConnectedTasksFromUser(user))
               .Bind(_ => _userTasksMonadFuncs.CreateUserTask(user.Id, taskid))
               .OnFailure(error => BadRequest(error))
               .Map(result => Ok(result));
        }
    }
}
