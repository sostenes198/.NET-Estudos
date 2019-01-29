namespace Estudos.IdempotentConsumer.Enums;

public enum RepositoryEntryState
{
    None = 0,
    Reserved = 1,
    Processing = 2,
    Committed = 3
}