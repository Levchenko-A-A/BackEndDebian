using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BackEndDebian.Model;

public partial class Role
{
    [JsonPropertyName("roleid")]
    public int Roleid { get; set; }
    [JsonPropertyName("rolename")]
    public string Rolename { get; set; } = null!;
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonIgnore]
    public virtual ICollection<Personrole> Personroles { get; set; } = new List<Personrole>();
}
