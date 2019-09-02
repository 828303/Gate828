using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GateBoys.Models
{
    public class addInfo
    {

        [Key]
        public int MoreInfoId { get; set; }

        [Display(Name = "Info of Email")]
        public string addInfoOf { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }


        [Display(Name = "Middle Name")]
        public string midName { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string surname { get; set; }

        [Display(Name = "ID Number")]
        [Required]
        [MaxLength(13)]
        [MinLength(13)]
        [Range(13, Int64.MaxValue, ErrorMessage = "ID Number should not contain charecters and must be 13 digits")]
        public string idNum { get; set; }
        

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        [Range(10, Int64.MaxValue, ErrorMessage = "Phone number should not contain charecters and must be 10 digits")]
        [Display(Name = "Phone Number")]
        public string phone { get; set; }

        public string dateRegistered { get; set; }

        [Display(Name = "Country")]
        public string country { get; set; }

        [Display(Name = "Street Number")]
        public string street_number { get; set; }


        [Display(Name = "Street Name")]
        public string route { get; set; }

        [Display(Name = "Province")]
        public string administrative_area_level_1 { get; set; }


        [Display(Name = "City")]
        public string locality { get; set; }

        [Display(Name = "Code")]
        public string postal_code { get; set; }

        public string adress { get; set; }

        public string addressCMBN()
        {
            string ad = (country + " " + street_number + " " + route + " " + administrative_area_level_1 + " " + locality + " " + postal_code);
            return ad;
        }


        //public addInfo()
        //{
        //    this.bookingSomes = new HashSet<bookingSome>();
        //    this.bookings = new HashSet<booking>();
        //}

        //public virtual ICollection<booking> bookings { get; set; }
        //public virtual ICollection<bookingSome> bookingSomes { get; set; }

    }
}