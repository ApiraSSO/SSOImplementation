using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using ElasticSearchClient.Connection.Modifiers;
using Kernel.Initialisation;
using Kernel.Reflection;
using Nest;

namespace ElasticSearchClient.Connection
{
    internal class ConnectionSettingsProvider : IConnectionSettingsProvider, IAutoRegisterAsTransient
    {
        private readonly Func<IConnection> _connectionFactory;
        private readonly Func<IConnectionPool> _connectionPoolFactory;

        public ConnectionSettingsProvider(Func<IConnection> connectionFactory, Func<IConnectionPool> connectionPoolFactory)
        {
            this._connectionFactory = connectionFactory;
            this._connectionPoolFactory = connectionPoolFactory;
        }
        public ConnectionSettings DefaultSettings()
        {
            var connection = this._connectionFactory();
            var connectionPool = this._connectionPoolFactory();
            var settings = new ConnectionSettings(connectionPool, connection);
            this.ApplyModifiers(settings);
            return settings;
        }

        private void ApplyModifiers(ConnectionSettings settings)
        {
            var modifiers = this.ResolveModifiers();
            modifiers.Aggregate(settings, (s, next) => next.Modify(s));
        }

        private IEnumerable<ConnectionSettingModifier> ResolveModifiers()
        {
            var resolver = ApplicationConfiguration.Instance.DependencyResolver;

            var modifiers = this.ResolveModifiersFromAssembly();

            if (resolver != null)
            {
                var resolved = resolver.ResolveAll<ConnectionSettingModifier>();
                modifiers = modifiers.Except(resolved);
            }
                
            return modifiers;
        }

        private IEnumerable<ConnectionSettingModifier> ResolveModifiersFromAssembly()
        {
            var assemblyToScan = this.GetType().Assembly;
            var modifiers = ReflectionHelper.GetAllTypes(new[] { assemblyToScan },
                t => !t.IsAbstract && !t.IsInterface && typeof(ConnectionSettingModifier).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t) as ConnectionSettingModifier);
            return modifiers;
        }
    }
}