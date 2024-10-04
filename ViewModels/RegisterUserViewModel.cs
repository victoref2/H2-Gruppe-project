using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;
using System;
using System.Globalization;
using Avalonia.Data.Converters;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Tmds.DBus.Protocol;

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
        private string passwordConfirm;

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
            Credit = 0;
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

                if (Password != PasswordConfirm)
                {
                    Message = "Passwords do not match!";
                    return;
                }

                if (!ValidatePassword(Password))
                {
                    Message = "Password must be at least 8 characters long, and contain at least one upper case and one special character.";
                    return;
                }

                if (!ValidateEmail(Email))
                {
                    Message = "Email must be from hotmail, gmail, or similar domain.";
                    return;
                }

                if (!IsCorporateUser)
                {
                    if (!ValidateCPR(CprNumber))
                    {
                        Message = "CPR number must be in the format DDMMYY-XXXX";
                        return;
                    }
                }

                if (IsCorporateUser)
                {
                    if (string.IsNullOrWhiteSpace(CvrNumber))
                    {
                        Message = "CVR Number cannot be empty.";
                        return;
                    }

                    if (!ValidateCVR(CvrNumber))
                    {
                        Message = "CVR number must be an 8-digit number.";
                        return;
                    }
                }


                var hashedPassword = User.HashPassword(Password);

                await Task.Run(() =>
                {
                    if (IsCorporateUser)
                    {

                        if (string.IsNullOrWhiteSpace(CvrNumber))
                        {
                            Message = "CVR Number cannot be empty.";
                            return;
                        }
                        var newCorporateUser = new CorporateUser(
                            id: 0,
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
                            id: 0,
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

        private bool ValidatePassword(string password)
        {
            return password.Length >= 8 &&
            Regex.IsMatch(password, @"[A-Z]") &&
            Regex.IsMatch(password, @"[\W_]");
        }

        private bool ValidateCPR(string cprNumber)
        {
            return Regex.IsMatch(cprNumber, @"^\d{6}-\d{4}$");
        }

        private bool ValidateCVR(string cvrNumber)
        {
            return Regex.IsMatch(cvrNumber, @"^\d{8}$");
        }

        private bool ValidateEmail(string email)
        {
            return Regex.IsMatch(email, @"@(hotmail|gmail|outlook|yahoo)\.(com|dk)");
        }


        public void GoBackCancel()
        {
            _mainWindowViewModel.SwitchViewModel(new LoginViewModel(_mainWindowViewModel, _database));
        }
    }
}
