
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class CoverTypeController : Controller
    {
        // private readonly ApplicationDbContext _db;
        //private readonly ICategoryRepository _db;
        private readonly IUnitOfWork _unitOfWork;

        // we pass as argument whatever we registered in the builder in Program.cs
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        public IActionResult Create()
        {
            
            return View();
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // inject a key into forms and key will be validated to prevent Cross Site Request Forgery
        public IActionResult Create(CoverType obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save(); // here it goes to the db and saves changes
                TempData["success"] = "CoverType created successfully";
                return RedirectToAction("Index");
            }
            // if it is not valid we return the Create View with the object passed on
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            
            var coverTypefromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (coverTypefromDbFirst == null)
            {
                return NotFound();
            }

            return View(coverTypefromDbFirst);
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // inject a key into forms and key will be validated to prevent Cross Site Request Forgery
        public IActionResult Edit(CoverType obj)
        {

            if (ModelState.IsValid)
            {
                // based on the primary key it will update all the properties
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save(); // here it goes to the db and saves changes
                TempData["success"] = "CoverType updated successfully";
                return RedirectToAction("Index");
            }
            // if it is not valid we return the Create View with the object passed on
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
           
            var coverTypefromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if (coverTypefromDbFirst == null)
            {
                return NotFound();
            }

            return View(coverTypefromDbFirst);
        }

        // POST ACTION METHOD
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // inject a key into forms and key will be validated to prevent Cross Site Request Forgery
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "CoverType deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
