using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Monad;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend.MonadFunc
{
    public class NewsMonadFuncs
    {
        private readonly ApplicationDbContext _context;

        public NewsMonadFuncs(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<UserObject>> FindUser(int userId)
        {
            if (_context.Users != null)
            {
                UserObject foundUser = await _context.Users.FindAsync(userId);

                if (foundUser != null)
                    return Result<UserObject>.Success(foundUser);
            }

            return Result<UserObject>.Failure($"Benutzer mit der ID {userId} wurde nicht gefunden.");

        }

        public Result<UserObject> AuthenticateUser(UserObject foundUser, string? password, UserRoles necessaryRole)
        {
            if (!(string.IsNullOrEmpty(password) && string.IsNullOrWhiteSpace(password)) && foundUser.Password != password && foundUser.Role != necessaryRole)
                return Result<UserObject>.Failure("Benutzerauthentifizierung fehlgeschlagen.");

            return Result<UserObject>.Success(foundUser);
        }

        public async Task<Result<List<NewsObject>>> GetAllNews()
        {
            var newsList = await _context.News.ToListAsync();
            return Result<List<NewsObject>>.Success(newsList);
        }

        public async Task<Result<NewsObject>> AddNewsArticle(NewsObject article)
        {
            await _context.News.AddAsync(article);
            await _context.SaveChangesAsync();
            return Result<NewsObject>.Success(article);
        }

        public async Task<Result<NewsObject>> FindNewsArticle(int articleId)
        {
            var article = await _context.News.FindAsync(articleId);
            if (article == null)
            {
                return Result<NewsObject>.Failure("News-Artikel wurde nicht gefunden.");
            }

            return Result<NewsObject>.Success(article);
        }

        public async Task<Result<NewsObject>> UpdateNewsArticle(NewsObject existingArticle, NewsObject updatedArticle)
        {
            _context.Entry(existingArticle).CurrentValues.SetValues(updatedArticle);
            await _context.SaveChangesAsync();
            return Result<NewsObject>.Success(existingArticle);
        }

        public async Task<Result<NewsObject>> DeleteNewsArticle(NewsObject article)
        {
            _context.News.Remove(article);
            await _context.SaveChangesAsync();
            return Result<NewsObject>.Success(article);
        }
    }

}
