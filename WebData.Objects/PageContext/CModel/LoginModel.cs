using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebData.Objects.PageContext.CModel
{
    /// <summary>
    /// Definiert das API-Model für die Login-Daten
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Definiert das Email-Feld
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        /// <summary>
        /// Definiert das Passwort
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
