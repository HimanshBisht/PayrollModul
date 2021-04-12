<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="MidMonthIncrementSalaryMaking.aspx.cs" Inherits="SalaryModule_MidMonthIncrementSalaryMaking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
        function ConfirmPreviousBasicAndPaidDays() {
            if (confirm("Are you sure that Previous Basic Scale is Filled Correctly?") == true)
                return true;
            else
                return false;
        }

        function Confirm() {
            if (confirm("Are you sure you want to Save Final Calculated Salary?") == true)
                return true;
            else
                return false;
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 25px; height: auto;">
                <fieldset>
                    <center>
                        <legend>Mid Month Increment Salary Making</legend>
                        <asp:Panel ID="pnlData" runat="server">
                            <table style="margin: 5px 0 0 21px;">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="GetDetails"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="GetDetails"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlemployee" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlemployee" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="GetDetails"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGetDetails" runat="server" Text="Get Details" CssClass="btn btn-default" ValidationGroup="GetDetails" OnClick="btnGetDetails_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                                <td>
                                    <td colspan="4">
                                        <br />
                                        <br />
                                    </td>
                                </td>
                            </table>
                        </asp:Panel>
                    </center>
                    <center>
                        <asp:Panel ID="pnlCurrentBasic" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <h4>Current Basic</h4>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtCurrentBasic" runat="server" AutoComplete="off"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtCurrentBasic" ErrorMessage="*" ForeColor="Red" ValidationGroup="CalculateCurrentBasic"></asp:RequiredFieldValidator>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txtCurrentBasic" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <h4>Month Days</h4>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtCurrentBasicMonthDays" runat="server" placeholder="Current Basic Month Days" AutoComplete="off" AutoPostBack="true" OnTextChanged="txtCurrentBasicMonthDays_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" TargetControlID="txtCurrentBasicMonthDays" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789"></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCurrentBasicMonthDays" ErrorMessage="*" ForeColor="Red" ValidationGroup="CalculateCurrentBasic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h4>Effected From</h4>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtEffectedCurrentBasicFrom" runat="server" placeholder="Current Basic Effected From" AutoComplete="off" AutoPostBack="true" OnTextChanged="txtEffectedCurrentBasicFrom_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEffectedCurrentBasicFrom" ErrorMessage="*" ForeColor="Red" ValidationGroup="CalculateCurrentBasic"></asp:RequiredFieldValidator>
                                        <asp:CalendarExtender ID="CalEffectedCurrentBasicFrom" runat="server" TargetControlID="txtEffectedCurrentBasicFrom" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <h4>Paid Days</h4>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtCurrentBasicPaidDays" runat="server" placeholder="Current Basic Paid Days" AutoComplete="off" AutoPostBack="true" OnTextChanged="txtCurrentBasicPaidDays_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" TargetControlID="txtCurrentBasicPaidDays" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtCurrentBasicPaidDays" ErrorMessage="*" ForeColor="Red" ValidationGroup="CalculateCurrentBasic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnCalculateCurrentSalary" runat="server" Text="Calculate Current Salary" CssClass="btn btn-default" ValidationGroup="CalculateCurrentBasic" OnClick="btnCalculateCurrentSalary_Click" />
                                    </td>
                                    <td colspan="4"></td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
                <asp:Panel ID="pnlCalculateCurrentBasic" runat="server" Visible="false" Style="margin-top: 20px; max-height: 600px; width: 100%; overflow: auto; margin-bottom: 20px;">

                    <asp:GridView ID="grdCalculateCurrentBasic" runat="server" AutoGenerateColumns="false" CssClass="grdTable">
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#C8C6C6" Font-Bold="True" ForeColor="Black" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="#E2DED6" ForeColor="#284775" />
                        <RowStyle HorizontalAlign="Center" BackColor="#F7F6F3" ForeColor="#284775" />
                        <EmptyDataRowStyle ForeColor="Red" />
                        <Columns>
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
                            <asp:TemplateField HeaderText="NatureID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblNatureID" runat="server" Text='<%# Eval("NatureOfEmp") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="StaffTypeID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStaffTypeID" runat="server" Text='<%# Eval("StaffType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DesignationID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Designation">
                                <ItemTemplate>
                                    <asp:Label ID="lblDesignationText" runat="server" Text='<%# Eval("DesignationText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Basic">
                                <ItemTemplate>
                                    <asp:Label ID="lblBasicScale" runat="server" Text='<%# Eval("BasicScale") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Change" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblChangeScale" runat="server" Text='<%# Eval("ChangeScaleText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effected Scale" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectedScale" runat="server" Text='<%# Eval("EffectedScale") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Salary Mode" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSalaryMode" runat="server" Text='<%# Eval("ModeOfSalary") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PF Deduct" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPFDeduct" runat="server" Text='<%# Eval("PFDeduct") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ESI Deduct" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblESIDeduct" runat="server" Text='<%# Eval("ESIDeduct") %>'></asp:Label>
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
                            <asp:TemplateField HeaderText="Pay Drawn Basic" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPayDrawnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DA" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDAApply" runat="server" Text='<%# Eval("DA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DA On Basic" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDaOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HRA" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblHRAApply" runat="server" Text='<%# Eval("SelectHRA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HRA On Basic" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblHraOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Transport" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTransportOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Medical" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblMedicalOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Washing" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblWashingOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Revised Salary" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossRevisedsalary" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExGratia" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblExGratiaOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Arrear Adjust" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblArearAdjust" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Total" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossTotal" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DA For Report" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDaForReportOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PF" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPFOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DED" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeduction" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TDS" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTDS" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ADV" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvance" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TPT Rec" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTPTRECOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GIS" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblGISOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ESI" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblESIOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Deduction" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalDeduction" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Total Salary" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossTotalSalary" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effected Gross" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectedGrossTotalSalary" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Record Found
                        </EmptyDataTemplate>
                    </asp:GridView>
                </asp:Panel>
                <fieldset>
                    <center>
                        <asp:Panel ID="pnlPreviousBasic" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <h4>Previous Basic</h4>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtPreviousBasic" runat="server" AutoComplete="off" placeholder="Enter Previous Basic Scale" AutoPostBack="true" OnTextChanged="txtPreviousBasic_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPreviousBasic" ErrorMessage="*" ForeColor="Red" ValidationGroup="CalculatePreviousBasic"></asp:RequiredFieldValidator>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="txtPreviousBasic" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <h4>Paid Days</h4>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtPreviousBasicPaidDays" runat="server" placeholder="Previous Basic Paid Days" AutoComplete="off" AutoPostBack="true" OnTextChanged="txtPreviousBasicPaidDays_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtPreviousBasicPaidDays" ErrorMessage="*" ForeColor="Red" ValidationGroup="CalculatePreviousBasic"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCalculatePreviousSalary" runat="server" Text="Calculate Previous Salary" CssClass="btn btn-default" ValidationGroup="CalculatePreviousBasic" OnClientClick="return ConfirmPreviousBasicAndPaidDays()" OnClick="btnCalculatePreviousSalary_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
                <asp:Panel ID="pnlCalculatePreviousBasic" runat="server" Visible="false" Style="margin-top: 20px; max-height: 600px; width: 100%; overflow: auto; margin-bottom: 20px;">

                    <asp:GridView ID="grdCalculatePreviousBasic" runat="server" AutoGenerateColumns="false" CssClass="grdTable">
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#C8C6C6" Font-Bold="True" ForeColor="Black" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="#E2DED6" ForeColor="#284775" />
                        <RowStyle HorizontalAlign="Center" BackColor="#F7F6F3" ForeColor="#284775" />
                        <EmptyDataRowStyle ForeColor="Red" />
                        <Columns>
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
                            <asp:TemplateField HeaderText="NatureID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblNatureID" runat="server" Text='<%# Eval("NatureOfEmp") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="StaffTypeID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStaffTypeID" runat="server" Text='<%# Eval("StaffType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DesignationID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Designation">
                                <ItemTemplate>
                                    <asp:Label ID="lblDesignationText" runat="server" Text='<%# Eval("DesignationText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Basic">
                                <ItemTemplate>
                                    <asp:Label ID="lblBasicScale" runat="server" Text='<%# Eval("BasicScale") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Change" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblChangeScale" runat="server" Text='<%# Eval("ChangeScaleText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effected Scale" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectedScale" runat="server" Text='<%# Eval("EffectedScale") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Salary Mode" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSalaryMode" runat="server" Text='<%# Eval("ModeOfSalary") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PF Deduct" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPFDeduct" runat="server" Text='<%# Eval("PFDeduct") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ESI Deduct" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblESIDeduct" runat="server" Text='<%# Eval("ESIDeduct") %>'></asp:Label>
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
                            <asp:TemplateField HeaderText="Pay Drawn Basic" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPayDrawnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DA" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDAApply" runat="server" Text='<%# Eval("DA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DA On Basic" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDaOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HRA" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblHRAApply" runat="server" Text='<%# Eval("SelectHRA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HRA On Basic" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblHraOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Transport" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTransportOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Medical" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblMedicalOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Washing" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblWashingOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Revised Salary" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossRevisedsalary" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExGratia" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblExGratiaOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Arrear Adjust" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblArearAdjust" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Total" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossTotal" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DA For Report" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDaForReportOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PF" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPFOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DED" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeduction" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TDS" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTDS" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ADV" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvance" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TPT Rec" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTPTRECOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GIS" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblGISOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ESI" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblESIOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Deduction" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalDeduction" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Total Salary" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossTotalSalary" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effected Gross" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectedGrossTotalSalary" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Record Found
                        </EmptyDataTemplate>
                    </asp:GridView>
                </asp:Panel>
                <fieldset>
                    <center>
                        <asp:Panel ID="pnlFinalSalary" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="bthCalculateFinalSalary" runat="server" Text="Calculate Final Salary" CssClass="btn btn-default" OnClick="bthCalculateFinalSalary_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>

                <asp:Panel ID="pnlCalculateFinalSalary" runat="server" Visible="false" Style="margin-top: 20px; max-height: 600px; width: 100%; overflow: auto; margin-bottom: 30px;">

                    <asp:GridView ID="grdCalculateFinalSalary" runat="server" AutoGenerateColumns="false" CssClass="grdTable">
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#C8C6C6" Font-Bold="True" ForeColor="Black" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="#E2DED6" ForeColor="#284775" />
                        <RowStyle HorizontalAlign="Center" BackColor="#F7F6F3" ForeColor="#284775" />
                        <EmptyDataRowStyle ForeColor="Red" />
                        <Columns>
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
                            <asp:TemplateField HeaderText="NatureID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblNatureID" runat="server" Text='<%# Eval("NatureOfEmp") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="StaffTypeID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStaffTypeID" runat="server" Text='<%# Eval("StaffType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DesignationID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Designation">
                                <ItemTemplate>
                                    <asp:Label ID="lblDesignationText" runat="server" Text='<%# Eval("DesignationText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Basic">
                                <ItemTemplate>
                                    <asp:Label ID="lblBasicScale" runat="server" Text="0.00"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Change" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblChangeScale" runat="server" Text='<%# Eval("ChangeScaleText") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effected Scale" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectedScale" runat="server" Text='<%# Eval("EffectedScale") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Salary Mode" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSalaryMode" runat="server" Text='<%# Eval("ModeOfSalary") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PF Deduct" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPFDeduct" runat="server" Text='<%# Eval("PFDeduct") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ESI Deduct" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblESIDeduct" runat="server" Text='<%# Eval("ESIDeduct") %>'></asp:Label>
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
                            <asp:TemplateField HeaderText="Pay Drawn Basic" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPayDrawnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DA" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDAApply" runat="server" Text='<%# Eval("DA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DA On Basic" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDaOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HRA" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblHRAApply" runat="server" Text='<%# Eval("SelectHRA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HRA On Basic" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblHraOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Transport" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTransportOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Medical" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblMedicalOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Washing" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblWashingOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Revised Salary" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossRevisedsalary" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ExGratia" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblExGratiaOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Arrear Adjust" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblArearAdjust" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Total" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossTotal" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DA For Report" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDaForReportOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PF" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPFOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DED" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDeduction" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TDS" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTDS" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ADV" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvance" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TPT Rec" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTPTRECOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GIS" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblGISOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ESI" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblESIOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Deduction" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalDeduction" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gross Total Salary" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblGrossTotalSalary" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Record Found
                        </EmptyDataTemplate>
                    </asp:GridView>
                </asp:Panel>
                <center>
                    <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSaveFinalSalary" runat="server" Text="Save Final Salary" CssClass="btn btn-default" OnClientClick="return Confirm()" OnClick="btnSaveFinalSalary_Click" />
                                </td>
                                <td style="width: 5px;"></td>
                                <td>
                                    <asp:Button ID="btnReset" Text="Reset All" runat="server" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
            </div>
            <style type="text/css">
                .grdTable {
                    max-width: 250% !Important;
                    min-width: 250%;
                    overflow: auto;
                }
            </style>
            <div style="min-height: 380px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

