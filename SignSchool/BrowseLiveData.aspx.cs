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
    public partial class BrowseLiveData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //DisplayData();

            
        }

        protected void DisplayAllData()
        {
            string user = HttpContext.Current.User.Identity.Name;
            //get data here
            DataTable dataTable = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select * from [dbo].[DataView] order by [id] DESC", con);
            con.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            con.Close();
            da.Dispose();

            if (dataTable != null)
            {
                if (dataTable.Rows.Count > 0)
                {
                    GridView1.DataSource = dataTable;
                    GridView1.DataBind();
                }
            }

        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            //get contents of search
            string value = SearchTextBox.Text;
            string searchCriteria = SearchDropdownList.SelectedValue;
            string value2 = SearchTextBox2.Text;

            //for all
            if (searchCriteria.Equals("All"))
            {
                DisplayAllData();
                return;
            }
            //for normal
            if (!searchCriteria.ToLower().Contains("date"))
            {
                SearchByCriteria(value, searchCriteria);
                return;
            } 
            //for dates
            if (searchCriteria.ToLower().Contains("date"))
            {
                SearchByCriteriaDate(value, value2, searchCriteria);
                return;
            }



        }

        protected void SearchByCriteria(string value, string searchCriteria)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("select * from [dbo].[DataView] WHERE [" + searchCriteria + "] LIKE '%" + value + "%'", con))
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

            if (dataTable != null)
            {
                GridView1.DataSource = dataTable;
                GridView1.DataBind();
                if (dataTable.Rows.Count > 0)
                {
                    lblMessage.ForeColor = Color.Black;
                    lblMessage.Text = "Search Found " + dataTable.Rows.Count + " Results";
                }
                else
                {
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "Search Returned No Results";
                }
            }

        }

        protected void SearchByCriteriaDate(string date1, string date2, string searchCriteria)
        {
            if (date2.Equals("") && date1.Equals(""))
            {
                ClearView();
                return;
            }
            
            if (date2.Equals("")) date2 = date1;
            if (date1.Equals("")) date1 = date2;

            date1 = DateTime.Parse(date1).ToShortDateString();
            date2 = DateTime.Parse(date2).ToShortDateString();



            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("select * from [dbo].[DataView] WHERE CONVERT(date,[" + searchCriteria + "],103) BETWEEN CONVERT(date,'" + date1 + "',103) AND CONVERT(date,'" + date2 + "',103)", con))
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

            if (dataTable != null)
            {
                GridView1.DataSource = dataTable;
                GridView1.DataBind();
                if (dataTable.Rows.Count > 0)
                {
                    lblMessage.ForeColor = Color.Black;
                    lblMessage.Text = "Search Found " + dataTable.Rows.Count + " Results";
                }
                else
                {
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "Search Returned No Results";
                }
            }

        }

        protected void SearchDropdownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string searchCriteria = SearchDropdownList.SelectedValue;

            ClearView();

            SearchTextBox.Enabled = true;

            if (searchCriteria.ToLower().Contains("date"))
            {
                SearchTextBox.TextMode = TextBoxMode.Date;
                SearchTextBox2.Visible = true;
                DateLabel1.Visible = true;
                DateLabel2.Visible = true;
            }
            else
            {
                SearchTextBox.TextMode = TextBoxMode.Search;
                SearchTextBox2.Visible = false;
                DateLabel1.Visible = false;
                DateLabel2.Visible = false;
            }

            if (searchCriteria.Equals("All"))
            {
                SearchTextBox.Enabled = false;
            }

        }

        protected void ClearView()
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            lblMessage.Text = "";
        }
    }
}