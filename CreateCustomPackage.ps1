# Custom NuGet Package Creation Script for System.Windows.Forms
# This script automates the manual process described in the requirements

param(
    [Parameter(Mandatory = $true)]
    [string]$OriginalPackagePath,
    
    [Parameter(Mandatory = $true)]
    [string]$CustomPackagePath,
    
    [Parameter(Mandatory = $true)]
    [string]$DesignBinDir,
    
    [switch]$WhatIf = $false
)

$ErrorActionPreference = "Stop"

Write-Host "Starting custom NuGet package creation for System.Windows.Forms..." -ForegroundColor Green
Write-Host "Original package: $OriginalPackagePath" -ForegroundColor Yellow
Write-Host "Target package: $CustomPackagePath" -ForegroundColor Yellow
Write-Host "Design bin directory: $DesignBinDir" -ForegroundColor Yellow

if ($WhatIf) {
    Write-Host "WhatIf mode: Would process package but not make changes" -ForegroundColor Cyan
    return
}

# Validate inputs
if (-not (Test-Path $OriginalPackagePath)) {
    Write-Error "Original package not found: $OriginalPackagePath"
    exit 1
}

# Ensure the Design bin directory exists
if (-not (Test-Path $DesignBinDir)) {
    Write-Error "System.Windows.Forms.Design output directory not found at: $DesignBinDir"
    exit 1
}

Write-Host "Source directory: $DesignBinDir" -ForegroundColor Cyan

# Validate that the required files exist in the source directory
$requiredFiles = @(
    "System.Windows.Forms.Primitives.dll",
    "System.Windows.Forms.Primitives.xml", 
    "System.Windows.Forms.Design.dll",
    "System.Windows.Forms.Design.xml"
)

$missingFiles = @()
foreach ($file in $requiredFiles) {
    if (-not (Test-Path (Join-Path $DesignBinDir $file))) {
        $missingFiles += $file
    }
}

if ($missingFiles.Count -gt 0) {
    Write-Warning "Some required files are missing from the source directory:"
    foreach ($file in $missingFiles) {
        Write-Warning "  - $file"
    }
    Write-Warning "The package will be created but may be incomplete. Ensure System.Windows.Forms.Design project is built correctly."
}

# Create working directory
$WorkingDir = Join-Path ([System.IO.Path]::GetDirectoryName($CustomPackagePath)) "temp_package_work"
$ExtractedPackageDir = Join-Path $WorkingDir "extracted"

if (Test-Path $WorkingDir) {
    Remove-Item $WorkingDir -Recurse -Force
}
New-Item -ItemType Directory -Path $ExtractedPackageDir -Force | Out-Null

