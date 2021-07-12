using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExportingAndImportingData.Models;
using OfficeOpenXml;

namespace ExportingAndImportingData.Controllers
{
    public class EmployeeController : Controller
    {
        private PredictionEntities PE = new PredictionEntities();
        // GET: Employee
        public ActionResult Index()
        {
            var employees = (from i in PE.Employees 
                              select i).ToList();
            return View(employees);
        }

        public void ExportingToExcel()
        {
            var employees = (from i in PE.Employees
                             select i).ToList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A1"].Value = "EmployeeName";
            ws.Cells["B1"].Value = "BirthDate";
            ws.Cells["C1"].Value = "HireDate";
            ws.Cells["D1"].Value = "Gender";
            ws.Cells["E1"].Value = "MaritalStatus";
            ws.Cells["F1"].Value = "Email";
            ws.Cells["G1"].Value = "PhoneNumber";

            int rowStart = 2;
            foreach (var item in employees)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.EmployeeName;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.BirthDate;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.HireDate;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.Gender;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.MaritalStatus;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Email;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.PhoneNumber;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        public ActionResult Export()
        {
            return View();
        }

        [HttpPost]
        //Alternative method for exporting the data
        public ActionResult GetEmp()
        {
            var Emp = (from i in PE.Employees
                       select new
                       {
                           i.EmployeeName,
                           i.BirthDate,
                           i.HireDate,
                           i.Gender,
                           i.MaritalStatus,
                           i.Email,
                           i.PhoneNumber,
                           i.EmpId
                       }).ToList();
            return Json(new { data = Emp }, JsonRequestBehavior.AllowGet);
        }
    }
}