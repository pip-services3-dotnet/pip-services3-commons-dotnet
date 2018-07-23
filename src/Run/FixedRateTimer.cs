using System;
using System.Threading;

namespace PipServices.Commons.Run
{
    public class FixedRateTimer
    {
        private Timer _timer;
        private readonly object _lock = new object();

        public FixedRateTimer()
        {
            Interval = 60000;
            Delay = 0;
        }

        public FixedRateTimer(Action task, int interval, int delay)
        {
            Task = task;
            Interval = interval;
            Delay = delay;
        }

        public Action Task { get; set; }
        public int Delay { get; set; }
        public int Interval { get; set; }
        public bool IsStarted { get; private set; }

        public void Start()
        {
            lock (_lock)
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }

                _timer = new Timer(
                    (state) =>
                    {
                        if (Task != null) Task();
                    },
                    null, Delay, Interval
                );

                IsStarted = true;
            }
        }

        public void Restart()
        {
            Stop();

            var oldDelay = Delay;
            Delay = Interval;

            Start();

            Delay = oldDelay;
        }

        public void Stop()
        {
            lock (_lock)
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }

                IsStarted = false;
            }
        }

    }
}