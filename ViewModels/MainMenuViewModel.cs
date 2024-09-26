using CommunityToolkit.Mvvm.Input;

namespace H2_Gruppe_project.ViewModels
{
    public partial class MainMenuViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainViewModel;

        public MainMenuViewModel(MainWindowViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        [RelayCommand]
        public void GoToAddVHView()
        {
            _mainViewModel.CurrentViewModel = new AddVHViewModel(_mainViewModel);
        }
    }
}
