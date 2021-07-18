using PackageDeliveryTrackingAppOTaiwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Text;
using PackageDeliveryTrackingAppOTaiwo.Common;
//using CsvHelper;


namespace PackageDeliveryTrackingAppOTaiwo.Implementation
{
    public class WarehousePackagesImpl
    {
        string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
      

        public string RegisterPackages(List<WarehouseRegisterPackages> pkgList)
        {
            string res2 = "";
            try
            {
                string pkgFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/WarehouseRegisteredPackages.csv";
                if (string.IsNullOrEmpty(pkgFilePath))
                    return "The file does not exist";

                if (pkgList.Count > 0)
                {
                    
                    var sb = new StringBuilder();
                    int i = 0;
                    foreach (var data in pkgList)
                    {
                        var wppn = WarehousePackagesByPackageName(data.Package_Name);
                        if(wppn != null) //ie the package already exist
                        {
                            res2 = "One or more package(s) already registered and are not entered. ";
                        }

                        else
                        {
                            i = i + 1;
                            //get the file record count, then plus it to 1 to form the next packageId
                            data.PackageId = GetRecordCountOfaFile(pkgFilePath) + i;
                            //sb.AppendLine(data.PackageId + "," + data.Package_Name + "," + data.Package_Code + "," + data.Status);
                            sb.AppendLine(data.PackageId + "," + data.Package_Name + "," + data.Package_Code + "," + data.Status);

                        }
                    }
                    
                    //File.WriteAllText(pkgFilePath, sb.ToString());
                    File.AppendAllText(pkgFilePath, sb.ToString());
                  
                    return res2  + "Operation Complete. ";
                }
                else return "No record(s) has been entered by the user.";

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Opeeration failed", ex);
            }

        }

