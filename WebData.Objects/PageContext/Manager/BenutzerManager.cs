using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Objs;

namespace WebData.Objects.PageContext.Manager
{
    public class BenutzerManager : BaseManager
    {
        public BenutzerObject CurrentUser { get; set; } = new BenutzerObject()
        {
            Id = 1,
            Name = "admin",
            Email = "admin@gmail.com"
        };

        public BenuterListe AllUsers { get; set; }
    }
}
