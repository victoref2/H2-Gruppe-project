using CommunityToolkit.Mvvm.Input;
using H2_Gruppe_project.Classes;

namespace H2_Gruppe_project.ViewModels
{
    public partial class MainMenuViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainViewModel;
        private readonly User _loggedInUser;

        public MainMenuViewModel(MainWindowViewModel mainViewModel, User loggedInUser)
        {
            _mainViewModel = mainViewModel;
            _loggedInUser = loggedInUser;
        }

        [RelayCommand]
        public void GoToAddVHView()
        {
            _mainViewModel.CurrentViewModel = new AddVHViewModel(_mainViewModel, _loggedInUser);
        }
    }
}
