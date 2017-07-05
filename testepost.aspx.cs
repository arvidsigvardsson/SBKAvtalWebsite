using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

public partial class testepost : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string debug;
        SmtpClient smtpClient = new SmtpClient();
        MailMessage message = new MailMessage();

        try
        {
            MailAddress fromAddress = new MailAddress("arvid.sigvardsson@malmo.se");
            smtpClient.Host = "161.52.15.219";
            smtpClient.Port = 25;

            smtpClient.UseDefaultCredentials = true;

            message.From = fromAddress;
            message.To.Add("arvid.sigvardsson@malmo.se");
            message.Subject = "Testar att skicka från katalogen";
            message.Body = "En test";

            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Send(message);

            debug = "Mail skickat";
        }
        catch (Exception ex)
        {
            debug = ex.Message;
            
        }

        Label1.Text = debug;
    }
}