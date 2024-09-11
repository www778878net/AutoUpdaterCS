@echo off
setlocal enabledelayedexpansion

echo Pre-commit hook is running...

REM Get current  branch name
for /f "delims=" %%i in ('git rev-parse --abbrev-ref HEAD') do set "current_branch=%%i"

if "!current_branch!" == "develop" (
    echo Current branch is develop. Running pre-commit checks...

    

    REM Switch to repository root directory
    for /f "delims=" %%i in ('git rev-parse --show-toplevel') do set "repo_root=%%i"
    cd /d "!repo_root!"
    echo Switched to repository root: !cd!

    REM 设置 Visual Studio 环境变量
    call "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat"

    REM 查找 .csproj 文件
    for /f "delims=" %%i in ('dir /b /s *.csproj') do set "csproj_file=%%i"
    if not defined csproj_file (
        echo Error: Could not find a .csproj file in the repository.
        exit /b 1
    )
    echo Found project file: !csproj_file!

    REM 构建项目
    echo Building project...
    msbuild "!csproj_file!" /p:Configuration=Release /p:Platform=x64
    if !errorlevel! neq 0 (
        echo Build failed, commit aborted
        exit /b 1
    )

    echo Build succeeded

    REM Add generated files to  Git staging area
    echo Adding generated files to Git staging area...
    git add .
    echo Files added to staging area

    echo Pre-commit hook execution completed
) else (
    echo Current branch is !current_branch!. Skipping pre-commit checks.
)

endlocal