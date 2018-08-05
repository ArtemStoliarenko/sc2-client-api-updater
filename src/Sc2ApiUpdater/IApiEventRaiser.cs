using System;
using System.Threading.Tasks;

namespace Sc2ApiUpdater
{
    internal interface IApiEventRaiser : IDisposable
    {
        event EventHandler<ApiObject> ApiObjectChanged;

        Task CheckApiObject();
    }
}
