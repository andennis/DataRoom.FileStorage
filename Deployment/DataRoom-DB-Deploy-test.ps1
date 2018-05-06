param ($config = "Debug", $SqlPackageVer = 140)

$SqlPackageExe = "C:\Program Files (x86)\Microsoft SQL Server\$SqlPackageVer\DAC\bin\SqlPackage.exe"

& $SqlPackageExe /a:Publish /sf:"..\Src\FileStorage.DB.SqlServer\bin\$config\FileStorage.DB.SqlServer.dacpac" /pr:"..\Src\FileStorage.DB.SqlServer\FileStorage.DB.SqlServer.publish.xml"