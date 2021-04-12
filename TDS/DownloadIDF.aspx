<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="DownloadIDF.aspx.cs" Inherits="SalaryModule_DownloadIDF" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script language="javascript" type="text/javascript">
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

        function validate() {
            var Count = 0;
            var gridView = document.getElementById("<%=grdrecord.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    Count = Count + 1;
                }
            }
            if (Count > 0) {
                return true;
            }
            else {
                alert("Please Select at least one Row.");
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
                    <table style="margin: 20px 0 0 0;">
                        <tr>
                            <td>
                                <asp:Label ID="lblReport" runat="server" Font-Bold="true" Font-Size="15px" Text="Download Investment Declaration Form (IDF)"></asp:Label>
                            </td>
                            <asp:Panel ID="pnlTotalRecords" runat="server" Visible="false">
                                <td>
                                    <asp:Label ID="lblTotal" runat="server" Style="margin-left: 500px;" Font-Bold="true" ForeColor="Red" Font-Size="15px"
                                        Text="Total Employees : "></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblTotalEmployees" runat="server" Font-Bold="true" ForeColor="Red"
                                        Font-Size="15px"></asp:Label>
                                </td>
                            </asp:Panel>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <br />
                            </td>
                        </tr>
                    </table>
                    <center>
                        <asp:Panel ID="pnlSelect" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlFromFinancialYear" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlFromFinancialYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="btnGetEmployee"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEmployeeStatus" runat="server">
                                            <asp:ListItem Text="Select Employee Status" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="All Active Employees" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="All Deactive Employees" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEmployeeStatus" InitialValue="2" ErrorMessage="*" ForeColor="Red" ValidationGroup="btnGetEmployee"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNatureOfEmp" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 5px;"></td>
                                    <td>
                                        <asp:Button ID="btnGetEmployee" runat="server" Text="Get Employee's" CssClass="btn btn-default" ValidationGroup="btnGetEmployee" OnClick="btnGetEmployee_Click" />
                                        <asp:Button ID="btnReset" Text="Reset" runat="server" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <style type="text/css">
                        .grdTable {
                            max-width: 100% !important;
                            min-width: 100%;
                            overflow: auto;
                        }
                    </style>

                    <center>
                        <asp:Panel ID="pnlStmt" runat="server" Visible="false">
                            <asp:Label ID="lblSTMT" runat="server" Font-Size="Medium" Font-Bold="true"></asp:Label>
                        </asp:Panel>
                    </center>
                    <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnDownloadPDF" runat="server" Text="Print / Save All Selected" OnClientClick="return validate()"
                                        OnClick="btnDownloadPDF_Click" CssClass="btn btn-default" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlDetail" runat="server" Visible="false" Style="margin-top: 20px; max-height: 500px; width: 100%; overflow: auto; margin-bottom: 20px;">
                        <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" CssClass="grdTable"
                            AllowPaging="true" PageSize="500" OnPageIndexChanging="grdrecord_PageIndexChanging">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" />
                            <RowStyle HorizontalAlign="Center" />
                            <EmptyDataRowStyle ForeColor="Red" HorizontalAlign="Center" />
                            <FooterStyle Font-Bold="true" HorizontalAlign="Center" />
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
                                <asp:TemplateField HeaderText="Emp Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmp_Code" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Name" DataField="Name" />
                                <asp:BoundField HeaderText="Designation" DataField="DesignationText" />
                                <asp:BoundField HeaderText="Nature" DataField="NatureOfEmpText" />
                                <asp:BoundField HeaderText="Appointment" DataField="AppointmentText" />
                                <asp:BoundField HeaderText="Staff Type" DataField="StaffTypeText" />
                            </Columns>
                            <EmptyDataTemplate>
                                No Record Found
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Panel ID="Panel1" runat="server" Visible="false">
                        <asp:Panel ID="pnlPrint" runat="server">
                            <asp:Repeater ID="rptIDFData" runat="server" OnItemDataBound="rptIDFData_ItemDataBound">
                                <ItemTemplate>
                                    <asp:Panel ID="pnlGetData" runat="server">
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblHead1" runat="server" Font-Bold="true" Font-Size="18px" Font-Underline="true" Text='<%# Eval("SchoolName") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>

                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblHead2" runat="server" Font-Bold="true" Font-Size="18px" Font-Underline="true" Text='<%# Eval("IDFText") %>'></asp:Label></h4>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" Text="Employee Code" Font-Bold="true" Font-Size="15px"></asp:Label>
                                                    </td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblProfileID" runat="server" Font-Size="15px" Visible="false" Text='<%# Eval("ProfileID") %>'></asp:Label>
                                                        <asp:Label ID="lblEmp_Code" runat="server" Font-Size="15px" Text='<%# Eval("EmpCode") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 20px;"></td>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" Text="Name" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblName" runat="server" Font-Size="15px" Text='<%# Eval("Name") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 20px;"></td>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" Text="Gender" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblGender" runat="server" Font-Size="15px" Text='<%# Eval("Gender") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="17">
                                                        <div style="height: 5px;"></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="Designation" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblDesignation" runat="server" Font-Size="15px" Text='<%# Eval("Designation") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 20px;"></td>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" Text="ContactNo" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblContactNo" runat="server" Font-Size="15px" Text='<%# Eval("ContactNo") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 20px;"></td>
                                                    <td>
                                                        <asp:Label ID="Label6" runat="server" Text="Email ID" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblEmailID" runat="server" Font-Size="15px" Text='<%# Eval("EmailID") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="17">
                                                        <div style="height: 5px;"></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label7" runat="server" Text="PAN No." Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblPanNo" runat="server" Font-Size="15px" Text='<%# Eval("PanCardNo") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 20px;"></td>
                                                    <td>
                                                        <asp:Label ID="Label10" runat="server" Text="DOB" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblDOB" runat="server" Font-Size="15px" Text='<%# Eval("DOB") %>'></asp:Label>
                                                    </td>
                                                    <td style="width: 20px;"></td>
                                                    <td>
                                                        <asp:Label ID="Label11" runat="server" Text="Age As on Date" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblEmpAge" runat="server" Font-Size="15px" Text='<%# Eval("AgeAsOnDate") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="17">
                                                        <div style="height: 5px;"></div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label8" runat="server" Text="Senior Citizen" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblSeniorCitizen" runat="server" Font-Size="15px" Text='<%# Eval("SeniorCitizen") %>'></asp:Label></td>
                                                    <td style="width: 20px;"></td>
                                                    <td>
                                                        <asp:Label ID="Label9" runat="server" Text="Age Criteria" Font-Bold="true" Font-Size="15px"></asp:Label>
                                                    </td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="lblAgeCriteria" runat="server" Font-Size="15px" Text='<%# Eval("AgeCriteria") %>'></asp:Label>
                                                    </td>
                                                    <td colspan="6"></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="17">
                                                        <div style="height: 5px;"></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                        <center>
                                            <asp:Panel ID="pnlHeader1" runat="server" Visible="true" Width="100%" Style="margin-top: 10px; margin-bottom: 10px;">
                                                <asp:GridView ID="grdHeader1" runat="server" ShowHeader="false" AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdHeader1_RowDataBound">
                                                    <RowStyle Font-Size="15px" Height="35px" />
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ProfileID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="ProfileID" runat="server" Text='<%# Eval("ProfileID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="HeaderID1" runat="server" Text='<%# Eval("HeaderID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="RuleID1" runat="server" Text='<%# Eval("RuleID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="RuleText1" runat="server" Text='<%# Eval("RuleText") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtHeaderAmount1" runat="server" placeholder="Enter Amount" Text="0.00" Font-Size="18px"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlHeader2" runat="server" Visible="true" Width="100%" Style="margin-top: 10px; margin-bottom: 10px;">
                                                <asp:GridView ID="grdHeader2" runat="server" ShowHeader="false" ShowFooter="true" AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdHeader2_RowDataBound">
                                                    <FooterStyle HorizontalAlign="Right" Height="50px" Font-Bold="true" Font-Size="15px" />
                                                    <RowStyle Font-Size="15px" Height="35px" />
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ProfileID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="ProfileID" runat="server" Text='<%# Eval("ProfileID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="HeaderID2" runat="server" Text='<%# Eval("HeaderID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="RuleID2" runat="server" Text='<%# Eval("RuleID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="MaxAmount2" runat="server" Text='<%# Eval("MaxAmount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="RuleText2" runat="server" Text='<%# Eval("RuleText") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblRuleTextFooter2" runat="server" Style="float: left;"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtHeaderAmount2" runat="server" placeholder="Enter Amount" Text="0.00" Font-Size="18px"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="txtFooterAmount2" runat="server" Enabled="false" Font-Size="18px"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </center>
                                        <asp:Panel ID="pnlOthers" runat="server" Visible="false" Width="100%" Style="margin-top: 10px; margin-bottom: 10px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblOthers" runat="server" Font-Bold="true" Font-Size="16px" Text="Others Details *"></asp:Label>
                                                    </td>
                                                    <td style="width: 10px;"></td>
                                                    <td>:</td>
                                                    <td style="width: 10px;"></td>
                                                    <td>
                                                        <asp:Label ID="txtOthers" runat="server" placeholder="Provide Other Details here." TextMode="MultiLine" Rows="2"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <center>
                                            <asp:Panel ID="pnlHeader3" runat="server" Visible="true" Width="100%" Style="margin-bottom: 10px;">
                                                <asp:GridView ID="grdHeader3" runat="server" ShowHeader="false" AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdHeader3_RowDataBound">
                                                    <RowStyle Font-Size="15px" Height="35px" />
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ProfileID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="ProfileID" runat="server" Text='<%# Eval("ProfileID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="HeaderID3" runat="server" Text='<%# Eval("HeaderID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="RuleID3" runat="server" Text='<%# Eval("RuleID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="MaxAmount3" runat="server" Text='<%# Eval("MaxAmount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="RuleText3" runat="server" Text='<%# Eval("RuleText") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtHeaderAmount3" runat="server" placeholder="Enter Amount" Text="0.00" Font-Size="18px"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlHeader4" runat="server" Visible="true" Width="100%" Style="margin-top: 10px; margin-bottom: 10px;">
                                                <asp:GridView ID="grdHeader4" runat="server" ShowHeader="false" AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdHeader4_RowDataBound">
                                                    <RowStyle Font-Size="15px" Height="35px" />
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ProfileID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="ProfileID" runat="server" Text='<%# Eval("ProfileID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="HeaderID4" runat="server" Text='<%# Eval("HeaderID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="RuleID4" runat="server" Text='<%# Eval("RuleID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="MaxAmount4" runat="server" Text='<%# Eval("MaxAmount") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="RuleText4" runat="server" Text='<%# Eval("RuleText") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtHeaderAmount4" runat="server" placeholder="Enter Amount" Text="0.00" Font-Size="18px"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlHeader5" runat="server" Visible="true" Width="100%" Style="margin-top: 10px; margin-bottom: 5px;">
                                                <asp:GridView ID="grdHeader5" runat="server" Width="100%" AutoGenerateColumns="false" OnRowDataBound="grdHeader5_RowDataBound">
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <RowStyle HorizontalAlign="Center" Height="35px" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="ProfileID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="ProfileID" runat="server" Text='<%# Eval("ProfileID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblHead1" runat="server"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtLandlordNameAndAddress" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblHead2" runat="server"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtAddressofAccommodation" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblHead3" runat="server"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtCityName" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblHead4" runat="server"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtPANNOoftheowner" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblHead5" runat="server"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtRentAmountPM" runat="server" Text="0.00"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblHead6" runat="server"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="ddlEffectedFrom" runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblHead7" runat="server"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="txtTotalRentAmountPerAnnum" runat="server" placeholder="Amount Per Annum" Text="0.00"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlDeclaration" runat="server" Width="100%">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblDeclaration" runat="server" Font-Size="17px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label12" runat="server" Text="Place" Font-Size="17px" Font-Bold="true"></asp:Label></td>
                                                        <td><b>:</b></td>
                                                        <td>
                                                            <asp:Label ID="txtPlace" runat="server" Font-Size="17px"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="Label13" runat="server" Text="Signature" Font-Size="17px" Font-Bold="true" Style="float: right;"></asp:Label></td>
                                                        <td><b>:</b></td>
                                                        <td>
                                                            <asp:Label ID="txtSignature" runat="server" Font-Size="17px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label14" runat="server" Text="Date" Font-Size="17px" Font-Bold="true"></asp:Label></td>
                                                        <td><b>:</b></td>
                                                        <td>
                                                            <asp:Label ID="txtDate" runat="server" Font-Size="17px"></asp:Label>
                                                        </td>
                                                        <td colspan="3" style="float: right;"></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </center>
                                    </asp:Panel>
                                    <p style="page-break-after: always;"></p>
                                </ItemTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                    </asp:Panel>
                </fieldset>
            </div>
            <div style="min-height: 300px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

