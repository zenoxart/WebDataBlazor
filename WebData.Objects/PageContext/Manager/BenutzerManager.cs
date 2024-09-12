using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebData.Objects.PageContext.CModel;
using WebData.Objects.PageContext.Objs;
using WebData.Objects.PageContext.Service;
using WebData.Objects.PageContext.Utilities;

namespace WebData.Objects.PageContext.Manager
{
    /// <summary>
    /// Definiert die clientseite Verwaltung der Benutzer-Informationen
    /// </summary>
    public class BenutzerManager : BaseManager
    {
        /// <summary>
        /// Definiert den aktuellen Benutzer
        /// </summary>
        public UserObject CurrentUser { get; set; } = new UserObject()
        {
        };

        /// <summary>
        /// Definiert das Diagram für die Benutzeraktivitäten
        /// </summary>
        public TimelineChart PersonalActivities { get; set; } = new TimelineChart()
        {
            Title = "Profile Activities",
            XAxisLabels = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep" },
            Series = new List<ChartSeries>()
            {
                new ChartSeries() { Name = "Series 1", Data = new double[] { 90, 79, 72, 69, 62, 62, 55, 65, 70 } },
                new ChartSeries() { Name = "Series 2", Data = new double[] { 35, 41, 35, 51, 49, 62, 69, 91, 148 } },
            }
        };

        /// <summary>
        /// Definiert den Verschlüsselungshandler für die Registrierung/Anmeldung
        /// </summary>
        public EncryptionHandler Encrypter { get; set; }

        #region Authentification-Process

        #region LoginValues
        /// <summary>
        /// Gibt an ob das Anmelden erfolgreich war
        /// </summary>
        public bool LoginSuccess { get; set; }
        /// <summary>
        /// Gibt an ob gerade versucht wird sich Anzumelden
        /// </summary>

        public bool isLoggingIn = false;

        /// <summary>
        /// Gibt an ob eine Warnmeldung bei Anmeldefehlschlag angezeigt wird
        /// </summary>
        public bool LoginAlert { get; set; } = false;

        /// <summary>
        /// Gibt an ob der aktuelle Benutzer authentifiziert & angemeldet ist
        /// </summary>
        public bool IsAuthenticated { get; set; } = false;
        #endregion

        #region RegisterValues
        /// <summary>
        /// Gibt an ob das Benutzer erstellen erfolgreich war
        /// </summary>
        public bool RegisterSuccess { get; set; }

        /// <summary>
        /// Gibt an ob gerade versucht wird einen Benutzer zu erstellen
        /// </summary>
        public bool isRegistering = false;

        /// <summary>
        /// Gibt an ob eine Warnmeldung bei Fehlschlag der Benutzererstellung angezeigt wird
        /// </summary>
        public bool RegisterAlert { get; set; } = false;
        #endregion

        /// <summary>
        /// Gibt an ob der Login oder der Register-Tab angezeigt wird
        /// </summary>
        public int activeLoginRegisterTab = 0;

        #region Form-Validation
        /// <summary>
        /// Definiert ein Formular für das Anmelden
        /// </summary>
        public MudForm loginForm;

        /// <summary>
        /// Definiert ein Formular für das Benutzer erstellen
        /// </summary>
        public MudForm registerForm;
        #endregion

        #region Form-Model-Data

        /// <summary>
        /// Definiert das API-Model für das Senden der Anmeldedaten
        /// </summary>
        public LoginModel loginModel = new LoginModel();

        /// <summary>
        /// Definiert das API-Model für das Senden der Benutzererstellungsdaten
        /// </summary>
        public RegisterModel registerModel = new RegisterModel();
        #endregion

        #region Login/Register

