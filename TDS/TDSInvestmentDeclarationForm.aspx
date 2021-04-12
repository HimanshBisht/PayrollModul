<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="TDSInvestmentDeclarationForm.aspx.cs" Inherits="SalaryModule_TDSInvestmentDeclarationForm" %>

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
                        </h4>
                    </span>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script type="text/javascript">
        function ConfirmDeactivate() {
            if (confirm("Are you sure you want to Deactivate this Record?") == true)
                return true;
            else
                return false;
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 25px; height: auto;">
                <fieldset>
                    <center>
                        <legend>Investment Declaration Form (IDF)</legend>

                        <asp:Panel ID="pnlSelect" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="GetForm"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlemployee" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlemployee" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="GetForm"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGetForm" runat="server" Text="Get IDF Form" CssClass="btn btn-default" ValidationGroup="GetForm" OnClick="btnGetForm_Click" />
                                        <asp:Button ID="btnReset" Text="Reset" runat="server" CssClass="btn btn-primary" OnClick="btnReset_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>

                    <asp:Panel ID="pnlGetData" runat="server" Visible="false">
                        <center>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHead1" runat="server" Font-Bold="true" Font-Size="18px" Font-Underline="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblHead2" runat="server" Font-Bold="true" Font-Size="18px" Font-Underline="true"></asp:Label></h4>
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
                                        <asp:Label ID="lblEmp_Code" runat="server" Font-Size="15px"></asp:Label>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Name" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                    <td style="width: 10px;"></td>
                                    <td>:</td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:Label ID="lblName" runat="server" Font-Size="15px"></asp:Label>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Gender" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                    <td style="width: 10px;"></td>
                                    <td>:</td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:Label ID="lblGender" runat="server" Font-Size="15px"></asp:Label>
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
                                        <asp:Label ID="lblDesignation" runat="server" Font-Size="15px"></asp:Label>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="ContactNo" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                    <td style="width: 10px;"></td>
                                    <td>:</td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:Label ID="lblContactNo" runat="server" Font-Size="15px"></asp:Label>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Email ID" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                    <td style="width: 10px;"></td>
                                    <td>:</td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:Label ID="lblEmailID" runat="server" Font-Size="15px"></asp:Label>
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
                                        <asp:Label ID="lblPanNo" runat="server" Font-Size="15px"></asp:Label>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="DOB" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                    <td style="width: 10px;"></td>
                                    <td>:</td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:Label ID="lblDOB" runat="server" Font-Size="15px"></asp:Label>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="Age As on Date" Font-Bold="true" Font-Size="15px"></asp:Label></td>
                                    <td style="width: 10px;"></td>
                                    <td>:</td>
                                    <td style="width: 10px;"></td>
                                    <td>
                                        <asp:Label ID="lblEmpAge" runat="server" Font-Size="15px"></asp:Label>
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
                                        <asp:Label ID="lblSeniorCitizen" runat="server" Font-Size="15px"></asp:Label></td>
                                    <td style="width: 20px;"></td>
                                    <asp:Panel ID="pnlAge" runat="server" Visible="false">
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Text="Age Criteria" Font-Bold="true" Font-Size="15px"></asp:Label>
                                        </td>
                                        <td style="width: 10px;"></td>
                                        <td>:</td>
                                        <td style="width: 10px;"></td>
                                        <td>
                                            <asp:Label ID="lblAgeCriteria" runat="server" Font-Size="15px"></asp:Label>
                                        </td>
                                    </asp:Panel>
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
                                    <RowStyle Font-Size="15px" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
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
                                        <asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="MaxAmount1" runat="server" Text='<%# Eval("MaxAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="RuleText1" runat="server" Text='<%# Eval("RuleText") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtHeaderAmount1" runat="server" placeholder="Enter Amount" Text="0" Font-Size="18px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="txtHeaderAmount1" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvHeaderAmount1" runat="server" ControlToValidate="txtHeaderAmount1" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                            <asp:Panel ID="pnlHeader2" runat="server" Visible="true" Width="100%" Style="margin-top: 10px; margin-bottom: 10px;">
                                <asp:GridView ID="grdHeader2" runat="server" ShowHeader="false" AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdHeader2_RowDataBound">
                                    <FooterStyle HorizontalAlign="Center" Height="50px" Font-Bold="true" Font-Size="15px" />
                                    <RowStyle Font-Size="15px" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
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
                                                <asp:Label ID="lblRuleTextFooter2" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtHeaderAmount2" runat="server" placeholder="Enter Amount" Text="0" Font-Size="18px" AutoPostBack="true" OnTextChanged="txtHeaderAmount2_TextChanged"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" TargetControlID="txtHeaderAmount2" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvHeaderAmount2" runat="server" ControlToValidate="txtHeaderAmount2" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtFooterAmount2" runat="server" Enabled="false" Font-Size="18px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredFooterAmount2" runat="server" Enabled="True" TargetControlID="txtFooterAmount2" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvFooterAmount2" runat="server" ControlToValidate="txtFooterAmount2" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
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
                                        <asp:TextBox ID="txtOthers" runat="server" placeholder="Provide Other Details here." TextMode="MultiLine" Rows="2" Width="200%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <center>
                            <asp:Panel ID="pnlHeader3" runat="server" Visible="true" Width="100%" Style="margin-bottom: 10px;">
                                <asp:GridView ID="grdHeader3" runat="server" ShowHeader="false" AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdHeader3_RowDataBound">
                                    <RowStyle Font-Size="15px" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
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
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtHeaderAmount3" runat="server" placeholder="Enter Amount" Text="0" Font-Size="18px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" TargetControlID="txtHeaderAmount3" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvHeaderAmount3" runat="server" ControlToValidate="txtHeaderAmount3" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                            <asp:Panel ID="pnlHeader4" runat="server" Visible="true" Width="100%" Style="margin-top: 10px; margin-bottom: 10px;">
                                <asp:GridView ID="grdHeader4" runat="server" ShowHeader="false" AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdHeader4_RowDataBound">
                                    <RowStyle Font-Size="15px" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
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
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtHeaderAmount4" runat="server" placeholder="Enter Amount" Text="0" Font-Size="18px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" TargetControlID="txtHeaderAmount4" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvHeaderAmount4" runat="server" ControlToValidate="txtHeaderAmount4" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                            <asp:Panel ID="pnlHeader5" runat="server" Visible="true" Width="100%" Style="margin-top: 10px; margin-bottom: 5px;">
                                <asp:GridView ID="grdHeader5" runat="server" Width="100%" AutoGenerateColumns="false" OnRowDataBound="grdHeader5_RowDataBound">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <RowStyle HorizontalAlign="Left" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHead1" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtLandlordNameAndAddress" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHead2" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAddressofAccommodation" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHead3" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCityName" runat="server" TextMode="MultiLine" Rows="3" Width="100px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHead4" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPANNOoftheowner" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHead5" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRentAmountPM" runat="server" Text="0" Width="70px" AutoPostBack="true" OnTextChanged="txtRentAmountPM_TextChanged"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" TargetControlID="txtRentAmountPM" FilterType="Custom" FilterMode="ValidChars" ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHead6" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlEffectedFrom" runat="server" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlEffectedFrom_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="January" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="February" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="March" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="April" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="June" Value="6"></asp:ListItem>
                                                    <asp:ListItem Text="July" Value="7"></asp:ListItem>
                                                    <asp:ListItem Text="August" Value="8"></asp:ListItem>
                                                    <asp:ListItem Text="September" Value="9"></asp:ListItem>
                                                    <asp:ListItem Text="October" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="November" Value="11"></asp:ListItem>
                                                    <asp:ListItem Text="December" Value="12"></asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblHead7" runat="server"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotalRentAmountPerAnnum" runat="server" placeholder="Amount Per Annum" Text="0" Enabled="false" ReadOnly="true" Width="100px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                            <asp:Panel ID="pnlDeclaration" runat="server" Visible="false" Width="100%">
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
                                <table>
                                    <tr>
                                        <td><b>Place</b></td>
                                        <td><b>:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtPlace" runat="server" AutoComplete="off" BorderStyle="None"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtPlace" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 550px;"></td>
                                        <td><b>Signature</b></td>
                                        <td><b>:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtSignature" runat="server" placeholder="Enter Name here." AutoComplete="off" BorderStyle="None"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtSignature" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Date</b></td>
                                        <td><b>:</b></td>
                                        <td oncontextmenu="return false">
                                            <asp:TextBox ID="txtDate" runat="server" placeholder="Pick Date" AutoComplete="off" BorderStyle="None"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalDate" runat="server" TargetControlID="txtDate" Format="dd MMM yyyy"></asp:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtDate" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 550px;"></td>
                                        <td colspan="3"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </center>
                    </asp:Panel>
                    </center>
                    <center>
                        <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="Save Details" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnSave_Click" />
                                        <%-- <asp:Button ID="btnDeactivate" runat="server" Visible="false" Text="Deactivate" CssClass="btn btn-default" OnClientClick="return ConfirmDeactivate()" OnClick="btnDeactivate_Click" />--%>
                                        <asp:Button ID="btnPrintSelected" runat="server" CssClass="btn btn-default" Visible="false" Text="Print OR Save Form" OnClick="btnPrintSelected_Click" />
                                        <asp:Button ID="btnCancelForm" Text="Cancel" CssClass="btn btn-primary" runat="server" OnClick="btnCancelForm_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <div style="min-height: 380px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

