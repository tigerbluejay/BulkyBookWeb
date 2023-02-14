
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        // private readonly ApplicationDbContext _db;
        //private readonly ICategoryRepository _db;
        private readonly IUnitOfWork _unitOfWork;

        // we pass as argument whatever we registered in the builder in Program.cs
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            // retrieve Categories from db
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            // when user clicks on Create Category Button it will bring them to this action
            // this action redirects them to the Create View, but we don't pass any model
            // because the user will be creating the model in the View
            return View();
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // inject a key into forms and key will be validated to prevent Cross Site Request Forgery
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                // this will appear in the summary tag helper div
                // ModelState.AddModelError("CustomError", "The Display Order cannot exactly match the name");
                // this will appear below the Name input element
                ModelState.AddModelError("Name", "The Display Order cannot exactly match the name");

            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save(); // here it goes to the db and saves changes
                TempData["success"] = "Category created successfully";
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
            // tries to find based on the PK of the table
            // var categoryFromDb = _db.Categories.Find(id);

            // returns only 1 element, if there is more than 1 element returns the first
            var categoryfromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            // returns only 1 element, if there is more than 1 element throws exception, if no elements are found it will return empty
            //var categoryfromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            // returns only 1 element, if no elements are found it will throw an exception
            //var categoryfromDbSingle = _db.Categories.Single(u => u.Id == id);

            if (categoryfromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryfromDbFirst);
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // inject a key into forms and key will be validated to prevent Cross Site Request Forgery
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                // this will appear in the summary tag helper div
                // ModelState.AddModelError("CustomError", "The Display Order cannot exactly match the name");
                // this will appear below the Name input element
                ModelState.AddModelError("Name", "The Display Order cannot exactly match the name");

            }

            if (ModelState.IsValid)
            {
                // based on the primary key it will update all the properties
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save(); // here it goes to the db and saves changes
                TempData["success"] = "Category updated successfully";
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
            // tries to find based on the PK of the table
            // var categoryFromDb = _db.Categories.Find(id);

            // returns only 1 element, if there is more than 1 element returns the first
            var categoryfromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            // returns only 1 element, if there is more than 1 element throws exception, if no elements are found it will return empty
            //var categoryfromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            // returns only 1 element, if no elements are found it will throw an exception
            //var categoryfromDbSingle = _db.Categories.Single(u => u.Id == id);

            if (categoryfromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryfromDbFirst);
        }

        // POST ACTION METHOD
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // inject a key into forms and key will be validated to prevent Cross Site Request Forgery
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
