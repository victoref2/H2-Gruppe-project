using CommunityToolkit.Mvvm.Input;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;


namespace H2_Gruppe_project.ViewModels
{
    public partial class MainMenuViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainViewModel;
        private readonly User _loggedInUser;

        private readonly Database _database;

        public MainMenuViewModel(MainWindowViewModel mainViewModel, User loggedInUser, Database database)
        {
            _mainViewModel = mainViewModel;
            _loggedInUser = loggedInUser;
            _database = database;
        }

        [RelayCommand]
        public void GoToAddVHView()
        {
            _mainViewModel.CurrentViewModel = new AddVHViewModel(_mainViewModel, _loggedInUser);
        }
    }
}
