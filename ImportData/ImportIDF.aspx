<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="ImportIDF.aspx.cs" Inherits="SalaryModule_ImportIDF" %>

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
                        <legend>Import Investment Declaration Form (IDF)</legend>
                        <asp:Panel ID="pnlData" runat="server">
                            <table style="margin: 5px 0 0 21px;">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlFromFinancialYear" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlFromFinancialYear" ErrorMessage="*" InitialValue="0" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEmployeeStatus" runat="server">
                                            <asp:ListItem Text="Select Employee Status" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="All Active Employees" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="All Deactive Employees" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlEmployeeStatus" InitialValue="2" ErrorMessage="*" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="flIDFImport" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnDownloadTemplate" runat="server" Text="Download IDF Template" CssClass="btn btn-default" ValidationGroup="Save" OnClick="btnDownloadTemplate_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSaveTemplate" runat="server" Text="Save IDF Template" CssClass="btn btn-default" OnClick="btnSaveTemplate_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <div style="margin: 30px;">
                        <b>Points to remember while uploading the excel file to save IDF data -</b>
                        <ul style="margin: 20px;">
                            <li><b>1-</b> Each column name is auto generated, please do not try to change any column name.</li>
                            <li><b>2-</b> Before saving the IDF template, make sure that you have downloaded the latest version of it.</li>
                            <li><b>3-</b> Do not (Add/Remove) or Interchange(Swap) any column from the downloaded template.</li>
                            <li><b>4-</b> If all columns are having blank value for any employee that means his/her IDF data is not yet stored in the system.</li>
                            <li><b>5-</b> If the amount is greater than zero in the "Others" column for any employee then the amount details must be provided in the "Others Details" column.</li>
                            <li><b>6-</b> When entering any value greater than zero in the "Rent Amount P.M." column, then the "Effected From" column must have the value ranging from 1 to 12, where 1 indicates Januray and 12 indicates December.</li>
                            <li><b>7-</b> Downloaded IDF template will be having naming convention as per the specific institution.</li>
                            <li>e.g. - IDF template downloaded for "Greater Kailash II" school will have "_GKII" keyword added in the file name.</li>
                            <li><b>8-</b> You can change downloaded file name if required, but please make sure that the changed file name must contain the same institution's keyword, as mentioned above. Not doing so, will result in exception and the data will not be saved.</li>
                        </ul>
                    </div>
                </fieldset>
            </div>
            <div style="min-height: 50px;"></div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownloadTemplate" />
            <asp:PostBackTrigger ControlID="btnSaveTemplate" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

