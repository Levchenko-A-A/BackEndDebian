using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackEndDebian.Model
{
    class NewPerson
    {
        [JsonPropertyName("personid")]
        public int Personid { get; set; }
        [JsonPropertyName("personname")]
        public string Personname { get; set; } = null!;
        [JsonPropertyName("passwordhash")]
        public string Passwordhash { get; set; } = null!;
        [JsonPropertyName("salt")]
        public string Salt { get; set; } = null!;
        [JsonPropertyName("createdat")]
        public DateTime? Createdat { get; set; }
        [JsonPropertyName("isadmin")]
        public bool IsAdmin { get; set; }
        [JsonPropertyName("ismanager")]
        public bool IsManager { get; set; }
        [JsonPropertyName("isuser")]
        public bool IsUser { get; set; }
        [JsonPropertyName("isguest")]
        public bool IsGuest { get; set; }
        //[JsonIgnore]
        //public virtual ICollection<Personrole> Personroles { get; set; } = new List<Personrole>();
    }
}
