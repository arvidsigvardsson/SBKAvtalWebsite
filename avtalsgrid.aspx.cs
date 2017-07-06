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
            var sqlquery = "select id as \"ID\", enligt_avtal as \"Enligt avtal\", diarienummer, datakontakt from sbk_avtal.avtal;";
            using (var da = new NpgsqlDataAdapter(sqlquery, conn))
            {
                da.Fill(ds);
            }

        }
        dt = ds.Tables[0];
        AvtalTable.DataSource = dt;
        AvtalTable.DataBind();

        Session.Add("AvtalTableDataSource", dt);
    }

    protected void AvtalTable_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = Session["AvtalTableDataSource"] as DataTable;

        if (dt != null)
        {

            //Sort the data.
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
            AvtalTable.DataSource = Session["AvtalTableDataSource"];
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