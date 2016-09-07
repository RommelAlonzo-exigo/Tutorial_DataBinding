using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public class UsernameViewModel : INotifyPropertyChanged
    {
        private INavigation navigation;
        public UsernameViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }


        private string username = string.Empty;
        public string Username
        {
            get { return username; }
            set
            {
                if (username == value)
                    return;

                username = value;
                OnPropertyChanged("Username");
                OnPropertyChanged("Display");
            }
        }

        public string Display
        {
            get { return "Hello there: " + username; }
        }

        private Command loginCommand;
        public Command LoginCommand
        {
            get { return loginCommand ?? 
                    (loginCommand = new Command<string>(ExecuteLoginCommand, CanLogin));
            }
        }

        private void ExecuteLoginCommand(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            navigation.PushAsync(new TimeListPage());
        }

        private bool CanLogin(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    public class LoginPage : ContentPage
    {
        public LoginPage()
        {
            this.BindingContext = new UsernameViewModel(Navigation);
            var entry = new Entry
            {
                Placeholder = "Enter Username"
            };
            entry.SetBinding<UsernameViewModel>(Entry.TextProperty,
                vm => vm.Username, BindingMode.TwoWay);

            var label = new Label
            {

            };

            label.SetBinding<UsernameViewModel>(Label.TextProperty,
                vm => vm.Display);

            var button = new Button
            {
                Text = "Login"
            };

            button.SetBinding<UsernameViewModel>(Button.CommandProperty,
                vm => vm.LoginCommand);

            button.SetBinding<UsernameViewModel>(Button.CommandParameterProperty,
                vm => vm.Username);

            Content = new StackLayout
            {
                Padding = 10,
                Spacing = 10,
                Children = { entry, label, button }
            };
        }
    }
}
