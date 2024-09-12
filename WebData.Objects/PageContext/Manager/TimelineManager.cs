using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Objs;

namespace WebData.Objects.PageContext.Manager
{

    /// <summary>
    /// Definiert die clientseite Verwaltung der Zeitlinien-Diagramme
    /// </summary>
    public class TimelineManager : BaseManager
    {
        /// <summary>
        /// Definiert die aktuelle Sammlung an Zeitlinien-Objekten
        /// </summary>
        public TimelineList CurrentTimeline { get; set; } = new TimelineList()
        {
            new TimelineObject
            {
                Id = 1,
                Message ="Hello",
                Time = DateTime.Now,
                Color = MudBlazor.Color.Success,
                Type = MudBlazor.Severity.Success
            }
        };
    }
}
