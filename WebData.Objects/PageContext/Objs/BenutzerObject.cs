using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebData.Objects.PageContext.Objs
{
    public class BenutzerObject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }

        public string PasswordIV {  get; set; }


    }

    public class BenuterListe : List<BenutzerObject> { }
}
