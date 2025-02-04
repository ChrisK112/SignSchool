using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignSchool
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Make sure labels have been merged and are correct.')", true);
                Refresh();
            }
            //ConfirmLabelsButton.Enabled = true;


        }

        protected void Refresh()
        {
            int count = GetLabelCount();
            CountLabel.Text = "Current labels count: " + count;
        }

        protected int GetLabelCount()
        {
            //get data here
            DataTable dataTable = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select * from [dbo].[LabelView]", con);
            con.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            con.Close();
            da.Dispose();

            return dataTable.Rows.Count;
        }

        protected void ConfirmLabelsButton_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            string user = HttpContext.Current.User.Identity.Name;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("dbo.UpdatePacks", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //command.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Edit_User", SqlDbType.NVarChar, 50).Value = user;

                    con.Open();
                    da.Fill(dataTable);
                    con.Close();
                    da.Dispose();

                    ViewState["gridTable"] = dataTable;
                }
            }

            if(dataTable != null)
            {
                if (dataTable.Rows.Count > 0)
                {
                    GridView1.DataSource = dataTable;
                    GridView1.DataBind();
                    UndoLabelButton.Enabled = true;
                    ConfirmLabelsButton.Enabled = false;
                    ProgressLabel.Text = "Updated the below records." + "<br/>" + "Count: " + dataTable.Rows.Count;

                    //update 12 packs
                    UpdatePack12();

                }
                else
                {
                    ProgressLabel.Text = "No records to update";
                    UndoLabelButton.Enabled = false;
                }
            }
            else
            {
                ProgressLabel.Text = "No records to update";
                UndoLabelButton.Enabled = false;
            }

        }

        protected void UpdatePack12()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("dbo.Update12Packs", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
            }
        }

        protected void UndoLabelButton_Click(object sender, EventArgs e)
        {
 
            string ids = "";
            string pack12s = "";
            DataTable dt2 = (DataTable)ViewState["gridTable"];
            foreach (DataRow row in dt2.Rows)
            {
                string id = row["ID"].ToString();
                string pack = row["Pack Sent"].ToString();
                if (pack.Equals("12")) {
                    if (pack12s.Equals("")) pack12s = pack12s + id;
                    else pack12s = pack12s + "," + id;
                }
                if (ids.Equals("")) ids = ids + id;
                else ids = ids + "," + id;                

            }
            //ids = ids + "'";



            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("dbo.RestorePack12Record", con))
                {
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@List", pack12s));
                    cmd.Parameters["@List"].SqlDbType = SqlDbType.VarChar;

                    cmd.ExecuteNonQuery();

                }
                con.Close();
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("dbo.UndoUpdatePacks", con))
                {
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@List", ids));
                    cmd.Parameters["@List"].SqlDbType = SqlDbType.VarChar;

                    cmd.ExecuteNonQuery();
                    
                }
                con.Close();
            }


            ProgressLabel.Text = "Changes Reverted";
            UndoLabelButton.Enabled = false;
            ConfirmLabelsButton.Enabled = true;

            //clear gridview
            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        protected void LabelCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (LabelCheckbox.Checked)
            {
                ConfirmLabelsButton.Style.Remove("display");
                UndoLabelButton.Style.Remove("display");
                ProgressLabel.Style.Remove("display");
            }
            else
            {
                ConfirmLabelsButton.Style.Add("display", "none");
                UndoLabelButton.Style.Add("display", "none");
                ProgressLabel.Style.Add("display", "none");
            }
        }

        protected void datagrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                DataTable dt2 = (DataTable)ViewState["gridTable"];
                GridView1.PageIndex = e.NewPageIndex;
                GridView1.DataSource = dt2;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
    }
}