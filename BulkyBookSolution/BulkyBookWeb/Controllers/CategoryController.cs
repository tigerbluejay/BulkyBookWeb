using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        // we pass as argument whatever we registered in the builder in Program.cs
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            // retrieve Categories from db
            IEnumerable<Category> objCategoryList = _db.Categories;
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
                _db.Categories.Add(obj);
                _db.SaveChanges(); // here it goes to the db and saves changes
                return RedirectToAction("Index");
            }
            // if it is not valid we return the Create View with the object passed on
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            // tries to find based on the PK of the table
            var categoryFromDb = _db.Categories.Find(id);
            
            // returns only 1 element, if there is more than 1 element returns the first
            //var categoryfromDbFirst = _db.Categories.FirstOrDefault(u => u.Id== id);

            // returns only 1 element, if there is more than 1 element throws exception, if no elements are found it will return empty
            //var categoryfromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            // returns only 1 element, if no elements are found it will throw an exception
            //var categoryfromDbSingle = _db.Categories.Single(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
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
                _db.Categories.Add(obj);
                _db.SaveChanges(); // here it goes to the db and saves changes
                return RedirectToAction("Index");
            }
            // if it is not valid we return the Create View with the object passed on
            return View(obj);
        }
    }
}
