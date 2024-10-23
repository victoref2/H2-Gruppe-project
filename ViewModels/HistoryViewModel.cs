using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace H2_Gruppe_project.ViewModels
{
    public partial class HistoryViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly User _loggedInUser;
        private readonly Database _database;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private ObservableCollection<Auction> yourAuctions;

        [ObservableProperty]
        private ObservableCollection<Auction> currentAuctions;

        public HistoryViewModel(MainWindowViewModel mainWindowViewModel, User loggedInUser, Database database)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _loggedInUser = loggedInUser;
            _database = database;

            UserName = _loggedInUser.Name;
        }

        private void LoadAuctions()
        {
            YourAuctions = new ObservableCollection<Auction>(_database.GetAllEndedAuctionsByBuyerId(_loggedInUser.Id));
            CurrentAuctions = new ObservableCollection<Auction>(_database.GetAllEndedAuctionsBySellerId(_loggedInUser.Id));
        }
        [RelayCommand]
        public void GoToAddVehicle()
        {
            _mainWindowViewModel.SwitchViewModel(new AddVHViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }

        [RelayCommand]
        public void GoToProfile()
        {
            _mainWindowViewModel.SwitchViewModel(new ProfileViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }

        [RelayCommand]
        public void Logout()
        {
            _mainWindowViewModel.SwitchViewModel(new LoginViewModel(_mainWindowViewModel, _database));
        }

        [RelayCommand]
        public void GoToAuctionHistory()
        {
            _mainWindowViewModel.SwitchViewModel(new DashboardViewModel(_mainWindowViewModel, _loggedInUser, _database));
        }
    }
}
