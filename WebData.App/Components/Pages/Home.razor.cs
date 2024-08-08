﻿using Microsoft.AspNetCore.Components;
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
            // Referenziere die Services zu den Managern
            AppBehavior.InitServices(ApiService, DialogService);


            // Läd die Daten
            // await AppBehavior.Timeline.LoadData();


        }
    }
}
