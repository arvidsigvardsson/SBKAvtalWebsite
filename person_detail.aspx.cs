﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using System.Configuration;

public partial class person_detail : System.Web.UI.Page
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
            var personinsertquery = "insert into sbk_avtal.person(first_name, last_name, belagenhetsadress, postnummer, postort, tfn_nummer, epost) values(@first_name, @last_name, @belagenhetsadress, @postnummer, @postort, @tfn_nummer, @epost);";   //"select id, first_name, last_name from sbk_avtal.person order by last_name asc;";
            using (var cmd = new NpgsqlCommand(personinsertquery, conn))
            {
                cmd.Parameters.Add(new NpgsqlParameter("first_name", firstnametb.Text));
                cmd.Parameters.Add(new NpgsqlParameter("last_name", lastnametb.Text));
                cmd.Parameters.Add(new NpgsqlParameter("belagenhetsadress", belagentb.Text));
                cmd.Parameters.Add(new NpgsqlParameter("postnummer", postnummertb.Text));
                cmd.Parameters.Add(new NpgsqlParameter("postort", postorttb.Text));
                cmd.Parameters.Add(new NpgsqlParameter("tfn_nummer", tfntb.Text));
                cmd.Parameters.Add(new NpgsqlParameter("epost", eposttb.Text));

                cmd.ExecuteNonQuery();
            }
        }

        // ändra text på knappen
        submitbtn.Text = "Sparat";
    }
}