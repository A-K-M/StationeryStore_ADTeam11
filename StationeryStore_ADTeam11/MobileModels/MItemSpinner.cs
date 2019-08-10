using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MItemSpinner
    {
        private string _id;
        private int _categoryId;
        private string _description;

        public string Id { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }

    }
}