using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Avtalsmodel
/// </summary>
public class Avtalsmodel
{
    public int id { get; set; }
    public long diarienummer { get; set; }
    public DateTime startdate { get; set; }
    public DateTime enddate { get; set; }
    public string orgnummer { get; set; }
    public string enligtAvtal { get; set; }

}