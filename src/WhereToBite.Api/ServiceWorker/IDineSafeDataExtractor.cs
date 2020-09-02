using System;

namespace WhereToBite.Api.ServiceWorker
{
    public interface IDineSafeDataExtractor : IDisposable
    {
        void Extract(object info);
    }
}