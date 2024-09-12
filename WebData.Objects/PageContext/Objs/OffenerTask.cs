using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebData.Objects.PageContext.Objs
{
    /// <summary>
    /// Definiert alle Werte die eine Benutzer-Aufgabe benötigt
    /// </summary>
    public class UserTasks
    {
        /// <summary>
        /// Definiert den Index des Objekts 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Definiert den Namen / Titel der Aufgabe 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Definiert den Inhalt der Aufgabe
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Definiert den Status der Aufgabe
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Definiert eine Sammlung an Aufgaben die einem Benutzer zugewiesen sind
        /// </summary>
        /// <remarks>
        /// Muss für die API eine ICollection sein
        /// </remarks>
        public ICollection<ZugewieseneAufgabe> UserAssignedTasks { get; set; }

    }

    /// <summary>
    /// Definiert eine Sammlung an Aufgaben
    /// </summary>
    public class AufgabenListe : List<UserTasks> { }

    /// <summary>
    /// Definiert die Status-Möglichkeiten von Aufgaben
    /// </summary>
    public enum TaskStatus
    {
        Created,
        Pending,
        Working,
        Waiting,
        Finished
    }

    /// <summary>
    /// Definiert alle Werte die eine Zugewiesene Aufgabe benötigt
    /// </summary>
    public class ZugewieseneAufgabe
    {
        /// <summary>
        /// Definiert den Aufgaben-Index
        /// </summary>
        public int UserTaskId { get; set; }

        /// <summary>
        /// Definiert die Aufgabe
        /// </summary>
        public UserTasks UserTask { get; set; }
        /// <summary>
        /// Definiert den Benutzer-Index
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Definiert den Benutzer
        /// </summary>
        public UserObject User { get; set; }
    }
}
