using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPIMV2.Models
{
    public class TupleModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext,
                  ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType == typeof(Tuple<AddFacStaff, FacultyProfile>))
                return new Tuple<AddFacStaff, FacultyProfile>(new AddFacStaff(), new FacultyProfile());
         //   if (modelType == typeof(Tuple<ContactModel, CommunicationModel, AddressModel, CustomerModel>))
         //       return new Tuple<ContactModel, CommunicationModel, AddressModel, CustomerModel>(new ContactModel(), new CommunicationModel(), new AddressModel(), new CustomerModel());

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            string actionName = controllerContext.RouteData.Values["action"].ToString();

            // Only check views with Tuples
            if ((actionName == "ManuallyDeleteFaculty") || (actionName == "ManuallyUpdateFaculty") || (actionName == "ManuallyAddFaculty") || (actionName == "ManageNonHRFaculty"))
            {

                if (propertyDescriptor.Name == "ProfileId")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item2.ProfileId"]);
                    return;
                }

                if (propertyDescriptor.Name == "UserId")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.UserId"]);
                    return;
                }
                if (propertyDescriptor.Name == "FirstName")
                {
                    //     DateTime dt = new DateTime(int.Parse(controllerContext.HttpContext.Request.Form["year"]),
                    // int.Parse(controllerContext.HttpContext.Request.Form["month"]),
                    // int.Parse(controllerContext.HttpContext.Request.Form["day"]));
                    //   propertyDescriptor.SetValue(bindingContext.Model, dt);
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.FirstName"]);
                    return;
                }
                if (propertyDescriptor.Name == "LastName")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.LastName"]);
                    return;
                }
                if (propertyDescriptor.Name == "MiddleName")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.MiddleName"]);
                    return;
                }
                if (propertyDescriptor.Name == "Suffix")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.Suffix"]);
                    return;
                }
                if (propertyDescriptor.Name == "OffcBldg")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.OffcBldg"]);
                    return;
                }
                if (propertyDescriptor.Name == "OffcRoom")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.OffcRoom"]);
                    return;
                }
                if (propertyDescriptor.Name == "OffcPhone")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.OffcPhone"]);
                    return;
                }
                if (propertyDescriptor.Name == "SpeedDialExt")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.SpeedDialExt"]);
                    return;
                }
                if (propertyDescriptor.Name == "MailAddrBldg")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.MailAddrBldg"]);
                    return;
                }
                if (propertyDescriptor.Name == "MailAddrRoom")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.MailAddrRoom"]);
                    return;
                }
                if (propertyDescriptor.Name == "AltOffcPhone")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.AltOffcPhone"]);
                    return;
                }
                if (propertyDescriptor.Name == "EmailId")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.EmailId"]);
                    return;
                }
                if (propertyDescriptor.Name == "CampusTitle")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.CampusTitle"]);
                    return;
                }
                if (propertyDescriptor.Name == "WorkLoc")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.WorkLoc"]);
                    return;
                }
                if (propertyDescriptor.Name == "CivilServiceTitle")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.CivilServiceTitle"]);
                    return;
                }
                if (propertyDescriptor.Name == "ESFAD")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.ESFAD"]);
                    return;
                }
                if (propertyDescriptor.Name == "SUAD")
                {
                    propertyDescriptor.SetValue(bindingContext.Model, controllerContext.HttpContext.Request.Form["Item1.SUAD"]);
                    return;
                }
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}