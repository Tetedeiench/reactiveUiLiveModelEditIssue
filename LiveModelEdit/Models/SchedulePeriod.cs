using System;
using System.ComponentModel;

namespace LiveModelEdit.Models;

public class SchedulePeriod : INotifyPropertyChanged
{
    private bool _isInfinite { get; set; }

    public bool IsInfinite
    {
        get => _isInfinite;
        set {
        if (_isInfinite == value)
            return;
        _isInfinite = value;
            NotifyPropertyChanged("IsInfinite");
        }
    }

    private TimeSpan _duration;

    public TimeSpan Duration
    {
        get => _duration;
        set {
            if (_duration == value)
                return;
            _duration = value;
            NotifyPropertyChanged("Duration");
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;  
    
    private void NotifyPropertyChanged(string propertyName)  
    {  
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }  
}