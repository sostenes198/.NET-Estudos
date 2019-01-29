using System;
using System.Collections.Generic;
using System.Linq;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Configurations.AppConfigurations
{
    internal sealed class ReactManagementAppConfiguration
    {
        public static readonly string SectionName = $"{AppSettingsSectionReactiveManagementConfigurations.AppSettingsSectionName}:AppConfiguration";
        public string ConnectionString { get; set; }
        public string SentinelKey { get; set; }
        public int CacheExpirationInHours { get; set; }
        public FilterConfiguration Filter { get; set; }

        private const int DAY = 24;
        private const int MONTH = 30;

        public ReactManagementAppConfiguration()
        {
            ConnectionString = string.Empty;
            SentinelKey = string.Empty;
            CacheExpirationInHours = DAY * MONTH;
            Filter = new FilterConfiguration();
        }

        public void Validate()
        {
            var erros = new List<Exception>();

            if (string.IsNullOrWhiteSpace(ConnectionString))
                erros.Add(new Exception($"{SectionName}:{nameof(ConnectionString)} não poder ser nullo ou vazio"));

            if (string.IsNullOrWhiteSpace(SentinelKey))
                erros.Add(new Exception($"{SectionName}:{nameof(SentinelKey)} não poder ser nullo ou vazio"));

            if (erros.Any())
                throw new AggregateException($"{SectionName} possui erros de configuração.", erros);
        }
    }
}