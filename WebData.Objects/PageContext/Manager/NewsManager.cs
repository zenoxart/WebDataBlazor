using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Objs;

namespace WebData.Objects.PageContext.Manager
{
    /// <summary>
    /// Definiert die clientseite Verwaltung der Nachrichten
    /// </summary>
    public class NewsManager : BaseManager
    {
        /// <summary>
        /// Definiert die Nachrichtenliste die auf dem Client geladen wurden
        /// </summary>
        public NewsObjectList NewsList { get; set; } = new NewsObjectList()
        {
        };
    }
}
