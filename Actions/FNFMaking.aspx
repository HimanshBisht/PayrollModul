<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="FNFMaking.aspx.cs" Inherits="SalaryModule_FNFMaking" %>

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
                        <legend>Full N Final (FNF) Making</legend>
                        <asp:Panel ID="pnlData" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Get"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Get"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlemployee" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlemployee" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Get"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate" CssClass="btn btn-default" OnClientClick="return ConfirmDeactivate()" ValidationGroup="Get" Visible="false" OnClick="btnDeactivate_Click" />
                                        <asp:Button ID="btnGetDetails" runat="server" Text="Get Details" CssClass="btn btn-default" ValidationGroup="Get" OnClick="btnGetDetails_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" CssClass="btn btn-primary" runat="server" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
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
                                            <td><b>Recovery Days</b></td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblRecoveryDays" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </center>
                            </center>
                        </asp:Panel>

                        <asp:Panel ID="pnlGrid" runat="server" Visible="false" CssClass="btn btn-default" GroupingText="Hold Payable Salary" Style="margin-top: 30px; max-height: 600px; width: 100%; overflow: auto; margin-bottom: 20px;">

                            <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" CssClass="grdTable" ShowFooter="true" OnRowDataBound="grdrecord_RowDataBound">
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
                                    <%--<asp:BoundField HeaderText="Employee ID" DataField="EmployeeID" />--%>
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
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalText" runat="server" Text="Grand Total"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Pay Drawn Basic">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPayDrawnBasic" runat="server" Text='<%# Eval("PayDrawnBasic") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalPayDrawnBasic" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <%-- <asp:TemplateField HeaderText="DA(%)">
                            <ItemTemplate>
                                <asp:Label ID="lblDAApply" runat="server" Text='<%# Eval("DAApply") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="DA on Basic">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDA" runat="server" Text='<%# Eval("DA") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalDA" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <%-- <asp:TemplateField HeaderText="HRA">
                            <ItemTemplate>
                                <asp:Label ID="lblHRAApply" runat="server" Text='<%# Eval("HRAApply") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="HRA on Basic">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHRA" runat="server" Text='<%# Eval("HRA") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalHRA" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Transport">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTransport" runat="server" Text='<%# Eval("Transport") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalTransport" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Medical">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMedical" runat="server" Text='<%# Eval("Medical") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalMedical" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Washing" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWashing" runat="server" Text='<%# Eval("Washing") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalWashing" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Gross Revised Salary">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrossRevisedSalary" runat="server" Text='<%# Eval("GrossRevisedSalary") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalGrossRevisedSalary" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Ex-Gratia">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExGratia" runat="server" Text='<%# Eval("ExGratia") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalExGratia" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Arrear Adjust">
                                        <ItemTemplate>
                                            <asp:Label ID="lblArearAdjust" runat="server" Text='<%# Eval("ArearAdjust") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalArearAdjust" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Gross Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrossTotal" runat="server" Text='<%# Eval("GrossTotal") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalGrossTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="PF">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPF" runat="server" Text='<%# Eval("PF") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalPF" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Ded.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeduction" runat="server" Text='<%# Eval("Deduction") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalDeduction" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="TDS">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTDS" runat="server" Text='<%# Eval("TDS") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalTDS" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ADV">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdvance" runat="server" Text='<%# Eval("Advance") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalAdvance" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="TPT REC">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTPTREC" runat="server" Text='<%# Eval("TPTREC") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalTPTREC" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="GIS" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGIS" runat="server" Text='<%# Eval("GIS") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalGIS" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="ESI">
                                        <ItemTemplate>
                                            <asp:Label ID="lblESI" runat="server" Text='<%# Eval("ESI") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalESI" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Gross Deduction">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalDeduction" runat="server" Text='<%# Eval("TotalDeduction") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalGrossDeduction" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Gross Total Salary">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrossTotalSalary" runat="server" Text='<%# Eval("GrossTotalSalary") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotalGrossTotalSalary" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <EmptyDataTemplate>
                                    No Record Found For Hold Payable Salary.
                                </EmptyDataTemplate>

                            </asp:GridView>
                        </asp:Panel>

                        <asp:Panel ID="pnlCalculateRecoveryButton" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnCalculateRecovery" runat="server" CssClass="btn btn-default" OnClick="btnCalculateRecovery_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <asp:Panel ID="pnlCalculateRecovery" runat="server" Visible="false" Style="margin-top: 10px;" Width="100%" CssClass="btn btn-default" GroupingText="Recovery Due to Incomplete Notice Period Days">

                            <asp:GridView ID="grdCalculateRecovery" runat="server" AutoGenerateColumns="false" CssClass="grdTable">
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
                                    <asp:TemplateField HeaderText="Emp Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmp_Code" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
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
                                    <asp:TemplateField HeaderText="Month Days">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonthDays" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recovery Days" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFinalRecoveryDays" runat="server"></asp:Label>
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
                                    <asp:TemplateField HeaderText="Gross Revised Recovery" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrossRevisedRecovery" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ExGratia" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExGratiaOnBasic" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gross Total" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrossTotal" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DED" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDeduction" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Gross Total Recovery" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGrossTotalRecovery" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Record Found
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>

                        <asp:Panel ID="pnlCalculateFNFButton" runat="server" Visible="false" Style="margin-top: 20px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnCalculateFNF" runat="server" Text="Calculate FNF" CssClass="btn btn-default" OnClick="btnCalculateFNF_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                        <asp:Panel ID="pnlCalculateFNF" runat="server" CssClass="btn btn-default" GroupingText="Final FNF Structure" Visible="false">
                            <center>
                                <table>
                                    <tr>
                                        <td><b>Grand Hold Total</b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblGrandTotal" runat="server"></asp:Label></td>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td><b>Recovery Days</b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblFNFRecoveryDays" runat="server"></asp:Label></td>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td><b>Recovery Amount</b></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblRecoveryAmount" runat="server"></asp:Label></td>
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
                                            <asp:TextBox ID="txtFNFRemarks" runat="server" AutoComplete="off" TextMode="MultiLine" Rows="3" Width="97%" placeholder="Please Mention Remarks For any Addition or Deduction."></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNetFNFText" runat="server" Font-Bold="true"></asp:Label></td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblNetFNFPayable" runat="server"></asp:Label></td>
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
                                                <asp:Button ID="btnSaveFNF" runat="server" Text="Save FNF" CssClass="btn btn-default" OnClick="btnSaveFNF_Click" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="btnCancelFNF" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancelFNF_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </center>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <div style="min-height: 400px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

