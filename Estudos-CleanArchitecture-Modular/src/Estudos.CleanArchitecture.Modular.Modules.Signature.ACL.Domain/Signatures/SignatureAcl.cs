using Estudos.CleanArchitecture.Modular.Commons.Domain;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Domain.Signatures;

public class SignatureAcl : Entity, IAggregateRoot
{
    public bool IsEmpty { get; }

    public long Document { get; }

    public string Account { get; }

    public SignaturePassword Password { get; }

    public bool IsMigrated { get; }

    private SignatureAcl()
        : this(0, string.Empty, SignaturePassword.Empty, false)
    {
        IsEmpty = true;
    }

    public SignatureAcl(long document, string account, SignaturePassword password, bool isMigrated)
    {
        Document = document;
        Account = account;
        Password = password;
        IsMigrated = isMigrated;
    }

    public static SignatureAcl Empty => new();

    public bool CanMigrate() => !IsEmpty && !IsMigrated && Document != default;
}