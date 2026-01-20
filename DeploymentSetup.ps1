$pkgId = 'CargoWiseCloudDeployment'

# this uses the latest version of the package, as listed at https://proget.wtg.zone/packages?PackageSearch=CargoWiseCloudDeployment
# if you need a specific version, you can use:
#$pkgVersion = "1.0.0.70"
#url = "https://proget.wtg.zone/nuget/WTG-Internal/package/${pkgId}/$pkgVersion"

$Destination = '.\bin'

$pkgVersion = "1.0.0.362" # Set this to $null for most recent package.
$pkgRoot = Join-Path $Destination ("$pkgId.$pkgVersion")
$url = "https://proget.wtg.zone/nuget/WTG-Internal/package/${pkgId}/$pkgVersion"


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
Copy-Item -Path (Join-Path (Join-Path $pkgRoot '\lib\net10.0-windows7.0') '*') -Destination $Destination -Recurse -Force

Remove-Item -Path "$pkgRoot.zip" -Force
Remove-Item -Path $pkgRoot -Recurse -Force