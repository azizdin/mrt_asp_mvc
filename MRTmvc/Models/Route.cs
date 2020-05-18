using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MRTmvc.Models
{
    public class Route
    {
        [Key]
        [Display(Name = "Store ID")]
        public int StoreId { get; set; }

        [Display(Name = "User ID")]
        public int UserID { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Trip Type")]
        [Required(ErrorMessage = "Please select your trip type")]
        public string tripType { get; set; }

        [Display(Name = "Level Category")]
        [Required(ErrorMessage = "Please select your category")]
        public string LevelCategory { get; set; }

        [Display(Name = "Station From")]
        [Required(ErrorMessage = "Please select your origin station")]
        public string StationFrom { get; set; }

        [Display(Name = "Station To")]
        [Required(ErrorMessage = "Please select your destination station")]
        public string StationTo { get; set; }

        [Display(Name = "Number Of Ticket")]
        [Required(ErrorMessage = "Please select your number of pax")]
        public string NumTicket { get; set; }

        [Display(Name = "Date And Time")]
        public String DateTime { get; set; }

        [DisplayFormat(DataFormatString = "RM{0:n2}")]
        public Decimal Charge { get; set; }

        [Display(Name = "Discount Percentage")]
        public string DiscountPercent { get; set; }
    }


    public class Register
    {

        [Key]
        [Display(Name = "Register ID")]
        public int RegId { get; set; }

        [Display(Name ="Name")]
        [Required(ErrorMessage = "Please enter your username")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress()]
        public string Email { get; set; }

        [Display(Name = "IC Number")]
        [Required(ErrorMessage = "Please enter your IC/Passport number")]
        public string Nric { get; set; }

        [Display (Name ="Password")]
        [Required(AllowEmptyStrings = false ,ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password1 { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your confirmation password")]
        [DataType(DataType.Password)]
        [Compare("Password1", ErrorMessage = "Your password not match")]
        public string Password2 { get; set; }

    }

   
    public class Login
    {
        [Display(Name = "Login ID")]
        [Key]
        public int LoginId { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter email address")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter your password")]
        public string Password1 { get; set; }
    }
}