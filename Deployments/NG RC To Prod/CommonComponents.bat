ECHO OFF
SET PATH=%PATH%;C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE
set mydate=%date:~4,2%-%date:~7,2%-%date:~10,4%

set hr=%TIME: =0%
set hr=%hr:~0,2%

set hrMin=%TIME: =0%
set hrMin=%hrMin:~3,2%

set branchDate=%mydate%_%hr%-%hrMin%

REM tf branch "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Production/Current" "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Production/%branchDate%" /checkin /noprompt /silent
REM tf branch "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Production/Current" "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Production/%branchDate%" /checkin /noprompt /silent
REM tf branch "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Production/Current" "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Production/%branchDate%" /checkin /noprompt /silent

tf merge "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Production/Current" /recursive /nosummary /lock:checkout
tf merge "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Production/Current" /recursive /nosummary /lock:checkout
tf merge "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Production/Current" /recursive /nosummary /lock:checkout

tf checkout "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Production/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Production/Current/*.csproj" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Production/Current/*.config" /lock:checkout /recursive

tf checkout "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Production/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Production/Current/*.csproj" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Production/Current/*.config" /lock:checkout /recursive

tf checkout "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Production/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Production/Current/*.csproj" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Production/Current/*.config" /lock:checkout /recursive

"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Services\API\Common\Phytel.API.Common\Production\Current" /RW:"Release Candidate" /WW:"Production"
"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Services\API\Common\Phytel.API.DataAudit\Production\Current" /RW:"Release Candidate" /WW:"Production"
"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Services\API\Interfaces\Production\Current" /RW:"Release Candidate" /WW:"Production"

tf checkin "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Production/Current" /comment:"Merge Changes for Prod Release" /recursive /force /noprompt
tf checkin "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Production/Current" /comment:"Merge Changes for Prod Release" /recursive /force /noprompt
tf checkin "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Production/Current" /comment:"Merge Changes for Prod Release" /recursive /force /noprompt
