# EDDNCV
A simple .NET / C# console stream of data from the EDDN Network

## Features
Currently, it simply shows the stream of data from EDDN line by line in the console.  Future versions will allow for schema filtering among other possibilities.

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
