using System.Text.Json.Serialization;
using Tech2023.Core.Json.Converters;

#nullable disable

namespace Tech2023.Web.Shared.Statistics;

/// <summary>
/// A ping response object, to indicate the server is connected
/// </summary>
public class PingResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PingResponse"/> class
    /// </summary>
    /// <param name="s">The timespan duration</param>
    /// <param name="ipAddress">The internet protocol address of the remote host that called it</param>
    public PingResponse(TimeSpan s, string ipAddress)
    {
        Runtime = s;
        Ip = ipAddress;
    }

    /// <summary>
    /// The runtime of the server
    /// </summary>
    [JsonConverter(typeof(TimeSpanConverter))]
    [JsonPropertyName("runtime")]
    public TimeSpan Runtime { get; set; }

    /// <summary>
    /// The internet protocol address of the remote host, this can be IPv4 or v6
    /// </summary>
    [JsonPropertyName("ip")]
    public string Ip { get; set; }
}
