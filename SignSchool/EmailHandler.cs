using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SignSchool
{
    public class EmailHandler
    {

        public EmailHandler()
        {

        }

        public void SendEmail(string fromEmail, string toEmails, string ccEmails, string subject, string body)
        {
            System.Diagnostics.Debug.WriteLine("in email SendEmail");
            //using (SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587)) unusable with current MFA security setup
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.UseDefaultCredentials = false;
                //smtpClient.Credentials = new System.Net.NetworkCredential("Support@telebank-online.com", "PicturesNothings43\\"); no longer usable
                smtpClient.Credentials = new System.Net.NetworkCredential("elovate.itslough@gmail.com", "ngicjeearqysmtfy");
                // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                using (MailMessage mail = new MailMessage())
                {
                    //Setting From , To and CC
                    mail.From = new MailAddress("elovate.itslough@gmail.com", "Support (Telebank)");
                    foreach (var address in toEmails.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mail.To.Add(address);
                    }
                    foreach (var address in ccEmails.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mail.CC.Add(address);
                    }
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    smtpClient.Send(mail);
                }

            }
                        
        }
    
    }

    /*
     * Public Shared Function SendEmail( _
                ByVal sFromEmail As String, _
                ByVal sToEmail As String, _
                ByVal sCcEmail As String, _
                ByVal sBccEmail As String, _
                ByVal sSubject As String, _
                ByVal sBodyHTML As String, _
                ByVal sBodyText As String, _
                ByVal sFiles As String) As String

        Dim xreturn As String = ""

        Dim NewMsg As New MailMessage

        If sToEmail <> "" Then
            NewMsg.To.Add(sToEmail)
        End If

        If sCcEmail <> "" Then
            NewMsg.CC.Add(sCcEmail)
        End If

        If sBccEmail <> "" Then
            NewMsg.Bcc.Add(sBccEmail)
        End If

        NewMsg.From = New MailAddress(sFromEmail)
        NewMsg.Subject = sSubject

        Dim strFiles As String = sFiles.Replace(",", ";")

        If strFiles <> "" Then
            For Each strX As String In strFiles.Split(";")
                If strX.Trim.Length > 0 Then
                    NewMsg.Attachments.Add(New Attachment(strX))
                End If
            Next
        End If

        ' Create Alternative View - TEXT
        Dim objAVText As AlternateView
        objAVText = AlternateView.CreateAlternateViewFromString(sBodyText, Nothing, "text/plain")

        ' Create Alternative View - HTML
        Dim objAVHTML As AlternateView
        objAVHTML = AlternateView.CreateAlternateViewFromString(sBodyHTML, Nothing, "text/HTML")

        NewMsg.AlternateViews.Add(objAVText)
        NewMsg.AlternateViews.Add(objAVHTML)

        Try
            Dim smtp As New SmtpClient
            smtp.Credentials = New System.Net.NetworkCredential("Support@telebank-online.com", "PicturesNothings43\")
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            smtp.Port = 587
            smtp.Host = "smtp.office365.com"
            smtp.EnableSsl = True
            smtp.Send(NewMsg)
            xreturn = "OK"

            'Dim smtp As New SmtpClient
            'smtp.Credentials = New System.Net.NetworkCredential("Telebank.Support@telebank-online.com", "T3leb@nk41")
            'smtp.DeliveryMethod = SmtpDeliveryMethod.Network
            'smtp.Host = "hosted.aspirets.com" ' ** Sending Emails from company domain names.
            ''smtp.Port = 25
            ''smtp.Host = "smtp.gxn.co.uk"  ' ** Sending Email from different domain i.e. spana.org
            'smtp.Send(NewMsg)
            'xreturn = "OK"

        Catch ex As Exception
            xreturn = ex.Message()
        Finally
            NewMsg.Dispose()
        End Try

        Return xreturn
    End Function
    */
}
 