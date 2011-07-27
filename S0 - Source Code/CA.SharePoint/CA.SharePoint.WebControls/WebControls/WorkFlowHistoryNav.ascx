<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkFlowHistoryNav.ascx.cs"
    Inherits="CA.SharePoint.WebControls.WorkFlowHistoryNav" %>
<script type="text/javascript" src="/_layouts/CAResources/themeCA/js/framework.js"></script>
<script type="text/javascript" language="javascript">

    var date1 = new Date("2010/12/28").getTime();
    var date2 = new Date().getTime();
    var date3 = new Date("2010/11/15").getTime()

    function fun() {
        if (date1 > date2) {
            alert('This workflow is not available until 2011/01/01.\nPlease keep using the current paper form for the rest of year 2010.');
            return false;
        }

    }
    function fun1() {
        if (date3 > date2) {
            alert('This workflow is not available until 2010/11/15.');
            return false;
        }

    }
    function fun2() {
        alert('This workflow is not available at this point. Please keep using the current paper form until further notice.\n该工作流还未开始使用,在接到下一步通知前请继续使用现有纸质流程.');
        return false;
    }
 </script>

<table width="612" border="0" cellpadding="0" cellspacing="0" class="Company_New">
    <tr>
        <td width="50%" valign="top">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
              <%--  <tr>
                    <tr>
                        <td valign="top" class="line">
                            <span class="txt1">Workflow History Navigation</span>
                        </td>
                    </tr>
                </tr>--%>
                <tr>
                    <th>
                        <div id="divWrokFlowNav" runat="server" style="width: 610px;">
                            <div id="container">
                                <ul id="filter">
                                    <li class="current"><a href="#">All</a></li>
                                    <li><a href="#">IT</a></li>
                                    <li><a href="#">HR</a></li>
                                    <li><a href="#">Legal</a></li>
                                    <li><a href="#">Commercial</a></li>
                                    <li><a href="#">Construction</a></li>
                                    <li><a href="#">Finance</a></li>
                                </ul>
                                <ul id="portfolio">
                                    <li class="hr"><a href="/WorkFlowCenter/lists/TimeOffWorkflow" onclick="return fun();" >
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_1.jpg" width="110" height="90" border="0"></a> </li>
                                    <li class="hr">
                                        <a href="/WorkFlowCenter/lists/NewEmployeeEquipmentApplication">
                                            <img src="/_layouts/CAResources/themeCA/images/workflow_2.jpg" width="110" height="90" border="0">
                                        </a>
                                    </li>
                                    <li class="hr"><a href="/WorkFlowCenter/lists/BusinessCardApplication">
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_3.jpg" width="110" height="90" border="0"></a> </li>
                                    <li class="hr">
                                    <a href ="/WorkFlowCenter/lists/TravelRequestWorkflow" onclick="return fun1();">
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_4.jpg" width="110" height="90" border="0" >
                                         </a></li>
                                    <li class="commercial">
                                    <a  href="/WorkFlowCenter/lists/SupplierReticketingWorkflow"> 
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_5.jpg" width="110" height="90" border="0">
                                        </a></li>
                                    <li class="commercial">
                                        <a href="/WorkFlowCenter/lists/StoreSamplingWorkflow">
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_6.jpg" width="110" height="90" border="0">
                                        </a></li>
                                    <li class="commercial">
                                        <a href="/WorkFlowCenter/lists/NewSupplierCreationWorkflow">
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_7.jpg" width="110" height="90" border="0"></a></li>
                                    <li class="construction">
                                        <a href="/WorkFlowCenter/lists/ConstructionPurchasingWorkflow" onclick="return fun2();">
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_8.jpg" width="110" height="90" border="0"></a></li>
                                    <li class="legal">
                                        <a href="/WorkFlowCenter/lists/ChoppingApplicationWorkflow">
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_9.jpg" width="110" height="90" border="0"></a></li>
                                    <li class="it">
                                        <a href="/WorkFlowCenter/lists/ChangeRequestWorkflow">
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_10.jpg" width="110" height="90" border="0"></a></li>
                                    <li class="it">
                                    <a  href="/WorkFlowCenter/lists/ITRequestWorkFlow">
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_11.jpg" width="110" height="90" border="0"></a></li>
                                    <li class="construction">
                                        <a href="/WorkFlowCenter/lists/NewStoreConstructionBudgetApplicationWorkflow" onclick="return fun2();">
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_12.jpg" width="110" height="90" border="0"></a></li>
                                    <li class="commercial">
                                    <a  href="/WorkFlowCenter/lists/SupplierReinspectionWorkflow">
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_13.jpg" width="110" height="90" border="0"></a></li>
                                    <li class="construction">
                                        <a href="/WorkFlowCenter/lists/StoreMaintenanceWorkflow" onclick="return fun2();">
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_14.jpg" width="110" height="90" border="0"></a></li>
                                    <li class="finance">
                                        <a href="/WorkFlowCenter/lists/NonTradeSupplierSetupMaintenanceWorkflow" >
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_15.jpg" width="110" height="90" border="0"></a></li>
                                    <li class="finance">
                                        <a href="/WorkFlowCenter/lists/CreationOrder" >
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_16.jpg" width="110" height="90" border="0"></a></li>
                                    <li class="finance">
                                        <a href="/WorkFlowCenter/lists/InternalOrderMaintenanceWorkflow" >
                                        <img src="/_layouts/CAResources/themeCA/images/workflow_17.jpg" width="110" height="90" border="0"></a></li>
                                    
                                </ul>
                            </div>
                        </div>
                    </th>
                </tr>
            </table>
        </td>
    </tr>
</table>
