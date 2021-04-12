<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="YearSetup.aspx.cs" Inherits="SalaryModule_YearSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function ConfirmDeactivate() {
            if (confirm("Are you sure you want to Deactivate this Record?") == true)
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

            <div style="margin-top: 25px; height: auto;">

                <fieldset>
                    <center>
                        <legend>Year Set Up</legend>

                        <asp:Panel ID="pnlData" runat="server">

                            <table style="margin: 5px 0 0 21px;">
                                <tr>
                                    <td>Year
                                    </td>
                                    <td style="width: 30px;"></td>

                                    <td>
                                        <asp:TextBox ID="txtYear" runat="server" autocomplete="off" placeholder="Enter Year"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" Enabled="True" TargetControlID="txtYear" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtYear" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>

                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkExportToExcel" runat="server" Visible="false" OnClick="lnkExportToExcel_Click">
                                            <asp:Image ID="ImgExportToExcel" runat="server" Height="35px" Width="35px" ToolTip="Export To Excel" ImageUrl="~/images/ExportToExcel.png" />
                                        </asp:LinkButton>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td style="width: 30px;"></td>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:Panel ID="pnlDetail" runat="server" Style="margin-top: 30px; max-height: 500px; width: 100%; overflow: auto; margin-bottom: 20px;">

                        <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" OnRowCommand="grdrecord_RowCommand" Width="100%">
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
                                <asp:BoundField HeaderText="Year" DataField="Year" />
                                <asp:BoundField HeaderText="Created By" DataField="CreatedBy" />
                                <asp:BoundField HeaderText="Created On" DataField="CreatedOn" />
                                <asp:BoundField HeaderText="Updated By" DataField="UpdatedBy" />
                                <asp:BoundField HeaderText="Updated On" DataField="UpdatedOn" />
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="lnkEdit" CommandArgument='<%#Eval("YearID") %>' />
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
            <div style="min-height: 300px;"></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

