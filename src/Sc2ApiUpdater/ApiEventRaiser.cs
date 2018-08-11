using System;
using System.IO;
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

        private T currentApiObject;

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

            T newApiObject = await GetApiObject();
            if (!(currentApiObject?.Equals(newApiObject) ?? currentApiObject == newApiObject))
            {
                currentApiObject = newApiObject;
                RaiseApiObjectChanged(currentApiObject);
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

        private async Task<T> GetApiObject()
        {
            try
            {
                using (var httpStream = await httpClient.GetStreamAsync(fullUri))
                using (var resultStream = new MemoryStream())
                {
                    await httpStream.CopyToAsync(resultStream);
                    if (resultStream.Length != 0)
                    {
                        resultStream.Position = 0;
                        var serializer = new DataContractJsonSerializer(typeof(T));
                        return serializer.ReadObject(resultStream) as T;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        private void RaiseApiObjectChanged(T newValue)
        {
            ApiObjectChanged?.Invoke(this, newValue);
        }
    }
}
