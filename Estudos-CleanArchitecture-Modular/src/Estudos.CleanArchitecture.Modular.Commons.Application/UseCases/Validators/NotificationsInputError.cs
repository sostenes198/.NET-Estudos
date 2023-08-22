namespace Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.Validators;

public readonly struct NotificationsInputError
{
    private readonly IDictionary<string, IList<string>> _errorMessages = new Dictionary<string, IList<string>>();

    public NotificationsInputError()
    {
    }

    public IDictionary<string, string[]> Errors => _errorMessages.ToDictionary(item => item.Key, item => item.Value.ToArray());

    public void Add(string key, string message)
    {
        if (!_errorMessages.ContainsKey(key))
        {
            _errorMessages[key] = new List<string>();
        }

        _errorMessages[key].Add(message);
    }

    public static NotificationsInputError Empty => new();
}