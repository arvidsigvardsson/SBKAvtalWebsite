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
        // debugl.Text = Page.Header.Description;

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

            // Response.Redirect("./sparat_avtal.aspx");
            return;
        }

        var test = new Avtalsmodel();
        //var id = Request.Params["id"];
        //idlabel.Text = id;
        //diarietb.Text = "endast läsning";
        //statusdd.SelectedIndex = 1;

        var avtal = new Avtalsmodel();
        var persons = new List<Person>();
        
        // ta fram personer till rullister
        string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
        using (var conn = new NpgsqlConnection(connstr))
        {
            conn.Open();
            var personquery = "select id, first_name, last_name from sbk_avtal.person order by last_name asc;";
            using (var cmd = new NpgsqlCommand(personquery, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    persons = Avtalsfactory.GetNamesAndId(reader);
                }
            }
        }

        // sparar i sessionen
        Session.Add("persons", persons);

        for (int i = 0; i < persons.Count; i++)
        {
            var person = persons[i];
            person.dropdownindex = i;
            persondd.Items.Add(string.Format("{0} {1}", person.FirstName, person.LastName));
            kontaktdd.Items.Add(string.Format("{0} {1}", person.FirstName, person.LastName));
            upphandlatdd.Items.Add(string.Format("{0} {1}", person.FirstName, person.LastName));
        }

        // lägger till val för ny person
        persondd.Items.Add("+ Ny avtalstecknare");
        kontaktdd.Items.Add("+ Ny avtalskontakt");
        upphandlatdd.Items.Add("+ Ny person");

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

            // string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
            using (var conn = new NpgsqlConnection(connstr))
            {
                conn.Open();

                // avtal
                var sqlquery = "select id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar,  avtalstecknare, avtalskontakt, ansvarig_sbk, ansvarig_avd, ansvarig_enhet from sbk_avtal.avtal where id = @p1;";
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
                //var personquery = "select id, first_name, last_name from sbk_avtal.person;";
                //using (var cmd = new NpgsqlCommand(personquery, conn))
                //{
                //    using (var reader = cmd.ExecuteReader())
                //    {
                //        persons = Avtalsfactory.GetNamesAndId(reader);
                //    }
                //}
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

        debugl.Text = "";

    }

    private void PostbackUpdateAvtal()
    {
        var idx = persondd.SelectedIndex;
        var persons = (List<Person>)Session["persons"];
        var person = persons.Where(x => x.dropdownindex == idx).First();
        // debugl.Text = person.LastName;
    }

    private void PostbackNewAvtal()
    {
        // debugl.Text = "sparar nytt avtal";
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        submitbtn.Text = "Sparat";
        submitbtn.CssClass = "btn btn-success";
        // debugl.Text = diarietb.Text;
    }

    protected void persondd_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (persondd.SelectedValue == "+ Ny avtalstecknare")
        {
            Response.Redirect("./person_detail.aspx?ny_perspon=true");
        }

        if (submitbtn.Text == "Sparat")
        {
            submitbtn.Text = "Uppdatera";
            submitbtn.Enabled = true;
        }
    }
    protected void kontaktdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (kontaktdd.SelectedValue == "+ Ny avtalstecknare")
        {
            Response.Redirect("./person_detail.aspx?ny_perspon=true");
        }

        if (submitbtn.Text == "Sparat")
        {
            submitbtn.Text = "Uppdatera";
            submitbtn.Enabled = true;
        }
    }
    protected void upphandlatdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (upphandlatdd.SelectedValue == "+ Ny avtalstecknare")
        {
            Response.Redirect("./person_detail.aspx?ny_perspon=true");
        }

        if (submitbtn.Text == "Sparat")
        {
            submitbtn.Text = "Uppdatera";
            submitbtn.Enabled = true;
        }
    }
}