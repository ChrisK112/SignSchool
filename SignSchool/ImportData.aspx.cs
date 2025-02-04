using DocumentFormat.OpenXml.Packaging;
using ICSharpCode.SharpZipLib.Zip;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
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
    public partial class ImportData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hideImportButton();
        }

        protected void BtnUpload_Click(object sender, EventArgs e)
        {
            string datetime = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");
            string user = HttpContext.Current.User.Identity.Name;
            string fullFilename = "E:/TBWeb/SignSchool/SignSchool/DataFiles/" + datetime + "_" + user + "_" + DataUpload.FileName;
            string reportFilenameImported = "E:/TBWeb/SignSchool/SignSchool/DataFiles/" + datetime + "_" + user + "_reportImported.xlsx";
            string reportFilenameNotImported = "E:/TBWeb/SignSchool/SignSchool/DataFiles/" + datetime + "_" + user + "_reportNotImported.xlsx";
            string reportZipFilename = "E:/TBWeb/SignSchool/SignSchool/DataFiles/" + datetime + "_" + user + "_ImportReport";

            if (DataUpload.HasFile)
            {
                //save cache of filename
                string filenameid = user + "filename";
                string reportfileimid = user + "reportimported";
                string reportfilenimid = user + "reportnotimported";
                string reportzipid = user + "reportzip";

                HttpContext.Current.Cache.Remove(filenameid);
                HttpContext.Current.Cache[filenameid] = DataUpload.FileName;

                HttpContext.Current.Cache.Remove(reportfileimid);
                HttpContext.Current.Cache[reportfileimid] = reportFilenameImported;

                HttpContext.Current.Cache.Remove(reportfilenimid);
                HttpContext.Current.Cache[reportfilenimid] = reportFilenameNotImported;

                HttpContext.Current.Cache.Remove(reportzipid);
                HttpContext.Current.Cache[reportzipid] = reportZipFilename;

                string extension = System.IO.Path.GetExtension(DataUpload.FileName);
                if (extension.Equals(".xlsx") || extension.Equals(".xls") || extension.Equals(".csv"))
                {
                    DataUpload.SaveAs(fullFilename);
                    lblMessage.Text = "File Successfully Uploaded";
                                        
                    DataTable dt = ExtractData(fullFilename, extension);

                    if (CheckDataHeaders(dt))
                    {
                        PreviewData(dt, reportFilenameNotImported, reportFilenameImported);
                        //WriteExcelFile(reportFilenameNotImported, dt); this is done in PreviewData()
                        //WriteExcelFile(reportFilenameImported, dt);
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

            correctFormat.Columns.Add("ConsID");
            correctFormat.Columns.Add("Constituent Import ID");
            correctFormat.Columns.Add("Title");
            correctFormat.Columns.Add("Firstname");
            correctFormat.Columns.Add("Surname");
            correctFormat.Columns.Add("Preferred Address Line 1");
            correctFormat.Columns.Add("Preferred Address Line 2");
            correctFormat.Columns.Add("Preferred City");
            correctFormat.Columns.Add("Preferred Postcode");
            correctFormat.Columns.Add("Email");
            correctFormat.Columns.Add("Appeal ID");
            correctFormat.Columns.Add("Gift Status");
            correctFormat.Columns.Add("Registration date");
            correctFormat.Columns.Add("First fulfilment date");
            correctFormat.Columns.Add("Regular fulfilment day");
            correctFormat.Columns.Add("Gift Added By");

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
            string chid = user + "importtable";
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
                    using (var cmd = new SqlCommand("dbo.ImportNewDataRow", con))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;

                        //convert types

                        string Constituent_ID = Convert.ToString(dr["ConsID"]);
                        string Constituent_Import_ID = Convert.ToString(dr["Constituent Import ID"]);
                        string Title_1 = Convert.ToString(dr["Title"]);
                        string First_Name = Convert.ToString(dr["Firstname"]);
                        string Surname = Convert.ToString(dr["Surname"]);
                        string Preferred_Address_Line_1 = Convert.ToString(dr["Preferred Address Line 1"]);
                        string Preferred_Address_Line_2 = Convert.ToString(dr["Preferred Address Line 2"]);
                        string Preferred_City = Convert.ToString(dr["Preferred City"]);
                        string Preferred_Postcode = Convert.ToString(dr["Preferred Postcode"]);
                        string Email_Number = Convert.ToString(dr["Email"]);
                        string Appeal_ID = Convert.ToString(dr["Appeal ID"]);
                        string Gift_Status = Convert.ToString(dr["Gift Status"]);
                        DateTime Registration_date = Convert.ToDateTime(dr["Registration date"]);
                        DateTime First_fulfilment_date = Convert.ToDateTime(dr["First fulfilment date"]);
                        string Regular_fulfilment_day = Convert.ToString(dr["Regular fulfilment day"]);
                        string Gift_Added_By = Convert.ToString(dr["Gift Added By"]);

                        cmd.Parameters.Add("@Constituent_ID", SqlDbType.NVarChar, 50).Value = Constituent_ID;
                        cmd.Parameters.Add("@Constituent_Import_ID", SqlDbType.NVarChar, 50).Value = Constituent_Import_ID;
                        cmd.Parameters.Add("@Title_1", SqlDbType.NVarChar, 10).Value = Title_1;
                        cmd.Parameters.Add("@First_Name", SqlDbType.NVarChar, 50).Value = First_Name;
                        cmd.Parameters.Add("@Surname", SqlDbType.NVarChar, 50).Value = Surname;
                        cmd.Parameters.Add("@Preferred_Address_Line_1", SqlDbType.NVarChar, 50).Value = Preferred_Address_Line_1;
                        cmd.Parameters.Add("@Preferred_Address_Line_2", SqlDbType.NVarChar, 50).Value = Preferred_Address_Line_2;
                        cmd.Parameters.Add("@Preferred_City", SqlDbType.NVarChar, 50).Value = Preferred_City;
                        cmd.Parameters.Add("@Preferred_Postcode", SqlDbType.NVarChar, 50).Value = Preferred_Postcode;
                        cmd.Parameters.Add("@Email_Number", SqlDbType.NVarChar, 50).Value = Email_Number;
                        cmd.Parameters.Add("@Appeal_ID", SqlDbType.NVarChar, 50).Value = Appeal_ID;
                        cmd.Parameters.Add("@Gift_Status", SqlDbType.NVarChar, 50).Value = Gift_Status;
                        cmd.Parameters.Add("@Registration_date", SqlDbType.NVarChar, 50).Value = Registration_date.ToString("dd/MM/yyyy");
                        cmd.Parameters.Add("@First_fulfilment_date", SqlDbType.NVarChar, 50).Value = First_fulfilment_date.ToString("dd/MM/yyyy");
                        cmd.Parameters.Add("@Regular_fulfilment_day", SqlDbType.NVarChar, 60).Value = Regular_fulfilment_day;
                        cmd.Parameters.Add("@Gift_Added_By", SqlDbType.NVarChar, 50).Value = Gift_Added_By;

                        cmd.Parameters.Add("@Pack_Sent_Previously", SqlDbType.Int).Value = 0;
                        cmd.Parameters.Add("@Last_Date_Pack_Changed", SqlDbType.NVarChar, 50).Value = "";
                        cmd.Parameters.Add("@Pack_Change_User", SqlDbType.NVarChar, 50).Value = "";
                        cmd.Parameters.Add("@Date_Cancelled", SqlDbType.NVarChar, 50).Value = "";
                        cmd.Parameters.Add("@Cancellation_User", SqlDbType.NVarChar, 50).Value = "";
                        cmd.Parameters.Add("@Date_Restored", SqlDbType.NVarChar, 50).Value = "";
                        cmd.Parameters.Add("@Date_Edited", SqlDbType.NVarChar, 50).Value = "";
                        cmd.Parameters.Add("@Edit_User", SqlDbType.NVarChar, 50).Value = "";
                        cmd.Parameters.Add("@Date_Imported", SqlDbType.NVarChar, 50).Value = DateTime.Now.ToString("dd/MM/yyyy");
                        cmd.Parameters.Add("@Import_User", SqlDbType.NVarChar, 50).Value = HttpContext.Current.User.Identity.Name;
                        cmd.Parameters.Add("@Import_Filename", SqlDbType.NVarChar, 250).Value = filename;

                        cmd.ExecuteNonQuery();

                    }
                    con.Close();
                }
            }
            lblMessage.Text = "Data imported!";

        }

        protected void PreviewData(DataTable dt, string reportFilenameNotImported, string reportFilenameImported)
        {
            string user = HttpContext.Current.User.Identity.Name;
            string chid = user + "importtable";

            HttpContext.Current.Cache.Remove(chid);
            HttpContext.Current.Cache[chid] = dt;

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    checkForDuplicates(dt, reportFilenameNotImported, reportFilenameImported);
                    showImportButton();

                }
            }
        }

        protected void checkForDuplicates(DataTable dt, string reportFilenameNotImported, string reportFilenameImported)
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
                nr["ConsId"] = r["ConsID"];
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



            DataTable dupTable = dt.Clone(); //clone i.e no data
            DataTable importTable = dt.Copy(); //copy i.e. has data

            
            //GridView2.DataSource = dups;
            //GridView2.DataBind();
                       
            foreach (DataRow dr in dups.Rows)
            {                
                string consid = dr["ConsID"].ToString();

                foreach (DataRow row in importTable.Rows)
                {
                    string constitid = row["ConsID"].ToString();

                    if (constitid.Equals(consid)) 
                    { 
                        //delete if type is double
                        string type = dr["Type"].ToString();
                        
                        if (type.Equals("Double"))
                        {
                            //add row to dup table
                            dupTable.Rows.Add(row.ItemArray);
                            //remove from old table
                            row.Delete();
                            importTable.AcceptChanges();
                            DupLabel.Visible = true;
                        }
                        break;
                    }
                }
            }
            importTable.AcceptChanges();
            ImportLabel.Visible = true;

            //bind old grid and cache
            string user = HttpContext.Current.User.Identity.Name;
            string chid = user + "importtable";

            HttpContext.Current.Cache.Remove(chid);
            HttpContext.Current.Cache[chid] = importTable;

            GridView1.DataSource = importTable;
            GridView1.DataBind();

            GridView2.DataSource = dupTable;
            GridView2.DataBind();

            //set GridView 2 to red
            
            foreach (GridViewRow row in GridView2.Rows)
            {
                row.BackColor = Color.Red;
            }

            //set yellow colour for single dups
            foreach (DataRow dr in dups.Rows)
            {
                string consid = dr["ConsID"].ToString();

                foreach (GridViewRow row in GridView1.Rows)
                {
                    if (row.Cells[0].Text.ToString().Equals(consid))
                    {
                        //set highlight based on type
                        string type = dr["Type"].ToString();

                        if (type.Equals("Single"))
                        {
                            //set to yellow
                            row.BackColor = Color.LightYellow;
                        }
                        ImportDupLabel.Visible = true;

                        break;
                    }
                }
            }

            //find dups in import table itself
            Hashtable ht = new Hashtable();
            foreach (GridViewRow row in GridView1.Rows)
            {
                string combFields = row.Cells[0].Text.ToString() + row.Cells[12].Text.ToString();
                ;
                if (ht.ContainsKey(combFields))
                {
                    row.BackColor = Color.Orange;
                    ImportDupOnFileLabel.Visible = true;

                    GridView1.Rows[(int)ht[combFields]].BackColor = Color.Orange;

                    break;
                }
                else
                {
                    ht.Add(combFields, row.RowIndex);
                }
            }


            WriteExcelFile(reportFilenameNotImported, dupTable, "notimported");
            WriteExcelFile(reportFilenameImported, importTable, "imported");


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

        private void WriteExcelFile(string outputPath, DataTable table, string gridview)
        {
            ExcelWriter ew = new ExcelWriter(outputPath);

            ExcelWorksheet ws = ew.AddWorksheet("Sheet1");
            ws.DefaultColWidth = 16;
            ws.DefaultRowHeight = 15;
            ew.SetValues(ws, table);
            GridView gw = (gridview.Equals("imported")) ? GridView1 : GridView2;

            int i = 1; //header row
            
            foreach (GridViewRow r in gw.Rows)
            {
                i++;
                ExcelRange erTemp = ws.Cells["A" + i + ":P" + i];
                erTemp.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                erTemp.Style.Fill.BackgroundColor.SetColor(r.BackColor);
                if (r.BackColor.IsEmpty) erTemp.Style.Fill.BackgroundColor.SetColor(Color.White);
                erTemp.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            }


            //header row style
            ExcelRange erH = ws.Cells["A1:P1"];
            erH.Style.Font.Bold = true;
            erH.Style.Border.BorderAround(ExcelBorderStyle.Medium);

            //column borders
            int columns = 16;
            int rows = gw.Rows.Count + 1;
            for (int column = 1; column <= columns; column++)
            {
                ExcelRange erTemp = ws.Cells[1, column, rows, column]; //start row, start column, end row, end column
                erTemp.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                erTemp.Style.WrapText = true;
            }


            //add info rows
            ws.InsertRow(1, 3);

            string red = "Red: These records are duplicates found on the database.";
            string yl = "Light Yellow: These records are duplicate URNs, but different Registration Date.";
            string or = "Orange: These records are duplicates on the file.";

            ws.Cells[1, 1].Value = (gridview.Equals("imported")) ? yl:red;
            if (gridview.Equals("imported")) ws.Cells[2, 1].Value = or;

            //merge info rows
            ExcelRange er1 = ws.Cells["A1:P1"];
            ExcelRange er2 = ws.Cells["A2:P2"];
            ExcelRange er3 = ws.Cells["A3:P3"];
            ExcelRange erAll = ws.Cells["A1:P3"]; //for easier styling
            er1.Merge = true;
            er2.Merge = true;
            er3.Merge = true;
            erAll.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            erAll.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            erAll.Style.Fill.BackgroundColor.SetColor(Color.White);


            ew.Save();
        }

        protected void DownloadReportButton_Click(object sender, EventArgs e)
        {
            string user = HttpContext.Current.User.Identity.Name;
            string filenameImported = HttpContext.Current.Cache[user + "reportimported"].ToString();
            string filenameNotImported = HttpContext.Current.Cache[user + "reportnotimported"].ToString();
            string filenameZip = HttpContext.Current.Cache[user + "reportzip"].ToString();
            
            //zip

            //create + send ZIP as DL
            string[] filesToZip = new string[3];
            string zipFile = filenameZip;
            filesToZip[1] = filenameImported;
            filesToZip[2] = filenameNotImported;
            ZipFiles(filesToZip, zipFile);

            //Delete all but zip file
            deleteExcessDocs(filesToZip);

            //Send DL
            DownloadZip(filenameZip + ".zip");
        }

        private void ZipFiles(string[] files, string zipfile)
        {
            ZipOutputStream zip = new ZipOutputStream(File.Create(zipfile + ".zip"));
            zip.SetLevel(9);

            for (int i = 1; i < files.Length; i++) //start at 1, since we need first file (0)
            {
                AddFileToZip(zip, zipfile, files[i]);
            }
            zip.Finish();
            zip.Close();
        }

        private static void AddFileToZip(ZipOutputStream zStream, string relativePath, string file)
        {
            byte[] buffer = new byte[4096];

            //the relative path is added to the file in order to place the file within
            //this directory in the zip
            string fileRelativePath = Path.GetFileName(file);

            ZipEntry entry = new ZipEntry(fileRelativePath);
            entry.DateTime = DateTime.Now;
            zStream.PutNextEntry(entry);

            using (FileStream fs = File.OpenRead(file))
            {
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }

        private void deleteExcessDocs(string[] filepaths)
        {
            for (int i = 1; i < filepaths.Length; i++) //start at 1, since we need first file (0)
            {
                File.Delete(filepaths[i]);
            }
        }

        private void DownloadZip(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/x-zip-compressed";
                Response.Flush();
                Response.TransmitFile(file.FullName);
                Response.End();
            }
        }

    }
}