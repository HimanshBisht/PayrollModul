<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="SalaryModule_ChangePassword" %>

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
            <div style="margin-top: 25px;">
                <fieldset>
                    <center>
                        <legend>Change Your Current Login Password</legend>
                        <asp:Panel ID="pnlData" runat="server">
                            <table>
                                <tr>
                                    <td>User Name</td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblUserName" runat="server" Font-Bold="true" Font-Size="15px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Current Password</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtCurrentPassword" runat="server" autocomplete="off" TextMode="Password" placeholder="Enter Your Current Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCurrentPassword" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>New Password</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtNewPassword" runat="server" autocomplete="off" TextMode="Password" placeholder="Set Your New Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Confirm Password</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtConfirmPassword" runat="server" autocomplete="off" TextMode="Password" placeholder="Confirm New Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtConfirmPassword" Display="Dynamic" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cv" runat="server" ControlToValidate="txtConfirmPassword" Font-Bold="true" ForeColor="Red" ControlToCompare="txtNewPassword" ErrorMessage="?" ToolTip="Password Mismatch" ValidationGroup="Save"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"></td>
                                    <td>
                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnUpdate_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <div style="min-height: 280px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

