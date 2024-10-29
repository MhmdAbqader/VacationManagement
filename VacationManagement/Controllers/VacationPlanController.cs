using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using VacationManagement.Data;
using VacationManagement.Models;

namespace VacationManagement.Controllers
{
    public class VacationPlanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverter _converter;
        public VacationPlanController(ApplicationDbContext applicationDbContext,    IConverter converter)
        {
            _context = applicationDbContext;
            _converter = converter;
        }
        public IActionResult Index()
        {
            var result = _context.RequestVacations
                .Include(o => o.Employee)
                .Include(o => o.VacationType)
                .OrderByDescending(o => o.RequestDate).ToList();
            return View(result);
        }

        public IActionResult Create()
        {
           // ViewBag.VacationType = _context.VacationTypes.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VacationPlan model, int[] includeDaysCheckBox)
        {
            if (ModelState.IsValid) 
            {
                //var checkVacation = _context.VacationPlans
                //    .Where(a => a.RequestVacation.EmpId == model.RequestVacation.EmpId
                //    && a.RequestVacation.StartDate >= model.RequestVacation.StartDate
                //    && a.RequestVacation.EndDate <= model.RequestVacation.EndDate).FirstOrDefault();
                var checkVacation = _context.VacationPlans
                    .Where(a => a.RequestVacation.EmpId == model.RequestVacation.EmpId
                    && a.VacationDate >= model.RequestVacation.StartDate
                    && a.VacationDate <= model.RequestVacation.EndDate).FirstOrDefault();

                if (checkVacation != null)
                {
                    ViewBag.ErrorVacation = true;
                    return View(model);
                }


                for (DateTime date = model.RequestVacation.StartDate;
                    date < model.RequestVacation.EndDate; date = date.AddDays(1))
                {
                    int yCheckValueOfDay = (int)date.DayOfWeek;
                    int xCheckValueInsideArray = Array.IndexOf(includeDaysCheckBox, (int)date.DayOfWeek); // check if the value selected inside this array of 
                                                                                //selected days  or no
                    if (Array.IndexOf(includeDaysCheckBox, (int)date.DayOfWeek) != -1) 
                    {
                        model.Id = 0;
                        model.VacationDate = date;
                        model.RequestVacation.RequestDate = DateTime.Now;
                        _context.VacationPlans.Add(model);
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            return View(model);

        }

            public IActionResult Edit(int? id)
        {
            ViewBag.Employee = _context.Employees.ToList();
            ViewBag.VacationType = _context.VacationTypes.ToList();
            var result = _context.RequestVacations
                .Include(a => a.Employee)
                .Include(a => a.VacationType)
                .Include(a => a.VacationPlansList).FirstOrDefault(a => a.Id == id);
            return View(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RequestVacation model)
        {
            if (ModelState.IsValid)
            {
                if (model.Approve == true)
                    model.ApproveDate = DateTime.Now;

                _context.RequestVacations.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Employee = _context.Employees.ToList();
            ViewBag.VacationType = _context.VacationTypes.ToList();
            return View(model);
        }

        public IActionResult GetVacationTypes() 
        {
            //return Json(new { VactionsResult = _context.VacationTypes.OrderByDescending(a => a.NoOfdays).ToList() });
            return Json(_context.VacationTypes.OrderByDescending(a => a.NoOfdays).ToList());
        }


        public IActionResult DeleteVacationRequest(int id) 
        {
            var vac = _context.RequestVacations.Find(id);
            if (vac != null) 
            {           
                List<VacationPlan> vacationPlans = _context.VacationPlans.Where(a => a.RequestVacationId == id).ToList();
                _context.RequestVacations.Remove(vac);
                _context.VacationPlans.RemoveRange(vacationPlans);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return Content("Error, please Call Developer team.!!");
        }


        public IActionResult ViewReportVacationByUsing_DataTable() 
        {
            ViewBag.Employee = _context.Employees.ToList();
            return View();
        }

        public IActionResult GetReportVacationByUsing_DataTable(int empId  , DateTime fromStartDate  , DateTime toEndDate)
        {
            string id = "";

            if (empId != 0 && empId.ToString() != "") 
            {
                id = $" and Employees.Id = {empId}";
            }

            string myQuery = $@"SELECT   distinct     dbo.Employees.Id, dbo.Employees.Name, dbo.Employees.VacationBalance , 
	                        sum(dbo.VacationTypes.NoOfdays) as TotalVacation,  dbo.Employees.VacationBalance - sum(dbo.VacationTypes.NoOfdays) as FinalTotalBalance
	                        FROM  dbo.Employees INNER JOIN
							 dbo.RequestVacations ON dbo.Employees.Id = dbo.RequestVacations.EmpId INNER JOIN
							 dbo.VacationPlans ON dbo.RequestVacations.Id = dbo.VacationPlans.RequestVacationId INNER JOIN
							 dbo.VacationTypes ON dbo.RequestVacations.VacationTypeId = dbo.VacationTypes.Id

	                        where [dbo].[VacationPlans].VacationDate between '{fromStartDate.ToString("yyyy-MM-dd")}' and '{toEndDate.ToString("yyyy-MM-dd")}' and
	                        [dbo].[RequestVacations].Approve = 'true' {id}

	                        group by  dbo.Employees.Id, dbo.Employees.Name, dbo.Employees.VacationBalance ";

            var dataTableResult = _context.SqlDataTableMyExtensionMethod(myQuery);



            ViewBag.Employee = _context.Employees.ToList();
            return View(nameof(ViewReportVacationByUsing_DataTable), dataTableResult);
        }


        //this way is so easy

        public IActionResult ViewReportVacationByUsing_FromSqlRaw()
        {
            ViewBag.Employee = _context.Employees.ToList();
            return View();
        }

        public IActionResult GetReportVacationByUsing_FromSqlRaw(int empId, DateTime fromStartDate, DateTime toEndDate)
        {
            string id = "";

            if (empId != 0 && empId.ToString() != "")
            {
                id = $" and Employees.Id = {empId}";
            }

            string myQuery = $@"SELECT   distinct     dbo.Employees.Id, dbo.Employees.Name, dbo.Employees.VacationBalance , 
	                        sum(dbo.VacationTypes.NoOfdays) as TotalVacation,  dbo.Employees.VacationBalance - sum(dbo.VacationTypes.NoOfdays) as FinalTotalBalance
	                        FROM  dbo.Employees INNER JOIN
							 dbo.RequestVacations ON dbo.Employees.Id = dbo.RequestVacations.EmpId INNER JOIN
							 dbo.VacationPlans ON dbo.RequestVacations.Id = dbo.VacationPlans.RequestVacationId INNER JOIN
							 dbo.VacationTypes ON dbo.RequestVacations.VacationTypeId = dbo.VacationTypes.Id

	                        where [dbo].[VacationPlans].VacationDate between '{fromStartDate.ToString("yyyy-MM-dd")}' and '{toEndDate.ToString("yyyy-MM-dd")}' and
	                        [dbo].[RequestVacations].Approve = 'true' {id}

	                        group by  dbo.Employees.Id, dbo.Employees.Name, dbo.Employees.VacationBalance ";

            var SqlRawQuery = _context.GetReportsVacationDatas.FromSqlRaw(myQuery).ToList();
           
            // i can create a stored procedure in sql and call it by this way 
            //var SqlRawQuery = _context.GetReportsVacationDatas
            //    .FromSqlRaw($"myStoredProcdure_Name {empId}, {fromStartDate}, {toEndDate}").ToList();



            ViewBag.Employee = _context.Employees.ToList();
            return View(nameof(ViewReportVacationByUsing_FromSqlRaw), SqlRawQuery);
        }

        public IActionResult PrintReport() 
        {

            // Define HTML content for the report
            var htmlContent = @"
            <html>
                <head>
                    <title>Report</title>
                </head>
                <body>
                    <h1>Employee Report</h1>
                    <p>  Vacation report for employees.</p>
                    <table class=""table"">
                        <thead>
                            <tr>
                 
                                    <th>Id</th>
                                    <th>Name</th>
                                <th>VacationBalance</th>
                                <th>TotalVacation</th>
                                <th>FinalTotalBalance </th>
                 
                            </tr>
                        </thead>
                        <tbody>
                            @foreach (var item in Model)
                            {
                                <tr>

                                    <td>@item.Id</td>             
                                    <td>@item.Name</td>              
                                    <td>@item.VacationBalance</td>              
                                    <td>@item.TotalVacation</td>            
                                    <td>@item.FinalTotalBalance</td>              
         
                                </tr>
                            }
                        </tbody>
                    </table>
                </body>
            </html>";

            // Configure PDF settings
            var pdfDoc = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                    Out = null // No file output, we are returning a stream
                },
                Objects = {
                new ObjectSettings
                {
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
            };

            // Generate PDF
            var pdf = _converter.Convert(pdfDoc);

            // return Content("Print Report Action under Developing");

            // Return PDF file as a downloadable response
            return File(pdf, "application/pdf", "VacationReport.pdf");
          
        }
        public IActionResult CVSFile(int empId, DateTime fromStartDate, DateTime toEndDate, string type)
        {
            string id = "";

            if (empId != 0 && empId.ToString() != "")
            {
                id = $" and Employees.Id = {empId}";
            }

            string myQuery = $@"SELECT   distinct     dbo.Employees.Id, dbo.Employees.Name, dbo.Employees.VacationBalance , 
	                        sum(dbo.VacationTypes.NoOfdays) as TotalVacation,  dbo.Employees.VacationBalance - sum(dbo.VacationTypes.NoOfdays) as FinalTotalBalance
	                        FROM  dbo.Employees INNER JOIN
							 dbo.RequestVacations ON dbo.Employees.Id = dbo.RequestVacations.EmpId INNER JOIN
							 dbo.VacationPlans ON dbo.RequestVacations.Id = dbo.VacationPlans.RequestVacationId INNER JOIN
							 dbo.VacationTypes ON dbo.RequestVacations.VacationTypeId = dbo.VacationTypes.Id

	                        where [dbo].[VacationPlans].VacationDate between '{fromStartDate.ToString("yyyy-MM-dd")}' and '{toEndDate.ToString("yyyy-MM-dd")}' and
	                        [dbo].[RequestVacations].Approve = 'true' {id}

	                        group by  dbo.Employees.Id, dbo.Employees.Name, dbo.Employees.VacationBalance ";

            DataTable dataTableResult = _context.SqlDataTableMyExtensionMethod(myQuery);

            string dirPath = @"C:\CSVReportVacation\";
            string fileContent = string.Empty;
            byte[] bytes = null;

            if (!Directory.Exists(dirPath)) 
            {
                Directory.CreateDirectory(dirPath);
            }
            using (StreamWriter writer = new StreamWriter($"C:\\CSVReportVacation\\VacationReport{fromStartDate.ToString("yyyy-MM-dd")}.csv")) 
            {
                writer.WriteLine(string.Join(",", dataTableResult.Columns.Cast<DataColumn>().Select(col => col.ColumnName)));

                foreach (DataRow row in dataTableResult.Rows)
                {
                    //writer.WriteLine(string.Join(",", row.ItemArray.Select(r => r.ToString())));
                    writer.WriteLine(string.Join(",", row.ItemArray.Select(r => r?.ToString() ?? "")));
                }
            }

            using (StreamReader reader = new StreamReader($"C:\\CSVReportVacation\\VacationReport{fromStartDate.ToString("yyyy-MM-dd")}.csv"))
            {
                fileContent = reader.ReadToEnd();
                bytes = Encoding.UTF8.GetBytes(fileContent);
            }
            if (type == "csv")
                return File(bytes, "text/csv", "VR.csv");
            else if (type == "pdf")
                return File(bytes, "application/pdf", "VR.pdf");
            else
                return Content("sorry, File Error occured ..! ");
            //return Content("CVS File Action under Developing");
        }
    }
}
