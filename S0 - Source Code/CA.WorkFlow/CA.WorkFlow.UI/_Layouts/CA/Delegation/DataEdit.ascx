<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataEdit.ascx.cs" Inherits="CA.WorkFlow.UI.Delegation.DataEdit" %>
<%@ Register Assembly="CA.WorkFlow.UI" Namespace="CA.WorkFlow.UI" TagPrefix="ca" %>
<div class="ca-workflow-form-buttons">
    <asp:Button ID="btnSave" runat="server" Text="OK" OnClick="btnSave_Click" CausesValidation="true" />
    <input type="button" value="Cancel" onclick="location.href = '/WorkFlowCenter/default.aspx'" />
</div>
<div id="ca-user-delegation">
    <table class="ca-workflow-form-table">
        <tr>
            <td class="label align-center w25">
                Delegate From
                <br />
                被代理人
            </td>
            <td class="value">
                <asp:Label ID="lblUser" runat="server" />
                <asp:HiddenField ID="hidUserAccount" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Delegate To
                <br />
                代理人
            </td>
            <td class="value">
                <ca:CAPeopleFinder ID="pfSelector" runat="server" AllowTypeIn="true" MultiSelect="false" Width="200" CssClass="ca-people-finder" />
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                Workflows Delegated
                <br />
                代理审批流程
            </td>
            <td class="value" id="ca-moss-menu">
                <ca:CAMossMenu ID="mossMenu" runat="server" CssClass="ca-menu" DisappearAfter="5000">
                    <StaticMenuItemStyle CssClass="ca-menu-item w35 parent-menu-item" />
                    <StaticItemTemplate>
                        <asp:CheckBox runat="server" ID="cat1" Text='<%# Eval("Text") %>' AutoPostBack="false" />
                    </StaticItemTemplate>
                    <DynamicMenuStyle CssClass="ca-sub-menu" />
                    <DynamicMenuItemStyle CssClass="ca-menu-item sub-menu-item" />
                    <DynamicItemTemplate>
                        <input type="checkbox" value='<%# Eval("Value") %>' id='ca-module-<%# Eval("Value") %>' />
                        <label for='ca-module-<%# Eval("Value") %>'><%# Eval("Text") %></label>
                    </DynamicItemTemplate>
                </ca:CAMossMenu>
                <asp:HiddenField ID="hidSelectedCategories" runat="server" />
                <asp:HiddenField ID="hidLoadStatus" runat="server" />
                <br />
                &nbsp;&nbsp;You have selected:
                <div id="ca-selected-categories">
                </div>
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                From Date
                <br />
                超始日期
            </td>
            <td class="value">
                <ca:CADateTimeControl runat="server" ID="dtBegin" DateOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="label align-center w25">
                End Date
                <br />
                结束日期
            </td>
            <td class="value">
                <ca:CADateTimeControl runat="server" ID="dtEnd" DateOnly="true" />
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        (function ($) {
            var selectedCategories = $('#ca-selected-categories');

            // Disable auto postback.
            var categories = $('#<%= this.mossMenu.ClientID %> a.parent-menu-item').each(function () {
                this.href = this.href.replace('__doPostBack', 'void');
            });

            var modules = $('#ca-moss-menu a.sub-menu-item').each(function () {
                this.href = this.href.replace('__doPostBack', 'void');
            });

            var status = $('#<%= this.hidLoadStatus.ClientID %>');

            // Store selected module Tag in a form as ",tag1,,tag2,,tag3,"
            var hid = $('#<%= this.hidSelectedCategories.ClientID %>');
            function updateTags(tag, add) {
                tag = ',' + tag + ',';
                var tags = hid.val();

                if (add) {
                    if (tags.indexOf(tag) < 0) {
                        tags += tag;
                    }
                }
                else if (tags.indexOf(tag) > -1) {
                    tags = tags.replace(tag, '');
                }

                hid.val(tags);
                status.val('ClientUpdate');
            }

            categories.click(function (event) {
                var partial = this.href.substr(this.href.indexOf(',') + 2);
                var cat = partial.substring(0, partial.length - 2);

                var checked = $(this).find(':checked').length;

                var c = modules.filter("[href*='" + cat + "\\\\']").find(':checkbox');

                if (checked) {
                    c.attr('checked', 'checked');
                    selectedCategories.append('<div class="ca-selected-category" data-category="' + cat + '">' + cat + '</div>');
                }
                else {
                    c.removeAttr('checked');
                    selectedCategories.find('div[data-category="' + cat + '"]').remove();
                }

                c.each(function () {
                    updateTags(this.value, checked);
                });
            });

            function moduleClick(cat) {
                var subModules = modules.filter("[href*='" + cat + "\\\\']");

                var allChecked = subModules.find(":checked").length;

                var c = categories.filter('[href$=",\'' + cat + '\')"]').find(':checkbox')

                var i = selectedCategories.find('div[data-category="' + cat + '"]');

                if (!allChecked) {
                    c.removeAttr('checked');
                    i.remove();
                }
                else {
                    c.attr('checked', 'checked');

                    if (!i.length) {
                        selectedCategories.append('<div class="ca-selected-category" data-category="' + cat + '">' + cat + '</div>');
                    }
                }
            }

            modules.click(function (event) {
                event.stopPropagation();

                var cb = $(this).find(':checkbox');

                updateTags(cb.val(), cb.is(':checked'));

                var partial = this.href.substr(this.href.indexOf(',') + 2);

                moduleClick(partial.substr(0, partial.indexOf('\\')));
            });

            if (status.val() === 'InitialUpdate' && hid.val()) {
                var val = hid.val();
                var tags = val.split(',');

                for (var i = 0; i < tags.length; i++) {
                    if (!tags[i]) {
                        continue;
                    }

                    var href = modules.find(':checkbox[value="' + tags[i] + '"]').attr('checked', 'checked').parent().attr('href');

                    var partial = href.substr(href.indexOf(',') + 2);
                    moduleClick(partial.substr(0, partial.indexOf('\\')));
                }
            }
        })(jQuery);
    </script>
</div>