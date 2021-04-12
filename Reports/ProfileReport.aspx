<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master"
    AutoEventWireup="true" CodeFile="ProfileReport.aspx.cs" Inherits="SalaryModule_ProfileReport" %>

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
                    </span></h4>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: auto; width: 100%; overflow: auto;">
                <fieldset>
                    <center>
                        <table style="margin: 30px 0 0 0;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReport" runat="server" Font-Bold="true" Font-Size="15px" Text="Employee Profile Report"></asp:Label>
                                </td>
                                <td style="width: 700px;"></td>
                                <asp:Panel ID="pnlTotalRecords" runat="server" Visible="false">
                                    <td>
                                        <asp:Label ID="lblTotal" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="15px"
                                            Text="Total Employees : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalEmployees" runat="server" Font-Bold="true" ForeColor="Red"
                                            Font-Size="15px"></asp:Label>
                                    </td>
                                </asp:Panel>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlData" runat="server">
                            <table>
                                <tr>
                                    <td colspan="5">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlNatureOfEmp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNatureOfEmp_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAppointment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAppointment_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStaffType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStaffType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDesignation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSubjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlPFDeduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPFDeduct_SelectedIndexChanged">
                                            <asp:ListItem Text="All PF/Non-PF Employees" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="All PF Employees" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="All Non PF Employees" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlESIDeduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlESIDeduct_SelectedIndexChanged">
                                            <asp:ListItem Text="All ESI/Non-ESI Employees" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="All ESI Employees" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="All Non ESI Employees" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlModeOfSalary" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlModeOfSalary_SelectedIndexChanged">
                                            <asp:ListItem Text="All Salary Modes" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Cheque" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Bank Account" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Cash" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBusUser" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBusUser_SelectedIndexChanged">
                                            <asp:ListItem Text="All Bus User Types" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Incharge" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlIsActive" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIsActive_SelectedIndexChanged">
                                            <asp:ListItem Text="Select Status" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="All Status Employees" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Active Employees" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Deactive Employees" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlIsActive" ErrorMessage="*" ForeColor="Red" InitialValue="3" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlemployee" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSearch" runat="server" Text="Get Report" CssClass="btn btn-default" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" CssClass="btn btn-primary" runat="server" OnClick="btnCancel_Click" />
                                        <asp:LinkButton ID="lnkExportToExcel" runat="server" Visible="false" OnClick="lnkExportToExcel_Click">
                                            <asp:Image ID="ImgExportToExcel" runat="server" Height="35px" Width="35px" ToolTip="Export To Excel"
                                                ImageUrl="~/images/ExportToExcel.png" />
                                        </asp:LinkButton>
                                    </td>
                                    <td colspan="3"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <style type="text/css">
                .grdTable {
                    max-width: 900% !important;
                    min-width: 900%;
                    overflow: auto;
                }
            </style>

            <center>
                <asp:Panel ID="pnlStmt" runat="server" Visible="false" Style="margin-top: 30px;">
                    <asp:Label ID="lblSTMT" runat="server" Font-Size="Medium" Font-Bold="true"></asp:Label>
                </asp:Panel>
            </center>
            <asp:Panel ID="pnlDetail" runat="server" Visible="false" Style="margin-top: 30px; max-height: 500px; width: 100%; overflow: auto; margin-bottom: 20px;">
                <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" CssClass="grdTable"
                    AllowPaging="true" PageSize="300" OnPageIndexChanging="grdrecord_PageIndexChanging">
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
                        <%--<asp:BoundField HeaderText="Profile ID" DataField="ProfileID" />--%>
                        <asp:BoundField HeaderText="Emp Code" DataField="Emp_Code" />
                        <asp:BoundField HeaderText="Name" DataField="Name" />            
                        <asp:BoundField HeaderText="Father/Husband" DataField="Father/Husband" />
                        <asp:BoundField HeaderText="Email" DataField="EmailID" />
                        <asp:BoundField HeaderText="Mobile" DataField="MobileNo" />
                        <asp:BoundField HeaderText="Gender" DataField="GenderText" />
                        <asp:BoundField HeaderText="DOB" DataField="DOB" />
                        <asp:BoundField HeaderText="DOJ" DataField="DOJ" />
                        <asp:BoundField HeaderText="Designation" DataField="DesignationText" />
                        <%--<asp:BoundField HeaderText="Emp ID" DataField="EmployeeID" />--%>
                        <%--<asp:BoundField HeaderText="SystemNumber" DataField="System Number" />--%>
                        <asp:BoundField HeaderText="Nature" DataField="NatureOfEmpText" />
                        <asp:BoundField HeaderText="Appointment" DataField="AppointmentText" />
                        <asp:BoundField HeaderText="Staff Type" DataField="StaffTypeText" />
                        <asp:BoundField HeaderText="Subject For" DataField="SubjectName" />
                        <asp:BoundField HeaderText="PF No." DataField="PFNo" />
                        <asp:BoundField HeaderText="UAN No." DataField="UANNo" />
                        <asp:BoundField HeaderText="ESI No." DataField="ESINo" />
                        <asp:BoundField HeaderText="PAN No." DataField="PanCardNo" />
                        <asp:BoundField HeaderText="From Date" DataField="FromDate" />
                        <asp:BoundField HeaderText="To Date" DataField="ToDate" />
                        <asp:BoundField HeaderText="Contract Dt" DataField="ContractDate" />
                        <asp:BoundField HeaderText="Probation Dt" DataField="ProbationDate" />
                        <asp:BoundField HeaderText="Confirm Dt" DataField="ConfirmationDate" />
                        <asp:BoundField HeaderText="Resign Dt" DataField="ResignationDate" />
                        <asp:BoundField HeaderText="Last Working Dt" DataField="LWD" />
                        <asp:BoundField HeaderText="Address" DataField="Address" />
                        <asp:BoundField HeaderText="Salary Mode" DataField="ModeOfSalaryText" />
                        <asp:BoundField HeaderText="Type" DataField="BankTypeText" />
                        <asp:BoundField HeaderText="Bank Name" DataField="BankName" />
                        <asp:BoundField HeaderText="Account No" DataField="AccountNo" />
                        <asp:BoundField HeaderText="IFSC Code" DataField="IFSCCode" />
                        <asp:BoundField HeaderText="Maternity Leave" DataField="MaternityLeaveText" />
                        <asp:BoundField HeaderText="From Maternity" DataField="FromMaternity" />
                        <asp:BoundField HeaderText="To Maternity" DataField="ToMaternity" />
                        <asp:BoundField HeaderText="Grade Pay" DataField="GradePay" />
                        <asp:BoundField HeaderText="Basic" DataField="BasicScale" />
                        <asp:BoundField HeaderText="Bus User" DataField="BusUserText" />
                        <asp:BoundField HeaderText="Gov Acc" DataField="GovAccText" />
                        <asp:BoundField HeaderText="DA Allow" DataField="DaAllowText" />
                        <asp:BoundField HeaderText="DA(%)" DataField="DA" />
                        <asp:BoundField HeaderText="Total DA" DataField="DaValue" />
                        <asp:BoundField HeaderText="HRA Allow" DataField="HraAllowText" />
                        <asp:BoundField HeaderText="HRA(%)" DataField="HRA" />
                        <asp:BoundField HeaderText="HRA Value" DataField="HRAValue" />
                        <asp:BoundField HeaderText="Transport Allow" DataField="TransportAllowText" />
                        <asp:BoundField HeaderText="Transport" DataField="TransportValue" />
                        <asp:BoundField HeaderText="Medical Allow" DataField="MedicalAllowText" />
                        <asp:BoundField HeaderText="Medical" DataField="MedicalValue" />
                        <%--<asp:BoundField HeaderText="Washing Allow" DataField="WashingAllowText" />
                        <asp:BoundField HeaderText="Washing" DataField="WashingValue" />--%>
                        <asp:BoundField HeaderText="Ex-Gratia Allow" DataField="ExGratiaAllowText" />
                        <asp:BoundField HeaderText="Ex Gratia" DataField="ExGratiaValue" />
                        <asp:BoundField HeaderText="PF Deduct" DataField="PFDeductText" />
                        <asp:BoundField HeaderText="PF" DataField="PFValue" />
                        <asp:BoundField HeaderText="ESI Deduct" DataField="ESIDeductText" />
                        <asp:BoundField HeaderText="ESI" DataField="ESIValue" />
                        <%-- <asp:BoundField HeaderText="GIS Deduct" DataField="GISDeductText" />
                        <asp:BoundField HeaderText="GIS" DataField="GISValue" />--%>
                        <asp:BoundField HeaderText="TPT REC" DataField="TransportRecovery" />
                        <asp:BoundField HeaderText="Gross Allowance" DataField="GrossAllowance" />
                        <asp:BoundField HeaderText="Gross Total" DataField="GrossTotal" />
                        <asp:BoundField HeaderText="Gross Deduction" DataField="GrossDeduction" />
                        <asp:BoundField HeaderText="Net Salary" DataField="GrossTotalSalary" />
                        <asp:BoundField HeaderText="Reimbursement" DataField="ReimbursementText" />
                        <asp:BoundField HeaderText="Reimbursement For 1" DataField="ReimbursementFor1" />
                        <asp:BoundField HeaderText="Reimbursement Value 1" DataField="ReimbursementValue1" />
                        <asp:BoundField HeaderText="Reimbursement For 2" DataField="ReimbursementFor2" />
                        <asp:BoundField HeaderText="Reimbursement Value 2" DataField="ReimbursementValue2" />
                        <asp:BoundField HeaderText="Reimbursement For 3" DataField="ReimbursementFor3" />
                        <asp:BoundField HeaderText="Reimbursement Value 3" DataField="ReimbursementValue3" />
                        <asp:BoundField HeaderText="Reimbursement For 4" DataField="ReimbursementFor4" />
                        <asp:BoundField HeaderText="Reimbursement Value 4" DataField="ReimbursementValue4" />
                        <asp:BoundField HeaderText="Reimbursement For 5" DataField="ReimbursementFor5" />
                        <asp:BoundField HeaderText="Reimbursement Value 5" DataField="ReimbursementValue5" />
                        <asp:BoundField HeaderText="Total Reimbursement" DataField="TotalReimbursement" />
                        <asp:BoundField HeaderText="Created By" DataField="CreatedBy" />
                        <asp:BoundField HeaderText="Created On" DataField="CreatedOn" />
                        <asp:BoundField HeaderText="Updated By" DataField="UpdatedBy" />
                        <asp:BoundField HeaderText="Updated On" DataField="UpdatedOn" />
                        <asp:BoundField HeaderText="Salary Status" DataField="SalaryStatusText" />
                        <asp:BoundField HeaderText="Profile Status" DataField="ProfileStatus" />
                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </asp:Panel>
            <div style="min-height: 330px;">
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
