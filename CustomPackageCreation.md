# Automated NuGet Package Creation for System.Windows.Forms

This document describes the automated NuGet package creation process that replaces the manual steps previously required.

## Overview

The automated system creates a custom `System.Windows.Forms` NuGet package from the `WTG.System.Windows.Forms` package built by the standard MSBuild process. The automation includes:

1. Extracting the original `WTG.System.Windows.Forms.*.nupkg` package
2. Renaming the `.nuspec` file from `WTG.System.Windows.Forms.nuspec` to `System.Windows.Forms.nuspec`
3. Modifying the package ID in the `.nuspec` file to `System.Windows.Forms`
4. Removing the `System.Windows.Forms.Primitives` dependency
5. Copying additional files from `System.Windows.Forms.Design` build output
6. Copying resource folders (localization files)
7. Creating the final `System.Windows.Forms.*.nupkg` package

## Automated Methods

### Method 1: MSBuild Integration (Recommended)

The automation is integrated into the MSBuild process via custom targets that call a dedicated PowerShell script.

**Files involved:**
- `eng/CustomPackaging.targets` - MSBuild targets file (calls PowerShell script)
- `CreateCustomPackage.ps1` - Core PowerShell script with packaging logic
- `src/System.Windows.Forms/src/System.Windows.Forms.csproj` - Updated to import the custom targets

**How to use:**
```bash
# Build the solution in Release configuration
dotnet build Winforms.sln -c Release

# The custom package will be automatically created in:
# artifacts/packages/Release/Shipping/System.Windows.Forms.*.nupkg
```

**What happens:**
1. Standard MSBuild packaging creates `WTG.System.Windows.Forms.*.nupkg`
2. Custom target `CreateCustomNuGetPackage` runs after packaging
3. MSBuild calls `CreateCustomPackage.ps1` with appropriate parameters
4. Final `System.Windows.Forms.*.nupkg` is created alongside the original

### Method 2: Standalone PowerShell Script

For more control, debugging, or manual package creation from existing build outputs.

**File:** `CreateCustomPackageStandalone.ps1`

**Usage:**
```powershell
# Run with default Release configuration (auto-discovers latest package)
.\CreateCustomPackageStandalone.ps1

# Run with specific configuration
.\CreateCustomPackageStandalone.ps1 -Configuration Release

# Preview mode (shows what would be done without making changes)
.\CreateCustomPackageStandalone.ps1 -WhatIf
```

This script automatically finds the latest `WTG.System.Windows.Forms.*.nupkg` in the artifacts directory and processes it.

### Method 3: Batch File (Simplest)

For users who prefer a simple click-and-run approach.

**File:** `BuildAndPackage.cmd`

**Usage:**
1. Double-click the `BuildAndPackage.cmd` file, or
2. Run from command prompt: `BuildAndPackage.cmd`

This will:
1. Clean the workspace (`git clean -dfx`)
2. Build the solution in Release configuration
3. Run the standalone custom packaging script

## Output

After running any of the methods above, you'll find:

- **Original package:** `artifacts/packages/Release/Shipping/WTG.System.Windows.Forms.*.nupkg`
- **Custom package:** `artifacts/packages/Release/Shipping/System.Windows.Forms.*.nupkg`

The custom package is the one you should push to ProGet.

## Publishing to ProGet

Once the custom package is created, push it to ProGet:

```bash
dotnet nuget push "artifacts/packages/Release/Shipping/System.Windows.Forms.*.nupkg" \
  --api-key "your-api-key" \
  --source "https://proget.wtg.zone/nuget/WTG-Internal/v3/index.json"
```

Verify the package at: https://proget.wtg.zone/feeds/WTG-Internal/System.Windows.Forms/versions

## Technical Details

### Files Modified

1. **System.Windows.Forms.csproj**: Added import for `CustomPackaging.targets`

### Files Added

1. **eng/CustomPackaging.targets**: MSBuild targets file that calls the PowerShell script
2. **CreateCustomPackage.ps1**: Core PowerShell script with all packaging logic  
3. **CreateCustomPackageStandalone.ps1**: Standalone wrapper script for manual use
4. **BuildAndPackage.cmd**: Simple batch file wrapper
5. **CustomPackageCreation.md**: This documentation

### Architecture

The refactored solution uses a clean separation of concerns:

- **MSBuild targets**: Handle integration with the build process and parameter passing
- **Core PowerShell script**: Contains all the packaging logic (can be called from MSBuild or standalone)
- **Standalone wrapper**: Provides auto-discovery and convenience for manual testing
- **Batch wrapper**: Simple entry point for non-technical users

### Custom Target Details

The `CreateCustomNuGetPackage` target:
- Only runs in Release configuration
- Only runs for the `System.Windows.Forms` project  
- Validates that required files exist
- Calls `CreateCustomPackage.ps1` with the correct parameters
- Provides detailed logging

### Dependencies

The automation requires:
- PowerShell (available on Windows by default)
- .NET SDK (for the build process)
- Git (for workspace cleaning)

## Troubleshooting

### Common Issues

1. **Package not found**: Ensure the solution builds successfully in Release configuration first
2. **Design files missing**: Verify that `System.Windows.Forms.Design` project builds correctly
3. **PowerShell execution policy**: If PowerShell scripts are blocked, run: `Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope CurrentUser`

### Debug Mode

To debug the packaging process:
1. Use the standalone PowerShell script with `-WhatIf` flag
2. Check MSBuild output for custom target messages
3. Look for temporary files in `artifacts/packages/Release/Shipping/temp_package_work` (if cleanup fails)

### Manual Fallback

If automation fails, the original manual process can still be used as documented in the original requirements.

## Maintenance

### Updating Package Dependencies

If the dependencies in the `.nuspec` file need to be updated:
1. Update the original `WTG.System.Windows.Forms.nuspec` file
2. Update the PowerShell script in `CustomPackaging.targets` if new dependency removal logic is needed

### Version Updates

The automation automatically uses the version from the built package, so no manual version updates are required.

### Adding New Files

To include additional files in the custom package:
1. Update the `$filesToCopy` array in the PowerShell script
2. Ensure the files are available in the `System.Windows.Forms.Design` output directory
