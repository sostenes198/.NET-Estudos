using Estudos.CleanArchitecture.Modular.Commons.Events;
using Estudos.CleanArchitecture.Modular.Commons.Events.Internal;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.IntegratedEvents;

public sealed class MigrateSignatureInternalEvent : BaseEvent, IEventInternal
{
    public long Document { get; }

    public string Password { get; }

    public Guid GuidPassword { get; }
    
    public MigrateSignatureInternalEvent(long document, string password, Guid guidPassword)
    {
        Document = document;
        Password = password;
        GuidPassword = guidPassword;
    }
}