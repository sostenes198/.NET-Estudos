using Estudos.CleanArchitecture.Modular.Commons.Domain;
using Estudos.CleanArchitecture.Modular.Commons.Events;
using Estudos.CleanArchitecture.Modular.Commons.Events.Internal;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.IntegratedEvents;

public sealed class ResetSignatureNewApiInternalInternalCommandEvent : BaseEvent, ICommandEventInternal<OperationResult>
{
    public ResetSignatureNewApiInternalInternalCommandEvent(long document, string password, Guid guidPassword)
    {
        Document = document;
        Password = password;
        GuidPassword = guidPassword;
    }

    public long Document { get; }

    public string Password { get; }

    public Guid GuidPassword { get; }
}