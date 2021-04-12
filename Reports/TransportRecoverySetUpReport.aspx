<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="TransportRecoverySetUpReport.aspx.cs" Inherits="SalaryModule_TransportRecoverySetUpReport" %>

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
                        <table>
                            <tr>
                                <td>
                                    <legend>Transport Recovery Set Up For Consolidate Employees (TPT-Rec) Report</legend>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkExportToExcel" runat="server" Visible="false" OnClick="lnkExportToExcel_Click">
                                        <asp:Image ID="ImgExportToExcel" runat="server" Height="35px" Width="35px" ToolTip="Export To Excel" ImageUrl="~/images/ExportToExcel.png" />
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <asp:Panel ID="pnlDetail" runat="server" Style="margin-top: 30px; max-height: 500px; width: 100%; overflow: auto; margin-bottom: 20px;">

                        <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" Width="100%">
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
                                <asp:BoundField HeaderText="Recovery Amount" DataField="RecoveryAmount" />
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
            <div style="min-height: 370px;"></div>
        </ContentTemplate>
          <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

