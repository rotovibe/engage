ECHO OFF
SET PATH=%PATH%;C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE
set mydate=%date:~4,2%-%date:~7,2%-%date:~10,4%

set hr=%TIME: =0%
set hr=%hr:~0,2%

set hrMin=%TIME: =0%
set hrMin=%hrMin:~3,2%

set branchDate=%mydate%_%hr%-%hrMin%

tf branch "$/PhytelCode/Phytel.Net/Services/API/DataDomain/Phytel.API.DataDomain.Module/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Services/API/DataDomain/Phytel.API.DataDomain.Module/Release Candidate/%branchDate%" /checkin /noprompt /silent

tf merge "$/PhytelCode/Phytel.Net/Services/API/DataDomain/Phytel.API.DataDomain.Module/Development/Current" "$/PhytelCode/Phytel.Net/Services/API/DataDomain/Phytel.API.DataDomain.Module/Release Candidate/Current" /recursive /nosummary /lock:checkout

tf checkout "$/PhytelCode/Phytel.Net/Services/API/DataDomain/Phytel.API.DataDomain.Module/Release Candidate/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/DataDomain/Phytel.API.DataDomain.Module/Release Candidate/Current/*.csproj" /lock:checkout /recursive

"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Services\API\DataDomain\Phytel.API.DataDomain.Module\Release Candidate\Current" /RW:"Development" /WW:"Release Candidate"

tf checkin "$/PhytelCode/Phytel.Net/Services/API/DataDomain/Phytel.API.DataDomain.Module/Release Candidate/Current" /comment:"Merge Changes for RC Release" /recursive /force /noprompt

TFSBuild start http://hillstfs2013:8080/tfs PhytelCode "API Module Data Domain (RC)" /silent
