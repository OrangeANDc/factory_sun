using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SystemTool.Model
{
    public class PDFOperation
    {
        private string fontName = "c:\\windows\\fonts\\Arial.ttf";
        private Font font;
        private Rectangle rect;
        private Document document;
        private BaseFont basefont;

        public PDFOperation()
        {
            rect = PageSize.A4;
            document = new Document(rect);
        }

        public PDFOperation(string type)
        {
            SetPageSize(type);
            document = new Document(rect);
        }

        public PDFOperation(
          string type,
          float marginLeft,
          float marginRight,
          float marginTop,
          float marginBottom)
        {
            SetPageSize(type);
            document = new Document(rect, marginLeft, marginRight, marginTop, marginBottom);
        }

        public void SetFontName(string path) => fontName = path;

        public void SetBaseFont(string path) => basefont = BaseFont.CreateFont(path, "Identity-H", false);

        public void SetFont(float size) => font = new Font(basefont, size);

        public void SetPageSize(string type)
        {
            switch (type.Trim())
            {
                case "A4":
                    rect = PageSize.A4;
                    break;
                case "A8":
                    rect = PageSize.A8;
                    break;
            }
        }

        public void GetInstance(Stream os) => PdfWriter.GetInstance(document, os);

        public void Open(Stream os)
        {
            GetInstance(os);
            document.Open();
        }

        public void Close() => document.Close();

        public void AddParagraph(string content, float fontsize)
        {
            Font font = new Font(BaseFont.CreateFont(fontName, "Identity-H", false), fontsize);
            document.Add(new Paragraph(content, font)
            {
                Alignment = 1
            });
        }

        public void AddTitle(string content, float fontsize)
        {
            Font font = new Font(BaseFont.CreateFont(fontName, "Identity-H", false), fontsize);
            Paragraph paragraph = new Paragraph(content, font);
            document.AddTitle(content);
        }

        public void AddParagraph(
          string content,
          float fontsize,
          int Alignment,
          float SpacingAfter,
          float SpacingBefore,
          float MultipliedLeading)
        {
            SetFont(fontsize);
            Paragraph paragraph = new Paragraph(content, font);
            paragraph.Alignment = Alignment;
            if ((double)SpacingAfter != 0.0)
                paragraph.SpacingAfter = SpacingAfter;
            if ((double)SpacingBefore != 0.0)
                paragraph.SpacingBefore = SpacingBefore;
            if ((double)MultipliedLeading != 0.0)
                paragraph.MultipliedLeading = MultipliedLeading;
            document.Add(paragraph);
        }

        public void AddImage(string path, int Alignment, float newWidth, float newHeight)
        {
            Image instance = Image.GetInstance(path);
            instance.Alignment = Alignment;
            if ((double)newWidth != 0.0)
                instance.ScaleAbsolute(newWidth, newHeight);
            else if ((double)instance.Width > (double)PageSize.A4.Width)
                instance.ScaleAbsolute(rect.Width, instance.Width * instance.Height / rect.Height);
            document.Add(instance);
        }

        public void AddAnchorReference(string Content, float FontSize, string Reference)
        {
            SetFont(FontSize);
            document.Add(new Anchor(Content, font)
            {
                Reference = Reference
            });
        }

        public void AddAnchorName(string Content, float FontSize, string Name)
        {
            SetFont(FontSize);
            document.Add(new Anchor(Content, font)
            {
                Name = Name
            });
        }

        public void Add(IElement element) => document.Add(element);

        public void CreatPDFTable(string pdfName)
        {
            FileStream os = new FileStream(pdfName, FileMode.Create);
            Document document = new Document(PageSize.A7.Rotate());
            PdfWriter.GetInstance(document, os);
            document.Open();
            document.Add(new Paragraph("1"));
            document.Add(PDFTable1());
            document.SetPageSize(PageSize.A6);
            document.NewPage();
            document.Add(new Paragraph("2"));
            document.Add(PDFTable2());
            document.Add(new Paragraph("3"));
            document.Add(PDFTable3());
            document.Close();
            os.Close();
        }

        private PdfPTable PDFTable1()
        {
            PdfPTable pdfPtable = new PdfPTable(4);
            int[] relativeWidths = new int[4] { 1, 2, 3, 4 };
            pdfPtable.SetWidths(relativeWidths);
            for (int index = 0; index < 16; ++index)
                pdfPtable.AddCell((index + 1).ToString());
            return pdfPtable;
        }

        private PdfPTable PDFTable2()
        {
            BaseFont font1 = BaseFont.CreateFont(fontName, "Identity-H", false);
            Font font2 = new Font(font1, 12f);
            Font font3 = new Font(font1, 10f);
            Font font4 = new Font(font1, 8f);
            Font font5 = new Font(font1, 12f, 2, BaseColor.RED);
            PdfPTable pdfPtable = new PdfPTable(4);
            pdfPtable.AddCell(new PdfPCell(new Phrase(Convert.ToString(1), font3))
            {
                HorizontalAlignment = 1,
                Colspan = 2
            });
            pdfPtable.AddCell(new PdfPCell(new Phrase(Convert.ToString(2), font4))
            {
                HorizontalAlignment = 1,
                Rowspan = 2
            });
            PdfPCell cell1 = new PdfPCell(new Phrase(Convert.ToString(3), font3));
            cell1.BackgroundColor = BaseColor.GRAY;
            pdfPtable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase(Convert.ToString(4), font5));
            pdfPtable.AddCell(cell2);
            for (int index = 0; index < 16; ++index)
                pdfPtable.AddCell((index + 1).ToString());
            return pdfPtable;
        }

        private PdfPTable PDFTable3()
        {
            PdfPTable pdfPtable = new PdfPTable(4);
            int[] relativeWidths = new int[4] { 1, 1, 4, 1 };
            pdfPtable.SetWidths(relativeWidths);
            for (int index = 0; index < 16; ++index)
            {
                if (index == 10)
                    pdfPtable.AddCell(new PdfPCell(PDFTable2())
                    {
                        Padding = 0.0f
                    });
                else
                    pdfPtable.AddCell("3");
            }
            return pdfPtable;
        }

        public PdfPTable CreateTable(string title, List<OneItem> list)
        {
            Font font = new Font(BaseFont.CreateFont(fontName, "Identity-H", false), 8f);
            PdfPTable table = new PdfPTable(3);
            int[] relativeWidths = new int[3] { 4, 4, 2 };
            table.SetWidths(relativeWidths);
            PdfPCell cell = new PdfPCell(new Phrase(title, font));
            cell.HorizontalAlignment = 1;
            cell.Colspan = 3;
            cell.BackgroundColor = BaseColor.GRAY;
            table.AddCell(cell);
            for (int index = 0; index < list.Count; ++index)
            {
                OneItem oneItem = list[index];
                table.AddCell(new Phrase(oneItem.Name.ToString(), font));
                table.AddCell(new Phrase(oneItem.Value.ToString(), font));
                table.AddCell(new Phrase(oneItem.Unit.ToString(), font));
            }
            return table;
        }

        public PdfPTable CreateTable2(string title, List<OneItem> list)
        {
            Font font = new Font(BaseFont.CreateFont(fontName, "Identity-H", false), 8f);
            PdfPTable table2 = new PdfPTable(2);
            int[] relativeWidths = new int[2] { 4, 6 };
            table2.SetWidths(relativeWidths);
            PdfPCell cell = new PdfPCell(new Phrase(title, font));
            cell.HorizontalAlignment = 1;
            cell.Colspan = 2;
            cell.BackgroundColor = BaseColor.GRAY;
            table2.AddCell(cell);
            for (int index = 0; index < list.Count; ++index)
            {
                OneItem oneItem = list[index];
                table2.AddCell(new Phrase(oneItem.Name.ToString(), font));
                table2.AddCell(new Phrase(oneItem.Value.ToString(), font));
            }
            return table2;
        }
    }

    public class OneItem
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Unit { get; set; }
    }
}
