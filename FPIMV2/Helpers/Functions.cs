using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.IO;

using FPIMV2.Models;

namespace FPIMV2.Helpers
{
    public class fns
    {

        // Faculty system helper functions
        #region fpimFns
        // NiceTitle
        //  Filters information out of titles for public display.
        public static string NiceTitle(string input)
        {
            string output = "";
            if (input != null)
            {
                switch (input)
                {
                    case "Professor-10 Months":
                        output = "Professor";
                        break;
                    case "Assistant Professor-10 Months":
                        output = "Assistant Professor";
                        break;
                    case "Assistant Professor-12 Months":
                        output = "Assistant Professor";
                        break;
                    case "Assistant Professor-12 mo":
                        output = "Assistant Professor";
                        break;
                    case "Associate Professor-10 Months":
                        output = "Associate Professor";
                        break;
                    case "Visiting Asst Professor 10M":
                        output = "Visiting Assistant Professor";
                        break;
                    case "Assistant Librarian 08":
                        output = "Assistant Librarian";
                        break;
                    case "Visiting Assoc Professor 10M":
                        output = "Visiting Associate Professor";
                        break;
                    case "Disting Teach Prof Emeritus":
                        output = "Distinguished Teaching Professor Emeritus";
                        break;
                    case "Professor-12 Months":
                        output = "Professor";
                        break;
                    case "Prof Emeritus":
                        output = "Professor Emeritus";
                        break;
                    case "Provost & VP Academic Affairs and Prof Emeritus":
                        output = "Professor Emeritus";
                        break;
                    case "Instructor-10 Months":
                        output = "Instructor";
                        break;
                    case "Visiting Instructor-10 months":
                        output = "Visiting Instructor";
                        break;
                    case "Visiting Professor 12 Mo.":
                        output = "Visiting Professor";
                        break;
                    case "Visiting Professor 10M":
                        output = "Visiting Professor";
                        break;
                    case "Visiting Professor-10 Months":
                        output = "Visiting Professor";
                        break;
                    case "Associate Professor-12 Months":
                        output = "Associate Professor";
                        break;
                    case "Visiting Associate Professor-10 Months":
                        output = "Visiting Associate Professor";
                        break;
                    case "Visiting Assistant Professor 10M":
                        output = "Visiting Assistant Professor";
                        break;
                    case "Visiting Assistant Professor-10 Months":
                        output = "Visiting Assistant Professor";
                        break;
                    case "Visiting Assistant Professor-12 Months":
                        output = "Visiting Assistant Professor";
                        break;
                    case "Distinguished Teach Prof 12 Mo":
                        output = "Distinguished Teaching Professor";
                        break;
                    case "Distinguished Teaching Professor-12 Months":
                        output = "Distinguished Teaching Professor";
                        break;
                    case "Distinguished Service Professor-12 Months":
                        output = "Distinguished Service Professor";
                        break;
                    case "Visiting Instructor 10M":
                        output = "Visiting Instructor";
                        break;
                    case "Lecturer 10 months":
                        output = "Lecturer";
                        break;
                    case "Disting Service Prof Emeritus":
                        output = "Distinguished Service Professor Emeritus";
                        break;
                    case "Dist Service Prof 12":
                        output = "Distinguished Service Professor";
                        break;
                    case "Chair and Professor 12":
                        output = "Chair and Professor";
                        break;
                    default:
                        output = input; // put in input for further processing below.
                        break;
                }

                output.Replace("Asst ", "Assistant ");
                output.Replace("Chr", "Chair");
                output.Replace("Chem ", "Chemistry");
                output.Replace("Cel Res Ins", "Cellulose Research Institute");
                output.Replace("Chemistry& ", "Chemistry & ");
                output.Replace("Chemistryistry ", "Chemistry");
                output.Replace("Cnt", "Center");
                output.Replace("Ctr", "Center");
                output.Replace("Centr ", "Center ");
                output.Replace("Mgmt ", "Management ");
                output.Replace(" and Renewal Energy", " and Renewable Energy");
                output.Replace(" and Professor 12", " and Professor");
                output.Replace(" Professor 12", " Professor");
                output.Replace(" Professor 10", " Professor");
                output.Replace("Polymer Res Institute", "Polymer Research Institute");
                output.Replace("Sr ", "Senior ");
                output.Replace("Dir ", "Director, ");
                output.Replace("&Dir,", "& Director,");
                output.Replace("Dir. ", "Director, ");
                output.Replace("Dir, ", "Director, ");
                output.Replace("Great Lakes Research Con ", "Great Lakes Research Consortium ");
                output.Replace("Sust Construction", "Sustainable Construction");
                output.Replace("Env Res & For Eng", "Environmental Resources Engineering");
                output.Replace("AEC", "Adirondack Ecological Center");
                output.Replace("Adir Ecol Ctr", "Adirondack Ecological Center");
                output.Replace("Adir Ecol Center", "Adirondack Ecological Center");
                output.Replace("Sust Construction Mgmt", "Sustainable Construction Management and Engineering");
                output.Replace("ES/", "Environmental Studies & ");
                output.Replace("RG Pack Env. Inst", "Randolph G. Pack Environmental Institute");
                output.Replace("Environmental Resources & Forest Engineering", "Environmental Resources Engineering");
                output.Replace("Associate (cy)", "Associate");
                output.Replace(" mo", "");
                return output;
            }
            else
            {
                output = "Nada";
                return output;
            }
        }
        // SpecifyTitle
        //  Specify a title by last name
        public static string SpecifyTitle(string title, string lname)
        {
            string output = "";
            if (lname != null)
            {
                switch (lname)
                {
                    case "Marko":
                    case "Daley":
                    case "Endreny":
                    case "Gerber":
                    case "Kroll":
                    case "Tully":
                        output = title + " & P.E.";
                        break;
                    case "Werner":
                        output = "Visiting Instructor & P.Eng.";
                        break;
                    case "Castello":
                        output = "Professor and Associate Chair";
                        break;
                    case "Briggs":
                        output = "Distinguished Teaching Professor and Director, Division of Environmental Science";
                        break;
                    case "McGee":
                        output = "Undergraduate Curriculum Director and Assistant Professor";
                        break;
                    case "Hawks":
                        output = "SUNY Distinguished Service Professor and William M. Kennedy, Jr. Faculty Chair";
                        break;
                    case "Curry":
                        output = "Distinguished Teaching Professor Emeritus";
                        break;
                    case "Luzadis":
                    case "Anagnost":
                        output = "Chair and Professor";
                        break;
                    default:
                        output = title; // return original title
                        break;
                }
                return output;
            }
            else
            {
                output = "Nada";
                return output;
            }
        }

