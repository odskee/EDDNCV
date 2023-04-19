# EDDNCV
A simple .NET / C# console app to show a real-time stream from the EDDN Network with some filtering options.  Primary purpose is to assist in the development and debugging of third-party tools that contribute / consume EDDN data.


## Features
Can be run without command line args to display all EDDN entries in real-time.  Optionally, the following filters can be used to restrict what you see:
* -uploader <uploader_ID> - Provide an uploader ID to see only entries corresponding to them.  Note that Cmdr names / Identities are not known or available.
* -schema <EDDN_Schema_Name> - Provide the EDDN schema to see only those responses (See https://github.com/EDCD/EDDN/tree/master/schemas).
* -provider <Provider_Name> - Provide the software name to see entries from a specific third-part tool.
* -fc <Fleet_Carrier_Name> - Show only docking entries against a specific fleet carrier landing location.


## How to Use
You can either download, build and run the full repo (using VS or dotnet run).  Alternatively, you can move this into your own environment:

```
dotnet new console -n EDDCV -o "C:\EDDNCV"
dotnet add EDDNCV package NetMQ
dotnet add EDDNCV package Ionic.Zlib
```

Now paste the contents of Program.cs into your program.cs inside the EDDNCV project directory.  To run the console app, in Powershell:
```
dotnet run --project EDDNCV
```

### Examples
> dotnet run --project EDDNCV -fc "My Fleet Carrier"

> dotnet run --project EDDNCV -schema https://eddn.edcd.io/schemas/fsssignaldiscovered/1 -provider EDDiscovery


## Future
Looking to add text file logging options.
