<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="ImportBasicScale.aspx.cs" Inherits="SalaryModule_ImportBasicScale" %>

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
                        <legend>Bulk Update 'Basic Scale' of Existing Active Employees</legend>
                        <asp:Panel ID="pnlData" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlNatureOfEmp" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlStaffType" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlAppointment" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="flImportBasicScale" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        <asp:FileUpload ID="flImportBasicScale" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnDownloadTemplate" runat="server" CssClass="btn btn-default" Text="Download Template" ValidationGroup="Download" OnClick="btnDownloadTemplate_Click" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <asp:Button ID="btnSaveTemplate" runat="server" Text="Save Template" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnSaveTemplate_Click" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
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

