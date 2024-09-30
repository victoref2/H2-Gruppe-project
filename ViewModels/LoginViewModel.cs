using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using H2_Gruppe_project.DatabaseClasses;
using H2_Gruppe_project.Classes;

using System;

namespace H2_Gruppe_project.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string errorMessage;

        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly Database _database;

        public LoginViewModel(MainWindowViewModel mainWindowViewModel, Database database)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _database = database;
        }

        [RelayCommand]
        public async Task LoginAsync()
        {
            try
            {
                var user = _database.GetUserByEmail(Email);
                
                if (user != null && user.PassWord == User.HashPassword(Password))
                {
                    _mainWindowViewModel.SwitchViewModel(new DashboardViewModel(_mainWindowViewModel, user));
                }
                else
                {
                    ErrorMessage = "Invalid email or password.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Login error: {ex.Message}";
            }
        }
        [RelayCommand]
        public void GoToRegisterUser()
        {
            _mainWindowViewModel.SwitchViewModel(new RegisterUserViewModel(_mainWindowViewModel));
        }
    }
}
