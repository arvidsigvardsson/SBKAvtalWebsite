﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Npgsql;


public partial class avtal_detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Page.IsPostBack)
        {
            // kolla om det är ett nytt avtal eller uppdaterat avtal, hur ska detta göras?

            PostbackNewAvtal();           
        }

        var test = new Avtalsmodel();
        //var id = Request.Params["id"];
        //idlabel.Text = id;
        //diarietb.Text = "endast läsning";
        //statusdd.SelectedIndex = 1;

        if (Request.Params["nytt_avtal".ToLower()] == "true")
        {
            debugl.Text = "nytt avtal";
        }
        else
        {
            var dbid = Request.Params["id"];

            string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
            using (var conn = new NpgsqlConnection(connstr))
            {
                conn.Open();

                var sqlquery = "select id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar from sbk_avtal.avtal where id = @p1;";
                using (var cmd = new NpgsqlCommand(sqlquery, conn))
                {
                    // cmd.Connection = conn;
                    // cmd.CommandText = "select id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar from sbkavtal.avtal";
                    cmd.Parameters.AddWithValue("p1", dbid);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                        }
                    }
                }
            }
        }
    }

    private void PostbackNewAvtal()
    {
        debugl.Text = diarietb.Text;
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        submitbtn.Text = "Skickat!";
        submitbtn.CssClass = "btn btn-success";
        // debugl.Text = diarietb.Text;
    }
}