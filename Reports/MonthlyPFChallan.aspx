<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="MonthlyPFChallan.aspx.cs" Inherits="SalaryModule_MonthlyPFChallan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/jquery.min.js"></script>

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
                        <table style="margin: 30px 0 0 0;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReport" runat="server" Font-Bold="true" Font-Size="15px" Text="Monthly PF Challen Report"></asp:Label>
                                </td>
                                <td style="width: 500px;"></td>
                                <asp:Panel ID="pnlTotalRecords" runat="server" Visible="false">
                                    <td>
                                        <asp:Label ID="lblTotal" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="15px"
                                            Text="Total Records : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalRecords" runat="server" Font-Bold="true" ForeColor="Red"
                                            Font-Size="15px"></asp:Label>
                                    </td>
                                </asp:Panel>
                            </tr>
                        </table>
                    </center>
                    <asp:Panel ID="pnlData" runat="server">
                        <center>
                            <table style="margin: 15px 0 0 21px;">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>

                                    <asp:Panel ID="pnlEmployees" runat="server" Visible="false">
                                        <td>
                                            <asp:DropDownList ID="ddlemployee" runat="server"></asp:DropDownList>
                                        </td>
                                    </asp:Panel>
                                    <td>
                                        <asp:Button ID="btnSearch" runat="server" Text="Get Report" CssClass="btn btn-default" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                                        <td>
                                            <asp:Button ID="btnPrintSelected" runat="server" CssClass="btn btn-default" Text="Print OR Save Sheet" OnClick="btnPrintSelected_Click" />
                                            <asp:LinkButton ID="lnkExportToExcel" runat="server" OnClick="lnkExportToExcel_Click">
                                                <asp:Image ID="ImgExportToExcel" runat="server" Height="35px" Width="35px" ToolTip="Export To Excel" ImageUrl="~/images/ExportToExcel.png" />
                                            </asp:LinkButton>
                                        </td>

                                    </asp:Panel>

                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                        <br />
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnExport" Text="Export Grid View" OnClick="btnExport_Click" runat="server" />

                                    </td>
                                </tr>
                            </table>
                        </center>
                    </asp:Panel>
                </fieldset>
            </div>

            <center>
                <asp:Panel ID="pnlStmt" runat="server" Style="margin-top: 30px;">
                    <asp:Label ID="lblSTMT" runat="server" Font-Size="Medium" Font-Bold="true"></asp:Label>
                </asp:Panel>
            </center>
            <asp:Panel ID="pnlDetail" runat="server" Style="margin-top: 30px; max-height: 550px; width: 100%; overflow: auto; margin-bottom: 20px;">

                <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" Width="100%" ShowFooter="false" OnRowDataBound="grdrecord_RowDataBound">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" Height="50px" />
                    <RowStyle HorizontalAlign="Center" Height="50px" />
                    <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                    <FooterStyle Font-Bold="true" HorizontalAlign="Center" Height="50px" />

                    <Columns>
                        <%-- <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lbl" runat="server" Text="<%#Container.DataItemIndex+1 %>"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <%--<asp:TemplateField HeaderText="S.No">
                 <ItemTemplate>
                     <%# Container.DataItemIndex + 1 %>
                 </ItemTemplate>
             </asp:TemplateField>--%>
                        <%--<asp:BoundField HeaderText="Emp Code" DataField="Emp_Code" Visible="false" />
                        <asp:BoundField HeaderText="PF No." DataField="PFNO" Visible="false" />--%>
                        <asp:BoundField HeaderText="UAN No." DataField="UANNO" />
                        <asp:BoundField HeaderText="Staff Name" DataField="Name" />

                        <%-- <asp:TemplateField HeaderText="Paid Days" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDAForReporta" runat="server" Text='<%# Eval("PaidDays") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblText" runat="server" Text="Grand Total"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField HeaderText="GrossTotalSalary" DataField="GrossTotalSalary" />
                        <%--<asp:TemplateField HeaderText="Gross Wages">
                            <ItemTemplate><%#Eval("GrossTotalSalary") %></ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:BoundField HeaderText="EPF(Item)" DataField="DAForReport" />

                        <%-- <asp:TemplateField HeaderText="(EPF Wages)Basic + DA">
                            <ItemTemplate>
                                <asp:Label ID="lblDAForReport" runat="server" Text='<%# Eval("DAForReport") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblGrandDAForReport" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField HeaderText="EPSWages" DataField="NewEPSWages" />
                        <%--<asp:TemplateField HeaderText="EPS Wages">
                            <ItemTemplate>
                                <asp:Label ID="lblEPFWages" runat="server" Text='<%# Eval("EPSWages") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblGranddsDAForReport" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>--%>
                        <%--<asp:BoundField HeaderText="EDLI Wages" DataField="EPSWages" />--%>

                        <%--  <asp:TemplateField HeaderText="EDLI Wages">
                            <ItemTemplate>
                                <asp:Label ID="lblEPSWages" runat="server" Text='<%# Eval("EPSWages") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField HeaderText="EDLI Wages" DataField="NewEPSWages" />

                        <asp:BoundField HeaderText="EPF Contribution Being Remitted" DataField="EPFContribute" />
                        <asp:BoundField HeaderText="EPS Contribution Being Remitted" DataField="EPSContributesWages" />   
                       <%-- EPSContribute--%>
                        <asp:BoundField HeaderText="Diff Amount(EPF-EPS)" />
                         <asp:BoundField HeaderText="NCP Days" DataField="LWP"/>
                      <%--  <asp:TemplateField HeaderText="NCP Days">
                            <ItemTemplate>
                                <asp:Label ID="lblNcp" runat="server" Text='<%# Eval("LWP") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblGrandTotalPF" runat="server"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField HeaderText="Refund Of Advances" DataField="Advance" />

                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </asp:Panel>

            <div style="min-height: 340px;"></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportToExcel" />
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

