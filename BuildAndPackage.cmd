@echo off
echo Custom NuGet Package Build Script for System.Windows.Forms
echo =========================================================
echo.

REM Clean the workspace
echo Cleaning workspace...
git clean -dfx
if errorlevel 1 (
    echo Error: Failed to clean workspace
    exit /b 1
)

REM Build the solution in Release configuration
echo Building solution in Release configuration...
dotnet build Winforms.sln -c Release
if errorlevel 1 (
    echo Error: Failed to build solution
    exit /b 1
)

REM Run the custom packaging script
echo Running custom packaging...
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0CreateCustomPackageStandalone.ps1"
if errorlevel 1 (
    echo Error: Failed to create custom package
    exit /b 1
)

echo.
echo Build and packaging completed successfully!
echo Check the artifacts\packages\Release\Shipping folder for the System.Windows.Forms.*.nupkg file.
pause
