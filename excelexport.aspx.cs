using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class excelexport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string attachment = "attachment; filename=avtal.csv";
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.AddHeader("content-disposition", attachment);
        HttpContext.Current.Response.ContentType = "text/csv";
        HttpContext.Current.Response.AddHeader("Pragma", "public");

        var lst = (List<Avtalsmodel>)Session["avtalslista"];
        var sb = new StringBuilder();
        sb.AppendLine("status;avtalsid");     // "diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar");

        foreach (var avtal in lst)
        {
            sb.AppendLine(string.Format("{0};{1}", avtal.status, avtal.id));
        }
        HttpContext.Current.Response.Write(sb.ToString());
        HttpContext.Current.Response.End();
    }
}