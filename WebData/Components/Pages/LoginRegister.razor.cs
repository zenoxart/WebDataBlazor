using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Xml.Linq;
using WebData.Objects.PageContext.CModel;
using WebData.Objects.PageContext.Manager;
using WebData.Objects.PageContext.Objs;
using WebData.Objects.PageContext.Service;
using WebData.Objects.PageContext.Utilities;

namespace WebData.Components.Pages
{
    public partial class LoginRegister
    {
        #region Injecte die Servies
        [Inject]
        public AppBehaviorManager AppBehavior { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        #endregion
        protected override Task OnInitializedAsync()
        {
            AppBehavior.BenutzerVerwaltung.AppBehavior.NavigationManager = NavigationManager;
            return base.OnInitializedAsync();
        }
    }
}
