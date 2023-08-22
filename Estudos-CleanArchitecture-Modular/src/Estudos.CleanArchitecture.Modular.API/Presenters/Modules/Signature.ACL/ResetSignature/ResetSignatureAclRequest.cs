using System.Text.Json.Serialization;

namespace Estudos.CleanArchitecture.Modular.API.Presenters.Modules.Signature.ACL.ResetSignature;

public class ResetSignatureAclRequest
{
    [JsonPropertyName("document")]
    public long Document { get; set; }

    [JsonPropertyName("account")]
    public string Account { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("guidPassword")]
    public Guid GuidPassword { get; set; }
}