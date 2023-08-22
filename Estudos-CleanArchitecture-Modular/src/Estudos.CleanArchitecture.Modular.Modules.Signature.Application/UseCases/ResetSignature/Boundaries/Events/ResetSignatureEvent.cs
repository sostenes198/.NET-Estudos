using Estudos.CleanArchitecture.Modular.Commons.Events;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.ResetSignature.Boundaries.Events;

public abstract class ResetSignatureEvent : BaseEvent
{
    protected ResetSignatureEvent(long document, string password, Guid guidPassword)
    {
        Document = document;
        Password = password;
        GuidPassword = guidPassword;
    }

    public long Document { get; }

    public string Password { get; }

    public Guid GuidPassword { get; }
}