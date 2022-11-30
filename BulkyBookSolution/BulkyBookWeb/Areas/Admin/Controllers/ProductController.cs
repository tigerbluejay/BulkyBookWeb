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
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);

            }
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

                    // delete existing file
                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }


                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                } else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
                _unitOfWork.Save(); // here it goes to the db and saves changes
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            // if it is not valid we return the Create View with the object passed on
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productList });
        }

        // POST ACTION METHOD
        [HttpDelete]
        [ValidateAntiForgeryToken] // inject a key into forms and key will be validated to prevent Cross Site Request Forgery
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { sucess = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();

            return Json(new { sucess = true, message = "Delete Successful" });

        }


        #endregion
    }
}
