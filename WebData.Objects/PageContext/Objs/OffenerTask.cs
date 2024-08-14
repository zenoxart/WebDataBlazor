using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebData.Objects.PageContext.Objs
{
    public class UserTasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskType Type { get; set; }

        public ICollection<ZugewieseneAufgabe> UserAssignedTasks { get; set; }

    }

    public class AufgabenListe : List<UserTasks> { }

    public enum TaskType
    {
        Created,
        Pending,
        Working,
        Waiting,
        Finished
    }

    public class ZugewieseneAufgabe
    {
        public int UserTaskId { get; set; }
        public UserTasks UserTask { get; set; }
        public int UserId { get; set; }
        public UserObject User { get; set; }
    }
}
