using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Application.UseCases.ResetSignatures;

public sealed class ResetSignatureAclUseCaseInput : UseCaseInput
{
    public long Document { get; }

    public string Account { get; }

    public string Password { get; }

    public Guid GuidPassword { get; }
    
    public ResetSignatureAclUseCaseInput(Guid guidPassword, long document, string account, string password)
    {
        GuidPassword = guidPassword;
        Document = document;
        Account = account;
        Password = password;
    }
}