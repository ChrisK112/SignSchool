using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SignSchool
{
    public class EmailSend
    {

        public void SendEmail()
        {
            //next print date
            DateTime nextPrintDate = GetPrintDate();

            //if no result, cancel email
            if (nextPrintDate.Equals(DateTime.MinValue))
            {
                return;
            }

            if (!(nextPrintDate.ToShortDateString().Equals(DateTime.Now.ToShortDateString()) || CheckIfXWorkingDaysPrior(nextPrintDate, 2)))
            {
                return;
            }

            DataTable dt = GetData(nextPrintDate);

            int total = GetTotal(dt);
            string breakdown = GetBreakdown(dt);

            //send emails
            EmailHandler eh = new EmailHandler();

            //prepare email variables

            string fromEmail = "support@telebank-online.com";
            string ccEmails = GetEmails("CC");
            string toEmails = GetEmails("To");

            string subject = "Sense Sign School - Print for " + nextPrintDate.ToShortDateString();

            string body = CreateEmailBody(nextPrintDate, total, breakdown);

            eh.SendEmail(fromEmail, toEmails, ccEmails, subject, body);

        }

        private string GetEmails(string type)
        {
            string emails = "";

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("select email AS [email] from Users where sendemail='"+type+"'", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    
                    da.Fill(dt);

                }
                con.Close();
            }

            

            if (dt.Rows.Count > 0)
            {
                //string[] emailsA = new string[dt.Rows.Count];
                //int cnt = 0;
                //foreach (DataRow dr in dt.Rows)
                //{
                    //emails = emails + dr.Field<string>("email").ToString() + ";";
                //    emailsA[cnt] = emailsA;
               // }
                emails = string.Join(";", dt.Rows.OfType<DataRow>().Select(r => r[0].ToString()));
            }

            return emails;

        }

        private bool CheckIfXWorkingDaysPrior(DateTime nextPrintDate, int x)
        {

            int workingDaysDelay = x;

            //loop to find how what day is 2 working days prior.
            int i = 0;
            DateTime alertDate = nextPrintDate; //start with day prior to print day
            while (i != workingDaysDelay)
            {
                alertDate = alertDate.AddDays(-1);
                //check current date //if working day, add 1 to total working days
                if (CheckIfWorkingDay(alertDate))
                {
                    i++;
                }                
            }


            /* old code, not working as intended
            DateTime alertDate = nextPrintDate.AddDays(-2);
            string printDate = nextPrintDate.DayOfWeek.ToString();

            if(printDate.Equals("Monday"))
            {
                alertDate = nextPrintDate.AddDays(-1);
            }
            if (printDate.Equals("Tuesday"))
            {
                alertDate = nextPrintDate.AddDays(-2);
            }
            */

            //check if todays date = alert day
            if (DateTime.Now.ToShortDateString().Equals(alertDate.ToShortDateString())) return true;
            return false;

        }

        private string GetBreakdown(DataTable dt)
        {
            string breakdown = "";

            breakdown = breakdown + "<table text-align=\"center\" cellpadding=\"4\" border=\"1\"><tbody><tr><th scope = \"col\">Pack</th><th scope=\"col\">Count</th>";

            foreach (DataRow dr in dt.Rows)
            {
                breakdown = breakdown + "<tr>";

                string p = Convert.ToString(dr["Pack"]);
                string c = Convert.ToString(dr["Count"]);

                breakdown = breakdown + "<td>" + p + "</td>";
                breakdown = breakdown + "<td>" + c + "</td>";

                breakdown = breakdown + "</tr>";
            }
            breakdown = breakdown + "</table></tbody>";
            return breakdown;
        }


        private int GetTotal(DataTable dataTable)
        {

            int total = 0;

            foreach (DataRow dr in dataTable.Rows)
            {
                // Convert.ToString(dr["Constituent ID"]);
                int c = Convert.ToInt16(dr["Count"]);
                // dr.Field<int>("Count");
                total += c;
            }

            return total;
        }

        private string CreateEmailBody(DateTime datePrint, int total, string breakdown)
        {
            string date = "01/01/01";
            if (datePrint.ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
            {
                date = "TODAY";
            }
            else
            {
                date = datePrint.ToShortDateString();
            }

            string body = "[THIS IS AN AUTOMATED MESSAGE]<br/><br/>" +
                "Please note the next date for printing labels and sending Sign School packs is <b>" + date + ".</b><br/> The summary for the print can be found below:";
            body = body + "<br/><br/>Total amount of packs: " + total.ToString();

            body = body + "<br/><br/>Breakdown: <br/>" + breakdown;

            body = body + "<br/><br/>Sense Sign School site: <br/>https://client.telebank-online.com/SignSchool/Login.aspx<br/>";


            return body;

        }

        private DateTime GetPrintDate()
        {
            //loop through days, from today, max 30 days, checking the DB response
            DateTime toCheck = DateTime.Now;
            int maxDays = 30;
            for (int i = 0; i < maxDays; i++)
            {
                if (CheckDate(toCheck))
                {
                    return toCheck;
                }
                toCheck = toCheck.AddDays(1);
            }
            return DateTime.MinValue;

        }

        private bool CheckDate(DateTime toCheck)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("dbo.GetPacksToPrintOnDay", con))
                {

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    string date = toCheck.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = date;

                    con.Open();
                    da.Fill(dataTable);
                    con.Close();
                    da.Dispose();

                }
            }
            if (dataTable != null)
            {
                if (dataTable.Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private DataTable GetData(DateTime dateDT)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("dbo.GetPacksToPrintOnDay", con))
                {

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    string date = dateDT.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = date;

                    con.Open();
                    da.Fill(dataTable);
                    con.Close();
                    da.Dispose();

                }
            }
            return dataTable;
        }

        private bool CheckIfWorkingDay(DateTime dateDT)
        {
            bool result = false;
            DataTable dataTable = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("dbo.CheckIfWorkingDay", con))
                {

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    string date = dateDT.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@dateToCheck", SqlDbType.DateTime).Value = date;

                    // @ReturnVal could be any name
                    var returnParameter = cmd.Parameters.Add("@WorkingDay", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.Output;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    if ((int) returnParameter.Value == 1) result = true;


                    con.Close();


                }
            }
            return result;
        }

        private int GetDays(DataTable dataTable)
        {

            int total = 0;

            foreach (DataRow dr in dataTable.Rows)
            {
                // Convert.ToString(dr["Constituent ID"]);
                int c = Convert.ToInt16(dr["Count"]);
                // dr.Field<int>("Count");
                total += c;
            }

            return total;
        }

    }
}