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
                Description = "Init a new Server",
                Type = TaskType.Created
            }
        };

    }
}
