REM ***** RUN AS Administrator *****
@setlocal enableextensions
@cd /d "%~dp0"
copy dsofile_x64.dll %WINDIR%\SysWOW64\dsofile_x64.dll
%WINDIR%\SysWOW64\RegSvr32.exe /s dsofile_x64.dll

