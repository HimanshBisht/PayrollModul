<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master"
    AutoEventWireup="true" CodeFile="IDFDisapproval.aspx.cs" Inherits="SalaryModule_IDFDisapproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script language="javascript" type="text/javascript">

        function ConfirmNotApproved() {
            if (confirm("Are you sure you want to Dis-approve this IDF?") == true)
                return true;
            else
                return false;
        }

        function disable_textbox(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if ((charCode > 0) && (charCode != 9))
                return false;
            return true;
        }

        function CheckAllDataGridCheckBoxes(aspCheckBoxID, checkVal) {
            re = new RegExp(aspCheckBoxID + '$')  //generated control name contains a $
            for (i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i]
                if (elm.type == 'checkbox' && elm.disabled == false) {
                    if (re.test(elm.name)) {
                        elm.checked = checkVal
                    }
                }
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
                        <table style="margin: 20px 0 0 0;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblReport" runat="server" Font-Bold="true" Font-Size="15px" Text="IDF Dis-approval"></asp:Label>
                                </td>
                                <td style="width: 650px;"></td>
                                <asp:Panel ID="pnlTotalRecords" runat="server" Visible="false">
                                    <td>
                                        <asp:Label ID="lblTotal" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="15px"
                                            Text="Total Records : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTotalEmployees" runat="server" Font-Bold="true" ForeColor="Red"
                                            Font-Size="15px"></asp:Label>
                                    </td>
                                </asp:Panel>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <hr />
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlData" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNatureOfEmp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNatureOfEmp_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAppointment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAppointment_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStaffType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStaffType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlDesignation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <asp:Panel ID="pnlEmployee" runat="server" Visible="false">
                                        <td>
                                            <asp:DropDownList ID="ddlemployee" runat="server"></asp:DropDownList>
                                        </td>
                                    </asp:Panel>
                                    <td>
                                        <asp:Button ID="btnSearch" runat="server" Text="Get Report" CssClass="btn btn-default" ValidationGroup="Search" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" CssClass="btn btn-primary" runat="server" OnClick="btnCancel_Click" />
                                    </td>
                                    <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                                        <td>
                                            <asp:Button ID="btnPrintSelected" runat="server" CssClass="btn btn-default" Text="Print OR Save Sheet" OnClick="btnPrintSelected_Click" />
                                            <asp:LinkButton ID="lnkExportToExcel" runat="server" OnClick="lnkExportToExcel_Click">
                                                <asp:Image ID="ImgExportToExcel" runat="server" Height="35px" Width="35px" ToolTip="Export To Excel"
                                                    ImageUrl="~/images/ExportToExcel.png" />
                                            </asp:LinkButton>
                                        </td>
                                    </asp:Panel>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <style type="text/css">
                .grdTable {
                    max-width: 100% !important;
                    min-width: 100%;
                    overflow: auto;
                }
            </style>

            <center>
                <asp:Panel ID="pnlStmt" runat="server" Visible="false" Style="margin-top: 30px;">
                    <asp:Label ID="lblSTMT" runat="server" Font-Size="Medium" Font-Bold="true"></asp:Label>
                </asp:Panel>
            </center>
            <asp:Panel ID="pnlApproval" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnDisapproved" runat="server" Text="Dis-Approve IDF" OnClientClick="return ConfirmNotApproved()" CssClass="btn btn-default" OnClick="btnDisapproved_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlDetail" runat="server" Visible="false" Style="margin-top: 30px; max-height: 500px; width: 100%; overflow: auto; margin-bottom: 20px;">
                <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" CssClass="grdTable">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" Height="50px" />
                    <RowStyle HorizontalAlign="Center" Height="50px" />
                    <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                    <FooterStyle Font-Bold="true" HorizontalAlign="Center" Height="50px" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <input id="chkAllItems" type="checkbox" onclick="CheckAllDataGridCheckBoxes('SelectChk', document.forms[0].chkAllItems.checked)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="SelectChk" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Profile ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblProfileID" runat="server" Text='<%# Eval("ProfileID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Emp Code" DataField="Emp_Code" />
                        <asp:BoundField HeaderText="Name" DataField="Name" />
                        <asp:BoundField HeaderText="Designation" DataField="DesignationText" />
                        <asp:BoundField HeaderText="Nature" DataField="NatureOfEmpText" />
                        <asp:BoundField HeaderText="Staff Type" DataField="StaffTypeText" />
                        <asp:BoundField HeaderText="Appointment" DataField="AppointmentText" />
                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </asp:Panel>
            <div style="min-height: 350px;">
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
