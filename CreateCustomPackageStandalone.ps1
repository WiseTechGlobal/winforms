# Standalone Custom NuGet Package Creation Script for System.Windows.Forms
# This script can be used independently for testing or manual package creation

param(
    [string]$Configuration = "Release",
    [string]$ArtifactsDir = "$PSScriptRoot\artifacts",
    [switch]$WhatIf = $false
)

$ErrorActionPreference = "Stop"

Write-Host "Standalone custom NuGet package creation for System.Windows.Forms..." -ForegroundColor Green

# Define paths
$PackagesOutputDir = Join-Path $ArtifactsDir "packages\$Configuration\Shipping"
$DesignBinDir = Join-Path $ArtifactsDir "bin\System.Windows.Forms.Design\$Configuration\net8.0"

# Find the original package
$OriginalPackages = Get-ChildItem $PackagesOutputDir -Filter "WTG.System.Windows.Forms.*.nupkg" -ErrorAction SilentlyContinue | Sort-Object LastWriteTime -Descending
if ($OriginalPackages.Count -eq 0) {
    Write-Error "No WTG.System.Windows.Forms package found in $PackagesOutputDir. Make sure the solution is built in $Configuration configuration."
    exit 1
}

$OriginalPackage = $OriginalPackages[0]
$OriginalPackagePath = $OriginalPackage.FullName
$PackageVersion = $OriginalPackage.Name -replace "WTG\.System\.Windows\.Forms\.(.*?)\.nupkg", '$1'
$CustomPackageName = "System.Windows.Forms.$PackageVersion.nupkg"
$CustomPackagePath = Join-Path $PackagesOutputDir $CustomPackageName

Write-Host "Using the main PowerShell script..." -ForegroundColor Yellow

# Call the main PowerShell script
$mainScriptPath = Join-Path $PSScriptRoot "CreateCustomPackage.ps1"
if (-not (Test-Path $mainScriptPath)) {
    Write-Error "Main PowerShell script not found at: $mainScriptPath"
    exit 1
}

$params = @{
    OriginalPackagePath = $OriginalPackagePath
    CustomPackagePath = $CustomPackagePath
    DesignBinDir = $DesignBinDir
}

if ($WhatIf) {
    $params.WhatIf = $true
}

& $mainScriptPath @params

if (-not $WhatIf) {
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "1. To push to proget: dotnet nuget push `"$CustomPackagePath`" --api-key `"your-api-key`" --source `"https://proget.wtg.zone/nuget/WTG-Internal/v3/index.json`"" -ForegroundColor White
    Write-Host "2. Verify at: https://proget.wtg.zone/feeds/WTG-Internal/System.Windows.Forms/versions" -ForegroundColor White
}
