using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BackEndDebian.Model;

public partial class Device
{
    [JsonPropertyName("deviceid")]
    public int Deviceid { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    [JsonPropertyName("categoriid")]
    public int? Categoryid { get; set; }
    [JsonPropertyName("manufacturerid")]
    public int? Manufacturerid { get; set; }
    [JsonPropertyName("locationid")]
    public int? Locationid { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("createdat")]
    public DateTime? Createdat { get; set; }
    [JsonIgnore]
    public virtual ICollection<Barcode> Barcodes { get; set; } = new List<Barcode>();
    [JsonIgnore]
    public virtual Category? Category { get; set; }
    //[JsonPropertyName("inventorynumbers")]
    [JsonIgnore]
    public virtual ICollection<Inventorynumber> Inventorynumbers { get; set; } = new List<Inventorynumber>();
    [JsonIgnore]
    public virtual Location? Location { get; set; }
    [JsonIgnore]
    public virtual Manufacturer? Manufacturer { get; set; }
}
