using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BackEndDebian.Model;

public partial class Person
{
    [JsonPropertyName("personid")]
    public int Personid { get; set; }
    [JsonPropertyName("personname")]
    public string Personname { get; set; } = null!;
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;
    [JsonPropertyName("passwordhash")]
    public string Passwordhash { get; set; } = null!;
    [JsonPropertyName("salt")]
    public string Salt { get; set; } = null!;
    [JsonPropertyName("createdat")]
    public DateTime? Createdat { get; set; }
    [JsonIgnore]
    public virtual ICollection<Personrole> Personroles { get; set; } = new List<Personrole>();
}
