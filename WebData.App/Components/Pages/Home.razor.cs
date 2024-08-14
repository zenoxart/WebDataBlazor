using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Manager;
using WebData.Objects.PageContext.Service;

namespace WebData.App.Components.Pages
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
            base.OnInitializedAsync();  
        }
    }
}
