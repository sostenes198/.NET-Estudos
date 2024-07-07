// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

using System.ComponentModel.DataAnnotations;

namespace Scheduled.Message.Infrastructure.Databases.Mongo.Configurations;

public class MongoConfigurations
{
    public const string Section = "Mongo";

    [Required] public MongoConfigurationsHangfire Hangfire { get; set; } = new();

    [Required] public MongoConfigurationsCreateIndex CreateIndex { get; set; } = new();
}

public class MongoConfigurationsHangfire
{
    [Required(AllowEmptyStrings = false)] public string ConnectionString { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)] public string HangfireParametersCollection { get; set; } = string.Empty;
}

public class MongoConfigurationsCreateIndex
{
    [Required] public bool Enabled { get; set; } = false;

    [Required] public MongoConfigurationsCreateIndexTTL TTL { get; set; } = new();
}

public class MongoConfigurationsCreateIndexTTL
{
    [Required] public bool Enabled { get; set; } = false;
}