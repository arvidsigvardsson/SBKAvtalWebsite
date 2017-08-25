using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Npgsql;
using System.Data;
using System.Text.RegularExpressions;

public partial class avtalsgrid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            return;
        }

        filterdiv.Visible = false;

        var ds = new DataSet();
        var dt = new DataTable();

        // för filtrering
        var medarbetare = new List<Person>();
        var avdelningar = new List<string>();
        
        string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
        using (var conn = new NpgsqlConnection(connstr))
        {
            conn.Open();
            var sqlquery = "select avtal.id, avtalstyp, enligt_avtal, diarienummer, startdate, enddate, status, motpartstyp, sbk_avtal.ansvarig_avd.namn as ansvarig_avd, avtalstecknare.first_name || ' ' || avtalstecknare.last_name as \"Avtalstecknare\", ansvarig_sbk.first_name || ' ' || ansvarig_sbk.last_name as \"ansvarig_sbk\", avtalskontakt.first_name || ' ' || avtalskontakt.last_name as \"Avtalskontakt\", ansvarigsbk.first_name as \"Ansvarig SBK - Förnamn\", ansvarigsbk.last_name as \"Ansvarig SBK - efternamn\", ansvarigsbk.epost as \"Ansvarig SBK - epost\", data.first_name as \"datakontakt förnamn\", data.last_name as \"datakontakt efternamn\", data.epost as \"datakontakt epost\", kontakt.last_name as \"Avtalskontakt efternamn\", orgnummer, (select string_agg(beskrivning, ', ') from sbk_avtal.avtalsinnehall left join sbk_avtal.map_avtal_innehall on map_avtal_innehall.avtalsinnehall_id = avtalsinnehall.id where avtal.id = map_avtal_innehall.avtal_id) as avtalsinnehall from sbk_avtal.avtal left join sbk_avtal.person data on data.id=sbk_avtal.avtal.datakontakt left join sbk_avtal.person kontakt on kontakt.id=sbk_avtal.avtal.avtalskontakt left join sbk_avtal.person ansvarigsbk on ansvarigsbk.id=sbk_avtal.avtal.ansvarig_sbk left join sbk_avtal.person avtalstecknare on avtalstecknare.id=sbk_avtal.avtal.avtalstecknare left join sbk_avtal.person avtalskontakt on avtalskontakt.id=sbk_avtal.avtal.avtalskontakt left join sbk_avtal.person ansvarig_sbk on ansvarig_sbk.id=sbk_avtal.avtal.ansvarig_sbk left join sbk_avtal.ansvarig_avd on sbk_avtal.ansvarig_avd.id = sbk_avtal.avtal.ansvarig_avd;";
            using (var da = new NpgsqlDataAdapter(sqlquery, conn))
            {
                da.Fill(ds);
            }

            var avdelningsquery = "select namn from sbk_avtal.ansvarig_avd;";
            using (var cmd = new NpgsqlCommand(avdelningsquery, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        avdelningar.Add(reader.GetString(0));
                    }
                }
            }

            var medarbetarequery = "select person.id, person.first_name, person.last_name from sbk_avtal.medarbetare_sbk left join sbk_avtal.person on sbk_avtal.medarbetare_sbk.person_id = sbk_avtal.person.id;";
            using (var cmd = new NpgsqlCommand(medarbetarequery, conn))
            {
                using(var reader = cmd.ExecuteReader())
	            {
                    while (reader.Read())
                    {
                        medarbetare.Add(new Person
                        {
                            id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                        });
                    }
	            }
            }
        }
        dt = ds.Tables[0];

        // filtrerar bort inaktiva avtal från AvtalTableDataSource, men behåller dem i "OriginalDataSource
        Session.Add("OriginalDataSource", dt);
        dt = dt.AsEnumerable()
            .Where(row => row.Field<string>("status") == "Aktivt")
            .CopyToDataTable();
        
        Session.Add("AvtalTableDataSource", dt);

        AvtalTable.DataSource = dt;

        //AvtalTable.DataKeys = "ID";

        AvtalTable.DataBind();

        // filtrerings-rullistor
        avdelningdd.Items.Add("Alla avdelningar");
        medarbetaredd.Items.Add("Alla");

        foreach (var avd in avdelningar)
        {
            avdelningdd.Items.Add(avd);
        }

        for (int i = 0; i < medarbetare.Count; i++)
        {
            var person = medarbetare[i];
            person.dropdownindex = i + 1;
            medarbetaredd.Items.Add(string.Format("{0} {1}", person.FirstName, person.LastName));
        }

        Session.Add("AvtalTableDataSource", dt);
        

        Session.Add("medarbetare", medarbetare);
        Session.Add("avdelningar", avdelningar);
    }

    protected void AvtalTable_DataBound(object sender, GridViewRowEventArgs e) 
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var id = DataBinder.Eval(e.Row.DataItem, "id");
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
            e.Row.ToolTip = "Redigera avtal";
            e.Row.Attributes["onClick"] = "location.href='./avtal_detail.aspx?id=" + id.ToString() + "'";  //String.Format("location.href={0}'", link);
        }
    }

    protected void AvtalTable_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = Session["AvtalTableDataSource"] as DataTable;

        if (dt != null)
        {
            var dv = dt.DefaultView;
            dv.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            // dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            // AvtalTable.DataSource = Session["AvtalTableDataSource"];
            var newTable = dv.ToTable();
            AvtalTable.DataSource = newTable;
            Session.Add("AvtalTableDataSource", newTable);
            
            AvtalTable.DataBind();
                
        }
    }

    private string GetSortDirection(string column)
    {

        // By default, set the sort direction to ascending.
        string sortDirection = "ASC";

        // Retrieve the last column that was sorted.
        string sortExpression = ViewState["SortExpression"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }

        // Save new values in ViewState.
        ViewState["SortDirection"] = sortDirection;
        ViewState["SortExpression"] = column;

        return sortDirection;
    }

    protected void filterlb_Click(object sender, EventArgs e)
    {
        filterdiv.Visible = !filterdiv.Visible;
    }

    protected void filterbtn_Click(object sender, EventArgs e)
    {
        var avtalstyp = avtalstyprbl.SelectedValue;
        var ansvarig_avd = avdelningdd.SelectedValue;
        var ansvarig_sbk = medarbetaredd.SelectedValue;
        var status = statusrbl.SelectedValue;

        var dt = (DataTable)Session["OriginalDataSource"];

        DataTable filteredDt;
        try
        {
            filteredDt = dt.AsEnumerable()
                .Where(row => avtalstyp == "Alla typer" ? true : row.Field<string>("avtalstyp") == avtalstyp)
                .Where(row => ansvarig_avd == "Alla avdelningar" ? true : row.Field<string>("ansvarig_avd") == ansvarig_avd)
                .Where(row => ansvarig_sbk == "Alla" ? true : row.Field<string>("ansvarig_sbk") == ansvarig_sbk)
                .Where(row => status == "Alla" ? true : row.Field<string>("status") == status)
                .CopyToDataTable();
           
        }
        catch (Exception)
        {
            // TODO spara table headers
            filteredDt = dt.Copy();
            //while (filteredDt.Rows.Count > 1) filteredDt.Rows.RemoveAt(1);
            filteredDt.Clear();
        }

        AvtalTable.DataSource = filteredDt;
        AvtalTable.DataBind();
        Session.Add("AvtalTableDataSource", filteredDt);
    }
}