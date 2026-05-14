::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Install MCExternalFishingBot Via MSBuild                                             ::
:: Gihub https://github.com/RussDev7/Undetected-External-Auto-Fishing-Bot-For-Minecraft ::
:: Developed and maintained by RussDev7 / Discord: dannyruss                            ::
::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
@ECHO OFF
SETLOCAL ENABLEDELAYEDEXPANSION

REM ============================================================
REM Paths
REM ============================================================
SET "RootDir=%~dp0"
SET "SolutionPath=%RootDir%MCExternalFishingBot.sln"
SET "ProjectOutput=%RootDir%MCExternalFishingBot\bin\x64\Release"
SET "ReleaseDir=%RootDir%release"
SET "ExeName=MCExternalFishingBot.exe"

REM ============================================================
REM Find Visual Studio / Build Tools MSBuild
REM ============================================================
SET "VsWhere=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"

IF NOT EXIST "%VsWhere%" (
    ECHO ERROR: vswhere.exe was not found.
    ECHO Install Visual Studio or Visual Studio Build Tools with .NET desktop build tools.
    PAUSE
    EXIT /B 1
)

FOR /F "usebackq tokens=*" %%I IN (`"%VsWhere%" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) DO (
    SET "MSBuildPath=%%I"
)

IF NOT DEFINED MSBuildPath (
    ECHO ERROR: Could not find Visual Studio MSBuild.
    ECHO Install Visual Studio Build Tools and include MSBuild / .NET desktop workload.
    PAUSE
    EXIT /B 1
)

ECHO Using MSBuild:
ECHO "%MSBuildPath%"
ECHO(

REM ============================================================
REM Build Solution
REM ============================================================
ECHO Building MCExternalFishingBot.sln...

"%MSBuildPath%" "%SolutionPath%" ^
    /m ^
    /restore ^
    /p:Configuration=Release ^
    /p:Platform=x64

IF ERRORLEVEL 1 (
    ECHO(
    ECHO Build failed.
    PAUSE
    EXIT /B 1
)

REM ============================================================
REM Read File Version From Built EXE
REM ============================================================
IF NOT EXIST "%ProjectOutput%\%ExeName%" (
    ECHO ERROR: Built EXE was not found:
    ECHO "%ProjectOutput%\%ExeName%"
    PAUSE
    EXIT /B 1
)

FOR /F "usebackq tokens=*" %%V IN (`powershell.exe -NoLogo -NoProfile -Command "(Get-Item '%ProjectOutput%\%ExeName%').VersionInfo.FileVersion"`) DO (
    SET "VersionPrefix=%%V"
)

IF NOT DEFINED VersionPrefix (
    ECHO ERROR: Could not read file version from EXE.
    PAUSE
    EXIT /B 1
)

SET "FileName=MCExternalFishingBot-%VersionPrefix%"

ECHO(
ECHO Detected File Version: %VersionPrefix%
ECHO Release Name: %FileName%
ECHO(

REM ============================================================
REM Recreate Release Folder
REM ============================================================
IF EXIST "%ReleaseDir%" (
    RMDIR /S /Q "%ReleaseDir%"
)

MKDIR "%ReleaseDir%\%FileName%"

REM ============================================================
REM Copy Build Output
REM ============================================================
XCOPY /E /Y /I "%ProjectOutput%\*" "%ReleaseDir%\%FileName%\"

IF ERRORLEVEL 1 (
    ECHO(
    ECHO Copy failed.
    PAUSE
    EXIT /B 1
)

REM ============================================================
REM Clean Release Files
REM ============================================================
DEL /F /Q /S "%ReleaseDir%\*.pdb" >NUL 2>&1
DEL /F /Q /S "%ReleaseDir%\*.config" >NUL 2>&1

REM ============================================================
REM Create ZIP Release
REM ============================================================
IF EXIST "%RootDir%%FileName%.zip" (
    DEL /F /Q "%RootDir%%FileName%.zip"
)

powershell.exe -NoLogo -NoProfile -Command ^
    "Compress-Archive -Path '%ReleaseDir%\%FileName%' -DestinationPath '%RootDir%%FileName%.zip' -Force"

IF ERRORLEVEL 1 (
    ECHO(
    ECHO ZIP creation failed.
    PAUSE
    EXIT /B 1
)

REM ============================================================
REM Operation Complete
REM ============================================================
ECHO(
ECHO Build complete.
ECHO Created: "%RootDir%%FileName%.zip"
ECHO(
PAUSE
ENDLOCAL