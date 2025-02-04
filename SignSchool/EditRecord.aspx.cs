using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignSchool
{
    public partial class EditRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Request.QueryString["editid"].ToString()))
                {
                    Response.Redirect("Home.aspx");
                }
                string id = Request.QueryString["editid"].ToString();

                FillData(id);
            }
            
        }

        private void FillData(string id)
        {
            //get data
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("select top 1 * from [dbo].[EditView] WHERE [id] = '" + id + "'", con))
                {

                    con.Open();

                    // create data adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                    con.Close();
                    da.Dispose();

                }
            }
            
            //fill data in textboxes
            //should only loop once
            foreach(DataRow dr in dataTable.Rows)
            {
                ConsId_txt.Text = dr.Field<string>("Constituent ID");
                Title_txt.Text = dr.Field<string>("Title 1");
                Fname_txt.Text = dr.Field<string>("First Name");
                Sname_txt.Text = dr.Field<string>("Surname");
                ad1_txt.Text = dr.Field<string>("Preferred Address Line 1");
                ad2_txt.Text = dr.Field<string>("Preferred Address Line 2");
                ad3_txt.Text = dr.Field<string>("Preferred Address Line 3");
                city_txt.Text = dr.Field<string>("Preferred City");
                county_txt.Text = dr.Field<string>("Preferred County");
                pcode_txt.Text = dr.Field<string>("Preferred Postcode");
                country_txt.Text = dr.Field<string>("Preferred Country");
                email_txt.Text = dr.Field<string>("Email Number");
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {

            //get data strings
            string consid = ConsId_txt.Text;
            string title = Title_txt.Text;
            string fname = Fname_txt.Text;
            string sname = Sname_txt.Text;
            string ad1 = ad1_txt.Text;
            string ad2 = ad2_txt.Text;
            string ad3 = ad3_txt.Text;
            string city = city_txt.Text;
            string county = county_txt.Text;
            string pcode = pcode_txt.Text;
            string country = country_txt.Text;
            string email = email_txt.Text;

            string user = HttpContext.Current.User.Identity.Name;
            string date = DateTime.Now.ToString("dd/MM/yyyy");

            if (string.IsNullOrEmpty(Request.QueryString["editid"].ToString()))
            {
                Response.Redirect("Home.aspx");
            }
            string id = Request.QueryString["editid"].ToString();


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("dbo.UpdateRecord", con))
                {
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 50).Value = id;

                    cmd.Parameters.Add("@Constituent_ID", SqlDbType.NVarChar, 50).Value = consid;
                    cmd.Parameters.Add("@Title_1", SqlDbType.NVarChar, 10).Value = title;
                    cmd.Parameters.Add("@First_Name", SqlDbType.NVarChar, 50).Value = fname;
                    cmd.Parameters.Add("@Surname", SqlDbType.NVarChar, 50).Value = sname;
                    cmd.Parameters.Add("@Preferred_Address_Line_1", SqlDbType.NVarChar, 50).Value = ad1;
                    cmd.Parameters.Add("@Preferred_Address_Line_2", SqlDbType.NVarChar, 50).Value = ad2;
                    cmd.Parameters.Add("@Preferred_Address_Line_3", SqlDbType.NVarChar, 50).Value = ad3;
                    cmd.Parameters.Add("@Preferred_City", SqlDbType.NVarChar, 50).Value = city;
                    cmd.Parameters.Add("@Preferred_County", SqlDbType.NVarChar, 50).Value = county;
                    cmd.Parameters.Add("@Preferred_Postcode", SqlDbType.NVarChar, 50).Value = pcode;
                    cmd.Parameters.Add("@Preferred_Country", SqlDbType.NVarChar, 50).Value = country;
                    cmd.Parameters.Add("@Email_Number", SqlDbType.NVarChar, 50).Value = email;

                    cmd.Parameters.Add("@Date_Edited", SqlDbType.NVarChar, 50).Value = date;
                    cmd.Parameters.Add("@Edit_User", SqlDbType.NVarChar, 50).Value = user;

                    cmd.ExecuteNonQuery();

                }
                con.Close();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Updated!')", true);
                Response.Redirect("EditRecordSearch.aspx");
            }
        }
    }
}