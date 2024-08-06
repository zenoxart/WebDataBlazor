using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.Service;

namespace WebData.Objects.PageContext.Manager
{
    public class AppManager
    {

        public void InitServices(ApiService _apiService, IDialogService _dialogService)
        {
            Timeline.APIService = _apiService;
            Timeline.DialogService = _dialogService;

            News.APIService = _apiService;
            News.DialogService = _dialogService;

            Aufgaben.APIService = _apiService;
            Aufgaben.DialogService = _dialogService;

            BenutzerVerwaltung.APIService = _apiService;
            BenutzerVerwaltung.DialogService = _dialogService;


        }

        public TimelineManager Timeline { get; set; } = new TimelineManager();

        public NewsManager News { get; set; } = new NewsManager();

        public AufgabenManager Aufgaben { get; set; } = new AufgabenManager();

        public BenutzerManager BenutzerVerwaltung { get; set; } = new BenutzerManager();

    }
}
