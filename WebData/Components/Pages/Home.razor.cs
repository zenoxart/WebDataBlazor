using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.NetworkInformation;
using WebData.Objects.PageContext.Manager;
using WebData.Objects.PageContext.Service;
using Microsoft.AspNetCore.Components;

namespace WebData.Components.Pages
{
    public partial class Home
    {


        #region Injecte die Servies
        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        public ApiService ApiService { get; set; }

        [Inject]
        IDialogService DialogService { get; set; }

        #endregion
        /// <summary>
        /// Stellt das Verhalten für den Einlauf zur Verfügung
        /// </summary>
        public AppManager AppBehavior { get; set; } = new AppManager();
        /// <summary>
        /// Triggert NACH dem das Initialisieren abgeschlossen ist sozusagen OnLoaded()
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (!BenutzerManager.IsAuthenticated)
            {

                NavigationManager.NavigateTo("/login", true);
            }

            // Referenziere die Services zu den Managern
            AppBehavior.InitServices(ApiService, DialogService);

            // Läd die Daten
            //await AppBehavior.Timeline.LoadData();


        }
    }
}
