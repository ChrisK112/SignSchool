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
    public partial class CancelRecord : System.Web.UI.Page
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
                RecordId_txt.Text = dr.Field<int>("Id").ToString();
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
                regdate_txt.Text = dr.Field<string>("Registration date");
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {


            string user = HttpContext.Current.User.Identity.Name;
 
            if (string.IsNullOrEmpty(Request.QueryString["editid"].ToString()))
            {
                Response.Redirect("Home.aspx");
            }
            string id = Request.QueryString["editid"].ToString();


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("dbo.CancelRecord", con))
                {
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@Edit_User", SqlDbType.NVarChar, 50).Value = user;

                    cmd.ExecuteNonQuery();

                }
                con.Close();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Cancelled!')", true);
                Response.Redirect("CancelRecordSearch.aspx");
            }
        }
    }
}