using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Objs;

namespace WebData.Objects.PageContext.Manager
{
    /// <summary>
    /// Verwaltet clientseitig alle Aufgaben
    /// </summary>
    public class AufgabenManager : BaseManager
    {
        /// <summary>
        /// Definiert alle Aufgaben die am Client geladen sind
        /// </summary>
        public AufgabenListe AufgabenVerzeichnis { get; set; } = new AufgabenListe()
        {
            new UserTasks
            {
                Id = 1,
                Name = "Loading",
                Description = "Loading...",
                Status = Objs.TaskStatus.Pending
            }
        };

        /// <summary>
        /// Läd alle benötigten Daten von der REST-API
        /// </summary>
        public async Task LoadData()
        {
            AufgabenVerzeichnis = await this.ApiService.PostAsync<AufgabenListe>("UserTasks/GetTasksForUser", AppBehavior.BenutzerVerwaltung.CurrentUser);
        }

    }
}
