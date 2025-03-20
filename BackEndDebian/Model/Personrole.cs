using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BackEndDebian.Model;

public partial class Personrole
{
    [JsonPropertyName("personroleid")]
    public int Personroleid { get; set; }
    [JsonPropertyName("personid")]
    public int? Personid { get; set; }
    [JsonPropertyName("roleid")]
    public int? Roleid { get; set; }
    [JsonIgnore]
    public virtual Person? Person { get; set; }
    [JsonIgnore]
    public virtual Role? Role { get; set; }
}
