<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="ImportTDS.aspx.cs" Inherits="SalaryModule_ImportTDS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                        <legend>Import Tax Deducted at Source (TDS)</legend>

                        <asp:Panel ID="pnlData" runat="server">

                            <table style="margin: 5px 0 0 21px;">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnDownloadTemplate" runat="server" Text="Download Template" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnDownloadTemplate_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:FileUpload ID="flTDSImport" runat="server" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSaveTemplate" runat="server" Text="Save Template" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnSaveTemplate_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <div style="min-height: 370px;"></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownloadTemplate" />
            <asp:PostBackTrigger ControlID="btnSaveTemplate" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

