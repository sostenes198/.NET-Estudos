namespace Estudos.CleanArchitecture.Modular.Commons.Domain;

public record OperationResult
{
    public bool IsSuccess { get; }

    protected OperationResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public static OperationResult Success() => new(true);

    public static OperationResult Fail() => new(false);
}

public record OperationResult<TResult> : OperationResult
{
    public TResult? Result { get; }

    protected OperationResult(TResult? result, bool isSuccess)
        : base(isSuccess)
    {
        Result = result;
    }

    public static OperationResult<TResult> Success(TResult result) => new(result, true);

    public new static OperationResult<TResult> Fail() => new(default, false);
}