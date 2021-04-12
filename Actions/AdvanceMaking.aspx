<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="AdvanceMaking.aspx.cs" Inherits="SalaryModule_AdvanceMaking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/jquery.min.js"></script>
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
    <script type="text/javascript">
        function ConfirmUpdate() {
            if (confirm("Are you sure you want to Update this Record?") == true) {
                return true;
            }
            else {
                return false;
            }
        }

        function ConfirmDeactivate() {
            if (confirm("Make Sure that all Pending Advance Amount will be '0.00' for this Employee. Are you sure you want to Deactivate this Record?") == true) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 25px; height: auto;">
                <fieldset>
                    <center>
                        <legend>Advance Making</legend>
                        <asp:Panel ID="pnlSelectEmployee" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFill" runat="server" Font-Bold="true" Text="To Fill Advance"></asp:Label>
                                    </td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlemployee" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlemployee_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlPending" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPendingAmount" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="18px"></asp:Label>
                                    </td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:Button ID="btnAddAdvanceAmount" runat="server" CssClass="btn btn-default" OnClick="btnAddAdvanceAmount_Click" />
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlFillData" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAddAmount" runat="server" Text="Add Amount"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtAddAmount" runat="server" placeholder="Amount For Advance" AutoComplete="off"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" Enabled="True" TargetControlID="txtAddAmount" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtAddAmount" ErrorMessage="*" ForeColor="Red" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                    </td>
                                    <asp:Panel ID="pnlAddAmount" runat="server" Visible="false">
                                        <td>
                                            <asp:Button ID="btnAddAmount" runat="server" Text="ADD" CssClass="btn btn-default" OnClick="btnAddAmount_Click" ValidationGroup="Add" />
                                        </td>
                                    </asp:Panel>
                                    <td style="width: 20px;"></td>
                                    <asp:Panel ID="pnlTotal" runat="server" Visible="false">
                                        <td>
                                            <asp:Label ID="lblTotal" runat="server" Text="Net Amount" Font-Bold="true" Font-Size="15px"></asp:Label>
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblTotalAmount" runat="server" Font-Bold="true" Font-Size="15px"></asp:Label>
                                        </td>
                                    </asp:Panel>
                                </tr>
                                <asp:Panel ID="pnlType" runat="server" Visible="false">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Choose Type"></asp:Label>
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlAmtRec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAmtRec_SelectedIndexChanged">
                                                <asp:ListItem Text="Select Recovery Type" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Monthly EMI" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Monthly Fixed Amount" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ControlToValidate="ddlAmtRec" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="GetStructure"></asp:RequiredFieldValidator>
                                        </td>
                                        <asp:Panel ID="pnlEMI" runat="server" Visible="false">
                                            <td>
                                                <asp:TextBox ID="txtNoOfEMI" runat="server" placeholder="Enter No of EMI" AutoComplete="off"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtNoOfEMI" FilterType="Custom"
                                                    FilterMode="ValidChars" ValidChars="0123456789">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNoOfEMI" ErrorMessage="*" ForeColor="Red" ValidationGroup="GetStructure"></asp:RequiredFieldValidator>
                                            </td>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlMonthlyAmt" runat="server" Visible="false">
                                            <td>
                                                <asp:TextBox ID="txtFixedAmt" runat="server" placeholder="Define Monthly Fixed Amount" AutoComplete="off"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txtFixedAmt" FilterType="Custom"
                                                    FilterMode="ValidChars" ValidChars="0123456789">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFixedAmt" ErrorMessage="*" ForeColor="Red" ValidationGroup="GetStructure"></asp:RequiredFieldValidator>
                                            </td>
                                        </asp:Panel>
                                        <td></td>
                                    </tr>
                                </asp:Panel>
                                <asp:Panel ID="pnlStructure" runat="server" Visible="false">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Effected From"></asp:Label>
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="GetStructure"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="GetStructure"></asp:RequiredFieldValidator>
                                        </td>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnShowStructure" runat="server" Text="Show Monthly Structure" CssClass="btn btn-default" ValidationGroup="GetStructure" OnClick="btnShowStructure_Click" />
                                            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                        </td>
                                        <td colspan="5"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <br />
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </asp:Panel>

                        <asp:Panel ID="pnlDetail" runat="server" Visible="false">
                            <asp:Label ID="lblHeaderText" runat="server" Font-Bold="true" Font-Size="16px"></asp:Label>
                            <table style="float: left;">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSaveAdvance" runat="server" Text="Save Advance" CssClass="btn btn-default" OnClick="btnSaveAdvance_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="grdrecord" runat="server" Style="margin-top: 20px;" AutoGenerateColumns="false" Width="100%" ShowFooter="true" OnRowDataBound="grdrecord_RowDataBound">
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#C8C6C6" Font-Bold="True" ForeColor="Black" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="#E2DED6" ForeColor="#284775" />
                                <RowStyle HorizontalAlign="Center" BackColor="#F7F6F3" ForeColor="#284775" />
                                <EmptyDataRowStyle ForeColor="Red" />
                                <FooterStyle Font-Bold="true" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNo" runat="server" Text='<%# Eval("SNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Month">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("Month") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Year">
                                        <ItemTemplate>
                                            <asp:Label ID="lblYear" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblNetAmountText" runat="server" Text="Net Amount"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Monthly Recovery Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonthlyAmount" runat="server" Text='<%# Eval("MonthlyAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblNetAdvanceAmount" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>

                        <asp:Panel ID="pnlUpdate" runat="server" Visible="false">
                            <table style="float: left;">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAddRow" runat="server" Text="Add New Row" CssClass="btn btn-default" OnClick="btnAddRow_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnUpdateStructure" runat="server" Text="Update Advance" CssClass="btn btn-default" OnClick="btnUpdateStructure_Click" OnClientClick="return ConfirmUpdate()" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate Record" CssClass="btn btn-default" OnClick="btnDeactivate_Click" OnClientClick="return ConfirmDeactivate()" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="grdUpdate" runat="server" Style="margin-top: 20px;" AutoGenerateColumns="false" Width="100%" ShowFooter="true" OnRowDataBound="grdUpdate_RowDataBound">
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#C8C6C6" Font-Bold="True" ForeColor="Black" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="#E2DED6" ForeColor="#284775" />
                                <RowStyle HorizontalAlign="Center" BackColor="#F7F6F3" ForeColor="#284775" />
                                <EmptyDataRowStyle ForeColor="Red" />
                                <FooterStyle Font-Bold="true" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Advance Import ID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdvanceImportID" runat="server" Text='<%# Eval("AdvanceImportID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Profile ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProfileID" runat="server" Text='<%# Eval("ProfileID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="EmployeeID" Visible="false">
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
                                    <asp:TemplateField HeaderText="Designation" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Month ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonthID" runat="server" Text='<%# Eval("MonthID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Month">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("Month") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Year ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblYearID" runat="server" Text='<%# Eval("YearID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recovery Type" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdvanceRecoveryType" runat="server" Text='<%# Eval("AdvanceRecoveryType") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Year">
                                        <ItemTemplate>
                                            <asp:Label ID="lblYear" runat="server" Text='<%# Eval("Year") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblNetAmountText" runat="server" Text="Net Amount"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Monthly Recovery Amount">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMonthlyAmount" runat="server" AutoPostBack="true" OnTextChanged="txtMonthlyAmount_TextChanged" placeholder="Enter Monthly Recovery" Text='<%# Eval("AdvanceValue") %>'></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtMonthlyAmount" FilterType="Custom"
                                                FilterMode="ValidChars" ValidChars="0123456789.">
                                            </asp:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblNetAdvanceAmount" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <div style="min-height: 400px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

