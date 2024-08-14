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
    public interface IAppBehavior
    {
        public AppBehaviorManager AppBehavior { get; set; }

        public ApiService ApiService { get; set; }

        public IDialogService DialogService { get; set; }

        public NavigationManager NavigationManager { get; set; }
    }
    public class AppBehaviorManager
    {
        //[Inject]
        public NavigationManager NavigationManager { get; set; }

        //[Inject]
        public ApiService ApiService { get; set; }

        //[Inject]
        public IDialogService DialogService { get; set; }

        public virtual T Produziere<T>() where T : IAppBehavior, new()
        {
            var NeuesObjekt = new T();

            // Die Infrastruktur im neuen Objekt einstellen
            NeuesObjekt.AppBehavior = this;
            NeuesObjekt.ApiService = ApiService;
            NeuesObjekt.DialogService = DialogService;
            NeuesObjekt.NavigationManager = NavigationManager;

            // Im Ausgabefenster vom Studio
            // einen Produktionshinweis hinterlassen
            System.Console.WriteLine($"{NeuesObjekt} wurde produziert...");

            return NeuesObjekt;
        }


        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private TimelineManager _timeline;

        /// <summary>
        /// Timeline-Verwaltung
        /// </summary>
        public TimelineManager Timeline
        {
            get
            {
                if (_timeline == null)
                {
                    _timeline = this.Produziere<TimelineManager>();
                }
                return _timeline;
            }
            set { _timeline = value; }
        }

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private NewsManager _news;

        /// <summary>
        /// Nachrichten-Verwaltung
        /// </summary>
        public NewsManager News
        {
            get
            {
                if (_news == null)
                {
                    _news = this.Produziere<NewsManager>();
                }
                return _news;
            }
            set { _news = value; }
        }

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private AufgabenManager _aufgaben;

        /// <summary>
        /// Aufgaben-Verwaltung
        /// </summary>
        public AufgabenManager Aufgaben
        {
            get
            {
                if (_aufgaben == null)
                {
                    _aufgaben = this.Produziere<AufgabenManager>();
                }
                return _aufgaben;
            }
            set { _aufgaben = value; }
        }

        /// <summary>
        /// Internes Feld für die Eigenschaft
        /// </summary>
        private BenutzerManager _benutzerVerwaltung;

        /// <summary>
        /// Benutzer-Verwaltung
        /// </summary>
        public BenutzerManager BenutzerVerwaltung
        {
            get
            {
                if (_benutzerVerwaltung == null)
                {
                    _benutzerVerwaltung = this.Produziere<BenutzerManager>();
                }
                return _benutzerVerwaltung;
            }
            set { _benutzerVerwaltung = value; }
        }



    }
}
