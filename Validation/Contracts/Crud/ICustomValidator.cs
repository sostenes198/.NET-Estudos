namespace Validation.Contracts.Crud
{
    public interface ICustomValidator
    {
        IValidate Validate{ get; set; }
    }
}
