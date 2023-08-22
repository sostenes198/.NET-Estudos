using Estudos.CleanArchitecture.Modular.Commons.Domain;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.Domain.Signatures;

public class Signature : Entity, IAggregateRoot
{
    public bool IsEmpty { get; }

    public long Document { get; }

    public SignaturePassword Password { get; }

    private Signature()
        : this(0, SignaturePassword.Empty)
    {
        IsEmpty = true;
    }

    public Signature(long document, SignaturePassword password)
    {
        Document = document;
        Password = password;
    }

    public static Signature Empty => new();
}