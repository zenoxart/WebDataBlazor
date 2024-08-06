using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebData.Objects.PageContext.Objs
{
    public class Aufgabe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskType Type { get; set; }
    }

    public class AufgabenListe : List<Aufgabe> { }

    public enum TaskType
    {
        Created,
        Pending,
        InProgress,
        Waiting,
        Finished

    }
}
