using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebData.Backend.MonadFunc;
using WebData.Objects.PageContext.CModel;
using WebData.Objects.PageContext.Monad;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly NewsMonadFuncs _newsMonadFuncs;

        public NewsController(ILogger<NewsController> logger, ApplicationDbContext context)
        {
            _newsMonadFuncs = new NewsMonadFuncs(context);
        }

        [HttpGet(Name = "GetAllNews")]
        public async Task<IActionResult> GetAllTasks(UserObject user)
        {
            return await _newsMonadFuncs.FindUser(user.Id)
                .Bind(foundUser => Task.FromResult(_newsMonadFuncs.AuthenticateUser(foundUser, user.Password, UserRoles.Moderator)))
                .Bind(_ => _newsMonadFuncs.GetAllNews())
                .OnFailure(error => BadRequest(error))
                .Map(result => Ok(result));
        }

        [HttpPost("CreateNews", Name = "CreateNews")]
        public async Task<IActionResult> CreateNews(UserNewsRequest userNews)
        {
            return await _newsMonadFuncs.FindUser(userNews.User.Id)
                .Bind(foundUser => Task.FromResult(_newsMonadFuncs.AuthenticateUser(foundUser, userNews.User.Password, UserRoles.Admin)))
                .Bind(_ => _newsMonadFuncs.AddNewsArticle(userNews.Artikle))
                .OnFailure(error => BadRequest(error))
                .Map(_ => Ok("Newsartikel erfolgreich hinzugefügt."));
        }

        [HttpPost("UpdateNews", Name = "UpdateNews")]
        public async Task<IActionResult> UpdateNews(UserNewsRequest userNews)
        {
            return await _newsMonadFuncs.FindUser(userNews.User.Id)
                .Bind(foundUser => Task.FromResult(_newsMonadFuncs.AuthenticateUser(foundUser, userNews.User.Password, UserRoles.Moderator)))
                .Bind(_ => _newsMonadFuncs.FindNewsArticle(userNews.Artikle.Id))
                .Bind(existingArticle => _newsMonadFuncs.UpdateNewsArticle(existingArticle, userNews.Artikle))
                .OnFailure(error => BadRequest(error))
                .Map(_ => Ok("Newsartikel erfolgreich aktualisiert."));
        }

        [HttpDelete("DeleteNews", Name = "DeleteNews")]
        public async Task<IActionResult> DeleteNews(UserNewsRequest userNews)
        {
            return await _newsMonadFuncs.FindUser(userNews.User.Id)
                .Bind(foundUser => Task.FromResult(_newsMonadFuncs.AuthenticateUser(foundUser, userNews.User.Password, UserRoles.Moderator)))
                .Bind(_ => _newsMonadFuncs.FindNewsArticle(userNews.Artikle.Id))
                .Bind(existingArticle => _newsMonadFuncs.DeleteNewsArticle(existingArticle))
                .OnFailure(error => BadRequest(error))
                .Map(_ => Ok("Newsartikel erfolgreich gelöscht."));
        }
    }



}
