ECHO OFF
SET PATH=%PATH%;C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE
set mydate=%date:~4,2%-%date:~7,2%-%date:~10,4%

set hr=%TIME: =0%
set hr=%hr:~0,2%

set hrMin=%TIME: =0%
set hrMin=%hrMin:~3,2%

set branchDate=%mydate%_%hr%-%hrMin%

tf branch "$/PhytelCode/Phytel.Net/Services/API/KeepAliveProcessor/Production/Current" "$/PhytelCode/Phytel.Net/Services/API/KeepAliveProcessor/Production/%branchDate%" /checkin /noprompt /silent

tf merge "$/PhytelCode/Phytel.Net/Services/API/KeepAliveProcessor/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Services/API/KeepAliveProcessor/Production/Current" /recursive /nosummary /lock:checkout

tf checkout "$/PhytelCode/Phytel.Net/Services/API/KeepAliveProcessor/Production/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/KeepAliveProcessor/Production/Current/*.csproj" /lock:checkout /recursive

"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Services\API\KeepAliveProcessor\Production\Current" /RW:"Release Candidate" /WW:"Production"

tf checkin "$/PhytelCode/Phytel.Net/Services/API/KeepAliveProcessor/Production/Current" /comment:"Merge Changes for Prod Release" /recursive /force /noprompt
