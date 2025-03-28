using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BackEndDebian.Model;

public partial class Inventorynumber
{
    [JsonPropertyName("inventorynumberid")]
    public int Inventorynumberid { get; set; }
    [JsonPropertyName("deviceid")]
    public int Deviceid { get; set; }
    [JsonPropertyName("number")]
    public string Number { get; set; } = null!;
    [JsonPropertyName("createdat")]
    public DateTime? Createdat { get; set; }
    [JsonIgnore]
    public virtual Device? Device { get; set; }
}
