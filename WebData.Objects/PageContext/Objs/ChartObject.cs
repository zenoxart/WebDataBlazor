using MudBlazor;

namespace WebData.Objects.PageContext.Objs
{
    public class ChartObject
    {
        public string Title { get; set; }   
        public List<ChartSeries> Series { get; set; } = new List<ChartSeries>();

        public required string[] XAxisLabels { get; set; }

        public ChartOptions Options { get; set; } = new ChartOptions() { InterpolationOption = InterpolationOption.NaturalSpline };
    }
}
