<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="MakeUser.aspx.cs" Inherits="SalaryModule_MakeUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function ConfirmDeactivate() {
            var ddlUsers = $("#ContentPlaceHolder1_ddlUsers").val();

            if (ddlUsers > 0) {
                if (confirm("Are you sure you want to Deactivate this User?") == true)
                    return true;
                else
                    return false;
            }
            else {
                alert("Please Select an User to Deactivate.");
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
            <div style="margin-top: 25px; height: auto;">
                <fieldset>
                    <center>
                        <legend>Make User and Assign Page Rights</legend>
                    </center>
                    <asp:Panel ID="pnlData" runat="server">
                        <style type="text/css">
                            table, #chkbxSetupPageList, #chkbxImportDataPageList, #chkbxActionsPageList, #chkbxReportsChkbx, #chkbxTDSChkbx, label {
                                margin-left: 20px;
                                margin-top: -16px;
                                margin-bottom: 0px;
                            }

                            table, #chkbxSetupPageList, #chkbxImportDataPageList, #chkbxActionsPageList, #chkbxReportsChkbx, #chkbxTDSChkbx, radio {
                                margin-top: 5px;
                            }
                        </style>
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblText" runat="server" Text="To Update"></asp:Label>
                                    </td>
                                    <td style="width: 15px;"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlUsers" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlUsers" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Deactivate"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnDeactivate" runat="server" Text="Want to Deactivate" CssClass="btn btn-default" ValidationGroup="Deactivate" OnClientClick="return ConfirmDeactivate()" OnClick="btnDeactivate_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                    </td>
                                </tr>
                            </table>

                            <asp:Panel ID="pnlAdd" runat="server" GroupingText="Add/Update User">
                                <table style="margin: 5px 0 0 21px;">
                                    <tr>
                                        <td>User Name
                                        </td>
                                        <td style="width: 10px;"></td>
                                        <td>
                                            <asp:TextBox ID="txtUserName" runat="server" autocomplete="off" placeholder="Define User Name For Login"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUserName" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>Password
                                        </td>
                                        <td style="width: 10px;"></td>
                                        <td>
                                            <asp:TextBox ID="txtPassword" runat="server" autocomplete="off" placeholder="Make a Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <h3>Select Pages From Page List to Assign Rights</h3>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </center>
                    </asp:Panel>
                </fieldset>
                <center>
                    <table style="margin-left: -40px;">
                        <tr>
                            <td style="padding-left: 20px;">
                                <h4>Pages in Set Up</h4>
                            </td>
                            <td style="padding-left: 20px;">
                                <h4>Pages in Import Data</h4>
                            </td>
                            <td style="padding-left: 20px;">
                                <h4>Pages in Actions</h4>
                            </td>
                            <td style="padding-left: 20px;">
                                <h4>Pages in Reports</h4>
                            </td>
                            <td style="padding-left: 20px;">
                                <h4>Pages in TDS</h4>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlSetupChkbx" runat="server" Height="300px" Width="250px" Style="overflow: auto;">
                                    <asp:CheckBox ID="chkSetupAll" Text="Select All" runat="server" AutoPostBack="true" OnCheckedChanged="chkSetupAll_CheckedChanged" />
                                    <asp:CheckBoxList ID="chkbxSetupPageList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkbxSetupPageList_SelectedIndexChanged"></asp:CheckBoxList>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlImportDataChkbx" runat="server" Height="300px" Width="250px" Style="overflow: auto;">
                                    <asp:CheckBox ID="chkImportDataAll" Text="Select All" runat="server" AutoPostBack="true" OnCheckedChanged="chkImportDataAll_CheckedChanged" />
                                    <asp:CheckBoxList ID="chkbxImportDataPageList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkbxImportDataPageList_SelectedIndexChanged"></asp:CheckBoxList>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlActionsChkbx" runat="server" Height="300px" Width="250px" Style="overflow: auto;">
                                    <asp:CheckBox ID="chkActionsAll" Text="Select All" runat="server" AutoPostBack="true" OnCheckedChanged="chkActionsAll_CheckedChanged" />
                                    <asp:CheckBoxList ID="chkbxActionsPageList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkbxActionsPageList_SelectedIndexChanged"></asp:CheckBoxList>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlReportsChkbx" runat="server" Height="300px" Width="250px" Style="overflow: auto;">
                                    <asp:CheckBox ID="chkReportsAll" Text="Select All" runat="server" AutoPostBack="true" OnCheckedChanged="chkReportsAll_CheckedChanged" />
                                    <asp:CheckBoxList ID="chkbxReportsPageList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkbxReportsPageList_SelectedIndexChanged"></asp:CheckBoxList>
                                </asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="pnlTDSChkbx" runat="server" Height="300px" Width="250px" Style="overflow: auto;">
                                    <asp:CheckBox ID="chkTDSAll" Text="Select All" runat="server" AutoPostBack="true" OnCheckedChanged="chkTDSAll_CheckedChanged" />
                                    <asp:CheckBoxList ID="chkbxTDSPageList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkbxTDSPageList_SelectedIndexChanged"></asp:CheckBoxList>
                                </asp:Panel>
                            </td>
                        </tr>
                        <td>
                            <br />
                        </td>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save User" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnSave_Click" />
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <div style="min-height: 50px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

