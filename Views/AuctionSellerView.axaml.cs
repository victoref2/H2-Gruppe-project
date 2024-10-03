using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using H2_Gruppe_project.ViewModels;
using H2_Gruppe_project.Classes;
using H2_Gruppe_project.DatabaseClasses;

namespace H2_Gruppe_project.Views
{
    public partial class AuctionSellerView : UserControl
    {
        public AuctionSellerView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
