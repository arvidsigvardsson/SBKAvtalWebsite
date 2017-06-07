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
        var row = new TableRow();
        var cell1 = new TableCell();
        cell1.Text = "en cell";
        var cell2 = new TableCell();
        cell2.Text = "en till";
        row.Cells.Add(cell1);
        row.Cells.Add(cell2);
        avtalstabell.Rows.Add(row);
        
        
        var lst = new List<Avtalsmodel>();

        string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
        using (var conn = new NpgsqlConnection(connstr))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "select id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt avtal, internt_alias, kommentar from sbkavtal.avtal";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lst.Add(new Avtalsmodel
                        {
                            id = reader.GetInt32(0),
                            diarienummer = reader.GetInt64(1),
                            startdate = reader.GetDateTime(2),
                            enddate = reader.GetDateTime(3),
                            status = reader.GetString(4),
                            motpartstyp = reader.GetString(5),
                            sbkid = reader.GetInt32(6),
                            scan_url = reader.GetString(7),
                            orgnummer = reader.GetString(8),
                            enligtAvtal = reader.GetString(9),

                        });
                    }
                }
            }
        }
    }

    public class Avtalsmodel
    {
        public long id { get; set; }
        public long diarienummer { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string orgnummer { get; set; }
        public string enligtAvtal { get; set; }
        public string interntAlias { get; set; }
        public string motpartstyp { get; set; }
        public string status { get; set; }
        public int sbkid { get; set; }
        public string scan_url { get; set; }

    }
}