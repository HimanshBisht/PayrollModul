<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="CalculateTax.aspx.cs" Inherits="SalaryModule_CalculateTax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script language="javascript" type="text/javascript">
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

        function validateSave() {
            var Count = 0;
            var gridView = document.getElementById("<%=grdrecord.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    Count = Count + 1;
                }
            }
            if (Count > 0) {
                if (confirm("It will save only those records who's salary is not prepared for the selected month and year.") == true)
                    return true;
                else
                    return false;
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
                        </h4>
                    </span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 25px;">
                <fieldset>
                    <center>
                        <legend>Calculate Tax</legend>
                        <p style="color: red">Note : Select From Financial Year as <b>2019,</b> If you want to calculate tax for <b>"2019-2020"</b> Financial year.</p>
                    </center>
                    <asp:Panel ID="pnlSelect" runat="server">
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlFromFinancialYear" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlFromFinancialYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="CalculateTaxableIncome"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMonthForSaveTax" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlMonthForSaveTax" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="CalculateTaxableIncome"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlYearForSaveTax" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlYearForSaveTax" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="CalculateTaxableIncome"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCalculateTaxableIncome" runat="server" Text="Calculate Taxable Income" CssClass="btn btn-default" ValidationGroup="CalculateTaxableIncome" OnClick="btnCalculateTaxableIncome_Click" />
                                        <asp:Button ID="btnReset" Text="Reset" runat="server" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>

                    <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnCalculateTax" runat="server" Text="Calculate Tax" CssClass="btn btn-default" OnClientClick="return validate()" OnClick="btnCalculateTax_Click" />
                                </td>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSaveTax" runat="server" Text="Save Tax" CssClass="btn btn-default" Visible="false" OnClientClick="return validateSave()" OnClick="btnSaveTax_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <style type="text/css">
                        .grdTable {
                            max-width: 200% !Important;
                            min-width: 200%;
                            overflow: auto;
                        }
                    </style>
                    <asp:Panel ID="pnlDetail" runat="server" Visible="false" Style="margin-top: 15px; max-height: 600px; width: 100%; overflow: auto; margin-bottom: 20px;">
                        <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="grdTable">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" Height="40px" />
                            <RowStyle HorizontalAlign="Center" />
                            <EmptyDataRowStyle ForeColor="Red" />
                            <FooterStyle Font-Bold="true" HorizontalAlign="Center" Height="50px" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <input id="chkAllItems" type="checkbox" onclick="CheckAllDataGridCheckBoxes('SelectChk', document.forms[0].chkAllItems.checked)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="SelectChk" runat="server" />
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
                                <asp:TemplateField HeaderText="Emp Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Designation" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("DesignationText") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DOB">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("DOB") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblAgeText" runat="server"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAge" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gross Per Annum">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrossPerAnnum" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Standard Deduction">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStandardDeduction" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HRA Exemption">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHRAExemption" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Other IDF Adjustments">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOtherIDFAdjustment" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Taxable Income">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxableIncome" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Tax">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalTax" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Already Paid Tax">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAlreadyPaidTax" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Balance Tax">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBalanceTax" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actual Tax For This Month">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActualTaxForThisMonth" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Final (Rounded To 1000) Tax">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFinalRoundedTax" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </fieldset>
            </div>
            <div style="min-height: 370px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

