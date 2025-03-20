using System;
using System.Collections.Generic;

namespace BackEndDebian.Model;

public partial class Barcode
{
    public int Barcodeid { get; set; }

    public int? Deviceid { get; set; }

    public string Barcodevalue { get; set; } = null!;

    public DateTime? Createdat { get; set; }

    public virtual Device? Device { get; set; }
}
