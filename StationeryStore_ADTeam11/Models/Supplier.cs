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
        [Required, StringLength(4, ErrorMessage="Supplier Code maximum length is 4 characters")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Display(Name = "Supplier Name")]
        [Required, StringLength(100, ErrorMessage = "Supplier Name maximum length is 100 characters")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Display(Name = "GST Registration No")]
        [StringLength(12, ErrorMessage = "GST Registration No maximum length is 12 characters")]
        public string GstNumber
        {
            get { return _gstNumber; }
            set { _gstNumber = value; }
        }

        [Display(Name = "Contact Name")]
        [Required, StringLength(25, ErrorMessage = "Contact Name maximum length is 25 characters")]
        public string ContactName
        {
            get { return _contactName; }
            set { _contactName = value; }
        }

        [Display(Name = "Phone No")]
        [Required, Range(10000000, 999999999999999, ErrorMessage = "Phone no length is between 8 to 15 digits")]
        public int PhoneNo
        {
            get { return _phoneNo; }
            set { _phoneNo = value; }
        }

        [Display(Name = "Fax No")]
        [Required, Range(10000000, 999999999999999, ErrorMessage = "Fax no length is between 8 to 15 digits")]
        public int Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        [Display(Name = "Address")]
        [Required, StringLength(255, ErrorMessage = "Address maximum length is 255 characters")]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
    }
}