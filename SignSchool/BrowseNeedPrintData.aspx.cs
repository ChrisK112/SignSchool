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
    public partial class BrowseNeedPrintData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            DisplayData();

            
        }

        protected void DisplayData()
        {
            string user = HttpContext.Current.User.Identity.Name;
            //get data here
            DataTable dataTable = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select * from [dbo].[NeedPrintingView] order by [id] DESC", con);
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

    }
}