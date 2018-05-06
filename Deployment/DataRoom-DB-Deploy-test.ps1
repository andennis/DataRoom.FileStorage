param ($config = "Debug", $SqlPackageVer = 140)

$SqlPackageExe = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\$SqlPackageVer\SqlPackage.exe"
if (-not (Test-Path $SqlPackageExe)) 
{
	$SqlPackageExe = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\$SqlPackageVer\SqlPackage.exe"
}

& $SqlPackageExe /a:Publish /sf:"..\Src\FileStorage.DB.SqlServer\bin\$config\FileStorage.DB.SqlServer.dacpac" /pr:"..\Src\FileStorage.DB.SqlServer\FileStorage.DB.SqlServer.publish.xml"