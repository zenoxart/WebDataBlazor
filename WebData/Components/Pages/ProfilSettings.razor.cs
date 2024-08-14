using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebData.Objects.PageContext.Manager;
using WebData.Objects.PageContext.Service;

namespace WebData.Components.Pages
{
    public partial class ProfilSettings
    {
        #region Injecte die Servies
        [Inject]
        public AppBehaviorManager AppBehavior { get; set; }


        #endregion

        /// <summary>
        /// Triggert NACH dem das Initialisieren abgeschlossen ist sozusagen OnLoaded()
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Läd die Daten
            await AppBehavior.Aufgaben.LoadData();


        }
    }
}
