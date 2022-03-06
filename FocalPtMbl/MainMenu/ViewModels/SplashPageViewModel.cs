using FocalPoint.Data;
using FocalPoint.Data.API;
using FocalPtMbl;
using FocalPtMbl.MainMenu.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace FocalPoint.MainMenu.ViewModels
{
    public class SplashPageViewModel : ThemeBaseViewModel
    {
        ICommand buttonCommand = null;
        HttpClient clientHttp = new HttpClient();
        public SplashPageViewModel()
        {
            clientHttp = PreparedClient();
            buttonCommand = new ValidationCommand2(this);
        }
        private HttpClient PreparedClient()
        {
            HttpClientHandler handler = new HttpClientHandler();

            //not sure about this one, but I think it should work to allow all certificates:
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chaun, ssPolicyError) =>
            {
                return true;
            };


            HttpClient client = new HttpClient(handler);

            return client;
        }
        public HttpClient ClientHTTP
        {
            get { return clientHttp; }
        }
        public ICommand ButtonCommand
        {
            get { return buttonCommand; }
            set
            {
                if (buttonCommand != value)
                {
                    buttonCommand = value;
                    OnPropertyChanged("ButtonCommand");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        bool textHasError = false;
        public bool TextHasError
        {
            get { return textHasError; }
            set
            {
                if (textHasError != value)
                {
                    textHasError = value;
                    OnPropertyChanged("TextHasError");
                }
            }
        }

        string validationResultText = "";
        public string ValidationResultText
        {
            get { return validationResultText; }
            set
            {
                validationResultText = value;
                OnPropertyChanged("ValidationResultText");
            }
        }
    }
    public class ValidationCommand2 : ICommand
    {
        public event EventHandler CanExecuteChanged;

        SplashPageViewModel viewModel;
        public ValidationCommand2(SplashPageViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object Login)
        {
            if (string.IsNullOrEmpty((string)Login) || Login == null)
            {
                viewModel.ValidationResultText = "Login cannot be empty";
                viewModel.TextHasError = true;
                return false;
            }
            viewModel.ValidationResultText = "";
            viewModel.TextHasError = false;
            return true;
        }

        public async void Execute(object Login)
        {
            try
            {

                //Uri uri = new Uri(string.Format("https://10.0.2.2/RestServiceDemo/restservice/"));
                // var response = await clientHttp.GetAsync(uri);
                Uri uri = new Uri(string.Format("https://10.0.2.2:56883/Mobile/Compatible"));
                Uri uri2 = new Uri(string.Format("https://10.0.2.2:56883/Mobile/Login"));
                Uri uri3 = new Uri(string.Format("https://10.0.2.2:56883/Mobile/V1/ConnectionLimit"));
                var stringContent = new StringContent(
                                                      JsonConvert.SerializeObject(3.0),
                                                      Encoding.UTF8,
                                                      "application/json");
                string Password = "+Eg/bz+kAL5rp1moMUdS7B5o1MQZPNxbvi3bdu05huI=";
                string Username =Login.ToString();
                var stringContent2 = new StringContent(
                                      JsonConvert.SerializeObject(new { Username, Password }),
                                      Encoding.UTF8,
                                      "application/json");
                //var response =  viewModel.ClientHTTP.PostAsync(uri, stringContent).Result;
                var response = viewModel.ClientHTTP.PostAsync(uri, stringContent).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    //"{\"APIHighVersion\":1,\"APILowVersion\":1,\"FocalPtVersion\":{\"_Build\":3,\"_Major\":2,\"_Minor\":55,\"_Revision\":-1},\"LocalIP\":\"172.24.64.1\"}"
                    string content =  response.Content.ReadAsStringAsync().Result;
                    var compatible = JsonConvert.DeserializeObject<CompatibleResults>(content); //JsonSerializer.Deserialize<List<CompatibleResults>>(content);
                    if (compatible.APIHighVersion <= 3 && compatible.APILowVersion >=1)
                    {
                       response = viewModel.ClientHTTP.PostAsync(uri2, stringContent2).Result;
                       var content2 = response.Content.ReadAsStringAsync().Result.ToString();
                        var token = JsonConvert.DeserializeObject<Guid>(content2).ToString();
                        if(token == "00000000-0000-0000-0000-000000000000")
                        {
                            App.Current.MainPage.DisplayAlert("Failure to login", "UserName or Password Incorrect", "OK");
                        }
                        DataManager.Settings.UserToken = token;
                    }
                    else
                    {
                      //  await App.Current.MainPage.DisplayAlert("Version Incompatibale with Server", "OK");
                    }
                     ;//JsonSerializer.Deserialize<List<TodoItem>>(content, serializerOptions);
                }
            }
            catch (Exception ex)
            {
                //await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            viewModel.ValidationResultText = $"Login {Login} is correct";
        }
    }
}

