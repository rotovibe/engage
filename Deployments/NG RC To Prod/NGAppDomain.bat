ECHO OFF

set mydate=%date:~4,2%-%date:~7,2%-%date:~10,4%

set hr=%TIME: =0%
set hr=%hr:~0,2%

set hrMin=%TIME: =0%
set hrMin=%hrMin:~0,2%

set branchDate=%mydate%_%hr%-%hrMin%

tf branch "$/PhytelCode/Phytel.Net/Services/API/AppDomain/Phytel.API.AppDomain.NG/Production/Current" "$/PhytelCode/Phytel.Net/Services/API/AppDomain/Phytel.API.AppDomain.NG/Production/%branchDate%" /checkin /noprompt /silent

tf merge "$/PhytelCode/Phytel.Net/Services/API/AppDomain/Phytel.API.AppDomain.NG/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Services/API/AppDomain/Phytel.API.AppDomain.NG/Production/Current" /recursive /nosummary /lock:checkout

tf checkout "$/PhytelCode/Phytel.Net/Services/API/AppDomain/Phytel.API.AppDomain.NG/Production/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/AppDomain/Phytel.API.AppDomain.NG/Production/Current/*.csproj" /lock:checkout /recursive

"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Services\API\AppDomain\Phytel.API.AppDomain.NG\Production\Current" /RW:"Release Candidate" /WW:"Production"

tf checkin "$/PhytelCode/Phytel.Net/Services/API/AppDomain/Phytel.API.AppDomain.NG/Production/Current" /comment:"Merge Changes for Prod Release" /recursive /force /noprompt

TFSBuild start http://hillstfs2013:8080/tfs PhytelCode "API NG App Domain (Prod Svr1)" /silent
TFSBuild start http://hillstfs2013:8080/tfs PhytelCode "API NG App Domain (Prod Svr2)" /silent
