using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Project.Models
{
    

    class SignInViewModel : Base
    {
        public string UserType { get; set; }
        string username = string.Empty;
        public string Username
        {
            get => username;
            set
            {
                if (username == value)
                {
                    return;
                }
                else
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }
        string password = string.Empty;
        public string Password
        {
            get => password;
            set
            {
                if (password == value)
                {
                    return;
                }
                else
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        public string DisplayName => $"Name Entered: {Username} \r\n Password Entered: {Password}";
    }
}
