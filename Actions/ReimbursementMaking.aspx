<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="ReimbursementMaking.aspx.cs" Inherits="SalaryModule_ReimbursementMaking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script language="javascript" type="text/javascript">

        function disable_textbox(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode > 0) && (charCode != 9))
                return false;
            return true;
        }

        function CheckAllDataGridCheckBoxes(aspCheckBoxID, checkVal) {
            re = new RegExp(aspCheckBoxID + '$')  //generated control name contains a $
            for (i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i]
                if (elm.type == 'checkbox' && elm.disabled == false) {
                    if (re.test(elm.name)) {
                        elm.checked = checkVal
                    }
                }
            }
        }

        function ConfirmDeactivate() {
            if (confirm("Are you sure you want to Deactivate this Record?") == true)
                return true;
            else
                return false;
        }

        function validate() {
            var Count = 0;
            var gridView = document.getElementById("<%=grdrecord.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    Count = Count + 1;
                }
            }
            if (Count > 0) {
                return true;
            }
            else {
                alert("Please Select at least one Row.");
                return false;
            }
        }

    </script>

    <asp:UpdateProgress ID="MyProcess" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="5">
        <ProgressTemplate>
            <div style="left: 0; position: fixed; width: 100%; height: 100%; z-index: 9999999; top: 0; background: rgba(0,0,0,0.5);">
                <div style="text-align: center; z-index: 10; margin: 300px auto;">
                    <img alt="img" src="../Images/loading-gif-animation.gif" style="height: 100px; width: 100px;" /><br />
                    <br />
                    <span>
                        <h4>
                            <asp:Label runat="server" Text="Please Wait..." ID="lblPleaseWait"></asp:Label>
                    </span>
                    </h4>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 25px; height: auto;">
                <fieldset>
                    <center>
                        <legend>Reimbursement Making</legend>

                        <asp:Panel ID="pnlData" runat="server">

                            <table style="margin: 5px 0 0 21px;">
                                <tr>

                                    <td>
                                        <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="MakeReimbursement"></asp:RequiredFieldValidator>
                                    </td>

                                    <td>
                                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="MakeReimbursement"></asp:RequiredFieldValidator>
                                    </td>

                                    <td>
                                        <asp:Button ID="btnMakeReimbursement" runat="server" Text="Get Records" CssClass="btn btn-default" ValidationGroup="MakeReimbursement" OnClick="btnMakeReimbursement_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <style type="text/css">
                .grdTable {
                    max-width: 200% !Important;
                    min-width: 200%;
                }
            </style>
            <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                <table>
                    <tr>
                        <td colspan="3">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnCalculateReimbursement" runat="server" Text="Calculate Reimbursement" CssClass="btn btn-default" OnClientClick="return validate()" OnClick="btnCalculateReimbursement_Click" />
                        </td>
                        <td></td>
                        <td>
                            <asp:Button ID="btnSaveReimbursement" runat="server" Text="Save Reimbursement" CssClass="btn btn-default" Visible="false" OnClientClick="return validate()" OnClick="btnSaveReimbursement_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnlDetail" runat="server" Visible="false" Style="margin-top: 20px; max-height: 1000px; width: 100%; overflow: auto; margin-bottom: 20px;">

                <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" CssClass="grdTable" OnRowCommand="grdrecord_RowCommand" OnRowDataBound="grdrecord_RowDataBound">
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#C8C6C6" Font-Bold="True" ForeColor="Black" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="#E2DED6" ForeColor="#284775" />
                    <RowStyle HorizontalAlign="Center" BackColor="#F7F6F3" ForeColor="#284775" />
                    <EmptyDataRowStyle ForeColor="Red" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input id="chkAllItems" type="checkbox" onclick="CheckAllDataGridCheckBoxes('SelectChk', document.forms[0].chkAllItems.checked)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="SelectChk" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDeactivate" runat="server" Text="Deactivate" OnClientClick="return ConfirmDeactivate()" CommandName="lnkDeactivate" CommandArgument='<%# Eval("ProfileID") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Profile ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblProfileID" runat="server" Text='<%# Eval("ProfileID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeID" runat="server" Text='<%# Eval("EmployeeID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Emp Code">
                            <ItemTemplate>
                                <asp:Label ID="lblEmp_Code" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="System Number" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSystemNumber" runat="server" Text='<%# Eval("SystemNumber") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Assign Code" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAssignEmpCode" runat="server" Text='<%# Eval("AssignEmpCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Designation">
                            <ItemTemplate>
                                <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("DesignationText") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Month Days">
                            <ItemTemplate>
                                <asp:Label ID="lblMonthDays" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LWP" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblLWP" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Paid Days" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblPaidDays" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reimbursement For" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblReimbursementFor1" runat="server" Text='<%#Eval("ReimbursementFor1")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reimbursement Value" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblReimbursementValue1" runat="server" Text='<%#Eval("ReimbursementValue1")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LWP Effected Value" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblLWPEffectedValue1" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reimbursement For" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblReimbursementFor2" runat="server" Text='<%#Eval("ReimbursementFor2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reimbursement Value" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblReimbursementValue2" runat="server" Text='<%#Eval("ReimbursementValue2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LWP Effected Value" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblLWPEffectedValue2" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reimbursement For" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblReimbursementFor3" runat="server" Text='<%#Eval("ReimbursementFor3")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reimbursement Value" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblReimbursementValue3" runat="server" Text='<%#Eval("ReimbursementValue3")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LWP Effected Value" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblLWPEffectedValue3" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reimbursement For" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblReimbursementFor4" runat="server" Text='<%#Eval("ReimbursementFor4")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reimbursement Value" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblReimbursementValue4" runat="server" Text='<%#Eval("ReimbursementValue4")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LWP Effected Value" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblLWPEffectedValue4" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reimbursement For" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblReimbursementFor5" runat="server" Text='<%#Eval("ReimbursementFor5")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reimbursement Value" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblReimbursementValue5" runat="server" Text='<%#Eval("ReimbursementValue5")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LWP Effected Value" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblLWPEffectedValue5" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Total Reimbursement" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalReimbursement" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </asp:Panel>
            <div style="min-height: 420px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

