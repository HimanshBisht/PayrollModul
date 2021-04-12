<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="EmpWiseSalaryReport.aspx.cs" Inherits="SalaryModule_EmpWiseSalaryReport" EnableEventValidation="false" %>

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 25px; height: auto; width: 100%; overflow: auto;">
                <fieldset>
                    <center>
                        <table style="margin: 30px 0 0 0;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReport" runat="server" Font-Bold="true" Font-Size="15px" Text="Employee Wise Salary Report"></asp:Label>
                                </td>
                                <td style="width: 600px;"></td>
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
                                        <asp:DropDownList ID="ddlFromMonth" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlFromMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFromYear" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlFromYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlToMonth" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlToMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlToYear" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlToYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlReportType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                            <asp:ListItem Text="Select Report Type" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="All Employees" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Individual Employee" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlReportType" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                    <asp:Panel ID="pnlSelectEmp" runat="server" Visible="false">
                                        <td>
                                            <asp:DropDownList ID="ddlEmployeeStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEmployeeStatus_SelectedIndexChanged">
                                                <asp:ListItem Text="Select Employee Status" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="All Active Employees" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="All Deactive Employees" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlEmployeeStatus" InitialValue="2" ErrorMessage="*" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlemployee" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlemployee" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                        </td>
                                    </asp:Panel>
                                    <td>
                                        <asp:Button ID="btnSearch" runat="server" Text="Get Report" CssClass="btn btn-default" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                                        <td>
                                            <asp:Button ID="btnPrintSelected" runat="server" CssClass="btn btn-default" Text="Print OR Save Sheet" OnClick="btnPrintSelected_Click" />
                                            <asp:LinkButton ID="lnkExportToExcel" runat="server" OnClick="lnkExportToExcel_Click">
                                                <asp:Image ID="ImgExportToExcel" runat="server" Height="35px" Width="35px" ToolTip="Export To Excel" ImageUrl="~/images/ExportToExcel.png" />
                                            </asp:LinkButton>
                                        </td>
                                    </asp:Panel>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                </fieldset>
            </div>
            <style type="text/css">
                .grdTable {
                    max-width: 100% !Important;
                    min-width: 100%;
                    overflow: auto;
                }

                .grdIndividualTable {
                    max-width: 200% !Important;
                    min-width: 200%;
                    overflow: auto;
                }
            </style>

            <center>
                <asp:Panel ID="pnlStmt" runat="server" Style="margin-top: 30px;">
                    <asp:Label ID="lblSTMT" runat="server" Font-Size="Medium" Font-Bold="true"></asp:Label>
                </asp:Panel>
            </center>

            <asp:Panel ID="pnlDetail" runat="server" Visible="false" Style="margin-top: 30px; max-height: 550px; width: 100%; overflow: auto; margin-bottom: 20px;">
                <asp:GridView ID="grdrecord" runat="server" CssClass="grdTable" OnRowDataBound="grdrecord_RowDataBound">
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
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="pnlIndividualEmpSalary" runat="server" Visible="false" Style="margin-top: 30px; max-height: 550px; width: 100%; overflow: auto; margin-bottom: 20px;">
                <asp:GridView ID="grdIndividualEmpSalary" runat="server" AutoGenerateColumns="false" CssClass="grdIndividualTable" ShowFooter="true" OnRowDataBound="grdIndividualEmpSalary_RowDataBound">
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
                        <asp:BoundField HeaderText="Emp Code" DataField="Emp_Code" />
                        <asp:BoundField HeaderText="Name" DataField="Name" />
                        <asp:BoundField HeaderText="Des" DataField="Designation" />
                        <asp:BoundField HeaderText="Month-Year" DataField="MY" />
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

                        <asp:TemplateField HeaderText="DA on Basic">
                            <ItemTemplate>
                                <asp:Label ID="lblDA" runat="server" Text='<%# Eval("DA") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblGrandTotalDA" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>

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
                        No Record Found
                    </EmptyDataTemplate>

                </asp:GridView>
            </asp:Panel>
            <div style="min-height: 300px;"></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

