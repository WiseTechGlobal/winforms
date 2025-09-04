$pkgId = 'CargoWiseCloudDeployment'
$pkgVersion = '1.0.0.4' # Update this to the latest version as needed
$Destination = '.\bin'

$pkgRoot = Join-Path $Destination ("$pkgId.$pkgVersion")
$url = "https://proget.wtg.zone/nuget/WTG-Internal/package/${pkgId}/${pkgVersion}"
try {
	Invoke-WebRequest -Uri $url -OutFile "$pkgRoot.zip" -ErrorAction Stop
	Write-Host "download nuget successfully"
}
catch {
	Write-Error "download nuget failed: $($_.Exception.Message)"
}

try {
	Expand-Archive -Path "$pkgRoot.zip" -DestinationPath $pkgRoot -Force
	Write-Host "unzip successfully" -ForegroundColor Green
}
catch {
	Write-Error "unzip failed: $($_.Exception.Message)"
}
Copy-Item -Path (Join-Path (Join-Path $pkgRoot '\content') '*') -Destination $Destination -Recurse -Force
Copy-Item -Path (Join-Path (Join-Path $pkgRoot '\lib\net48') '*') -Destination $Destination -Recurse -Force