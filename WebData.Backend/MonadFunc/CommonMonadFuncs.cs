using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using WebData.Objects.PageContext.Monad;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend.MonadFunc
{
    /// <summary>
    /// Basis-Monad-Funktionen
    /// </summary>
    /// <param name="context">Gibt den Entity-Framework-Datenbank-Kontext an</param>
    /// <remarks>
    /// Hier handelt es sich um Monad-Functionen die anwendungsweit benötigt werden. 
    /// Diese Klasse umfasst Wrapper-Funktionen die sehr oft in Serie erfolgreich sein müssen.
    /// </remarks>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    internal class CommonMonadFuncs(ApplicationDbContext context)
    {
        public readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Sucht nach einem Benutzer aufgrund der BenutzerID und liefert ihn als Monad zurück
        /// </summary>
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

        /// <summary>
        /// Sucht nach einem Benutzer aufgrund der BenutzerID. 
        /// Wenn Kein Benutzer gefunden worden ist, wird NULL zurückgegeben.
        /// Hier soll ein 
        /// </summary>
        public async Task<Result<UserObject>> DontFindUser(int userId)
        {
            if (_context.Users != null)
            {
                UserObject foundUser = await _context.Users.FindAsync(userId);

                if (foundUser == null)
                    return Result<UserObject>.Success(null);
            }

            return Result<UserObject>.Failure($"Benutzer mit der ID {userId} wurde gefunden.");

        }

        /// <summary>
        /// Verifiziert das ein Benutzer mit dem angegeben Passwort und der dazu gehörigen Benutzerrolle übereinstimmt
        /// </summary>
        public Result<UserObject> AuthenticateUser(UserObject foundUser, string? password, UserRoles necessaryRole)
        {
            if (!(string.IsNullOrEmpty(password) && string.IsNullOrWhiteSpace(password))
                && foundUser.Password != password
                && foundUser.Role != necessaryRole)

                return Result<UserObject>.Failure("Benutzerauthentifizierung fehlgeschlagen.");

            return Result<UserObject>.Success(foundUser);
        }

        /// <summary>
        /// Debugging-Vereinfachungs
        /// </summary>
        private string? GetDebuggerDisplay() => ToString();

        public async Task<Result<T>> SaveChanges<T>(Func<Task<string?>> func, T resultObject)
        {
            try
            { // Execute the update logic and return the result of the action
                var result = await func();

                if (!string.IsNullOrEmpty(result)) // If there's an error message, return failure
                    return Result<T>.Failure(result);

                await _context.SaveChangesAsync(); // Saves changes to the database
                return Result<T>.Success(resultObject); // Returns success with the result object

            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"{ex.Message} : {ex.InnerException} => Stacktrace: {ex.StackTrace}");
            }

        }
    }
}
