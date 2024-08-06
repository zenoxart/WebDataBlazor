using MudBlazor;

namespace WebData.Objects.PageContext.Objs
{
    /// <summary>
    /// Defines Every Property a Object need to pop up in a Timeline
    /// </summary>
    public class TimelineObject
    {
        public int Id { get; set; }

        public required string Message { get; set; }

        public required Severity Type { get; set; }

        public required DateTime Time { get; set; }

        public required Color Color { get; set; }
    }

    public class TimelineList : List<TimelineObject> { }
}
