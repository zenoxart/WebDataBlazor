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
    public class BenutzerManager : BaseManager
    {
        public UserObject CurrentUser { get; set; } = new UserObject()
        {
        };

        //Personal Activity
        public ChartObject PersonalActivities { get; set; } = new ChartObject()
        {
            Title = "Profile Activities",
            XAxisLabels = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep" },
            Series = new List<ChartSeries>()
            {
                new ChartSeries() { Name = "Series 1", Data = new double[] { 90, 79, 72, 69, 62, 62, 55, 65, 70 } },
                new ChartSeries() { Name = "Series 2", Data = new double[] { 35, 41, 35, 51, 49, 62, 69, 91, 148 } },
            }
        };

        public EncryptionHandler Encrypter { get; set; }

        #region Authentification-Process

        #region LoginValues
        public bool LoginSuccess { get; set; }

        public bool isLoggingIn = false;
        public bool LoginAlert { get; set; } = false;
        public bool IsAuthenticated { get; set; } = false;
        #endregion

        #region RegisterValues
        public bool RegisterSuccess { get; set; }

        public bool isRegistering = false;
        public bool RegisterAlert { get; set; } = false;
        #endregion

        public int activeLoginRegisterTab = 0;

        #region Form-Validation
        public MudForm loginForm;
        public MudForm registerForm;
        #endregion

        #region Form-Model-Data
        public LoginModel loginModel = new LoginModel();
        public RegisterModel registerModel = new RegisterModel();
        #endregion

        #region Login/Register

        public async Task Register()
        {
            await registerForm.Validate();

            if (registerForm.IsValid)
            {
                isRegistering = true;

                Encrypter = new EncryptionHandler(registerModel.Password);

                byte[] pwIV = Encrypter.GenerateIV();


                UserObject person = new UserObject()
                {
                    Email = registerModel.Email,
                    Password = Encrypter.Encrypt(registerModel.Password, pwIV),
                    Name = registerModel.Name,
                    PasswordIV = Convert.ToBase64String(pwIV)
                };

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

                registerModel = new RegisterModel();
            }
        }

        public async Task Login()
        {
            await loginForm.Validate();
            if (loginForm.IsValid)
            {
                isLoggingIn = true;

                Encrypter = new EncryptionHandler(loginModel.Password);
                UserObject? currentUser = null;
                UserObject dummyUser = new UserObject()
                {
                    Id = 0,
                    Name = string.Empty,
                    Password = string.Empty,
                    PasswordIV = string.Empty,
                    Email = loginModel.Email,   
                    Role = UserRoles.Default
                };

                UserObject BenutzerIVRecieved = await ApiService.PostAsync<UserObject>("User/GetUserIV", dummyUser);

                if (BenutzerIVRecieved != null && BenutzerIVRecieved.PasswordIV != string.Empty)
                {
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

                    try
                    {
                        currentUser = await ApiService.PostAsync<UserObject>("User/LoginUser", person);

                        currentUser.Password = Encrypter.Encrypt(
                            loginModel.Password,
                            Convert.FromBase64String(
                                BenutzerIVRecieved.PasswordIV
                            )
                        );
                    }
                    catch (Exception)
                    {
                    }
                }



                LoginSuccess = currentUser != null;

                AppBehavior.BenutzerVerwaltung.IsAuthenticated = LoginSuccess;
                if (LoginSuccess )
                {
                    CurrentUser = currentUser;
                    AppBehavior.NavigationManager.NavigateTo("/",true);
                }

                // Implement your login logic here
                isLoggingIn = false;
                LoginAlert = true;

                loginModel = new LoginModel();
            }
        }

        #endregion 
        #endregion
    }
}
