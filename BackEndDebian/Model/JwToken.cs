using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndDebian.Model
{
    public class JwToken
    {
        public string? userName { get; set; }
        public string? access_token { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
    }
}
