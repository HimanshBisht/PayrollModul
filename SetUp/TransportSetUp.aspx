<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="TransportSetUp.aspx.cs" Inherits="SalaryModule_TransportSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">

        function ConfirmDeactivate() {
            if (confirm("Note : After Deactivating any record you need to be Update DA as well, by using DA set up Page.\nAre you sure you want to Deactivate this Record?") == true)
                return true;
            else
                return false;
        }

        function ConfirmSave() {
            if (confirm("Note : After Save/Update any record you need to be Update DA as well, by using DA set up Page.\nAre you sure you want to Continue?") == true)
                return true;
            else
                return false;
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
            <div style="margin-top: 25px;">
                <fieldset>
                    <center>
                        <legend>Transport Allowance Set Up According to Grade Pay Range</legend>
                        <asp:Panel ID="pnlData" runat="server">
                            <table>
                                <tr>
                                    <td>Min Range</td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <asp:TextBox ID="txtMinRange" runat="server" autocomplete="off" placeholder="Define Starting Value"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtMinRange"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMinRange" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 30px;"></td>
                                    <td>Max Range</td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <asp:TextBox ID="txtMaxRange" runat="server" autocomplete="off" placeholder="Define Ending Value"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtMaxRange"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td style="width: 30px;"></td>
                                    <td>Transport Value</td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <asp:TextBox ID="txtTransportValue" runat="server" autocomplete="off" placeholder="Transport According to Range"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtTransportValue"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtTransportValue" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="11">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-default" OnClientClick="return ConfirmSave()" ValidationGroup="Save" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>

                    <asp:Panel ID="pnlDetail" runat="server" Visible="true" Style="margin-top: 30px; max-height: 500px; width: 100%; overflow: auto; margin-bottom: 20px;">

                        <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="grdrecord_RowCommand">
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#C8C6C6" Font-Bold="True" ForeColor="Black" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="#E2DED6" ForeColor="#284775" />
                            <RowStyle HorizontalAlign="Center" BackColor="#F7F6F3" ForeColor="#284775" />
                            <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField HeaderText="S.No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Min Range" DataField="MinRange" />
                                <asp:BoundField HeaderText="Max Range" DataField="MaxRange" />
                                <asp:BoundField HeaderText="Transport Value" DataField="TransportValue" />
                                <asp:BoundField HeaderText="Created By" DataField="CreatedBy" />
                                <asp:BoundField HeaderText="Created On" DataField="CreatedOn" />
                                <asp:BoundField HeaderText="Updated By" DataField="UpdatedBy" />
                                <asp:BoundField HeaderText="Updated On" DataField="UpdatedOn" />
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="lnkEdit" CommandArgument='<%#Eval("TransportID") %>' />
                                        / 
                                        <asp:LinkButton ID="lnkDeactivate" runat="server" Text="Deactivate" OnClientClick="return ConfirmDeactivate()" CommandName="lnkDeactivate" CommandArgument='<%#Eval("TransportID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </fieldset>
            </div>
            <div style="min-height: 220px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

