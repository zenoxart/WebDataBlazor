using System.ComponentModel.DataAnnotations;

namespace WebData.Objects.PageContext.CModel
{
    /// <summary>
    /// Definiert das API-Model für das neuerstellen eines Benutzers
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Definiert den Namen des Benutzers
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        /// <summary>
        /// Definiert die Email des Benutzers
        /// </summary>

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        /// <summary>
        /// Definiert das Passwort des Benutzers
        /// </summary>

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        /// <summary>
        /// Definiert das Bestätigungpasswort des Benutzers
        /// </summary>

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
