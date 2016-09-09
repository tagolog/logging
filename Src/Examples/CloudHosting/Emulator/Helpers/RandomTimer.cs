using System;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Tagolog.Examples.CloudHosting.Emulator.Helpers
{
    class RandomTimer
    {
        public RandomTimer( int minSeconds, int maxSeconds )
        {
            _minSeconds = minSeconds;
            _maxSeconds = maxSeconds;
            _timer.Elapsed += OnTimerElapsed;
        }

        public RandomTimer( string name, int minSeconds, int maxSeconds )
        {
            Name = name;
            _minSeconds = minSeconds;
            _maxSeconds = maxSeconds;
            _timer.Elapsed += OnTimerElapsed;
        }

        public string Name { get; set; }
        public ElapsedEventHandler Elapsed { get; set; }

        public bool IsStarted
        {
            get { return _timer.Enabled; }
        }

        public void Start()
        {
            if ( null == Elapsed )
                throw new InvalidOperationException( "Unable to start timer until correct elapsed hadler will be provided.");

            StartTimer();
        }

        public void Stop()
        {
            _timerEnabled = false;
            _timer.Stop();
        }

        void OnTimerElapsed( object sender, ElapsedEventArgs eventArgs )
        {
            if ( null != Elapsed )
                Elapsed( sender, eventArgs );

            StartTimer();
        }

        void StartTimer()
        {
            if ( !_timerEnabled )
                return;

            _timer.Interval = RandomHelper.GenerateInt( _minSeconds * 1000, _maxSeconds * 1000 );
            _timer.AutoReset = false;
            _timer.Start();
        }

        readonly Timer _timer = new Timer();
        bool _timerEnabled = true;
        readonly int _minSeconds;
        readonly int _maxSeconds;
    }
}
