using Microsoft.EntityFrameworkCore;
using WebData.Objects.PageContext.Monad;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend.MonadFunc
{
    /// <summary>
    /// Kapselt alle Monad (Wrapper) -Funktionen die beim Verwalten von Nachrichten benötigt werden 
    /// </summary>
    internal class NewsMonadFuncs : CommonMonadFuncs
    {
        public NewsMonadFuncs(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Läd alle Nachrichten
        /// </summary>
        public async Task<Result<List<NewsObject>>> GetAllNews()
        {
            var newsList = await _context.News.ToListAsync();
            return Result<List<NewsObject>>.Success(newsList);
        }

        /// <summary>
        /// Erstellt einen neuen Nachrichten-Artikel
        /// </summary>
        public async Task<Result<NewsObject>> AddNewsArticle(NewsObject article)
        {
            await _context.News.AddAsync(article);
            await _context.SaveChangesAsync();
            return Result<NewsObject>.Success(article);
        }

        /// <summary>
        /// Sucht nach einem bestehenden Nachrichten-Artikel
        /// </summary>
        public async Task<Result<NewsObject>> FindNewsArticle(int articleId)
        {
            var article = await _context.News.FindAsync(articleId);
            return article == null 
                ? Result<NewsObject>.Failure("News-Artikel wurde nicht gefunden.") 
                : Result<NewsObject>.Success(article);
        }

        /// <summary>
        /// Aktualisiert einen bestehenden Nachrichten-Artikel
        /// </summary>
        public async Task<Result<NewsObject>> UpdateNewsArticle(NewsObject existingArticle, NewsObject updatedArticle)
        {
            _context.Entry(existingArticle).CurrentValues.SetValues(updatedArticle);
            await _context.SaveChangesAsync();
            return Result<NewsObject>.Success(existingArticle);
        }

        /// <summary>
        /// Löscht einen bestehenden Nachrichten-Artikel
        /// </summary>
        public async Task<Result<NewsObject>> DeleteNewsArticle(NewsObject article)
        {
            _context.News.Remove(article);
            await _context.SaveChangesAsync();
            return Result<NewsObject>.Success(article);
        }
    }

}
