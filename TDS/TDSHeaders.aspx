<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="TDSHeaders.aspx.cs" Inherits="SalaryModule_TDSHeaders" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script language="javascript" type="text/javascript">
        function ConfirmDeactivate() {
            if (confirm("If you Deactivating any record, it will be Deactivate all it's Rules and Footer also. Are you sure you want to Deactivate this Record?") == true)
                return true;
            else
                return false;
        }

        function CheckLengthHeads(e) {
            var textHeads = document.getElementById("ContentPlaceHolder1_txtHeads");
            if (textHeads != null) {
                if (textHeads.value.trim().length >= 500) {
                    textHeads.value = textHeads.value.substring(0, 500);
                    CheckHeadsCharacter();
                    return false;
                }
            }
        }

        function CheckHeadsCharacter() {
            var textHeads = document.getElementById("ContentPlaceHolder1_txtHeads").value;
            $("#divHeads").css("display", "block");
            var Maxsize = 500;
            $("#divHeads").text(Maxsize - textHeads.trim().length + " " + "Characters Left.");
            if (textHeads == 0) {
                $("#divHeads").css("display", "none");
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
                        <legend>TDS Headers Set Up</legend>

                        <asp:Panel ID="pnlData" runat="server">

                            <table>
                                <tr>
                                    <td>
                                        <h4>Define Heades</h4>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:TextBox ID="txtHeads" runat="server" autocomplete="off" TextMode="MultiLine" Width="500px" Rows="4" placeholder="Ex- 'Deduction to be Claimed U/S 80' (Max 500 Characters)."
                                            onkeyup="return CheckHeadsCharacter();" onkeypress="return CheckLengthHeads();" onchange="return CheckLengthHeads();"
                                            OnPaste="return CheckLengthHeads();"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtHeads" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"></td>
                                    <td>
                                        <div id="divHeads" style="color: Red;">
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <h4>Display Level</h4>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:TextBox ID="txtDisplayLevel" runat="server" autocomplete="off" placeholder="Enter Only Numbers"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDisplayLevel" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtDisplayLevel" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                        <asp:LinkButton ID="lnkExportToExcel" runat="server" Visible="false" OnClick="lnkExportToExcel_Click">
                                            <asp:Image ID="ImgExportToExcel" runat="server" Height="35px" Width="35px" ToolTip="Export To Excel" ImageUrl="~/images/ExportToExcel.png" />
                                        </asp:LinkButton>
                                    </td>
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
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="lnkEdit" CommandArgument='<%#Eval("HeaderID") %>' />
                                        / 
                                        <asp:LinkButton ID="lnkDeactivate" runat="server" Text="Deactivate" OnClientClick="return ConfirmDeactivate()" CommandName="lnkDeactivate" CommandArgument='<%#Eval("HeaderID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Header Text" DataField="HeaderText" />
                                <asp:BoundField HeaderText="Display Level" DataField="DisplayLevel" />
                                <asp:BoundField HeaderText="Created By" DataField="CreatedBy" />
                                <asp:BoundField HeaderText="Created On" DataField="CreatedOn" />
                                <asp:BoundField HeaderText="Updated By" DataField="UpdatedBy" />
                                <asp:BoundField HeaderText="Updated On" DataField="UpdatedOn" />
                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </fieldset>
            </div>
            <div style="min-height: 250px;"></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

