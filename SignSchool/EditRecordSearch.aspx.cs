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
    public partial class EditRecordSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            //get contents of search
            string value = SearchTextBox.Text;
            string searchCriteria = SearchDropdownList.SelectedValue;

            SearchByCriteria(value, searchCriteria);

        }

        protected void SearchByCriteria(string value, string searchCriteria)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("select * from [dbo].[EditView] WHERE [" + searchCriteria + "] LIKE '%" + value + "%'", con))
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
                if (dataTable.Rows.Count > 0)
                {
                    GridView_EditRecord.DataSource = dataTable;
                    GridView_EditRecord.DataBind();
                    lblMessage.Text = "Search Found " + dataTable.Rows.Count + " Results";
                }
                else
                {
                    lblMessage.ForeColor = Color.Red;
                    lblMessage.Text = "Search Returned No Results";
                }
            }

        }

        protected void GridView_EditRecord_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int itemIndex = e.NewEditIndex;
            GridViewRow row = GridView_EditRecord.Rows[itemIndex];

            TableCell cell = row.Cells[1]; //first cell is edit button, id is second (hopefully)
            string id = cell.Text;
            Response.Redirect("EditRecord.aspx?editid=" + id);
           
        }

    }
}