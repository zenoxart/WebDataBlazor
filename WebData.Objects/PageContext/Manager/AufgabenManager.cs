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
                Name = "Loading",
                Description = "Loading...",
                Type = TaskType.Pending
            }
        };

        public async Task LoadData()
        {
            AufgabenVerzeichnis = await APIService.GetAsync<AufgabenListe>("PersonalBacklog");
        }

    }
}
