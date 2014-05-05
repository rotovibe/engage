ECHO OFF

set mydate=%date:~4,2%-%date:~7,2%-%date:~10,4%

set hr=%TIME: =0%
set hr=%hr:~0,2%

set hrMin=%TIME: =0%
set hrMin=%hrMin:~0,2%

set branchDate=%mydate%_%hr%-%hrMin%

tf branch "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Core/Production/Current" "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Core/Production/%branchDate%" /checkin /noprompt /silent
tf branch "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Site/Production/Current" "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Site/Production/%branchDate%" /checkin /noprompt /silent

tf merge "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Core/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Core/Production/Current" /recursive /nosummary /lock:checkout
tf merge "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Site/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Site/Production/Current" /recursive /nosummary /lock:checkout

tf checkout "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Core/Production/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Site/Production/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Core/Production/Current/*.csproj" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Site/Production/Current/*.csproj" /lock:checkout /recursive

"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Client Apps\Atmosphere\Atmosphere.Core\Production\Current" /RW:"Release Candidate" /WW:"Production"
"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Client Apps\Atmosphere\Atmosphere.Site\Production\Current" /RW:"Release Candidate" /WW:"Production"

tf checkin "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Core/Production/Current" /comment:"Merge Changes for Prod Release" /recursive /force /noprompt
tf checkin "$/PhytelCode/Phytel.Net/Client Apps/NG/Atmosphere.Site/Production/Current" /comment:"Merge Changes for Prod Release" /recursive /force /noprompt