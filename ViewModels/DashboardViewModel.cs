using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;
using System.Threading.Tasks;


namespace H2_Gruppe_project.ViewModels
{
    public partial class DashboardViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string userName;

        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly User _loggedInUser;

        public DashboardViewModel(MainWindowViewModel mainWindowViewModel, User loggedInUser)
        {
            _mainWindowViewModel = mainWindowViewModel;
            _loggedInUser = loggedInUser;

            UserName = _loggedInUser.Name;
        }

        [RelayCommand]
        public void GoToAddVehicle()
        {
            _mainWindowViewModel.SwitchViewModel(new AddVHViewModel(_mainWindowViewModel, _loggedInUser));
        }
    }
}
