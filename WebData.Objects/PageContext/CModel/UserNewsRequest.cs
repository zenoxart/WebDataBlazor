using WebData.Objects.PageContext.Objs;

namespace WebData.Objects.PageContext.CModel
{
    /// <summary>
    /// Definiert das API-Model eines Nachrichten-Artikels und dessen Benutzer
    /// </summary>
    public class UserNewsRequest
    {
        /// <summary>
        /// Definiert den Benutzer
        /// </summary>
        public UserObject User { get; set; }

        /// <summary>
        /// Definiert die Nachricht
        /// </summary>
        public NewsObject Article { get; set; }
    }
}
