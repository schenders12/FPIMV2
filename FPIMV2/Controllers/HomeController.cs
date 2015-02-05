using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Net.Mail;
using System.Web.Routing;
using System.Web.Mvc.Ajax;
using System.Web.Security;
using System.IO;
using Newtonsoft.Json;

using FPIMV2.Models;
using FPIMV2.Helpers;

namespace FPIMV2.Controllers
{
    public class HomeController : Controller
    {
        private PeopleEntities db = new PeopleEntities();
        // *** Test page for logging in***
        [HttpGet]
        public ActionResult TestIndex()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TestIndex(string id, string atype, string target)
        {
            // register the authentication cookie with passed userid
            // create errata object to pass authentication method
            //user.username = id;
            //user.authtype = atype;
            FormsAuthentication.SetAuthCookie(id, true);
            return RedirectToAction("Index", new { id = id });
        }
        // Default page
        public ActionResult Index(string id, string suad, string esfad, string firstname, string lastname)
        {
            if (id == null || id == "")
            {
                // Viewing your own
                id = HttpContext.User.Identity.Name;
            }

            // Get Employee record
            CommEmployee employee = fns.GetEmployee(id, suad, esfad, firstname, lastname);
            AddFacStaff addFacStaff = null;
            if (employee == null)
            {
               // return null;
               addFacStaff = fns.GetAddFacStaff(id, suad, esfad, firstname, lastname);
            }

            FacultyProfile profile = db.FacultyProfiles.SingleOrDefault(m => m.UserId == id);
            if (profile == null)
            {
                // User does not have a profile, create one from existing data in ProfileErratta table
                // If a profile already exists with this last name, create firstname.lastname profile
                FacultyProfile p = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == lastname);
                string profileID = null;
                if (p != null)
                {
                    profileID = firstname + '.' + lastname;
                }
                else
                {
                    profileID = lastname;
                }
                // Create profile
                int result = db.spCreateFacultyProfile(profileID, id, suad, esfad, firstname, lastname);
                profile = db.FacultyProfiles.SingleOrDefault(m => m.UserId == id);

            }
            string firstName = (employee != null) ? employee.FirstName : addFacStaff.FirstName;
            string lastName = (employee != null) ? employee.LastName : addFacStaff.LastName;
            string middleName = (employee != null) ? employee.MiddleName : addFacStaff.MiddleName;
            string userId = (employee != null) ? employee.UserId : addFacStaff.UserId;

            if (id != User.Identity.Name)
            {
                ViewBag.WelcomeMsg = "Editing for " + firstName + " " + lastName;
            }
            else
            {
                ViewBag.WelcomeMsg = "Welcome, " + firstName + " " + lastName + "!";
            }

            ViewBag.fname = firstName;
            ViewBag.lname = lastName;
            ViewBag.userid = userId;
            if (middleName != "")
                ViewBag.PTitle = "SUNY-ESF: " + firstName + " " + middleName + " " + lastName + " - " + "Profile";
            else
                ViewBag.PTitle = "SUNY-ESF: " + firstName + " " + lastName + " - " + "Profile";
            ViewBag.PageTitle = "Profile";

            ViewBag.profileId = profile.ProfileId;

            // Get the main page ID
            List<FacultyPage> pages = db.FacultyPages.Where(m => m.ProfileId == profile.ProfileId).ToList();
            FacultyPage page = pages.Find(m => m.PageTitle == "Main");
            ViewBag.MainPageID = page.FacultyPageId;
            return View(profile);
        }

        // Default page
        public ActionResult AdminIndex(string profileId)
        {
            FacultyProfile profile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            // Get Employee record
            CommEmployee employee = fns.GetEmployee(profile.UserId, profile.SUAD, profile.ESFAD);
            AddFacStaff addFacStaff = null;
            if (employee == null)
            {
                // return null;
                addFacStaff = fns.GetAddFacStaff(profile.UserId, profile.SUAD, profile.ESFAD );
            }
            string firstName = (employee != null) ? employee.FirstName : addFacStaff.FirstName;
            string lastName = (employee != null) ? employee.LastName : addFacStaff.LastName;
            string middleName = (employee != null) ? employee.MiddleName : addFacStaff.MiddleName;
            string userId = (employee != null) ? employee.UserId : addFacStaff.UserId;

            if (profile.UserId != User.Identity.Name)
            {
                ViewBag.WelcomeMsg = "Editing for " + firstName + " " + lastName;
            }
            else
            {
                ViewBag.WelcomeMsg = "Welcome, " + firstName + " " + lastName + "!";
            }

            ViewBag.fname = firstName;
            ViewBag.lname = lastName;
            ViewBag.userid = userId;
            if (middleName != "")
                ViewBag.PTitle = "SUNY-ESF: " + firstName + " " + middleName + " " + lastName + " - " + "Profile";
            else
                ViewBag.PTitle = "SUNY-ESF: " + firstName + " " + lastName + " - " + "Profile";
            ViewBag.PageTitle = "Profile";

            ViewBag.profileId = profile.ProfileId;

            // Get the main page ID
            List<FacultyPage> pages = db.FacultyPages.Where(m => m.ProfileId == profile.ProfileId).ToList();
            FacultyPage page = pages.Find(m => m.PageTitle == "Main");
            ViewBag.MainPageID = page.FacultyPageId;
            return View("Index", profile);
        }


