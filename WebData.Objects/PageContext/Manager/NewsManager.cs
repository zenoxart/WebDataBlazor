using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Objs;

namespace WebData.Objects.PageContext.Manager
{
    public class NewsManager : BaseManager
    {
        public NewsObjectList NewsList { get; set; } = new NewsObjectList()
        {
        };
    }
}
