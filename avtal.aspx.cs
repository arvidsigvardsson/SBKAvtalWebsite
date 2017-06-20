using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Npgsql;

public partial class avtal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        var lst = new List<Avtalsmodel>();

        string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
        using (var conn = new NpgsqlConnection(connstr))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "select id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar from sbk_avtal.avtal;";
                //cmd.CommandText = "select @fields from sbkavtal.avtal";
                //cmd.Parameters.AddWithValue("fields", Avtalsfactory.Fields);
                
                using (var reader = cmd.ExecuteReader())
                {
                    lst = Avtalsfactory.ParseAvtal(reader);
                    
                }
            }
        }

        Session.Add("avtalslista", lst);

        

        //var row = new TableRow();
        //var cell1 = new TableCell();
        //cell1.Text = "en cell";
        //var cell2 = new TableCell();
        //cell2.Text = "en till";
        //row.Cells.Add(cell1);
        //row.Cells.Add(cell2);
        //avtalstabell.Rows.Add(row);

        foreach (var item in lst)
        {
            var row = new TableRow();

            // lite fix...
            string alink;
            if (item.scan_url != "")
            {
                alink = string.Format("<a href=\"{0}\">Länk till scannat avtal</a>", new Uri(item.scan_url).AbsoluteUri);
            }
            else
            {
                alink = "";
            }

            // redigeringslänken
            var editlink = new HyperLink();
            editlink.NavigateUrl = String.Format("./avtal_detail.aspx?id={0}&nytt_avtal=false", item.id);
            editlink.Text = "Redigera";
            var editc = new TableCell();
            editc.Controls.Add(editlink);

            // var editc = new TableCell() { Text = string.Format("<a href=\"./avtal_detail.aspx?id={0}>Redigera</a>", item.id) };

            var dc = new TableCell() { Text = item.diarienummer.ToString() };
            var sdc = new TableCell() { Text = string.Format("{0:d}", item.startdate) };
            sdc.CssClass = "text-nowrap";
            var edc = new TableCell() { Text = string.Format("{0:d}", item.enddate) };
            var sc = new TableCell() { Text = item.status };
            var mpc = new TableCell() { Text = item.motpartstyp };
            var idc = new TableCell() { Text = item.sbkid.ToString() };
            var urlc = new TableCell() { Text = alink };
            var orgc = new TableCell() { Text = item.orgnummer };
            var enlavt = new TableCell() { Text = item.enligtAvtal };
            var alc = new TableCell() { Text = item.interntAlias };
            var kommc = new TableCell() { Text = item.kommentar };

            row.Cells.Add(editc);
            row.Cells.Add(dc);
            row.Cells.Add(sdc);
            row.Cells.Add(edc);
            row.Cells.Add(sc);
            row.Cells.Add(mpc);
            row.Cells.Add(idc);
            row.Cells.Add(urlc);
            row.Cells.Add(orgc);
            row.Cells.Add(enlavt);
            row.Cells.Add(alc);
            row.Cells.Add(kommc);

            avtalstabell.Rows.Add(row);
        }
        

    }

    //public class Avtalsmodel
    //{
    //    public long id { get; set; }
    //    public long? diarienummer { get; set; }
    //    public DateTime? startdate { get; set; }
    //    public DateTime? enddate { get; set; }
    //    public string orgnummer { get; set; }
    //    public string enligtAvtal { get; set; }
    //    public string interntAlias { get; set; }
    //    public string motpartstyp { get; set; }
    //    public string status { get; set; }
    //    public int sbkid { get; set; }
    //    public string scan_url { get; set; }
    //    public string kommentar { get; set; }
    //}
   
}