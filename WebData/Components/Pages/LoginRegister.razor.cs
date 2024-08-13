using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Xml.Linq;
using WebData.Objects.PageContext.CModel;
using WebData.Objects.PageContext.Objs;
using WebData.Objects.PageContext.Service;
using WebData.Objects.PageContext.Utilities;

namespace WebData.Components.Pages
{
    public partial class LoginRegister
    {

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        private int activeTab = 0;
        private bool isLoggingIn = false;
        private bool isRegistering = false;

        private MudForm loginForm;
        private MudForm registerForm;

        private LoginModel loginModel = new LoginModel();
        private RegisterModel registerModel = new RegisterModel();

        public EncryptionHandler Encrypter { get; set; }

        [Inject]
        protected ApiService ApiService { get; set; }

        private async Task Login()
        {
            await loginForm.Validate();
            if (loginForm.IsValid)
            {
                isLoggingIn = true;

                Encrypter = new EncryptionHandler(loginModel.Password);
                BenutzerObject? currentUser = null;

                BenutzerObject BenutzerIVRecieved = await ApiService.PostAsync<BenutzerObject>("UserManagement/GetUserIV", new BenutzerObject() { Id=0,Name=string.Empty,Password=string.Empty,PasswordIV=string.Empty, Email = loginModel.Email});
                
                if (BenutzerIVRecieved != null && BenutzerIVRecieved.PasswordIV != string.Empty)
                {
                    BenutzerObject person = new BenutzerObject()
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

                        currentUser = await ApiService.PostAsync<BenutzerObject>("UserManagement/LoginUser", person);
                    }
                    catch (Exception)
                    {
                    }
                }



                LoginSuccess = currentUser != null;


                // Implement your login logic here
                isLoggingIn = false;
                LoginAlert = true;
            }
        }

        public bool RegisterSuccess { get; set; }

        public bool LoginSuccess { get; set; }

        public bool RegisterAlert { get; set; } = false;
        public bool LoginAlert { get; set; } = false;

        private async Task Register()
        {
            await registerForm.Validate();

            if (registerForm.IsValid)
            {
                isRegistering = true;

                Encrypter = new EncryptionHandler(registerModel.Password);

                byte[] pwIV = Encrypter.GenerateIV();


                BenutzerObject person = new BenutzerObject()
                {
                    Email = registerModel.Email,
                    Password = Encrypter.Encrypt(registerModel.Password, pwIV),
                    Name = registerModel.Name,
                    PasswordIV = Convert.ToBase64String(pwIV)
                };

                try
                {
                    RegisterSuccess = (await ApiService.PostAsync<bool>("UserManagement/RegisterUser", person));
                }
                catch (Exception)
                {
                    RegisterSuccess = false;
                }
                RegisterAlert = true;
                isRegistering = false;


            }
        }

       
    }
}
