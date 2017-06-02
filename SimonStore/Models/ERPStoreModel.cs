using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimonStore.Models
{
    public class ERPStoreModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Line1 { get; set; }
        public string Line2 { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class ErpScheduleModel
    {
        //TODO: create properties in this class that are the same type of data returned by the stored procedure.
    }

    public class ErpRevenueModel
    {
        public DateTime Date { get; set; }
        public Decimal Revenue { get; set; }
    }
}