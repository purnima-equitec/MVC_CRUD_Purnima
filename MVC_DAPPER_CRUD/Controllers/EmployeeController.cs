using ClosedXML.Excel;
using Dapper;
using MVC_DAPPER_CRUD.Models;
using PagedList;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace MVC_DAPPER_CRUD.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index(int? page, int? pageSize)
        {
            int pageNumber = (page.HasValue ? page.Value : 1);
            int currentPageSize = pageSize.HasValue ? pageSize.Value : 5;
            IEnumerable<EmployeeModel> employees = DapperORM.ReturnList<EmployeeModel>("EmployeeViewAll");
            List<EmployeeModel> employeeList = employees.ToList();
            int[] pageSizeOptions = { 5, 10, 15, 20,50 };
            SelectList selectedPageSize = new SelectList(pageSizeOptions, currentPageSize);
            ViewBag.SelectedPageSize = selectedPageSize;
            return View(employees.ToPagedList(pageNumber, currentPageSize));
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View();
            else
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@EMPID", id);
                return View(DapperORM.ReturnList<EmployeeModel>("EmployeeViewByID", dynamicParameters).FirstOrDefault<EmployeeModel>());
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(EmployeeModel emp)
        {
            string[] selectedSkills = Request.Form.GetValues("skills");
            emp.EMP_SKILLS = selectedSkills != null ? string.Join(",", selectedSkills) : string.Empty;
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@EMPID", emp.EMPID);
            dynamicParameters.Add("@EMPNAME", emp.EMPNAME);
            dynamicParameters.Add("@EMP_DESIGNATION", emp.EMP_DESIGNATION);
            dynamicParameters.Add("@EMP_SALARY", emp.EMP_SALARY);
            dynamicParameters.Add("@EMP_GENDER", emp.EMP_GENDER);
            dynamicParameters.Add("@EMP_EMAIL", emp.EMP_EMAIL);
            dynamicParameters.Add("@EMP_AGE", emp.EMP_AGE);
            dynamicParameters.Add("@EMP_SKILLS", emp.EMP_SKILLS);
            DapperORM.ExecuteWithoutReturn("EmployeeAddOrEdit", dynamicParameters);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteData(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@EMPID", id);
            DapperORM.ExecuteWithoutReturn("EmployeeDeleteByID", dynamicParameters);
            return RedirectToAction("Index");
        }

        public ActionResult DisplayData(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@EMPID", id);
            return View(DapperORM.ReturnList<EmployeeModel>("EmployeeViewByID", dynamicParameters).FirstOrDefault<EmployeeModel>());
        }
        public ActionResult GetAllDeletedEmployees()
        {
            var deletedEmployees = DapperORM.ReturnList<EmployeeModel>("GetAllDeletedEmployees");
            return View(deletedEmployees);
        }
        public ActionResult RestoreEmployee(int id)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@EMPID", id);
            DapperORM.ExecuteWithoutReturn("RestoreEmployee", dynamicParameters);
            return RedirectToAction("Index");
        }

        public ActionResult ExportToExcel()
        {
            var employees = DapperORM.ReturnList<EmployeeModel>("EmployeeViewAll");
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Employees");
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Designation";
                worksheet.Cell(1, 4).Value = "Salary";
                worksheet.Cell(1, 5).Value = "Gender";
                worksheet.Cell(1, 6).Value = "Email";
                worksheet.Cell(1, 7).Value = "Age";
                worksheet.Cell(1, 8).Value = "Skills";
                worksheet.Cell(2, 1).InsertData(employees);
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
            }
        }
    }
}