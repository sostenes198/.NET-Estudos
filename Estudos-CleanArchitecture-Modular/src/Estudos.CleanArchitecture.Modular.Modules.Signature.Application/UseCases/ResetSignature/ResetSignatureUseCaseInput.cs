using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.ResetSignature;

public sealed class ResetSignatureUseCaseInput : UseCaseInput
{
    public long Document { get; }

    public string Password { get; }

    public Guid GuidPassword { get; }
    
    public ResetSignatureUseCaseInput(long document, string password, Guid guidPassword)
    {
        Document = document;
        Password = password;
        GuidPassword = guidPassword;
    }
}