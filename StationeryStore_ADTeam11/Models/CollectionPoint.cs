using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class CollectionPoint
    {
        private int _id;
        private string _name;
        private string _clerkId;
        private string _address;
        private string _collectionTime;
        public int Id { get; set; }

        public string Name { get; set; }

        public string ClerkId { get; set;  }

        public string Address { get; set; }

        public string CollectionTime { get; set; }
       
    }

}
