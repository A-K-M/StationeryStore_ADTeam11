using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class Supplier
    {
        private string _id;
        private string _name;
        private string _gstNumber;
        private string _contactName;
        private int _phoneNo;
        private int _fax;
        private string _address;

        [Display(Name = "Supplier Code")]
        [Required(ErrorMessage = "Supplier Code is required")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Display(Name = "Supplier Name")]
        [Required(ErrorMessage = "Supplier Name is required")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Display(Name = "GST Registration No")]
        [Required(ErrorMessage = "GST Registration No is required")]
        public string GstNumber
        {
            get { return _gstNumber; }
            set { _gstNumber = value; }
        }

        [Display(Name = "Contact Name")]
        [Required(ErrorMessage = "Contact name is required")]
        public string ContactName
        {
            get { return _contactName; }
            set { _contactName = value; }
        }

        [Display(Name = "Phone No")]
        [Required(ErrorMessage = "Phone No is required")]
        public int PhoneNo
        {
            get { return _phoneNo; }
            set { _phoneNo = value; }
        }

        [Display(Name = "Fax No")]
        [Required(ErrorMessage = "Fax No is required")]
        public int Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required")]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
    }
}