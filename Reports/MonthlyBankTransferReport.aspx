<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="MonthlyBankTransferReport.aspx.cs" Inherits="SalaryModule_BankTransferReport" %>

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
            <div style="margin-top: 25px; height: auto;">
                <fieldset>
                    <center>
                        <table style="margin: 10px 0 0 0;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReport" runat="server" Font-Bold="true" Font-Size="15px" Text="Monthly Bank Transfer Salary Report"></asp:Label>
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
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTransferType" runat="server">
                                                <asp:ListItem Text="All Type" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="NEFT" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Transfer" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </asp:Panel>
                                    <td style="width: 5px;"></td>
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

            <center>
                <asp:Panel ID="pnlStmt" runat="server" Style="margin-top: 20px;">
                    <asp:Label ID="lblSTMT" runat="server" Font-Size="Medium" Font-Bold="true"></asp:Label>
                </asp:Panel>
            </center>

            <asp:Panel ID="pnlDetail" runat="server" Visible="false" Style="margin-top: 30px; max-height: 550px; width: 100%; overflow: auto; margin-bottom: 20px;">

                <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" Width="100%" ShowFooter="true" OnRowDataBound="grdrecord_RowDataBound">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" Height="50px" />
                    <RowStyle HorizontalAlign="Center" Height="50px" />
                    <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                    <FooterStyle Font-Bold="true" HorizontalAlign="Center" Height="50px" />
                    <Columns>
                        <asp:BoundField HeaderText="Emp Code" DataField="Emp_Code" />
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Transaction Type" DataField="AccountType" />
                        <asp:TemplateField HeaderText="Credit Account Number">
                            <ItemTemplate>
                                <asp:Label ID="lblSBAccount" runat="server" Text='<%# Eval("AccountNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Credit Account Name" DataField="Name" />
                        <asp:TemplateField HeaderText="IFSC Code">
                            <ItemTemplate>
                                <asp:Label ID="lblIFSCCode" runat="server" Text='<%# Eval("IFSCCODE") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblText" runat="server" Text="Grand Total"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:Label ID="lblGrossTotalSalary" runat="server" Text='<%# Eval("GrossTotalSalary") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblGrandTotalGrossTotalSalary" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Narration">
                            <ItemTemplate>
                                <asp:Label ID="lblNarration" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </asp:Panel>
            <div style="min-height: 360px;"></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

