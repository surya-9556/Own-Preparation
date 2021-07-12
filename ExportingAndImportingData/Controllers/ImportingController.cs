using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExportingAndImportingData.Models;
using OfficeOpenXml;

namespace ExportingAndImportingData.Controllers
{
    public class ImportingController : Controller
    {
        [HttpPost]
        public ActionResult Upload(FormCollection formCollection)
        {
            var LinkList = new List<Importing>();
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    using (var package = new ExcelPackage(file.InputStream))
                    { 
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var Imp = new Importing();
                            Imp.Sno = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                            Imp.Name = workSheet.Cells[rowIterator, 2].Value.ToString();
                            Imp.Link = workSheet.Cells[rowIterator, 3].Value.ToString();
                            LinkList.Add(Imp);
                        }
                    }
                }
            }
            using (masterEntities ME = new masterEntities())
            {
                foreach (var item in LinkList)
                {
                    ME.Importings.Add(item);
                }
                ME.SaveChanges();
            }
            return View("Import");
        }

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetLink()
        {
            using (masterEntities ME = new masterEntities())
            {
                var link = (from i in ME.Importings
                            select new
                            {
                                Sno = i.Sno,
                                Name = i.Name,
                                Link = i.Link
                            }).ToList();
                return Json(new { data = link }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}