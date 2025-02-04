using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignSchool
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string user = System.Web.HttpContext.Current.User.Identity.Name;
            ShowAdminUserContent(user);
        }

        protected void UserConfigButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserConfiguration.aspx", true);
        }

        protected void ShowAdminUserContent(string user)
        {
            bool admin = GetAdminStatus(user);
            if (admin) //enable admin features
            {
                UserConfigButton.Style.Remove("display");
            }
            else
            {

            }

        }

        protected bool GetAdminStatus(string user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
                using (SqlCommand command = new SqlCommand("select adminuser from Users where username =@username", connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@username", user);

                    return (bool)command.ExecuteScalar();

                }
            }
            catch (Exception ex)
            {
                // Handle any exception.
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }
        }

    }
}