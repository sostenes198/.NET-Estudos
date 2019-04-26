namespace Validation.Contracts.Crud
{
    public interface ICrudValidor
    {
        ICreateValidator CreateValidator { get; set; }

        IUpdateValidator UpdateValidator { get; set; }

        IDeleteValidator DeleteValidator{ get; set; }

        ICustomValidator CustomValidato{ get; set; }
    }
}
