using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SignSchool
{
    public partial class Home : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //string username1 = User.Identity.Name;
            //string username2 = System.Web.HttpContext.Current.User.Identity.Name;

            //get next print info
            GeneratePrintInfo();

            //BasecampHandler bch = new BasecampHandler();
            //bch.StartAPI();
            /*if (Request.QueryString["authBC"] != null)
            {

                Response.Redirect("https://launchpad.37signals.com/authorization/new?type=web_server&" +
                "client_id=58108be6af33000fd7ca131c1dae089b8c9f3b37&" +
                "redirect_uri=https://client.telebank-online.com/SignSchool/Login.aspx");
            }
            if (Request.QueryString["code"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["code"].ToString()))
                {
                    string code = Request.QueryString["code"].ToString();
                    try
                    {
                        HandleBaseCampAuth(code).Wait();
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                    
                    
                }
            }

             */

        }
                
        private async Task HandleBaseCampAuth(string code)
        {
            BasecampHandler bch = new BasecampHandler();
            Task task = bch.StartAPI(code);

            await Task.WhenAll(task);


        }


        private void GeneratePrintInfo()
        {
            //get next print date;
            GetPrintDate();
        }

        private void GetPrintDate()
        {
            //loop through days, from today, max 30 days, checking the DB response
            DateTime toCheck = DateTime.Now;
            int maxDays = 30;
            for(int i = 0; i < maxDays; i++)
            {
                if (CheckDate(toCheck)) break;
                toCheck = toCheck.AddDays(1);
            }

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
                    GridView_NextPrint.DataSource = dataTable;
                    GridView_NextPrint.DataBind();
                    GridView_NextPrint.CellPadding = 5;

                    SetLabels(dataTable, toCheck.ToString("dd/MM/yyyy"));


                    return true;
                }
            }
            return false;
        }

        private void SetLabels(DataTable dataTable, string date)
        {
            //Next Date
            DateLabelDATE.Text = date;

            //Total
            int total = 0;

            foreach(DataRow dr in dataTable.Rows)
            {
               // Convert.ToString(dr["Constituent ID"]);
                int c = Convert.ToInt16(dr["Count"]);
                // dr.Field<int>("Count");
                total += c;
            }

            TotalLabelTOTAL.Text = total.ToString();
        }

    }
}