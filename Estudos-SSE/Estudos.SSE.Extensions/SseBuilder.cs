using System;
using Estudos.SSE.Core.Authorizations;
using Estudos.SSE.Core.Authorizations.Contracts;
using Estudos.SSE.Core.ClientsIdProviders;
using Estudos.SSE.Core.ClientsIdProviders.Contracts;
using Estudos.SSE.Core.ClientsStorages;
using Estudos.SSE.Core.ClientsStorages.Contracts;

namespace Estudos.SSE.Extensions
{
    // ReSharper disable once InconsistentNaming
    public class SseBuilder
    {
        internal Type AuthorizationType { get; private set; }

        internal Type ClientIdProviderType { get; private set; }

        internal Type ClientSseStorageType { get; private set; }

        public SseBuilder()
        {
            AuthorizationType = typeof(EmptyAuthorizationSse);
            ClientIdProviderType = typeof(NewGuidClientIdProvider);
            ClientSseStorageType = typeof(DistributedCacheClientSseStorage);
        }

        public SseBuilder WithAuthorization<TAuthorization>()
            where TAuthorization : IAuthorizationSse
        {
            AuthorizationType = typeof(TAuthorization);

            return this;
        }

        public SseBuilder WithClientIdProvider<TClientIdProvider>()
            where TClientIdProvider : IClientIdProvider
        {
            ClientIdProviderType = typeof(TClientIdProvider);

            return this;
        }

        public SseBuilder WithClientSseStorage<TClientSseStorage>()
            where TClientSseStorage : IClientSseStorage
        {
            ClientSseStorageType = typeof(TClientSseStorage);

            return this;
        }
    }
}