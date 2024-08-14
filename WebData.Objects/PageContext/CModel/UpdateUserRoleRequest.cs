using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Objs;

namespace WebData.Objects.PageContext.CModel
{
    public class UpdateUserRoleRequest
    {
        public UserObject ChangedUser { get; set; }
        public UserObject AdminUser { get; set; }
    }
}
