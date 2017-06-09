using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;

/// <summary>
/// Summary description for Avtalsfactory
/// </summary>
public static class Avtalsfactory
{
    public const string Fields = "id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar";

    public static List<Avtalsmodel> ParseAvtal(NpgsqlDataReader reader)
    {
        var lst = new List<Avtalsmodel>();

        while (reader.Read())
        {
            long? diarienr;
            if (reader.GetValue(1) != DBNull.Value)
            {
                diarienr = reader.GetInt64(1);
            }
            else
            {
                diarienr = null;
            }

            DateTime? sd;
            if (reader.GetValue(2) != DBNull.Value)
            {
                sd = reader.GetDateTime(2);
            }
            else
            {
                sd = null;
            }

            DateTime? ed;
            if (reader.GetValue(3) != DBNull.Value)
            {
                ed = reader.GetDateTime(3);
            }
            else
            {
                ed = null;
            }

            lst.Add(new Avtalsmodel
            {
                id = reader.GetInt32(0),
                diarienummer = diarienr,
                startdate = sd,
                enddate = ed,
                status = reader.GetString(4),
                motpartstyp = reader.GetString(5),
                sbkid = reader.GetInt32(6),
                scan_url = reader.GetString(7),
                orgnummer = reader.GetString(8),
                enligtAvtal = reader.GetString(9),
                interntAlias = reader.GetString(10),
                kommentar = reader.GetString(11)
            });
        }

        return lst;
    }
}