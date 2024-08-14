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
        public AppBehaviorManager AppBehavior { get; set; }


        #endregion

        /// <summary>
        /// Triggert NACH dem das Initialisieren abgeschlossen ist sozusagen OnLoaded()
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            //AppBehavior.NavigationManager = NavigationManager;
            //AppBehavior.DialogService = DialogService;
            //AppBehavior.ApiService = ApiService;

            if (!AppBehavior.BenutzerVerwaltung.IsAuthenticated)
            {

                AppBehavior.NavigationManager.NavigateTo("/login", true);
            }

        }
    }
}
