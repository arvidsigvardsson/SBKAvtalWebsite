using System;
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
        debugl.Text = Page.Header.Description;

        if (Page.IsPostBack)
        {
            // kolla om det är ett nytt avtal eller uppdaterat avtal, hur ska detta göras?
            // Page.MetaDescription = "postback";
            if (submitbtn.Text == "Lägg till nytt avtal")
            {
                PostbackNewAvtal();
            }
            else
            {
                PostbackUpdateAvtal();
            }

            Response.Redirect("./sparat_avtal.aspx");
            return;
        }

        var test = new Avtalsmodel();
        //var id = Request.Params["id"];
        //idlabel.Text = id;
        //diarietb.Text = "endast läsning";
        //statusdd.SelectedIndex = 1;

        var avtal = new Avtalsmodel();
        var persons = new List<Person>();
        
        if (Request.Params["nytt_avtal".ToLower()] == "true")
        {
            // debugl.Text = "nytt avtal";
            submitbtn.Text = "Lägg till nytt avtal";
            return;
        }
        else
        {
            submitbtn.Text = "Uppdatera avtal";

            var dbid = Request.Params["id"];

            string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
            using (var conn = new NpgsqlConnection(connstr))
            {
                conn.Open();

                // avtal
                var sqlquery = "select id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar from sbk_avtal.avtal where id = @p1;";
                using (var cmd = new NpgsqlCommand(sqlquery, conn))
                {
                    // cmd.Connection = conn;
                    // cmd.CommandText = "select id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar from sbkavtal.avtal";
                    cmd.Parameters.AddWithValue("p1", dbid);

                    using (var reader = cmd.ExecuteReader())
                    {
                        avtal = Avtalsfactory.ParseAvtal(reader).First();
                    }
                }

                // personer
                var personquery = "select id, first_name, last_name from sbk_avtal.person;";
                using (var cmd = new NpgsqlCommand(personquery, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        persons = Avtalsfactory.GetNamesAndId(reader);
                    }
                }
            }
        }

        diarietb.Text = avtal.diarienummer;
        startdatetb.Text = string.Format("{0:d}", avtal.startdate);
        enddate.Text = string.Format("{0:d}", avtal.enddate);

        statusdd.Items.FindByValue(avtal.status).Selected = true;
        motpartsdd.Items.FindByValue(avtal.motpartstyp).Selected = true;

        sbkidtb.Text = avtal.sbkid.ToString();
        orgnrtb.Text = avtal.orgnummer;
        enlavttb.Text = avtal.enligtAvtal;
        intidtb.Text = avtal.interntAlias;
        kommentartb.Text = avtal.kommentar;

        Session.Add("persons", persons);

        // TODO slutade här onsdag 14/6
        // for

    }

    private void PostbackUpdateAvtal()
    {
        debugl.Text = "uppdaterar avtal";
    }

    private void PostbackNewAvtal()
    {
        debugl.Text = "sparar nytt avtal";
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        submitbtn.Text = "Sparat";
        submitbtn.CssClass = "btn btn-success";
        // debugl.Text = diarietb.Text;
    }
}