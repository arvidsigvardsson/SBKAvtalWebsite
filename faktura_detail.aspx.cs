using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Npgsql;

public partial class faktura_detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void submitbtn_Click(object sender, EventArgs e)
    {
        // stäng av knappen
        submitbtn.Enabled = false;

        string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
        using (var conn = new NpgsqlConnection(connstr))
        {
            conn.Open();
            var fakturainsertquery = "insert into sbk_avtal.fakturaadress(first_name, last_name, belagenhetsadress, postnummer, postort) values(@first_name, @last_name, @belagenhetsadress, @postnummer, @postort);";   //"select id, first_name, last_name from sbk_avtal.person order by last_name asc;";
            using (var cmd = new NpgsqlCommand(fakturainsertquery, conn))
            {
                cmd.Parameters.Add(new NpgsqlParameter("first_name", firstnametb.Text));
                cmd.Parameters.Add(new NpgsqlParameter("last_name", lastnametb.Text));
                cmd.Parameters.Add(new NpgsqlParameter("belagenhetsadress", belagentb.Text));
                cmd.Parameters.Add(new NpgsqlParameter("postnummer", postnummertb.Text));
                cmd.Parameters.Add(new NpgsqlParameter("postort", postorttb.Text));
                                
                cmd.ExecuteNonQuery();
            }
        }

        // ändra text på knappen
        submitbtn.Text = "Sparat";
    }
}