using System;
using System.Threading;

namespace PipServices3.Commons.Run
{
    /// <summary>
    /// Timer that is triggered in equal time intervals.
    /// 
    /// It has summetric cross-language implementation
    /// and is often used by Pip.Services toolkit to
    /// perform periodic processing and cleanup in microservices.
    /// </summary>
    /// <example>
    /// <code>
    /// class MyComponent 
    /// {
    ///     var timer = new FixedRateTimer(() => { this.cleanup }, 60000, 0);
    ///     ...
    ///     public void )pen(string correlationId)
    ///     {
    ///         ...
    ///         timer.Start();
    ///         ...
    ///     }
    ///     
    ///     public void Open(string correlationId)
    ///     {
    ///         ...
    ///         timer.Stop();
    ///         ...
    ///     }
    ///     
    ///     private void Cleanup()
    ///     {
    ///     ...
    ///     }
    ///     
    ///     ...
    /// }
    /// </code>
    /// </example>
    /// See <see cref="INotifiable"/>
    public class FixedRateTimer
    {
        private Timer _timer;
        private readonly object _lock = new object();

        /// <summary>
        /// Creates new instance of the timer with default parameters.
        /// </summary>
        public FixedRateTimer()
        {
            Interval = 60000;
            Delay = 0;
        }

        /// <summary>
        /// Creates new instance of the timer and sets its values.
        /// </summary>
        /// <param name="task">(optional) a Notifiable object to call when timer is triggered.</param>
        /// <param name="interval">(optional) an interval to trigger timer in milliseconds.</param>
        /// <param name="delay">(optional) a delay before the first triggering in milliseconds.</param>
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

        /// <summary>
        /// Starts the timer.
        /// 
        /// Initially the timer is triggered after delay.After that it is triggered
        /// after interval until it is stopped.
        /// </summary>
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

        /// <summary>
        /// Restart the timer.
        /// </summary>
        public void Restart()
        {
            Stop();

            var oldDelay = Delay;
            Delay = Interval;

            Start();

            Delay = oldDelay;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
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