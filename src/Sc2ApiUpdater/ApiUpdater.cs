using System;
using System.Collections.Generic;
using System.Threading;

namespace Sc2ApiUpdater
{
    public sealed class ApiUpdater : IDisposable
    {
        private const string uiRequest = "ui";

        private const string gameRequest = "game";

        private const int maxApiRaisers = 2;

        private const int millisecondsMultiplier = 1000;

        private readonly Timer checkTimer;

        private readonly int periodMilliseconds;

        private readonly List<IApiEventRaiser> eventRaisers = new List<IApiEventRaiser>(maxApiRaisers);

        private bool started = false;

        private bool disposed = false;

        public event EventHandler<UiObject> UiChanged;

        public event EventHandler<GameObject> GameChanged;

        public event EventHandler<ApiObject> ApiObjectChanged;

        public ApiUpdater(ApiCalls calls, int periodSeconds, int timeoutSeconds)
        {
            if (calls == ApiCalls.None)
                throw new ArgumentOutOfRangeException(nameof(calls));

            if (periodSeconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(periodSeconds));

            if (timeoutSeconds < 0)
                throw new ArgumentOutOfRangeException(nameof(timeoutSeconds));

            this.periodMilliseconds = periodSeconds * millisecondsMultiplier;
            this.checkTimer = new Timer(this.CheckApiObjects, null, Timeout.Infinite, this.periodMilliseconds);

            if (calls.HasFlag(ApiCalls.Ui))
            {
                var eventRaiser = new ApiEventRaiser<UiObject>(uiRequest, timeoutSeconds);
                eventRaiser.ApiObjectChanged += RaiseUiChanged;
                eventRaisers.Add(eventRaiser);
            }

            if (calls.HasFlag(ApiCalls.Game))
            {
                var eventRaiser = new ApiEventRaiser<GameObject>(uiRequest, timeoutSeconds);
                eventRaiser.ApiObjectChanged += RaiseGameChanged;
                eventRaisers.Add(eventRaiser);
            }
        }

        public void Start()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(ApiUpdater));

            if (!started)
            {
                checkTimer.Change(0, periodMilliseconds);
                started = true;
            }
        }

        public void Stop()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(ApiUpdater));

            if (started)
            {
                checkTimer.Change(Timeout.Infinite, periodMilliseconds);
                started = false;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                if (started)
                {
                    Stop();
                    started = false;
                }
                checkTimer.Dispose();
                disposed = true;
            }
        }

        private void CheckApiObjects(object state)
        {
            eventRaisers.ForEach(eventRaiser => eventRaiser.CheckApiObject());
        }

        private void RaiseUiChanged(object sender, ApiObject apiObject)
        {
            UiChanged?.Invoke(this, (UiObject)apiObject);
            ApiObjectChanged?.Invoke(this, apiObject);
        }

        private void RaiseGameChanged(object sender, ApiObject apiObject)
        {
            GameChanged?.Invoke(this, (GameObject)apiObject);
            ApiObjectChanged?.Invoke(this, apiObject);
        }
    }
}
