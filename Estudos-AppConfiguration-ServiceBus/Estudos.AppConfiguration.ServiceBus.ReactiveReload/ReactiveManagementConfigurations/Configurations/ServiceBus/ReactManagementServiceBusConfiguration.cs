using System;
using System.Collections.Generic;
using System.Linq;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Configurations.ServiceBus
{
    internal sealed class ReactManagementServiceBusConfiguration
    {
        public static readonly string SectionName = $"{AppSettingsSectionReactiveManagementConfigurations.AppSettingsSectionName}:ServiceBus"; 
        public string ConnectionString { get; set; }

        public string Topic { get; set; }

        public string ApplicationName { get; set; }

        private const int MAX_LENGHT_APPLICATION_NAME = 28;

        public ReactManagementServiceBusConfiguration()
        {
            ConnectionString = string.Empty;
            Topic = string.Empty;
        }
        
        public void Validate()
        {
            var erros = new List<Exception>();

            if (string.IsNullOrWhiteSpace(ConnectionString))
                erros.Add(new Exception($"{SectionName}:{nameof(ConnectionString)} não poder ser nullo ou vazio"));

            if (string.IsNullOrWhiteSpace(Topic))
                erros.Add(new Exception($"{SectionName}:{nameof(Topic)} não poder ser nullo ou vazio"));
            
            if (string.IsNullOrWhiteSpace(ApplicationName) || ApplicationName.Length > MAX_LENGHT_APPLICATION_NAME)
                erros.Add(new Exception($"{SectionName}:{nameof(ApplicationName)} não poder ser nullo ou vazio e não ter mais que {MAX_LENGHT_APPLICATION_NAME} caracters"));

            if (erros.Any())
                throw new AggregateException($"{SectionName} possui erros de configuração.", erros);
        }
    }
}