<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="CalculateRecovery.aspx.cs" Inherits="SalaryModule_CalculateRecovery" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script language="javascript" type="text/javascript">
        function ConfirmHRA() {
            if (confirm("If HRA is applicable for the Employee then Make Sure that 'HRA with Percentage (%)' should be written with '%' sign in HRA Column. Otherwise, the system will be treated as 'HRA with Fixed Value.'") == true)
                return true;
            else
                return false;
        }
        function ConfirmSave() {
            if (confirm("Are you sure you want to save the record ?") == true)
                return true;
            else
                return false;
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
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script type="text/javascript">
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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 25px; height: auto; width: 100%; overflow: auto;">
                <fieldset>
                    <center>
                        <table style="margin: 30px 0 0 0;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReport" runat="server" Font-Bold="true" Font-Size="15px" Text="Calculate Recovery"></asp:Label>
                                </td>
                                <td style="width: 700px;"></td>
                                <asp:Panel ID="pnlTotalRecords" runat="server" Visible="false">
                                    <td>
                                        <asp:Label ID="lblTotal" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="15px"
                                            Text="Total Records : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" ForeColor="Red"
                                            Font-Size="15px"></asp:Label>
                                    </td>
                                </asp:Panel>
                            </tr>
                        </table>
                    </center>
                    <asp:Panel ID="pnlData" runat="server">
                        <center>
                            <table style="margin: 15px 0 0 21px;">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                    <asp:Panel ID="pnlEmployees" runat="server" Visible="false">
                                        <td>
                                            <asp:DropDownList ID="ddlemployee" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlemployee" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                        </td>
                                    </asp:Panel>
                                    <td>
                                        <asp:Button ID="btnSearch" runat="server" Text="Get Salary Report" CssClass="btn btn-default" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                </fieldset>
            </div>
            <style type="text/css">
                .grdTable {
                    max-width: 200% !Important;
                    min-width: 200%;
                    overflow: auto;
                }
            </style>
            <asp:Panel ID="pnlPrint" runat="server">
                <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                    <center>
                        <asp:Panel ID="pnlStmt" runat="server" Style="margin-top: 30px; margin-bottom: 20px;">
                            <asp:Label ID="lblActualSalaryText" runat="server" Font-Size="18px"></asp:Label>
                        </asp:Panel>
                    </center>
                    <asp:Panel ID="pnlActualSalary" runat="server" Style="margin-top: 10px; max-height: 550px; width: 100%; overflow: auto; margin-bottom: 20px;">
                        <asp:GridView ID="grdActualSalary" runat="server" AutoGenerateColumns="false" CssClass="grdTable">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" Height="50px" />
                            <RowStyle HorizontalAlign="Center" Height="50px" />
                            <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                            <FooterStyle Font-Bold="true" HorizontalAlign="Center" Height="50px" />

                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Salary ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSalaryID" runat="server" Text='<%# Eval("SalaryID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Emp Code" DataField="Emp_Code" />
                                <asp:BoundField HeaderText="Name" DataField="Name" />
                                <asp:BoundField HeaderText="Des" DataField="Designation" />
                                <asp:BoundField HeaderText="Basic" DataField="Basic" />
                                <asp:BoundField HeaderText="Month Days" DataField="MonthDays" />
                                <asp:BoundField HeaderText="LWP" DataField="LWP" />
                                <asp:TemplateField HeaderText="Paid Days">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaidDays" runat="server" Text='<%# Eval("PaidDays") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pay Drawn Basic">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPayDrawnBasic" runat="server" Text='<%# Eval("PayDrawnBasic") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DA on Basic">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDA" runat="server" Text='<%# Eval("DA") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HRA on Basic">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHRA" runat="server" Text='<%# Eval("HRA") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Transport">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransport" runat="server" Text='<%# Eval("Transport") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Medical">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMedical" runat="server" Text='<%# Eval("Medical") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Washing" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWashing" runat="server" Text='<%# Eval("Washing") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gross Revised Salary">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrossRevisedSalary" runat="server" Text='<%# Eval("GrossRevisedSalary") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ex-Gratia">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExGratia" runat="server" Text='<%# Eval("ExGratia") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Arrear Adjust">
                                    <ItemTemplate>
                                        <asp:Label ID="lblArearAdjust" runat="server" Text='<%# Eval("ArearAdjust") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gross Total">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrossTotal" runat="server" Text='<%# Eval("GrossTotal") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PF">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPF" runat="server" Text='<%# Eval("PF") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ded.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeduction" runat="server" Text='<%# Eval("Deduction") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TDS">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTDS" runat="server" Text='<%# Eval("TDS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ADV">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAdvance" runat="server" Text='<%# Eval("Advance") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TPT REC">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTPTREC" runat="server" Text='<%# Eval("TPTREC") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="GIS" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGIS" runat="server" Text='<%# Eval("GIS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ESI">
                                    <ItemTemplate>
                                        <asp:Label ID="lblESI" runat="server" Text='<%# Eval("ESI") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gross Deduction">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalDeduction" runat="server" Text='<%# Eval("TotalDeduction") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Gross Total Salary">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrossTotalSalary" runat="server" Text='<%# Eval("GrossTotalSalary") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCalculatedText" runat="server" Font-Size="18px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <asp:Panel ID="pnlSalaryChanges" runat="server" Style="margin-top: 10px; max-height: 550px; width: 100%; overflow: auto; margin-bottom: 20px;">
                        <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" CssClass="grdTable">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" Height="40px" />
                            <RowStyle HorizontalAlign="Center" />
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
                                <asp:TemplateField HeaderText="Designation" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignationText" runat="server" Text='<%# Eval("DesignationText") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Basic">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblBasicScale" runat="server" Style="width: 70px; text-align: center;" Text='<%# Eval("BasicScale") %>'></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderBasic" runat="server" Enabled="True" TargetControlID="lblBasicScale" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
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
                                        <asp:TextBox ID="lblPaidDays" runat="server" Style="width: 70px; text-align: center;" AutoPostBack="true" OnTextChanged="lblPaidDays_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderPaidDays" runat="server" Enabled="True" TargetControlID="lblPaidDays" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pay Drawn Basic" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPayDrawnBasic" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DA" Visible="true">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblDAApply" runat="server" Style="width: 70px; text-align: center;" Text='<%# Eval("DA") %>'></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderDA" runat="server" Enabled="True" TargetControlID="lblDAApply" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DA On Basic" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDaOnBasic" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HRA" Visible="true">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblHRAApply" runat="server" Style="width: 70px; text-align: center;" Text='<%# Eval("SelectHRA") %>'></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderHRA" runat="server" Enabled="True" TargetControlID="lblHRAApply" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.%">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="HRA On Basic" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHraOnBasic" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Transport" Visible="true">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblTransportOnBasic" runat="server" Style="width: 70px; text-align: center;" Text='<%# Eval("TransportValue") %>'></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderTransport" runat="server" Enabled="True" TargetControlID="lblTransportOnBasic" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Medical" Visible="true">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblMedicalOnBasic" runat="server" Style="width: 70px; text-align: center;" Text='<%# Eval("MedicalValue") %>'></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderMedical" runat="server" Enabled="True" TargetControlID="lblMedicalOnBasic" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
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
                                        <asp:TextBox ID="lblExGratiaOnBasic" runat="server" Style="width: 70px; text-align: center;" Text='<%# Eval("ExGratiaValue") %>'></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderExGratia" runat="server" Enabled="True" TargetControlID="lblExGratiaOnBasic" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
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
                                <asp:TemplateField HeaderText="Total Deduction" Visible="false">
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
                    <center>
                        <asp:Panel ID="pnlButtons" runat="server" Style="margin-top: 30px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnCalculateRecovery" runat="server" CssClass="btn btn-default" Text="Calculate Recovery" OnClientClick="return ConfirmHRA()" OnClick="btnCalculateRecovery_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </asp:Panel>
                <asp:Panel ID="pnlRecovery" runat="server" Visible="false">
                    <center>
                        <legend>Final Details For Recovery</legend>
                    </center>
                    <asp:Panel ID="pnlRecoveryAmount" runat="server" Style="margin-top: 30px; max-height: 550px; width: 100%; overflow: auto; margin-bottom: 20px;">
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Font-Size="18px" Text="Recovery Gross Total Salary"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Font-Size="18px" Text=" : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRecoveryGrossTotalSalary" runat="server" Font-Size="18px" Text="0.00"></asp:Label>
                                    </td>
                                    <td style="width: 70px;"></td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Font-Size="18px" Text="Actual Gross Total Salary"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Font-Size="18px" Text=" : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblActualGrossTotalSalary" runat="server" Font-Size="18px" Text="0.00"></asp:Label>
                                    </td>
                                    <td style="width: 70px;"></td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Font-Size="18px" Text="Total Recovery Amount"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Font-Size="18px" Text=" : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalRecoveryAmount" runat="server" Font-Size="18px" Text="0.00"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Font-Size="18px" Text="Any Other Recovery / Deduction"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" Font-Size="18px" Text=" : "></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtOtherAmt" runat="server" placeholder="0.00" AutoPostBack="true" OnTextChanged="txtOtherAmt_TextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderExGratia" runat="server" Enabled="True" TargetControlID="txtOtherAmt" FilterType="Custom"
                                                FilterMode="ValidChars" ValidChars="0123456789.">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <center>
                                <table>
                                    <tr>
                                        <td colspan="7">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label11" runat="server" Font-Size="18px" Text="Pre Recovery Amount"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Font-Size="18px" Text=" : "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblPreRecoveryAmount" runat="server" Font-Size="18px" Text="0.00"></asp:Label>
                                        </td>
                                        <td style="width: 70px;"></td>

                                        <td>
                                            <asp:Label ID="Label3" runat="server" Font-Size="18px" ForeColor="Red" Text="Final Recovery Amount"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Font-Size="18px" Text=" : "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFinalRecoveryAmount" runat="server" Font-Size="18px" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                            <hr />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </center>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlSaveSection" runat="server" Visible="false">
                <center>
                    <legend>Select Month and Year in which you want to save the Recovery Details.</legend>
                    <table>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlRecoveryMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRecoveryMonth_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlRecoveryMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlRecoveryYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRecoveryYear_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlRecoveryMonth" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                        </tr>
                    </table>
                </center>
                <table style="margin-left: 100px;">
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" Font-Size="18px" Text="Remarks"></asp:Label></td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Font-Size="18px" Text=" : "></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="500%" runat="server" TextMode="MultiLine" Rows="3" AutoComplete="off"
                                placeholder="Remarks if Any. (Max 1000 Characters)" onkeyup="return CheckRemarksCharacter();"
                                onkeypress="return CheckLengthRemarks();" onchange="return CheckLengthRemarks();" OnPaste="return CheckLengthRemarks();">
                            </asp:TextBox>
                            <div id="divRemarks" style="color: Red;">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                        </td>
                    </tr>
                </table>
                <center>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnPrintSelected" runat="server" CssClass="btn btn-default" ValidationGroup="Save" Text="Print OR Save Sheet" OnClick="btnPrintSelected_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnSaveRecovery" runat="server" CssClass="btn btn-default" Text="Save Recovery" ValidationGroup="Save" OnClientClick="return ConfirmSave()" OnClick="btnSaveRecovery_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnClear_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <div style="min-height: 350px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

