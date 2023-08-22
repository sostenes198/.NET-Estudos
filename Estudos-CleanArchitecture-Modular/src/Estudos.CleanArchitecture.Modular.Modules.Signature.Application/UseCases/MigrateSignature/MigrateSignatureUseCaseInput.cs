using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.MigrateSignature;

public sealed class MigrateSignatureUseCaseInput : UseCaseInput
{
    public long Document { get; }

    public string Password { get; }

    public Guid GuidPassword { get; }
    
    public MigrateSignatureUseCaseInput(long document, string password, Guid guidPassword)
    {
        Document = document;
        Password = password;
        GuidPassword = guidPassword;
    }
}