        /// <summary>
        /// Erstellt einen neuen Benutzer
        /// </summary>
        public async Task Register()
        {
            // Überprüft ob das Formular korrekt ausgefüllt wurde
            await registerForm.Validate();

            if (registerForm.IsValid)
            {
                isRegistering = true;

                // Initialisiert den Verschlüsselungsmanager mit dem Passwort
                Encrypter = new EncryptionHandler(registerModel.Password);

                // Erstellt für den Benutzer eigene Salt-IV-Werte
                byte[] pwIV = Encrypter.GenerateIV();

                // Erstellt das zu sendene Benutzerobjekt
                UserObject person = new UserObject()
                {
                    Email = registerModel.Email,
                    Password = Encrypter.Encrypt(registerModel.Password, pwIV),
                    Name = registerModel.Name,
                    PasswordIV = Convert.ToBase64String(pwIV)
                };

                // Versucht den Benutzer anzulegen an der REST-API
                try
                {
                    RegisterSuccess = (await ApiService.PostAsync<bool>("User/RegisterUser", person));
                }
                catch (Exception)
                {
                    RegisterSuccess = false;
                }

                RegisterAlert = true;
                isRegistering = false;

                // Bereinigt das Benutzererstellungsmodel
                registerModel = new RegisterModel();
            }
        }

        /// <summary>
        /// Meldet einen bestehenden Benutzer an
        /// </summary>
        /// <returns></returns>
        public async Task Login()
        {
            // Überprüft ob das Formular korrekt ausgefüllt wurde
            await loginForm.Validate();

            if (loginForm.IsValid)
            {
                isLoggingIn = true;

                // Initialisiert den Verschlüsselungsmanager mit dem Passwort
                Encrypter = new EncryptionHandler(loginModel.Password);

                UserObject? currentUser = null;

                // Erstellt das zu sendene Benutzerobjekt
                UserObject sendUser = new UserObject()
                {
                    Id = 0,
                    Name = string.Empty,
                    Password = string.Empty,
                    PasswordIV = string.Empty,
                    Email = loginModel.Email,   
                    Role = UserRoles.Default
                };

                // Erhält die zugehörigen Salt-IV-Werte über die Email
                UserObject BenutzerIVRecieved = await ApiService.PostAsync<UserObject>("User/GetUserIV", sendUser);

                if (BenutzerIVRecieved != null && BenutzerIVRecieved.PasswordIV != string.Empty)
                {
                    // Befüllt das zu sendene Benutzerobjekt
                    UserObject person = new UserObject()
                    {
                        Email = BenutzerIVRecieved.Email,
                        Password = Encrypter.Encrypt(
                            loginModel.Password,
                            Convert.FromBase64String(
                                BenutzerIVRecieved.PasswordIV
                            )
                        ),
                        Id = BenutzerIVRecieved.Id,
                        Name = BenutzerIVRecieved.Name,
                        PasswordIV = string.Empty
                    };

                    // Versucht den Benutzer anzumelden an der REST-API
                    try
                    {
                        currentUser = await ApiService.PostAsync<UserObject>("User/LoginUser", person);

                        // Auf dem Client das Verschlüsselte Passwort speichern (TODO: gehört noch sicherer gespeichert!)
                        currentUser.Password = Encrypter.Encrypt(
                            loginModel.Password,
                            Convert.FromBase64String(
                                BenutzerIVRecieved.PasswordIV
                            )
                        );
                    }
                    catch (Exception)
                    {
                        // TODO: Exceptionhandeling
                    }
                }

                LoginSuccess = currentUser != null;

                AppBehavior.BenutzerVerwaltung.IsAuthenticated = LoginSuccess;

                // Wenn die Anmeldung erfolgreich ist, setzte den aktuellen Benutzer und Navigiere auf die Home-Page
                if (LoginSuccess)
                {
                    CurrentUser = currentUser;
                    AppBehavior.NavigationManager.NavigateTo("/",true);
                }

                // Implement your login logic here
                isLoggingIn = false;
                LoginAlert = true;

                // Bereinigt das Anmeldungsmodel
                loginModel = new LoginModel();
            }
        }

        #endregion 
        #endregion
    }
}
