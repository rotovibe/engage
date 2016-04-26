ECHO OFF
SET PATH=%PATH%;C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE

tf merge "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Release Candidate/Current" "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Production/Current" /recursive /nosummary /lock:checkout

tf checkout "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Production/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Production/Current/*.csproj" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Production/Current/*.config" /lock:checkout /recursive

"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Services\API\NGExtract\Production\Current" /RW:"Release Candidate" /WW:"Production"

tf checkin "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Production/Current" /comment:"Merge Changes for Prod Release" /recursive /force /noprompt