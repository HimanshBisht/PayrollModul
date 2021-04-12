<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master"
    AutoEventWireup="true" CodeFile="NoticePeriodReport.aspx.cs" Inherits="SalaryModule_NoticePeriodReport" %>

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
                    </span></h4>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: auto; width: 100%; overflow: auto;">
                <fieldset>
                    <center>
                        <table style="margin: 30px 0 0 0;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReport" runat="server" Font-Bold="true" Font-Size="15px" Text="Notice Period Report"></asp:Label>
                                </td>
                                <td style="width: 700px;"></td>
                                <asp:Panel ID="pnlTotalRecords" runat="server" Visible="false">
                                    <td>
                                        <asp:Label ID="lblTotal" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="15px"
                                            Text="Total Record : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalEmployees" runat="server" Font-Bold="true" ForeColor="Red"
                                            Font-Size="15px"></asp:Label>
                                    </td>
                                </asp:Panel>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlData" runat="server">
                            <table>
                                <tr>
                                    <td colspan="5">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlemployee" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlType" runat="server">
                                            <asp:ListItem Text="All Notice Type" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Notice Period Payment" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Notice Period Recovery" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSearch" runat="server" Text="Get Report" CssClass="btn btn-default" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnCancel" Text="Clear All" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                        <asp:LinkButton ID="lnkExportToExcel" runat="server" Visible="false" OnClick="lnkExportToExcel_Click">
                                            <asp:Image ID="ImgExportToExcel" runat="server" Height="35px" Width="35px" ToolTip="Export To Excel"
                                                ImageUrl="~/images/ExportToExcel.png" />
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <style type="text/css">
                .grdTable {
                    max-width: 150% !important;
                    min-width: 150%;
                    overflow: auto;
                }
            </style>
            <center>
                <asp:Panel ID="pnlStmt" runat="server" Style="margin-top: 30px;">
                    <asp:Label ID="lblSTMT" runat="server" Font-Size="Medium" Font-Bold="true"></asp:Label>
                </asp:Panel>
            </center>    
            <asp:Panel ID="pnlDetail" runat="server" Style="margin-top: 30px; max-height: 600px; width: 100%; overflow: auto; margin-bottom: 20px;">
                <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" CssClass="grdTable" OnRowCommand="grdrecord_RowCommand"
                    AllowPaging="true" PageSize="100" OnPageIndexChanging="grdrecord_PageIndexChanging">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" Height="50px" />
                    <RowStyle HorizontalAlign="Center" Height="50px" />
                    <EmptyDataRowStyle ForeColor="Red" />
                    <FooterStyle Font-Bold="true" HorizontalAlign="Center" Height="50px" />
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkTrainingItemDetails" runat="server" NavigateUrl='<%#"~/Reports/GetNoticeDetails.aspx?Item=" + DataBinder.Eval(Container.DataItem,"ProfileID").ToString().Trim() %>'
                                    Name='link' CssClass="btn btn-info" Target="_blank">View</asp:HyperLink>                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Emp Code" DataField="Emp_Code" />
                        <asp:BoundField HeaderText="Name" DataField="Name" />
                        <asp:BoundField HeaderText="Designation" DataField="Designation" />
                        <asp:BoundField HeaderText="Nature" DataField="NatureOfEmp" />
                        <asp:BoundField HeaderText="DOJ" DataField="DOJ" />
                        <asp:BoundField HeaderText="Resign Date" DataField="ResignationDate" />
                        <asp:BoundField HeaderText="Actual LWD" DataField="LWD" />
                        <asp:BoundField HeaderText="As Per Norms LWD" DataField="AsPerNormsLWD" />
                        <asp:BoundField HeaderText="Total Working" DataField="TotalWorking" />
                        <asp:BoundField HeaderText="Notice Days" DataField="NoticePeriodDays" />
                        <asp:BoundField HeaderText="Notice Type" DataField="NoticeType" />
                        <asp:BoundField HeaderText="Notice Amount" DataField="NoticeAmount" />
                        <asp:BoundField HeaderText="Created By" DataField="CreatedBy" />
                        <asp:BoundField HeaderText="Created On" DataField="CreatedOn" />
                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </asp:Panel>
            <div style="min-height: 290px;">
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
