<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="MonthlyReimbursementReport.aspx.cs" Inherits="SalaryModule_MonthlyReimbursementReport" %>

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
                        <legend>Monthly Reimbursement Report</legend>     
                    </center>
                    <asp:Panel ID="pnlData" runat="server">
                        <center>
                             <table style="margin: 5px 0 0 21px;">
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
                                            <asp:DropDownList ID="ddlemployee" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlemployee_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlemployee" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                        </td>
                                    </asp:Panel>                                  
                                    <td>
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                </fieldset>
            </div>
            <asp:Panel ID="pnlResult" runat="server" Visible="false">

                <asp:Panel ID="pnlDetail" runat="server" Visible="false" Style="margin-top: 30px; max-height: 500px; width: 100%; overflow: auto; margin-bottom: 20px;">
                    <center>

                        <asp:Label ID="lblSTMT" runat="server" Font-Size="15px" Font-Bold="true"></asp:Label>

                        <asp:Panel ID="pnlReimbursement" runat="server" Style="margin-top: 40px;">
                            <table border="1" cellpadding="10" width="70%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSRNO1" runat="server" Text="1."></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReimbursementFor1" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReimbursementValue1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSRNO2" runat="server" Text="2."></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReimbursementFor2" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReimbursementValue2" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblSRNO3" runat="server" Text="3."></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReimbursementFor3" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReimbursementValue3" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblSRNO4" runat="server" Text="4."></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReimbursementFor4" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReimbursementValue4" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblSRNO5" runat="server" Text="5."></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReimbursementFor5" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReimbursementValue5" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lblTotalText" runat="server" Text="Total"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>                            
                            <br />
                            <br />
                            <br />                            
                            <b>(Approved By Chairman)</b>
                        </asp:Panel>
                    </center>
                </asp:Panel>

                <asp:Panel ID="pnlPrint" runat="server" Visible="false">
                    <center>
                        <asp:Button ID="btnPrint" runat="server" Text="Print OR Save Sheet" CssClass="btn btn-default" OnClick="btnPrint_Click" />
                    </center>
                </asp:Panel>

            </asp:Panel>
            <div style="min-height: 420px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

