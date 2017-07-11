ECHO OFF
SET PATH=%PATH%;C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE
set mydate=%date:~4,2%-%date:~7,2%-%date:~10,4%

set hr=%TIME: =0%
set hr=%hr:~0,2%

set hrMin=%TIME: =0%
set hrMin=%hrMin:~3,2%

set branchDate=%mydate%_%hr%-%hrMin%

REM tf branch "$/Nightingale/Client Apps/NG/Nightingale/Production/Current" "$/Nightingale/Client Apps/NG/Nightingale/Production/%branchDate%" /checkin /noprompt /silent

tf merge "$/Nightingale/Client Apps/NG/Nightingale/Release Candidate/Current" "$/Nightingale/Client Apps/NG/Nightingale/Production/Current" /recursive /nosummary /lock:checkout

tf checkout "$/Nightingale/Client Apps/NG/Nightingale/Production/Current/*.sln" /lock:checkout /recursive
tf checkout "$/Nightingale/Client Apps/NG/Nightingale/Production/Current/*.csproj" /lock:checkout /recursive
tf checkout "$/Nightingale/Client Apps/NG/Nightingale/Production/Current/*.config" /lock:checkout /recursive

"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\Nightingale\Client Apps\NG\Nightingale\Production\Current" /RW:"Release Candidate" /WW:"Production"

tf checkin "$/Nightingale/Client Apps/NG/Nightingale/Production/Current" /comment:"Merge Changes for Prod Release" /recursive /force /noprompt