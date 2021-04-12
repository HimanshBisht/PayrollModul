<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="GetNoticeDetails.aspx.cs" Inherits="SalaryModule_GetNoticeDetails" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script language="javascript" type="text/javascript">
       
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
            <style type="text/css">
                .grdTable {
                    max-width: 150% !Important;
                    min-width: 150%;
                    overflow: auto;
                }

                table span, b {
                    font-size: 15px;
                }
            </style>
            <asp:Panel ID="pnlShowData" runat="server">
                <asp:Panel ID="pnlPrint" runat="server">
                    <div style="margin-top: 35px; height: auto;">
                        <fieldset>
                            <center>
                                <asp:Label ID="lblTitle" runat="server" Font-Bold="true" Font-Size="18px"></asp:Label>
                                <hr />
                                <asp:Panel ID="pnlDetails" runat="server" CssClass="btn btn-default" GroupingText="Employee Profile Summary View">
                                    <center>
                                        <table>
                                            <tr>
                                                <td><b>Name</b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblEmpName" runat="server"></asp:Label></td>
                                                <td style="width: 80px;"></td>
                                                <td><b>Emp Code</b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblEmpCode" runat="server"></asp:Label></td>
                                                <td style="width: 80px;"></td>
                                                <td><b>DOJ</b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblDOJ" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td><b>Designation</b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblDesignation" runat="server"></asp:Label></td>
                                                <td style="width: 80px;"></td>
                                                <td><b>Staff Type</b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblStaffType" runat="server"></asp:Label></td>
                                                <td style="width: 80px;"></td>
                                                <td><b>Nature</b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblNature" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td><b>Resign Date</b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblResignDate" runat="server"></asp:Label></td>
                                                <td style="width: 80px;"></td>
                                                <td><b>Actual LWD</b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblActualLWD" runat="server"></asp:Label></td>
                                                <td style="width: 80px;"></td>
                                                <td><b>As Per Norms LWD<b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblAsPerNormsLWD" runat="server"></asp:Label></td>
                                            </tr>
                                        </table>
                                        <center>
                                            <table>
                                                <tr>
                                                    <td colspan="11">
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><b>Total Working</b></td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblTotalWorking" runat="server"></asp:Label></td>
                                                    <td style="width: 50px;"></td>
                                                    <td><b>Notice Period Days</b></td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblNoticePeriodDays" runat="server"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </center>
                                    </center>
                                </asp:Panel>
                                <asp:Panel ID="pnlSalaryChanges" runat="server" Style="margin-top: 30px; max-height: 550px; width: 100%; overflow: auto; margin-bottom: 30px;">
                                    <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" CssClass="grdTable" CaptionAlign="Top">
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" Height="50px" />
                                        <RowStyle HorizontalAlign="Center" Height="50px" />
                                        <EmptyDataRowStyle ForeColor="Red" />
                                        <FooterStyle Font-Bold="true" HorizontalAlign="Center" Height="50px" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Emp Code" DataField="Emp_Code" />
                                            <asp:BoundField HeaderText="Name" DataField="Name" />
                                            <asp:BoundField HeaderText="Des" DataField="Designation" />
                                            <asp:BoundField HeaderText="Basic" DataField="Basic" />
                                            <asp:BoundField HeaderText="Standard Month Days" DataField="StandardMonthDays" />
                                            <asp:BoundField HeaderText="LWP" DataField="LWP" />
                                            <asp:BoundField HeaderText="Paid Days" DataField="PaidDays" />
                                            <asp:BoundField HeaderText="Pay Drawn Basic" DataField="PayDrawnBasic" />
                                            <asp:BoundField HeaderText="DA On Basic" DataField="DA" />
                                            <asp:BoundField HeaderText="HRA On Basic" DataField="HRA" />
                                            <asp:BoundField HeaderText="Transport" DataField="Transport" />
                                            <asp:BoundField HeaderText="Medical" DataField="Medical" />
                                            <asp:BoundField HeaderText="Washing" DataField="Washing" Visible="false" />
                                            <asp:BoundField HeaderText="Gross Revised Salary" DataField="GrossRevisedSalary" />
                                            <asp:BoundField HeaderText="Ex-Gratia" DataField="ExGratia" />
                                            <asp:BoundField HeaderText="Arrear Adjust" DataField="ArearAdjust" />
                                            <asp:BoundField HeaderText="Gross Total" DataField="GrossTotal" />
                                            <asp:BoundField HeaderText="PF" DataField="PF" />
                                            <asp:BoundField HeaderText="DED" DataField="Deduction" />
                                            <asp:BoundField HeaderText="TDS" DataField="TDS" />
                                            <asp:BoundField HeaderText="ADV" DataField="Advance" />
                                            <asp:BoundField HeaderText="TPT REC" DataField="TPTREC" />
                                            <asp:BoundField HeaderText="GIS" DataField="GIS" Visible="false" />
                                            <asp:BoundField HeaderText="ESI" DataField="ESI" />
                                            <asp:BoundField HeaderText="Gross Deduction" DataField="TotalDeduction" />
                                            <asp:BoundField HeaderText="Gross Total Salary" DataField="GrossTotalSalary" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Record Found
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                                <asp:Panel ID="pnlNoticePeriodAdjustment" runat="server" CssClass="btn btn-default" GroupingText="Final Notice Period Adjustment">
                                    <center>
                                        <table>
                                            <tr>
                                                <td><b>Notice Amount</b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblNoticeAmount" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td><b>Any Other Adjustment</b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblOtherAdjustment" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><b>Remarks</b></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <div style="width: 500px; overflow: auto;">
                                                        <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNetNoticePeriodAdjustmentText" runat="server" Font-Bold="true" Text="Final Notice Amount"></asp:Label></td>
                                                <td style="width: 10px;"></td>
                                                <td>:</td>
                                                <td style="width: 10px;"></td>
                                                <td>
                                                    <asp:Label ID="lblNetNoticePeriodAdjustmentAmount" runat="server"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    <br />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </asp:Panel>
                                <br />
                                <br />
                            </center>
                        </fieldset>
                    </div>
                </asp:Panel>
                <center>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnPrintSelected" runat="server" CssClass="btn btn-default" Text="Print OR Save Data" OnClick="btnPrintSelected_Click" />
                            </td>
                            <td></td>
                            <td>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
            </asp:Panel>
            <div style="min-height: 400px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

