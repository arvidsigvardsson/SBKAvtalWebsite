# -*- coding: utf-8 -*-
import csv
import sys
import psycopg2
try:
    conn = psycopg2.connect("dbname='avtalskatalogSBK' user='postgres' host='localhost'")
    print conn
except:
    print 'Kunde inte ansluta'

cursor = conn.cursor()
#
# cursor.execute("""insert into sbkavtal.avtalsinnehall(innehall, avtalsid) values('avtalsinnehåll 3', 1);""")
# conn.commit()

# cursor.execute("select * from sbkavtal.avtal")
# rows = cursor.fetchall()
#
# for row in rows:
#     print row
# try:
#     cursor.execute("""insert into sbkavtal.avtalsinnehall(innehall, avtalsid) values('avtalsinnehåll 3', 1);""")
#     print 'insert verkar funka'
# except:
#     print 'insert funkar inte'
#
with open("avtal psycopg.csv") as f:
    #for row in file:
    #    print row
    data = csv.DictReader(f)
    for row in data:
        # fakturaadress
        fakt_fname = row['fakt Fornamn'].decode('iso-8859-1').encode('utf8')
        fakt_ename = row['fakt Efternamn'].decode('iso-8859-1').encode('utf8')
        fakt_ref = row['fakt Referens'].decode('iso-8859-1').encode('utf8')
        fakt_addr = row['fakt Belagenhetsadress'].decode('iso-8859-1').encode('utf8')
        fakt_pnr = row['fakt Postnummer'].decode('iso-8859-1').encode('utf8')
        fakt_port = row['fakt Postort'].decode('iso-8859-1').encode('utf8')

        fakt_values = (fakt_fname, fakt_ename, fakt_addr, fakt_pnr, fakt_port, fakt_ref)
        if fakt_fname != "":
            cursor.execute("insert into sbkavtal.fakturaadress(first_name, last_name, belagenhetsadress, postnummer, postort, referens) values(%s, %s, %s, %s, %s, %s) on conflict do nothing returning id;", fakt_values)
            fakt_id = cursor.fetchone()[0]
        else:
            fakt_id = None
        # print fakt_fname, 'fick id', fakt_id

        # avtalstecknare
        teckn_fname = row['avtteckn Fornamn'].decode('iso-8859-1').encode('utf8')
        teckn_ename = row['avtteckn Efternamn'].decode('iso-8859-1').encode('utf8')
        teckn_addr = row['avtteckn Belagenhetsadress'].decode('iso-8859-1').encode('utf8')
        teckn_pnr = row['avtteckn Postnummer'].decode('iso-8859-1').encode('utf8')
        teckn_port = row['avtteckn Postort'].decode('iso-8859-1').encode('utf8')
        teckn_tel = row['avtteckn Telefon'].decode('iso-8859-1').encode('utf8')
        teckn_epost = row['avtteckn E-post'].decode('iso-8859-1').encode('utf8')

        teckn_values = (teckn_fname, teckn_ename, teckn_addr, teckn_pnr, teckn_port, teckn_tel, teckn_epost)

        if teckn_fname != "":
            cursor.execute("insert into sbkavtal.person(first_name, last_name, belagenhetsadress, postnummer, postort, tfn_nummer, epost) values(%s, %s, %s, %s, %s, %s, %s) on conflict do nothing returning id;", teckn_values)
            teckn_id = cursor.fetchone()[0]
            print teckn_fname, 'fick id', teckn_id
        else:
            teckn_id = None

        # avtalskontakt
        avtkontakt_values = (
            row['avtkont Fornamn'].decode('iso-8859-1').encode('utf8'),
            row['avtkont Efternamn'].decode('iso-8859-1').encode('utf8'),
            row['avtkont Belagenhetsadress'].decode('iso-8859-1').encode('utf8'),
            row['avtkont Postnummer'].decode('iso-8859-1').encode('utf8'),
            row['avtkont Postort'].decode('iso-8859-1').encode('utf8'),
            row['avtkont Telefon'].decode('iso-8859-1').encode('utf8'),
            row['avtkont E-post'].decode('iso-8859-1').encode('utf8'),
            )

        if avtkontakt_values[0] != "":
            cursor.execute("insert into sbkavtal.person(first_name, last_name, belagenhetsadress, postnummer, postort, tfn_nummer, epost) values(%s, %s, %s, %s, %s, %s, %s) on conflict do nothing returning id;", avtkontakt_values)
            avtkontakt_id = cursor.fetchone()[0]
            print avtkontakt_values[0], 'fick id', avtkontakt_id
        else:
            avtkontakt_id = None

        # datakontakt
        datakont_values = (
            row['datakont Fornamn'].decode('iso-8859-1').encode('utf8'),
            row['datakont Efternamn'].decode('iso-8859-1').encode('utf8'),
            row['datakont Belagenhetsadress'].decode('iso-8859-1').encode('utf8'),
            row['datakont Postnummer'].decode('iso-8859-1').encode('utf8'),
            row['datakont Postort'].decode('iso-8859-1').encode('utf8'),
            row['datakont Telefon'].decode('iso-8859-1').encode('utf8'),
            row['datakont E-post'].decode('iso-8859-1').encode('utf8'),
        )

        if datakont_values[0] != "":
            cursor.execute("insert into sbkavtal.person(first_name, last_name, belagenhetsadress, postnummer, postort, tfn_nummer, epost) values(%s, %s, %s, %s, %s, %s, %s) on conflict do nothing returning id;", avtkontakt_values)
            datakont_id = cursor.fetchone()[0]
        else:
            datakont_id = None

        # TODO nästa gång, lägga in i avtalstabellen

        # str = row['fakt Belagenhetsadress'].decode('iso-8859-1').encode('utf8')
        #
        # dt = (str,)
        # cursor.execute("insert into sbkavtal.foo(fritext) values(%s);", dt)

conn.commit()

# with open("avtal omencoding.csv") as f:
#     data = csv.DictReader(f)
#     for row in data:
#         print row['Fakt Förnamn'].decode('iso-8859-10')
