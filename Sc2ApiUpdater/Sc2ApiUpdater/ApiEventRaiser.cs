using System;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Sc2ApiUpdater
{
    internal sealed class ApiEventRaiser<T> : IApiEventRaiser
        where T : ApiObject
    {
        private static readonly Uri baseUri = new Uri(@"http://localhost:6119/");

        private readonly HttpClient httpClient;

        private readonly Uri fullUri;

        private T current;

        private bool disposed = false;

        public event EventHandler<ApiObject> ApiObjectChanged;

        public ApiEventRaiser(string request, int timeoutSeconds)
        {
            if (string.IsNullOrWhiteSpace(request))
                throw new ArgumentException(nameof(request));

            if (timeoutSeconds < 0)
                throw new ArgumentOutOfRangeException(nameof(timeoutSeconds));

            this.fullUri = new Uri(baseUri, request);

            this.httpClient = new HttpClient();
            this.httpClient.Timeout = timeoutSeconds != 0 ?
                TimeSpan.FromSeconds(timeoutSeconds) :
                System.Threading.Timeout.InfiniteTimeSpan;
        }

        public async Task CheckApiObject()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(ApiEventRaiser<T>));

            try
            {
                using (var resultStream = await httpClient.GetStreamAsync(fullUri))
                {
                    T newValue = null;
                    if (resultStream.Length != 0)
                    {
                        var serializer = new DataContractJsonSerializer(typeof(T));
                        newValue = (T)serializer.ReadObject(resultStream);
                    }

                    if (newValue != current)
                    {
                        RaiseApiObjectChanged(newValue);
                        current = newValue;
                    }
                }
            }
            catch (HttpRequestException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                httpClient.Dispose();
                disposed = true;
            }
        }

        private void RaiseApiObjectChanged(T newValue)
        {
            ApiObjectChanged?.Invoke(this, newValue);
        }
    }
}
