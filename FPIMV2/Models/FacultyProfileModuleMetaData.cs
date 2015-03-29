using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FPIMV2.Models
{
    public class ProfileModMetaData
    {
           [Required(ErrorMessage="Module Title is required")]
           [StringLength(100, ErrorMessage = "Module Title length Should be less than 100 characters")]
           public string ModTitle { get; set; }

          // [Required(ErrorMessage = "Please add content to this module, or cancel using the button at the lower left.")]
          // public string ModuleData { get; set; }
    }
    [MetadataType(typeof(ProfileModMetaData))]
    public partial class ProfileMod
    {
    }
}