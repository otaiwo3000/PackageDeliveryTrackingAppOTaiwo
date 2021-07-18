using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PackageDeliveryTrackingAppOTaiwo.Common
{
    //public class PackageStatusEnum
    //{
    //}

    //public enum PackageStatusEnum : int
    public enum PackageStatusEnum
    {
        WAREHOUSE = 1,
        PICKED_UP = 2,
        IN_TRANSIT = 3,
        DELIVERED = 4
    }

}