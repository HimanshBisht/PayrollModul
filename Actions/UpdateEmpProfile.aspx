<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="UpdateEmpProfile.aspx.cs" Inherits="UpdateEmpProfile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="MyProcess" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="5">
        <ProgressTemplate>
            <div style="left: 0; position: fixed; width: 100%; height: 100%; z-index: 9999999; top: 0; background: rgba(0,0,0,0.5);">
                <div style="text-align: center; z-index: 10; margin: 300px auto;">
                    <img alt="img" src="../images/loading-gif-animation.gif" style="height: 100px; width: 100px;" /><br />
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
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script type="text/javascript">
        function CheckHomeLengthAddress(e) {
            var textHomeAddress = document.getElementById("ContentPlaceHolder1_txtHomeAddress");
            if (textHomeAddress != null) {
                if (textHomeAddress.value.trim().length >= 500) {
                    textHomeAddress.value = textHomeAddress.value.substring(0, 500);
                    CheckHomeAddressCharacter();
                    return false;
                }
            }
        }

        function CheckHomeAddressCharacter() {
            var textHomeAddress = document.getElementById("ContentPlaceHolder1_txtHomeAddress").value;
            $("#divHomeAddress").css("display", "block");
            var Maxsize = 500;
            $("#divHomeAddress").text(Maxsize - textHomeAddress.trim().length + " " + "Characters Left.");
            if (textHomeAddress == 0) {
                $("#divHomeAddress").css("display", "none");
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="margin-top: 25px; height: auto;">
                <asp:Panel ID="pnlData" runat="server" Visible="true">
                    <fieldset>
                        <center>
                            <legend>Update Employee Profile</legend>
                        </center>
                        <center>
                            <table>
                                <tr>
                                    <td><b>To Update</b></td>
                                    <td style="width: 30px;"></td>
                                    <td>
                                        <asp:DropDownList ID="ddlemployee" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlemployee_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlemployee" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <asp:Panel ID="pnlInfo" runat="server" Visible="false">
                            <center>
                                <table style="margin-top: 0px">
                                    <tr>
                                        <td colspan="8">
                                            <h4>Employee Details - </h4>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Name</td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" placeholder="Employee Name" ReadOnly="true" AutoComplete="off"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>Father/Husband</td>
                                        <td>
                                            <asp:TextBox ID="txtFatherORHusbandName" runat="server" placeholder="Father / Husband Name" AutoComplete="off"></asp:TextBox>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>Email ID</td>
                                        <td>
                                            <asp:TextBox ID="txtEmailID" runat="server" placeholder="Email Address" AutoComplete="off"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmailID" ErrorMessage="?" Font-Bold="true" ForeColor="Red" ToolTip="Invalid Email" ValidationGroup="Save" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                            </asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mobile No.</td>
                                        <td>
                                            <asp:TextBox ID="txtMobileNo" runat="server" placeholder="10 Digit Mobile No." MaxLength="10" AutoComplete="off"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" Enabled="True" TargetControlID="txtMobileNo" FilterType="Custom"
                                                FilterMode="ValidChars" ValidChars="0123456789">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>Gender</td>
                                        <td>
                                            <asp:DropDownList ID="ddlGender" runat="server">
                                                <asp:ListItem Text="Select Gender" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Female" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator49" runat="server" ControlToValidate="ddlGender" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>DOB</td>
                                        <td oncontextmenu="return false">
                                            <asp:TextBox ID="txtDOB" runat="server" AutoComplete="off" AutoPostBack="true" placeholder="Date of Birth" Text="01-jan-1900" onpaste="return false;" OnTextChanged="txtDOB_TextChanged"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalDOB" runat="server" TargetControlID="txtDOB"
                                                Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtDOB" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Age</td>
                                        <td>
                                            <asp:TextBox ID="txtEmpAge" runat="server" AutoComplete="off" placeholder="Age As On Date" ReadOnly="true" onpaste="return false;">
                                            </asp:TextBox>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>DOJ</td>
                                        <td oncontextmenu="return false">
                                            <asp:TextBox ID="txtDOJ" runat="server" ReadOnly="true" AutoComplete="off" placeholder="Date of Joining" onpaste="return false;"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalDOJ" runat="server" TargetControlID="txtDOJ"
                                                Format="dd-MMM-yyyy">
                                            </asp:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtDOJ" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>Designation</td>
                                        <td>
                                            <asp:DropDownList ID="ddlDesignation" runat="server" ReadOnly="true" Enabled="false"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="ddlDesignation" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Emp_Code</td>
                                        <td>
                                            <asp:TextBox ID="txtEmp_Code" runat="server" ReadOnly="true" placeholder="Enter Emp_Code" AutoComplete="off"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator48" runat="server" ControlToValidate="txtEmp_Code" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>Appointment</td>
                                        <td>
                                            <asp:DropDownList ID="ddlAppointment" runat="server" ReadOnly="true" Enabled="false"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="ddlAppointment" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>Emp Nature</td>
                                        <td>
                                            <asp:DropDownList ID="ddlNatureOfEmp" runat="server" ReadOnly="true" Enabled="false"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlNatureOfEmp" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>PAN No.</td>
                                        <td>
                                            <asp:TextBox ID="txtPANCardNo" runat="server" AutoComplete="off" placeholder="Enter PAN Card No."></asp:TextBox>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>Staff Type</td>
                                        <td>
                                            <asp:DropDownList ID="ddlStaffType" runat="server" ReadOnly="true" Enabled="false"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="ddlStaffType" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <asp:Panel ID="pnlSubjectFor" runat="server" Visible="false">
                                            <td>Subject For</td>
                                            <td>
                                                <asp:DropDownList ID="ddlSubjects" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="ddlSubjects" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </asp:Panel>
                                    </tr>
                                    <tr>
                                        <td>PF No.</td>
                                        <td>
                                            <asp:TextBox ID="txtPfNo" runat="server" AutoComplete="off" placeholder="Provident Fund No."></asp:TextBox>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>UAN No.</td>
                                        <td>
                                            <asp:TextBox ID="txtUANNo" runat="server" AutoComplete="off" placeholder="Universal Account No. For PF"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" Enabled="True" TargetControlID="txtUANNo" FilterType="Custom"
                                                FilterMode="ValidChars" ValidChars="0123456789">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 30px;"></td>
                                        <td>ESI No.</td>
                                        <td>
                                            <asp:TextBox ID="txtEsiNo" runat="server" AutoComplete="off" placeholder="Employee State Insurance No."></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Aadhar No.</td>
                                        <td>
                                            <asp:TextBox ID="txtAadharCardNo" runat="server" AutoComplete="off" placeholder="Enter Aadhar Card No."></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" Enabled="True" TargetControlID="txtAadharCardNo" FilterType="Custom"
                                                FilterMode="ValidChars" ValidChars="0123456789">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td colspan="6"></td>
                                    </tr>
                                    <tr>
                                        <td>Address</td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txtHomeAddress" Width="98%" runat="server" TextMode="MultiLine" Rows="3" AutoComplete="off"
                                                placeholder="Define Present / Permanent Home Address. (Max 500 Characters)" onkeyup="return CheckHomeAddressCharacter();"
                                                onkeypress="return CheckHomeLengthAddress();" onchange="return CheckHomeLengthAddress();" OnPaste="return CheckHomeLengthAddress();">
                                            </asp:TextBox>
                                            <div id="divHomeAddress" style="color: Red;">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnUpdate_Click" />
                                        </td>
                                        <td style="width: 10px;"></td>
                                        <td>
                                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </asp:Panel>
                    </fieldset>
                </asp:Panel>
            </div>
            <div style="min-height: 420px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