        // *** Manage File functions ***
        // *** Add and delete documents ***
        [HttpGet]
        public ActionResult ManageFiles(string profileId)
        {
            Console.Write("Managing files...");
            ViewBag.profileID = profileId;
            return View();
        }

        // *** Link to a file ***
        [HttpGet]
        public ActionResult LinkToFile(string profileId)
        {
            ViewBag.profileID = profileId;
            //return View();
            return PartialView();
        }

        // *** Submit a photo ***
        [HttpGet]
        public ActionResult SubmitPhoto(string profileId)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            // Get Employee record
            CommEmployee employee = fns.GetEmployee(myProfile.UserId, myProfile.SUAD, myProfile.ESFAD);
            AddFacStaff addFacStaff = null;
            if (employee == null)
            {
                // return null;
                addFacStaff = fns.GetAddFacStaff(myProfile.UserId, myProfile.SUAD, myProfile.ESFAD);
            }
            string firstName = (employee != null) ? employee.FirstName : addFacStaff.FirstName;
            string lastName = (employee != null) ? employee.LastName : addFacStaff.LastName;
            string eMail = (employee != null) ? employee.EmailId : addFacStaff.EmailId;

            ProfilePhoto myPhoto = new ProfilePhoto(profileId, firstName, lastName, eMail);
            return View(myPhoto);
        }
        [HttpPost]
        public ActionResult SubmitPhoto(ProfilePhoto myPhoto)
        {
            myPhoto.profileId = Request.Form["profileId"];
            myPhoto.profileFirstName = Request.Form["profileFirstName"];
            myPhoto.profileLastName = Request.Form["profileLastName"];
            myPhoto.profileEMail = Request.Form["profileEMail"];

            // Create the Mail message
            MailMessage mail = new MailMessage();
            // Get the attachment
            if (myPhoto.Attachment != null && myPhoto.Attachment.ContentLength > 0)
            {
                var attachment = new Attachment(myPhoto.Attachment.InputStream, myPhoto.Attachment.FileName);
                mail.Attachments.Add(attachment);
            }

            // string strFileName = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
            // string attachmentPath = Environment.CurrentDirectory + @"\test.txt";
            // if (strFileName != "")
            // {
            //     Attachment item = new Attachment(FileUpload1.PostedFile.InputStream, strFileName);
            //     mail.Attachments.Add(item);
            // }

            //Set From , To and CC
            var name = myPhoto.profileFirstName + " " + myPhoto.profileLastName;
            mail.From = new MailAddress("annonymous@esf.edu", name);
            mail.To.Add(new MailAddress("schender@esf.edu"));
            mail.To.Add(new MailAddress("web@esf.edu"));
            mail.CC.Add(new MailAddress(myPhoto.profileEMail));
            // mail.Bcc.Add(new MailAddress("schender@esf.edu"));
            mail.Subject = "New Profile Photo from " + name;
            // mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));

            // Fill in body info
            // string Locationstr = Location.Text;
            // string Emailstr = Email.Text;
            //string Groupstr = Group.Text;

            mail.Body = @"Please use this new picture as my profile photo.  " + "\n";

            // Create the email client and send mail
            SmtpClient smtpClient = new SmtpClient("149.119.6.91", 25);
            smtpClient.Send(mail);

            return RedirectToAction("AdminIndex", new { profileId = myPhoto.profileId });

            /*   if (Request.Files.Count > 0)
               {
                   foreach (string file in Request.Files)
                   {
                       string pathFile = string.Empty;
                       if (file != null)
                       {
                           string path = string.Empty;
                           string fileName = string.Empty;
                           string fullPath = string.Empty;
                           path = AppDomain.CurrentDomain.BaseDirectory + "directory where you want to upload file";//here give the directory where you want to save your file
                           if (!System.IO.Directory.Exists(path))//if path do not exit
                           {
                               System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "directory_name/");//if given directory dont exist, it creates with give directory name
                           }
                           fileName = Request.Files[file].FileName;

                           fullPath = Path.Combine(path, fileName);
                           if (!System.IO.File.Exists(fullPath))
                           {
                               if (fileName != null && fileName.Trim().Length > 0)
                               {
                                   Request.Files[file].SaveAs(fullPath);
                               }
                           }
                       }
                   }
               }*/

        }


        // *** Manage Page functions ***
        [HttpGet]
        public ActionResult ManagePages(string profileId)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            ViewBag.profileId = profileId;
            return View(myProfile);
        }

        // *** Add a page ***
       // [Authorize]
        public ActionResult AddFacultyPage(string profileID)
        {
            FacultyPage myPage = new FacultyPage();
            myPage.ProfileId = profileID;
            myPage.FacultyPageId = -1;
            myPage.PageTitle = "";

            // DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~\faculty\" + myPage.ProfileId));
            // string fullPath = Server.MapPath("~/faculty/" + myPage.ProfileId + "/");
            // ViewBag.LinkURL = fullPath;
            ViewBag.LinkURL = "";
            Console.Write("Adding New Page");

            FacultyProfile profile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileID);
            profile.FacultyPages.Add(myPage);

            return View(myPage);
        }
        // *** Add an external page ***
        public ActionResult AddFacultyExternalLink(string profileID)
        {
            FacultyPage myPage = new FacultyPage();
            myPage.ProfileId = profileID;
            myPage.FacultyPageId = -1;
            myPage.PageTitle = "";
            Console.Write("Adding New External Page");

            FacultyProfile profile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileID);
            profile.FacultyPages.Add(myPage);

            return View(myPage);
        }

        [HttpPost]
        // *** Add a page ***
        // public ActionResult AddFacultyPage(FacultyPage myPage)
        public ActionResult AddFacultyPage(List<string> items, string title, string profileID)
        {
            FacultyPage myPage = new FacultyPage();
            myPage.PageTitle = title;
            myPage.ProfileId = profileID;

            // Set the link URL
            // myPage.LinkURL = myPage.LinkURL + myPage.PageTitle;

            // Loop over all added modules
            // string modules = Request.Form["addMe"];
            myPage.FacultyProfileModules = new EntityCollection<FacultyProfileModule>();
            if (items != null)
            {
                //  var parts = modules.Split(',');
                foreach (var item in items)
                {
                    int selectedModuleId = Convert.ToInt32(item);
                    // Retrieve the data from the existing module and create a new module
                    FacultyProfileModule oldMod = db.FacultyProfileModules.First(m => m.FacultyProfileModuleId == selectedModuleId);
                    FacultyProfileModule newMod = new FacultyProfileModule();

                    newMod.ProfileId = myPage.ProfileId;
                    newMod.FacultyPageId = oldMod.FacultyPageId;
                    newMod.ModuleType = oldMod.ModuleType;
                    newMod.ModuleTitle = oldMod.ModuleTitle;
                    newMod.ModuleData = oldMod.ModuleData;
                    newMod.DisplayOrder = oldMod.DisplayOrder;

                    // Add the module dta to the page
                    myPage.FacultyProfileModules.Add(newMod);

                }
            }
            // Add the Page to the database.  Modules added based on Entity Framework Foreign Key relationship.
            if (ModelState.IsValid)
            {
                db.FacultyPages.AddObject(myPage);
                db.SaveChanges();
            }
            // return RedirectToAction("ManagePages", new { profileId = profileID });
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ManagePages", new { profileId = profileID });
            return Json(new { Url = redirectUrl });
        }

        [HttpPost]
        // *** Add an External page ***
        public ActionResult AddFacultyExternalLink(FacultyPage myPage)
        {
            if (ModelState.IsValid)
            {
                db.FacultyPages.AddObject(myPage);
                db.SaveChanges();
            }
            return RedirectToAction("AdminIndex", new { profileId = myPage.ProfileId });
        }

        // *** Edit a Page ***
        [HttpGet]
        public ActionResult EditFacultyPage(string profileId, int pageId)
        {
            FacultyPage page = db.FacultyPages.First(m => m.FacultyPageId == pageId);
            return View(page);
        }

        [HttpPost]
        public ActionResult EditFacultyPage(List<string> items, string title, int id)
        {
            FacultyPage page = new FacultyPage();
            page = db.FacultyPages.First(m => m.FacultyPageId == id);
            page.PageTitle = title;

            // for each profile, save the display order
            List<FacultyProfileModule> modules = db.FacultyProfileModules.Where(m => m.FacultyPageId == id).ToList();
            foreach (FacultyProfileModule module in modules)
            {
                int index = items.IndexOf(module.FacultyProfileModuleId.ToString());
                module.DisplayOrder = index;
            }
            if (ModelState.IsValid)
            {
                db.SaveChanges();
            }
            //return RedirectToAction("ManagePages", new { profileId = page.ProfileId });
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("ManagePages", new { profileId = page.ProfileId });
            return Json(new { Url = redirectUrl });
        }



        // *** Delete a Page ***
        [HttpGet]
        public ActionResult DeletePagePartial(string pageId)
        {
            int selectedPageId = Convert.ToInt32(pageId);
            FacultyPage myPage = db.FacultyPages.First(m => m.FacultyPageId == selectedPageId);
            return PartialView("DeletePagePartial", myPage);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeletePagePartial(FacultyPage myPage)
        {
            if (ModelState.IsValid)
            {
                // Get all the modules contained in this page and delete
                List<FacultyProfileModule> modules = db.FacultyProfileModules.Where(m => m.FacultyPageId == myPage.FacultyPageId).ToList();
                foreach (FacultyProfileModule module in modules)
                {
                    db.DeleteObject(module);
                    if (ModelState.IsValid)
                    {
                        db.SaveChanges();
                    }
                }
                // Delete the page
                FacultyPage pageUpdate = new FacultyPage();
                pageUpdate = db.FacultyPages.Single(m => m.FacultyPageId == myPage.FacultyPageId);
                db.DeleteObject(pageUpdate);
                db.SaveChanges();
            }
            return RedirectToAction("ManagePages", new { profileId = myPage.ProfileId });
           // var redirectUrl = new UrlHelper(Request.RequestContext).Action("ManagePages", new { profileId = myPage.ProfileId });
            //return Json(new { Url = redirectUrl });
        }

        // *** Choose a Page Type ***
        [HttpGet]
        public ActionResult ChoosePageTypePartial(string profileID)
        {
            ViewBag.profileID = profileID;
            return PartialView("ChoosePageTypePartial");
        }
        [HttpPost]
        public ActionResult ChoosePageTypePartial()
        {
            string profileID = Request.Form["profileID"];
            string modType = Request.Form["PageType"];
            switch (modType)
            {
                case "Internal":
                    return RedirectToAction("AddFacultyPage", new { profileID = profileID });
                case "External":
                    return RedirectToAction("AddFacultyExternalLink", new { profileID = profileID });
                default:
                    return RedirectToAction("AdminIndex", new { profileID = profileID });
            }
        }


        // *** Manage Module functions***
        [HttpGet]
        public ActionResult ManageModules(string profileId)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            ViewBag.profileId = profileId;
            return View(myProfile);
        }
        // *** Choose a Module Type ***
        [HttpGet]
        public ActionResult ChooseModuleTypePartial(string profileID)
        {
            ViewBag.profileID = profileID;
            return PartialView("ChooseModuleTypePartial");
        }
        [HttpPost]
        public ActionResult ChooseModuleTypePartial()
        {
            string profileID = Request.Form["profileID"];
            string modType = Request.Form["ModuleType"];

            switch (modType)
            {
                case "HTML":
                    return RedirectToAction("AddHTMLModule", new { profileID = profileID });
                case "GradModule":
                    return RedirectToAction("AddGradModule", new { profileID = profileID });
                default:
                    return RedirectToAction("ManageModules", new { profileId = profileID });
            }
        }


        // *** Add an HTML Module ***
        [HttpGet]
        public ActionResult AddHTMLModule(string profileID)
        {
            FacultyProfile profile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileID);
            FacultyProfileModule htmlMod = new FacultyProfileModule();
            htmlMod.ProfileId = profileID;
            htmlMod.FacultyPageId = -1;
            htmlMod.ModuleType = "HTML";
            htmlMod.DisplayOrder = -1;

            profile.FacultyProfileModules.Add(htmlMod);
            Console.Write("Adding Faculty Module of type HTML");

            return View(htmlMod);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddHTMLModule(FacultyProfileModule module)
        {
            if (ModelState.IsValid)
            {
                db.FacultyProfileModules.AddObject(module);
                db.SaveChanges();
            }
            return RedirectToAction("ManageModules", new { profileId = module.ProfileId });
        }

        // *** Add Grad Module ***
        [HttpGet]
        public ActionResult AddGradModule(string profileID)
        {
            FacultyProfileModule fileMod = new FacultyProfileModule();
            fileMod.ProfileId = profileID;
            fileMod.FacultyPageId = -1;
            fileMod.ModuleType = "Grad";
            fileMod.DisplayOrder = -1;
            Console.Write("Adding Grad Faculty Module...");

            return View(fileMod);
            // return null;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddGradModule(FacultyProfileModule module)
        {
            if (ModelState.IsValid)
            {
                db.FacultyProfileModules.AddObject(module);
                db.SaveChanges();
            }
            return RedirectToAction("ManageModules", new { profileId = module.ProfileId });
        }

        // *** Edit a Module ***
        [HttpGet]
        public ActionResult EditFacultyModule(string profileId, int moduleId)
        {
            //int selectedModuleId = Convert.ToInt32(id);
            FacultyProfileModule module = db.FacultyProfileModules.First(m => m.FacultyProfileModuleId == moduleId);
            return View("EditFacultyModule", module);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditFacultyModule(FacultyProfileModule module)
        {
            if (ModelState.IsValid)
            {
                FacultyProfileModule modUpdate = new FacultyProfileModule();
                modUpdate = db.FacultyProfileModules.Single(m => m.FacultyProfileModuleId == module.FacultyProfileModuleId);

                // Save original title
                var originalTitle = modUpdate.ModuleTitle;

                modUpdate.ModuleData = module.ModuleData;
                modUpdate.ModuleTitle = module.ModuleTitle;
                modUpdate.ModuleType = module.ModuleType;

                // Update all modules with identical titles
                List<FacultyProfileModule> modules = db.FacultyProfileModules.Where(m => m.ModuleTitle == originalTitle).ToList();
                foreach (FacultyProfileModule mod in modules)
                {
                    if (mod.ModuleType == module.ModuleType)
                    {
                        mod.ModuleData = module.ModuleData;
                        mod.ModuleTitle = module.ModuleTitle;
                        mod.ModuleType = module.ModuleType;
                    }
                }

                db.SaveChanges();
            }
            return RedirectToAction("ManageModules", new { profileId = module.ProfileId });
        }

        // *** Delete a Module ***
        [HttpGet]
        public ActionResult DeleteModulePartial(string id)
        {
            int selectedModuleId = Convert.ToInt32(id);
            FacultyProfileModule module = db.FacultyProfileModules.First(m => m.FacultyProfileModuleId == selectedModuleId);
            return PartialView("DeleteModulePartial", module);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteModulePartial(FacultyProfileModule module)
        {
            if (ModelState.IsValid)
            {
                // Get a list of all the modules with this Title for this user
                List<FacultyProfileModule> modules = db.FacultyProfileModules.Where(m => m.ProfileId == module.ProfileId).Where(m => m.ModuleTitle == module.ModuleTitle).ToList();
                // Delete the module
                foreach (FacultyProfileModule pageModule in modules)
                {
                    db.DeleteObject(pageModule);
                    if (ModelState.IsValid)
                    {
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ManageModules", new { profileId = module.ProfileId });
        }
        [HttpGet]
        public JsonResult GetModules(string myId)
        {
            // Get all distinct modules based on title
            //var myModules = db.FacultyProfileModules.Select(m => m.ModuleTitle).Distinct().ToList();

            // Return all modules for this profile ID that have no page assignment
            var allMyModules = db.FacultyProfileModules.Select(m => new
            {
                m.FacultyProfileModuleId,
                m.FacultyPageId,
                m.ProfileId,
                m.ModuleType,
                m.ModuleTitle,
                m.ModuleData
            }).Where(m => m.ProfileId == myId).Where(m => m.FacultyPageId == -1).ToList();

            //var results = (from ta in allMyModules
            //                select ta.ModuleTitle).Distinct();
            return Json(allMyModules, JsonRequestBehavior.AllowGet);
        }

        // View/Preview pages
        // *** View a Page ***
        [HttpGet]
        public ActionResult ViewMainFacultyPage(string profileId, string fname, string lname)
        {
            return null;

        }
        // *** View a Page ***
        [HttpGet]
        public ActionResult ViewFacultyPage(int pageId, string profileId, string fname, string lname)
        {
            // Redirect to viewer 
            FacultyProfile profile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            // Get Employee record
            CommEmployee employee = fns.GetEmployee(profile.UserId, profile.SUAD, profile.ESFAD, fname, lname);
            if (employee == null)
            {
                return null;
            }

            ViewBag.fname = employee.FirstName;
            ViewBag.lname = employee.LastName;
            ViewBag.userid = employee.UserId;
            if (employee.MiddleName != "")
                ViewBag.PTitle = "SUNY-ESF: " + employee.FirstName + " " + employee.MiddleName + " " + employee.LastName + " - " + "Profile";
            else
                ViewBag.PTitle = "SUNY-ESF: " + employee.FirstName + " " + employee.LastName + " - " + "Profile";
            ViewBag.PageTitle = "Main";

            ViewBag.profileId = profile.ProfileId;

            List<FacultyPage> pages = db.FacultyPages.Where(m => m.ProfileId == profileId).ToList();
            FacultyPage page = pages.Find(m => m.FacultyPageId == pageId);
            // Set the department to display the correct banner
            ControllerContext.RouteData.Values["dept"] = profile.Department;
            ViewBag.PageId = pageId;
            ViewBag.dept = profile.Department;

            if (page.LinkURL == null || page.LinkURL == "")
            {
                return View(profile);
            }
            else
            {
                return Redirect(page.LinkURL);
            }
        }

        [HttpGet]
        public JsonResult GetMainPage(string profileId, string pageTitle)
        {
            // Get all distinct modules based on title
            //var myModules = db.FacultyProfileModules.Select(m => m.ModuleTitle).Distinct().ToList();

            // Return all modules for this profile ID that have no page assignment
            var myMainPage = db.FacultyPages.Select(m => new
            {
                m.FacultyPageId,
                m.ProfileId,
                m.PageTitle
            }).Where(m => m.ProfileId == profileId).Where(m => m.PageTitle == pageTitle).ToList();

            //var results = (from ta in allMyModules
            //                select ta.ModuleTitle).Distinct();
            return Json(myMainPage, JsonRequestBehavior.AllowGet);
        }

        // Faculty/Staff Dept Associations/Areas of Study (AOS)
        [HttpGet]
        public ActionResult ManageDeptAssocAOS(string profileId)
        {
            // Get profile
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);

            // Get Employee record
            CommEmployee employee = fns.GetEmployee(myProfile.UserId, myProfile.SUAD, myProfile.ESFAD);
            AddFacStaff addFacStaff = null;
            if (employee == null)
            {
                // return null;
                addFacStaff = fns.GetAddFacStaff(myProfile.UserId, myProfile.SUAD, myProfile.ESFAD);
            }
            string firstName = (employee != null) ? employee.FirstName : addFacStaff.FirstName;
            string lastName = (employee != null) ? employee.LastName : addFacStaff.LastName;

            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;

            List<FacultyDept> myDepts = db.FacultyDepts.Where(m => m.UserId == myProfile.UserId).ToList();
            List<spFetchFacultyAOSAssocList_Result> myAOSs = db.spFetchFacultyAOSAssocList(myProfile.UserId, myProfile.ESFAD, myProfile.SUAD).ToList();

            ViewBag.myDepts = myDepts;
            ViewBag.myAOSs = myAOSs;

            return View(myProfile);
        }
        // Faculty/Staff Dept Association
        [HttpGet]
        public ActionResult EditDeptAssoc(string profileId)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            ViewBag.profileId = myProfile.ProfileId;
            ViewBag.userID = myProfile.UserId;

            return View(myProfile);
        }

        [HttpPost]
        public ActionResult EditDeptAssoc(FacultyProfile passedProfile)
        {
            string dept = Request.Form["dept"];
            string id = Request.Form["userID"];
            string profileId = Request.Form["profileID"];
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.UserId == id);

            // Loop over all checked boxes
            if (dept != null)
            {
                // Remove existing associations
                db.spRemoveFDs(myProfile.UserId, myProfile.ESFAD, myProfile.SUAD);

                // Create new associations
              //  var deptId = 0;
                var parts = dept.Split(',');
                foreach (var part in parts)
                {
                    // Create a new DB entry
                    FacultyDept newDept = new FacultyDept();
                    newDept.UserId = myProfile.UserId;
                    newDept.ESFAD = myProfile.ESFAD;
                    newDept.SUAD = myProfile.SUAD;
                    newDept.Dept = part;
                    newDept.ForceTop = false;
                    newDept.AOSCode = "";

                    // Add object to DB
                    db.FacultyDepts.AddObject(newDept);
                    db.SaveChanges();
                   // deptId++;
                }
            }
            return RedirectToAction("AdminIndex", new { profileId = myProfile.ProfileId });
        }
        // Faculty/Staff Dept Association
        [HttpGet]
        public ActionResult EditDeptAOS(string profileId)
        {
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            ViewBag.profileId = myProfile.ProfileId;
            ViewBag.userID = myProfile.UserId;
            ViewBag.dept = myProfile.Department;
            ViewBag.esfad = myProfile.ESFAD;
            ViewBag.reconciledAreas = myProfile.ReconciledAreas;

            List<FacultyDept> myDepts = db.FacultyDepts.Where(m => m.UserId == myProfile.UserId).ToList();
            ViewBag.myDepts = myDepts;
            return View(myProfile);
        }
        [HttpPost]
        public ActionResult EditDeptAOS(FacultyProfile passedProfile)
        {
            string aos = Request.Form["assoc"];
            string id = Request.Form["userID"];

            string profileId = Request.Form["profileID"];
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.UserId == id);

            // Delete existing AOSs
            List<FacultyAreasXRef> xRefs = db.FacultyAreasXRefs.Where(m => m.UserId == myProfile.UserId).ToList();
            foreach (FacultyAreasXRef xRef in xRefs)
            {
                    db.DeleteObject(xRef);
                    if (ModelState.IsValid)
                    {
                        db.SaveChanges();
                    }
            }

            // Add new AOSs 
            if (aos != null)
            {
                var assocId = 0;
                var parts = aos.Split(',');
                foreach (var part in parts)
                {
                    // Create a new DB entry
                    FacultyAreasXRef newAOS = new FacultyAreasXRef();
                    newAOS.UserId = myProfile.UserId;
                    newAOS.ESFAD = myProfile.ESFAD;
                    newAOS.SUAD = myProfile.SUAD;
                    newAOS.AOSCode = part;

                    // Get participating areas (if any)
                   // string aosParAreas = part + "_PA";
                   // string participatingAreas = Request.Form[aosParAreas];
                   // newAssocAOS.ParticipatingAreas = participatingAreas;

                    // Add object to DB
                    db.FacultyAreasXRefs.AddObject(newAOS);
                    db.SaveChanges();
                    assocId++;
                }
            } 

            return RedirectToAction("AdminIndex", new { profileId = profileId });

        }

        [HttpGet]
        public JsonResult GetAOSCodes()
        {
            var myAOSCodes = db.CodesAOS.ToList();
            return Json(myAOSCodes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDeptAssoc(string userID)
        {
            // Return all departments for this user
            var myDepts = db.FacultyDepts.Select(m => new
            {
                m.UserId,
                m.ESFAD,
                m.SUAD,
                m.Dept
            }).Where(m => m.UserId == userID).ToList();

            //var results = (from ta in allMyModules
            //                select ta.ModuleTitle).Distinct();
            return Json(myDepts, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDeptAOS(string userID, string ESFAD, string SUAD)
        {
            // Return all areas of study for this user
            List<spFetchFacultyAOSAssocList_Result> myAOS = db.spFetchFacultyAOSAssocList(userID, ESFAD, SUAD).ToList();
            return Json(myAOS, JsonRequestBehavior.AllowGet);
        }

        // Return URL for image (needed for testing locally
        public ActionResult GetImage(string profileId)
        {
            FileInfo photoFile = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(@"~\faculty\profiles\" + profileId + ".jpg"));

            //DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~\faculty\profiles\" + profileId));
            //var root = @"~\faculty\profiles\";
            // var path = Path.Combine(root, profileId);
            //path = Path.GetFullPath(path);
            // if (!path.StartsWith(root))
            // {
            // Ensure that we are serving file only inside the root folder
            // and block requests outside like "../web.config"
            //    throw new HttpException(403, "Forbidden");
            //}
            System.Diagnostics.Debug.WriteLine("Getting file from " + photoFile.FullName);
            ViewBag.Message = ("Getting file from " + photoFile.FullName);
            return File(photoFile.FullName, "image/jpeg");

        }
        [HttpGet]
        public ActionResult AdministerContent(string profileId)
        {
            List<spFacultyList_Result> MyList = db.spFacultyList("").ToList();
            ViewBag.ProfileId = profileId;
            return View(MyList);

        }

        // Add/Delete/Update Non-HR faculty
        [HttpGet]
        public ActionResult ManageNonHRFaculty(string profileId)
        {
            IEnumerable<AddFacStaff> myList = db.AddFacStaffs.ToList();
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            return View(Tuple.Create(myList, myProfile));
        }

        [HttpGet]
        public ActionResult ManuallyAddFaculty(string profileId)
        {
            AddFacStaff facultyAdd = new AddFacStaff();
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            return View(Tuple.Create(facultyAdd, myProfile));
        }

        [HttpPost]
        public ActionResult ManuallyAddFaculty(Tuple<AddFacStaff, FacultyProfile> tuple)
        {
            string fac = "F";
            if (ModelState.IsValid)
            {
                AddFacStaff facultyUpdate = new AddFacStaff();
                // Save data
                facultyUpdate.UserId = tuple.Item1.UserId;
                facultyUpdate.LastName = tuple.Item1.LastName;
                facultyUpdate.FirstName = tuple.Item1.FirstName;
                facultyUpdate.MiddleName = (tuple.Item1.MiddleName != null) ? tuple.Item1.MiddleName : "";
                facultyUpdate.Suffix = (tuple.Item1.Suffix != null) ? tuple.Item1.Suffix : "";
                facultyUpdate.OffcBldg = (tuple.Item1.OffcBldg != null) ? tuple.Item1.OffcBldg : "";
                facultyUpdate.OffcRoom = (tuple.Item1.OffcRoom != null) ? tuple.Item1.OffcRoom : "";
                facultyUpdate.OffcPhone = (tuple.Item1.OffcPhone != null) ? tuple.Item1.OffcPhone : "";
                facultyUpdate.SpeedDialExt = (tuple.Item1.SpeedDialExt != null) ? tuple.Item1.SpeedDialExt : "";
                facultyUpdate.MailAddrBldg = (tuple.Item1.MailAddrBldg != null) ? tuple.Item1.MailAddrBldg : "";
                facultyUpdate.MailAddrRoom = (tuple.Item1.MailAddrRoom != null) ? tuple.Item1.MailAddrRoom : "";
                facultyUpdate.AltOffcPhone = (tuple.Item1.AltOffcPhone != null) ? tuple.Item1.AltOffcPhone : "";
                facultyUpdate.EmailId = (tuple.Item1.EmailId != null) ? tuple.Item1.EmailId : "";
                facultyUpdate.CampusTitle = (tuple.Item1.CampusTitle != null) ? tuple.Item1.CampusTitle : "";
                facultyUpdate.WorkLoc = (tuple.Item1.WorkLoc != null) ? tuple.Item1.WorkLoc : "";
                facultyUpdate.FacStaff = fac;
                facultyUpdate.CivilServiceTitle = (tuple.Item1.CivilServiceTitle != null) ? tuple.Item1.CivilServiceTitle : "";
                facultyUpdate.ESFAD = (tuple.Item1.ESFAD != null) ? tuple.Item1.ESFAD : "";
                facultyUpdate.SUAD = (tuple.Item1.SUAD != null) ? tuple.Item1.SUAD : "";

                db.AddFacStaffs.AddObject(facultyUpdate);
                db.SaveChanges();
            }
            return RedirectToAction("ManageNonHRFaculty", new { profileId = tuple.Item2.ProfileId });

        }

        [HttpGet]
        public ActionResult ManuallyUpdateFaculty(string userId, string profileId)
        {
            AddFacStaff facultyMod = db.AddFacStaffs.SingleOrDefault(m => m.UserId == userId);
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            return View(Tuple.Create(facultyMod, myProfile));
        }

        [HttpPost]
        public ActionResult ManuallyUpdateFaculty(Tuple<AddFacStaff, FacultyProfile> tuple)
        {
            string fac = "F";
            if (ModelState.IsValid)
            {
                AddFacStaff facultyUpdate = db.AddFacStaffs.Single(m => m.UserId == tuple.Item1.UserId);
                // Save modified data
                facultyUpdate.LastName = tuple.Item1.LastName;
                facultyUpdate.FirstName = tuple.Item1.FirstName;
                facultyUpdate.MiddleName = (tuple.Item1.MiddleName != null) ? tuple.Item1.MiddleName : "";
                facultyUpdate.Suffix = (tuple.Item1.Suffix != null) ? tuple.Item1.Suffix : "";
                facultyUpdate.OffcBldg = (tuple.Item1.OffcBldg != null) ? tuple.Item1.OffcBldg : "";
                facultyUpdate.OffcRoom = (tuple.Item1.OffcRoom != null) ? tuple.Item1.OffcRoom : "";
                facultyUpdate.OffcPhone = (tuple.Item1.OffcPhone != null) ? tuple.Item1.OffcPhone : "";
                facultyUpdate.SpeedDialExt = (tuple.Item1.SpeedDialExt != null) ? tuple.Item1.SpeedDialExt : "";
                facultyUpdate.MailAddrBldg = (tuple.Item1.MailAddrBldg != null) ? tuple.Item1.MailAddrBldg : "";
                facultyUpdate.MailAddrRoom = (tuple.Item1.MailAddrRoom != null) ? tuple.Item1.MailAddrRoom : "";
                facultyUpdate.AltOffcPhone = (tuple.Item1.AltOffcPhone != null) ? tuple.Item1.AltOffcPhone : "";
                facultyUpdate.EmailId = (tuple.Item1.EmailId != null) ? tuple.Item1.EmailId : "";
                facultyUpdate.CampusTitle = (tuple.Item1.CampusTitle != null) ? tuple.Item1.CampusTitle : "";
                facultyUpdate.WorkLoc = (tuple.Item1.WorkLoc != null) ? tuple.Item1.WorkLoc : "";
                facultyUpdate.FacStaff = fac;
                facultyUpdate.CivilServiceTitle = (tuple.Item1.CivilServiceTitle != null) ? tuple.Item1.CivilServiceTitle : "";
                facultyUpdate.ESFAD = (tuple.Item1.ESFAD != null) ? tuple.Item1.ESFAD : "";
                facultyUpdate.SUAD = (tuple.Item1.SUAD != null) ? tuple.Item1.SUAD : "";

                db.SaveChanges();
            }
             return RedirectToAction("ManageNonHRFaculty", new { profileId = tuple.Item2.ProfileId });
        }

        [HttpGet]
        public ActionResult ManuallyDeleteFaculty(string userId, string profileId)
        {
            AddFacStaff facultyMod = db.AddFacStaffs.SingleOrDefault(m => m.UserId == userId);
            FacultyProfile myProfile = db.FacultyProfiles.SingleOrDefault(m => m.ProfileId == profileId);
            return View(Tuple.Create(facultyMod, myProfile));

        }
        [HttpPost]
        public ActionResult ManuallyDeleteFaculty(Tuple<AddFacStaff, FacultyProfile> tuple)
        {
            if (ModelState.IsValid)
            {
                AddFacStaff fac = db.AddFacStaffs.Single(m => m.UserId == tuple.Item1.UserId);
                db.AddFacStaffs.DeleteObject(fac);
                db.SaveChanges();
            }
            return RedirectToAction("ManageNonHRFaculty", new { profileId = tuple.Item2.ProfileId });

        }
    }

}
