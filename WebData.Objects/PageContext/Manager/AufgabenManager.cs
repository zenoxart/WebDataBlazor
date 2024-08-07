using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Objs;

namespace WebData.Objects.PageContext.Manager
{
    public class AufgabenManager : BaseManager
    {
        public AufgabenListe AufgabenVerzeichnis { get; set; } = new AufgabenListe()
        {
            new Aufgabe
            {
                Id = 1,
                Name = "Server init",
                Description = "Create a new Server",
                Type = TaskType.Working
            },
            new Aufgabe
            {
                Id = 2,
                Name = "Server config",
                Description = "Update the config of a Server",
                Type = TaskType.Pending
            },
            new Aufgabe
            {
                Id = 3,
                Name = "Server delete",
                Description = "Delete a Server",
                Type = TaskType.Created
            },
            new Aufgabe
            {
                Id = 3,
                Name = "Server paused",
                Description = "Pause a Server",
                Type = TaskType.Finished
            }
        };

    }
}
