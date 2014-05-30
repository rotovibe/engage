ECHO OFF
SET PATH=%PATH%;C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE
set mydate=%date:~4,2%-%date:~7,2%-%date:~10,4%

set hr=%TIME: =0%
set hr=%hr:~0,2%

set hrMin=%TIME: =0%
set hrMin=%hrMin:~3,2%

set branchDate=%mydate%_%hr%-%hrMin%

tf branch "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Release Candidate/%branchDate%" /checkin /noprompt /silent
tf branch "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Release Candidate/%branchDate%" /checkin /noprompt /silent
tf branch "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Release Candidate/%branchDate%" /checkin /noprompt /silent

tf merge "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Development/Current" "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Release Candidate/Current" /recursive /nosummary /lock:checkout
tf merge "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Development/Current" "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Release Candidate/Current" /recursive /nosummary /lock:checkout
tf merge "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Development/Current" "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Release Candidate/Current" /recursive /nosummary /lock:checkout

tf checkout "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Release Candidate/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Release Candidate/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Release Candidate/Current/*.sln" /lock:checkout /recursive

tf checkout "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Release Candidate/Current/*.csproj" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Release Candidate/Current/*.csproj" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Release Candidate/Current/*.csproj" /lock:checkout /recursive

"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Services\API\Common\Phytel.API.Common\Release Candidate\Current" /RW:"Development" /WW:"Release Candidate"
"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Services\API\Common\Phytel.API.DataAudit\Release Candidate\Current" /RW:"Development" /WW:"Release Candidate"
"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Services\API\Interfaces\Release Candidate\Current" /RW:"Development" /WW:"Release Candidate"

tf checkin "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.Common/Release Candidate/Current" /comment:"Merge Changes for RC Release" /recursive /force /noprompt
tf checkin "$/PhytelCode/Phytel.Net/Services/API/Common/Phytel.API.DataAudit/Release Candidate/Current" /comment:"Merge Changes for RC Release" /recursive /force /noprompt
tf checkin "$/PhytelCode/Phytel.Net/Services/API/Interfaces/Release Candidate/Current" /comment:"Merge Changes for RC Release" /recursive /force /noprompt
