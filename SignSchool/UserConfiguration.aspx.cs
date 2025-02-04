using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignSchool
{
    public partial class UserConfiguration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckIfAdmin(HttpContext.Current.User.Identity.Name)) Response.Redirect("https://client.telebank-online.com/SignSchool/Home.aspx");
            HideForms();
        }

        private bool CheckIfAdmin(string user)
        {
            if(user.Equals("")) return false;

            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("select top 1 * from [dbo].[Users] WHERE [username] = '" + user + "' AND adminuser = 1", con))
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
                    return true;
                }
            }
            return false;
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideForms();
            //System.Diagnostics.Debug.WriteLine(DropDownList1.SelectedValue);
            switch (DropDownList1.SelectedValue)
            {
                case "0": //select
                    HideForms();
                    break;
                case "1": //new user
                    ShowNewUserForms();
                    break;
                case "2": //edit user
                    ShowEditUserListForms();
                    break;
            }
        }

        private void ShowEditUserListForms()
        {
            GridView_ViewUsers.Enabled = true;
            GridView_ViewUsers.Visible = true;
            FillGrid();
            
        }

        private void ShowEditUserForm(string id)
        {
            HideForms();

            DataEditTable.Enabled = true;
            DataEditTable.Visible = true;
            FillData(id);
        }

        private void ShowNewUserForms()
        {
            NewUserCreation.Visible = true;
            NewUserCreation.Enabled = true;
        }

        private void HideForms()
        {
            NewUserCreation.Visible = false;
            NewUserCreation.Enabled = false;

            DataEditTable.Visible = false;
            DataEditTable.Enabled = false;

            GridView_ViewUsers.Visible = false;
            GridView_ViewUsers.Enabled = false;

        }


        protected void CreateNewUserButton_Click(object sender, EventArgs e)
        {

            string un = UsernameTextBox.Text;
            string p1 = Password1TextBox.Text;
            string p2 = Password2TextBox.Text;
            string em = EmailTextBox.Text;
            bool ad = AdminRightsCheckbox.Checked;

            //check if all fields are filled in
            if (!CheckFieldsRequired(un, p1, p2, em)) return;

            //compare p1 and p2
            if (!p1.Equals(p2))
            {
                ProgressLabel.Text = "Passwords do not match!";
                return;
            }

            //check for existing users
            if (CheckExistingUser(un))
            {
                ProgressLabel.Text = "User already exists!";
                return;
            }

            //create user
            bool created = CreateNewUser(un, p1, em, ad);
            HideForms();

            //refresh page
            if(created)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('New user added')", true);
                return;
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Erorr adding user')", true);
            return;

        }

        private bool CreateNewUser(string un, string p1, string em, bool ad)
        {
            bool toReturn = false;

            int bit = 0;
            if (ad) bit = 1;

            //create salt
            byte[] salt = new byte[64];
            salt = CreateSalt();

            //create password hash
            byte[] phash = new byte[64];
            phash = CreatePassHash(salt, p1);

            string saltS = Convert.ToBase64String(salt);
            string passS = Convert.ToBase64String(phash);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("dbo.CreateUser", con))
                {
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    //objcmd.Parameters.Add("@Q20_C8", SqlDbType.NVarChar, 1).Value = checkboxvalue(cbx_q20_8)
                    //objcmd.Parameters.Add("@Q20_T1", SqlDbType.NVarChar, 500).Value = tbx_q20_1.Text
                    //cmd.Parameters.Add(new SqlParameter("@username", un));
                    //cmd.Parameters["@username"].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 20).Value = un;
                    cmd.Parameters.Add("@passwordHash", SqlDbType.NVarChar, 88).Value = passS;
                    cmd.Parameters.Add("@salt", SqlDbType.NVarChar, 88).Value = saltS;
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = em;
                    cmd.Parameters.Add("@admin", SqlDbType.Bit, 1).Value = ad;

                    cmd.ExecuteNonQuery();
                    toReturn = true;

                }
                con.Close();
            }
            return toReturn;
        }

        private byte[] CreatePassHash(byte[] salt, string pass)
        {
            // Generate the hash
            int hashsize = 64; //size used is 64
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(pass, salt, 10000);  //10000 iterations used
            byte[] hashb = pbkdf2.GetBytes(hashsize);

            //string hash = Convert.ToBase64String(hashb);
            return hashb;
        }

        private byte[] CreateSalt()
        {

            byte[] saltb = new byte[64];
            using (RNGCryptoServiceProvider rngCsp = new
            RNGCryptoServiceProvider())
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(saltb);
            }

            //string saltC = Convert.ToBase64String(saltb);
            return saltb;
        }

        private bool CheckFieldsRequired(string un, string p1, string p2, string em)
        {
            if (un.Equals(""))
            {
                ProgressLabel.Text = "Username is blank!";
                return false;
            }
            if (p1.Equals(""))
            {
                ProgressLabel.Text = "Password is blank!";
                return false;
            }
            if (p2.Equals(""))
            {
                ProgressLabel.Text = "Confirm Password is blank!";
                return false;
            }
            if (em.Equals(""))
            {
                ProgressLabel.Text = "Email is blank!";
                return false;
            }
            return true;
        }

        private bool CheckExistingUser(string user)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
                {

                    connection.Open();

                    SqlCommand cmd = new SqlCommand("select * from Users where username =@username", connection);
                    cmd.Parameters.AddWithValue("@username", user);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    da.Fill(dt);
                    connection.Close();
                    if (dt.Rows.Count > 0) return true;
                    return false;

                }
            }
            catch (Exception ex)
            {
                // Handle any exception.
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }

        }


        protected void GridView_ViewUsers_EditUser(object sender, GridViewEditEventArgs e)
        {
            int itemIndex = e.NewEditIndex;
            GridViewRow row = GridView_ViewUsers.Rows[itemIndex];

            TableCell cell = row.Cells[1]; //first cell is edit button, id is second (hopefully)
            string id = cell.Text;
            e.NewEditIndex = -1;
            ShowEditUserForm(id);
            

        }

        protected void FillGrid()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("select * from [dbo].[UsersView]", con))
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
                    GridView_ViewUsers.DataSource = dataTable;
                    GridView_ViewUsers.DataBind();
                }

            }

        }

        private void FillData(string id)
        {
            //get data
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("select top 1 * from [dbo].[UsersView] WHERE [userid] = '" + id + "'", con))
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

            //fill data in textboxes + checkbox
            //should only loop once
            foreach (DataRow dr in dataTable.Rows)
            {
                UserId_txt.Text = dr.Field<int>("UserId").ToString();
                Username_txt.Text = dr.Field<string>("Username");
                Email_txt.Text = dr.Field<string>("Email");
                SendEmailOption_drp.SelectedValue = dr.Field<string>("SendEmail");
                string usertype = dr.Field<string>("UserType");
                //if (usertype.Equals("Admin")) Admin_cbx.Checked = true;
                Admin_cbx.Checked = usertype.Equals("Admin") ? true:false;
                string userstatus = dr.Field<string>("UserStatus");
                //if (userstatus.Equals("Live")) Live_cbx.Checked = true;
                Live_cbx.Checked = (userstatus.Equals("Live")) ? true : false;
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {

            //get data strings
            string userid = UserId_txt.Text;
            string email = Email_txt.Text;
            string sendemail = SendEmailOption_drp.SelectedValue;
            bool admin = Admin_cbx.Checked;
            bool disabled = !Live_cbx.Checked;
            string newpass = NewPassword_txt.Text;

            if (newpass.Equals(""))
            {
                ChangeUserDetails(userid, email, sendemail, admin, disabled);
            }
            else
            {
                ChangeUserDetails(userid, email, sendemail, admin, disabled);
                ChangeUserPassword(userid, newpass);
            }

            HideForms();
            ShowEditUserListForms();
        }

        private void ChangeUserDetails(string userid, string email, string sendemail, bool admin, bool disabled)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("dbo.UpdateUser", con))
                {
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;

                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = email;
                    cmd.Parameters.Add("@sendemail", SqlDbType.NVarChar, 2).Value = sendemail;
                    cmd.Parameters.Add("@admin", SqlDbType.Bit).Value = admin;
                    cmd.Parameters.Add("@disabled", SqlDbType.Bit).Value = disabled;

                    cmd.ExecuteNonQuery();

                }
                con.Close();
                
            }
        }

        private void ChangeUserPassword(string userid, string newpass)
        {

            //create salt
            byte[] salt = new byte[64];
            salt = CreateSalt();

            //create password hash
            byte[] phash = new byte[64];
            phash = CreatePassHash(salt, newpass);

            string saltS = Convert.ToBase64String(salt);
            string passS = Convert.ToBase64String(phash);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand("dbo.UpdateUserPassword", con))
                {
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userid;

                    cmd.Parameters.Add("@newpass", SqlDbType.NVarChar, 88).Value = passS;
                    cmd.Parameters.Add("@newsalt", SqlDbType.NVarChar, 88).Value = saltS;

                    cmd.ExecuteNonQuery();

                }
                con.Close();

            }
        }



    }
}