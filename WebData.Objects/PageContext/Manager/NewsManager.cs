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
            new NewsObject
            {
                Id = 1,
                Title = "New Update 1.0",
                Message = "News have been added to the WebData-App"
            },
            new NewsObject
            {
                Id=2,
                Title="New Update 1.1",
                Message = "User-Profiles have been added to the WebData-App"
            }
        };
    }
}
