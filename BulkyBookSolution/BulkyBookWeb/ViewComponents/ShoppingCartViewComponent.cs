using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.ViewComponents
{
	// THIS PAGE IS THE CODE BEHIND FILE OF THE VIEW COMPONENT - VIEW
	// COMPONENTS HAVE CODE BEHIND FILES - WE NEEDED TO CREATE ONE TO
	// HANDLE SESSIONS
	public class ShoppingCartViewComponent : ViewComponent
	{
		// WE GET unit of work with dependency injection
		private readonly IUnitOfWork _unitOfWork;

		public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IViewComponentResult> InvokeAsync() // IViewComponentResult is a type of IViewActionResult
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); 
			if (claim != null) // this means the user is logged in
			{
				if (HttpContext.Session.GetInt32(SD.SessionCart) != null) // so we dont need to go to the database
				{
					return View(HttpContext.Session.GetInt32(SD.SessionCart)); // retrieve the value of the sesion and return it to the view
				}
				else // we go to db and retrieve the count
				{
					HttpContext.Session.SetInt32(SD.SessionCart, 
						_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count); // retrieving the number of shopping carts of our user
					return View(HttpContext.Session.GetInt32(SD.SessionCart));
				}
			} else // it happens when the user has not signed in and when the user has signed out
			{
				HttpContext.Session.Clear();
				return View(0);
			}
		}
	}
}
