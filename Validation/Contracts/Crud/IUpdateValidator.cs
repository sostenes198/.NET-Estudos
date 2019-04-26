namespace Validation.Contracts.Crud
{
    public interface IUpdateValidator
    {
        IValidate Validate { get; set; }
    }
}
