using Estudos.CleanArchitecture.Modular.Commons.Domain;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Domain.Signatures;

public record SignaturePassword(string Password, Guid GuidPassword) : ValueObject
{
    public bool IsEmpty { get; }

    private SignaturePassword()
        : this(string.Empty, Guid.Empty)
    {
        IsEmpty = true;
    }

    public static SignaturePassword Empty => new();
}