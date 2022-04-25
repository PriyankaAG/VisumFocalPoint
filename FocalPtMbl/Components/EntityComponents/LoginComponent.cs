using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FocalPoint.Components.Interface;
using Newtonsoft.Json;
using Visum.Services.Mobile.Entities;

namespace FocalPoint.Components.EntityComponents
{
    class LoginComponent : ILoginComponent
    {
        IAPICompnent apiComponent;

        const string Login = "Login";
        const string LogOff = "logoff";
        const string LoginStores = "LoginStores";

        public LoginComponent()
        {
            apiComponent = new APIComponent();
        }

        public async Task<Guid> UserLogin(string url, string requestString)
        {
            Guid result;
            try
            {
                result = await apiComponent.PostAsync<Guid>(url, requestString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<bool> Logoff()
        {
            bool result;
            try
            {
                result = await apiComponent.SendAsync<bool>(LogOff, "", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<List<Company>> GetLoginStores()
        {
            List<Company> result;
            try
            {
                result = await apiComponent.GetAsync<List<Company>>(LoginStores);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
