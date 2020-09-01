using System;
using System.Threading.Tasks;

namespace WhereToBite.Api.ServiceWorker
{
    public interface IDineSafeDataExtractor : IDisposable
    {
        void Extract(object info);
    }
}