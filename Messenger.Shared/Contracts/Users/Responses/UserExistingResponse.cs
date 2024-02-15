using System.Text.Json.Serialization;

namespace Messenger.Shared.Contracts.Users.Responses;

public class UserExistingResponse
{
    [JsonPropertyName("isExist")]
    public bool IsExist { get; set; }
}

