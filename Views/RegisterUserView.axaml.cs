using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace H2_Gruppe_project.Views
{
    public partial class RegisterUserView : UserControl
    {
        public RegisterUserView()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