try {
    Write-Host "Extracting original package..." -ForegroundColor Yellow
    
    # Extract the nupkg (it's a zip file)
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [System.IO.Compression.ZipFile]::ExtractToDirectory($OriginalPackagePath, $ExtractedPackageDir)
    
    Write-Host "Renaming nuspec file..." -ForegroundColor Yellow
    
    # Rename the nuspec file
    $originalNuspec = Join-Path $ExtractedPackageDir "WTG.System.Windows.Forms.nuspec"
    $newNuspec = Join-Path $ExtractedPackageDir "System.Windows.Forms.nuspec"
    
    if (Test-Path $originalNuspec) {
        Move-Item $originalNuspec $newNuspec -Force
    } else {
        Write-Error "Original nuspec file not found: $originalNuspec"
        exit 1
    }
    
    Write-Host "Modifying nuspec content..." -ForegroundColor Yellow
    
    # Modify the nuspec file content
    $content = Get-Content $newNuspec -Raw -Encoding UTF8
    
    # Replace the package ID
    $content = $content -replace '<id>WTG\.System\.Windows\.Forms</id>', '<id>System.Windows.Forms</id>'
    
    # Remove the System.Windows.Forms.Primitives dependency line
    $content = $content -replace '\s*<dependency\s+id="System\.Windows\.Forms\.Primitives"[^>]*version="[^"]*"[^/]*/>\s*', ''
    
    # Clean up any extra whitespace
    $content = $content -replace '\r?\n\s*\r?\n', "`r`n"
    
    Set-Content $newNuspec -Value $content -Encoding UTF8
    
    Write-Host "Copying additional files from System.Windows.Forms.Design..." -ForegroundColor Yellow
    
    $libNetDir = Join-Path $ExtractedPackageDir "lib\net8.0"
    Write-Host "  Target directory: $libNetDir" -ForegroundColor Cyan
    
    # Copy the main files (these should replace any existing files)
    $filesToCopy = @(
        "System.Windows.Forms.Primitives.dll",
        "System.Windows.Forms.Primitives.xml", 
        "System.Windows.Forms.Design.dll",
        "System.Windows.Forms.Design.xml"
    )
    
    Write-Host "  Copying main files..." -ForegroundColor Cyan
    foreach ($file in $filesToCopy) {
        $sourcePath = Join-Path $DesignBinDir $file
        $destPath = Join-Path $libNetDir $file
        
        if (Test-Path $sourcePath) {
            $existsAlready = Test-Path $destPath
            Copy-Item $sourcePath $destPath -Force
            if ($existsAlready) {
                Write-Host "    Replaced: $file" -ForegroundColor Green
            } else {
                Write-Host "    Copied: $file" -ForegroundColor Green
            }
        } else {
            Write-Warning "    File not found: $sourcePath"
        }
    }
    
    # Copy resource folders (two-character country codes)
    $resourceFolders = Get-ChildItem $DesignBinDir -Directory
    
    if ($resourceFolders.Count -gt 0) {
        Write-Host "  Found $($resourceFolders.Count) resource folders to copy" -ForegroundColor Cyan
    }
    
    foreach ($folder in $resourceFolders) {
        $destFolder = Join-Path $libNetDir $folder.Name
        
        # Create the destination folder
        New-Item -ItemType Directory -Path $destFolder -Force | Out-Null
        
        # Copy all files from source folder to destination
        $sourceFiles = Get-ChildItem $folder.FullName -File
        if ($sourceFiles.Count -gt 0) {
            Copy-Item $sourceFiles.FullName $destFolder -Force
            Write-Host "  Copied resource folder: $($folder.Name) ($($sourceFiles.Count) files)" -ForegroundColor Green
        } else {
            Write-Host "  Resource folder $($folder.Name) is empty, skipping" -ForegroundColor DarkYellow
        }
    }
    
    Write-Host "Creating new package..." -ForegroundColor Yellow
    
    # Create the new package
    if (Test-Path $CustomPackagePath) {
        Remove-Item $CustomPackagePath -Force
    }
    
    [System.IO.Compression.ZipFile]::CreateFromDirectory($ExtractedPackageDir, $CustomPackagePath)
    
    Write-Host "Package created successfully: $CustomPackagePath" -ForegroundColor Green
    
    # Display package info
    $packageInfo = Get-ChildItem $CustomPackagePath
    Write-Host "Package size: $([math]::Round($packageInfo.Length / 1MB, 2)) MB" -ForegroundColor Cyan
    
    # Show summary of what was included
    Write-Host ""
    Write-Host "Package contents summary:" -ForegroundColor Yellow
    Write-Host "  - Original System.Windows.Forms files from WTG package" -ForegroundColor White
    Write-Host "  - Additional files from System.Windows.Forms.Design:" -ForegroundColor White
    $filesToCopy = @(
        "System.Windows.Forms.Primitives.dll",
        "System.Windows.Forms.Primitives.xml", 
        "System.Windows.Forms.Design.dll",
        "System.Windows.Forms.Design.xml"
    )
    foreach ($file in $filesToCopy) {
        $sourcePath = Join-Path $DesignBinDir $file
        if (Test-Path $sourcePath) {
            Write-Host "    [OK] $file" -ForegroundColor Green
        } else {
            Write-Host "    [MISSING] $file (not found)" -ForegroundColor Red
        }
    }
    
    if ($resourceFolders.Count -gt 0) {
        Write-Host "  - Resource folders: $($resourceFolders.Name -join ', ')" -ForegroundColor White
    } else {
        Write-Host "  - No resource folders found" -ForegroundColor DarkYellow
    }
    
} finally {
    # Clean up working directory
    if (Test-Path $WorkingDir) {
        Remove-Item $WorkingDir -Recurse -Force
    }
}

Write-Host "Custom NuGet package creation completed!" -ForegroundColor Green
