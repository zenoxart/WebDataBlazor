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
    public class BaseManager : IAppBehavior
    {
        public AppBehaviorManager AppBehavior { get; set; }

        #region Services & Service-Initialisierung
        public IDialogService DialogService { get; set; }

        public ApiService ApiService { get; set; }

        public NavigationManager NavigationManager { get; set; }

        #endregion
    }
}
