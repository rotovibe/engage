ECHO OFF
SET PATH=%PATH%;C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE

tf merge "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Development/Current" "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Release Candidate/Current" /recursive /nosummary /lock:checkout

tf checkout "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Release Candidate/Current/*.sln" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Release Candidate/Current/*.csproj" /lock:checkout /recursive
tf checkout "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Release Candidate/Current/*.config" /lock:checkout /recursive

"UpdateRCProjectInfo.exe" /RF:"C:\Projects\TFS2013\PhytelCode\Phytel.Net\Services\API\NGExtract\Release Candidate\Current" /RW:"Development" /WW:"Release Candidate"

tf checkin "$/PhytelCode/Phytel.Net/Services/API/NGExtract/Release Candidate/Current" /comment:"Merge Changes for RC Release" /recursive /force /noprompt
