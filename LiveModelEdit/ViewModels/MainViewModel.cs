using LiveModelEdit.Models;

namespace LiveModelEdit.ViewModels;

public class MainViewModel
{
    public ScheduleViewModel ScheduleViewModel { get; }

    public MainViewModel()
    {
        ScheduleViewModel = new ScheduleViewModel();
    }
}