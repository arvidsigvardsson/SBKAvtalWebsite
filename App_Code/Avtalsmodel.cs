using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Avtalsmodel
/// </summary>
public class Avtalsmodel
{
    public long id { get; set; }
    public string diarienummer { get; set; }
    public DateTime? startdate { get; set; }
    public DateTime? enddate { get; set; }
    public string orgnummer { get; set; }
    public string enligtAvtal { get; set; }
    public string interntAlias { get; set; }
    public string motpartstyp { get; set; }
    public string status { get; set; }
    public int sbkid { get; set; }
    public string scan_url { get; set; }
    public string kommentar { get; set; }
    public long? avtalstecknare { get; set; }
    public long? avtalskontakt { get; set; }
    public long? upphandlat_av { get; set; }
    public string ansvarig_avdelning { get; set; }
    public string ansvarig_enhet { get; set; }
    public long? ansvarig_sbk { get; set; }
    public long? datakontakt { get; set; }
}