$url="http://client.telebank-online.com/SignSchool/Login.aspx?SENDEMAILS=yes846ac45fe18364dda48b87d719f3d52d"
$webClient = New-Object System.Net.WebClient
$webclient.DownloadString("$url");