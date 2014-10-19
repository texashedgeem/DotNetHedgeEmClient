<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_facebook_canvas.aspx.cs"
    Inherits="frm_facebook_canvas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/resources/css/online/facebook_canvas.css" rel="stylesheet" type="text/css" />
    <link href="/resources/css/online/hedgeem_buttons.css" rel="stylesheet" type="text/css" />
    <script type='text/javascript' src='https://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js'></script>
    <script src="../resources/javascript/facebook_canvas.js" type="text/javascript"></script>
    <script src="../resources/javascript/preload_images_with_progressbar.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://connect.facebook.net/en_US/all.js"></script>
        <script type="text/javascript">
            function disableButton(button, resetDelay) {
                button.oldonclick = button.onclick;
                document.getElementById(button.id).style.opacity = "0.7";
                button.onclick = noClick; //new Function(“return false;”;
                setTimeout("enableButton('" + button.id + "');", resetDelay);
            }
            function noClick() {
                return false;
            }
            function enableButton(buttonId) {
                var button = document.getElementById(buttonId);
                if (button != null) {
                    button.onclick = button.oldonclick;
                }
            }
    </script>
</head>
<body>
    <%--<!-- Div to show Progress Bar while images are loading on Page_Load -->
    <div id="progressbar">
        <p id="progressbar_content">
        </p>
        <div id="div_progress" class="progressbar">
            <div id="div_progress_bar" class="bar">
            </div>
            <div id="div_percent" class="bar">
            </div>
        </div>
    </div>--%>
    <div id="fb-root">
    </div>
    <script type="text/javascript">

        // Load the SDK Asynchronously
        (function (d) {
            var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement('script'); js.id = id; js.async = true;
            js.src = "https://connect.facebook.net/en_US/all.js";
            ref.parentNode.insertBefore(js, ref);
        } (document));

         
    </script>
    <form id="form1" runat="server">
    <div id="wrapper">
        <asp:Label ID="lb_alert" runat="server" Text="" ForeColor="Red"></asp:Label>
        <div id="table_background">
            <!-- Backdrop for game information -->
            <div id="hedgeem_table_info_bar_background">
            </div>
            <!-- Image to show Casino logo -->
            <div id="table_logo">
            </div>
            <!-- Image to show house chips -->
            <div id="house_chips">
            </div>
            <!-- social_media_container -->
            <div id="social_media_container">
                <!-- Facebook Invite Friends -->
                <a href="#" onclick="Facebook_Invite_Friends()" class="fb-invite"></a>
                <!-- Twitter -->
                <div id="twitter_container">
                    <a href="https://twitter.com/TexasHedgeEm" class="twitter-follow-button" data-show-count="false"
                        data-show-screen-name="false">Follow @TexasHedgeEm</a>
                    <script>                        !function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + '://platform.twitter.com/widgets.js'; fjs.parentNode.insertBefore(js, fjs); } } (document, 'script', 'twitter-wjs');</script>
                </div>
                <!-- Facebook Like Button -->
                <div class="fb-like" data-href="https://apps.facebook.com/texashedgeem" data-layout="button_count"
                    data-action="like" data-show-faces="false" data-share="false">
                </div>
            </div>
            <!-- Login Button -->
            <div id="LoginDiv" runat="server">
                <input type="button" value="Login" id="Login" class="btnlogin" onclick="show_login();" />
            </div>
            <!-- Login Section -->
            <div align="left" id="userdetails" class="white_content">
                <div id="hide_login" onclick="hide_login();">
                </div>
                <table cellpadding="5" cellspacing="2" class="logintable">
                    <tr>
                        <td>
                            <%-- <div id="chk_fb_visibility" runat="server">
                        <div id="auth-status">
                            <div id="auth-loggedout">
                                <div id="chk_fb_visibility_logout" runat="server">--%>
                            <div class="fb-login-button" scope="user_photos,email,user_checkins">
                                <a href="#" id="fblogin">
                                    <img src="../resources/buttons/loginfb.png" id="fbloginimg" /></a>
                            </div>
                            <%--</div>
                            </div>
                        </div>
                    </div>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="login_notice">
                            Connect with your Facebook account, or quickly sign up for Playing Texas Hold’Em
                            Poker.
                            <hr />
                        </td>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_login_status_msg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtUsername" Text="" runat="server" CssClass="int" placeholder="Username"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                ControlToValidate="txtUsername" ValidationGroup="check"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPassword" Text="" runat="server" CssClass="int" placeholder="Password"
                                TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                ControlToValidate="txtPassword" ValidationGroup="check"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a href="#" onclick="show_forgot_password();">Forgot Password?</a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnLogin" runat="server" CssClass="btns" Text="Login" OnClick="btnLogin_Click"
                                ValidationGroup="check" TabIndex="1" />
                            <input type="button" id="btnRegister" class="btns" onclick="show_register();" value="Register" />
                            <%--   <asp:Button ID="btnRegister" CssClass="btns" runat="server" OnClientClick="show_register();"
                                Text="Register" TabIndex="2" Height="30px" />--%>
                            <!-- Add New User Section -->
                            <div id="newuser" runat="server" style="display: none;">
                                <a href="JavaScript:newPopup('frm_new_user.aspx');" id="newuser">New User</a>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblmsg" runat="server" Text="Label" Visible="false" ForeColor="#ff3300"
                                Font-Size="10"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <!-- Forgot Password -->
            <div id="forgot_password" class="white_content" style="display: none;">
                <div id="hide_forgot_password" onclick="hide_forgot_password();">
                </div>
                <table class="logintable" cellpadding="5" cellspacing="2">
                    <tr>
                        <td colspan="3">
                            Please Enter your Email below to get your Password :
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Email :
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Email" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="email_validator" runat="server" ControlToValidate="txt_Email"
                                ValidationGroup="check_fp_val" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Button ID="btnSend" runat="server" Text="Send" ValidationGroup="check_fp_val"
                                OnClick="btnSend_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2">
                            <asp:RegularExpressionValidator ID="email_format_validator" runat="server" ControlToValidate="txt_Email"
                                ErrorMessage="Please enter valid Email Address" ValidationExpression="^([a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]){1,70}$"
                                SetFocusOnError="True" ValidationGroup="check_fp_val"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                </table>
            </div>
            <!-- Rules Div -->
            <div id="content" class="white_content">
                <div id="hide_rules" onclick="hide_rules();">
                </div>
                <h3>
                    Rules</h3>
                <hr />
                <p>
                    A Player vs. House version of Texas Hold’Em, where:</p>
                <p>
                    Players watch games of Hold’em unfold …</p>
                <p>
                    At each stage, odds-based payouts for each hand prospects of winning are presented
                    …</p>
                <p>
                    Players place bets on hands (or hands) they think will win …</p>
                <p>
                    When game ends, House pays out on winning hands</p>
                <p>
                    Players can bet on any hand and at any stage and can ‘Hedge their bets’ to reduce
                    risk or even guarantee a win.</p>
            </div>
            <!-- session time out message div -->
            <div id="session_timeout_message">
                <p>
                    Your Session to sit on this table has expired.</p>
                <p>
                    Please Login again to continue.</p>
                <input type="button" onclick="javascript:window.location='frm_facebook_canvas.aspx';"
                    value="Proceed" />
            </div>
            <!-- Register new user popup-->
            <div id='content_register_user' class="white_content" style="display: none;">
                <div id="hide_register_cross" onclick="hide_register();">
                </div>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
                <asp:UpdatePanel ID="UpdatePane" runat="server">
                    <ContentTemplate>
                        <div class="New_user_popup">
                            <table cellspacing="6" cellpadding="2" align="center">
                                <tr>
                                    <td id="signup_notice" class="signup_notice">
                                        Sign up for Playing Texas Hold’Em Poker.
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_register_status_msg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:RegularExpressionValidator ID="expEmail" runat="server" ControlToValidate="txt_username"
                                            ErrorMessage="Please enter valid Email Address" ValidationExpression="^([a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]){1,70}$"
                                            SetFocusOnError="True" ValidationGroup="checkval"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_username" runat="server" CssClass="int" placeholder="Email"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqd_validator_txt_username" runat="server" ControlToValidate="txt_username"
                                            ValidationGroup="checkval" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_password" runat="server" TextMode="Password" CssClass="int"
                                            placeholder="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqd_validator_txt_password" runat="server" ControlToValidate="txt_password"
                                            ValidationGroup="checkval" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_full_name" runat="server" CssClass="int" placeholder="Display Name"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqd_validator_txt_full_name" runat="server" ControlToValidate="txt_full_name"
                                            ValidationGroup="checkval" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_new_user" runat="server" Text="Register" ValidationGroup="checkval"  OnClientClick="javascript:disableButton(this,3000);"
                                            OnClick="btn_new_user_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- Edit Profile -->
            <div id="edit_profile" class="edit_profile_content" style="display: none;">
            </div>
            <script type="text/javascript">
                function load_edit_profile() {
                    var request = $.ajax({
                        url: "frm_user_profile_edit.aspx",
                        cache: false,
                        success: function (html) {
                            $("#edit_profile").append(html);
                        }
                    });
                    request.fail(function (jqXHR, textStatus) {
                        alert("Request failed: " + textStatus);
                    });
                }
            </script>
            <!-- Register new facebook user popup-->
            <div id='content_register_fb_user' class="white_content" style="display: none;">
                <div id="hide_register_fb_cross" onclick="hide_fb_register();">
                </div>
                <asp:UpdatePanel ID="updatepanel" runat="server">
                    <ContentTemplate>
                        <div class="New_user_popup">
                            <table cellspacing="6" cellpadding="2" align="center">
                                <tr>
                                    <td id="signup_fb_notice" class="signup_notice" align="center">
                                        Thanks you are now connected via facebook and can start to play. Optionally you
                                        can create a local HedgeEm password and display name.<br>
                                        <hr />
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_register_fb_status_msg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:RegularExpressionValidator ID="expEmail1" runat="server" ControlToValidate="txt_fb_username"
                                            ErrorMessage="Please enter valid Email Address" ValidationExpression="^([a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]){1,70}$"
                                            SetFocusOnError="True" ValidationGroup="checkfbval"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_fb_username" runat="server" CssClass="int" placeholder="Email"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rqd_validator_txt_fb_username" runat="server" ControlToValidate="txt_fb_username"
                                            ValidationGroup="checkfbval" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_fb_password" runat="server" TextMode="Password" CssClass="int"
                                            placeholder="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_fb_password"
                                            ValidationGroup="checkfbval" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_fb_display_name" runat="server" CssClass="int" placeholder="Display Name"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_fb_display_name"
                                            ValidationGroup="checkfbval" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_new_fb_user" runat="server" Text="Register" ValidationGroup="checkfbval"
                                            OnClick="btn_new_fb_user_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- error message div -->
            <div id="error_message" class="white_content">
                <p>
                    The username/password you have entered is incorrect.</p>
                <input type="button" onclick="hide_error_message();" value="OK" />
            </div>
            <!-- Admin Button -->
            <asp:Button ID="btnAdmin" runat="server" Text="Hi Admin, click here!" OnClick="btnAdmin_Click" />
            <!-- Facebook Logout Section -->
            <div id="facebooklogout" runat="server">
                <%-- <div id="fb" runat="server">
                    <div id="auth-loggedin" style="display: none;">
                        <span id="auth-displayname" onclick="go_to_profile_edit();"></span><a href="#" id="auth-logoutlink">
                            <img src="../resources/fb_logout.png" class="logout" /></a>
                    </div>
                </div>--%>
                <div id="Logout" runat="server" style="display: none;">
                    <div id="logged_in_user" onclick="go_to_profile_edit();">
                        <asp:Label ID="lbl_user_name" runat="server"></asp:Label>
                    </div>
                    <asp:Button ID="btnLogout" CssClass="btns" runat="server" Text="Logout" OnClick="btnLogout_Click"
                        Height="30px" /></div>
            </div>
            <!-- User Details -->
            <div id="newuser1">
                <table>
                    <tr>
                        <td>
                            <asp:Image ID="usr_image" runat="server" CssClass="img" Height="50px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="btn_deal_buttons_container">
                                <asp:ImageButton ID="btn_play_now" Enabled="true" runat="server" OnClick="btn_play_now_Click"
                                    OnClientClick="javascript: play_multi_sound('sound_deal');" onmouseover="javascript:return ChangeImage('mouseoverImage');"
                                    onmouseout="javascript:return ChangeImage('mouseoutImage');" Text="Play Now"
                                    ImageUrl="~/resources/buttons/btn_play.png" CssClass="play_now_enabled" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txt_get_fm_email_id" runat="server" Style="display: none;" AutoPostBack="True"></asp:TextBox>
                            <asp:TextBox ID="txt_fullname" runat="server" Style="display: none;" AutoPostBack="True"></asp:TextBox>
                            <asp:HiddenField ID="fb_user_id" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <table id="deposit_update" runat="server" visible="false">
                <tr>
                    <td>
                        <a href="JavaScript:depositupdatePopup('frm_deposit.aspx');" id="deposit_update">Deposit</a>
                    </td>
                </tr>
            </table>
            <!--Command buttons content -->
            <div id='command_buttons_container'>
                <!-- Help button -->
                <div id='btn_help'>
                    <%--   <asp:ImageButton ID="btn_help" runat="server" ImageUrl="../resources/buttons/btn_help.png" />--%>
                </div>
                <!-- Rules button -->
                <div id='btn_rules' onclick="show_rules();">
                    <%-- <asp:ImageButton ID="btn_rules" runat="server" 
                        ImageUrl="../resources/buttons/btn_rules.png" onclick="btn_rules_Click" />--%>
                </div>
                <!-- Cashier button -->
                <div id='btn_cashier' onclick="btn_cashier_Click();" style="display: none">
                    <%-- <asp:ImageButton ID="btn_cashier"
                        ImageUrl="../resources/buttons/btn_cashier.png" OnClientClick="btn_cashier_Click()" />--%>
                </div>
            </div>
        </div>
    </div>
    <div id="fade" class="black_overlay" onclick="hide_rules();">
    </div>
    <div id="fade_session_timeout_message" class="fade_overlay">
    </div>
    <div id="hide_register" class="black_overlay" onclick="hide_register();">
    </div>
    <div id="hide_fb_register" class="black_overlay" onclick="hide_fb_register();">
    </div>
    <div id="fade_userdetails" class="black_overlay" onclick="hide_login();">
    </div>
    <div id="fade_error_message" class="black_overlay" onclick="hide_error_message();">
    </div>
    <div id="fade_forgot_password" class="black_overlay" onclick="hide_forgot_password();">
    </div>
    <div id="hide_edit_profile" class="black_overlay" onclick="hide_edit_profile();">
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePane"
        DisplayAfter="10">
        <ProgressTemplate>
            <div class="loading-register" align="center">
                <img src="../resources/loading.gif" alt="" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <style type="text/css">
     .loading-register
        {
            position: fixed;
            top: 30%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index:99999999;
        }
        
        .loading-register img
        {
            width: 50px;
            height: 50px;
        }
    </style>
    <!-- script to login and invite friends to facebook -->
    <script type="text/javascript">

        // Init the SDK upon load
        window.fbAsyncInit = function () {
            FB.init({
                appId: '1436611759889065', // HedgeEm Developent Facebook App
                channelUrl: '//' + window.location.hostname + '/channel', // Path to your Channel File
                status: true, // check login status
                cookie: true, // enable cookies to allow the server to access the session
                xfbml: true  // parse XFBML
            });
            var email = "";

            // listen for and handle auth.statusChange events
            FB.Event.subscribe('auth.statusChange', function (response) {
            });

            $("#fblogin").click(function () {
                document.getElementById('fbloginimg').src = 'resources/buttons/connecting.png';
                FB.getLoginStatus(function (response) {
                    if (response.status === 'connected') {
                        // connected
                        // user has auth'd your app and is logged into Facebook
                        FB.api('/me', function (me) {
                            if (me.name) {
                                var uid = "http://graph.facebook.com/" + response.authResponse.userID + "/picture";
                                var accessToken = response.authResponse.accessToken;
                                //  document.getElementById('auth-displayname').innerHTML = me.name;
                                document.getElementById('txt_get_fm_email_id').value = me.email;
                                document.getElementById('usr_image').src = uid;
                                document.getElementById('btn_play_now').style.display = 'block';
                                document.getElementById('Logout').style.display = 'block';
                                document.getElementById('lbl_user_name').innerHTML = me.name;
                                document.getElementById('txt_fullname').value = me.name;

                                document.getElementById('fb_user_id').value = response.authResponse.userID;

                                document.getElementById('btn_play_now').disabled = false;
                                document.getElementById('btn_play_now').style.cursor = 'pointer';
                                document.getElementById('LoginDiv').style.display = 'none';

                                hide_login();
                                var email = document.getElementById('txt_get_fm_email_id').value;

                                $(document).ready(function () {
                                    $.ajax({
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        url: "frm_facebook_canvas.aspx/f_check_user_role?uid=" + email,
                                        data: "{}",
                                        dataType: "json",
                                        success: function (data) {
                                            var role = data.d;
                                            if (role == "ADMIN") {
                                                document.getElementById('btnAdmin').style.display = 'block';

                                            }
                                            else {
                                                document.getElementById('btnAdmin').style.display = 'none';

                                            }

                                        }
                                    });
                                });

                                $(document).ready(function () {
                                    $.ajax({
                                        type: "POST",
                                        contentType: "application/json; charset=utf-8",
                                        url: "frm_facebook_canvas.aspx/f_check_user_logged_in_from_facebook_first_time?uid=" + email,
                                        data: "{}",
                                        dataType: "json",
                                        success: function (data) {
                                            var if_exists = data.d;
                                            if (if_exists == true) {
                                                // let them login and play
                                                //   document.getElementById('signup_notice').innerHTML = "Sign up for Playing Texas Hold’Em Poker.<br> <hr />";
                                                var request = $.ajax({
                                                    url: "frm_user_profile_edit.aspx",
                                                    cache: false,
                                                    success: function (html) {
                                                        $("#edit_profile").append(html);
                                                    }
                                                });
                                                request.fail(function (jqXHR, textStatus) {
                                                    alert("Request failed: " + textStatus);
                                                });
                                            }
                                            else {
                                                hide_login();
                                                // document.getElementById('signup_notice').innerHTML = "Thanks you are now connected  via facebook and can start to play.Optionally you can create a local HedgeEm password and display name.<br> <hr />";
                                                show_fb_register();
                                                document.getElementById('txt_fb_username').value = email;

                                            }

                                        }
                                    });
                                });
                            }
                        })
                        document.getElementById('btn_play_now').style.display = 'block';
                        document.getElementById('btnLogin').style.display = 'none';

                    } else if (response.status === 'not_authorized') {
                        // not_authorized
                        login();

                    }


                    else {
                        // not_logged_in

                        login();


                    }
                });
            });

            function login() {
                FB.login(function (response) {
                    if (response.authResponse) {
                        console.log('Welcome!  Fetching your information.... ');
                        FB.api('/me', function (response) {
                            console.log('Good to see you, ' + response.name + '.');

                            var uid = "http://graph.facebook.com/" + (FB.getAuthResponse() || {}).userID + "/picture";
                            var access_token = FB.getAuthResponse()['accessToken'];

                            var userID = (FB.getAuthResponse() || {}).userID;

                            //  document.getElementById('auth-displayname').innerHTML = response.name;
                            document.getElementById('txt_get_fm_email_id').value = response.email;
                            document.getElementById('Logout').style.display = 'block';
                            document.getElementById('lbl_user_name').innerHTML = response.name;
                            document.getElementById('txt_fullname').value = response.name;

                            document.getElementById('usr_image').src = uid;
                            document.getElementById('btn_play_now').style.display = 'block';


                            document.getElementById('fb_user_id').value = (FB.getAuthResponse() || {}).userID;

                            document.getElementById('btn_play_now').disabled = false;
                            document.getElementById('btn_play_now').style.cursor = 'pointer';
                            document.getElementById('LoginDiv').style.display = 'none';

                            hide_login();
                            var email = document.getElementById('txt_get_fm_email_id').value;
                            $(document).ready(function () {
                                $.ajax({
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    url: "frm_facebook_canvas.aspx/f_check_user_logged_in_from_facebook_first_time?uid=" + email,
                                    data: "{}",
                                    dataType: "json",
                                    success: function (data) {
                                        var if_exists = data.d;
                                        if (if_exists == true) {
                                            // let them login and play

                                            var request = $.ajax({
                                                url: "frm_user_profile_edit.aspx",
                                                cache: false,
                                                success: function (html) {
                                                    $("#edit_profile").append(html);
                                                }
                                            });
                                            request.fail(function (jqXHR, textStatus) {
                                                alert("Request failed: " + textStatus);
                                            });
                                        }
                                        else {
                                            hide_login();

                                            // document.getElementById('signup_notice').innerHTML = "Thanks you are now connected  via facebook and can start to play.Optionally you can create a local HedgeEm password and display name.<br> <hr />";
                                            show_fb_register();
                                            //show_register();
                                            document.getElementById('txt_fb_username').value = email;
                                        }

                                    }
                                });
                            });
                        });
                    } else {
                        // cancelled
                        console.log('User cancelled login or did not fully authorize.');
                        document.getElementById('btn_play_now').disabled = true;
                        document.getElementById('fbloginimg').src = 'resources/buttons/loginfb.png';
                    }

                }, { scope: 'email' });
            }
        }
        // Load the SDK Asynchronously
        (function (d) {
            var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement('script'); js.id = id; js.async = true;
            js.src = "https://connect.facebook.net/en_US/all.js";
            ref.parentNode.insertBefore(js, ref);
        } (document));


        function Facebook_Invite_Friends() {
            FB.ui({ method: 'apprequests',
                message: 'Play a game of Texas Hedgeem with me.......!'
            });
        }
    </script>
    </form>
</body>
</html>
