using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;
using System;
using System.Globalization;
using Avalonia.Data.Converters;

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
        private string cprNumber;

        [ObservableProperty]
        private string cvrNumber;

        [ObservableProperty]
        private decimal credit;

        [ObservableProperty]
        private bool isCorporateUser; 
        

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

                var hashedPassword = User.HashPassword(Password);

                await Task.Run(() =>
                {
                    if (IsCorporateUser)
                    {
                        var newCorporateUser = new CorporateUser(
                            id: null,
                            name: Name,
                            passWord: hashedPassword,
                            mail: Email,
                            balance: 0, // Initial balance
                            credit: Credit,
                            cvrNumber: CvrNumber
                        );
                        _database.AddCorporateUser(newCorporateUser);
                    }
                    else
                    {
                        var newPrivateUser = new PrivateUser(
                            id: null,
                            name: Name,
                            passWord: hashedPassword,
                            mail: Email,
                            balance: 0, // Initial balance
                            cprNumber: CprNumber
                        );
                        _database.AddPrivateUser(newPrivateUser);
                    }
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
