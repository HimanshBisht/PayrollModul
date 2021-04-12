<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="keywords" content="K.R Mangalam,KRMangalam,KRM SMS,SMS ,Best Delhi School,Delhi School,Gurgaon School,KRM Gurgaon,KRMangalam Gurgaon,Gurgaon,Vaishali,Institute,Management,BBA,MBA" />
    <title>:: Login ::</title>
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="css/Login.css" />
</head>
<body style="background-color: teal">
    <form id="form1" class="form-signin" runat="server" defaultbutton="btnLogin">
        <asp:ScriptManager ID="sp1" runat="server"></asp:ScriptManager>
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
                <div class="container">
                    <div class="card card-container">
                        <img alt="Img" class="img-responsive" style="width: 100%" src="Images/Logo.png" />
                        <p id="profile-name" class="profile-name-card"></p>
                        <span id="reauth-email" class="reauth-email" style="color: red;">* All Fields are Mandatory</span>
                        <asp:TextBox ID="txt_login" runat="server" class="form-control" placeholder="User Name" AutoComplete="off"></asp:TextBox>
                        <asp:TextBox ID="txt_pass" runat="server" class="form-control" TextMode="Password" placeholder="Password" AutoComplete="off"></asp:TextBox>
                        <div class="row">
                            <div class="col-md-7">
                                <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="High" CaptchaChars="abcdefghijklmnpqrstuvwxyz0123456789" CaptchaFont="Calibri"
                                    CaptchaLength="6" CaptchaHeight="50" CaptchaWidth="180" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                    FontColor="#D20B0C" NoiseColor="#B1B1B1" Style="font-family: Calibri"></cc1:CaptchaControl>
                            </div>
                            <div class="col-md-5">
                                <asp:ImageButton ImageUrl="~/Images/refresh.png" class="img-responsive" ToolTip="Get New Code" runat="server" CausesValidation="false" />
                            </div>
                        </div>
                        <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control" placeholder="Enter Captcha Code" AutoComplete="off"></asp:TextBox>
                        <div id="remember" class="checkbox">
                            <label>
                                <asp:CheckBox ID="chkbxRememberMe" runat="server" Text="Remember Me" />
                            </label>
                        </div>
                        <asp:Button ID="btnLogin" runat="server" ValidationGroup="Save" Text="Login" CssClass="btn btn-lg btn-primary btn-block btn-signin" />
                        <asp:CustomValidator ErrorMessage="Invalid Captcha, Please try again." ForeColor="Red" OnServerValidate="ValidateCaptcha" ValidationGroup="Save"
                            runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_login" ValidationGroup="Save"
                            ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCaptcha" ValidationGroup="Save"
                            ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_pass" ValidationGroup="Save"
                            ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnLogin" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
</body>
</html>