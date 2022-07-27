using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using LiveModelEdit.Models;
using ReactiveUI;

namespace LiveModelEdit.ViewModels;

public class ScheduleViewModel : ViewModelBase
{
    private readonly Schedule _schedule;

    public Schedule Schedule => _schedule;

    private SchedulePeriod _period;

    public SchedulePeriod Period
    {
        get => _period;
        set => this.RaiseAndSetIfChanged(ref _period, value);
    }

    private bool _isInfinite;

    public bool IsInfinite
    {
        get => _isInfinite;
        set => this.RaiseAndSetIfChanged(ref _isInfinite, value);
    }

    private int _hoursPart;

    public int HoursPart
    {
        get => _hoursPart;
        set => this.RaiseAndSetIfChanged(ref _hoursPart, value);
    }

    private int _minutesPart;

    public int MinutesPart
    {
        get => _minutesPart;
        set => this.RaiseAndSetIfChanged(ref _minutesPart, value);
    }
    
    public ReactiveCommand<SchedulePeriod, Unit> SelectPeriodCommand { get; }

    public ScheduleViewModel()
    {
        _schedule = new Schedule();
        
        _schedule.Periods.Add(new SchedulePeriod()
        {
            Duration = new TimeSpan(1,11,0),
            IsInfinite = false
        });
        
        _schedule.Periods.Add(new SchedulePeriod()
        {
            Duration = new TimeSpan(2,22,0),
            IsInfinite = true
        });
        
        _schedule.Periods.Add(new SchedulePeriod()
        {
            Duration = new TimeSpan(3,33,0),
            IsInfinite = false
        });

        Period = _schedule.Periods.First();

        //IsInfinite bindings
        
        //model to ViewModel
        this.WhenAnyValue(x => x.Period.IsInfinite)
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .BindTo(this, x => x.IsInfinite);
        
        //viewmodel to model
        this.WhenAnyValue(x => x.IsInfinite)
            .Skip(1)
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .BindTo(this, x => x.Period.IsInfinite);
        
        
        //Duration - hour bindings
        //we merge hours and days into a single int value for display and edit
        
        //model to ViewModel
        this.WhenAnyValue(x => x.Period.Duration,
                duration => duration.Days * 24 + duration.Hours)
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .BindTo(this, x => x.HoursPart);
        
        this.WhenAnyValue(x => x.Period.Duration,
                duration => duration.Minutes)
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .BindTo(this, x => x.MinutesPart);
        
        //viewmodel to model
        this.WhenAnyValue(x => x.HoursPart,
                x => x.MinutesPart,
                (hoursPart, minutesPart) => new TimeSpan(hoursPart,minutesPart,0))
            .Skip(2)
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .BindTo(this, x => x.Period.Duration);

        SelectPeriodCommand = ReactiveCommand.Create<SchedulePeriod>(schedulePeriod =>
        {
            Period = schedulePeriod;
        });
    }
    
}