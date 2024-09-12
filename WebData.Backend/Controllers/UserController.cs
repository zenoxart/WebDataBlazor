using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Objs;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebData.Objects.PageContext.Utilities;
using WebData.Objects.PageContext.CModel;
using WebData.Backend.MonadFunc;
using WebData.Objects.PageContext.Monad;

namespace WebData.Backend.Controllers
{
    /// <summary>
    /// API-Controller zum Verwalten von Benutzerinformationen
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly UserMonadFuncs _userMonadFuncs;

        /// <summary>
        /// Konstruktor für UserController, initialisiert Logger und DbContext
        /// </summary>
        public UserController(ILogger<UserController> logger, ApplicationDbContext context)
        {
            _userMonadFuncs = new UserMonadFuncs(context);
            _logger = logger;
        }

        /// <summary>
        /// Gibt alle Benutzer zurück
        /// </summary>
        [HttpGet(Name = "GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(UserObject admin)
            => await _userMonadFuncs.FindUser(admin.Id)
               .Bind(foundUser => Task.FromResult(_userMonadFuncs.AuthenticateUser(foundUser, admin.Password, UserRoles.Admin)))
               .Bind(_ => _userMonadFuncs.GetAllUsers())
               .OnFailure(error => BadRequest(error))
               .Map(result => Ok(result));

        /// <summary>
        /// Aktualisiert einen Benutzer oder erstellt einen neuen, wenn er nicht existiert
        /// </summary>
        [HttpPost("UpdateUser", Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserObject user)
            => await _userMonadFuncs.FindUser(user.Id)
               .Bind(foundUser => Task.FromResult(_userMonadFuncs.AuthenticateUser(foundUser, user.Password, UserRoles.Moderator)))
               .Bind(foundUser => _userMonadFuncs.UpdateUser(foundUser, user))
               .OnFailure(error => BadRequest(error))
               .Map(_ => Ok("Benutzer erfolgreich aktualisiert."));

        /// <summary>
        /// Löscht einen Benutzer, wenn er existiert
        /// </summary>
        [HttpDelete("DeleteUser", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(UserObject user)
            => await _userMonadFuncs.FindUser(user.Id)
               .Bind(foundUser => Task.FromResult(_userMonadFuncs.AuthenticateUser(foundUser, user.Password, UserRoles.Moderator)))
               .Bind(foundUser => _userMonadFuncs.DeleteUser(foundUser))
               .OnFailure(error => BadRequest(error))
               .Map(_ => Ok("Benutzer erfolgreich gelöscht."));

        /// <summary>
        /// Registriert einen neuen Benutzer, wenn die E-Mail nicht bereits existiert
        /// </summary>
        [HttpPost("RegisterUser", Name = "RegisterUser")]
        public async Task<bool> Register(UserObject user)
            => await _userMonadFuncs.DontFindUser(user.Id)
               .Bind(_ => _userMonadFuncs.RegisterUser(user))
               .OnFailure(error => BadRequest(error))
               .Map(_ => Ok("Benutzer erfolgreich registriert.")) != null;

        /// <summary>
        /// Führt den Benutzerlogin durch, wenn die E-Mail und das Passwort übereinstimmen
        /// </summary>
        [HttpPost("LoginUser", Name = "LoginUser")]
        public async Task<IActionResult> Login(UserObject user)
            => await _userMonadFuncs.FindUser(user.Id)
               .Bind(foundUser => Task.FromResult(_userMonadFuncs.AuthenticateUser(foundUser, user.Password, UserRoles.Default)))
               .OnFailure(error => BadRequest(error))
               .Map(_ => Ok("Benutzer erfolgreich angemeldet."));

        /// <summary>
        /// Gibt die Initialisierungsvektordaten eines Benutzers zurück, wenn die E-Mail übereinstimmt
        /// </summary>
        [HttpPost("GetUserIV", Name = "GetUserIV")]
        public async Task<IActionResult> GetUserIV(UserObject user)
            => await _userMonadFuncs.FindUserViaMail(user.Email)
               .OnFailure(error => BadRequest(error))
               .Map(_ => Ok("Benutzer erfolgreich angemeldet."));

        /// <summary>
        /// Aktualisiert die Benutzerrolle eines Benutzers
        /// </summary>
        /// <param name="users">Es muss ein Admin-Account & ein Normaler Benutzer angegeben werden</param>
        [HttpPost("UpdateUserRole", Name = "UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleRequest users)
        {
            var adminFound = (await _userMonadFuncs.FindUser(users.AdminUser.Id)
              .Bind(foundAdmin => Task.FromResult(_userMonadFuncs.AuthenticateUser(foundAdmin, users.AdminUser.Password, necessaryRole: UserRoles.Admin))));

            if (adminFound != null)
            {
                return await _userMonadFuncs.FindUser(users.ChangedUser.Id)
                   .Bind(foundUser => _userMonadFuncs.UpdateUserRole(foundUser, users.ChangedUser.Role))
                   .OnFailure(error => BadRequest(error))
                   .Map(_ => Ok("Benutzerrolle wurde erfolgreich aktualisiert."));
            }

            return new BadRequestObjectResult("Adminrechte konnten nicht verifiziert werden.");
        }
    }
}
