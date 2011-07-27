@SET STSADM="c:\program files\common files\microsoft shared\web server extensions\12\bin\STSADM"
@SET WFSOURCE=D:\CA\Dev\Wsq\CA_EWF_2011_P1\S0 - Source Code\CA.WorkFlow
@SET SPSOURCE=D:\CA\Dev\Wsq\CA_EWF_2011_P1\S0 - Source Code\CA.SharePoint
@SET TEMPLATE=C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\TEMPLATE
@SET IISPATH=C:\Inetpub\wwwroot\wss\VirtualDirectories\83
@SET WFSITE=http://172.17.1.45:91/WorkFlowCenter

echo Prepare to deploy, press ANY KEY to start
pause

echo -------------------------------------------------
echo Start to active features
%STSADM% -o activatefeature -name CommonList -url %WFSITE%
%STSADM% -o activatefeature -name CreationOrder -url %WFSITE%
%STSADM% -o activatefeature -name InternalOrderMaintenanceWorkflow -url %WFSITE%
%STSADM% -o activatefeature -name NonTradeSupplierSetupMaintenanceWorkflow -url %WFSITE%
echo End

echo -------------------------------------------------
echo Auto Fill Data for common list
%STSADM% -o activatefeature -name CommonDataAutoFill -url %WFSITE%

echo -------------------------------------------------
echo Start to copy dll
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\bin\CA.WorkFlow.UI.*" "%IISPATH%\bin\"
xcopy /s /Y /C /R "%WFSOURCE%\..\ref\CA.SharePoint.Utilities.dll" "%IISPATH%\bin\"
echo End


echo -------------------------------------------------
iisreset


pause
 