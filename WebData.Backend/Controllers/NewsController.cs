using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebData.Backend.MonadFunc;
using WebData.Objects.PageContext.CModel;
using WebData.Objects.PageContext.Monad;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend.Controllers
{

    /// <summary>
    /// API-Controller zum Verwalten von Nachrichten
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly NewsMonadFuncs _newsMonadFuncs;

        /// <summary>
        /// Konstruktor für NewsController, initialisiert DbContext
        /// </summary>
        public NewsController(ILogger<NewsController> logger, ApplicationDbContext context)
        {
            _newsMonadFuncs = new NewsMonadFuncs(context);
        }

        /// <summary>
        /// Läd alle Nachrichten
        /// </summary>
        [HttpGet(Name = "GetAllNews")]
        public async Task<IActionResult> GetAllTasks(UserObject user)
        {
            return await _newsMonadFuncs.FindUser(user.Id)
                .Bind(foundUser => Task.FromResult(_newsMonadFuncs.AuthenticateUser(foundUser, user.Password, UserRoles.Moderator)))
                .Bind(_ => _newsMonadFuncs.GetAllNews())
                .OnFailure(error => BadRequest(error))
                .Map(result => Ok(result));
        }

        /// <summary>
        /// Erstellt eine neue Nachricht
        /// </summary>
        [HttpPost("CreateNews", Name = "CreateNews")]
        public async Task<IActionResult> CreateNews(UserNewsRequest userNews)
        {
            return await _newsMonadFuncs.FindUser(userNews.User.Id)
                .Bind(foundUser => Task.FromResult(_newsMonadFuncs.AuthenticateUser(foundUser, userNews.User.Password, UserRoles.Moderator)))
                .Bind(_ => _newsMonadFuncs.AddNewsArticle(userNews.Article))
                .OnFailure(error => BadRequest(error))
                .Map(_ => Ok("Newsartikel erfolgreich hinzugefügt."));
        }

        /// <summary>
        /// Aktualisiert eine bestehende Nachricht
        /// </summary>
        /// <param name="userNews"></param>
        /// <returns></returns>
        [HttpPost("UpdateNews", Name = "UpdateNews")]
        public async Task<IActionResult> UpdateNews(UserNewsRequest userNews)
        {
            return await _newsMonadFuncs.FindUser(userNews.User.Id)
                .Bind(foundUser => Task.FromResult(_newsMonadFuncs.AuthenticateUser(foundUser, userNews.User.Password, UserRoles.Moderator)))
                .Bind(_ => _newsMonadFuncs.FindNewsArticle(userNews.Article.Id))
                .Bind(existingArticle => _newsMonadFuncs.UpdateNewsArticle(existingArticle, userNews.Article))
                .OnFailure(error => BadRequest(error))
                .Map(_ => Ok("Newsartikel erfolgreich aktualisiert."));
        }

        /// <summary>
        /// Löscht eine bestehende Nachricht
        /// </summary>
        [HttpDelete("DeleteNews", Name = "DeleteNews")]
        public async Task<IActionResult> DeleteNews(UserNewsRequest userNews)
        {
            return await _newsMonadFuncs.FindUser(userNews.User.Id)
                .Bind(foundUser => Task.FromResult(_newsMonadFuncs.AuthenticateUser(foundUser, userNews.User.Password, UserRoles.Moderator)))
                .Bind(_ => _newsMonadFuncs.FindNewsArticle(userNews.Article.Id))
                .Bind(existingArticle => _newsMonadFuncs.DeleteNewsArticle(existingArticle))
                .OnFailure(error => BadRequest(error))
                .Map(_ => Ok("Newsartikel erfolgreich gelöscht."));
        }
    }
}
