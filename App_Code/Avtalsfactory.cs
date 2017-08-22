using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Configuration;

/// <summary>
/// Summary description for Avtalsfactory
/// </summary>
public static class Avtalsfactory
{
    // public const string Fields = "id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar";

    public static List<Person> GetPersons(NpgsqlDataReader reader)
    {
        var lst = new List<Person>();

        while (reader.Read())
        {
            lst.Add(new Person
            {
                id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Belagenhetsadress = reader.GetString(3),
                Postnummer = reader.GetString(4),
                Postort = reader.GetString(5),
                Telefonnummer = reader.GetString(6),
                epost = reader.GetString(7)
            });
        }
        return lst;
    }

    public static List<Person> GetNamesAndId(NpgsqlDataReader reader)
    {
        var lst = new List<Person>();

        while (reader.Read())
        {
            lst.Add(new Person
            {
                id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
            });
        }
        return lst;
    }

    public static List<Avtalsmodel> ParseAvtal(NpgsqlDataReader reader)
    {
        var lst = new List<Avtalsmodel>();

        while (reader.Read())
        {
            string diarienr;
            if (reader.GetValue(1) != DBNull.Value)
            {
                diarienr = reader.GetString(1);
            }
            else
            {
                diarienr = "";
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

            //string mptyp;
            //if (reader.GetString(5) != DBNull.Value)
            //{
            //    mptyp = reader.GetString(5);
            //}
            //else
            //{
            //    mptyp = "";
            //}

            long dbid;
            if (reader.GetValue(0) != DBNull.Value)
            {
                dbid = reader.GetInt32(0);
            }
            else
            {
                dbid = -1;
            }

            long? avtalstecknare;
            if (reader.GetValue(12) != DBNull.Value)
            {
                avtalstecknare = reader.GetInt32(12);
            }
            else
            {
                avtalstecknare = null;
            }

            long? avtalskontakt;
            if (reader.GetValue(13) != DBNull.Value)
            {
                avtalskontakt = reader.GetInt32(13);
            }
            else
            {
                avtalskontakt = null;
            }

            long? ansvarig_sbk;
            if (reader.GetValue(14) != DBNull.Value)
            {
                ansvarig_sbk = reader.GetInt32(14);
            }
            else
            {
                ansvarig_sbk = null;
            }

            long? upphandlat_av;
            if (reader.GetValue(17) != DBNull.Value)
            {
                upphandlat_av = reader.GetInt32(17);
            }
            else
            {
                upphandlat_av = null;
            }

            long? datakontakt;
            if (reader.GetValue(18) != DBNull.Value)
            {
                datakontakt = reader.GetInt32(18);
            }
            else
            {
                datakontakt = null;
            }

            // TODO lägg till id för avtalstecknare, kontakt etc, så att rätt person väljs i rullisterna
            lst.Add(new Avtalsmodel
            {
                id = dbid, // reader.GetInt32(0),
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
                kommentar = reader.GetString(11),
                avtalstecknare = avtalstecknare,
                avtalskontakt = avtalskontakt,
                ansvarig_sbk = ansvarig_sbk,
                upphandlat_av = upphandlat_av,
                ansvarig_avdelning = reader.GetString(15),
                ansvarig_enhet = reader.GetString(16),
                datakontakt = datakontakt,
                konto = reader.GetString(19),
                kstl = reader.GetString(20),
                vht = reader.GetString(21),
                mtp = reader.GetString(22),
                aktivitet = reader.GetString(23),
                objekt = reader.GetString(24),
                avtalstyp = reader.GetString(25),
            });
        }

        return lst;
    }

    public static Person ParsePerson(int id)
    {
        Person person;
        string connstr = ConfigurationManager.ConnectionStrings["postgres connection"].ConnectionString;
        using (var conn = new NpgsqlConnection(connstr))
        {
            conn.Open();

            var personquery = "select id, first_name, last_name, belagenhetsadress, postnummer, postort, tfn_nummmer, epost from sbk_avtal.person where person.id = @id";// "select id, diarienummer, startdate, enddate, status, motpartstyp, SBKavtalsid, scan_url, orgnummer, enligt_avtal, internt_alias, kommentar,  avtalstecknare, avtalskontakt, ansvarig_sbk, ansvarig_avd, ansvarig_enhet from sbk_avtal.avtal where id = @p1;";
            using (var cmd = new NpgsqlCommand(personquery, conn))
            {
                cmd.Parameters.AddWithValue("id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();

                    person = new Person
                    {
                        id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Belagenhetsadress = reader.GetString(3),
                        Postnummer = reader.GetString(4),
                        Postort = reader.GetString(5),
                        Telefonnummer = reader.GetString(6),
                        epost = reader.GetString(7)
                    };
                }
            }
        }
        return person;
    }
}