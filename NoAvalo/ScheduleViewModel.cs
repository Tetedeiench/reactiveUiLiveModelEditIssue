using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using NoAvalo.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;

namespace NoAvalo;

public class ScheduleViewModel : ReactiveValidationObject
{
    private readonly Schedule _schedule;

    public Schedule Schedule => _schedule;

    [Reactive]
    public SchedulePeriod Period { get; set; }

    [Reactive]
    public bool IsInfinite { get; set; }

    [Reactive]
    public int HoursPart { get; set; }

    [Reactive]
    public int MinutesPart { get; set; }

    public ReactiveCommand<SchedulePeriod, Unit> SelectPeriodCommand { get; }

    public ScheduleViewModel()
    {
        _schedule = new Schedule();

        _schedule.Periods.Add(new SchedulePeriod()
        {
            Duration = new TimeSpan(1, 11, 0),
            IsInfinite = false
        });

        _schedule.Periods.Add(new SchedulePeriod()
        {
            Duration = new TimeSpan(2, 22, 0),
            IsInfinite = true
        });

        _schedule.Periods.Add(new SchedulePeriod()
        {
            Duration = new TimeSpan(3, 33, 0),
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
            .Throttle(TimeSpan.FromMilliseconds(50))
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .BindTo(this, x => x.Period.IsInfinite);


        //Duration - hour bindings
        //we merge hours and days into a single int value for display and edit

        //model to ViewModel
        this.WhenAnyValue(x => x.Period.Duration,
                duration => duration.Days * 24 + duration.Hours)
            .DistinctUntilChanged()
            .Do(x => Debug.WriteLine(x))
            .ObserveOn(RxApp.MainThreadScheduler)
            .BindTo(this, x => x.HoursPart);

        this.WhenAnyValue(x => x.Period.Duration,
                duration => duration.Minutes)
            .DistinctUntilChanged()
            .Do(x => Debug.WriteLine(x))
            .ObserveOn(RxApp.MainThreadScheduler)
            .BindTo(this, x => x.MinutesPart);

        //viewmodel to model
        this.WhenAnyValue(x => x.HoursPart,
                x => x.MinutesPart,
                (hoursPart, minutesPart) => new TimeSpan(hoursPart, minutesPart, 0))
            .Skip(1)
            .Throttle(TimeSpan.FromMilliseconds(50))
            .DistinctUntilChanged()
            .Do(x => Debug.WriteLine(x))
            .ObserveOn(RxApp.MainThreadScheduler)
            .BindTo(this, x => x.Period.Duration);

        SelectPeriodCommand = ReactiveCommand.Create<SchedulePeriod>(schedulePeriod =>
        {
            Period = schedulePeriod;
        });
    }

}