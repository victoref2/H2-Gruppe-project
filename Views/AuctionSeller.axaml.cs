using Avalonia.Controls;
using H2_Gruppe_project.ViewModels;

namespace H2_Gruppe_project.Views
{
    public partial class AuctionSeller : UserControl
    {
        public AuctionSeller()
        {
            InitializeComponent();
            DataContext = new AuctionSellerModel(new MainWindowViewModel());
        }
    }
}
