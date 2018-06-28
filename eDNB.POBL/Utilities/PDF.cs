using eDNB.POBL.FileProcess;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDNB.POBL.Utilities
{
    public class PDF
    {
        public bool ConvertToPDF(string input, string output)
        {
            RANKProcess rProcess = new RANKProcess();

            Document doc = new Document();

            doc.SetPageSize(PageSize.A4.Rotate());

            //doc.SetMargins(45, 10, 25, 20);

            PdfWriter.GetInstance(doc, new FileStream(output, FileMode.Create));

            doc.Open();

            StringBuilder sb = new StringBuilder();

            int pageNum = 1;

            int ctr = 1;

            using(StreamReader rdr = new StreamReader(input))
            {
                while(rdr.Peek() >= 0)
                {
                    var strLine = rdr.ReadLine();

                    if(ctr.Equals(62))
                    {
                        StreamWriter sw = new StreamWriter(Path.Combine(rProcess.tmpXFolder, pageNum + ".txt"), true);

                        sw.Write(sb.ToString());

                        sw.Close();

                        sb.Clear();

                        pageNum++;

                        ctr = 1;
                    }
                    else
                    {
                        sb.AppendLine(strLine);

                        ctr++;

                        if(strLine.Contains("**** End of Report ****"))
                        {
                            StreamWriter sw = new StreamWriter(Path.Combine(rProcess.tmpXFolder, pageNum + ".txt"), true);

                            sw.Write(sb.ToString());

                            sw.Close();

                            sb.Clear();                                                        
                        }
                        
                    }
                }

                var countTxt = rProcess.GetFileToFolder(rProcess.tmpXFolder, "txt").Count();

                for(int i = 1; i <= countTxt; i++)
                {
                    using(StreamReader rdr1 = new StreamReader(Path.Combine(rProcess.tmpXFolder, i + ".txt")))
                    {
                        doc.NewPage();

                        Paragraph para = new Paragraph(rdr1.ReadToEnd(), new Font(Font.COURIER, 5f));

                        doc.Add(para);

                        rdr1.Close();
                    }
                }

                for(int i = 1; i <= countTxt; i++)
                {
                    File.Delete(Path.Combine(rProcess.tmpXFolder, i + ".txt"));
                }

                try
                {
                    rdr.Close();

                    doc.Close();

                    return true;

                }
                catch
                {
                    if(doc.PageNumber == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool ConvertToPDFPO(string input, string output, string compny)
        {
            POProcess rProcess = new POProcess();

            Document doc = new Document();
            
            PdfWriter.GetInstance(doc, new FileStream(output, FileMode.Create));

            doc.Open();

            StringBuilder sb = new StringBuilder();

            var uhh = Path.GetFileNameWithoutExtension(input);

            int pageNum = 1;
            
            using(StreamReader rdr = new StreamReader(input))
            {
                while(rdr.Peek() >= 0)
                {
                    var strLine = rdr.ReadLine();

                    sb.AppendLine(strLine);

                    if(strLine.Contains("Continue to Next Page"))
                    {
                        StreamWriter sw = new StreamWriter(Path.Combine(rProcess.tmpXFolder, uhh + pageNum + ".txt"), true);

                        sw.Write(sb.ToString());

                        sw.Close();

                        sb.Clear();

                        pageNum++;
                        
                    }
                    else
                    {
                        
                        if(strLine.Contains("General Manager"))
                        {
                            StreamWriter sw = new StreamWriter(Path.Combine(rProcess.tmpXFolder, uhh + pageNum + ".txt"), true);

                            sw.Write(sb.ToString());

                            sw.Close();

                            sb.Clear();
                        }

                    }
                }

                for(int i = 1; i <= pageNum; i++)
                {
                    using(StreamReader rdr1 = new StreamReader(Path.Combine(rProcess.tmpXFolder, uhh + i + ".txt")))
                    {
                        doc.NewPage();

                        iTextSharp.text.Image addlogo = default(iTextSharp.text.Image);

                        addlogo = iTextSharp.text.Image.GetInstance(Path.Combine(rProcess.tmpXFolder, compny));

                        addlogo.SetAbsolutePosition(36, 790);
                        //addlogo.Alignment = Image.ALIGN_LEFT | Image.TEXTWRAP;
                        //addlogo.Alignment = Image.TEXTWRAP;

                        doc.Add(addlogo);

                        Paragraph para = new Paragraph(rdr1.ReadToEnd(), new Font(Font.COURIER, 6.5f));

                        para.SetLeading(12f, 2f);

                        doc.Add(para);

                        rdr1.Close();
                    }
                }

                for(int i = 1; i <= pageNum; i++)
                {
                    File.Delete(Path.Combine(rProcess.tmpXFolder, uhh + i + ".txt"));
                }

                try
                {
                   
                    rdr.Close();

                    doc.Close();

                    return true;

                }
                catch
                {
                    if(doc.PageNumber == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void ConvertToPDFWithFormat(string input, string output)
        {
            Document doc = new Document();

            doc.SetPageSize(PageSize.A4.Rotate());

            doc.SetMargins(45, 10, 25, 20);

            PdfWriter.GetInstance(doc, new FileStream(output, FileMode.Create));

            doc.Open();

            StringBuilder sb = new StringBuilder();

            int pageNum = 1;

            using(StreamReader rdr = new StreamReader(input))
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                while(rdr.Peek() >= 0)
                {
                    var strLine = rdr.ReadLine();

                    if(strLine.Contains("*** (C) Copyright Bindawood"))
                    {
                        StreamWriter sw = new StreamWriter(@"D:\gRank\ss" + pageNum + ".txt", true);

                        sw.Write(sb.ToString());

                        sw.Close();

                        sb.Clear();

                        pageNum++;
                    }
                    else if(strLine.Contains("**** End of Report ****"))
                    {
                        StreamWriter sw = new StreamWriter(@"D:\gRank\ss" + pageNum + ".txt", true);

                        sw.Write(sb.ToString());

                        sw.Close();

                        sb.Clear();

                        pageNum++;
                    }
                    else
                    {
                        sb.AppendLine(strLine);
                    }
                }

                for(int i = 1; i < pageNum; i++)
                {
                    using(StreamReader rdr1 = new StreamReader(@"D:\gRank\ss" + i + ".txt"))
                    {
                        doc.NewPage();

                        Paragraph para = new Paragraph(rdr1.ReadToEnd(), new Font(Font.COURIER, 6f));

                        doc.Add(para);

                        rdr1.Close();
                    }
                }

                for(int i = 1; i < pageNum; i++)
                {
                    File.Delete(@"D:\gRank\ss" + i + ".txt");
                }

                try
                {
                    doc.NewPage();

                    Paragraph para = new Paragraph("Thank you! Visit us on www.DanubeCo.com", new Font(Font.COURIER, 12f));

                    doc.Add(para);

                    rdr.Close();

                    watch.Stop();

                    doc.Close();
                }
                catch
                {
                    if(doc.PageNumber == 0)
                    {

                    }
                }


                var elapsedMs = watch.ElapsedMilliseconds;

            }
        }
    }
}
