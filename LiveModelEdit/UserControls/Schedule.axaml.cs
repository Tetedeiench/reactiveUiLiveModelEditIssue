using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LiveModelEdit.UserControls;

public partial class Schedule : UserControl
{
    public Schedule()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}