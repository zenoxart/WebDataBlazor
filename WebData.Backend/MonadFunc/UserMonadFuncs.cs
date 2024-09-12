using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using WebData.Objects.PageContext.Monad;
using WebData.Objects.PageContext.Objs;

namespace WebData.Backend.MonadFunc
{
    /// <summary>
    /// Kapselt alle Monad (Wrapper) -Funktionen die beim Verwalten von Benutzern benötigt werden 
    /// </summary>
    internal class UserMonadFuncs : CommonMonadFuncs
    {
        public UserMonadFuncs(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Läd alle Benutzer
        /// </summary>
        public async Task<Result<List<UserObject>>> GetAllUsers()
        {
            var allUser = await _context.Users.ToListAsync();
            return Result<List<UserObject>>.Success(allUser);
        }

        /// <summary>
        /// Aktualisiert den bestehnden Benutzer mit dem neuen
        /// </summary>
        /// <remarks>
        /// !Aktualisiert nur Name & Email!
        /// </remarks>
        public async Task<Result<UserObject>> UpdateUser(UserObject existingUser, UserObject updatedUser)
            => await SaveChanges(
                () =>
                {
                    existingUser.Name = updatedUser.Name != existingUser.Name ? updatedUser.Name : existingUser.Name;
                    existingUser.Email = updatedUser.Email != existingUser.Email ? updatedUser.Email : existingUser.Email;

                    return null;
                }, existingUser
            );

        /// <summary>
        /// Aktualisiert die Benutzerrolle einen bestehenden Benutzers
        /// </summary>
        public async Task<Result<UserObject>> UpdateUserRole(UserObject existingUser, UserRoles? role)
            => await SaveChanges(
                () =>
                {

                    if (role == null)
                        return Task.FromResult<string?>("Keiner Userrolle angegeben");

                    existingUser.Role = role;

                    return Task.FromResult<string?>(null);
                }, existingUser
            );

        /// <summary>
        /// Löscht einen bestehenden Benutzer
        /// </summary>
        public async Task<Result<UserObject>> DeleteUser(UserObject User)
             => await SaveChanges(
                 () =>
                 {
                     if (_context.Users == null)
                     {
                         return Task.FromResult<string?>("Keiner Benutzer angegeben");
                     }
                     _context.Users.Remove(User);
                 
                     return Task.FromResult<string?>(null);
                 }, User

             );
        /// <summary>
        /// Legt einen neuen Benutzer an
        /// </summary>
        public async Task<Result<UserObject>> RegisterUser(UserObject newUser)
             => await SaveChanges(
                 () =>
                 {
                     var userEntity = new UserObject
                     {
                         Name = newUser.Name,
                         Email = newUser.Email,
                         Password = newUser.Password,
                         PasswordIV = newUser.PasswordIV,
                         Role = UserRoles.Default
                     };
                 
                     if (_context.Users.Where(x => x.Name == userEntity.Name && x.Email == userEntity.Email) != null)
                     {
                         return Task.FromResult<string?>("Benutzer existiert bereits");
                     }
                 
                     return Task.FromResult<string?>(null);
                 }, newUser
             );


        /// <summary>
        /// Sucht nach einem Benutzer nur über die Email-Adresse
        /// </summary>
        public async Task<Result<UserObject>> FindUserViaMail(string useremail)
             => await SaveChanges(
                 () =>
                 {
                     if (_context.Users == null)
                     {
                         return Task.FromResult<string?>("Diese Platform hat noch keine Benutzer.");
                     }
                 
                     UserObject? foundUser = _context.Users.FirstOrDefault(x => x.Email == useremail);
                 
                     if (foundUser == null)
                         return Task.FromResult<string?>($"Benutzer mit der Email {useremail} wurde nicht gefunden.");
                 
                     return Task.FromResult<string?>(null);
                 }, new UserObject()
             );
    }
}
