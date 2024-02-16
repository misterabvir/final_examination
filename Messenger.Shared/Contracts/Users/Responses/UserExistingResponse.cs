using System.Text.Json.Serialization;

namespace Messenger.Shared.Contracts.Users.Responses;

public class UserExistingResponse
{
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; } = Guid.Empty;
    [JsonPropertyName("is_existing")]
    public bool IsExisting { get; set; }
}

