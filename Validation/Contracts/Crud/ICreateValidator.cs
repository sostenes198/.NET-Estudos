namespace Validation.Contracts.Crud
{
    public interface ICreateValidator
    {
        IValidate Validate { get; set; }
    }
}
