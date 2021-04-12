<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="SurchargeRules.aspx.cs" Inherits="SalaryModule_SurchargeRules" %>

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
                        <legend>Surcharge Rules Set Up</legend>
                        <asp:Panel ID="pnlData" runat="server">
                            <table>
                                <tr>
                                    <td>From Year</td>
                                    <td>
                                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 15px;"></td>
                                    <td>Age From</td>
                                    <td>
                                        <asp:TextBox ID="txtAgeFrom" runat="server" placeholder="Age Starting From" AutoComplete="off"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtAgeFrom"
                                            FilterMode="ValidChars" ValidChars="0123456789">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAgeFrom" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 15px;"></td>
                                    <td>Age To</td>
                                    <td>
                                        <asp:TextBox ID="txtAgeTo" runat="server" placeholder="Age Ended To" AutoComplete="off"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtAgeTo"
                                            FilterMode="ValidChars" ValidChars="0123456789">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtAgeTo" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Net Income From</td>
                                    <td>
                                        <asp:TextBox ID="txtNetIncomeFrom" runat="server" placeholder="Net Income Starting From" AutoComplete="off"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtNetIncomeFrom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNetIncomeFrom" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 15px;"></td>
                                    <td>Net Income To</td>
                                    <td>
                                        <asp:TextBox ID="txtNetIncomeTo" runat="server" placeholder="Net Income Ended To" AutoComplete="off"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtNetIncomeTo"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNetIncomeTo" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 15px;"></td>
                                    <td>Surcharge Rates(%)</td>
                                    <td>
                                        <asp:TextBox ID="txtSurchargeRates" runat="server" placeholder="Mention Only % Figure" AutoComplete="off"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtSurchargeRates"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtSurchargeRates" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnSave_Click" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                                        <td>
                                            <asp:Button ID="btnPrintSelected" runat="server" CssClass="btn btn-default" Text="Print OR Save Sheet" OnClick="btnPrintSelected_Click" />
                                        </td>
                                        <td style="width: 5px;"></td>
                                        <td>
                                            <asp:LinkButton ID="lnkExportToExcel" runat="server" OnClick="lnkExportToExcel_Click">
                                                <asp:Image ID="ImgExportToExcel" runat="server" Height="35px" Width="35px" ToolTip="Export To Excel" ImageUrl="~/images/ExportToExcel.png" />
                                            </asp:LinkButton>
                                        </td>
                                    </asp:Panel>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:Panel ID="pnlDetail" runat="server" Visible="true" Style="width: 100%; margin-bottom: 20px;">

                        <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" OnRowCommand="grdrecord_RowCommand" Width="100%">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" Height="50px" />
                            <RowStyle HorizontalAlign="Center" Height="50px" />
                            <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                            <FooterStyle Font-Bold="true" HorizontalAlign="Center" Height="50px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="lnkEdit" CommandArgument='<%#Eval("SurchargeRuleID") %>' />
                                        / 
                                        <asp:LinkButton ID="lnkDeactivate" runat="server" Text="Deactivate" OnClientClick="return ConfirmDeactivate()" CommandName="lnkDeactivate" CommandArgument='<%#Eval("SurchargeRuleID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S.No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Financial Year" DataField="FinancialYear" />
                                <asp:BoundField HeaderText="Age Range" DataField="AgeCriteria" />
                                <asp:BoundField HeaderText="Net Income Range" DataField="NetIncomeCriteria" />
                                <asp:BoundField HeaderText="Surcharge Rates (%)" DataField="SurchargeRates" />
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
            <div style="min-height: 300px;"></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

