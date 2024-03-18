using System.Text.Json.Serialization;

namespace Sameposty.Services.PostsPublishers.FacebookPublisher.Models;
public class FacebookPostPublishErrorResponse
{
    [JsonPropertyName("error")]
    public ErrorDetails Error { get; set; }
}

public class ErrorDetails
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("error_subcode")]
    public int ErrorSubcode { get; set; }

    [JsonPropertyName("is_transient")]
    public bool IsTransient { get; set; }

    [JsonPropertyName("error_user_title")]
    public string ErrorUserTitle { get; set; }

    [JsonPropertyName("error_user_msg")]
    public string ErrorUserMessage { get; set; }

    [JsonPropertyName("fbtrace_id")]
    public string FbTraceId { get; set; }
}
