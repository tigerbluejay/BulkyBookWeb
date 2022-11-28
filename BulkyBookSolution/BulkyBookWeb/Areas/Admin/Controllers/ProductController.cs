
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // private readonly ApplicationDbContext _db;
        //private readonly ICategoryRepository _db;
        private readonly IUnitOfWork _unitOfWork;
        // we add hosting environment for correct processing of image upload
        private readonly IWebHostEnvironment _hostEnvironment;

        // we pass as argument whatever we registered in the builder in Program.cs
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Upsert(int? id)
        {
            // TIGHTLY BINDED VIEW - now our view is tightly bounded to ProductVM instead of passing by
            // ViewBag and View Data loosely bounded data
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                // update product
            }
            return View(productVM);
        }


            // WITHOUT VM IMPLEMENTATION: PROJECTION AND VIEW BAG AND VIEW DATA IMPLEMENTATION
        //    // with UnitOfWork we can access any Repository
        //    Product product = new();
        //    IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
        //        u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        });
        //    IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
        //        u => new SelectListItem
        //        {
        //            Text = u.Name,
        //            Value = u.Id.ToString()
        //        });

        //    if (id == null || id == 0)
        //    {
        //        // create product
        //        // VIEW BAG - a way of passing data to the view when the data is not in a model
        //        // we can give the ViewBag any meaningful name, here we named it Category List
        //        // and we pass the Category List IEnumerable to it.
        //        ViewBag.CategoryList = CategoryList;
        //        // VIEW DATA - another way of passing data to the view when the data is not in a model
        //        ViewData["CoverTypeList"] = CoverTypeList;
        //        return View(product);
        //    } else
        //    {
        //        // update product
        //    }
        //    return View(product);
        //}

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // inject a key into forms and key will be validated to prevent Cross Site Request Forgery
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    // if image is not empty we want to upload the image
                    // we need to give it a name based on GUID, because if two people
                    // decide to give the file the same name, then it is a mess
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save(); // here it goes to the db and saves changes
                TempData["success"] = "Product created successfully";
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

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll();
            return Json(new { data = productList });
        }
        #endregion
    }
}
