using CommunityToolkit.Mvvm.Input;
using H2_Gruppe_project.DatabaseClasses;

namespace H2_Gruppe_project.ViewModels
{
    public partial class AddVHViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainViewModel;

        public AddVHViewModel(MainWindowViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        /*[RelayCommand]
        public void GoToMainMenuView()
        {
            _mainViewModel.CurrentViewModel = new MainMenuViewModel(_mainViewModel);
        }*/
        [RelayCommand]
        public void AddVHToDB()
        {

        }
    }
}
