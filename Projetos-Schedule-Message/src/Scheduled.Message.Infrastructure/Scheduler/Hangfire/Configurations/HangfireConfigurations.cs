using System.ComponentModel.DataAnnotations;

namespace Scheduled.Message.Infrastructure.Scheduler.Hangfire.Configurations;

public class HangfireConfigurations
{
    public const string Section = "Hangfire";

    [Required(AllowEmptyStrings = false)] public int TtlHangfireDocumentInDays { get; set; } = 0;
}