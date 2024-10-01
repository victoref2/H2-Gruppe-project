using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using H2_Gruppe_project.DatabaseClasses;
using System;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.ViewModels
{
    public partial class AddVHViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainViewModel;

        private readonly User _loggedInUser;

        public AddVHViewModel(MainWindowViewModel mainViewModel, User loggedInUser)
        {
            _mainViewModel = mainViewModel;
            
            _loggedInUser = loggedInUser;
            
        }

        /*[RelayCommand]
        public void GoToMainMenuView()
        {
            _mainViewModel.CurrentViewModel = new MainMenuViewModel(_mainViewModel);
        }*/
        // [RelayCommand]
        // public void AddVHToDB()
        // {

        // }
                [RelayCommand]
        public void GoBackCancel()
        {
            _mainViewModel.SwitchViewModel(new DashboardViewModel(_mainViewModel,  _loggedInUser));
        }
    }
}