        public static string FacultyPageHeading(string UserId, string fname, string lname, string dept=null)
        {
            string output = "";
            // Open database context
            PeopleEntities db = new PeopleEntities();
            // Execute SP to get list of advisees for professor
            var ph = db.spFacultyHeading(UserId, fname, lname);
            foreach (spFacultyHeading_Result item in ph)
            {
                // Should only ever be one. Hopefully.
                var name = "";
                if (item.PreferredName != null)
                {
                    // Format: Last, First (MI)
                    name = item.PreferredName.Replace(", ", " ");
                    var parts = name.Split(' ');
                    name = "";
                    for (var i = 1; i < parts.Length; i++)
                    {
                        name = name + parts[i] + " ";
                    }
                    name = name + " " + parts[0];
                }
                else
                {
                    if (item.MiddleName == "" || item.MiddleName == null)
                    {
                        name = item.FirstName + " " + item.LastName;
                    }
                    else
                    {
                        name = item.FirstName + " " + item.MiddleName + " " + item.LastName;
                    }

                }
                string title = "";
                string phone = "";
               // if (item.CampusTitle == item.CivilServiceTitle)
               // {
               //     title = fns.NiceTitle(item.CampusTitle);
               // }
                title = fns.FixTitle(item.CampusTitle.Trim(), item.CivilServiceTitle.Trim(), item.LastName);
                phone = fns.FixPhone(item.OffcPhone, item.AltOffcPhone);
                if ((dept == "Env Studies -Writing Ctr") || (dept == "writing"))
                {
                    output = "<div id='content' class='navcol'>" + fns.ProfilePhoto(item.UserId, item.FirstName, item.LastName) + "<h1>" + name + "<br /><span class='twelve'><strong>" + title + "</strong></span></h1>" + "<p>" + item.OffcRoom + " " + fns.BldgName(item.OffcBldg.Trim()) + "<br />1 Forestry Dr.<br />Syracuse, New York 13210</p>" + "<p>Phone: " + phone + "<br />Email: <a href='mailto:" + item.EmailId + "'>" + item.EmailId + "</a>";
                }
                else
                {
                    output = fns.ProfilePhoto(item.UserId, item.FirstName, item.LastName) + "<h1>" + name + "<br /><span class='twelve'><strong>" + title + "</strong></span></h1>" + "<p>" + item.OffcRoom + " " + fns.BldgName(item.OffcBldg.Trim()) + "<br />1 Forestry Dr.<br />Syracuse, New York 13210</p>" + "<p>Phone: " + phone + "<br />Email: <a href='mailto:" + item.EmailId + "'>" + item.EmailId + "</a>";
                }
            }
            return output;
        }
        public static string FixTitle(string CampusTitle, string CivilServiceTitle, string lastName)
        {
            string output = "";
            if (CampusTitle == CivilServiceTitle)
            {
                output = fns.NiceTitle(CampusTitle);
            }
            else
            {
                string cTitle = fns.NiceTitle(CampusTitle);
                string lineBreak = "<br />";
                string csTitle = fns.NiceTitle(CivilServiceTitle);
                output = cTitle + lineBreak + csTitle;
            }
            output = fns.SpecifyTitle(output, lastName);
            return output;
        }
        // Phone number filter
        public static string FixPhone(string phoneno, string altphoneno)
        {
            if ((phoneno == "") || (altphoneno == ""))
            {
                return "";
            }
            string fixedno = "";
            if (phoneno == altphoneno || altphoneno == "" || altphoneno == null) // same, or alternate empty
            {
                fixedno = "(" + phoneno.Substring(0, 3) + ")" + phoneno.Substring(3, 3) + "-" + phoneno.Substring(6);
                return fixedno;
            }
            else // something in alternate, and different from primary
            {
                if (phoneno == "" || phoneno == null)
                {
                    fixedno = "(" + altphoneno.Substring(0, 3) + ")" + altphoneno.Substring(3, 3) + "-" + altphoneno.Substring(6);
                    return fixedno;
                }
                else
                {
                    string primary = "(" + phoneno.Substring(0, 3) + ") " + phoneno.Substring(3, 3) + "-" + phoneno.Substring(6);
                    string alt = "(" + altphoneno.Substring(0, 3) + ") " + altphoneno.Substring(3, 3) + "-" + altphoneno.Substring(6);

                    if (phoneno.StartsWith("315") && altphoneno.StartsWith("315"))
                    {
                        fixedno = primary + "/" + alt.Substring(6);
                    }
                    else
                    {
                        fixedno = primary + "/" + alt;
                    }
                }
            }
            return fixedno;
        }
        // Building name filter
        public static string BldgName(string bldg)
        {
            switch (bldg)
            {
                case "BAKER":
                    return "Baker Lab";
                case "ILLICK":
                    return "Illick Hall";
                case "JAHN":
                    return "Jahn Lab";
                case "BRAY":
                    return "Bray Hall";
                case "MARSHALL":
                    return "Marshall Hall";
                case "MOON LIB":
                    return "Moon Library";
                case "NEWCOMB":
                    return "Newcomb Campus";
                case "WALTERS":
                    return "Walters Hall";
                case "WANAKENA":
                    return "Wanakena Campus";
                default:
                    return bldg;
            }
        }
        // Faculty Profile Photo
        public static string ProfilePhoto(string userId, string fname, string lname)
        {
             FileInfo photoFile = new FileInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/faculty/profiles/"), userId + ".jpg"));
            if (!photoFile.Exists)
            {
                return "<img class='right mobhide' src='/images/clear.gif' alt='" + fname + " " + lname + "' />";
            }
            else
            {
                return "<img class='right mobhide' src='/faculty/profiles/" + userId + ".jpg'" + " alt='" + fname + " " + lname + "' />";
            }
        }
        // Reads in HTML Include Files from the server
        public static HtmlString HTMLFile(string fileName)
        {
           // FileInfo fileInfo = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(@"~\" + fileName));
            //FileInfo fileInfo = new FileInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/"), fileName));
            //FileInfo fileInfo = new FileInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/"), fileName));
            FileInfo fileInfo = new FileInfo(System.Web.HttpContext.Current.Server.MapPath(fileName));

           // if (!fileInfo.Exists)
           // {
           //     return null;
          //  }
          //  else
           // {
                // load from file
                //var filePath = HttpContext.Current.Server.MapPath(@"~\" + fileName);
                //var filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/"), fileName);
                var filePath = System.Web.HttpContext.Current.Server.MapPath(fileName);
                using (var streamReader = File.OpenText(filePath))
                {
                    var markup = streamReader.ReadToEnd();
                    return new HtmlString(markup);
                }


          //  }
        }
        // Graduate student Advisees module
        public static string AdviseesModule(string prof, string type)
        {
            if (prof == null || prof == "")
            {
                return "";
            }
            else
            {
                var headerlines = "";
                var closinglines = "";
                var set = "";
                var entries = "";
                // Open database context
                PeopleEntities db = new PeopleEntities();
                // Execute SP to get list of advisees for professor
                var advisees = db.spAdviseesPublic(prof);
                foreach (var item in advisees)
                {
                    var filepath = "E:\\Servers\\WWW\\forms\\pim\\gs\\" + item.UserId + ".jpg";
                    FileInfo PhotoFile = new FileInfo(filepath);
                    var studentphoto = "";
                    if (!PhotoFile.Exists)
                    {
                        studentphoto = "<h4 style='margin-top: 10px'><img class='right' src='http://www.esf.edu/images/clear.gif' alt='" + item.FirstNm + " " + item.LastNm + "' />" + item.FirstNm + " " + item.LastNm;
                    }
                    else
                    {
                        studentphoto = "<h4 style='margin-top: 10px'><img class='right' src='http://www.esf.edu/forms/pim/gs/" + item.UserId + ".jpg' alt='" + item.FirstNm + " " + item.LastNm + "' />" + item.FirstNm + " " + item.LastNm;
                    }
                    if (item.SEmailID)
                    {
                        studentphoto = studentphoto + "<br /><span class='ten'><a href='mailto:" + item.EmailId.Trim() + "'>" + item.EmailId.Trim() + "</a></span></h4>";
                    }
                    else
                    {
                        studentphoto = studentphoto + "</h4>";
                    }
                    entries = entries + studentphoto;
                    var degree = "";
                    // Degree information, if they allow it
                    if (item.SDegProg)
                    {
                        degree = "<li><strong>Degree Sought</strong>: " + item.DegProg;
                        if (item.AreaOfStudy == "" && item.CorrectAOS == "")
                        {
                            if (item.ProgStudy != "")
                            {
                                degree = degree + " in " + item.ProgStudy;
                            }
                            else
                            {
                                degree = degree + "</li>";
                            }
                        }
                    }
                    // Major Professor information
                    if (item.SMajorProf)
                    {
                        // CLEAN UP presentation layer - currently all caps ProfName
                        // ADD links
                        var mp = fns.FixMPName(item.MajorProf.Trim());
                        degree = degree + "<li><strong>Graduate Advisor(s):</strong> " + mp.Substring(0, 1) + mp.Substring(1).ToLower();

                        if (item.CoMajorProf != "" && item.CoMajorProf != null)
                        {
                            var cmp = fns.FixMPName(item.CoMajorProf.Trim());
                            degree = degree + " and " + cmp.Substring(0, 1) + cmp.Substring(1).ToLower() + "</li>";
                        }
                        else
                        {
                            degree = degree + "</li>";
                        }
                    }
                    // Area of study information
                    if (item.SAreaOfStudy)
                    {
                        // If Corrected AOS filled in, use it.
                        if (item.CorrectAOS != null && item.CorrectAOS != "" && item.CorrectAOS != item.AreaOfStudy)
                        {
                            degree = degree + "<li><strong>Area of Study</strong>: " + fns.FixProgramOfStudy(item.CorrectAOS) + "</li>";
                        }
                        else
                        {
                            if (item.AreaOfStudy.Length > 0)
                            {
                                var fix = item.AreaOfStudy.Substring(4); // Trim prefix 
                                degree = degree + "<li><strong>Area of Study</strong>: " + fns.FixProgramOfStudy(fix) + "</li>";
                            }
                        }
                    }
                    // Undergraduate information
                    if (item.SBaccDegCollNm)
                    {
                        degree = degree + "<li><strong>Undergraduate Institute</strong>: " + fns.FixInstitution(item.BaccDegCollNm);
                        if ((bool)item.SBaccDegCurr)
                        {
                            degree = degree + " (" + fns.FixMajor(item.BaccDegCurr) + ")</li>";
                        }
                        else
                        {
                            degree = degree + "</li>";
                        }
                    }
                    // Previous Graduate Study
                    if (item.SGradDegCollNm)
                    {
                        if (item.CorrectGradInst != null || item.GradDegCollNm != "")
                        {
                            degree = degree + "<li><strong>Previous Graduate Study</strong>: ";
                            if (item.CorrectGradInst != null && item.CorrectGradInst != "")
                            {
                                degree = degree + fns.FixInstitution(item.CorrectGradInst);
                            }
                            else
                            {
                                degree = degree + fns.FixInstitution(item.GradDegCollNm);
                            }
                            if ((bool)item.SGradDegCurr)
                            {
                                if (item.CorrectGradMajor != null && item.CorrectGradMajor != "")
                                {
                                    degree = degree + " (" + fns.FixMajor(item.CorrectGradMajor) + ")</li>";
                                }
                                else
                                {
                                    degree = degree + " (" + fns.FixMajor(item.GradDegCurr) + ")</li>";
                                }
                            }
                        }
                    }
                    entries = entries + "<ul>" + degree + "</ul>";
                    var theirentries = db.spCurrViewFields(item.UserId);
                    foreach (var entry in theirentries)
                    {
                        if (entry.FieldDisplay)
                        {
                           // if ((entry.FieldContents.Substring(0, 3) != "<a ") && (entry.FieldContents.Substring(0, 7) == "http://"))
                            string fieldContents = entry.FieldContents;
                            string link = "<a ";
                            string http = "http://";
                            string www = "www";
                            if ((entry.FieldContents.Contains(link)) && (entry.FieldContents.Contains(http)))
                            {
                                entries = entries + "<p><strong>" + entry.FieldName + "</strong><br /><a href='" + entry.FieldContents + "'>Web Link</a></p>";
                            }
                            else if (entry.FieldContents.Contains(www))
                            {
                                entries = entries + "<p><strong>" + entry.FieldName + "</strong><br /><a href='http://" + entry.FieldContents + "'>Web Link</a></p>";
                            }
                            else
                            {
                                entries = entries + "<p><strong>" + entry.FieldName + "</strong><br />" + entry.FieldContents + "</p>";
                            }
                        }
                    }
                }

                switch (type)
                {
                    case "full":
                        headerlines = "<hr class='clearer' /><div id='borderbox' class='bggray'><h3 style='background-image: url(http://www.esf.edu/includes/gpim/bgfull.jpg); background-position: right; background-repeat: no-repeat'><a href='http://www.esf.edu/graduate/' style='border: none'><img src='http://www.esf.edu/images/clear.gif' style='margin: 0; padding: 0; width: 280px; height: 24px; float: right' /></a>Current Graduate Advisees</h3><div style='clear: both'>";
                        closinglines = "</div></div>";
                        set = headerlines + entries + closinglines;
                        return set;
                    case "rightnarrow":
                        headerlines = "<hr class='clearer' /><div id='sideright' style='width: 420px; clear: right;' class='bggray' ><h3 style='background-image: url(http://www.esf.edu/includes/gpim/bgnarrow.jpg); background-position: right; background-repeat: no-repeat'>Graduate Advisees</h3><div style='clear: both;'>";
                        closinglines = "</div></div>";
                        set = headerlines + entries + closinglines;
                        return set;
                    case "rightwide":
                        headerlines = "<hr class='clearer' /><div id='sideright' style='width: 550px; clear: right;' class='bggray' ><h3 style='background-image: url(http://www.esf.edu/includes/gpim/bg.jpg); background-position: right; background-repeat: no-repeat'><a href='http://www.esf.edu/graduate/' style='border: none'><img src='http://www.esf.edu/images/clear.gif' style='margin: 0; padding: 0; width: 280px; height: 24px; float: right' /></a>Graduate Advisees</h3><div style='clear: both;'>";
                        closinglines = "</div></div>";
                        set = headerlines + entries + closinglines;
                        return set;
                    case "left":
                        headerlines = "<hr class='clearer' /><div id='sideleft' style='width: 550px; clear: left;' class='bggray' ><h3 style='background-image: url(http://www.esf.edu/includes/gpim/bg.jpg); background-position: right; background-repeat: no-repeat'><a href='http://www.esf.edu/graduate/' style='border: none'><img src='http://www.esf.edu/images/clear.gif' style='margin: 0; padding: 0; width: 280px; height: 24px; float: right' /></a>Graduate Advisees</h3><div style='clear: both;'>";
                        closinglines = "</div></div>";
                        set = headerlines + entries + closinglines;
                        return set;
                    default:
                        return ""; // Not properly formatted.
                }
                //return prof;
            }
        }
        // Filter junk out of Program of Study
        public static string FixProgramOfStudy(string ps)
        {
            switch (ps)
            {
                case "EFB Fish & Wildlife Biology & Mgt":
                    return "EFB Fish & Wildlife Biology & Management";
                case "FCH Organic Chem of Natural Products":
                    return "FCH Organic Chemistry of Natural Products";
                case "EAS Forest Engineering":
                    return "";
                case "EAS Paper and Bioprocess Engineering":
                    return "";
                case "CERT Bioprocessing":
                    return "Bioprocessing";
                default:
                    break;
            }
            if (ps != null)
            {
                ps = ps.Replace("Environ Systems & Risk Management", "Environmental Systems and Risk Management");
                ps = ps.Replace("Environ Policy & Dem Processes", "Environmental Policy and Democratic Processes");
                ps = ps.Replace("Environ & ", "Environmental and ");
                ps = ps.Replace("Environ Comm & Participatory Processes", "Environmental Communication and Participatory Processes");
                return ps;
            }
            else
            {
                return "";
            }
        }
        // Filter for Institution name
        public static string FixInstitution(string inst)
        {
            inst = inst.Replace("Universidad Nacional de Colomb", "Universidad Nacional de Colombia");
            inst = inst.Replace("Univ,", "University,");
            inst = inst.Replace("Univ of ", "University of ");
            inst = inst.Replace("Univ ", "University of ");
            inst = inst.Replace("Coll Geneseo", "College at Geneseo");
            inst = inst.Replace("Coll ", "College ");
            inst = inst.Replace(" Univ ", " University ");
            return inst;
        }
        // Filter for Major
        public static string FixMajor(string maj)
        {
            if (maj != "Environmental Science")
                maj = maj.Replace("Environmental Scienc", "Environmental Science");
            if (maj != "Public Administration")
                maj = maj.Replace("Public Administraio", "Public Administration");
            if (maj == "Electrical Engineeri")
                maj = "Electical Engineering";
            maj = maj.Replace("Land Arch", "Landscape Architecture");
            return maj;
        }
        // Fix Major Professor name
        public static string FixMPName(string mp)
        {
            Regex rgx = new Regex("[M][C]+[BCDFGHJKLMNPQRSTVWXYZ]+");
            var ism = rgx.IsMatch(mp);
            if (ism)
            {
                return "Mc" + mp.Substring(3, 1) + mp.Substring(4).ToLower();
            }
            else
            {
                return mp;
            }
            //return mp;
        }

