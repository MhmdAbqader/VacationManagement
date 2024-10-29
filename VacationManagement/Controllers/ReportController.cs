//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using System.IO;




namespace VacationManagement.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult CreateComplexPdf()
        {
            // Define output file path
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", "EmployeeReport.pdf");

            // Create PDF writer and document
            using (var writer = new PdfWriter(outputPath))
            {
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Fonts
                var font = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);
                var boldFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);

                // Cover Page
                AddCoverPage(document, font, boldFont);

                // Table of Employees
                AddEmployeeTable(document, font, boldFont, GetSampleEmployees());

                // Summary Page
                AddSummaryPage(document, font, boldFont);

                document.Close();
            }

            // Return PDF for download
            var fileBytes = System.IO.File.ReadAllBytes(outputPath);
            return File(fileBytes, "application/pdf", "EmployeeReport.pdf");
        }

        private void AddCoverPage(Document document, PdfFont font, PdfFont boldFont)
        {
            document.Add(new Paragraph("Employee Report")
                .SetFont(boldFont)
                .SetFontSize(24)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(100));

            document.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("MMMM dd, yyyy"))
                .SetFont(font)
                .SetFontSize(12)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(10));

            // Add logo (replace path with your logo path)
            Image logo = new Image(iText.IO.Image.ImageDataFactory.Create("wwwroot/Couple.jpg"));
            logo.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            logo.ScaleToFit(150, 150);
            document.Add(logo);

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));  // Add a page break
        }

        private void AddEmployeeTable(Document document, PdfFont font, PdfFont boldFont, List<Employee> employees)
        {
            // Add title
            document.Add(new Paragraph("Employee List")
                .SetFont(boldFont)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetMarginBottom(10));

            // Create table with headers
            float[] columnWidths = { 1, 3, 3, 2 };
            Table table = new Table(columnWidths);
            table.SetWidth(UnitValue.CreatePercentValue(100));

            // Table Headers
            string[] headers = { "ID", "Name", "Department", "Salary" };
            foreach (string header in headers)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(header))
                    .SetFont(boldFont)
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetFontSize(12)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(5));
            }

            // Table Rows
            foreach (var emp in employees)
            {
                table.AddCell(new Cell().Add(new Paragraph(emp.Id.ToString())).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(new Cell().Add(new Paragraph(emp.Name)).SetFont(font));
                table.AddCell(new Cell().Add(new Paragraph(emp.Department)).SetFont(font));
                table.AddCell(new Cell().Add(new Paragraph(emp.Salary.ToString("C"))).SetFont(font).SetTextAlignment(TextAlignment.RIGHT));
            }

            document.Add(table);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }

        private void AddSummaryPage(Document document, PdfFont font, PdfFont boldFont)
        {
            document.Add(new Paragraph("Summary")
                .SetFont(boldFont)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetMarginBottom(10));

            // Add some summary content
            document.Add(new Paragraph("This report includes the summary of employees and their respective departments. Total employee count: " + GetSampleEmployees().Count)
                .SetFont(font)
                .SetFontSize(12)
                .SetMarginBottom(10));

            // Footer with pagination
            //int totalPages = document.GetPdfDocument().GetNumberOfPages();
            //for (int i = 1; i <= totalPages; i++)
            //{
            //    document.ShowTextAligned(new Paragraph($"Page {i} of {totalPages}")
            //        .SetFont(font)
            //        .SetFontSize(10),
            //        520, 20, i, TextAlignment.RIGHT, VerticalAlignment.BOTTOM, 0);
            //}
        }

        // Sample data class
        public class Employee
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Department { get; set; }
            public decimal Salary { get; set; }
        }

        private List<Employee> GetSampleEmployees()
        {
            return new List<Employee>
        {
            new Employee { Id = 1, Name = "Alice Johnson", Department = "HR", Salary = 70000 },
            new Employee { Id = 2, Name = "Bob Smith", Department = "Finance", Salary = 85000 },
            new Employee { Id = 3, Name = "Carol White", Department = "IT", Salary = 95000 },
            new Employee { Id = 4, Name = "David Brown", Department = "Marketing", Salary = 60000 }
        };
        }
    }
}



