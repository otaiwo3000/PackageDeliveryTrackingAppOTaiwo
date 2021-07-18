
============= PackageTrackingApi ==============
NOTE:
This Web Api has two .csv files used as Data Source located in the StoredData folder (found in the root folder)
i.		WarehouseRegisteredPackages.csv   : Stores Registered Packages and their Current Status
ii.		PackageMonitor.csv				  : Keeps hourly records of the individual packages 
DO not open any of the file during testing.


============= Endpoints =======================

//get all registered packages 
http://localhost:52201/api/packages/getallpackages

//get current status of a package by package name
http://localhost:52201/api/packages/getpackage/packagename

//get current status of a package by package status
http://localhost:52201/api/packages/getpackagebystatus/packagestatus

//get movement history of all packages
http://localhost:52201/api/packages/allpackagesmovementhistory

//get package history by package name
http://localhost:52201/api/packages/packagesmovementhistorybyname/packagename

//get package history by package status
http://localhost:52201/api/packages/packagesmovementhistorybystatus/packagestatus

//get package history by package status and date range
http://localhost:52201/api/packages/packagesmovementhistorybystatusanddaterange/DELIVERED/date1/date2

//register one or more packages
http://localhost:52201/api/packages/addpackages

//record package movement status
http://localhost:52201/api/packages/addpackagestatus


=============== Sample data to register packages with their respective current status =======================
http://localhost:52201/api/packages/addpackages

[
 {
	"Package_Name": "pkg100",
	"Status": "WAREHOUSE"
 },
 {
   "Package_Name": "pkg110",
	"Status": "delivered"
  },
 {
	"Package_Name": "pkg120",
	"Status": "PICKED_UP"
  }
]



=============== Hourly Record of Package Status: Sample data to record status of individual package =======================
http://localhost:52201/api/packages/addpackagestatus
{	
	"Particular_DateTime": "12/31/2020 23:30",
	"Status": "WAREHOUSE",
    "Package_Name": "pkg100"
 }
