@echo off
setlocal EnableDelayedExpansion

echo Installing git hooks...

REM Get Git repository root directory
for /f "delims=" %%i in ('git rev-parse --show-toplevel') do set "repo_root=%%i"

REM Set hooks directory path
set "hooks_dir=!repo_root!\.git\hooks"

echo Hooks directory: !hooks_dir!

REM Ensure hooks directory exists
if not exist "!hooks_dir!" mkdir "!hooks_dir!"

REM Copy pre-commit hook
echo Copying pre-commit hook...
copy /Y "hooks\pre-commit" "!hooks_dir!\pre-commit"
if errorlevel 1 (
    echo Failed to copy pre-commit hook
    exit /b 1
)
echo Pre-commit hook copied successfully.

REM Copy pre-push hook
echo Copying pre-push hook...
copy /Y "hooks\pre-push" "!hooks_dir!\pre-push"
if errorlevel 1 (
    echo Failed to copy pre-push hook
    exit /b 1
)
echo Pre-push hook copied successfully.

echo Git hooks installation completed.

echo.
echo Pre-commit hook content:
type "!hooks_dir!\pre-commit"

echo.
echo Pre-push hook content:
type "!hooks_dir!\pre-push"

endlocal