        // Process Associations
        public static string ProcessAssociation(string id, string SUAD, string ESFAD, string assoc)
        {
            // Open database context
            PeopleEntities db = new PeopleEntities();
            // Execute SP to remove all associations
            // TBD var result = db.spRemoveFDs(id, ESFAD, SUAD);

            // Execute SP to add associations
            // Split assoc into string
            if (assoc != null)
            {
               var parts = assoc.Split(',');
               foreach (var part in parts)
               {
                  //result = db.spAssocFac(id, ESFAD, SUAD, part);
                  // FacultyAssocAOS myAssocAOS = new (db.FacultyAssocAOSs);
                  // myAssocAOS.ProfileId = id;
                 //  myAssocAOS.AOSCode = part;
                  //TBD  myAssocAOS.InterestAreas = intAreas;
                   // TBD myAssocAOS.ParticipatingAreas = partAreas;
               }
            }

            return "";
        }

        // Faculty Catalog module
        public static string CatalogModule(string prof, string type, string year)
        {
            return "";
        }

        // Submit a profile photo
        public static bool SubmitPhoto(string to, string from, string cc, string subject, string body, string photoFile)
        {
            MailMessage message = new MailMessage();
            try
            {
                // email to list is comma separated. 
                message.To.Add(to + "," + cc);
                message.From = new MailAddress(from);
                //message.CC = new MailAddress(cc);
                message.Subject = subject;
                message.Body = body;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.SubjectEncoding = System.Text.Encoding.UTF8;

                // Get the attachment
                //string strFileName = System.IO.Path.GetFileName(FileUpload1.PostedFile.FileName);
                // string attachmentPath = Environment.CurrentDirectory + @"\test.txt";
              //  if (photoFile != "")
              //  {
             //       Attachment item = new Attachment(FileUpload1.PostedFile.InputStream, strFileName);
             //       // item.Name = ("C\\Users\\schender\\test.txt");
             //       mail.Attachments.Add(item);
             //   }

                SmtpClient client = new SmtpClient();
                client.Host = "149.119.140.44";
                client.Port = 25;
                client.UseDefaultCredentials = false;
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Get an employee record from the DB
        public static CommEmployee GetEmployee(string userId, string suad, string esfad, string firstname = null, string lastname = null)
        {
            PeopleEntities db = new PeopleEntities();
            CommEmployee employee;
            try
            {
                // Get the employee record by user id
                // User has a profile, look up Employee record
                employee = db.CommEmployees.SingleOrDefault(m => m.UserId == userId || m.EmailId == esfad + "@esf.edu%" || m.EmailId == suad + "@syr.edu%");
                //employee = db.CommEmployees.SingleOrDefault(m => m.UserId == userId);
                if (employee == null)
                {
                    // User does not have a user id, find the employee by first name/last name
                    employee = db.CommEmployees.Single(m => m.FirstName == firstname && m.LastName == lastname);
                }
                return employee;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // 
        // GetMyAOSs
        //  List of AOS Codes for a profile
        // 
        public static string GetMyAOSs(string userid, string ESFAD, string SUAD)
        {
            PeopleEntities db = new PeopleEntities();

            List<spFetchFacultyAOSAssocList_Result> myAOSs = db.spFetchFacultyAOSAssocList(userid, ESFAD, SUAD).ToList();
            string thelist = "<ul>";
            foreach (spFetchFacultyAOSAssocList_Result aos in myAOSs)
            {
                // thelist = thelist + "<li><a href='/FPIMV2/Spaces/ViewPending/" + pend.ResId + "/'>" + pend.EventName + "</a> (" + pend.EventDate.ToShortDateString() + " " + pend.StartTime.ToShortTimeString() + ")";
                 thelist = thelist + "<li>" + aos.AreaOfStudy + "</li>";
            }
            if (thelist == "<ul>")
            {
                thelist = thelist + "<li>No Areas of Study at this time.</li></ul>";
            }
            else
            {
                thelist = thelist + "</ul>";
            }
            return thelist;
        }
        // 
        // GetMyDepts
        //  List of Department Associations for a profile
        // 
        public static string GetMyDepts(string userid, string ESFAD, string SUAD)
        {
            PeopleEntities db = new PeopleEntities();

            List<FacultyDept> myDepts = db.FacultyDepts.Where(m => m.UserId == userid).ToList();
            string thelist = "<ul>";
            foreach (FacultyDept dept in myDepts)
            {
                // thelist = thelist + "<li><a href='/FPIMV2/Spaces/ViewPending/" + pend.ResId + "/'>" + pend.EventName + "</a> (" + pend.EventDate.ToShortDateString() + " " + pend.StartTime.ToShortTimeString() + ")";
                thelist = thelist + "<li>" + dept.Dept + "</li>";
            }
            if (thelist == "<ul>")
            {
                thelist = thelist + "<li>No Departmental associations at this time.</li></ul>";
            }
            else
            {
                thelist = thelist + "</ul>";
            }
            return thelist;
        }
        public static int GetMainPage(string profileId, string pageTitle)
        {
            int mypage = 0;

            // Get all distinct modules based on title
            PeopleEntities db = new PeopleEntities();

            List<FacultyPage> myPages = db.FacultyPages.Where(m => m.ProfileId == profileId).Where(m => m.PageTitle == pageTitle).ToList();
            //Should return only one but search through anyway
            foreach (FacultyPage page in myPages)
            {
                // Return page number
                mypage = page.FacultyPageId;
            }
            return mypage;
        }
        #endregion

        // Menu functions
        #region menuFns
        public static string hijackRequest(string target, string Username, string useDomain)
        {
            string redirect = "";
            switch (target)
            {
                case "news":
                    redirect = "http://www.esf.edu/communications/contribute/admit.asp?u=" + Username + "&atype=" + useDomain;
                    break;
                case "newsadmin":
                    redirect = "http://www.esf.edu/communications/admin/admit.asp?u=" + Username;
                    break;
                case "grad":
                    redirect = "http://www.esf.edu/forms/pim/welcome.asp?u=" + Username + "&atype=" + useDomain + "&f=b";
                    break;
                case "spaces":
                    redirect = "http://www.esf.edu/FPIMV2/Spaces/";
                    break;
                case "spacesadmin":
                    redirect = "http://www.esf.edu/FPIMV2/Spaces/Admin/";
                    break;
                case "cocadmin":
                    redirect = "http://www.esf.edu/coc/admin/admit.asp?u=" + Username;
                    break;
                case "worldadmin":
                    redirect = "http://www.esf.edu/world/admin/admit.asp?u=" + Username;
                    break;
                case "fpim":
                    redirect = "http://www.esf.edu/faculty/admin/admit.asp?u=" + Username;
                    break;
                case "identity":
                    redirect = "http://www.esf.edu/forms/communications/logo/default.asp?u=" + Username;
                    break;
                case "bcards":
                    redirect = "http://www.esf.edu";
                    break;
                default:
                    redirect = "";
                    break;
            }
            return redirect;
        }

        #endregion

        // Global system helper functions
        #region globals

        //
        // SendEmail
        //  Function for sending notification emails
        // 
        public static bool SendEmail(string to, string from, string cc, string subject, string body)
        {
            MailMessage message = new MailMessage();
            try
            {
                // email to list is comma separated. 
                message.To.Add(to + "," + cc);
                message.From = new MailAddress(from);
                //message.CC = new MailAddress(cc);
                message.Subject = subject;
                message.Body = body;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.SubjectEncoding = System.Text.Encoding.UTF8;

                SmtpClient client = new SmtpClient();
                client.Host = "149.119.140.44";
                client.Port = 25;
                client.UseDefaultCredentials = false;
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        // SystemMessage
        public static string SystemMessage(string shortmessage)
        {
            switch (shortmessage)
            {
                case "Entered":
                    return "Your reservation request has been entered successfully. You will receive an email confirming your reservation request, and another when the request is acted upon.";
                case "EditedPending":
                    return "Your reservation has been modified successfully, but the changes will require secondary approval. You will receive an email when your request is acted upon.";
                case "EditedAdmin":
                    return "The reservation has been modified successfully.";
                case "Edited":
                    return "Your reservation has been modified successfully, and does not require secondary approval. If your event had not yet been approved, you will receive an email when your request is acted upon.";
                case "CancelSuccess":
                    return "Your reservation has been canceled successfully. You will receive an email confirming this cancelation.";
                case "CancelFail": // Cancellation failure, notification failed
                    return "An error has occurred in canceling your reservation. Please report this error to Aaron Knight at x6648.";
                case "DenySuccess":
                    return "The reservation has been denied successfully, and an email confirming this cancelation has been sent.";
                case "DenyFail": // Cancellation failure, notification failed
                    return "An error has occurred in denying this reservation. Please report this error to Aaron Knight at x6648.";
                default:
                    return "";
            }
        }

        // Override to replace form null strings with string.empty for null-disallowed fields
        public sealed class EmptyStringModelBinder : DefaultModelBinder
        {
            public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                    bindingContext.ModelMetadata.ConvertEmptyStringToNull = false;
                try
                {
                    return base.BindModel(controllerContext, bindingContext);

                }
                catch ( HttpRequestValidationException e)
                {
                return base.BindModel(controllerContext, bindingContext);

                }
            }
        }

        public static string BannerImage(string controller, string dept)
        {
            switch (controller)
            {
                case "Spaces":
                    return "http://www.esf.edu/FPIMV2/Content/banners/spaces.jpg";
                case "Home":
                    return "http://www.esf.edu/web/images/banner.jpg";
                case "FPIM":
                    if (dept != "")
                    {
                        return GetDeptBannerImage(dept);
                    }
                    else
                    {
                        return "http://www.esf.edu/faculty/admin/images/banner.jpg";
                    }
                default:
                    return "http://www.esf.edu/web/images/banner.jpg";
            }
        }

        public static string GetDeptBannerImage(string dept)
        {
            switch (dept.Trim())
            {
                case "Environmental Resources Engineering":
                case "ere":
                    return "http://www.esf.edu/ere/images/banners/960pumpkin.jpg";
                case "Environ & Forest Bio":
                case "efb":
                    return "http://www.esf.edu/efb/images/banners/960butterfly.jpg";
                case "envsci":
                    return "http://www.esf.edu/environmentalscience/images/960solarguys.jpg";
                case "Environ Studies":
                case "es":
                    return "http://www.esf.edu/es/images/banners/960windturbines.jpg";
                case "Forest & Natural Resources Management":
                case "fnrm":
                    return "http://www.esf.edu/fnrm/images/banners/960happyfaces.jpg";
                case "Landscape Architecture":
                case "la":
                    return "http://www.esf.edu/la/images/banners/960postergroup.jpg";
                case "Moon Library":
                case "lib":
                    return "http://www.esf.edu/moonlib/images/banner.jpg";
                case "Paper and Bioprocess Engineering":
                case "pbe":
                    return "http://www.esf.edu/pbe/images/banners/960bluelab.jpg";
                case "Forest Tech Prog - Wanakena":
                case "rs":
                    return "http://www.esf.edu/rangerschool/images/banner.jpg";
                case "Sustainable Construction Mgmt and Engineering":
                case "scme":
                    return "http://www.esf.edu/scme/images/banners/960bakerwalk.jpg";
                case "Chemistry":
                case "chem":
                    return "http://www.esf.edu/chemistry/images/banners/960tweezers.jpg";
                case "Env Studies -Writing Ctr":
                case "writing":
                    return "http://www.esf.edu/writingprogram/images/banner.jpg";
                default:
                    return "http://www.esf.edu/web/images/banner.jpg";
            }
        }

        public static HtmlString BannerMenu(string controller, string dept)
        {
            switch (controller)
            {
                case "Spaces":
                    return (fns.HTMLFile("/includes/FPIMV2_fpim.html"));
                case "Home":
                    return (fns.HTMLFile("/includes/FPIMV2_fpim.html"));
                case "FPIM":
                    if (dept != "")
                    {
                        return GetDeptBannerMenu(dept);
                    }
                    else
                    {
                        var ulHtml = "<ul id='droptab'>";
                        var ulEndHtml = "</ul>";
                        var menu = fns.HTMLFile("/includes/FPIMV2_fpim.html");
                        HtmlString menuHtml = new HtmlString(ulHtml + menu + ulEndHtml);
                        return (menuHtml);
                    }
                default:
                    return (fns.HTMLFile("/includes/FPIMV2_fpim.html"));
            }
        }

        public static HtmlString GetDeptBannerMenu(string dept)
        {
            switch (dept.Trim())
            {
                case "Environmental Resources Engineering":
                case "ere":
                    return (fns.HTMLFile("/ere/includes/tabbar.html"));
                case "Environ & Forest Bio":
                case "efb":
                    var ulHtml = "<ul id='droptab'>";
                    var ulEndHtml = "</ul>";
                    var menu = fns.HTMLFile("/efb/includes/menu.html");
                    HtmlString menuHtml = new HtmlString(ulHtml + menu + ulEndHtml);
                    return (menuHtml);
                case "envsci":
                    return(fns.HTMLFile("/environmentalscience/includes/tabbar.html"));
                case "Environ Studies":
                case "es":
                    return(fns.HTMLFile("/es/includes/tabbar.html"));
                case "Forest & Natural Resources Management":
                case "fnrm":
                    return(fns.HTMLFile("/fnrm/includes/tabbar.html"));
                case "Landscape Architecture":
                case "la":
                    return(fns.HTMLFile("/la/includes/tabbar.html"));
                case "Moon Library":
                case "lib":
                    return(fns.HTMLFile("/moonlib/includes/tabbar.html"));
                case "Paper and Bioprocess Engineering":
                case "pbe":
                    return(fns.HTMLFile("/pbe/includes/tabbar.html"));
                case "Forest Tech Prog - Wanakena":
                case "rs":
                    var ulRSHtml = "<ul id='droptab'>";
                    var ulRSEndHtml = "</ul>";
                    var rsMenu = fns.HTMLFile("/rangerschool/includes/menu.html");
                    HtmlString rsMenuHtml = new HtmlString(ulRSHtml + rsMenu + ulRSEndHtml);
                    return (rsMenuHtml);
                case "Sustainable Construction Mgmt and Engineering":
                case "scme":
                    return(fns.HTMLFile("/scme/includes/tabbar.html"));
                case "Chemistry":
                case "chem":
                    return(fns.HTMLFile("/chemistry/includes/tabbar.html"));
                case "Env Studies -Writing Ctr":
                case "writing":
                    return (null);
                default:
                    return(null);
            }
        }

        #endregion

    }
}