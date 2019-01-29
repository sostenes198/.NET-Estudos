using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Estudos.IdempotentConsumer.Enums;
using Estudos.IdempotentConsumer.Repositories.Base;

namespace Estudos.IdempotentConsumer.Tests.Integration.Repositories.Slq.BaseInserts;

public static class BaseInsert
{
    public const string InstanceId1 = "INSTANCE-1";
    public const string InstanceId2 = "INSTANCE-2";
    
    public static readonly Entry Instance1Item1 = new(InstanceId1, "4435b01c-8f24-4ee4-9f79-f9ba672a5d62", RepositoryEntryState.Committed, new DateTime(2022, 9, 4));
    public static readonly Entry Instance1Item2 = new(InstanceId1, "a7aef543-ad71-4b26-9062-ed1646501f3e", RepositoryEntryState.Committed, new DateTime(2022, 9, 4));
    public static readonly Entry Instance1Item3 = new(InstanceId1, "912562e1-2685-427a-8fca-3a23f9fc5fd1", RepositoryEntryState.Committed, new DateTime(2022, 9, 4));

    public static readonly Entry Instance2Item1 = new(InstanceId2, "01cbac19-4070-4694-b338-705ae78dad8f", RepositoryEntryState.Committed, new DateTime(2022, 9, 4));
    public static readonly Entry Instance2Item2 = new(InstanceId2, "f025cbe8-bfa9-4c1a-b3e1-ec357eb4de3d", RepositoryEntryState.Committed, new DateTime(2022, 9, 4));
    public static readonly Entry Instance2Item3 = new(InstanceId2, "b22377cb-1b06-4b99-b474-9768b6bee993", RepositoryEntryState.Committed, new DateTime(2022, 9, 4));

    public static readonly IEnumerable<Entry> Entries = new ReadOnlyCollection<Entry>(new List<Entry> {Instance1Item1, Instance1Item2, Instance1Item3, Instance2Item1, Instance2Item2, Instance2Item3});
}