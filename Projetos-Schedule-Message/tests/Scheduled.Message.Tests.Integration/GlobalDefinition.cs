namespace Scheduled.Message.Tests.Integration;

[CollectionDefinition(nameof(GlobalDefinition))]
public class GlobalDefinition : ICollectionFixture<GlobalFixture>
{
    public const string Name = nameof(GlobalDefinition);
}