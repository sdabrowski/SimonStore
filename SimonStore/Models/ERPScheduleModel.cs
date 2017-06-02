using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimonStore.Models
{
    public class ERPScheduleModel
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string EmployeeName { get; set; }
        //TODO: create properties in this class that are the same type of data returned by the stored procedure.
    }
}