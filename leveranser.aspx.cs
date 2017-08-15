using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using Npgsql;

public partial class leveranser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var ds = new DataSet();
        var dt = new DataTable();

        string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
        using (var conn = new NpgsqlConnection(connstr))
        {
            var sqlquery = "select sbk_avtal.avtal.id, datum, sbk_avtal.avtal.enligt_avtal as \"Enligt avtal\" from sbk_avtal.leveranser left join sbk_avtal.avtal on avtal_id=sbk_avtal.avtal.id order by datum;";
            using (var da = new NpgsqlDataAdapter(sqlquery, conn))
            {
                da.Fill(ds);
            }

        }
        dt = ds.Tables[0];

        LeveranserGrid.DataSource = dt;
        LeveranserGrid.DataBind();
    }

    protected void LeveranserGrid_DataBound(object sender, GridViewRowEventArgs e)
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
}