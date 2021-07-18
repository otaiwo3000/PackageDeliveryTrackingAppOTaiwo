
============= PackageTrackingApi ==============
NOTE:
This Web Api has two .csv files used as Data Source located in the StoredData folder (found in the root folder)
i.		WarehouseRegisteredPackages.csv   : Stores Registered Packages and their Current Status
ii.		PackageMonitor.csv				  : Keeps hourly records of the individual packages 

In this app, Status is also regards as Package Status which are:

Status		Denoted as
--------------------
WAREHOUSE 	1
PICKED_UP	2
IN_TRANSIT	3
DELIVERED	4

If the app is to be tested locally as against client-server environment, then, keep the two files closed before testing.


