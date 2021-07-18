using PackageDeliveryTrackingAppOTaiwo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PackageDeliveryTrackingAppOTaiwo.Models
{
    public class PackageMonitor
    {
        public int PackageId { get; set; }
        public DateTime Particular_DateTime { get; set; }
        //public int Status { get; set; }
        public PackageStatusEnum Status { get; set; }

        public string StringStatus { get; set; }
        public string Package_Name { get; set; }
    }
}