        public static string PackageStatusMonitor(PackageMonitor incomingpkgstatus)
        {
            try
            {
                string res = "no response";

                if(incomingpkgstatus != null)
                {      
                    //
                    var currentstatuspkg_fromTheMONITORtable = PackageMonitorByPackageName(incomingpkgstatus.Package_Name);
                    var currentstatuspkg = WarehousePackagesByPackageName(incomingpkgstatus.Package_Name);

                    if (currentstatuspkg != null)
                    {
                        //check the current status of the package
                        if (currentstatuspkg.Status == PackageStatusEnum.WAREHOUSE)
                        {
                            //check the status the user want to enter before saving it
                            if (incomingpkgstatus.Status == PackageStatusEnum.WAREHOUSE ||
                                incomingpkgstatus.Status == PackageStatusEnum.PICKED_UP)
                            {
                                res = InsertPackageCurrentState(incomingpkgstatus);
                                UpdateRegisteredPackage(incomingpkgstatus.Package_Name, incomingpkgstatus.Status);                              
                            }
                            //else res = "New Package state is not saved. Check the package status you intend to save";
                            else res = "Invalid status entry. The current status of the package " + "'" + incomingpkgstatus.Package_Name + "'" +  " is " + currentstatuspkg.Status + " but you are trying to update the status to " + incomingpkgstatus.Status;
                        }
                        else if (currentstatuspkg.Status == PackageStatusEnum.PICKED_UP)
                        {
                            if (incomingpkgstatus.Status == PackageStatusEnum.IN_TRANSIT)
                            {
                                res = InsertPackageCurrentState(incomingpkgstatus);
                                UpdateRegisteredPackage(incomingpkgstatus.Package_Name, incomingpkgstatus.Status);
                            }
                            //else res = "New Package state is not saved. Check the package status you intend to save";
                            else res = "Invalid status entry. The current status of the package " + "'" + incomingpkgstatus.Package_Name + "'" + " is " + currentstatuspkg.Status + " but you are trying to update the status to " + incomingpkgstatus.Status;
                        }
                        else if (currentstatuspkg.Status == PackageStatusEnum.IN_TRANSIT)
                        {
                            if (incomingpkgstatus.Status == PackageStatusEnum.IN_TRANSIT ||
                                incomingpkgstatus.Status == PackageStatusEnum.DELIVERED)
                            {
                                res = InsertPackageCurrentState(incomingpkgstatus);
                                UpdateRegisteredPackage(incomingpkgstatus.Package_Name, incomingpkgstatus.Status);
                            }
                            //else res = "New Package state is not saved. Check the package status you intend to save";
                            else res = "Invalid status entry. The current status of the package " + "'" + incomingpkgstatus.Package_Name + "'" + " is " + currentstatuspkg.Status + " but you are trying to update the status to " + incomingpkgstatus.Status;
                        }
                        else if (currentstatuspkg.Status == PackageStatusEnum.DELIVERED)
                        {
                            res = "Invalid status entry. The current status of the package " + "'" + incomingpkgstatus.Package_Name + "'" + " is " + currentstatuspkg.Status + " but you are trying to update the status to " + incomingpkgstatus.Status;
                            //UpdateRegisteredPackage(incomingpkgstatus.Package_Name, incomingpkgstatus.Status);
                        }
                        else
                        {
                            //res = InsertPackageCurrentState(incomingpkgstatus);
                            //UpdateRegisteredPackage(incomingpkgstatus.Package_Name, incomingpkgstatus.Status);
                        }
                    }
                    else
                    {
                        //res = InsertPackageCurrentState(incomingpkgstatus);
                        //UpdateRegisteredPackage(incomingpkgstatus.Package_Name, incomingpkgstatus.Status);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Operation failed with: ", ex);
            }
        }

        public static WarehouseRegisterPackages ParsePackageFromLine(string line)
        {
            string[] parts = line.Split(',');

            return new WarehouseRegisterPackages
            {
                PackageId = int.Parse(parts[0]),
                Package_Name = parts[1],
                Package_Code = parts[2],
                //Status = int.Parse(parts[3])
                Status = ConvertStringToEnum(parts[3])
            };
        }

        public static PackageStatusEnum ConvertStringToEnum(string enumname)
        {
            //PackageStatusEnum convertedenum = (PackageStatusEnum)Enum.Parse(typeof(PackageStatusEnum), "WAREHOUSE", true);
            PackageStatusEnum convertstringTOenum = (PackageStatusEnum)Enum.Parse(typeof(PackageStatusEnum), enumname, true);

            return convertstringTOenum;
        }

        public static string ConvertEnumToString(PackageStatusEnum pkgstatusenum)
        {
            string convertenumTOstring = Convert.ToString(pkgstatusenum);

            return convertenumTOstring;
        }

        public static string InsertPackageCurrentState(PackageMonitor incomingpkgcurrentstatus)
        {
            string pkgMonitorFilePath = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/PackageMonitor.csv";
            if (string.IsNullOrEmpty(pkgMonitorFilePath))
                return "The file does not exist";

            var sb = new StringBuilder();

            int pkgID = WarehousePackagesByPackageName(incomingpkgcurrentstatus.Package_Name).PackageId;
            //sb.AppendLine(incomingpkgcurrentstatus.PackageId + "," + incomingpkgcurrentstatus.Particular_DateTime + "," + incomingpkgcurrentstatus.Status + "," + incomingpkgcurrentstatus.Package_Name);
            sb.AppendLine(pkgID + "," + incomingpkgcurrentstatus.Particular_DateTime + "," + incomingpkgcurrentstatus.Status + "," + incomingpkgcurrentstatus.Package_Name);
            
            //File.WriteAllText(pkgFilePath, sb.ToString());
            File.AppendAllText(pkgMonitorFilePath, sb.ToString());

            return "Package currrent status saved.";
        }

        public static string UpdateRegisteredPackage(string incomingpkgName, PackageStatusEnum pkgstatus)
        {
            string pkgFilePath_2 = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/WarehouseRegisteredPackages.csv";
            if (string.IsNullOrEmpty(pkgFilePath_2))
                return "The file does not exist";

            var lines = System.IO.File.ReadAllLines(pkgFilePath_2).Skip(1);
            List<WarehouseRegisterPackages> whpkgList = new List<WarehouseRegisterPackages>();
            List<WarehouseRegisterPackages> whpkgList2 = new List<WarehouseRegisterPackages>();
            
            foreach (string item in lines)
            {
                var values = item.Split(',');

                whpkgList.Add(new WarehouseRegisterPackages
                {
                    PackageId = int.Parse(values[0]),
                    Package_Name = values[1],
                    Package_Code = values[2],
                    Status = ConvertStringToEnum(values[3])
                });
            }
            whpkgList2 = whpkgList.Where(x => x.Package_Name == incomingpkgName).Select(S => { S.Status = pkgstatus; return S; }).ToList();

            var sb = new StringBuilder();

            //append column header first
            sb.AppendLine("PackageId" + "," + "Package_Name" + "," + "Package_Code" + "," + "Status");

            foreach (var data in whpkgList)
            {
                sb.AppendLine(data.PackageId + "," + data.Package_Name + "," + data.Package_Code + "," + data.Status);
            }
            File.WriteAllText(pkgFilePath_2, sb.ToString());

            return "Package currrent status saved.";
        }

        public static int GetRecordCountOfaFile(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var filerecordcount = lines.Length - 1; // minus 1 bcos of the column header record

            return filerecordcount;
        }

      

        //=============== retrieval ============================================
        public static List<WarehouseRegisterPackages> AllWarehousePackages()
        {
            string pkgFilePath_2 = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/WarehouseRegisteredPackages.csv";

            var lines = System.IO.File.ReadAllLines(pkgFilePath_2).Skip(1);
            List<WarehouseRegisterPackages> whpkgList = new List<WarehouseRegisterPackages>();

            foreach (string item in lines)
            {
                var values = item.Split(',');

                whpkgList.Add(new WarehouseRegisterPackages
                {
                    PackageId = int.Parse(values[0]),
                    Package_Name = values[1],
                    Package_Code = values[2],
                    Status = ConvertStringToEnum(values[3]),
                    StringStatus = values[3]
                });                        
            }

            return whpkgList;
        }

        public static WarehouseRegisterPackages WarehousePackagesByPackageName(string packagename)
        {
            string pkgFilePath_2 = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/WarehouseRegisteredPackages.csv";

            var lines = System.IO.File.ReadAllLines(pkgFilePath_2).Skip(1);
            List<WarehouseRegisterPackages> whpkgList = new List<WarehouseRegisterPackages>();
            WarehouseRegisterPackages whpkgByName = new WarehouseRegisterPackages();

            foreach (string item in lines)
            {
                var values = item.Split(',');

                WarehouseRegisterPackages wrp = new WarehouseRegisterPackages();
                wrp.PackageId = int.Parse(values[0]);
                wrp.Package_Name = values[1];
                wrp.Package_Code = values[2];
                wrp.Status = ConvertStringToEnum(values[3]);
                wrp.StringStatus = values[3];

                whpkgList.Add(wrp);
                
            }

            whpkgByName = whpkgList.Where(x=>x.Package_Name.Trim().ToUpper()==packagename.Trim().ToUpper()).FirstOrDefault();

            return whpkgByName;
        }

        public static WarehouseRegisterPackages WarehousePackagesByPackageId(int packageID)
        {
            string pkgFilePath_2 = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/WarehouseRegisteredPackages.csv";

            var lines = System.IO.File.ReadAllLines(pkgFilePath_2).Skip(1);
            List<WarehouseRegisterPackages> whpkgList = new List<WarehouseRegisterPackages>();
            WarehouseRegisterPackages whpkgByID = new WarehouseRegisterPackages();

            foreach (string item in lines)
            {
                var values = item.Split(',');

                WarehouseRegisterPackages wrp = new WarehouseRegisterPackages();
                wrp.PackageId = int.Parse(values[0]);
                wrp.Package_Name = values[1];
                wrp.Package_Code = values[2];
                wrp.Status = ConvertStringToEnum(values[3]);
                wrp.StringStatus = values[3];

                whpkgList.Add(wrp);

            }

            whpkgByID = whpkgList.Where(x => x.PackageId == packageID).FirstOrDefault();

            return whpkgByID;
        }

        public static List<WarehouseRegisterPackages> WarehousePackagesByStatus(string pkgstatus)
        {
            string pkgFilePath_2 = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/WarehouseRegisteredPackages.csv";

            var lines = System.IO.File.ReadAllLines(pkgFilePath_2).Skip(1);
            List<WarehouseRegisterPackages> whpkgList = new List<WarehouseRegisterPackages>();
            List<WarehouseRegisterPackages> whpkgByStatusList = new List<WarehouseRegisterPackages>();

            foreach (string item in lines)
            {
                var values = item.Split(',');

                WarehouseRegisterPackages wrp = new WarehouseRegisterPackages();
                wrp.PackageId = int.Parse(values[0]);
                wrp.Package_Name = values[1];
                wrp.Package_Code = values[2];
                wrp.Status = ConvertStringToEnum(values[3]);
                wrp.StringStatus = values[3];

                whpkgList.Add(wrp);

            }

            PackageStatusEnum pstatus = ConvertStringToEnum(pkgstatus.Trim().ToUpper());
            whpkgByStatusList = whpkgList.Where(x => x.Status == pstatus).ToList();

            return whpkgByStatusList;
        }

        public static PackageMonitor PackageMonitorByPackageName(string packagename)
        {
            string pkgFilePath_2 = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/PackageMonitor.csv";

            var lines = System.IO.File.ReadAllLines(pkgFilePath_2).Skip(1);
            List<PackageMonitor> monitorwhpkgList = new List<PackageMonitor>();
            PackageMonitor monitorwhpkgByName = new PackageMonitor();

            if(lines.Count() > 0)
            {
                foreach (string item in lines)
                {
                    var values = item.Split(',');

                    PackageMonitor wrp = new PackageMonitor();
                    //wrp.PackageId = int.Parse(values[0]);
                    wrp.Particular_DateTime = DateTime.Now;
                    wrp.Status = ConvertStringToEnum(values[2]);
                    wrp.StringStatus = values[2];
                    wrp.Package_Name = values[3];

                    monitorwhpkgList.Add(wrp);
                    
                }

                var mp = monitorwhpkgList.Where(x => x.Package_Name.Trim().ToUpper() == packagename.Trim().ToUpper()).ToList();
                int max_filteredpkgs = 0;
                if (mp.Count() != 0)
                {
                    max_filteredpkgs = mp.Max(x => x.PackageId);
                    monitorwhpkgByName = mp.Where(x => x.PackageId == max_filteredpkgs).FirstOrDefault();
                }                
                else
                {
                    WarehouseRegisterPackages pkg = WarehousePackagesByPackageName(packagename);

                    monitorwhpkgByName.Status = pkg.Status;
                    monitorwhpkgByName.Package_Name = packagename;
                    monitorwhpkgByName.StringStatus = ConvertEnumToString(pkg.Status);
                }
            }
            else
            {
                WarehouseRegisterPackages pkg = WarehousePackagesByPackageName(packagename);

                monitorwhpkgByName.Status = pkg.Status;
                monitorwhpkgByName.Package_Name = packagename;
                monitorwhpkgByName.StringStatus = ConvertEnumToString(pkg.Status);
            }

            return monitorwhpkgByName;
        }

        public static List<PackageMonitor> AllPackagesMovtHistory()
        {
            string pkgFilePath_2 = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/PackageMonitor.csv";

            var lines = System.IO.File.ReadAllLines(pkgFilePath_2).Skip(1);
            List<PackageMonitor> whpkgList = new List<PackageMonitor>();

            foreach (string item in lines)
            {
                var values = item.Split(',');

                whpkgList.Add(new PackageMonitor
                {
                    PackageId = int.Parse(values[0]),
                    Particular_DateTime = DateTime.Parse(values[1]),
                    Status = ConvertStringToEnum(values[2]),
                    StringStatus = values[2],
                    Package_Name = values[3]
                });
            }

            return whpkgList;
        }

        public static List<PackageMonitor> PackagesMovtHistoryByName(string pkgName)
        {
            string pkgFilePath_2 = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/PackageMonitor.csv";

            var lines = System.IO.File.ReadAllLines(pkgFilePath_2).Skip(1);
            List<PackageMonitor> whpkgList = new List<PackageMonitor>();
            List<PackageMonitor> monitorwhpkgList = new List<PackageMonitor>();
            PackageMonitor monitorwhpkgByName = new PackageMonitor();

            foreach (string item in lines)
            {
                var values = item.Split(',');

                whpkgList.Add(new PackageMonitor
                {
                    PackageId = int.Parse(values[0]),
                    Particular_DateTime = DateTime.Parse(values[1]),
                    Status = ConvertStringToEnum(values[2]),
                    StringStatus = values[2],
                    Package_Name = values[3]
                });
            }
            monitorwhpkgList = whpkgList.Where(x => x.Package_Name == pkgName).ToList();

            return monitorwhpkgList;
        }

        public static List<PackageMonitor> PackagesMovtHistoryByStatus(string pkgStatus)
        {
            string pkgFilePath_2 = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/PackageMonitor.csv";

            var lines = System.IO.File.ReadAllLines(pkgFilePath_2).Skip(1);
            List<PackageMonitor> whpkgList = new List<PackageMonitor>();
            List<PackageMonitor> monitorwhpkgList = new List<PackageMonitor>();
            PackageMonitor monitorwhpkgByName = new PackageMonitor();

            foreach (string item in lines)
            {
                var values = item.Split(',');

                whpkgList.Add(new PackageMonitor
                {
                    PackageId = int.Parse(values[0]),
                    Particular_DateTime = DateTime.Parse(values[1]),
                    Status = ConvertStringToEnum(values[2]),
                    StringStatus = values[2],
                    Package_Name = values[3]
                });
            }
            monitorwhpkgList = whpkgList.Where(x => x.Status == ConvertStringToEnum(pkgStatus.Trim().ToUpper())).ToList();

            return monitorwhpkgList;
        }

        public static List<PackageMonitor> PackagesMovtHistoryByStatusandDaterange(string pkgStatus, DateTime fromdate, DateTime todate)
        {
            string pkgFilePath_2 = System.AppDomain.CurrentDomain.BaseDirectory + "StoredData/PackageMonitor.csv";

            var lines = System.IO.File.ReadAllLines(pkgFilePath_2).Skip(1);
            List<PackageMonitor> whpkgList = new List<PackageMonitor>();
            List<PackageMonitor> monitorwhpkgList = new List<PackageMonitor>();
            PackageMonitor monitorwhpkgByName = new PackageMonitor();

            foreach (string item in lines)
            {
                var values = item.Split(',');

                whpkgList.Add(new PackageMonitor
                {
                    PackageId = int.Parse(values[0]),
                    Particular_DateTime = DateTime.Parse(values[1]),
                    Status = ConvertStringToEnum(values[2]),
                    StringStatus = values[2],
                    Package_Name = values[3]
                });
            }
            monitorwhpkgList = whpkgList.Where(x => x.Status == ConvertStringToEnum(pkgStatus.Trim().ToUpper()) && (x.Particular_DateTime >= fromdate && x.Particular_DateTime <= todate)).ToList();

            return monitorwhpkgList;
        }


    }
}