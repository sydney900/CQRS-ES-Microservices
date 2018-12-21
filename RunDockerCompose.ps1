 $env:MYIP =  Get-NetIPAddress -AddressFamily IPv4 |Select -ExpandProperty IPV4Address | Select-Object  -First 1
 Docker-compose -f docker-compose.yml up