using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebData.Objects.PageContext.Objs
{
    /// <summary>
    /// Definiert alle Werte die ein Nachrichten-Objekt benötigt
    /// </summary>
    public class NewsObject
    {
        /// <summary>
        /// Definiert den Index des Objekts
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Definiert den Titel der Nachricht
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Definiert den Nachrichten-Inhalt
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Definiert eine Sammlung an Nachrichten-Objekten
    /// </summary>
    public class NewsObjectList : List<NewsObject> { }
}
