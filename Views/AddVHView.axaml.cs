using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using H2_Gruppe_project.ViewModels;

namespace H2_Gruppe_project.Views
{
    public partial class AddVHView : UserControl
    {
        public AddVHView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
