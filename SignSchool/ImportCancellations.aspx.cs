using DocumentFormat.OpenXml.Packaging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignSchool
{
    public partial class ImportCancellations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hideImportButton();
        }

        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            string datetime = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");
            string user = HttpContext.Current.User.Identity.Name;
            string fullFilename = "E:/TBWeb/SignSchool/SignSchool/CancellationFiles/" + datetime + "_" + user + "_" + DataUpload.FileName;
            string reportFilename = "E:/TBWeb/SignSchool/SignSchool/CancellationFiles/" + datetime + "_" + user + "_CancReport.xlsx";

            if (DataUpload.HasFile)
            {
                //save cache of filename
                string filenameid = user + "filename";
                string reportfileid = user + "report";

                HttpContext.Current.Cache.Remove(filenameid);
                HttpContext.Current.Cache[filenameid] = DataUpload.FileName;

                HttpContext.Current.Cache.Remove(reportfileid);
                HttpContext.Current.Cache[reportfileid] = reportFilename;

                string extension = System.IO.Path.GetExtension(DataUpload.FileName);
                if (extension.Equals(".xlsx") || extension.Equals(".xls") || extension.Equals(".csv"))
                {
                    DataUpload.SaveAs(fullFilename);
                    lblMessage.Text = "File Successfully Uploaded";
                                        
                    DataTable dt = ExtractData(fullFilename, extension);

                    if (CheckDataHeaders(dt))
                    {
                        PreviewData(dt);
                        WriteExcelFile(reportFilename, dt);
                    }
                    else
                    {
                        lblMessage.Text = "File Not In Correct Format";
                    }

                }
                else lblMessage.Text = "Unsupported file type!";


            }
            else lblMessage.Text = "No File Selected!";

        }

        protected bool CheckDataHeaders(DataTable dt)
        {

            DataTable correctFormat = new DataTable();

            correctFormat.Columns.Add("Constituent ID");
            correctFormat.Columns.Add("Constituent Import ID");
            correctFormat.Columns.Add("Title 1");
            correctFormat.Columns.Add("First Name");
            correctFormat.Columns.Add("Surname");
            correctFormat.Columns.Add("Preferred Address Line 1");
            correctFormat.Columns.Add("Preferred Address Line 2");
            correctFormat.Columns.Add("Preferred City");
            correctFormat.Columns.Add("Preferred Postcode");
            correctFormat.Columns.Add("Email Number");
            correctFormat.Columns.Add("Appeal ID");
            correctFormat.Columns.Add("Gift Status");
            correctFormat.Columns.Add("Registration Date");
            correctFormat.Columns.Add("First Fulfilment date");
            correctFormat.Columns.Add("Regular fulfilment day");
            correctFormat.Columns.Add("Gift Status Date");
            correctFormat.Columns.Add("Last Instalment/Payment Amount");
            correctFormat.Columns.Add("Latest Payment Amount");

            int i = 0;
            foreach(DataColumn odc in correctFormat.Columns)
            {
                if (!odc.ColumnName.Equals(dt.Columns[i].ColumnName)) return false;
                i++;
            }

            return true;
        }


        protected DataTable ExtractData(string file, string type)
        {
            if (type.Equals(".csv")) return ExtractCSV(file);
            else return ExtractExcel(file);
        }

        protected DataTable ExtractExcel(string file)
        {
            // string sqlquery= "Select * From [SheetName$] Where YourCondition";

            DataSet ds = new DataSet();
            string sheetName = GetSheetName(file);
            string sqlquery = "Select * From [" + sheetName + "]";
            string constring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + file + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=NO;\""; //header no to force string datatype
            OleDbConnection con = new OleDbConnection(constring + "");
            OleDbDataAdapter da = new OleDbDataAdapter(sqlquery, con);
            da.Fill(ds);
            DataTable dt = ds.Tables[0];

            //remove header row since we imported it as well

            //first set column names (since we have no columns names currently)
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dt.Columns[i].ColumnName = dt.Rows[0][i].ToString();
            }


            //remove header row
            dt.Rows.RemoveAt(0);
            dt.AcceptChanges();

            return dt;
        }

        protected DataTable ExtractCSV(string file)
        {
            // string sqlquery= "Select * From [SheetName$] Where YourCondition";
            
            DataSet ds = new DataSet();
            string sheetName = GetSheetName(file);
            string sqlquery = "Select * From [" + sheetName + "]";
            string constring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + file + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
            OleDbConnection con = new OleDbConnection(constring + "");
            OleDbDataAdapter da = new OleDbDataAdapter(sqlquery, con);
            da.Fill(ds);
            DataTable dt = ds.Tables[0];

            return dt;
        }

        protected string GetSheetName(string file)
        {
            OleDbConnection oconn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+ file+"; Extended Properties=Excel 12.0;Persist Security Info=False;");

            oconn.Open();
            DataTable dbSchema = oconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dbSchema == null || dbSchema.Rows.Count < 1)
            {
                throw new Exception("Error: Could not determine the name of the first worksheet.");
            }
            string firstSheetName = dbSchema.Rows[0]["TABLE_NAME"].ToString();

            oconn.Close();

            return firstSheetName;
        }



        protected void ImportDataButton_Click(object sender, EventArgs e)
        {
            //todo

            string user = HttpContext.Current.User.Identity.Name;
            string chid = user + "cancelimporttable";
            DataTable dt = HttpContext.Current.Cache[chid] as DataTable;

            if (dt != null && CheckDataHeaders(dt)) ImportFileToDB(dt);
            else lblMessage.Text = "Something went wrong!";

        }

        protected void ImportFileToDB(DataTable dt)
        {
            lblMessage.Text = "Importing Data...";

            string user = HttpContext.Current.User.Identity.Name;
            string filenameid = user + "filename";

            string filename = HttpContext.Current.Cache[filenameid].ToString();

            //loop through each row and call sql qr to add values
            foreach (DataRow dr in dt.Rows)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("dbo.ImportCancelDataRow", con))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;

                        //convert types

                        string Constituent_ID = Convert.ToString(dr["Constituent ID"]);
                        string Gift_Status = Convert.ToString(dr["Gift Status"]);
                        DateTime Registration_date = Convert.ToDateTime(dr["Registration date"]);
                        
                        cmd.Parameters.Add("@Constituent_ID", SqlDbType.NVarChar, 50).Value = Constituent_ID;
                        cmd.Parameters.Add("@Registration_date", SqlDbType.NVarChar, 50).Value = Registration_date.ToString("dd/MM/yyyy");

                        cmd.Parameters.Add("@Gift_Status", SqlDbType.NVarChar, 50).Value = Gift_Status;
                        cmd.Parameters.Add("@Cancellation_User", SqlDbType.NVarChar, 50).Value = HttpContext.Current.User.Identity.Name;
                        cmd.Parameters.Add("@Date_Cancelled", SqlDbType.NVarChar, 50).Value = DateTime.Now.ToString("dd/MM/yyyy");
                        cmd.Parameters.Add("@Cancellation_Import_Filename", SqlDbType.NVarChar, 250).Value = filename;

                        cmd.ExecuteNonQuery();

                    }
                    con.Close();
                }
            }
            lblMessage.Text = "Cancellation Data Applied!";

        }

        protected void PreviewData(DataTable dt)
        {
            string user = HttpContext.Current.User.Identity.Name;
            string chid = user + "cancelimporttable";

            HttpContext.Current.Cache.Remove(chid);
            HttpContext.Current.Cache[chid] = dt;

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    checkForDuplicates(dt);
                    showImportButton();

                }
            }
        }

        protected void checkForDuplicates(DataTable dt)
        {
            //send over consid column and registration date column, into temp table

            //create datatable needed
            DataTable dupcheck = new DataTable();

            dupcheck.Columns.Add("ConsId");
            dupcheck.Columns.Add("RegDate");

            //fill dupcheck
            foreach(DataRow r in dt.Rows)
            {
                DataRow nr = dupcheck.NewRow();
                nr["ConsId"] = r["Constituent ID"];
                //nr["RegDate"] = r["Registration date"];
                string reg = DateTime.Parse((r["Registration date"].ToString())).ToString("dd/MM/yyyy");
                nr["RegDate"] = reg;

                dupcheck.Rows.Add(nr);

            }
            dupcheck.AcceptChanges();

            DataTable dups = new DataTable();


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString))
            {
                using (var cmd = new SqlCommand("dbo.GetDupsAndType", con))
                {
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter para;
                    para = cmd.Parameters.AddWithValue("@dupTable", dupcheck);
                    para.SqlDbType = SqlDbType.Structured;
                    para.TypeName = "dbo.DupCheckType";


                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    con.Open();
                    da.Fill(dups);
                    con.Close();
                    da.Dispose();

                }
            }

            foreach (GridViewRow row in GridView1.Rows)
            {
                string type = "";
                foreach (DataRow dr in dups.Rows)
                {
                    
                    type = "";
                    string consid = dr["ConsID"].ToString();
                    //System.Diagnostics.Debug.WriteLine(row.Cells[0].Text.ToString() + "  " + consid);
                    if (row.Cells[0].Text.ToString().Equals(consid))
                    {
                        type = dr["Type"].ToString();
                        break;

                    }

                }
                if (type.Equals("Double"))
                {
                    continue;
                }
                if (type.Equals("Cancelled"))
                {
                    row.BackColor = Color.LightGreen;
                    CancLabelGR.Visible = true;
                    continue;
                }
                if (type.Equals("12Pack"))
                {
                    row.BackColor = Color.LightYellow;
                    CancLabelLY.Visible = true;
                    continue;
                }
                if (type.Equals("Single"))
                {
                    row.BackColor = Color.LightBlue;
                    CancLabelLB.Visible = true;
                    continue;
                }
                row.BackColor = Color.Red;
                CancLabelRED.Visible = true;

            }

        }

        private void showImportButton()
        {
            ImportDataButton.Style.Remove("display");
            DownloadReportButton.Style.Remove("display");
        }

        private void hideImportButton()
        {
            ImportDataButton.Style.Add("display", "none");
            //DownloadReportButton.Style.Add("display", "none");
        }

        private void WriteExcelFile(string outputPath, DataTable table)
        {
            ExcelWriter ew = new ExcelWriter(outputPath);

            ExcelWorksheet ws = ew.AddWorksheet("Sheet1");
            ws.DefaultColWidth = 14;
            ws.DefaultRowHeight = 15;
            ew.SetValues(ws, table);

            int i = 1; //header row
            GridView gw = GridView1;
            foreach(GridViewRow r in gw.Rows)
            {
                i++;
                ExcelRange erTemp = ws.Cells["A"+i+":R"+i];
                erTemp.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                erTemp.Style.Fill.BackgroundColor.SetColor(r.BackColor);
                if (r.BackColor.IsEmpty) erTemp.Style.Fill.BackgroundColor.SetColor(Color.White);
                erTemp.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            }


            //header row style
            ExcelRange erH = ws.Cells["A1:R1"];
            erH.Style.Font.Bold = true;
            erH.Style.Border.BorderAround(ExcelBorderStyle.Medium);

            //column borders
            int columns = 18;
            int rows = gw.Rows.Count+1;
            for(int column = 1; column <= columns; column++)
            {
                ExcelRange erTemp = ws.Cells[1, column, rows, column]; //start row, start column, end row, end column
                erTemp.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                erTemp.Style.WrapText = true;
            }


            //add info rows
            ws.InsertRow(1, 5);
            
       
            ws.Cells[1, 1].Value = "Red: These records could not be matched to the live or 12 pack database.";
            ws.Cells[2, 1].Value = "Light Yellow: These records are on the 12 pack database.";
            ws.Cells[3, 1].Value = "Light Green: These records were already cancelled.";
            ws.Cells[4, 1].Value = "Light Blue: These records match through URN, but not registration date.";

            //merge info rows
            ExcelRange er1 = ws.Cells["A1:R1"];
            ExcelRange er2 = ws.Cells["A2:R2"];
            ExcelRange er3 = ws.Cells["A3:R3"];
            ExcelRange er4 = ws.Cells["A4:R4"];
            ExcelRange er5 = ws.Cells["A5:R5"];
            ExcelRange erAll = ws.Cells["A1:R5"]; //for easier styling
            er1.Merge = true;
            er2.Merge = true;
            er3.Merge = true;
            er4.Merge = true;
            er5.Merge = true;
            erAll.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            erAll.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            erAll.Style.Fill.BackgroundColor.SetColor(Color.White);


            ew.Save();
        }

        protected void DownloadReportButton_Click(object sender, EventArgs e)
        {
            string user = HttpContext.Current.User.Identity.Name;
            string filename = HttpContext.Current.Cache[user + "report"].ToString();
            DownloadReport(filename);
        }

        private void DownloadReport(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.TransmitFile(file.FullName);
                Response.End();
            }
        }

    }
}