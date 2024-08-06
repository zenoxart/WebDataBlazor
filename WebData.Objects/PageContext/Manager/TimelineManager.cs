using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Objs;

namespace WebData.Objects.PageContext.Manager
{
    public class TimelineManager : BaseManager
    {

        public TimelineList CurrentTimeline { get; set; } = new TimelineList()
        {
            new TimelineObject
            {
                Id = 1,
                Message ="Hello",
                Time = DateTime.Now,
                Color = MudBlazor.Color.Success,
                Type = MudBlazor.Severity.Success
            }
        };
    }
}
