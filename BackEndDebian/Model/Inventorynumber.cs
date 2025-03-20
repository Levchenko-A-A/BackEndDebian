using System;
using System.Collections.Generic;

namespace BackEndDebian.Model;

public partial class Inventorynumber
{
    public int Inventorynumberid { get; set; }

    public int? Deviceid { get; set; }

    public string Number { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public virtual Device? Device { get; set; }
}
