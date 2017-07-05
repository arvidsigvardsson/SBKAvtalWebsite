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
            avtalstecknaredd.Items.Add(string.Format("{0} {1}", person.FirstName, person.LastName));
            kontaktdd.Items.Add(string.Format("{0} {1}", person.FirstName, person.LastName));
            upphandlatdd.Items.Add(string.Format("{0} {1}", person.FirstName, person.LastName));
            ansvarig_sbkdd.Items.Add(string.Format("{0} {1}", person.FirstName, person.LastName));
            datakontaktdd.Items.Add(string.Format("{0} {1}", person.FirstName, person.LastName));
        }

        // lägger till val för ny person
        avtalstecknaredd.Items.Add("+ Ny avtalstecknare");
        kontaktdd.Items.Add("+ Ny avtalskontakt");
        upphandlatdd.Items.Add("+ Ny person");
        ansvarig_sbkdd.Items.Add("+ Ny person");
        datakontaktdd.Items.Add("+ Ny person");

        // lägger till tom rad
        avtalstecknaredd.Items.Add("");
        kontaktdd.Items.Add("");
        upphandlatdd.Items.Add("");
        ansvarig_sbkdd.Items.Add("");
        datakontaktdd.Items.Add("");

        if (Request.Params["nytt_avtal".ToLower()] == "true")
        {
            // debugl.Text = "nytt avtal";
            submitbtn.Text = "Lägg till nytt avtal";
            avtalstecknaredd.Items.FindByText("").Selected = true;
            kontaktdd.Items.FindByText("").Selected = true;
            upphandlatdd.Items.FindByText("").Selected = true;
            ansvarig_sbkdd.Items.FindByText("").Selected = true;
            datakontaktdd.Items.FindByText("").Selected = true;
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
                var sqlquery = "select id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar,  avtalstecknare, avtalskontakt, ansvarig_sbk, ansvarig_avd, ansvarig_enhet, upphandlat_av, datakontakt from sbk_avtal.avtal where id = @p1;";
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

        //dropdowns
        Person avtalstecknare = persons.Where(x => x.id == avtal.avtalstecknare).FirstOrDefault();
        if (avtalstecknare != null)
        {
            avtalstecknaredd.SelectedIndex = (int)avtalstecknare.dropdownindex;
        }
        else
        {
            avtalstecknaredd.Items.FindByValue("").Selected = true;
        }

        Person avtalskontakt = persons.Where(x => x.id == avtal.avtalskontakt).FirstOrDefault();
        if (avtalskontakt != null)
        {
            kontaktdd.SelectedIndex = (int)avtalskontakt.dropdownindex;
        }
        else
        {
            kontaktdd.Items.FindByValue("").Selected = true;
        }

        Person ansvarig_sbk = persons.Where(x => x.id == avtal.ansvarig_sbk).FirstOrDefault();
        if (ansvarig_sbk != null)
        {
            ansvarig_sbkdd.SelectedIndex = (int)ansvarig_sbk.dropdownindex;
        }
        else
        {
            ansvarig_sbkdd.Items.FindByValue("").Selected = true;
        }

        Person upphandlat_av = persons.Where(x => x.id == avtal.upphandlat_av).FirstOrDefault();
        if (upphandlat_av != null)
        {
            upphandlatdd.SelectedIndex = (int)upphandlat_av.dropdownindex;
        }
        else
        {
            upphandlatdd.Items.FindByValue("").Selected = true;
        }

        Person datakontakt = persons.Where(x => x.id == avtal.datakontakt).FirstOrDefault();
        if (datakontakt != null)
        {
            datakontaktdd.SelectedIndex = (int)datakontakt.dropdownindex;
        }
        else
        {
            datakontaktdd.Items.FindByValue("").Selected = true;
        }


        debugl.Text = "";

    }

    private void PostbackUpdateAvtal()
    {
        //var idx = avtalstecknaredd.SelectedIndex;
        //var persons = (List<Person>)Session["persons"];
        //var person = persons.Where(x => x.dropdownindex == idx).First();
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
        if (avtalstecknaredd.SelectedValue == "+ Ny avtalstecknare")
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