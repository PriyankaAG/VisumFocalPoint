using System;
using System.Collections.Generic;
using System.Text;

namespace FocalPoint.MainMenu.Models
{
    public class LoginCredentials
    {
        private string username;
        private string password;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
