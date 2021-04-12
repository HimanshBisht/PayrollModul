<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="ManageNoticePeriod.aspx.cs" Inherits="SalaryModule_ManageNoticePeriod" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script language="javascript" type="text/javascript">
        function ConfirmDeactivate() {
            if (confirm("Are you sure you want to Deactivate this Record?") == true)
                return true;
            else
                return false;
        }

        function CheckLengthRemarks(e) {
            var textRemarks = document.getElementById("ContentPlaceHolder1_txtRemarks");
            if (textRemarks != null) {
                if (textRemarks.value.trim().length >= 1000) {
                    textRemarks.value = textRemarks.value.substring(0, 1000);
                    CheckRemarksCharacter();
                    return false;
                }
            }
        }

        function CheckRemarksCharacter() {
            var textRemarks = document.getElementById("ContentPlaceHolder1_txtRemarks").value;
            $("#divRemarks").css("display", "block");
            var Maxsize = 1000;
            $("#divRemarks").text(Maxsize - textRemarks.trim().length + " " + "Characters Left.");
            if (textRemarks == 0) {
                $("#divRemarks").css("display", "none");
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
            <div style="margin-top: 25px; height: auto; width: 100%; overflow: auto;">
                <fieldset>
                    <center>
                        <legend>Notice Period Payment / Recovery</legend>
                        <asp:Panel ID="pnlData" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <h4>To Manage Notice Period</h4>
                                    </td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlemployee" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlemployee" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Get"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <h4>Select Type</h4>
                                    </td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlType" runat="server">
                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Notice Period Payment" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Notice Period Recovery" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlType" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Get"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate" CssClass="btn btn-default" OnClientClick="return ConfirmDeactivate()" ValidationGroup="Get" Visible="false" OnClick="btnDeactivate_Click" />
                                        <asp:Button ID="btnGetDetails" runat="server" Text="Get Details" CssClass="btn btn-default" ValidationGroup="Get" OnClick="btnGetDetails_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" CssClass="btn btn-primary" runat="server" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlDetails" runat="server" CssClass="btn btn-default" GroupingText="Employee Profile Summary View" Visible="false">
                            <center>
                                <table>
                                    <tr>
                                        <td><b>Name</b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblEmpName" runat="server"></asp:Label></td>
                                        <td style="width: 80px;"></td>
                                        <td><b>Emp Code</b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblEmpCode" runat="server"></asp:Label></td>
                                        <td style="width: 80px;"></td>
                                        <td><b>DOJ</b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblDOJ" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><b>Designation</b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblDesignation" runat="server"></asp:Label></td>
                                        <td style="width: 80px;"></td>
                                        <td><b>Staff Type</b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblStaffType" runat="server"></asp:Label></td>
                                        <td style="width: 80px;"></td>
                                        <td><b>Nature</b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblNature" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td><b>Resign Date</b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblResignDate" runat="server"></asp:Label></td>
                                        <td style="width: 80px;"></td>
                                        <td><b>Actual LWD</b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblActualLWD" runat="server"></asp:Label></td>
                                        <td style="width: 80px;"></td>
                                        <td><b>As Per Norms LWD<b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="txtAsPerNormsLWD" runat="server" Width="50%" AutoComplete="off" placeholder="Offically LWD" Onkeydown="return false;" OnPaste="return false;" AutoPostBack="true" OnTextChanged="txtAsPerNormsLWD_TextChanged"></asp:TextBox></td>
                                        <asp:CalendarExtender ID="CalDOB" runat="server" TargetControlID="txtAsPerNormsLWD" Format="dd MMM yyyy"></asp:CalendarExtender>
                                    </tr>
                                </table>
                                <center>
                                    <table>
                                        <tr>
                                            <td colspan="7">
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>Total Working</b></td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblTotalWorking" runat="server"></asp:Label></td>
                                            <td style="width: 50px;"></td>
                                            <td><b>Notice Period Days</b></td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblNoticePeriodDays" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </center>
                            </center>
                        </asp:Panel>
                        <asp:Panel ID="pnlCalculateSalaryButton" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnCalculateSalary" runat="server" CssClass="btn btn-default" Text="Calculate Salary" OnClick="btnCalculateSalary_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <style type="text/css">
                .grdTable {
                    max-width: 170% !Important;
                    min-width: 170%;
                    overflow: auto;
                }
            </style>
            <center>
                <table>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCalculatedText" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlSalaryChanges" runat="server" Style="margin-top: 10px; max-height: 550px; width: 100%; overflow: auto; margin-bottom: 20px;" Visible="false">
                    <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" CssClass="grdTable" CaptionAlign="Top">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" Height="50px" />
                        <RowStyle HorizontalAlign="Center" Height="50px" />
                        <EmptyDataRowStyle ForeColor="Red" />
                        <FooterStyle Font-Bold="true" HorizontalAlign="Center" Height="50px" />
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
                            <asp:TemplateField HeaderText="Des">
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
                            <asp:TemplateField HeaderText="Standard Month Days">
                                <ItemTemplate>
                                    <asp:Label ID="lblMonthDays" runat="server" Text="30"></asp:Label>
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
                            <asp:TemplateField HeaderText="DA" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDAApply" runat="server" Text='<%# Eval("DA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DA On Basic" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblDaOnBasic" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="HRA" Visible="false">
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
                            <asp:TemplateField HeaderText="Gross Deduction" Visible="true">
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
                <asp:Panel ID="pnlNoticePeriodAdjustment" runat="server" CssClass="btn btn-default" GroupingText="Final Notice Period Adjustment" Visible="false">
                    <center>
                        <table>
                            <tr>
                                <td><b>Notice Amount</b></td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblNoticeAmount" runat="server"></asp:Label></td>
                                <td colspan="3"></td>
                            </tr>
                            <tr>
                                <td><b>Any Other Adjustment</b></td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtOtherAdjustment" runat="server" Text="0" AutoComplete="off" placeholder="Default Value is 0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtOtherAdjustment" ErrorMessage="*" ForeColor="Red" ValidationGroup="Adjust"></asp:RequiredFieldValidator>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" Enabled="True" TargetControlID="txtOtherAdjustment" FilterType="Custom"
                                        FilterMode="ValidChars" ValidChars="0123456789.">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnAdd" runat="server" Text="ADD" CssClass="btn btn-default" OnClick="btnAdd_Click" ValidationGroup="Adjust" />
                                </td>
                                <td>
                                    <asp:Button ID="btnDeduct" runat="server" Text="Deduct" CssClass="btn btn-default" OnClick="btnDeduct_Click" ValidationGroup="Adjust" />
                                </td>
                                <td>
                                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td><b>Remarks</b></td>
                                <td>:</td>
                                <td colspan="6">
                                    <asp:TextBox ID="txtRemarks" runat="server" AutoComplete="off" TextMode="MultiLine" Rows="3" Width="97%" placeholder="Remarks if Any. (Max 1000 Characters)" onkeyup="return CheckRemarksCharacter();"
                                        onkeypress="return CheckLengthRemarks();" onchange="return CheckLengthRemarks();" OnPaste="return CheckLengthRemarks();">
                                    </asp:TextBox>
                                    <div id="divRemarks" style="color: Red;">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNetNoticePeriodAdjustmentText" runat="server" Font-Bold="true" Text="Final Notice Amount"></asp:Label></td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblNetNoticePeriodAdjustmentAmount" runat="server"></asp:Label></td>
                                <td colspan="3"></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-default" OnClick="btnSave_Click" />
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnCancelNoticePeriodAdjustment" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancelNoticePeriodAdjustment_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </center>
                </asp:Panel>
            </center>
            <div style="min-height: 350px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

