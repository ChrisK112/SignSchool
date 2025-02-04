using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;

namespace SignSchool
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Refresh();
        }

        protected void Refresh()
        {
            int count = GetLabelCount();
            CountLabel.Text = "Current labels for print: " + count;
            if (count > 0) DLLabels.Enabled = true;
            else DLLabels.Enabled = false;
        }

        protected void RefreshButton_Click(object sender, EventArgs e)
        {
            Refresh();
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

        protected void DLLabels_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            for(int i = 1; i <= 8; i++)
            {
                dt.Columns.Add("T" + "_" + i);
                dt.Columns.Add("FN" + "_" + i);
                dt.Columns.Add("LN" + "_" + i);
                dt.Columns.Add("A1" + "_" + i);
                dt.Columns.Add("A2" + "_" + i);
                dt.Columns.Add("A3" + "_" + i);
                dt.Columns.Add("C" + "_" + i);
                dt.Columns.Add("PC" + "_" + i);
                dt.Columns.Add("Pack" + "_" + i);
            }

            //get data here
            DataTable dataTable = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SenseSignSchoolConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select * from [dbo].[LabelView] order by [Pack Needed]", con);
            con.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            con.Close();
            da.Dispose();

            if(dataTable.Rows.Count > 0)
            {
                CreateAndSendFiles(dataTable, dt);
            }
            else
            {
                Refresh();
            }

            

        }

        private void CreateAndSendFiles(DataTable originalData, DataTable newData)
        {
            //filenames
            string baseFilePath = "E:/TBWeb/SignSchool/SignSchool/LabelFile/Merged/SenseSignSchool_";
            string labelDocFile = "E:/TBWeb/SignSchool/SignSchool/LabelFile/SecureMail Sign School Labels.docx";

            //EXCEL FILE

            //get datetime
            string datetime = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");

            //create excel file
            string excelfilename = (baseFilePath + datetime + ".xlsx");
            WriteExcelFile(excelfilename, originalData);

            //WORD FILE

            //get pages
            int LABELS_PER_PAGE = 8;
            int pageCount = (originalData.Rows.Count + LABELS_PER_PAGE - 1) / LABELS_PER_PAGE;

            for (int j = 1; j <= pageCount; j++) //pages i.e. ROWS for word template
            {
                DataRow nr = newData.NewRow();
                int counter = 1;
                foreach (DataRow datarow in originalData.Rows)
                {

                    //setup
                    /*
                    string title = datarow.Field<string>("Title 1"); 
                    string firstname = datarow.Field<string>("First Name");
                    if (!title.Equals("")) title = title + " ";
                    if (!firstname.Equals("")) firstname = firstname + " ";
                    string fullname = title + firstname + datarow.Field<string>("Surname");
                    */
                    int rowNum = 5;
                    string[] rows = new string[rowNum];
                    rows[0] = datarow.Field<string>("Preferred Address Line 1");
                    rows[1] = datarow.Field<string>("Preferred Address Line 2");
                    rows[2] = datarow.Field<string>("Preferred Address Line 3");
                    rows[3] = datarow.Field<string>("Preferred City");
                    rows[4] = datarow.Field<string>("Preferred Postcode");

                    List<string> rList = new List<string>();
                    for (int i = 0; i < rowNum; i++)
                    {
                        if (rows[i] == null) continue;
                        if (rows[i].Equals("")) continue;
                        rList.Add(rows[i].ToString());
                    }
                    string[] newRows = new string[rowNum];
                    for (int i = 0; i < rowNum; i++)
                    {
                        if (rList.ElementAtOrDefault(i) != null)
                        {
                            newRows[i] = rList[i];
                            continue;
                        }
                        newRows[i] = "";
                    }

                    nr["A1" + "_" + counter] = newRows[0];
                    nr["A2" + "_" + counter] = newRows[1];
                    nr["A3" + "_" + counter] = newRows[2];
                    nr["C" + "_" + counter] = newRows[3];
                    nr["PC" + "_" + counter] = newRows[4];

                    nr["T" + "_" + counter] = datarow.Field<string>("Title 1");
                    nr["FN" + "_" + counter] =  datarow.Field<string>("First Name");
                    nr["LN" + "_" + counter] = datarow.Field<string>("Surname");
                    nr["Pack" + "_" + counter] = datarow.Field<int>("Pack Needed");

                    /*
                    nr["T" + "_" + counter] = datarow.Field<string>("Title 1");
                    nr["FN" + "_" + counter] = datarow.Field<string>("First Name");
                    nr["LN" + "_" + counter] = datarow.Field<string>("Surname");
                    nr["A1" + "_" + counter] = datarow.Field<string>("Preferred Address Line 1");
                    nr["A2" + "_" + counter] = datarow.Field<string>("Preferred Address Line 2");
                    nr["A3" + "_" + counter] = datarow.Field<string>("Preferred Address Line 3");
                    nr["C" + "_" + counter] = datarow.Field<string>("Preferred City");
                    nr["PC" + "_" + counter] = datarow.Field<string>("Preferred Postcode");
                    nr["Pack" + "_" + counter] = datarow.Field<int>("Pack Needed");

                     */

                    datarow.Delete(); //set to delete
                    counter++;
                    if (counter > LABELS_PER_PAGE) break;
                }
                originalData.AcceptChanges(); //confirm changes i.e delete
                newData.Rows.Add(nr);
            }



            string[] filepaths = new string[newData.Rows.Count];
            //string datetime = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");

            int inc = 0;
            string filePath = "";
            foreach (DataRow datar in newData.Rows)
            {
                string sourceFile = labelDocFile; //this is where you store your template
                if (inc == 0) filePath = baseFilePath + datetime + ".docx";
                else filePath = baseFilePath + datetime + "_" + inc + ".docx"; //this is where your result file locate
                Mailmerge(sourceFile, filePath, datar, newData.Columns);
                filepaths[inc] = filePath;
                inc++;
            }
            MergeDocuments(filepaths);

            //delete excess files
            if (filepaths.Length > 1) deleteExcessDocs(filepaths);

            //ZIP FILE

            //create + send ZIP as DL
            string[] filesToZip = new string[3];
            string zipFile = baseFilePath + datetime;
            filesToZip[1] = baseFilePath + datetime + ".docx";
            filesToZip[2] = baseFilePath + datetime + ".xlsx";
            ZipFiles(filesToZip, zipFile);

            //Delete all but zip file
            deleteExcessDocs(filesToZip);

            //Send DL
            DownloadZip(baseFilePath + datetime + ".zip");
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
            for(int i = 1; i < filepaths.Length; i++) //start at 1, since we need first file (0)
            {
                File.Delete(filepaths[i]);
            }
        }

        public static void dotx2docx(string sourceFile, string targetFile)
        {
            MemoryStream documentStream;
            using (Stream tplStream = File.OpenRead(sourceFile))
            {
                documentStream = new MemoryStream((int)tplStream.Length);
                CopyStream(tplStream, documentStream);
                documentStream.Position = 0L;
            }

            using (WordprocessingDocument template = WordprocessingDocument.Open(documentStream, true))
            {
                template.ChangeDocumentType(DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
                MainDocumentPart mainPart = template.MainDocumentPart;
                mainPart.DocumentSettingsPart.AddExternalRelationship("http://schemas.openxmlformats.org/officeDocument/2006/relationships/attachedTemplate",
                   new Uri(targetFile, UriKind.Absolute));

                mainPart.Document.Save();
            }
            File.WriteAllBytes(targetFile, documentStream.ToArray());
        }
        public static void CopyStream(Stream source, Stream target)
        {
            if (source != null)
            {
                MemoryStream mstream = source as MemoryStream;
                if (mstream != null) mstream.WriteTo(target);
                else
                {
                    byte[] buffer = new byte[2048];
                    int length = buffer.Length, size;
                    while ((size = source.Read(buffer, 0, length)) != 0)
                        target.Write(buffer, 0, size);
                }
            }
        }
        public static void Mailmerge(string templatePath, string DestinatePath, DataRow dr,DataColumnCollection columns)
        {
            try
            {
                dotx2docx(templatePath, DestinatePath);
            }
            catch //incase the server does not support MS Office Word 2003 / 2007 / 2010
            {
                File.Copy(templatePath, DestinatePath, true);
            }
            using (WordprocessingDocument doc = WordprocessingDocument.Open(DestinatePath, true))
            {
                
                var allParas = doc.MainDocumentPart.Document.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>();
                DocumentFormat.OpenXml.Wordprocessing.Text PreItem = null;
                string PreItemConstant = null;
                bool FindSingleAnglebrackets = false;
                bool breakFlag = false;
                List<DocumentFormat.OpenXml.Wordprocessing.Text> breakedFiled = new List<DocumentFormat.OpenXml.Wordprocessing.Text>();
                foreach (DocumentFormat.OpenXml.Wordprocessing.Text item in allParas)
                {
                    foreach (DataColumn cl in columns)
                    {
                        //<Today>
                        if (item.Text.Contains("«" + cl.ColumnName + "»") || item.Text.Contains("<" + cl.ColumnName + ">"))
                        {
                            item.Text = item.Text.Replace("<" + cl.ColumnName + ">", dr[cl.ColumnName].ToString())
                                                 .Replace("«" + cl.ColumnName + "»", dr[cl.ColumnName].ToString());
                            FindSingleAnglebrackets = false;
                            breakFlag = false;
                            breakedFiled.Clear();
                        }
                        else if //<Today
                        (item.Text != null
                            && (
                                    (item.Text.Contains("<") && !item.Text.Contains(">"))
                                    || (item.Text.Contains("«") && !item.Text.Contains("»"))
                                )
                            && (item.Text.Contains(cl.ColumnName))
                        )
                        {
                            FindSingleAnglebrackets = true;
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"\<" + cl.ColumnName + @"(?!\w)", dr[cl.ColumnName].ToString());
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"\«" + cl.ColumnName + @"(?!\w)", dr[cl.ColumnName].ToString());
                        }
                        else if //Today> or Today
                        (
                            PreItemConstant != null
                            && (
                                    (PreItemConstant.Contains("<") && !PreItemConstant.Contains(">"))
                                    || (PreItemConstant.Contains("«") && !PreItemConstant.Contains("»"))
                                )
                            && (item.Text.Contains(cl.ColumnName))
                        )
                        {
                            if (item.Text.Contains(">") || item.Text.Contains("»"))
                            {
                                FindSingleAnglebrackets = false;
                                breakFlag = false;
                                breakedFiled.Clear();
                            }
                            else
                            {
                                FindSingleAnglebrackets = true;
                            }
                            if (PreItemConstant == "<" || PreItemConstant == "«")
                            {
                                PreItem.Text = "";
                            }
                            else
                            {
                                PreItem.Text = global::System.Text.RegularExpressions.Regex.Replace(PreItemConstant, @"\<" + cl.ColumnName + @"(?!\w)", dr[cl.ColumnName].ToString());
                                PreItem.Text = global::System.Text.RegularExpressions.Regex.Replace(PreItemConstant, @"\«" + cl.ColumnName + @"(?!\w)", dr[cl.ColumnName].ToString());
                            }
                            if (PreItemConstant.Contains("<") || PreItemConstant.Contains("«")) // pre item is like '[blank]«'
                            {
                                PreItem.Text = PreItem.Text.Replace("<", "");
                                PreItem.Text = PreItem.Text.Replace("«", "");
                            }
                            if (item.Text.Contains(cl.ColumnName + ">") || item.Text.Contains(cl.ColumnName + "»"))
                            {
                                item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"(?<!\w)" + cl.ColumnName + @"\>", dr[cl.ColumnName].ToString());
                                item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"(?<!\w)" + cl.ColumnName + @"\»", dr[cl.ColumnName].ToString());

                            }
                            else
                            {
                                item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"(?<!\w)" + cl.ColumnName + @"(?!\w)", dr[cl.ColumnName].ToString());
                            }
                        }
                        else if (FindSingleAnglebrackets && (item.Text.Contains("»") || item.Text.Contains(">")))
                        {
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"(?<!\w)" + cl.ColumnName + @"\>", dr[cl.ColumnName].ToString());
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"(?<!\w)" + cl.ColumnName + @"\»", dr[cl.ColumnName].ToString());
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"^\s*\>", "");
                            item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"^\s*\»", "");
                            FindSingleAnglebrackets = false;
                            breakFlag = false;
                            breakedFiled.Clear();
                        }
                        else if (item.Text.Contains("<") || item.Text.Contains("«")) // no ColumnName
                        {

                        }
                    } //end of each columns
                    PreItem = item;
                    PreItemConstant = item.Text;
                    if (breakFlag
                        || (item.Text.Contains("<") && !item.Text.Contains(">"))
                        || (item.Text.Contains("«") && !item.Text.Contains("»"))
                       )
                    {
                        breakFlag = true;
                        breakedFiled.Add(item);
                        string combinedfiled = "";
                        foreach (DocumentFormat.OpenXml.Wordprocessing.Text t in breakedFiled)
                        {
                            combinedfiled += t.Text;
                        }
                        foreach (DataColumn cl in columns)
                        {
                            //<Today>
                            if (combinedfiled.Contains("«" + cl.ColumnName + "»") || combinedfiled.Contains("<" + cl.ColumnName + ">"))
                            {
                                //for the first part, remove the last '<' and tailing content
                                breakedFiled[0].Text = global::System.Text.RegularExpressions.Regex.Replace(breakedFiled[0].Text, @"<\w*$", "");
                                breakedFiled[0].Text = global::System.Text.RegularExpressions.Regex.Replace(breakedFiled[0].Text, @"<\w*$", "");

                                //remove middle parts
                                foreach (DocumentFormat.OpenXml.Wordprocessing.Text t in breakedFiled)
                                {
                                    if (!t.Text.Contains("<") && !t.Text.Contains("«") && !t.Text.Contains(">") && !t.Text.Contains("»"))
                                    {
                                        t.Text = "";
                                    }
                                }

                                //for the last part(as current item), remove leading content till the first '>' 
                                item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"^\s*\>", dr[cl.ColumnName].ToString());
                                item.Text = global::System.Text.RegularExpressions.Regex.Replace(item.Text, @"^\s*\»", dr[cl.ColumnName].ToString());

                                FindSingleAnglebrackets = false;
                                breakFlag = false;
                                breakedFiled.Clear();
                                break;
                            }
                        }
                    }
                }//end of each item
                #region go through footer
                MainDocumentPart mainPart = doc.MainDocumentPart;
                foreach (FooterPart footerPart in mainPart.FooterParts)
                {
                    Footer footer = footerPart.Footer;
                    var allFooterParas = footer.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>();
                    foreach (DocumentFormat.OpenXml.Wordprocessing.Text item in allFooterParas)
                    {
                        foreach (DataColumn cl in columns)
                        {
                            if (item.Text.Contains("«" + cl.ColumnName + "»") || item.Text.Contains("<" + cl.ColumnName + ">"))
                            {
                                item.Text = (string.IsNullOrEmpty(dr[cl.ColumnName].ToString()) ? " " : dr[cl.ColumnName].ToString());
                                FindSingleAnglebrackets = false;
                            }
                            else if (PreItem != null && (PreItem.Text == "<" || PreItem.Text == "«") && (item.Text.Trim() == cl.ColumnName))
                            {
                                FindSingleAnglebrackets = true;
                                PreItem.Text = "";
                                item.Text = (string.IsNullOrEmpty(dr[cl.ColumnName].ToString()) ? " " : dr[cl.ColumnName].ToString());
                            }
                            else if (FindSingleAnglebrackets && (item.Text == "»" || item.Text == ">"))
                            {
                                item.Text = "";
                                FindSingleAnglebrackets = false;
                            }
                        }
                        PreItem = item;
                    }
                }
                #endregion

                #region replace \v to new Break()
                var body = doc.MainDocumentPart.Document.Body;

                var paras = body.Elements<Paragraph>();
                foreach (var para in paras)
                {
                    foreach (var run in para.Elements<DocumentFormat.OpenXml.Wordprocessing.Run>())
                    {
                        foreach (var text in run.Elements<DocumentFormat.OpenXml.Wordprocessing.Text>())
                        {
                            if (text.Text.Contains("MS_Doc_New_Line"))
                            {
                                string[] ss = text.Text.Split(new string[] { "MS_Doc_New_Line" }, StringSplitOptions.None);
                                text.Text = text.Text = "";
                                int n = 0;
                                foreach (string s in ss)
                                {
                                    n++;
                                    run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(s));
                                    if (n != ss.Length)
                                    {
                                        run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break());
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                doc.MainDocumentPart.Document.Save();
            }
        }
        public static void MergeDocuments(params string[] filepaths)
        {

            //filepaths = new[] { "D:\\one.docx", "D:\\two.docx", "D:\\three.docx", "D:\\four.docx", "D:\\five.docx" };
            if (filepaths != null && filepaths.Length > 1)

                using (WordprocessingDocument myDoc = WordprocessingDocument.Open(@filepaths[0], true))
                {
                    MainDocumentPart mainPart = myDoc.MainDocumentPart;

                    for (int i = 1; i < filepaths.Length; i++)
                    {
                        string altChunkId = "AltChunkId" + i;
                        AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(
                            AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                        using (FileStream fileStream = File.Open(@filepaths[i], FileMode.Open))
                        {
                            chunk.FeedData(fileStream);
                        }
                        DocumentFormat.OpenXml.Wordprocessing.AltChunk altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
                        altChunk.Id = altChunkId;
                        //new page, if you like it...
                        mainPart.Document.Body.AppendChild(new Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Break() { Type = BreakValues.Page })));
                        //next document
                        mainPart.Document.Body.InsertAfter(altChunk, mainPart.Document.Body.Elements<Paragraph>().Last());
                    }
                    mainPart.Document.Save();
                    myDoc.Dispose();
                }
        }

        private static void WriteExcelFile(string outputPath, DataTable table)
        {
            ExcelWriter ew = new ExcelWriter(outputPath);

            ExcelWorksheet ws = ew.AddWorksheet("Sheet1");

            ew.SetValues(ws, table);

            //ExcelRow row = ws.Row(1);
            //row.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            //row.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);

            ew.Save();
        }

        //deprecated
        private static void WriteExcelFilem(string outputPath, DataTable table)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(outputPath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                sheets.Append(sheet);

                Row headerRow = new Row();

                List<String> columns = new List<string>();
                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);

                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);



                foreach (DataRow dsrow in table.Rows)
                {
                    Row newRow = new Row();
                    foreach (String col in columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dsrow[col].ToString());
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                workbookPart.Workbook.Save();
            }
        }


    }
}