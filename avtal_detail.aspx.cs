using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Npgsql;
using System.Text.RegularExpressions;
using System.Text;


public partial class avtal_detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // debugl.Text = Page.Header.Description;

        if (Page.IsPostBack)
        {
            return;
        }

        var test = new Avtalsmodel();
        //var id = Request.Params["id"];
        //idlabel.Text = id;
        //diarietb.Text = "endast läsning";
        //statusdd.SelectedIndex = 1;

        var avtal = new Avtalsmodel();
        var persons = new List<Person>();
        var innehallslist = new List<Innehall>();
        var leveranslist = new List<DateTime>();

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

            // fyll lista med avtalsinnehåll
            using (var cmd = new NpgsqlCommand("select id, beskrivning from sbk_avtal.avtalsinnehall;", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        innehallslist.Add(new Innehall
                        {
                            id = reader.GetInt32(0),
                            beskrivning = reader.GetString(1)
                        });
                    }
                }
            }
        }

        // sparar i sessionen
        Session.Add("persons", persons);
        Session.Add("innehallslist", innehallslist);

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
        avtalstecknaredd.Items.Add("+ Ny person"); // ("+ Ny avtalstecknare");
        kontaktdd.Items.Add("+ Ny person");
        upphandlatdd.Items.Add("+ Ny person");
        ansvarig_sbkdd.Items.Add("+ Ny person");
        datakontaktdd.Items.Add("+ Ny person");

        // lägger till tom rad
        avtalstecknaredd.Items.Add("");
        kontaktdd.Items.Add("");
        upphandlatdd.Items.Add("");
        ansvarig_sbkdd.Items.Add("");
        datakontaktdd.Items.Add("");

        // avtalsinnehåll
        for (int i = 0; i < innehallslist.Count; i++)
        {
            Innehall inn = innehallslist[i];
            innehallcbl.Items.Add(new ListItem(inn.beskrivning));
            inn.ListIndex = i;
        }

        if (Request.Params["nytt_avtal".ToLower()] == "true")
        {
            // debugl.Text = "nytt avtal";
            //submitbtn.Text = "Lägg till nytt avtal";
            SetSubmitButtonAppearance(SubmitButtonState.SparaNytt);
            avtalstecknaredd.Items.FindByText("").Selected = true;
            kontaktdd.Items.FindByText("").Selected = true;
            upphandlatdd.Items.FindByText("").Selected = true;
            ansvarig_sbkdd.Items.FindByText("").Selected = true;
            datakontaktdd.Items.FindByText("").Selected = true;
            return;
        }
        else
        {
            //submitbtn.Text = "Uppdatera avtal";
            SetSubmitButtonAppearance(SubmitButtonState.Uppdatera);

            var dbid = Request.Params["id"];

            // string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
            using (var conn = new NpgsqlConnection(connstr))
            {
                conn.Open();

                // avtal
                var sqlquery = "select id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar,  avtalstecknare, avtalskontakt, ansvarig_sbk, ansvarig_avd, ansvarig_enhet, upphandlat_av, datakontakt, konto, kstl, vht, mtp, aktivitet, objekt from sbk_avtal.avtal where id = @p1;";
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

                using (var cmd = new NpgsqlCommand("select avtalsinnehall_id from sbk_avtal.map_avtal_innehall where avtal_id = @avtal_id;", conn))
                {
                    cmd.Parameters.AddWithValue("avtal_id", dbid);
                    using (var reader = cmd.ExecuteReader())
                    {
                        // kryssa för avtalsinnehåll
                        while (reader.Read())
                        {
                            var innehalls_id = reader.GetInt32(0);
                            var idx = innehallslist.Where(x => x.id == innehalls_id).First().ListIndex;
                            innehallcbl.Items[idx].Selected = true;
                        }

                    }
                }

                // leveranser
                var levquery = "select datum from sbk_avtal.leveranser where avtal_id=@avtal_id order by datum;";
                using (var cmd = new NpgsqlCommand(levquery, conn))
                {
                    cmd.Parameters.AddWithValue("avtal_id", dbid);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            leveranslist.Add(reader.GetDateTime(0));
                        }
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
        ansvavdtb.Text = avtal.ansvarig_avdelning;
        ansvenhtb.Text = avtal.ansvarig_enhet;

        pathtoavtaltb.Text = avtal.scan_url;

        // ekonomi
        kontotb.Text = avtal.konto;
        kstltb.Text = avtal.kstl;
        vhttb.Text = avtal.vht;
        mtptb.Text = avtal.mtp;
        aktivitettb.Text = avtal.aktivitet;
        objekttb.Text = avtal.objekt;

        // leveranser
        var levsb = new StringBuilder();
        foreach (var date in leveranslist)
        {
            levsb.AppendLine(date.ToShortDateString());  //string.Format("{0:d}", date.ToString()));
        }
        manuellevtb.Text = levsb.ToString();

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
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Uppdaterar')", true);
    }

    private void PostbackNewAvtal()
    {
        // debugl.Text = "sparar nytt avtal";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Sparar nytt')", true);
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        submitbtn.Text = "Sparat";
        submitbtn.CssClass = "btn btn-success";
        // debugl.Text = diarietb.Text;
    }


    private class Innehall
    {
        public int id { get; set; }
        public string beskrivning { get; set; }
        public int ListIndex { get; set; }
    }

    protected void Dropdowns_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (avtalstecknaredd.SelectedValue == "+ Ny person")
        {
            Response.Redirect("./person_detail.aspx?ny_perspon=true");
        }

        if (submitbtn.Text == "Sparat")
        {
            submitbtn.Text = "Uppdatera";
            submitbtn.Enabled = true;
            submitbtn.CssClass = "btn btn-primary";
        }

        // lägger till ett state att det är en rullist som ändrats, så att postback fungerar korrekt
        //Session["change in dropdown"] = true;
        debugl.Text = "dropdown ändrad";
    }

    protected void submitbtn_Click(object sender, EventArgs e)
    {
        // debugl.Text = "submitknapp";
        if (!Page.IsValid)
        {
            return;
        }
        if (submitbtn.Text == "Spara nytt")
        {
            SetSubmitButtonAppearance(SubmitButtonState.Sparat);
            SaveNewAvtal();
        }
        else // if (submitbtn.Text == "Uppdatera")
        {
            SetSubmitButtonAppearance(SubmitButtonState.Uppdaterat);
            UpdateAvtal();
        }

    }

    private void UpdateAvtal()
    {
        debugl.Text = "Update metoden";

        var avtal = GetFormInputs();
        var leveranser = GetLeveransDates();
        var id = int.Parse(Request.Params["id"]);

        string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;

        using (var conn = new NpgsqlConnection(connstr))
        {
            // rensa maptabellen med avtalsinnehåll för givet avtal
            var deletequery = "delete from sbk_avtal.map_avtal_innehall where avtal_id=@id;";
            conn.Open();
            using (var cmd = new NpgsqlCommand(deletequery, conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }

            // rensa leveranstabellen för givet avtal
            var levdeletequery = "delete from sbk_avtal.leveranser where avtal_id=@avtal_id";
            using (var cmd = new NpgsqlCommand(levdeletequery, conn))
            {
                cmd.Parameters.AddWithValue("avtal_id", id);
                cmd.ExecuteNonQuery();
            }
        }
        
        using (var conn = new NpgsqlConnection(connstr))
        {
            conn.Open();
            var query = "update sbk_avtal.avtal set(diarienummer, startdate, enddate, status, motpartstyp, sbkavtalsid, orgnummer, enligt_avtal, internt_alias, kommentar, ansvarig_avd, ansvarig_enhet, avtalstecknare, avtalskontakt, upphandlat_av, ansvarig_sbk, datakontakt, scan_url, konto, kstl, vht, mtp, aktivitet, objekt) = (@diarienummer, @startdate, @enddate, @status, @motpartstyp, @sbkavtalsid, @orgnummer, @enligt_avtal, @internt_alias, @kommentar, @ansvarig_avd, @ansvarig_enhet, @avtalstecknare, @avtalskontakt, @upphandlat_av, @ansvarig_sbk, @datakontakt, @scan_url, @konto, @kstl, @vht, @mtp, @aktivitet, @objekt) where id = @id;";
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("diarienummer", avtal.diarienummer);
                cmd.Parameters.AddWithValue("startdate", avtal.startdate);
                cmd.Parameters.AddWithValue("enddate", avtal.enddate);
                cmd.Parameters.AddWithValue("status", avtal.status);
                cmd.Parameters.AddWithValue("motpartstyp", avtal.motpartstyp);
                cmd.Parameters.AddWithValue("sbkavtalsid", avtal.sbkid);
                cmd.Parameters.AddWithValue("orgnummer", avtal.orgnummer);
                cmd.Parameters.AddWithValue("enligt_avtal", avtal.enligtAvtal);
                cmd.Parameters.AddWithValue("internt_alias", avtal.interntAlias);
                cmd.Parameters.AddWithValue("kommentar", avtal.kommentar);
                cmd.Parameters.AddWithValue("ansvarig_avd", avtal.ansvarig_avdelning);
                cmd.Parameters.AddWithValue("ansvarig_enhet", avtal.ansvarig_enhet);
                cmd.Parameters.AddWithValue("avtalstecknare", avtal.avtalstecknare);
                cmd.Parameters.AddWithValue("avtalskontakt", avtal.avtalskontakt);
                cmd.Parameters.AddWithValue("upphandlat_av", avtal.upphandlat_av);
                cmd.Parameters.AddWithValue("ansvarig_sbk", avtal.ansvarig_sbk);
                cmd.Parameters.AddWithValue("datakontakt", avtal.datakontakt);

                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("scan_url", avtal.scan_url);

                cmd.Parameters.AddWithValue("konto", avtal.konto);
                cmd.Parameters.AddWithValue("kstl", avtal.kstl);
                cmd.Parameters.AddWithValue("vht", avtal.vht);
                cmd.Parameters.AddWithValue("mtp", avtal.mtp);
                cmd.Parameters.AddWithValue("aktivitet", avtal.aktivitet);
                cmd.Parameters.AddWithValue("objekt", avtal.objekt);

                cmd.ExecuteNonQuery();
            }
        

        //using (var conn = new NpgsqlConnection(connstr))
        //{
        //    // rensa maptabellen med avtalsinnehåll för givet avtal
        //    var deletequery = "delete from sbk_avtal.map_avtal_innehall where id=@id;";
        //    using (var cmd = new NpgsqlCommand(deletequery, conn))
        //    {
        //        cmd.Parameters.AddWithValue("id", id);
        //        cmd.ExecuteNonQuery();
        //    }
        //}

            // lägg till nytt avtalsinnehåll
            // lägger till i innehållmaptable
            foreach (var innehallId in GetCheckedInnehall())
            {
                var innehallsquery = "insert into sbk_avtal.map_avtal_innehall(avtal_id, avtalsinnehall_id) values(@avtal_id, @avtalsinnehall_id);";
                using (var cmd = new NpgsqlCommand(innehallsquery, conn))
                {
                    cmd.Parameters.AddWithValue("avtal_id", id);
                    cmd.Parameters.AddWithValue("avtalsinnehall_id", innehallId);

                    cmd.ExecuteNonQuery();
                }
            }

            // lägg till leveranser
            foreach (var date in leveranser)
            {
                var insertlevquery = "insert into sbk_avtal.leveranser(datum, avtal_id) values(@datum, @avtal_id);";
                using (var cmd = new NpgsqlCommand(insertlevquery, conn))
                {
                    cmd.Parameters.AddWithValue("datum", date);
                    cmd.Parameters.AddWithValue("avtal_id", id);
                    cmd.ExecuteNonQuery();
                }
            }
       

        }
    }

    private void SaveNewAvtal()
    {
        debugl.Text = "SaveNew metoden";

        Avtalsmodel avtal = GetFormInputs();
        int avtalId = -1;

        string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
        using (var conn = new NpgsqlConnection(connstr))
        {
            conn.Open();
            var query = "insert into sbk_avtal.avtal(diarienummer, startdate, enddate, status, motpartstyp, sbkavtalsid, orgnummer, enligt_avtal, internt_alias, kommentar, ansvarig_avd, ansvarig_enhet, avtalstecknare, avtalskontakt, upphandlat_av, ansvarig_sbk, datakontakt, scan_url, konto, kstl, vht, mtp, aktivitet, objekt) values(@diarienummer, @startdate, @enddate, @status, @motpartstyp, @sbkavtalsid, @orgnummer, @enligt_avtal, @internt_alias, @kommentar, @ansvarig_avd, @ansvarig_enhet, @avtalstecknare, @avtalskontakt, @upphandlat_av, @ansvarig_sbk, @datakontakt, @scan_url, @konto, @kstl, @vht, @mtp, @aktivitet, @objekt) returning id;";
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("diarienummer", avtal.diarienummer);
                cmd.Parameters.AddWithValue("startdate", avtal.startdate);
                cmd.Parameters.AddWithValue("enddate", avtal.enddate);
                cmd.Parameters.AddWithValue("status", avtal.status);
                cmd.Parameters.AddWithValue("motpartstyp", avtal.motpartstyp);
                cmd.Parameters.AddWithValue("sbkavtalsid", avtal.sbkid);
                cmd.Parameters.AddWithValue("orgnummer", avtal.orgnummer);
                cmd.Parameters.AddWithValue("enligt_avtal", avtal.enligtAvtal);
                cmd.Parameters.AddWithValue("internt_alias", avtal.interntAlias);
                cmd.Parameters.AddWithValue("kommentar", avtal.kommentar);
                cmd.Parameters.AddWithValue("ansvarig_avd", avtal.ansvarig_avdelning);
                cmd.Parameters.AddWithValue("ansvarig_enhet", avtal.ansvarig_enhet);
                cmd.Parameters.AddWithValue("avtalstecknare", (Object)avtal.avtalstecknare ?? DBNull.Value);
                cmd.Parameters.AddWithValue("avtalskontakt", (Object)avtal.avtalskontakt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("upphandlat_av", (Object)avtal.upphandlat_av ?? DBNull.Value);
                cmd.Parameters.AddWithValue("ansvarig_sbk", (Object)avtal.ansvarig_sbk ?? DBNull.Value);
                cmd.Parameters.AddWithValue("datakontakt", (Object)avtal.datakontakt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("scan_url", avtal.scan_url);
                
                cmd.Parameters.AddWithValue("konto", avtal.konto);
                cmd.Parameters.AddWithValue("kstl", avtal.kstl);
                cmd.Parameters.AddWithValue("vht", avtal.vht);
                cmd.Parameters.AddWithValue("mtp", avtal.mtp);
                cmd.Parameters.AddWithValue("aktivitet", avtal.aktivitet);
                cmd.Parameters.AddWithValue("objekt", avtal.objekt);

                //cmd.ExecuteNonQuery();
                var reader = cmd.ExecuteReader();
                
                while(reader.Read())
                {
                    avtalId = reader.GetInt32(0);
                }
            }
        }

        using (var conn = new NpgsqlConnection(connstr))
	    {
            conn.Open();
	

            // lägger till i innehållmaptable
            foreach (var innehallId in GetCheckedInnehall())
            {
                var innehallsquery = "insert into sbk_avtal.map_avtal_innehall(avtal_id, avtalsinnehall_id) values(@avtal_id, @avtalsinnehall_id);";
                using (var cmd = new NpgsqlCommand(innehallsquery, conn))
                {
                    cmd.Parameters.AddWithValue("avtal_id", avtalId);
                    cmd.Parameters.AddWithValue("avtalsinnehall_id", innehallId);

                    cmd.ExecuteNonQuery();
                }
            }
            
        }
    }

    private List<DateTime> GetLeveransDates()
    {
        var lst = manuellevtb.Text.Split('\n');
        DateTime dt;
        return lst.
            Where(x => x != "").
            // Where(x => DateTime.TryParse(x, out dt)).
            Select(x => DateTime.Parse(x)).ToList();

    }

    private List<int> GetCheckedInnehall()
    {
        var lst = (List<Innehall>)Session["innehallslist"];
        return innehallcbl.Items.
            Cast<ListItem>().
            Where(x => x.Selected).
            Select(x => x.Value).
            Select(x => lst.
                Where(y => y.beskrivning == x).
                First()).
            Select(x => x.id).
            ToList();
    }

    private Avtalsmodel GetFormInputs()
    {
        Avtalsmodel avtal = new Avtalsmodel
        {
            diarienummer = diarietb.Text,
            startdate = DateTime.Parse(startdatetb.Text),
            enddate = DateTime.Parse(enddate.Text),
            status = statusdd.SelectedValue,
            motpartstyp = motpartsdd.SelectedValue,
            sbkid = sbkidtb.Text != "" ? int.Parse(sbkidtb.Text) : -1,
            orgnummer = orgnrtb.Text,
            enligtAvtal = enlavttb.Text,
            interntAlias = intidtb.Text,
            kommentar = kommentartb.Text,
            ansvarig_avdelning = ansvavdtb.Text,
            ansvarig_enhet = ansvenhtb.Text,
            scan_url = pathtoavtaltb.Text,
            konto = kontotb.Text,
            kstl = kstltb.Text,
            vht = vhttb.Text,
            mtp = mtptb.Text,
            aktivitet = aktivitettb.Text,
            objekt = objekttb.Text,
        };

        List<Person> personer = (List<Person>)Session["persons"];
        try
        {
            avtal.avtalstecknare = personer.Where(x => avtalstecknaredd.SelectedIndex == x.dropdownindex).FirstOrDefault().id;
        }
        catch (Exception)
        {
            avtal.avtalstecknare = null;
        }
        try
        {
            avtal.avtalskontakt = personer.Where(x => kontaktdd.SelectedIndex == x.dropdownindex).FirstOrDefault().id;
        }
        catch (Exception)
        {
            avtal.avtalskontakt = null;
        }
        try
        {
            avtal.upphandlat_av = personer.Where(x => upphandlatdd.SelectedIndex == x.dropdownindex).FirstOrDefault().id;
        }
        catch (Exception)
        {
            avtal.upphandlat_av = null;
        }
        try
        {
            avtal.ansvarig_sbk = personer.Where(x => ansvarig_sbkdd.SelectedIndex == x.dropdownindex).FirstOrDefault().id;
        }
        catch (Exception)
        {
            avtal.ansvarig_sbk = null;
        }
        try
        {
            avtal.datakontakt = personer.Where(x => datakontaktdd.SelectedIndex == x.dropdownindex).FirstOrDefault().id;
        }
        catch (Exception)
        {
            avtal.datakontakt = null;
        }
       
        return avtal;
    }

    

    private void SetSubmitButtonAppearance(SubmitButtonState state)
    {
        string text;
        bool enabled;
        string cssclass;

        switch (state)
        {
            case SubmitButtonState.SparaNytt:
                text = "Spara nytt";
                enabled = true;
                cssclass = "btn btn-primary";
                break;
            case SubmitButtonState.Sparat:
                 text = "Sparat";
                enabled = false;
                cssclass = "btn btn-success";
                break;
            case SubmitButtonState.Uppdatera:
                 text = "Uppdatera";
                enabled = true;
                cssclass = "btn btn-primary";
                break;
            case SubmitButtonState.Uppdaterat:
                text = "Uppdaterat";
                enabled = false;
                cssclass = "btn btn-success";
                break;
            default:
               text = "Spara nytt";
                enabled = true;
                cssclass = "btn btn-primary";
                break;
        }

        submitbtn.Text = text;
        submitbtn.Enabled = enabled;
        submitbtn.CssClass = cssclass;
    }

    private enum SubmitButtonState { SparaNytt, Sparat, Uppdatera, Uppdaterat }

    protected void OrgNummerValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        debugl2.Text = "Validerar...";
        var regex = new Regex("^[0-9]{6}-[0-9]{4}$");
        var orgnr = args.Value;

        if (regex.IsMatch(orgnr))
        {
            if (ValidOrgNummer(orgnr))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
                OrgNummerValidator.ErrorMessage = "Checksiffra matchar inte";
            }
        }
        else
        {
            args.IsValid = false;
            OrgNummerValidator.ErrorMessage = "Använd formatet XXXXXX-XXXX";
        }
    }

    private bool ValidOrgNummer(string orgnummer)
    {
        var multipliers = new List<int>{ 2, 1, 2, 1, 2, 1, 2, 1, 2, 1 };
        var clean = orgnummer.Replace("-", "");
        var digits = clean.ToCharArray().Select(x => (int)Char.GetNumericValue(x));
        var sum = digits.Zip(multipliers, (a, b) => a * b).Select(x => x / 10 + x % 10).Sum();
        return sum % 10 == 0;
    }

    //private int LuhnChecksum(string orgnummer)
    //{
    //    return 0;
    //}

    //protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (leveransrbl.SelectedIndex == 0)
    //    {
    //        ettdatumdiv.Visible = true;
    //        datumvarjemanaddiv.Visible = false;
    //    }
    //    else
    //    {
    //        ettdatumdiv.Visible = false;
    //        datumvarjemanaddiv.Visible = true;
    //    }
    //}

    //protected void leveranscb_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (leveranscb.Checked == true)
    //    {
    //        leveransdiv.Visible = true;
    //    }
    //    else
    //    {
    //        leveransdiv.Visible = false;
    //    }
    //}

    //protected void nyleveranslb_Click(object sender, EventArgs e)
    //{

    //    var row = new TableRow();
    //    var cell = new TableCell();
    //    var tb = new TextBox();

    //    cell.Controls.Add(tb);
    //    row.Controls.Add(cell);
    //    manuelllevtable.Rows.Add(row);
    //}
}