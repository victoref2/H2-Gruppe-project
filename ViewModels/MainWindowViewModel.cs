using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;
using System;
using System.Threading.Tasks;

namespace H2_Gruppe_project.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        private void SetProperty(ref ViewModelBase currentViewModel, ViewModelBase value)
        {
            throw new NotImplementedException();
        }

        public MainWindowViewModel()
        {
            CurrentViewModel = new LoginViewModel(this, new Database());
        }

        public void SwitchViewModel(ViewModelBase viewModel)
        {
            CurrentViewModel = viewModel;
        }
    }
}