using Avalonia.Controls;
using LiveModelEdit.ViewModels;

namespace LiveModelEdit
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}