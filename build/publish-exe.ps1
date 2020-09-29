# Publish the app as a windows x64 exe
# For list of runtime identifiers see: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog
# Will output the exe to: ByteDev.Nuget.PackageViewer\src\ByteDev.Nuget.PackageViewer\bin\Release\netcoreapp3.1\win-x64
cd ..
& dotnet publish -c Release -r win-x64
cd src\ByteDev.Nuget.PackageViewer\bin\Release\netcoreapp3.1\win-x64
dir *.exe