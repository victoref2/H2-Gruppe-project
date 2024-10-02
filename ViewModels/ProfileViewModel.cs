using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using H2_Gruppe_project.DatabaseClasses;
using H2_Gruppe_project.Classes;
using System;


namespace H2_Gruppe_project.ViewModels
{
    public partial class ProfileViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private decimal balance;

        [ObservableProperty]
        private int auctionCount;

        [ObservableProperty]
        private decimal amountToChange;

        [ObservableProperty]
        private bool isCorporateUser;

        [ObservableProperty]
        private decimal credit; 

        [ObservableProperty]
        private int wonAuctionCount;

        [ObservableProperty]
        private string successMessage;

        private readonly Database _database;


        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly User _loggedInUser;

        public bool IsSuccessMessageVisible => !string.IsNullOrWhiteSpace(SuccessMessage);


        public ProfileViewModel(MainWindowViewModel mainWindowViewModel, User loggedInUser, Database database, string successMessage = null)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _loggedInUser = loggedInUser;
            _database = database;


            Username = _loggedInUser.Name;
            Balance = _loggedInUser.Balance;
            AuctionCount = 0;
            WonAuctionCount = 0;



            SuccessMessage = successMessage;

            IsCorporateUser = IsUserCorporate(_loggedInUser.Id);
            if (IsCorporateUser)
            {
                CorporateUser corporateUser = _database.GetCorporateUser(int.Parse(_loggedInUser.Id));
                Credit = corporateUser.Credit;
            }
        }


        private bool IsUserCorporate(string userId)
        {
            var user = _database.GetCorporateUser(int.Parse(userId));
            return user != null;
        }


        [RelayCommand]
        public void UpdateCredit()
        {
            if (IsCorporateUser && AmountToChange != 0)
            {
                CorporateUser corporateUser = _database.GetCorporateUser(int.Parse(_loggedInUser.Id));
                corporateUser.Credit += AmountToChange;
                _database.UpdateCorporateUser(corporateUser);
                Credit = corporateUser.Credit;

                if (AmountToChange > 0)
                {
                    SuccessMessage = $"Successfully added {AmountToChange} to your credit.";
                }
                else
                {
                    SuccessMessage = $"Successfully subtracted {Math.Abs(AmountToChange)} from your credit.";
                }

                AmountToChange = 0;
            }
            else
            {
                SuccessMessage = "Enter an amount to add or subtract.";
            }

            OnPropertyChanged(nameof(SuccessMessage));
            OnPropertyChanged(nameof(IsSuccessMessageVisible));
        }

        [RelayCommand]
        public void UpdateBalance()
        {
            if (AmountToChange != 0)
            {
                _loggedInUser.Balance += AmountToChange;
                _database.UpdateUserBalance(_loggedInUser.Id, _loggedInUser.Balance);
                SuccessMessage = "Balance updated successfully.";
                AmountToChange = 0;
            }
            else
            {
                SuccessMessage = "Enter an amount to add or subtract.";
            }
        }

        [RelayCommand]
        public void GoBack()
        {
            _mainWindowViewModel.SwitchViewModel(new DashboardViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }
        [RelayCommand]
        public void ChangePassword()
        {
            _mainWindowViewModel.SwitchViewModel(new ChangePasswordViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }
    }
}
