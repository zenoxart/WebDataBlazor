using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebData.Objects.PageContext.Objs
{
    /// <summary>
    /// Definiert alle Werte die ein Benutzer benötigt
    /// </summary>
    public class UserObject
    {
        /// <summary>
        /// Definiert den Index des Objekts
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Definiert den Namen des Benutzers
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Definiert die Email-Adresse des Benutzers
        /// </summary>
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// Definiert das Passwort des Benutzers
        /// </summary>
        [PasswordPropertyText]
        public string? Password { get; set; }

        /// <summary>
        /// Definiert die Salt-IV-Werte für das Passwort des Benutzers
        /// </summary>
        public string? PasswordIV {  get; set; }

        /// <summary>
        /// Definiert die Rolle des Benutzers
        /// </summary>
        public UserRoles? Role { get; set; }

        /// <summary>
        /// Definiert die Liste der zugewiesenen Aufgaben des Benutzers
        /// </summary>
        public ICollection<ZugewieseneAufgabe>? UserAssignedTasks { get; set; }


    }

    /// <summary>
    /// Deklariert die unterschiedlichen Arten von Benutzerrollen
    /// </summary>
    public enum UserRoles
    {
        Default,
        Developer,
        Moderator,
        Admin
    }

    /// <summary>
    /// Definiert eine Sammlung an Benutzer-Objekten
    /// </summary>
    public class UserList : List<UserObject> { }
}
