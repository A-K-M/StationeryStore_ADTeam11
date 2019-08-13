using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationeryStore_ADTeam11.Models
{
    public class Department
    {
        private string _id;
        private int _contactId;
        private int _headId;
        private int _repId;
        private int _delegationId;
        private int _collectionPointId;
        private string _name;
        private string _delegateStatus;

        public string Id { get; set; }
        public int ContactId { get; set; }
        public int HeadId { get; set; }
        public int RepId { get; set; }
        public string RepName { get; set; }
        public int DelegationId { get; set; }
        public int CollectionPoinId { get; set; }
        public string Name { get; set; }
        public string DelegateStatus { get; set; }

    }
}