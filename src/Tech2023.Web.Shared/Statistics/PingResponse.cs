using System.Text.Json.Serialization;
using Tech2023.Core.Json.Converters;

#nullable disable

namespace Tech2023.Web.Shared.Statistics;

public class PingResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PingResponse"/> class
    /// </summary>
    /// <param name="s"></param>
    /// <param name="ipAddress"></param>
    public PingResponse(TimeSpan s, string ipAddress)
    {
        Runtime = s;
        Ip = ipAddress;
    }

    [JsonConverter(typeof(TimeSpanConverter))]
    [JsonPropertyName("runtime")]
    public TimeSpan Runtime { get; set; }

  
    [JsonPropertyName("ip")]
    public string Ip { get; set; }
}
