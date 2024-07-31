using System.Text.Json.Serialization;

namespace Sameposty.Services.FacebookPixel;
public class FacebookPixelPayload
{
    [JsonPropertyName("data")]
    public List<EventData> Data { get; set; }

    [JsonPropertyName("test_event_code")]
    public string TestEventCode { get; set; } = "TEST8369";
}

public class EventData
{
    [JsonPropertyName("event_name")]
    public string EventName { get; set; }

    [JsonPropertyName("event_time")]
    public long EventTime { get; set; }

    [JsonPropertyName("action_source")]
    public string ActionSource { get; set; }

    [JsonPropertyName("user_data")]
    public UserData UserData { get; set; }

    [JsonPropertyName("custom_data")]
    public CustomData CustomData { get; set; }
}

public class UserData
{
    [JsonPropertyName("em")]
    public List<string> EmailHashes { get; set; }
}

public class CustomData
{
    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}
