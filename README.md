"# BulkyBookWeb" 

#DotNetMastery's BulkyBookWeb
#A Complete MVC E-Commerce Solution

This project is my actual full implementation of DotNetMastery's BulkyBookWeb.

This is a *complete N-Tier MVC Solution* consisting of four projects. The application is a marketplace for books where customers may register for an account and purchase various amounts of various items. The application calculates the total price and displays details about the books. The customer may add the items to a shopping cart, add or delete items at will. When the customer is ready they may proceed to checkout and make payments to have the items delivered.

By using the database the customer may grant admin privileges which allows users to manipulate and visualize existing orders, order details, and change the status of orders. The user may also register company customers who may place orders without having to make payments immediately.

Additional functionality like facebook login and stripe integration to process payments has been added in the form of comments. Such functionality has not been tested but it has been fully implemented. The reader need only create the corresponding Stripe and Facebook accounts and uncomment the commented code to implement it.

The app is complex and it utilizes *most all of the main features offered by MVC architecture.*

##Features of different Projects in the Solution

###1) BulkyBook.DataAccess

-ApplicationDbContext is defined to create tables on the backend using SQL Server.
-Auto-generated Entity Framework Migration Folder keeps track of migrations.
-Repository folder contains interfaces and concrete implemenetations for the UnitOfWork and Repository pattern to implement the different CRUD operations for each of the repositories in the Solution, for instance the IRepository defines GetAll(), GetFirstOrDefault(), Add(), Remove() and RemoveRange() and various Repositories define Update() and other operations. IUnitOfWork defines Save() to persist changes to the database.
	-The repositories include: ApplicationUserRepository, CategoryRepository, CompanyRepository, CoverTypeRepository, OrderDetailRepository, OrderHeaderRepository, ProductRepository, Repository, ShoppingCartRepository, and UnitOfWork

###2) BulkyBook.Models

-Here we find all the models with all the properties defined necessary for the creation of tables in the database. We have ApplicationUser, Category, Company, CoverType, ErrorViewModel, OrderDetail, OrderHeader, Product, and ShoppingCart.
-We also find all the View Models with the models they reference and other properties: We have OrderVM, ProductVM, ShoppingCartVM.

###3) BulkyBook.Utility

-Here we find three files. Static Details SD to avoid magic strings elsewhere in the project. SD defines properties for User Roles, Order Statuses, Payment Statuses, and Cart Session.
-The second file is StripeSettings which is part of the Stripe integration implementation.
-The third file is EmailSender contains the implementation for MimeKit and MailKit which are useful for sending gmail emails (inactive - commented), and an alternative (active - uncommented) SendGrid implementation for sending emails when you have control of a domain, to send emails of type @domain.com for example.

###4) BulkyBook.Web

This contains the core of the domain logic, views and more. This deserves a new section because BulkyBook.Web is very large. It is also the startup Project.


##BULKYBOOK.WEB PROJECT

###1) Some common Files

-Inside the Properties folder we have launchSettings.json. 
-We also have a wwwroot folder with all the static assets like css (bootsWatch theme), images, js (scripts: company, product, order and site validation scripts), lib (bootstrap, jquery, jquery-validation, jquery-validation-unobtrusive).
-We have appSettings.json where we configure Logging, DB Connection strings, and Stripe and SendGrid settings and SecretKeys.
-The Core Program.cs file, which contains the builder for adding Services and the application pipeline.
	-The builder adds Controllers with Views, Runtime Compilation, DbContext to use SQL Server, Stripe Settings, Identity, UnitOfWork, EmailSender, FaceBook Authentication, Application Cookies and Distributed Memory Cache (for sessions), and Sessions.
	-The pipeline where all the middleware lives: It includes HttpRedriection, the use of Static Files, Routing, Stripe Configuration, Authentication, Authorization, Sessions, Map Razor Pages, and Map Controller Routes Defaults.


###2 Areas

-Areas include Admin, Customer, and Identity

	####2.1 Admin Area

	Controllers: CategoryController, CompanyController, CoverTypeController, OrderController, ProductController

	Views: Category, Company, CoverType, Order, Product as well as _ViewStart (to define the master page, in this case _Layout) and _ViewImports (for imports common to the Admin Area)

	The controllers contain the main logic corresponding to Index(), Create(), Create() (POST), Edit(), Edit() (POST), Delete(), Delete() (POST) each of which has a View or a Redirect Associated with it. Other controller methods include Upsert(), UpsertAll(), GetAll(), Details(), Details() (POST), PaymentConfirmation(), UpdateOrderDetail(), StartProcessing(), ShipOrder(), CancelOrder(), and API Calls such as GetAll()

	Many of the controller actions have their correspoding Views which are .cshtml. This means the views use a mix of C# and HTML.

	####2.2 Customer Area

	Contollers: CartContoller, HomeController

	Views: Cart, Home

	The controllers contain the main logic correspoding to Index(), Summary(), Summary() (POST), OrderConfirmation(), Plus(), Minus(), Remove(), GetPriceBasedOnQuantity(), Detials(), Details() (POST), Privacy().

	####2.3 Identity

	Identity Contains various pages related to Email Services, Login and Registration, which are generated automatically, but which also have been customized in various parts. These are cshtml pages with code behind files.

###3 View Components

-View Components contains the single file "ShoppingCartViewComponent" with its code behind file. This was created to handle sessions.

###4 Views

-The Views folder contains Shared Views such as underscore Layout (the master page), underscore LoginPartial, underscore Notification, underscore ValidationScriptsPartial, underscore ViewImports and Error. Some of these pages user features like TempData and ViewData.
-It also has the Components Folder which contains the Shopping Cart folder which contains the Default.cshtml file corresponding toe the ShoppingCart View Component.
