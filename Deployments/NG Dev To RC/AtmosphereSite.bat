ECHO OFF
SET PATH=%PATH%;C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE
set mydate=%date:~4,2%-%date:~7,2%-%date:~10,4%

set hr=%TIME: =0%
set hr=%hr:~0,2%

set hrMin=%TIME: =0%
set hrMin=%hrMin:~3,2%

set branchDate=%mydate%_%hr%-%hrMin%

REM tf branch "$/Nightingale/Client Apps/NG/Atmosphere.Core/Release Candidate/Current" "$/Nightingale/Client Apps/NG/Atmosphere.Core/Release Candidate/%branchDate%" /checkin /noprompt /silent
REM tf branch "$/Nightingale/Client Apps/NG/Atmosphere.Site/Release Candidate/Current" "$/Nightingale/Client Apps/NG/Atmosphere.Site/Release Candidate/%branchDate%" /checkin /noprompt /silent

tf merge "$/Nightingale/Client Apps/NG/Atmosphere.Core/Development/Current" "$/Nightingale/Client Apps/NG/Atmosphere.Core/Release Candidate/Current" /recursive /nosummary /lock:checkout
tf merge "$/Nightingale/Client Apps/NG/Atmosphere.Site/Development/Current" "$/Nightingale/Client Apps/NG/Atmosphere.Site/Release Candidate/Current" /recursive /nosummary /lock:checkout

tf checkout "$/Nightingale/Client Apps/NG/Atmosphere.Core/Release Candidate/Current/*.csproj" /lock:checkout /recursive
tf checkout "$/Nightingale/Client Apps/NG/Atmosphere.Core/Release Candidate/Current/*.sln" /lock:checkout /recursive
tf checkout "$/Nightingale/Client Apps/NG/Atmosphere.Core/Release Candidate/Current/*.config" /lock:checkout /recursive

tf checkout "$/Nightingale/Client Apps/NG/Atmosphere.Site/Release Candidate/Current/*.sln" /lock:checkout /recursive
tf checkout "$/Nightingale/Client Apps/NG/Atmosphere.Site/Release Candidate/Current/*.csproj" /lock:checkout /recursive
tf checkout "$/Nightingale/Client Apps/NG/Atmosphere.Site/Release Candidate/Current/*.config" /lock:checkout /recursive

"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\Nightingale\Client Apps\NG\Atmosphere.Core\Release Candidate\Current" /RW:"Development" /WW:"Release Candidate"
"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\Nightingale\Client Apps\NG\Atmosphere.Site\Release Candidate\Current" /RW:"Development" /WW:"Release Candidate"

tf checkin "$/Nightingale/Client Apps/NG/Atmosphere.Core/Release Candidate/Current" /comment:"Merge Changes for RC Release" /recursive /force /noprompt
tf checkin "$/Nightingale/Client Apps/NG/Atmosphere.Site/Release Candidate/Current" /comment:"Merge Changes for RC Release" /recursive /force /noprompt

REM TFSBuild start http://hillstfs2013:8080/tfs PhytelCode "Atmosphere.Site (RC)" /silent
