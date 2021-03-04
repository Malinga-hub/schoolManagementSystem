using SchoolManagementSystem.Models;
using SchoolManagementSystem.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagementSystem.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Index(string d="-1")
        {
            System.Threading.Thread.Sleep(1000);
            //get departments, department select list and set data
            ViewData["departmentSelectionList"] = DepartmentSelectList(DBPersistence.GetDepartments().ToList(), d);
            return View();
        }
        [HttpPost]
        //post department selection
        public ActionResult Index([Bind(Include ="Id")] DepartmentModel departmentModel)
        {
            return RedirectToAction("Index", "Courses", new { d = departmentModel.Id });
        }


        //department selectlist
        public static List<SelectListItem> DepartmentSelectList(List<DepartmentModel> departments, string d)
        {
            //set select list
            List<SelectListItem> departmentItems = new List<SelectListItem>();
            foreach (var department in departments)
            {
                departmentItems.Add(new SelectListItem
                {
                    Text = department.Name,
                    Value = department.Id.ToString(),
                    Selected = department.Id.ToString().Equals(d)
                });
            }

            //insert the default all departments option
            departmentItems.Insert(0, new SelectListItem
            {
                Text = "All",
                Value = "-1",
                Selected = d.Equals("-1")
            });

            return departmentItems;
        }
    }
}