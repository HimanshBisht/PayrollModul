<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="TDSForm16AllEmp.aspx.cs" Inherits="SalaryModule_TDSForm16" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                    <table style="margin: 20px 0 0 0;">
                        <tr>
                            <td>
                                <asp:Label ID="lblReport" runat="server" Font-Bold="true" Font-Size="15px" Text="TDS Form 16 Multiple Employees"></asp:Label>
                            </td>
                            <asp:Panel ID="pnlTotalRecords" runat="server" Visible="false">
                                <td>
                                    <asp:Label ID="lblTotal" runat="server" Style="margin-left: 650px;" Font-Bold="true" ForeColor="Red" Font-Size="15px"
                                        Text="Total Employees : "></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblTotalEmployees" runat="server" Font-Bold="true" ForeColor="Red"
                                        Font-Size="15px"></asp:Label>
                                </td>
                            </asp:Panel>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <br />
                            </td>
                        </tr>
                    </table>
                    <center>
                        <asp:Panel ID="pnlSelect" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlFromFinancialYear" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlFromFinancialYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="GetForm"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEmployeeStatus" runat="server">
                                            <asp:ListItem Text="Select Employee Status" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="All Active Employees" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="All Deactive Employees" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlEmployeeStatus" InitialValue="2" ErrorMessage="*" ForeColor="Red" ValidationGroup="GetForm"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNatureOfEmp" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <asp:Button ID="btnGetReport" runat="server" Text="Get Employee's" CssClass="btn btn-default" ValidationGroup="GetForm" OnClick="btnGetReport_Click" />
                                        <asp:Button ID="btnReset" Text="Reset" runat="server" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <style type="text/css">
                .grdTable {
                    max-width: 100% !important;
                    min-width: 100%;
                    overflow: auto;
                }
            </style>

            <center>
                <asp:Panel ID="pnlStmt" runat="server" Visible="false">
                    <asp:Label ID="lblSTMT" runat="server" Font-Size="Medium" Font-Bold="true"></asp:Label>
                </asp:Panel>
            </center>
            <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnDownloadPDF" runat="server" Text="Print / Save All Selected" OnClientClick="return validate()"
                                OnClick="btnDownloadPDF_Click" CssClass="btn btn-default" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlDetail" runat="server" Visible="false" Style="margin-top: 20px; max-height: 500px; width: 100%; overflow: auto; margin-bottom: 20px;">
                <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" CssClass="grdTable"
                    AllowPaging="true" PageSize="500" OnPageIndexChanging="grdrecord_PageIndexChanging">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" />
                    <RowStyle HorizontalAlign="Center" />
                    <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                    <FooterStyle Font-Bold="true" HorizontalAlign="Center" />
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
                        <asp:TemplateField HeaderText="Emp Code">
                            <ItemTemplate>
                                <asp:Label ID="lblEmp_Code" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Name" DataField="Name" />
                        <asp:BoundField HeaderText="Designation" DataField="DesignationText" />
                        <asp:BoundField HeaderText="Nature" DataField="NatureOfEmpText" />
                        <asp:BoundField HeaderText="Appointment" DataField="AppointmentText" />
                        <asp:BoundField HeaderText="Staff Type" DataField="StaffTypeText" />
                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" Visible="false">
                <asp:Panel ID="pnlPrint" runat="server">
                    <asp:Repeater ID="rptForm16Data" runat="server" OnItemDataBound="rptForm16Data_ItemDataBound">
                        <ItemTemplate>
                            <center>
                                <asp:Panel ID="pnlData" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSchoolName" runat="server" Font-Bold="true" Font-Underline="true" Font-Size="15px" Text='<%# Eval("SchoolName") %>'></asp:Label>
                                            </td>
                                            <td style="width: 200px;"></td>
                                            <td>
                                                <asp:Label ID="lblTaxText" runat="server" Font-Bold="true" Font-Size="15px" Font-Underline="true" Text='<%# Eval("TaxText") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <b style="font-size: 14px;">Emp_Code</b>
                                            </td>
                                            <td style="width: 10px;">:</td>
                                            <td>
                                                <asp:Label ID="lblProfileID" runat="server" Font-Size="15px" Visible="false" Text='<%# Eval("ProfileID") %>'></asp:Label>
                                                <asp:Label ID="lblEmpCode" runat="server" Font-Size="15px" Text='<%# Eval("EmpCode") %>'></asp:Label>
                                            </td>
                                            <td style="width: 50px;"></td>
                                            <td>
                                                <b style="font-size: 14px;">Name</b>
                                            </td>
                                            <td style="width: 10px;">:</td>
                                            <td>
                                                <asp:Label ID="lblName" runat="server" Font-Size="15px" Text='<%# Eval("Name") %>'></asp:Label>
                                            </td>
                                            <td style="width: 50px;"></td>
                                            <td>
                                                <b style="font-size: 14px;">Designation</b>
                                            </td>
                                            <td style="width: 10px;">:</td>
                                            <td>
                                                <asp:Label ID="lblDesignation" runat="server" Font-Size="15px" Text='<%# Eval("Designation") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="11">
                                                <div style="width: 10px;"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b style="font-size: 14px;">PAN No</b>
                                            </td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblPanNo" runat="server" Font-Size="15px" Text='<%# Eval("PanCardNo") %>'></asp:Label>
                                            </td>
                                            <td style="width: 50px;"></td>
                                            <td>
                                                <b style="font-size: 14px;">DOB</b>
                                            </td>
                                            <td style="width: 10px;">:</td>
                                            <td>
                                                <asp:Label ID="lblDOB" runat="server" Font-Size="15px" Text='<%# Eval("DOB") %>'></asp:Label>
                                            </td>
                                            <td style="width: 50px;"></td>
                                            <td>
                                                <asp:Label ID="lblAgeText" runat="server" Font-Bold="true" Font-Size="15px" Text='<%# Eval("AgeText") %>'></asp:Label>
                                            </td>
                                            <td style="width: 10px;">:</td>
                                            <td>
                                                <asp:Label ID="lblEmpAge" runat="server" Font-Size="15px" Text='<%# Eval("Age") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </center>
                            <asp:Panel ID="pnlDetail" runat="server" Style="margin-top: 15px; margin-bottom: 20px;">
                                <asp:GridView ID="grdTDSForm16grid" runat="server" ShowFooter="true" AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdTDSForm16grid_RowDataBound">
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
                                        <asp:TemplateField HeaderText="Type" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
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
                                                <asp:Label ID="lblGrandTotalText" runat="server" Text="Grand Total"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Basic">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPayDrawnBasic" runat="server" Text='<%# Eval("Basic") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotalPayDrawnBasic" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DA">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDA" runat="server" Text='<%# Eval("DA") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotalDA" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="HRA">
                                            <ItemTemplate>
                                                <asp:Label ID="lblHRA" runat="server" Text='<%# Eval("HRA") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotalHRA" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TPT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransport" runat="server" Text='<%# Eval("Transport") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotalTransport" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="MED">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMedical" runat="server" Text='<%# Eval("Medical") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotalMedical" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Was" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWashing" runat="server" Text='<%# Eval("Washing") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotalWashing" runat="server"></asp:Label>
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
                                        <asp:TemplateField HeaderText="Arrear">
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
                                        <asp:TemplateField HeaderText="DED">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeduction" runat="server" Text='<%# Eval("Deduction") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotalDeduction" runat="server"></asp:Label>
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
                                        <asp:TemplateField HeaderText="TDS">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTDS" runat="server" Text='<%# Eval("TDS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotalTDS" runat="server"></asp:Label>
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
                                        <asp:TemplateField HeaderText="ESI">
                                            <ItemTemplate>
                                                <asp:Label ID="lblESI" runat="server" Text='<%# Eval("ESI") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotalESI" runat="server"></asp:Label>
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
                                        <asp:TemplateField HeaderText="Net Salary">
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
                            <asp:Panel ID="pnlCalculateGrossTaxable" runat="server" Width="100%" Style="margin-top: 15px; margin-bottom: 20px;">
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblGrossTaxableSalary" runat="server" Text="Gross Taxable Salary" Font-Bold="true" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblGrossTaxableSalaryValue" runat="server" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td style="width: 20px;"></td>
                                            <td>
                                                <asp:Label ID="lblHRAExemption" runat="server" Text="HRA Exemption" Font-Bold="true" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblHRAExemptionValue" runat="server" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td style="width: 20px;"></td>
                                            <td>
                                                <asp:Label ID="lblStandardDeduction" runat="server" Text="Standard Deduction" Font-Bold="true" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblStandardDeductionValue" runat="server" Font-Size="15px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="11">
                                                <div style="height: 10px;"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMedicalPolicyPremium" runat="server" Text="Medical Policy Premium [u/s 80D]" Font-Bold="true" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblMedicalPolicyPremiumValue" runat="server" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td style="width: 20px;"></td>
                                            <td>
                                                <asp:Label ID="lblIntersetOnHousingLoan" runat="server" Text="Interest On Housing Loan" Font-Bold="true" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblIntersetOnHousingLoanValue" runat="server" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td style="width: 20px;"></td>
                                            <td>
                                                <asp:Label ID="lblEducationLoan" runat="server" Text="Education Loan [u/s 80E]" Font-Bold="true" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblEducationLoanValue" runat="server" Font-Size="15px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="11">
                                                <div style="height: 10px;"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBasicDeductions" runat="server" Text="Basic Deductions [u/s 80C]" Font-Bold="true" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblBasicDeductionsValue" runat="server" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td style="width: 20px;"></td>
                                            <td>
                                                <asp:Label ID="lblNationalPentionScheme" runat="server" Text="National Pension Scheme [u/s 80CCD]" Font-Bold="true" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td>:</td>
                                            <td>
                                                <asp:Label ID="lblNationalPentionSchemeValue" runat="server" Font-Size="15px"></asp:Label>
                                            </td>
                                            <td style="width: 20px;"></td>
                                            <td colspan="3"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="11">
                                                <hr />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <b style="font-size: 17px;">Final Taxable Amount</b>
                                            </td>
                                            <td style="width: 10px;"></td>
                                            <td>:</td>
                                            <td style="width: 10px;"></td>
                                            <td>
                                                <asp:Label ID="lblFinalTaxableAmount" runat="server" Font-Size="20px"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td colspan="17">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b style="font-size: 17px;">Total Tax</b>
                                            </td>
                                            <td style="width: 10px;"></td>
                                            <td>:</td>
                                            <td style="width: 10px;"></td>
                                            <td>
                                                <asp:Label ID="lblTotalTax" runat="server" Font-Size="20px"></asp:Label>
                                            </td>
                                            <td style="width: 50px;"></td>
                                            <td>
                                                <b style="font-size: 17px;">Already Paid Tax</b>
                                            </td>
                                            <td style="width: 10px;"></td>
                                            <td>:</td>
                                            <td style="width: 10px;"></td>
                                            <td>
                                                <asp:Label ID="lblAlreadyPaidTax" runat="server" Font-Size="20px"></asp:Label>
                                            </td>
                                            <td style="width: 50px;"></td>
                                            <td>
                                                <b style="font-size: 17px;">Balance Tax</b>
                                            </td>
                                            <td style="width: 10px;"></td>
                                            <td>:</td>
                                            <td style="width: 10px;"></td>
                                            <td>
                                                <asp:Label ID="lblBalanceTax" runat="server" Font-Size="20px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="17">
                                                <hr />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFinalText" runat="server" Font-Bold="true" Font-Size="20px"></asp:Label>
                                            </td>
                                            <td style="width: 10px;"></td>
                                            <td>:</td>
                                            <td style="width: 10px;"></td>
                                            <td>
                                                <asp:Label ID="lblFinalTaxAmountForMonth" runat="server" Font-Size="20px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRoundedFinalTaxAmountForMonthText" runat="server" Font-Bold="true" Font-Size="20px"></asp:Label>
                                            </td>
                                            <td style="width: 10px;"></td>
                                            <td>:</td>
                                            <td style="width: 10px;"></td>
                                            <td>
                                                <asp:Label ID="lblRoundedFinalTaxAmountForMonth" runat="server" Font-Size="20px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <hr />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </asp:Panel>
                            <p style="page-break-after: always;"></p>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </asp:Panel>
            <div style="min-height: 300px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

