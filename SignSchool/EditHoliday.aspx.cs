using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignSchool
{
    public partial class EditHoliday : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RemoveDayButton_Click(object sender, EventArgs e)
        {
            DateTime date = Calendar.SelectedDate;
            System.Diagnostics.Debug.WriteLine(date.ToShortDateString());
            if (date == null) return;
            string dateString = date.ToString("yyyy-MM-dd");
            SetWorkDay(dateString, 1);
        }

        protected void AddDayButton_Click(object sender, EventArgs e)
        {
            DateTime date = Calendar.SelectedDate;
            System.Diagnostics.Debug.WriteLine(date.ToShortDateString());
            if (date == null) return;
            string dateString = date.ToString("yyyy-MM-dd");
            SetWorkDay(dateString, 0);
        }

        protected void Calendar_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime date = e.Day.Date;
            string dateString = date.ToString("yyyy-MM-dd HH:mm:ss:fff");
            bool workDay = IsWorkDay(dateString);

            if (!workDay)
            {
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#f595ad");
            }
            
        }

        private bool IsWorkDay(string date)
        {
            bool res = false;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("select top 1 * from [dbo].[WorkdayCalendar] where Date='"+ date + "'", con))
                {
                    
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    res = (bool)dr["WorkDay"];
                    con.Close();

                }
            }
            return res;
        }

        private void SetWorkDay(string date, int workday)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("UPDATE WorkdayCalendar SET [WorkDay] = '" + workday + "' WHERE [Date] = '" + date + "'", con))
                {

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
        }
    }
}