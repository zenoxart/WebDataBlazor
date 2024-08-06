using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebData.Objects.PageContext.Objs
{
    public class NewsObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class NewsObjectList : List<NewsObject> { }
}
