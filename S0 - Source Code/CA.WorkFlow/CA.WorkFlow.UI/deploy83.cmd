@SET STSADM="c:\program files\common files\microsoft shared\web server extensions\12\bin\STSADM"
@SET WFSOURCE=D:\CA_P1\S0 - Source Code\CA.WorkFlow
@SET SPSOURCE=D:\CA_P1\S0 - Source Code\CA.SharePoint
@SET TEMPLATE=C:\Program Files\Common Files\Microsoft Shared\web server extensions\12\TEMPLATE
@SET IISPATH=C:\Inetpub\wwwroot\wss\VirtualDirectories\9000
@SET WFSITE=http://toddy:9000/WorkFlowCenter

echo Prepare to deploy, press ANY KEY to start
pause


echo -------------------------------------------------
echo Start to deactive features
%STSADM% -o deactivatefeature -name CommonDataAutoFill -url %WFSITE%
%STSADM% -o deactivatefeature -name CommonList -url %WFSITE%
%STSADM% -o deactivatefeature -name CreationOrder -url %WFSITE%
%STSADM% -o deactivatefeature -name InternalOrderMaintenanceWorkflow -url %WFSITE%
%STSADM% -o deactivatefeature -name NonTradeSupplierSetupMaintenanceWorkflow -url %WFSITE%
echo End


echo -------------------------------------------------
echo Start to uninstall features
%STSADM% -o uninstallfeature -name CommonDataAutoFill -force
%STSADM% -o uninstallfeature -name CommonList -force
%STSADM% -o uninstallfeature -name CreationOrder -force
%STSADM% -o uninstallfeature -name InternalOrderMaintenanceWorkflow -force
%STSADM% -o uninstallfeature -name NonTradeSupplierSetupMaintenanceWorkflow -force
echo End

iisreset


echo -------------------------------------------------
echo Start to copy master page
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\CA\Application.Master" "%TEMPLATE%\LAYOUTS\CA\Application.Master"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\CA\Layout.Master" "%TEMPLATE%\LAYOUTS\CA\Layout.Master"
echo End


echo -------------------------------------------------
echo Start to copy theme resource, web controls
xcopy /s /Y /C /R "%SPSOURCE%\CA.SharePoint.Web\12\TEMPLATE\LAYOUTS\CAResources\themeCA\*" "%TEMPLATE%\LAYOUTS\CAResources\themeCA\"
xcopy /s /Y /C /R "%SPSOURCE%\CA.SharePoint.WebControls\WebControls\AllTasks.ascx" "%TEMPLATE%\LAYOUTS\CA\WebControls\"
xcopy /s /Y /C /R "%SPSOURCE%\CA.SharePoint.WebControls\WebControls\CALeaveData.ascx" "%TEMPLATE%\LAYOUTS\CA\WebControls\"
xcopy /s /Y /C /R "%SPSOURCE%\CA.SharePoint.WebControls\WebControls\WorkFlowHistoryNav.ascx" "%TEMPLATE%\LAYOUTS\CA\WebControls\"
xcopy /s /Y /C /R "%SPSOURCE%\CA.SharePoint.WebControls\WebControls\WorkFlowNav.ascx" "%TEMPLATE%\LAYOUTS\CA\WebControls\"
echo End

echo -------------------------------------------------
echo Start to copy web config
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\web.config" "%TEMPLATE%\LAYOUTS\"
echo End


echo -------------------------------------------------
echo Start to copy application page
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\CA\WorkFlows\NonTradeSupplierSetupMaintenance\*.aspx" "%TEMPLATE%\LAYOUTS\CA\WorkFlows\NonTradeSupplierSetupMaintenance\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\CA\WorkFlows\NonTradeSupplierSetupMaintenance\*.ascx" "%TEMPLATE%\LAYOUTS\CA\WorkFlows\NonTradeSupplierSetupMaintenance\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\CA\WorkFlows\CreationOrder\*.aspx" "%TEMPLATE%\LAYOUTS\CA\WorkFlows\CreationOrder\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\CA\WorkFlows\CreationOrder\*.ascx" "%TEMPLATE%\LAYOUTS\CA\WorkFlows\CreationOrder\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\CA\WorkFlows\InternalOrderMaintenance\*.aspx" "%TEMPLATE%\LAYOUTS\CA\WorkFlows\InternalOrderMaintenance\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\CA\WorkFlows\InternalOrderMaintenance\*.ascx" "%TEMPLATE%\LAYOUTS\CA\WorkFlows\InternalOrderMaintenance\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\CA\WorkFlows\TimeOff\*.aspx" "%TEMPLATE%\LAYOUTS\CA\WorkFlows\TimeOff\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\CA\WorkFlows\TimeOff\*.ascx" "%TEMPLATE%\LAYOUTS\CA\WorkFlows\TimeOff\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\_Layouts\CA\WorkFlows\UserControl\TaskTrace.ascx" "%TEMPLATE%\LAYOUTS\CA\WorkFlows\UserControl\"
echo End


echo -------------------------------------------------
echo Start to copy feature files
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\Features\CommonDataAutoFill\*" "%TEMPLATE%\FEATURES\CommonDataAutoFill\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\Features\CommonList\*" "%TEMPLATE%\FEATURES\CommonList\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\Features\CreationOrder\*" "%TEMPLATE%\FEATURES\CreationOrder\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\Features\InternalOrderMaintenanceWorkflow\*" "%TEMPLATE%\FEATURES\InternalOrderMaintenanceWorkflow\"
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\Features\NonTradeSupplierSetupMaintenanceWorkflow\*" "%TEMPLATE%\FEATURES\NonTradeSupplierSetupMaintenanceWorkflow\"
echo End


echo -------------------------------------------------
echo Start to install features
%STSADM% -o installfeature -name CommonList
%STSADM% -o installfeature -name CreationOrder
%STSADM% -o installfeature -name InternalOrderMaintenanceWorkflow
%STSADM% -o installfeature -name NonTradeSupplierSetupMaintenanceWorkflow
echo End


echo -------------------------------------------------
echo Start to active features
%STSADM% -o activatefeature -name CommonList -url %WFSITE%
%STSADM% -o activatefeature -name CreationOrder -url %WFSITE%
%STSADM% -o activatefeature -name InternalOrderMaintenanceWorkflow -url %WFSITE%
%STSADM% -o activatefeature -name NonTradeSupplierSetupMaintenanceWorkflow -url %WFSITE%
echo End


echo -------------------------------------------------
echo Auto Fill Data for common list
%STSADM% -o installfeature -name CommonDataAutoFill
%STSADM% -o activatefeature -name CommonDataAutoFill -url %WFSITE%


echo -------------------------------------------------
echo Start to copy dll
xcopy /s /Y /C /R "%WFSOURCE%\CA.WorkFlow.UI\bin\CA.WorkFlow.UI.*" "%IISPATH%\bin\"
xcopy /s /Y /C /R "%WFSOURCE%\..\ref\CA.SharePoint.Utilities.dll" "%IISPATH%\bin\"
echo End


echo -------------------------------------------------
iisreset


pause
 