using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;
using System;

namespace H2_Gruppe_project.ViewModels
{
    public partial class RegisterUserViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string message;

        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly Database _database;

        public RegisterUserViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _database = new Database(); 
        }

        [RelayCommand]
        public async Task RegisterUserAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    Message = "All fields are required.";
                    return;
                }

                await Task.Run(() =>
                {
                    var newUser = new User(
                        id: null, 
                        name: Name,
                        passWord: Password,
                        mail: Email
                    );

                    
                    _database.AddUser(newUser);
                });

                Message = "Registration successful!";
                _mainWindowViewModel.SwitchViewModel(new LoginViewModel(_mainWindowViewModel, _database));
            }
            catch (Exception ex)
            {
                Message = $"Error occurred: {ex.Message}";
            }
        }

        public void GoBackCancel()
        {
            _mainWindowViewModel.SwitchViewModel(new LoginViewModel(_mainWindowViewModel, _database));
        }
    }
}
