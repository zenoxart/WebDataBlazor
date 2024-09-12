using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Service;

namespace WebData.Objects.PageContext.Manager
{
    /// <summary>
    /// Stellt die Basisfunktionen die jeder Manager haben sollte
    /// </summary>
    public class BaseManager : IAppManager
    {
        /// <summary>
        /// Definiert die Client-Factory-Appkontext-Infrastruktur 
        /// </summary>
        public AppBehaviorManager AppBehavior { get; set; }

        #region Services & Service-Initialisierung

        /// <summary>
        /// Definiert den Service zum Abhandeln von Pop-Up-Dialogen
        /// </summary>
        public IDialogService DialogService { get; set; }

        /// <summary>
        ///  Definiert den Service zur REST-API
        /// </summary>
        public ApiService ApiService { get; set; }

        /// <summary>
        /// Definiert den Service fürs Navigations-Verhalten
        /// </summary>
        public NavigationManager NavigationManager { get; set; }

        #endregion
    }
}
