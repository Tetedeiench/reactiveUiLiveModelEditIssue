using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NoAvalo.Models;

public class Schedule : INotifyPropertyChanged
{
    private ObservableCollection<SchedulePeriod> _periods;

    public ObservableCollection<SchedulePeriod> Periods
    {
        get => _periods;
        set
        {
            if (_periods == value)
                return;

            _periods = value;

            NotifyPropertyChanged("Periods");
        }
    }

    public Schedule()
    {
        Periods = new ObservableCollection<SchedulePeriod>();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}