using PackageDeliveryTrackingAppOTaiwo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PackageDeliveryTrackingAppOTaiwo.Models
{
    public class WarehouseRegisterPackages
    {
        public int PackageId { get; set; }
        public string Package_Name { get; set; }
        public string Package_Code { get; set; }
        //public int Status { get; set; }
        public PackageStatusEnum Status { get; set; }
        public string StringStatus { get; set; }
    }

    public class WarehouseRegisterPackagesModel
    {
        //public int PackageId { get; set; }
        public string Package_Name { get; set; }
        public string Package_Code { get; set; } 
        public int Status { get; set; }
        //public PackageStatusEnum Status { get; set; }
        //public string StringStatus { get; set; }
    }
}