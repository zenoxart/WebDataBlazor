using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Objs;

namespace WebData.Objects.PageContext.CModel
{
    /// <summary>
    /// Definiert das API-Model für das aktualisieren einer Benutzerrolle
    /// </summary>
    public class UpdateUserRoleRequest
    {
        /// <summary>
        /// Definiert den neuen Status des Benutzers (inkl. neuer Benutzerrolle)
        /// </summary>
        public UserObject ChangedUser { get; set; }

        /// <summary>
        /// Definiert den benötigten Admin-User um die Benutzerrolle ändern zu können
        /// </summary>
        public UserObject AdminUser { get; set; }
    }
}
