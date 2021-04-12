<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/SalaryMaster.master" AutoEventWireup="true" CodeFile="NotAuthorized.aspx.cs" Inherits="NotAuthorized" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="left: 0; position: fixed; width: 100%; height: 100%; top: 0;">
                <div style="text-align: center; z-index: 10; margin: 300px auto;">
                    <br />
                    <span>
                        <asp:Label ID="lblText" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="25px" Text="Sorry, You are not authorized to access this page."></asp:Label>
                    </span>
                </div>
            </div>
            <div style="min-height: 550px;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

