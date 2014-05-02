ECHO OFF

set mydate=%date:~4,2%-%date:~7,2%-%date:~10,4%

set hr=%TIME: =0%
set hr=%hr:~0,2%

set hrMin=%TIME: =0%
set hrMin=%hrMin:~0,2%

set branchDate=%mydate%_%hr%-%hrMin%

tf branch "$/PhytelCode/Phytel.Net/Client Apps/Atmosphere/Nightingale/Production/Current" "$/PhytelCode/Phytel.Net/Client Apps/Atmosphere/Nightingale/Production/%branchDate%" /checkin /noprompt /silent

tf merge "$/PhytelCode/Phytel.Net/Client Apps/Atmosphere/Nightingale/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Client Apps/Atmosphere/Nightingale/Production/Current" /recursive /nosummary /lock:checkout

tf checkout "$/PhytelCode/Phytel.Net/Client Apps/Atmosphere/Nightingale/Production/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Client Apps/Atmosphere/Nightingale/Production/Current/*.csproj" /lock:checkout /recursive

"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Client Apps\Atmosphere\Nightingale\Production\Current" /RW:"Release Candidate" /WW:"Production"

tf checkin "$/PhytelCode/Phytel.Net/Client Apps/Atmosphere/Nightingale/Production/Current" /comment:"Merge Changes for Prod Release" /recursive /force /noprompt