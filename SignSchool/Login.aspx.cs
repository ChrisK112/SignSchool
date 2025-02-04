using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient; //database prvd
using System.Configuration;
using System.Data;
using System.Security.Cryptography;

namespace SignSchool
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["SENDEMAILS"] == null)
                {
                    Login1.Focus(); 
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["SENDEMAILS"].ToString()))
                {
                    if (Request.QueryString["SENDEMAILS"].ToString().Equals("yes846ac45fe18364dda48b87d719f3d52d"))
                    {
                        System.Diagnostics.Debug.WriteLine("in email send");
                        EmailSend se = new EmailSend();
                        se.SendEmail();
                    }
                }
                else
                {
                    Login1.Focus();
                }
            }
        }


        protected void OnAuthenticate(object sender, AuthenticateEventArgs e)
        {
            bool Authenticated = false;
            Authenticated = SiteSpecificAuthenticationMethod(Login1.UserName, Login1.Password);

            e.Authenticated = Authenticated;
        }

        private bool SiteSpecificAuthenticationMethod(string UserName, string Password)
        {

            //get salt
            string salt = GetSalt(UserName);

            if (String.IsNullOrEmpty(salt)) return false; //stop if no salt ie no username found

            byte[] saltb = Convert.FromBase64String(salt);

            // Generate the hash
            int hashsize = 64; //size used is 64
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(Password, saltb, 10000);  //10000 iterations used
            byte[] hashb = pbkdf2.GetBytes(hashsize);

            string hash = Convert.ToBase64String(hashb);

            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("select * from Users where username =@username and password=@password and salt=@salt and isDisabled=0", con))
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@username", UserName);
                    cmd.Parameters.AddWithValue("@password", hash);
                    cmd.Parameters.AddWithValue("@salt", salt);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    

                    da.Fill(dt);

                }
                con.Close();
            }
           
            if (dt.Rows.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("update Users set LastLogin = @date where username =@username", con))
                    {
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("@username", UserName);
                        cmd.Parameters.Add("@date", SqlDbType.DateTime, 50).Value = DateTime.Now;

                        cmd.ExecuteNonQuery();

                    }
                    con.Close();
                }
                return true;
            }
            return false;



        }

        private string GetSalt(string Username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
                using (SqlCommand command = new SqlCommand("select salt from Users where username =@username", connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@username", Username);

                    return (string)command.ExecuteScalar();

                }
            }
            catch (Exception ex)
            {
                // Handle any exception.
                return ex.ToString();
            }
        }


    }
}