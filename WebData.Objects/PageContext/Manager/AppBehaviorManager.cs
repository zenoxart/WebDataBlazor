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
    /// Das IAppManager-Interface definiert, dass jeder Manager diese Komponenten implementieren muss 
    /// um in der Client-Factory-Appkontext-Infrastruktur produziert werden zu können
    /// </summary>
    public interface IAppManager
    {
        /// <summary>
        /// Referenz zur Root-Instanz der Client-Factory-Appkontext-Infrastruktur 
        /// </summary>
        public AppBehaviorManager AppBehavior { get; set; }

        /// <summary>
        /// Referenz zum REST-API-Kommunikations-Manager
        /// </summary>
        public ApiService ApiService { get; set; }

        /// <summary>
        /// Regerenz zum Dialog-Manager
        /// </summary>
        public IDialogService DialogService { get; set; }
        /// <summary>
        /// Referenz zum Navigations-Manager
        /// </summary>
        public NavigationManager NavigationManager { get; set; }
    }

    /// <summary>
    /// Die AppBehavior definiert eine Client-Factory-Appkontext-Infrastruktur 
    /// um managerübergreifen Zugriff der selben Instanz zu ermöglichen
    /// </summary>
    public class AppBehaviorManager
    {
        /// <summary>
        /// Definiert den Service fürs Navigations-Verhalten
        /// </summary>
        public NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Definiert den Service zur REST-API
        /// </summary>
        public ApiService ApiService { get; set; }

        /// <summary>
        /// Definiert den Service zum Abhandeln von Pop-Up-Dialogen
        /// </summary>
        public IDialogService DialogService { get; set; }

        /// <summary>
        /// Erstellt einen neuen Manager mit Referenz zum Root der AppBehavior
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Produziere<T>() where T : IAppManager, new()
        {
            var NeuesObjekt = new T
            {
                // Die Infrastruktur im neuen Objekt einstellen
                AppBehavior = this,

                // Die Services übergeben
                ApiService = ApiService,
                DialogService = DialogService,
                NavigationManager = NavigationManager
            };

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
                _timeline ??= this.Produziere<TimelineManager>();

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
                _news ??= this.Produziere<NewsManager>();

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
                _aufgaben ??= this.Produziere<AufgabenManager>();

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
                _benutzerVerwaltung ??= this.Produziere<BenutzerManager>();

                return _benutzerVerwaltung;
            }
            set { _benutzerVerwaltung = value; }
        }



    }
}
