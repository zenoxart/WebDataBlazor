using MudBlazor;
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

        //Personal Activity
        public ChartObject PersonalActivities { get; set; } = new ChartObject()
        {
            Title = "Profile Activities",
            XAxisLabels = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep" },
            Series = new List<ChartSeries>()
            {
                new ChartSeries() { Name = "Series 1", Data = new double[] { 90, 79, 72, 69, 62, 62, 55, 65, 70 } },
                new ChartSeries() { Name = "Series 2", Data = new double[] { 35, 41, 35, 51, 49, 62, 69, 91, 148 } },
            }
        };

        public static bool IsAuthenticated { get; set; } = false;
    }
}
