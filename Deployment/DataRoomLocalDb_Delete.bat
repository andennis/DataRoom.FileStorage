"%ProgramFiles%\Microsoft SQL Server\130\Tools\Binn\SqlLocalDB.exe" stop "DataRoomLocalDb"
"%ProgramFiles%\Microsoft SQL Server\130\Tools\Binn\SqlLocalDB.exe" delete "DataRoomLocalDb"
RD /S /Q "%userprofile%\appdata\local\Microsoft\Microsoft SQL Server Local DB\Instances\DataRoomLocalDb"