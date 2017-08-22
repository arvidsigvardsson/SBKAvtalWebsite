using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Npgsql;
using System.Data;

public partial class avtalsgrid : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var ds = new DataSet();
        var dt = new DataTable();
        
        string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
        using (var conn = new NpgsqlConnection(connstr))
        {
            var sqlquery = "select avtal.id, enligt_avtal, diarienummer, startdate, enddate, status, motpartstyp, ansvarig_avd, avtalstecknare.first_name || ' ' || avtalstecknare.last_name as \"Avtalstecknare\", ansvarig_sbk.first_name || ' ' || ansvarig_sbk.last_name as \"ansvarig_sbk\", avtalskontakt.first_name || ' ' || avtalskontakt.last_name as \"Avtalskontakt\", ansvarigsbk.first_name as \"Ansvarig SBK - Förnamn\", ansvarigsbk.last_name as \"Ansvarig SBK - efternamn\", ansvarigsbk.epost as \"Ansvarig SBK - epost\", data.first_name as \"datakontakt förnamn\", data.last_name as \"datakontakt efternamn\", data.epost as \"datakontakt epost\", kontakt.last_name as \"Avtalskontakt efternamn\", orgnummer from sbk_avtal.avtal left join sbk_avtal.person data on data.id=sbk_avtal.avtal.datakontakt left join sbk_avtal.person kontakt on kontakt.id=sbk_avtal.avtal.avtalskontakt left join sbk_avtal.person ansvarigsbk on ansvarigsbk.id=sbk_avtal.avtal.ansvarig_sbk left join sbk_avtal.person avtalstecknare on avtalstecknare.id=sbk_avtal.avtal.avtalstecknare left join sbk_avtal.person avtalskontakt on avtalskontakt.id=sbk_avtal.avtal.avtalskontakt left join sbk_avtal.person ansvarig_sbk on ansvarig_sbk.id=sbk_avtal.avtal.ansvarig_sbk;";
            using (var da = new NpgsqlDataAdapter(sqlquery, conn))
            {
                da.Fill(ds);
            }

        }
        dt = ds.Tables[0];

      

        AvtalTable.DataSource = dt;

        //AvtalTable.DataKeys = "ID";

        AvtalTable.DataBind();

        Session.Add("AvtalTableDataSource", dt);
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
}