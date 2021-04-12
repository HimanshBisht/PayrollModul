<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="EmpSalaryProfile.aspx.cs" Inherits="EmpSalaryProfile" %>

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
        function CheckLengthAddress(e) {
            var textAddress = document.getElementById("ContentPlaceHolder1_txtAddress");
            if (textAddress != null) {
                if (textAddress.value.trim().length >= 500) {
                    textAddress.value = textAddress.value.substring(0, 500);
                    CheckAddressCharacter();
                    return false;
                }
            }
        }

        function CheckAddressCharacter() {
            var textAddress = document.getElementById("ContentPlaceHolder1_txtAddress").value;
            $("#divAddress").css("display", "block");
            var Maxsize = 500;
            $("#divAddress").text(Maxsize - textAddress.trim().length + " " + "Characters Left.");
            if (textAddress == 0) {
                $("#divAddress").css("display", "none");
            }
        }

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

        function CheckLengthRemarks(e) {
            var textRemarks = document.getElementById("ContentPlaceHolder1_txtRemarks");
            if (textRemarks != null) {
                if (textRemarks.value.trim().length >= 500) {
                    textRemarks.value = textRemarks.value.substring(0, 500);
                    CheckRemarksCharacter();
                    return false;
                }
            }
        }

        function CheckRemarksCharacter() {
            var textRemarks = document.getElementById("ContentPlaceHolder1_txtRemarks").value;
            $("#divRemarks").css("display", "block");
            var Maxsize = 500;
            $("#divRemarks").text(Maxsize - textRemarks.trim().length + " " + "Characters Left.");
            if (textRemarks == 0) {
                $("#divRemarks").css("display", "none");
            }
        }

        function ConfirmDeactivate() {
            if (confirm("Are you sure you want to Deactivate this Record?") == true)
                return true;
            else
                return false;
        }

        function ConfirmActivate() {
            if (confirm("Are you sure you want to Activate again this Record?") == true)
                return true;
            else
                return false;
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div style="margin-top: 25px; height: auto;">

                <asp:Panel ID="pnlData" runat="server" Visible="true">
                    <fieldset>
                        <center>
                            <legend>Employee Salary Profile</legend>
                        </center>

                        <div>

                            <asp:Panel ID="pnlInfo" runat="server">
                                <style type="text/css">
                                    table, #rdoModeOfSalary, #rdoMaternityLeave, label {
                                        margin-left: 20px;
                                        margin-top: -16px;
                                        margin-bottom: 0px;
                                    }

                                    table, #rdoModeOfSalary, #rdoMaternityLeave, radio {
                                        margin-top: 5px;
                                    }
                                </style>
                                <center>
                                    <table>
                                        <tr>
                                            <td><b>To Update</b></td>
                                            <td style="width: 20px;"></td>
                                            <td>
                                                <asp:DropDownList ID="ddlemployee" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlemployee_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                            <td style="width: 200px;"></td>
                                            <td><b>To Activate</b></td>
                                            <td style="width: 20px;"></td>
                                            <td>
                                                <asp:DropDownList ID="ddlDeactivateEmployee" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDeactivateEmployee_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Panel ID="PnlActivateButtons" runat="server" Visible="false">
                                                    <asp:Button ID="btnActivate" runat="server" Text="Activate" CssClass="btn btn-default" OnClientClick="return ConfirmActivate()" OnClick="btnActivate_Click" />
                                                    <asp:Button ID="btnCancelActivate" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancelActivate_Click" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="8">
                                                <hr />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <center>
                                    <table>

                                        <tr>
                                            <td colspan="8">
                                                <h4>Employee Details - </h4>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Name</td>
                                            <td>
                                                <asp:TextBox ID="txtName" runat="server" placeholder="Employee Name" AutoComplete="off"></asp:TextBox>
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
                                                <asp:TextBox ID="txtDOJ" runat="server" AutoComplete="off" placeholder="Date of Joining" onpaste="return false;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalDOJ" runat="server" TargetControlID="txtDOJ"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtDOJ" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Designation</td>
                                            <td>
                                                <asp:DropDownList ID="ddlDesignation" runat="server"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="ddlDesignation" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Emp ID</td>
                                            <td>
                                                <asp:TextBox ID="txtEmployeeID" runat="server" placeholder="Enter Employee ID" AutoComplete="off"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" Enabled="True" TargetControlID="txtEmployeeID" FilterType="Custom"
                                                    FilterMode="ValidChars" ValidChars="0123456789">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Emp_Code</td>
                                            <td>
                                                <asp:TextBox ID="txtEmp_Code" runat="server" placeholder="Enter Emp_Code" AutoComplete="off"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator48" runat="server" ControlToValidate="txtEmp_Code" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Assign Code</td>
                                            <td>
                                                <asp:TextBox ID="txtEmpCode" runat="server" placeholder="Assign a New Code" AutoComplete="off"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="txtEmpCode" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>System No.</td>
                                            <td>
                                                <asp:TextBox ID="txtSystemNumber" runat="server" placeholder="Enter Emp System Number" AutoComplete="off"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" Enabled="True" TargetControlID="txtSystemNumber" FilterType="Custom"
                                                    FilterMode="ValidChars" ValidChars="0123456789">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Appointment</td>
                                            <td>
                                                <asp:DropDownList ID="ddlAppointment" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="ddlAppointment" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Grade Pay</td>
                                            <td>
                                                <asp:TextBox ID="txtGradePay" runat="server" AutoComplete="off" placeholder="Total Grade Pay"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" TargetControlID="txtGradePay" FilterType="Custom"
                                                    FilterMode="ValidChars" ValidChars="0123456789.">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtGradePay" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Emp Nature</td>
                                            <td>
                                                <asp:DropDownList ID="ddlNatureOfEmp" runat="server"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlNatureOfEmp" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Staff Type</td>
                                            <td>
                                                <asp:DropDownList ID="ddlStaffType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStaffType_SelectedIndexChanged">
                                                </asp:DropDownList>
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
                                                <asp:RequiredFieldValidator ID="rfvPFNo" runat="server" ValidationGroup="Cancel" ControlToValidate="txtPfNo" ErrorMessage="*" ForeColor="Red">
                                                </asp:RequiredFieldValidator>
                                            </td>

                                            <td style="width: 30px;"></td>
                                            <td>ESI No.</td>
                                            <td>
                                                <asp:TextBox ID="txtEsiNo" runat="server" AutoComplete="off" placeholder="Employee State Insurance No."></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvESINo" runat="server" ValidationGroup="Cancel" ControlToValidate="txtEsiNo" ErrorMessage="*" ForeColor="Red">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>PAN No.</td>
                                            <td>
                                                <asp:TextBox ID="txtPANCardNo" runat="server" AutoComplete="off" placeholder="Enter PAN Card No."></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Basic Scale</td>
                                            <td>
                                                <asp:TextBox ID="txtScale" runat="server" AutoComplete="off" placeholder="Employee Basic Scale"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" Enabled="True" TargetControlID="txtScale" FilterType="Custom"
                                                    FilterMode="ValidChars" ValidChars="0123456789.">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtScale" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Change Scale</td>
                                            <td>
                                                <asp:DropDownList ID="ddlChangeScale" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChangeScale_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server" ControlToValidate="ddlChangeScale" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <asp:Panel ID="pnlEffectedScale" runat="server" Visible="false">
                                                <td>Effected Scale</td>
                                                <td>
                                                    <asp:TextBox ID="txtEffectedScale" runat="server" AutoComplete="off" placeholder="Define Effected Basic Scale"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" Enabled="True" TargetControlID="txtEffectedScale" FilterType="Custom"
                                                        FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator45" runat="server" ControlToValidate="txtEffectedScale" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                            </asp:Panel>
                                        </tr>

                                        <tr>
                                            <td>From Date</td>
                                            <td oncontextmenu="return false">
                                                <asp:TextBox ID="txtFromDt" runat="server" AutoComplete="off" AutoPostBack="true" OnTextChanged="txtFromDt_TextChanged" placeholder="Pick From Date" onpaste="return false;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalFromDt" runat="server" TargetControlID="txtFromDt"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtFromDt" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>To Date</td>
                                            <td oncontextmenu="return false">
                                                <asp:TextBox ID="txtToDate" runat="server" AutoComplete="off" placeholder="Pick To Date" onpaste="return false;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalToDt" runat="server" TargetControlID="txtToDate"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Contract Date</td>
                                            <td oncontextmenu="return false">
                                                <asp:TextBox ID="txtContractDate" runat="server" AutoComplete="off" placeholder="Pick Contract Date" onpaste="return false;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtContractDate"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Probation Date</td>
                                            <td oncontextmenu="return false">
                                                <asp:TextBox ID="txtProbationDate" runat="server" AutoComplete="off" placeholder="Pick Probation Date" onpaste="return false;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtProbationDate"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Confirm Date</td>
                                            <td oncontextmenu="return false">
                                                <asp:TextBox ID="txtConfirmationDate" runat="server" AutoComplete="off" placeholder="Pick Confirmation Date" onpaste="return false;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtConfirmationDate"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                            </td>

                                            <td style="width: 30px;"></td>
                                            <td>Resign Date
                                            </td>
                                            <td oncontextmenu="return false">
                                                <asp:TextBox ID="txtResignDate" runat="server" AutoComplete="off" placeholder="Pick Resignation Date" onpaste="return false;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtResignDate"
                                                    Format="dd-MMM-yyyy">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>L.W.D.
                                            </td>
                                            <td oncontextmenu="return false">
                                                <asp:TextBox ID="txtLWD" runat="server" AutoComplete="off" placeholder="Pick Last Working Date" onpaste="return false;"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtLWD" Format="dd-MMM-yyyy"></asp:CalendarExtender>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Aadhar No.</td>
                                            <td>
                                                <asp:TextBox ID="txtAadharCardNo" runat="server" AutoComplete="off" placeholder="Enter Aadhar Card No."></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" Enabled="True" TargetControlID="txtAadharCardNo" FilterType="Custom"
                                                    FilterMode="ValidChars" ValidChars="0123456789">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>UAN No.</td>
                                            <td>
                                                <asp:TextBox ID="txtUANNo" runat="server" AutoComplete="off" placeholder="Universal Account No. For PF"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvUANNo" runat="server" ControlToValidate="txtUANNo" ValidationGroup="Cancel" ErrorMessage="*" ForeColor="Red">
                                                </asp:RequiredFieldValidator>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" Enabled="True" TargetControlID="txtUANNo" FilterType="Custom"
                                                    FilterMode="ValidChars" ValidChars="0123456789">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
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
                                            <td>Remarks</td>
                                            <td colspan="7">
                                                <asp:TextBox ID="txtRemarks" Width="98%" runat="server" TextMode="MultiLine" Rows="3" AutoComplete="off"
                                                    placeholder="Remarks if Any. (Max 500 Characters)" onkeyup="return CheckRemarksCharacter();"
                                                    onkeypress="return CheckLengthRemarks();" onchange="return CheckLengthRemarks();" OnPaste="return CheckLengthRemarks();">
                                                </asp:TextBox>
                                                <div id="divRemarks" style="color: Red;">
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="8">
                                                <hr />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="8">
                                                <h4>Salary Criteria - </h4>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Bus User
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBusUser" runat="server">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Incharge" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlBusUser" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>

                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Gov-Acc
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlGovAcc" runat="server">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>

                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlGovAcc" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>

                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>DA Allow
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDA" runat="server">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlDA" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>

                                            </td>

                                        </tr>

                                        <tr>
                                            <td>HRA Allow
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlHRA" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlHRA_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes With Percentage (%)" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Yes With Fixed Value" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>

                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="ddlHRA" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>

                                            <td style="width: 30px;"></td>
                                            <td>Transport Allow
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTransportAllow" runat="server">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>

                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlTransportAllow" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>

                                            </td>

                                            <td style="width: 30px;"></td>
                                            <td>Medical Allow
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlMedicalAllow" runat="server">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlMedicalAllow" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>

                                        </tr>

                                        <tr>
                                            <td>Was Allow
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlWashingAllow" runat="server">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2" Selected="True"></asp:ListItem>

                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="ddlWashingAllow" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>Ex-Gratia
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlExGratia" runat="server">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlExGratia" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>PF
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlPFDeduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPFDeduct_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes With New Scheme" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Yes With Old Scheme" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlPFDeduct" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>ESI
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlESIDeduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlESIDeduct_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlESIDeduct" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 30px;"></td>
                                            <td>GIS
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlGISDeduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGISDeduct_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2" Selected="True"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ControlToValidate="ddlGISDeduct" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </td>
                                            <asp:Panel ID="pnlSelectGIS" runat="server" Visible="false">
                                                <td style="width: 30px;"></td>
                                                <td>Select GIS
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSelectGIS" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ControlToValidate="ddlSelectGIS" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                            </asp:Panel>
                                        </tr>

                                        <tr>
                                            <td colspan="8">
                                                <hr />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <h4>Mode of Salary</h4>
                                            </td>

                                            <td colspan="7">
                                                <asp:RadioButtonList ID="rdoModeOfSalary" runat="server" RepeatDirection="Horizontal" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="rdoModeOfSalary_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True" Text="Cheque" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Bank Account" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Cash" Value="3"></asp:ListItem>
                                                </asp:RadioButtonList>

                                            </td>
                                        </tr>

                                        <asp:Panel ID="pnlBankDetails" runat="server" Visible="false">

                                            <tr>
                                                <td>Type
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlTransferType" runat="server">
                                                        <asp:ListItem Text="Select Type" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="NEFT" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Transfer" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator50" runat="server" ControlToValidate="ddlTransferType" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                                <td style="width: 30px;"></td>
                                                <td>Bank Name
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBank" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="ddlBank" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                                <td style="width: 30px;"></td>
                                                <td>Account No.
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAccountNo" runat="server" AutoComplete="off" placeholder="Bank Account No."></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" TargetControlID="txtAccountNo" FilterType="Custom"
                                                        FilterMode="ValidChars" ValidChars="0123456789">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtAccountNo" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>IFSC Code
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtIFSCCode" runat="server" AutoComplete="off" placeholder="Branch IFSC Code" Style="text-transform: uppercase;"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtIFSCCode" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                                <td style="width: 30px;"></td>
                                                <td>Address
                                                </td>
                                                <td colspan="4">
                                                    <asp:TextBox ID="txtAddress" runat="server" AutoComplete="off" Width="98%" TextMode="MultiLine" Rows="3"
                                                        placeholder="Bank Branch Address (Max 500 Characters)" onkeyup="return CheckAddressCharacter();"
                                                        onkeypress="return CheckLengthAddress();" onchange="return CheckLengthAddress();" OnPaste="return CheckLengthAddress();">
                                                    </asp:TextBox>
                                                    <div id="divAddress" style="color: Red;">
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" Display="Dynamic" ControlToValidate="txtAddress" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </asp:Panel>

                                        <tr>
                                            <td colspan="8">
                                                <hr />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <h4>Maternity Leave</h4>
                                            </td>

                                            <td colspan="7">
                                                <asp:RadioButtonList ID="rdoMaternityLeave" runat="server" RepeatDirection="Horizontal" Width="15%" AutoPostBack="true" OnSelectedIndexChanged="rdoMaternityLeave_SelectedIndexChanged">
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>

                                        <asp:Panel ID="pnlMaternityLeave" runat="server" Visible="false">

                                            <tr oncontextmenu="return false">
                                                <td>From Date
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromMaternity" runat="server" AutoComplete="off" AutoPostBack="true" OnTextChanged="txtFromMaternity_TextChanged" placeholder="Pick From Date" onpaste="return false;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalFromMaternity" runat="server" TargetControlID="txtFromMaternity"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ControlToValidate="txtFromMaternity" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                                <td style="width: 30px;"></td>
                                                <td>To Date</td>
                                                <td>
                                                    <asp:TextBox ID="txtToMaternity" runat="server" AutoComplete="off" placeholder="Pick To Date" onpaste="return false;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalToMaternity" runat="server" TargetControlID="txtToMaternity"
                                                        Format="dd-MMM-yyyy">
                                                    </asp:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ControlToValidate="txtToMaternity" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="3"></td>
                                            </tr>
                                        </asp:Panel>

                                        <tr>
                                            <td colspan="8">
                                                <hr />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <h4>Reimbursement</h4>
                                            </td>

                                            <td colspan="7">
                                                <asp:RadioButtonList ID="rdoreimbursement" runat="server" Width="15%" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdoreimbursement_SelectedIndexChanged">
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="2" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>

                                        <asp:Panel ID="pnlReimbursement" runat="server" Visible="false">
                                            <tr>
                                                <td>Reimbursement For
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReimbursementFor1" runat="server" AutoComplete="off"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ControlToValidate="txtReimbursementFor1" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                                <td style="width: 30px;"></td>
                                                <td>Value
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReimbursementValue1" runat="server" AutoComplete="off" placeholder="Define Reimbursement Amount"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="txtReimbursementValue1" FilterType="Custom"
                                                        FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ControlToValidate="txtReimbursementValue1" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="3"></td>
                                            </tr>

                                            <tr>
                                                <td>Reimbursement For
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReimbursementFor2" runat="server" AutoComplete="off"></asp:TextBox>
                                                </td>
                                                <td style="width: 30px;"></td>
                                                <td>Value
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReimbursementValue2" runat="server" AutoComplete="off" placeholder="Define Reimbursement Amount"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True" TargetControlID="txtReimbursementValue2" FilterType="Custom"
                                                        FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </asp:FilteredTextBoxExtender>

                                                </td>
                                                <td colspan="3"></td>
                                            </tr>

                                            <tr>
                                                <td>Reimbursement For
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReimbursementFor3" runat="server" AutoComplete="off"></asp:TextBox>
                                                </td>
                                                <td style="width: 30px;"></td>
                                                <td>Value
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReimbursementValue3" runat="server" AutoComplete="off" placeholder="Define Reimbursement Amount"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True" TargetControlID="txtReimbursementValue3" FilterType="Custom"
                                                        FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </asp:FilteredTextBoxExtender>

                                                </td>
                                                <td colspan="3"></td>
                                            </tr>

                                            <tr>
                                                <td>Reimbursement For
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReimbursementFor4" runat="server" AutoComplete="off"></asp:TextBox>
                                                </td>
                                                <td style="width: 30px;"></td>
                                                <td>Value
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReimbursementValue4" runat="server" AutoComplete="off" placeholder="Define Reimbursement Amount"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True" TargetControlID="txtReimbursementValue4" FilterType="Custom"
                                                        FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                                <td colspan="3"></td>
                                            </tr>

                                            <tr>
                                                <td>Reimbursement For
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReimbursementFor5" runat="server" AutoComplete="off"></asp:TextBox>
                                                </td>
                                                <td style="width: 30px;"></td>
                                                <td>Value
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReimbursementValue5" runat="server" AutoComplete="off" placeholder="Define Reimbursement Amount"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True" TargetControlID="txtReimbursementValue5" FilterType="Custom"
                                                        FilterMode="ValidChars" ValidChars="0123456789.">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                                <td colspan="3"></td>
                                            </tr>
                                        </asp:Panel>

                                    </table>
                                </center>
                            </asp:Panel>
                        </div>

                        <asp:Panel ID="pnlButtons" runat="server" Style="margin-top: 20px;">
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnNext_Click" />
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

                <asp:Panel ID="pnlDetail" runat="server" Visible="false">
                    <fieldset>
                        <center>
                            <legend>Employee Summary Preview</legend>
                        </center>

                        <table style="margin-top: 0px" width="100%">
                            <tr>
                                <td colspan="11">
                                    <h4>Employee Details - </h4>
                                </td>
                            </tr>

                            <tr>
                                <td>Name
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblEmpName" runat="server"></asp:Label>

                                </td>
                                <td style="width: 20px;"></td>
                                <td>DOB</td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblDOB" runat="server"></asp:Label>

                                </td>
                                <td style="width: 20px;"></td>
                                <td>DOJ</td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblDOJ" runat="server"></asp:Label>

                                </td>

                            </tr>

                            <tr>

                                <td>Grade Pay</td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblTotalGradePay" runat="server"></asp:Label>

                                </td>
                                <td style="width: 20px;"></td>
                                <td>Basic Scale</td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblTotalBasicScale" runat="server"></asp:Label>

                                </td>
                                <td style="width: 20px;"></td>
                                <td>Mode of Salary</td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblModeOfSalary" runat="server"></asp:Label>

                                </td>

                            </tr>

                            <asp:Panel ID="pnlAccountInfo" runat="server" Visible="false">
                                <tr>
                                    <td>Bank Name</td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblBankName" runat="server"></asp:Label>

                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>Account No.</td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblAccountNo" runat="server"></asp:Label>

                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>IFSC CODE</td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblIFSCCode" runat="server"></asp:Label>

                                    </td>
                                </tr>
                            </asp:Panel>

                            <tr>
                                <td colspan="11">
                                    <hr />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="11">
                                    <h4>Allowance - </h4>
                                </td>
                            </tr>

                            <asp:Panel ID="pnlDAAllow" runat="server" Visible="false">
                                <tr>
                                    <td>DA (%)</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalDA" runat="server" AutoComplete="off" Text="0" AutoPostBack="true" placeholder="Default Value is 0" OnTextChanged="txtTotalDA_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True" TargetControlID="txtTotalDA" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtTotalDA" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>DA Value</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalDaValue" runat="server" AutoComplete="off" Text="0" AutoPostBack="true" ReadOnly="true" placeholder="Default Value is 0"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True" TargetControlID="txtTotalDaValue" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ControlToValidate="txtTotalDaValue" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td colspan="3"></td>

                                </tr>
                            </asp:Panel>

                            <asp:Panel ID="pnlHRAAllow" runat="server" Visible="false">
                                <tr>

                                    <td>HRA (%)</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalHRA" runat="server" AutoComplete="off" Text="0" AutoPostBack="true" placeholder="Default Value is 0" OnTextChanged="txtTotalHRA_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True" TargetControlID="txtTotalHRA" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtTotalHRA" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>HRA Value</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalHRAValue" runat="server" AutoComplete="off" Text="0" AutoPostBack="true" OnTextChanged="txtTotalHRAValue_TextChanged" ReadOnly="true" placeholder="Default Value is 0"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True" TargetControlID="txtTotalHRAValue" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtTotalHRAValue" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td colspan="3"></td>
                                </tr>
                            </asp:Panel>

                            <asp:Panel ID="pnlMedicalAllow" runat="server" Visible="false">
                                <tr>
                                    <td>Medical Allow</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalMedicalAllow" runat="server" AutoComplete="off" Text="0" AutoPostBack="true" placeholder="Default Value is 0" OnTextChanged="txtTotalMedicalAllow_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" TargetControlID="txtTotalMedicalAllow" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtTotalMedicalAllow" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td colspan="7"></td>
                                </tr>
                            </asp:Panel>

                            <asp:Panel ID="pnlTransportAllow" runat="server" Visible="false">
                                <tr>
                                    <td>Transport Allow</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalTransportAllow" runat="server" AutoComplete="off" Text="0" AutoPostBack="true" placeholder="Default Value is 0" OnTextChanged="txtTotalTransportAllow_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True" TargetControlID="txtTotalTransportAllow" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtTotalTransportAllow" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td colspan="7"></td>
                                </tr>
                            </asp:Panel>

                            <asp:Panel ID="pnlWashingAllow" runat="server" Visible="false">
                                <tr>
                                    <td>Washing Allow</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalWashingAllow" runat="server" AutoComplete="off" Text="0" AutoPostBack="true" placeholder="Default Value is 0" OnTextChanged="txtTotalWashingAllow_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True" TargetControlID="txtTotalWashingAllow" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="txtTotalWashingAllow" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td colspan="7"></td>
                                </tr>
                            </asp:Panel>

                            <asp:Panel ID="pnlExGratiaAllow" runat="server" Visible="false">
                                <tr>
                                    <td>Ex- Gratia</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalExGratia" runat="server" AutoComplete="off" Text="0" AutoPostBack="true" placeholder="Default Value is 0" OnTextChanged="txtTotalExGratia_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txtTotalExGratia" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ControlToValidate="txtTotalExGratia" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td colspan="7"></td>
                                </tr>
                            </asp:Panel>

                            <tr>

                                <td><b>Gross Allowance</b></td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblGrossAllowance" runat="server" Font-Size="Medium"></asp:Label>

                                </td>
                                <td style="width: 20px;"></td>

                                <td><b>Gross Total</b></td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblGrossTotal" runat="server" Font-Size="Medium"></asp:Label>

                                </td>
                                <td colspan="4"></td>


                            </tr>

                            <tr>
                                <td colspan="11">
                                    <hr />

                                </td>
                            </tr>

                            <tr>
                                <td colspan="11">
                                    <h4>Deductions - </h4>
                                </td>
                            </tr>

                            <asp:Panel ID="pnlPF" runat="server" Visible="false">
                                <tr>
                                    <td>PF</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalPF" runat="server" AutoComplete="off" AutoPostBack="true" placeholder="Default Value is 0" OnTextChanged="txtTotalPF_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True" TargetControlID="txtTotalPF" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ControlToValidate="txtTotalPF" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>

                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td colspan="7"></td>
                                </tr>
                            </asp:Panel>

                            <asp:Panel ID="pnlESI" runat="server" Visible="false">
                                <tr>
                                    <td>ESI</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalESI" runat="server" AutoComplete="off" AutoPostBack="true" placeholder="Default Value is 0" OnTextChanged="txtTotalESI_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True" TargetControlID="txtTotalESI" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ControlToValidate="txtTotalESI" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>

                                    </td>
                                    <td colspan="8"></td>
                                </tr>
                            </asp:Panel>

                            <asp:Panel ID="pnlGIS" runat="server" Visible="false">
                                <tr>
                                    <td>GIS</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalGIS" runat="server" AutoComplete="off" AutoPostBack="true" placeholder="Default Value is 0" OnTextChanged="txtTotalGIS_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True" TargetControlID="txtTotalGIS" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="txtTotalGIS" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>

                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td colspan="7"></td>
                                </tr>
                            </asp:Panel>

                            <asp:Panel ID="pnlTransportRecovery" runat="server">
                                <tr>
                                    <td>TPT Recovery</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTotalTPTRec" runat="server" AutoComplete="off" AutoPostBack="true" placeholder="Default Value is 0" OnTextChanged="txtTotalTPTRec_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" Enabled="True" TargetControlID="txtTotalTPTRec" FilterType="Custom"
                                            FilterMode="ValidChars" ValidChars="0123456789.">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator47" runat="server" ControlToValidate="txtTotalTPTRec" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>

                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td colspan="7"></td>
                                </tr>
                            </asp:Panel>

                            <tr>

                                <td><b>Gross Deduction</b></td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblGrossDeduction" runat="server" Font-Size="Medium"></asp:Label>
                                </td>
                                <td style="width: 20px;"></td>
                                <td><b>Net Salary</b></td>
                                <td>:</td>
                                <td>
                                    <asp:Label ID="lblNetSalary" runat="server" Font-Size="Medium"></asp:Label>

                                </td>
                                <td colspan="4"></td>
                            </tr>

                            <asp:Panel ID="pnlShowReimbursement" runat="server" Visible="false">

                                <tr>
                                    <td colspan="11">
                                        <hr />
                                    </td>
                                </tr>

                                <tr>
                                    <td colspan="11">
                                        <h4>Reimbursement - </h4>
                                    </td>
                                </tr>

                                <asp:Panel ID="pnlReimbursement1" runat="server" Visible="false">

                                    <tr>

                                        <td>Reimbursement For
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblTotalReimbursement1" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 20px;"></td>
                                        <td>Value
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblTotalReimbursementValue1" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="4"></td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlReimbursement2" runat="server" Visible="false">

                                    <tr>

                                        <td>Reimbursement For
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblTotalReimbursement2" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 20px;"></td>
                                        <td>Value
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblTotalReimbursementValue2" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="4"></td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlReimbursement3" runat="server" Visible="false">

                                    <tr>

                                        <td>Reimbursement For
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblTotalReimbursement3" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 20px;"></td>
                                        <td>Value
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblTotalReimbursementValue3" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="4"></td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlReimbursement4" runat="server" Visible="false">

                                    <tr>

                                        <td>Reimbursement For
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblTotalReimbursement4" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 20px;"></td>
                                        <td>Value
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblTotalReimbursementValue4" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="4"></td>
                                    </tr>
                                </asp:Panel>

                                <asp:Panel ID="pnlReimbursement5" runat="server" Visible="false">

                                    <tr>

                                        <td>Reimbursement For
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblTotalReimbursement5" runat="server"></asp:Label>
                                        </td>
                                        <td style="width: 20px;"></td>
                                        <td>Value
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:Label ID="lblTotalReimbursementValue5" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="4"></td>
                                    </tr>
                                </asp:Panel>

                                <tr>

                                    <td><b>Total</b></td>
                                    <td>:</td>
                                    <td>
                                        <asp:Label ID="lblGrossReimbursement" runat="server" Font-Size="Medium"></asp:Label>

                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td colspan="7"></td>
                                </tr>
                            </asp:Panel>

                            <tr>
                                <td colspan="11">
                                    <hr />

                                </td>
                            </tr>

                            <tr>
                                <td colspan="11">
                                    <h4>Salary Status - </h4>
                                </td>
                            </tr>

                            <tr>
                                <td>Select Status                                   
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="ddlSalaryStatus" runat="server">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Hold" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorDDlSalaryStatus" runat="server" ControlToValidate="ddlSalaryStatus" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>

                                </td>
                                <td style="width: 20px;"></td>
                                <td colspan="7"></td>
                            </tr>

                            <tr>
                                <td colspan="11">
                                    <br />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="11">
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <center>
                            <table>
                                <tr>
                                    <asp:Panel ID="pnlUpdateButtons" runat="server">
                                        <td>
                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-default" ValidationGroup="Submit" OnClick="btnSubmit_Click" />
                                        </td>
                                        <td style="width: 5px;"></td>
                                        <td>
                                            <asp:Button ID="btnDeactivate" runat="server" CssClass="btn btn-default" Text="Update and Deactivate" ToolTip="If You Deactivate any Employee, this will be disappear from the List." ValidationGroup="Deactivate" OnClientClick="return ConfirmDeactivate()" OnClick="btnDeactivate_Click" />
                                        </td>
                                        <td style="width: 5px;"></td>
                                    </asp:Panel>
                                    <td>
                                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-primary" OnClick="btnBack_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </fieldset>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

