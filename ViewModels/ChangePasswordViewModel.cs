using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using H2_Gruppe_project.DatabaseClasses;
using H2_Gruppe_project.Classes;
using System;
using System.Text.RegularExpressions;

namespace H2_Gruppe_project.ViewModels
{
    public partial class ChangePasswordViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string currentPassword;

        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private string errorMessage;

        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly Database _database;
        private readonly User _loggedInUser;

        public ChangePasswordViewModel(MainWindowViewModel mainWindowViewModel, User loggedInUser, Database database)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _loggedInUser = loggedInUser;
            _database = database;
        }

        [RelayCommand]
        public void ChangePassword()
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "All fields are required.";
                return;
            }

            if (User.HashPassword(CurrentPassword) != _loggedInUser.PassWord)
            {
                ErrorMessage = "Current password is incorrect.";
                return;
            }

            if (!ValidatePassword(NewPassword))
            {
                ErrorMessage = "New password must be at least 8 characters, contain an uppercase letter and a special character.";
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "New password and confirmation do not match.";
                return;
            }

            try
            {
                _database.UpdateUserPassword(_loggedInUser.Id, NewPassword);

                _loggedInUser.PassWord = User.HashPassword(NewPassword);

                _mainWindowViewModel.SwitchViewModel(new ProfileViewModel(_mainWindowViewModel, _loggedInUser, _database, "Password changed successfully."));
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error changing password: {ex.Message}";
            }
        }

        [RelayCommand]
        public void Cancel()
        {
            _mainWindowViewModel.SwitchViewModel(new ProfileViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }

        private bool ValidatePassword(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, @"[A-Z]") && 
                   Regex.IsMatch(password, @"[\W_]"); 
        }
    }
}
