using System;
using System.Collections.Generic;
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

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string GstNumber
        {
            get { return _gstNumber; }
            set { _gstNumber = value; }
        }

        public string ContactName
        {
            get { return _contactName; }
            set { _contactName = value; }
        }

        public int PhoneNo
        {
            get { return _phoneNo; }
            set { _phoneNo = value; }
        }

        public int Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
    }
}