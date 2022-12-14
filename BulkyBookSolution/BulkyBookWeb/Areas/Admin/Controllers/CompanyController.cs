using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        // private readonly ApplicationDbContext _db;
        //private readonly ICategoryRepository _db;
        private readonly IUnitOfWork _unitOfWork;


        // we pass as argument whatever we registered in the builder in Program.cs
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Upsert(int? id)
        {

            Company company = new();
            
            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);

            }
        }


  
        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // inject a key into forms and key will be validated to prevent Cross Site Request Forgery
        public IActionResult Upsert(Company obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "Company created successfully";
                } else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company updated successfully";
                }
                _unitOfWork.Save(); // here it goes to the db and saves changes
                
                return RedirectToAction("Index");
            }
            // if it is not valid we return the Create View with the object passed on
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        // POST ACTION METHOD
        [HttpDelete]
        [ValidateAntiForgeryToken] // inject a key into forms and key will be validated to prevent Cross Site Request Forgery
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { sucess = false, message = "Error while deleting" });
            }


            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();

            return Json(new { sucess = true, message = "Delete Successful" });

        }


        #endregion
    }
}
