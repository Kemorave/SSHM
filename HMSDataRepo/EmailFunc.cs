
using System;

namespace HMSDataRepo
{
    public static class EmailFunc
    {
        private const string ComplainHTMLBody = @"<h1 style=""text-align: left;""><span style=""color: #003366;""><strong><img src=""https://cdn.glitch.com/1328a33f-b817-4624-9c2d-34d57df7414d%2Fssh.png?v=1612106680697"" alt=""SSH Logo"" width=""96"" height=""96"" /></strong><strong>Self serving hotel managment</strong> <br /></span></h1>
<p>&nbsp;</p>
<h2><img style = ""float: left;"" src=""https://cdn.glitch.com/1328a33f-b817-4624-9c2d-34d57df7414d%2FConfused_96px.png?v=1612106606569"" alt=""Complaints icon"" width=""60"" height=""60"" /></h2>
<p style = ""padding-left: 90px;"" ><strong ><span style=""color: #000000;""><span style = ""text-align: center;"" > Complaint report</span></span></strong></p>
<p style = ""padding-left: 90px;"" ><strong ><span style=""color: #000000;""><span style = ""text-align: center;"" > Email </ span ><span style=""text-align: center;""> : <span style = ""color: #003366;"" >{0}</span><br /></span></span></strong></p>
<p style = ""padding-left: 90px;"" ><strong ><span style=""color: #000000;""><span style = ""text-align: center;"" > User ID : {1}<br /></span></span></strong></p>
<p style = ""padding-left: 90px;"" ><strong ><span style=""color: #000000;""><span style = ""text-align: center;"" > Submition date : <span style = ""color: #ff6600;"" >{2}</span></span></span></strong></p>
<p>{3}</p>
<p style = ""text-align: center;"" > &nbsp;</p>
<p style = ""text-align: center;"" ><span style=""text-decoration: underline;""><strong><span style = ""color: #003366; text-decoration: underline;"" > you can't reply to this message</span></strong></span></p>
<p style = ""text-align: center;"" ><span style=""text-decoration: underline;""><strong><span style = ""color: #003366; text-decoration: underline;"" > SSH Team</span></strong></span></p>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>&nbsp;</div>";
        private const string SecurityReportHTMLBody = @"<h1 style=""text-align: left;""><span style=""color: #003366;""><strong><img src=""https://cdn.glitch.com/1328a33f-b817-4624-9c2d-34d57df7414d%2Fssh.png?v=1612106680697"" alt=""SSH Logo"" width=""96"" height=""96"" /></strong><strong>Self serving hotel managment</strong> <br /></span></h1>
<p>&nbsp;</p>
<h2><img style = ""float: left;"" src=""https://cdn.glitch.com/1328a33f-b817-4624-9c2d-34d57df7414d%2FTV%20Off_96px.png?v=1612106629079"" alt=""Complaints icon"" width=""60"" height=""60"" /></h2>
<p style = ""padding-left: 90px;"" ><strong ><span style=""color: #000000;""><span style = ""text-align: center;"" ><span style=""color: #ff0000;"">Security report</span> </span></span></strong></p>
<p style = ""padding-left: 90px;"" ><strong ><span style= ""color: #000000;"" ><span style= ""text-align: center;"" > Submition date : <span style = ""color: #ff6600;"" >{0}</span></span></span></strong></p>
<p>{1}</p>
<p style = ""text-align: center;"" > &nbsp;</p>
<p style = ""text-align: center;"" ><span style=""text-decoration: underline;""><strong><span style = ""color: #003366; text-decoration: underline;"" > you can't reply to this message</span></strong></span></p>
<p style = ""text-align: center;"" ><span style=""text-decoration: underline;""><strong><span style = ""color: #003366; text-decoration: underline;"" > SSH Team</span></strong></span></p>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>&nbsp;</div>";
        private const string InformationReportHTMLBody = @"<h1 style="" text-align: left;""><span style=""color: #003366;""><strong><img src=""https://cdn.glitch.com/1328a33f-b817-4624-9c2d-34d57df7414d%2Fssh.png?v=1612106680697"" alt=""SSH Logo"" width=""96"" height=""96"" /></strong><strong>Self serving hotel managment</strong> <br /></span></h1>
<p>&nbsp;</p>
<h2><img style = ""float: left;"" src=""https://cdn.glitch.com/1328a33f-b817-4624-9c2d-34d57df7414d%2FInformation_96px.png?v=1612111520516"" alt=""Complaints icon"" width=""60"" height=""60"" /></h2>
<p style = ""padding-left: 90px;"" ><span style=""color: #00ff00;""><strong><span style = "" text-align: center;"" ><span style=""color: #008000;"">Information report</span> </span></strong></span></p>
<p style = ""padding-left: 90px;"" ><span style= ""color: #000000;"" ><strong ><span style= "" text-align: center;"" > Repot type : <span style = ""color: #003366;"" >{0}</span></span></strong></span></p>
<p style = ""padding-left: 90px;"" ><strong ><span style=""color: #000000;""><span style = "" text-align: center;"" > Submition date : <span style = ""color: #ff6600;"" >{1}</span></span></span></strong></p>
<p>{2}</p>
<p style = "" text-align: center;"" > &nbsp;</p>
<p style = "" text-align: center;"" ><span style="" text-decoration: underline;""><strong><span style = ""color: #003366; text-decoration: underline;"" > you can't reply to this message</span></strong></span></p>
<p style = "" text-align: center;"" ><span style="" text-decoration: underline;""><strong><span style = ""color: #003366; text-decoration: underline;"" > SSH Team</span></strong></span></p>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>&nbsp;</div>
<div>&nbsp;</div>";

        private static string GetComplaintString(string userEmail, string userID, string complaint)
        {
            return string.Format(ComplainHTMLBody, userEmail, userID, DateTime.UtcNow.ToLongDateString(), complaint);
        }

        private static string GetSecurityReportString(string report)
        {
            return string.Format(SecurityReportHTMLBody, DateTime.UtcNow.ToLongDateString(), report);
        }

        private static string GetInformationReportString(string type, string report)
        {
            return string.Format(InformationReportHTMLBody, type, DateTime.UtcNow.ToLongDateString(), report);
        }
        public static void Email(string email, string htmlString)
        {
            using (System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage())
            {
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                message.From = new System.Net.Mail.MailAddress("kemorave@gmail.com");
                message.To.Add(new System.Net.Mail.MailAddress(email));
                message.Subject = "Test";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("kemorave@gmail.com", "0121127623");
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
        }

        public static void SendComplaintMessage(string email, string userEmail, string userID, string complaint)
        {
            Email(email, GetComplaintString(userEmail, userID, complaint));
        }
        public static void SendInformationReportMessage(string email, string type, string report)
        {
            Email(email, GetInformationReportString(type, report));
        }
        public static void SendSecurityReportMessage(string email, string report)
        {
            Email(email, GetSecurityReportString(report));
        }
    }
}