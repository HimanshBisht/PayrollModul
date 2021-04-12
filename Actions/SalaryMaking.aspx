<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="SalaryMaking.aspx.cs" Inherits="SalaryModule_SalaryMaking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script language="javascript" type="text/javascript">

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

        function ConfirmDeactivate() {
            if (confirm("Are you sure you want to Deactivate this Record?") == true)
                return true;
            else
                return false;
        }

        function ConfirmDeactivateAll() {
            if (confirm("Are you sure you want to Deactivate All Records?") == true)
                return true;
            else
                return false;
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
                        <legend>Salary Making</legend>

                        <asp:Panel ID="pnlData" runat="server">

                            <table style="margin: 5px 0 0 21px;">
                                <tr>

                                    <td>
                                        <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlMonth" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="MakeSalary"></asp:RequiredFieldValidator>
                                    </td>

                                    <td>
                                        <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="MakeSalary"></asp:RequiredFieldValidator>
                                    </td>

                                    <td>
                                        <asp:Button ID="btnMakeSalary" runat="server" Text="Get Records" CssClass="btn btn-default" ValidationGroup="MakeSalary" OnClick="btnMakeSalary_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                </fieldset>
            </div>
            <style type="text/css">
                .grdTable {
                    max-width: 250% !Important;
                    min-width: 250%;
                    overflow: auto;
                }

                .modalBackground {
                    background-color: Black;
                    filter: alpha(opacity=90);
                    opacity: 0.8;
                }

                .modalPopup {
                    background-color: #FFFFFF;
                    border-width: 3px;
                    border-style: solid;
                    border-color: black;
                    padding-top: 10px;
                    padding-left: 10px;
                    width: 250px;
                    height: 100%;
                }

                h1 {
                    text-align: center;
                    font-family: Tahoma, Arial, sans-serif;
                    color: #06D85F;
                    margin: 80px 0;
                }

                .box {
                    width: 100%;
                    margin: 0 auto;
                    background: rgba(255,255,255,0.2);
                    padding: 35px;
                    border: 2px solid #fff;
                    border-radius: 20px/50px;
                    background-clip: padding-box;
                    text-align: center;
                }

                .button {
                    font-size: 1em;
                    padding: 10px;
                    color: #fff;
                    border: 2px solid #06D85F;
                    border-radius: 20px/50px;
                    text-decoration: none;
                    cursor: pointer;
                    transition: all 0.3s ease-out;
                }

                    .button:hover {
                        background: #06D85F;
                    }



                .overlay:target {
                    visibility: visible;
                    opacity: 1;
                }

                .popup {
                    margin: 70px auto;
                    padding: 20px;
                    background: #fff;
                    border-radius: 5px;
                    width: 550px;
                    height: 200px;
                    position: relative;
                    transition: all 5s ease-in-out;
                }

                    .popup h3 {
                        margin-top: 0;
                        color: red;
                        font-family: Tahoma, Arial, sans-serif;
                        text-align: center;
                    }

                    .popup .close {
                        position: absolute;
                        top: 20px;
                        right: 30px;
                        transition: all 200ms;
                        font-size: 30px;
                        font-weight: bold;
                        text-decoration: none;
                        color: #333;
                    }

                        .popup .close:hover {
                            color: #06D85F;
                        }

                    .popup .content {
                        max-height: 100%;
                        overflow: auto;
                    }

                @media screen and (max-width: 700px) {

                    .popup {
                        width: 100%;
                    }
                }
            </style>
            <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnCalculateSalary" runat="server" Text="Calculate Salary" CssClass="btn btn-default" OnClientClick="return validate()" OnClick="btnCalculateSalary_Click" />
                        </td>
                        <td></td>
                        <td>
                            <asp:Button ID="btnSaveSalary" runat="server" Text="Save Salary" CssClass="btn btn-default" Visible="false" OnClientClick="return validate()" OnClick="btnSaveSalary_Click" />
                        </td>
                        <td></td>
                        <td>
                            <asp:Button ID="btnDeactivateAll" runat="server" Text="Deactivate All" CssClass="btn btn-default" Visible="false" OnClientClick="return ConfirmDeactivateAll()" OnClick="btnDeactivateAll_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
            <asp:ModalPopupExtender ID="mpRemarks" runat="server" PopupControlID="pnlPopUpRemarks" TargetControlID="lnkFake"
                BackgroundCssClass="modalBackground">
            </asp:ModalPopupExtender>

            <asp:Panel ID="pnlPopUpRemarks" runat="server" Style="position: absolute; z-index: 100001; left: 430.5px; top: 79px;">
                <div class="popup">
                    <asp:Button ID="btnClose" runat="server" Style="float: right;" CssClass="btn btn-primary" class="close" Text="X" OnClick="btnClose_Click" />
                    <center>
                        <h3>Reason for Changing</h3>
                    </center>
                    <div class="content">
                        <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="3" Width="95%" placeholder="Please Specify the Reason for Changing the Payment Mode (Mandatory Field)"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReason" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                        <div class="modal-footer">
                            <asp:Button ID="btnUpdatePaymentMode" runat="server" ValidationGroup="Submit" CssClass="btn btn-default" Text="Update Payment Mode" OnClick="btnUpdatePaymentMode_Click" />
                            <asp:Button ID="btnCancelPopUp" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="btnCancelPopUp_Click" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlDetail" runat="server" Visible="false" Style="margin-top: 20px; max-height: 600px; width: 100%; overflow: auto; margin-bottom: 20px;">

                <asp:GridView ID="grdrecord" runat="server" AutoGenerateColumns="false" CssClass="grdTable" OnRowCommand="grdrecord_RowCommand" OnRowDataBound="grdrecord_RowDataBound">
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" ForeColor="Black" Height="40px" />
                    <RowStyle HorizontalAlign="Center" />
                    <EmptyDataRowStyle ForeColor="Red" />
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
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Panel ID="pnlActions" runat="server" Visible="false">
                                    <asp:LinkButton ID="lnkDeactivate" runat="server" Text="Deactivate" OnClientClick="return ConfirmDeactivate()" CommandName="lnkDeactivate" CommandArgument='<%# Eval("ProfileID") %>'></asp:LinkButton>
                                    /
                                <asp:LinkButton ID="lnkToggle" runat="server" CommandName="lnkToggle" CommandArgument='<%# Eval("ProfileID") %>'></asp:LinkButton>
                                    /
                                    <asp:LinkButton ID="lnkPaymentMode" runat="server" CssClass="Test" CommandName="lnkPaymentMode" CommandArgument='<%# Eval("ProfileID") %>'></asp:LinkButton>
                                </asp:Panel>
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
                        <asp:TemplateField HeaderText="Employee ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeID" runat="server" Text='<%# Eval("EmployeeID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Emp Code">
                            <ItemTemplate>
                                <asp:Label ID="lblEmp_Code" runat="server" Text='<%# Eval("Emp_Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="System Number" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSystemNumber" runat="server" Text='<%# Eval("SystemNumber") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Assign Code" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAssignEmpCode" runat="server" Text='<%# Eval("AssignEmpCode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NatureID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblNatureID" runat="server" Text='<%# Eval("NatureOfEmp") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="StaffTypeID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffTypeID" runat="server" Text='<%# Eval("StaffType") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DesignationID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Designation">
                            <ItemTemplate>
                                <asp:Label ID="lblDesignationText" runat="server" Text='<%# Eval("DesignationText") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Basic">
                            <ItemTemplate>
                                <asp:Label ID="lblBasicScale" runat="server" Text='<%# Eval("BasicScale") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Change" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblChangeScale" runat="server" Text='<%# Eval("ChangeScaleText") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Effected Scale" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEffectedScale" runat="server" Text='<%# Eval("EffectedScale") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Salary Mode" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSalaryMode" runat="server" Text='<%# Eval("ModeOfSalary") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PF Deduct" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPFDeduct" runat="server" Text='<%# Eval("PFDeduct") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ESI Deduct" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblESIDeduct" runat="server" Text='<%# Eval("ESIDeduct") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Month Days">
                            <ItemTemplate>
                                <asp:Label ID="lblMonthDays" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LWP" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblLWP" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Paid Days" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblPaidDays" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pay Drawn Basic" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblPayDrawnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DA" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblDAApply" runat="server" Text='<%# Eval("DA") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DA On Basic" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblDaOnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HRA" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblHRAApply" runat="server" Text='<%# Eval("SelectHRA") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HRA On Basic" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblHraOnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Transport" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblTransportOnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Medical" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblMedicalOnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Washing" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblWashingOnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gross Revised Salary" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblGrossRevisedsalary" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ExGratia" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblExGratiaOnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Arrear Adjust" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblArearAdjust" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gross Total" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblGrossTotal" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DA For Report" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblDaForReportOnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PF" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblPFOnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DED" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblDeduction" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TDS" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblTDS" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ADV" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblAdvance" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TPT Rec" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblTPTRECOnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GIS" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblGISOnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ESI" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblESIOnBasic" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Deduction" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalDeduction" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gross Total Salary" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblGrossTotalSalary" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Effected Gross" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEffectedGrossTotalSalary" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No Record Found
                    </EmptyDataTemplate>
                </asp:GridView>
            </asp:Panel>
            <div style="min-height: 430px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

