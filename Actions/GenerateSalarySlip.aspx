<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="GenerateSalarySlip.aspx.cs" Inherits="SalaryModule_GenerateSalarySlip" %>

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
            <div style="margin-top: 30px; height: auto;">
                <fieldset>
                    <center>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblGenerateSalarySlip" runat="server" Font-Bold="true" Font-Size="15px" Text="Generate Salary Slip"></asp:Label>
                                </td>
                                <td style="width: 500px;"></td>
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
                            <tr>
                                <td colspan="4">
                                    <hr />
                                </td>
                            </tr>
                        </table>
                    </center>
                    <asp:Panel ID="pnlData" runat="server">
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNatureOfEmp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNatureOfEmp_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlNatureOfEmp" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAppointment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAppointment_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlStaffType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStaffType_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <asp:Panel ID="pnlEmployees" runat="server" Visible="false">
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlemployee" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlemployee_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </asp:Panel>
                                    <td>
                                        <asp:Button ID="btnGetRecords" runat="server" Text="Get Records" ValidationGroup="Search" CssClass="btn btn-default" OnClick="btnGetRecords_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                </fieldset>
            </div>
            <br />

            <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnPrintSelected" runat="server" CssClass="btn btn-default" Text="Print / Save All Selected" OnClientClick="return validate()" OnClick="btnPrintSelected_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnlDetail" runat="server" Style="margin-top: 15px; max-height: 650px; width: 100%; overflow: auto; margin-bottom: 20px;">
                <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" Width="100%" AllowPaging="true" PageSize="100"
                    OnPageIndexChanging="grdrecord_PageIndexChanging">
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
                        <asp:BoundField HeaderText="Month" DataField="MonthName" />
                        <asp:BoundField HeaderText="Year" DataField="Year" />
                        <asp:BoundField HeaderText="Emp Code" DataField="Emp_Code" />
                        <asp:BoundField HeaderText="Emp Name" DataField="Name" />
                        <asp:BoundField HeaderText="Designation" DataField="Designation" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <asp:Panel ID="pnlPrintSalarySlip" runat="server" Visible="false">
                <center>
                    <asp:Repeater ID="rptSalarySlip" runat="server">
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td colspan="7">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <center>
                                            <asp:Label ID="lblSTMT" runat="server" Font-Size="20px" Font-Bold="true" Text='<%# "K.R.Mangalam World School,Faridabad " + "<br />" + "<u>PAY-SLIP FOR THE MONTH OF " + Eval("MonthName") + "-" + Eval("Year") + "</u>" %>'></asp:Label>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Emp Code</b>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblEmp_Code" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                    </td>
                                    <td style="width: 100px;"></td>
                                    <td><b>Employee Name</b>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblEmpName" runat="server" Font-Bold="true" Font-Size="15px" Font-Italic="true" Text='<%# Eval("Name") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Designation</b>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                    </td>
                                    <td style="width: 100px;"></td>
                                    <td><b>Account No.</b>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblAccountNo" runat="server" Text='<%# Eval("AccountNo") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>PF No.</b>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblPFNo" runat="server" Text='<%# Eval("PFNo") %>'></asp:Label>
                                    </td>
                                    <td style="width: 100px;"></td>
                                    <td><b>PAN No.</b>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblPANNo" runat="server" Text='<%# Eval("PanCardNo") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Leave Without Pay</b>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblLWP" runat="server" Text='<%# Eval("LWP") %>'></asp:Label>
                                    </td>
                                    <td style="width: 100px;"></td>
                                    <td><b>Paid Days</b>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblPaidDays" runat="server" Text='<%# Eval("PaidDays") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <b>
                                            <br />
                                            <br />
                                        </b>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblEarning" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="15px" Text="Earnings"></asp:Label>
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <b>Rs.</b>
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <asp:Label ID="lblDeductions" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="15px" Text="Deductions"></asp:Label>
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <b>Rs.</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <b>
                                            <hr style="height: 1px; border: none; color: #333; background-color: #333;" />
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblBasicPayText" runat="server" Font-Bold="true" ForeColor="Green" Text="Basic Pay"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblBasicPay" runat="server" Font-Bold="true" Text='<%# Eval("PayDrawnBasic") %>'></asp:Label>
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <asp:Label ID="lblIncomeTax" runat="server" Font-Bold="true" ForeColor="Red" Text="Income Tax"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblTDS" runat="server" Font-Bold="true" Text='<%# Eval("TDS") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDearnesAllowance" runat="server" Font-Bold="true" ForeColor="Green" Text="Dearness Allowance"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblDA" runat="server" Font-Bold="true" Text='<%# Eval("DA") %>'></asp:Label>
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <asp:Label ID="lblEPFText" runat="server" Font-Bold="true" ForeColor="Red" Text="E.P.F."></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblEPF" runat="server" Font-Bold="true" Text='<%# Eval("PF") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHRAText" runat="server" Font-Bold="true" ForeColor="Green" Text="H.R.A."></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblHRA" runat="server" Font-Bold="true" Text='<%# Eval("HRA") %>'></asp:Label>
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <asp:Label ID="lblAdvanceAdjustment" runat="server" Font-Bold="true" ForeColor="Red" Text="Advance Adjustment"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblADV" runat="server" Font-Bold="true" Text='<%# Eval("Advance") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTransportAllowance" runat="server" Font-Bold="true" ForeColor="Green" Text="Transport Allowance"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblTransport" runat="server" Font-Bold="true" Text='<%# Eval("Transport") %>'></asp:Label>
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <asp:Label ID="lblTPTRecovery" runat="server" Font-Bold="true" ForeColor="Red" Text="TPT Recovery"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblTPTRec" runat="server" Font-Bold="true" Text='<%# Eval("TPTREC") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMedicalAllowance" runat="server" Font-Bold="true" ForeColor="Green" Text="Medical Allowance"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblMedical" runat="server" Font-Bold="true" Text='<%# Eval("Medical") %>'></asp:Label>
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <asp:Label ID="lblOtherDeductions" runat="server" Font-Bold="true" ForeColor="Red" Text="Other Deductions"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblOtherDed" runat="server" Font-Bold="true" Text='<%# Eval("Deduction") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td>
                                        <asp:Label ID="lblWashingAllowance" runat="server" Font-Bold="true" ForeColor="Green" Text="Washing Allowance"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblWashing" runat="server" Font-Bold="true" Text='<%# Eval("Washing") %>'></asp:Label>
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <asp:Label ID="lblGISDeductions" runat="server" Font-Bold="true" ForeColor="Red" Text="G.I.S"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblGIS" runat="server" Font-Bold="true" Text='<%# Eval("GIS") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblArrearAdjustmentText" runat="server" Font-Bold="true" ForeColor="Green" Text="Arrear Adjustment"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblArrearAdjust" runat="server" Font-Bold="true" Text='<%# Eval("ArearAdjust") %>'></asp:Label>
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <asp:Label ID="lblESIContribution" runat="server" Font-Bold="true" ForeColor="Red" Text="ESI Contribution"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblESI" runat="server" Font-Bold="true" Text='<%# Eval("ESI") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblExGratiaAmount" runat="server" Font-Bold="true" ForeColor="Green" Text="Ex-Gratia Amount"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblExGratia" runat="server" Font-Bold="true" Text='<%# Eval("ExGratia") %>'></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblGrossPayText" runat="server" Font-Bold="true" Font-Size="15px" Text="GROSS PAY"></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <hr style="height: 1px; border: none; color: #333; background-color: #333;" />
                                        <asp:Label ID="lblGrossPay" runat="server" Font-Bold="true" Text='<%# Eval("GrossTotal") %>'></asp:Label>
                                        <hr style="height: 1px; border: none; color: #333; background-color: #333;" />
                                    </td>
                                    <td style="width: 50px;"></td>
                                    <td>
                                        <asp:Label ID="lblTotalDeductionText" runat="server" Font-Bold="true" Font-Size="15px" Text="TOTAL DED."></asp:Label>
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <hr style="height: 1px; border: none; color: #333; background-color: #333;" />
                                        <asp:Label ID="lblTotalDeduction" runat="server" Font-Bold="true" Text='<%# Eval("TotalDeduction") %>'></asp:Label>
                                        <hr style="height: 1px; border: none; color: #333; background-color: #333;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lblNetPayable" runat="server" Font-Bold="true" Font-Size="15px" Text="NET PAYABLE"></asp:Label>
                                    </td>
                                    <td style="width: 50px;">:</td>
                                    <td>
                                        <asp:Label ID="lblNetPayableAmount" runat="server" Font-Bold="true" Font-Size="15px" Text='<%# Eval("GrossTotalSalary") %>'></asp:Label>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <center>
                                            <asp:Label ID="lblText" runat="server" Font-Bold="true" Font-Italic="true" Font-Size="20px" Text="Computer Generated Pay - Slip, Need No Signature"></asp:Label>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                            <p style="page-break-after: always;"></p>
                        </ItemTemplate>
                    </asp:Repeater>
                </center>
            </asp:Panel>
            <div style="min-height: 330px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

