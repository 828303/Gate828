using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GateBoys.Models
{
    public abstract class employeeDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string employeeName { get; set; }

        [Display(Name = "Middle name")]
        public string employeeMidName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string employeeSurname { get; set; }

        [Required]
        [Display(Name = "ID Number")]
        [MaxLength(13)]
        [MinLength(13)]
        [Range(13, Int64.MaxValue, ErrorMessage = "Id number must be 13 digits only")]
        public string employeeIdNumber { get; set; }
        
        [Display(Name = "Email")]
        public string employeeEmail { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        [Range(10, Int64.MaxValue, ErrorMessage = "Phone number must be 10 digits")]
        [Display(Name = "Phone Number")]
        public string employeePhonenumber { get; set; }

        /// <summary>
        /// Uploads
        /// </summary>
        [Display(Name = "Profile Picture")]
        public byte[] employeeProfilePic { get; set; }

        [Display(Name = "Upload CV")]
        public byte[] employeeCv { get; set; }

        [Display(Name = "Upload ID")]
        public byte[] employeeIdDocument { get; set; }

        [Display(Name = "Upload Qualification")]
        public byte[] employeeQualification { get; set; }

        /// <summary>
        /// Address
        /// </summary>
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
        [Range(4,Int64.MaxValue)]
        public string postal_code { get; set; }
        public string fullAdress { get; set; }


        /// <summary>
        /// Security check
        /// </summary>
        [Display(Name ="Still Works Here")]
        public bool isStillEmployeed { get; set; }

        [Display(Name = "Reason to leave")]
        public string reasonLeft { get; set; }

        [Display(Name = "Added by")]
        public string addedByEmail { get; set; }

        [Display(Name = "Date Registered")]
        public string dateRegistered { get; set; }
        

        public string getFullAddress()
        {
            return fullAdress = $"{street_number} {route}, {locality}, {administrative_area_level_1} ,{country} , {postal_code}";
        }


    }
}