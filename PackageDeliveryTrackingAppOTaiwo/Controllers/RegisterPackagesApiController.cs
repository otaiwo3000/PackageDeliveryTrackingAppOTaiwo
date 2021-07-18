using PackageDeliveryTrackingAppOTaiwo.Common;
using PackageDeliveryTrackingAppOTaiwo.Implementation;
using PackageDeliveryTrackingAppOTaiwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PackageDeliveryTrackingAppOTaiwo.Controllers
{
    [RoutePrefix("api/packages")]
    public class RegisterPackagesApiController : ApiController
    {
        [HttpPost]
        [Route("addpackages")]
        public HttpResponseMessage AddPackages(HttpRequestMessage request, [FromBody]List<WarehouseRegisterPackages> addpackagesModel)
        {
            HttpResponseMessage res = null;
            
            WarehousePackagesImpl whp = new WarehousePackagesImpl();
            string returnstring = whp.RegisterPackages(addpackagesModel);

            //res = request.CreateResponse<IEnumerable>(HttpStatusCode.OK, newList3);
            res = request.CreateResponse(HttpStatusCode.OK, returnstring);

            return res;

        }

        [HttpGet]
        [Route("getpackage/{packagename}")]
        public HttpResponseMessage GetPackage(HttpRequestMessage request, string packagename)
        {
            HttpResponseMessage res = null;

            try
            {
                //WarehousePackagesImpl whp = new WarehousePackagesImpl();
                WarehouseRegisterPackages pkg = WarehousePackagesImpl.WarehousePackagesByPackageName(packagename.Trim().ToUpper());

                res = request.CreateResponse(HttpStatusCode.OK, pkg);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Occurs. ", ex);
            }
           
            return res;
        }

        [HttpGet]
        [Route("getpackagebystatus/{packagestatus}")]
        public HttpResponseMessage GetPackageByStatus(HttpRequestMessage request, string packagestatus)
        {
            HttpResponseMessage res = null;

            try
            {
                //WarehousePackagesImpl whp = new WarehousePackagesImpl();
                List<WarehouseRegisterPackages> pkgs = WarehousePackagesImpl.WarehousePackagesByStatus(packagestatus.Trim().ToUpper());

                res = request.CreateResponse(HttpStatusCode.OK, pkgs);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Occurs. ", ex);
            }

            return res;
        }


        [HttpGet]
        [Route("getallpackages")]
        public HttpResponseMessage GetAllPackages(HttpRequestMessage request)
        {
            HttpResponseMessage res = null;

            try
            {
                //WarehousePackagesImpl whp = new WarehousePackagesImpl();
                List<WarehouseRegisterPackages> pkgs = WarehousePackagesImpl.AllWarehousePackages();

                res = request.CreateResponse(HttpStatusCode.OK, pkgs);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Occurs. ", ex);
            }

            return res;
        }

        [HttpGet]
        [Route("allpackagesmovementhistory")]
        public HttpResponseMessage GetAllPackagesMovementHisory(HttpRequestMessage request)
        {
            HttpResponseMessage res = null;

            try
            {
                List<PackageMonitor> pkgs = WarehousePackagesImpl.AllPackagesMovtHistory();

                res = request.CreateResponse(HttpStatusCode.OK, pkgs);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Occurs. ", ex);
            }

            return res;
        }

        [HttpGet]
        [Route("packagesmovementhistorybyname/{packagename}")]
        public HttpResponseMessage GetPackagesMovementHisoryByName(HttpRequestMessage request, string packagename)
        {
            HttpResponseMessage res = null;

            try
            {
                List<PackageMonitor> pkgs = WarehousePackagesImpl.PackagesMovtHistoryByName(packagename);

                res = request.CreateResponse(HttpStatusCode.OK, pkgs);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Occurs. ", ex);
            }

            return res;
        }

        [HttpGet]
        [Route("packagesmovementhistorybystatus/{packagestatus}")]
        public HttpResponseMessage GetPackagesMovementHisoryByStatus(HttpRequestMessage request, string packagestatus)
        {
            HttpResponseMessage res = null;

            try
            {
                List<PackageMonitor> pkgs = WarehousePackagesImpl.PackagesMovtHistoryByStatus(packagestatus);

                res = request.CreateResponse(HttpStatusCode.OK, pkgs);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Occurs. ", ex);
            }

            return res;
        }

        [HttpGet]
        [Route("packagesmovementhistorybystatusanddaterange/{packagestatus}/{startdate}/{enddate}")]
        public HttpResponseMessage GetPackagesMovementHisoryByStatusAndDateRange(HttpRequestMessage request, string packagestatus, DateTime fromdate, DateTime todate)
        {
            HttpResponseMessage res = null;

            try
            {
                List<PackageMonitor> pkgs = WarehousePackagesImpl.PackagesMovtHistoryByStatus(packagestatus);

                res = request.CreateResponse(HttpStatusCode.OK, pkgs);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Occurs. ", ex);
            }

            return res;
        }



        [HttpPost]
        [Route("addpackagestatus")]
        //public HttpResponseMessage AddPackageStatus(HttpRequestMessage request)
        public HttpResponseMessage AddPackageStatus(HttpRequestMessage request, [FromBody]PackageMonitor incomingpkg)

        {           
            HttpResponseMessage res = null;

            PackageMonitor pkg = new PackageMonitor
            {
               //PackageId = incomingpkg.PackageId,
               Package_Name = incomingpkg.Package_Name,
               Particular_DateTime = incomingpkg.Particular_DateTime,
               Status = incomingpkg.Status
            };

            //WarehousePackagesImpl whp = new WarehousePackagesImpl();
            string returnstring = WarehousePackagesImpl.PackageStatusMonitor(pkg);

            res = request.CreateResponse(HttpStatusCode.OK, returnstring);

            return res;

        }

        //[HttpGet]
        //[Route("addmonitorpackage")]
        ////public HttpResponseMessage AddMonitorPackage(HttpRequestMessage request)
        //public HttpResponseMessage AddMonitorPackage(HttpRequestMessage request, PackageMonitor pm)
        //{
        //    HttpResponseMessage res = null;
            
        //    try
        //    {
        //        //WarehousePackagesImpl whp = new WarehousePackagesImpl();
        //        string pmres = WarehousePackagesImpl.PackageStatusMonitor(pm);

        //        res = request.CreateResponse(HttpStatusCode.OK, pmres);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException("Error Occurs. ", ex);
        //    }

        //    return res;
        //}

    }
}
