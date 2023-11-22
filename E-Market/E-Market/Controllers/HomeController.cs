using E_Market.EF;
using System.Linq;
using System.Web.Mvc;

namespace E_Market.Controllers
{
    public class HomeController : Controller
    {
        private EMarketEntities db = new EMarketEntities();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Registration login)
        {
            if (ModelState.IsValid)
            {
                var user = db.Registrations
                    .Where(usr => usr.Name.Equals(login.Name) && usr.Password.Equals(login.Password))
                    .SingleOrDefault();

                if (user != null)
                {
                    Session["UserId"] = user.UserId;
                    Session["user"] = user;

                    var returnUrl = Request["ReturnUrl"];

                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else if (user.Role.Equals("Customer"))
                    {
                        return RedirectToAction("Index", "Customer");
                    }
                    else if (user.Role.Equals("Admin"))
                    {
                        return RedirectToAction("EIndex", "Admin");
                    }

                }
            }

            return View(login);
        }


        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Registration signup)
        {
            if (ModelState.IsValid)
            {
                Registration user = new Registration()
                {
                    Name = signup.Name,
                    Password = signup.Password,
                    Role = signup.Role
                };

                db.Registrations.Add(user);
                db.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(signup);
        }
    }
}
