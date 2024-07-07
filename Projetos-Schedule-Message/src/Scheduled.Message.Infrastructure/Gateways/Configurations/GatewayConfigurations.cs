using System.ComponentModel.DataAnnotations;

namespace Scheduled.Message.Infrastructure.Gateways.Configurations;

public class GatewayConfigurations
{
    public const string Section = "Gateways";

    [Required] public GatewayConfigurationsVollConfigurations VollScheduler { get; set; } = new();
}

public class GatewayConfigurationsVollConfigurations
{
    public const string ClientName = nameof(VollScheduler);

    [Required(AllowEmptyStrings = false)] public string BaseUrl { get; set; } = string.Empty;
}