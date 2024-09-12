using MudBlazor;

namespace WebData.Objects.PageContext.Objs
{
    /// <summary>
    /// Definiert alle Werte die ein Zeitlinien-Diagram benötigt
    /// </summary>
    public class TimelineChart
    {
        /// <summary>
        /// Definiert den Titel des Timeline-Charts
        /// </summary>
        public string Title { get; set; }   

        /// <summary>
        /// Definiert eine Sammlung an Chart-Serien
        /// </summary>
        public List<ChartSeries> Series { get; set; } = new List<ChartSeries>();

        /// <summary>
        /// Definiert die benennung der X-Achse
        /// </summary>
        public required string[] XAxisLabels { get; set; }

        /// <summary>
        /// Definiert die Einstellungen des Timeline-Charts
        /// </summary>
        public ChartOptions Options { get; set; } = new ChartOptions() { InterpolationOption = InterpolationOption.NaturalSpline };
    }


    /// <summary>
    /// Definiert alle Werte die ein Zeitlinien-Diagram-Objekt benötigt
    /// </summary>
    public class TimelineObject
    {
        /// <summary>
        /// Definiert den Index des Objekts
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Definiert den Inhalt
        /// </summary>
        public required string Message { get; set; }

        /// <summary>
        /// Definiert die Art/Typ 
        /// </summary>
        public required Severity Type { get; set; } 

        /// <summary>
        /// Definiert das Datum 
        /// </summary>
        public required DateTime Time { get; set; }

        /// <summary>
        /// Definiert die Farbe 
        /// </summary>
        public required Color Color { get; set; }
    }

    /// <summary>
    /// Definiert eine Sammlung an Zeitlinien-Diagram-Objekten
    /// </summary>
    public class TimelineList : List<TimelineObject> { }
}
