using StationeryStore_ADTeam11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.MobileModels
{
    public class MDisbursement
    {
        private string _deptId;
        private string _deptName;
        private string _representative;
        private string _phone;
        private List<ItemRequest> _itemList;
    }
}