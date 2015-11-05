<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_facebook_canvas.aspx.cs"
    Inherits="frm_facebook_canvas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Texas Hedge'Em | Watch, Predict, Win</title>
    <link rel="icon" href="resources/favicon.png">
    <link href="resources/css/online/facebook_canvas.css" rel="stylesheet" type="text/css" />
    <link href="resources/css/online/hedgeem_buttons.css" rel="stylesheet" type="text/css" />
    <link href="resources/css/online/hedgeem_popup_message.css" rel="stylesheet" type="text/css" />
    <link href="resources/css/online/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="resources/css/online/animate.css" rel="stylesheet" type="text/css" />
    <link href="resources/css/online/introLoader.css" rel="stylesheet" type="text/css" />
    <link href='http://fonts.googleapis.com/css?family=Open+Sans' rel='stylesheet' type='text/css' />
    <script src="resources/javascript/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="resources/javascript/facebook_canvas.js" type="text/javascript"></script>
    <script src="resources/javascript/preload_images_with_progressbar.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://connect.facebook.net/en_US/all.js"></script>
    <script src="resources/javascript/jquery.easing.1.3.js" type="text/javascript"></script>
    <script src="resources/javascript/spin.min.js" type="text/javascript"></script>
    <script src="resources/javascript/jquery.introLoader.js" type="text/javascript"></script>
    <style type="text/css">
        .loading-register
        {
            position: fixed;
            top: 30%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 99999999;
        }
        
        .loading-register img
        {
            width: 50px;
            height: 50px;
        }
    </style>
    <script type="text/javascript">
        function GetParameterValues(param) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return urlparam[1];
                }
            }
        }
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };       
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#logged_in_user2").click(function () {
                window.location = "frm_user_profile_edit.aspx";

                //Uncomment these 3 lines if you want to show edit profile in Popup
                //                load_edit_profile();              
                $("#edit_profile").css("display", "inline-block");
                $("#hide_edit_profile").css("display", "inline-block");
                //            document.getElementById('edit_profile').style.display = 'block';
                //            document.getElementById('hide_edit_profile').style.display = 'block';
            });
        });

    </script>
</head>
<body>
    <div id="element" class="introLoading" style="display: none;">
    </div>
    <!-- Div to show Progress Bar while images are loading on Page_Load -->
    <%--<div id="progressbar">
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="topbar_fixed">
        <div class="container">
            <div class="row">
                <div class="col-sm-offset-7 col-md-3" id="social_media_container">
                    <div class="alignment">
                        <div id="social_media_container">
                            <!-- Facebook Invite Friends -->
                            <a href="#" onclick="Facebook_Invite_Friends()" class="fb-invite"></a>
                            <!-- Twitter -->
                            <div id="twitter_container">
                                <a href="https://twitter.com/TexasHedgeEm" class="twitter-follow-button" data-show-count="false"
                                    data-show-screen-name="false">Follow @TexasHedgeEm</a>
                                <script>                                    !function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + '://platform.twitter.com/widgets.js'; fjs.parentNode.insertBefore(js, fjs); } } (document, 'script', 'twitter-wjs');</script>
                            </div>
                            <!-- Facebook Like Button -->
                            <div class="fb-like" data-href="https://apps.facebook.com/texashedgeem" data-layout="button_count"
                                data-action="like" data-show-faces="false" data-share="false">
                            </div>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="wrapper" class="container">
        <div class="row">
            <asp:Label ID="lb_alert" runat="server" Text="" ForeColor="Red"></asp:Label>
            <div id="table_background">
            </div>
            <!-- Backdrop for game information -->
            <div class="table_inner">
                <div id="hedgeem_table_info_bar_background">
                </div>
                <!-- Image to show Casino logo -->
                <div id="table_logo">
                </div>
                <!-- Image to show house chips -->
                <div id="house_chips">
                </div>
                <!-- social_media_container -->
                <asp:Button ID="btnAdmin" CssClass="btn btn-primary" Style="display: none;" runat="server"
                    Text="Hi Admin, click here!" OnClick="btnAdmin_Click" />
                <!-- Login Section -->
                <div align="left" id="userdetails" class="white_content animated bounceInDown modal-content"
                    runat="server">
                    <div class="modal-header">
                        <div id="hide_login" class="close" onclick="hide_login();">
                            <span aria-hidden="true">×</span>
                        </div>
                        <h4 id="H1" class="modal-title">
                            Login</h4>
                    </div>
                    <table cellpadding="5" cellspacing="2" class="logintable">
                        <tr>
                            <td>
                                <div class="fb-login-button" scope="user_photos,email,user_checkins">
                                    <a href="#" id="fblogin">
                                        <img src="../resources/buttons/loginfb.png" id="fbloginimg" /></a>
                                </div>
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
                        <tr class="brdr">
                            <td>
                                <asp:Button ID="btnLogin" runat="server" CssClass="btns btn btn-default" Text="Login"
                                    OnClick="btn_login_Click" ValidationGroup="check" TabIndex="1" />
                                <input type="button" id="btnRegister" class="btns btn btn-primary" onclick="show_register();"
                                    value="Register" />
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
                <div id="popup_message">
                    <script type="text/javascript">
                        function onclose() {
                            document.getElementById('hide_edit_profile').style.display = 'none';
                            document.getElementById('popup_message').style.display = 'none';
                        }

                    </script>
                    <asp:PlaceHolder ID="Place_Holder_Popup_Message" runat="server"></asp:PlaceHolder>
                </div>
                <!-- Forgot Password -->
                <div id="forgot_password" class="white_content animated bounceInDown modal-content"
                    style="display: none;">
                    <div class="modal-header">
                        <div id="hide_forgot_password" onclick="hide_forgot_password();" class="close">
                            <span aria-hidden="true">×</span>
                        </div>
                        <h4 id="myModalLabel" class="modal-title">
                            Forgot Password</h4>
                    </div>
                    <table width="100%" class="logintable" cellpadding="5" cellspacing="2">
                        <tr>
                            <td colspan="3">
                                Please Enter your Email below to get your Password :
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 12px">
                                Email :
                            </td>
                        </tr>
                        <tr>
                            <td width="100%">
                                <asp:TextBox ID="txt_Email" CssClass="int" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="email_validator" runat="server" ControlToValidate="txt_Email"
                                    ValidationGroup="check_fp_val" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSend" CssClass="btn btn-primary" runat="server" Text="Send" ValidationGroup="check_fp_val"
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
                <div id="content" class="white_content modal-content animated bounceInDown">
                    <div class="modal-header">
                        <div id="hide_rules" onclick="hide_rules();" class="close">
                            <span aria-hidden="true">×</span>
                        </div>
                        <h4 id="myModalLabel" class="modal-title">
                            Rules</h4>
                    </div>
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
                <div id='content_register_user' class="white_content modal-content" style="display: none;">
                    <div class="modal-header">
                        <div id="hide_register_cross" onclick="hide_register();" class="close">
                            <span aria-hidden="true">×</span>
                        </div>
                        <h4 id="myModalLabel" class="modal-title">
                            Sign up for Playing Texas Hold’Em Poker.</h4>
                    </div>
                    <div class="New_user_popup">
                        <table width="100%" cellspacing="6" cellpadding="2">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_register_status_msg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RegularExpressionValidator ID="expEmail" runat="server" ControlToValidate="txt_username"
                                        ErrorMessage="Please enter valid Email Address" ValidationExpression="^([a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]){1,70}$"
                                        SetFocusOnError="True" ValidationGroup="checkval"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_username" runat="server" CssClass="int" placeholder="Email"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqd_validator_txt_username" runat="server" ControlToValidate="txt_username"
                                        ValidationGroup="checkval" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_password" runat="server" TextMode="Password" CssClass="int"
                                        placeholder="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqd_validator_txt_password" runat="server" ControlToValidate="txt_password"
                                        ValidationGroup="checkval" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_full_name" runat="server" CssClass="int" placeholder="Display Name"
                                        MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqd_validator_txt_full_name" runat="server" ControlToValidate="txt_full_name"
                                        ValidationGroup="checkval" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_new_user" CssClass="btn btn-primary" runat="server" Text="Register"
                                        ValidationGroup="checkval" OnClientClick="javascript:disableButton(this,3000);"
                                        OnClick="btn_new_user_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <!-- Edit Profile -->
                <div id="edit_profile" class="edit_profile_content" style="display: none;">
                </div>
             <%--   Uncomment this java script code if you want to load edit profile page--%>

             <%--   <script type="text/javascript">
                    function load_edit_profile() {

                        var request = $.ajax({
                            url: "frm_user_profile_edit.aspx",
                            cache: false,
                            success: function (html) {
                                $("#edit_profile").append(html);
                            },
                            error: function (response) {
                                errorValue = response.responseText;
                                alert(errorValue);

                            }
                        });
                        request.fail(function (jqXHR, textStatus) {
                            //                            alert("Request failed: " + textStatus);
                        });
                    }
                  
                </script>--%>
                <!-- Register new facebook user popup-->
                <div id='content_register_fb_user' class="white_content modal-content" style="display: none;">
                    <div class="modal-header">
                        <div id="hide_register_fb_cross" onclick="hide_fb_register();" class="close">
                            <span aria-hidden="true">×</span>
                        </div>
                    </div>
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
                                <td>
                                    <asp:Label ID="lbl_register_fb_status_msg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RegularExpressionValidator ID="expEmail1" runat="server" ControlToValidate="txt_fb_username"
                                        ErrorMessage="Please enter valid Email Address" ValidationExpression="^([a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]){1,70}$"
                                        SetFocusOnError="True" ValidationGroup="checkfbval"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_fb_username" runat="server" CssClass="int" placeholder="Email"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqd_validator_txt_fb_username" runat="server" ControlToValidate="txt_fb_username"
                                        ValidationGroup="checkfbval" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_fb_display_name" runat="server" CssClass="int" placeholder="Display Name"
                                        MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_fb_display_name"
                                        ValidationGroup="checkfbval" ErrorMessage="*"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_new_fb_user" CssClass="btn btn-primary" runat="server" Text="Register"
                                        ValidationGroup="checkfbval" OnClick="btn_new_fb_user_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <!-- error message div -->
                <div id="error_message" class="white_content modal-content">
                    <p>
                        The username/password you have entered is incorrect.</p>
                    <input type="button" onclick="hide_error_message();" value="OK" />
                </div>
                <!-- User Details & Play Now -->
                <div class="button_toolbar">
                    <div id="newuser1">
                        <asp:Image ID="usr_image" runat="server" CssClass="img" Height="50px" />
                        <asp:TextBox ID="txt_get_fm_email_id" runat="server" Style="display: none;" AutoPostBack="True"></asp:TextBox>
                        <asp:TextBox ID="txt_fullname" runat="server" Style="display: none;" AutoPostBack="True"></asp:TextBox>
                        <asp:HiddenField ID="fb_user_id" runat="server" />
                    </div>
                    <table id="deposit_update" runat="server" visible="false">
                        <tr>
                            <td>
                                <a href="JavaScript:depositupdatePopup()"></a>
                            </td>
                        </tr>
                    </table>
                    <!--Command buttons content -->
                    <div class="online" id="command_buttons_container">
                        <!-- Help button -->
                        <div id='btn_help'>
                            <%--   <asp:ImageButton ID="btn_help" runat="server" ImageUrl="../resources/buttons/btn_help.png" />--%>
                        </div>
                        <!-- Rules button -->
                        <div id='btn_rules' class="online" onclick="show_rules();">
                            <%-- <asp:Button ID="btn_rules" runat="server" 
                        ImageUrl="../resources/buttons/btn_rules.png" onclick="btn_rules_Click" Text="Rules" />--%>Rules
                        </div>
                        <!-- Cashier button -->
                        <div id='btn_cashier' onclick="btn_cashier_Click();" style="display: none">
                            <%-- <asp:ImageButton ID="btn_cashier"
                        ImageUrl="../resources/buttons/btn_cashier.png" OnClientClick="btn_cashier_Click()" />--%>
                        </div>
                    </div>
                    <div id="btn_deal_buttons_container" class="online">
                        <p id="login_wait_message" style="display: none;">
                            Please wait for a while, we are preparing your table.</p>
                        <asp:Button ID="btn_play_now" Enabled="true" runat="server" OnClick="btn_play_now_Click"
                            OnClientClick="javascript: play_multi_sound('sound_deal');" Text="Play Now" CssClass="play_now_enabled btn btn-success" />
                    </div>
                    <!-- Login Button -->
                    <div class="btn-group pull-right login" id="divLogin" runat="server" onclick="show_login();">
                        <button type="button" class="btn btn-warning" value="Login" id="Login" onclick="show_login();">
                            Login</button>
                        <button type="button" class="btn btn-warning dropdown-toggle" data-toggle="dropdown"
                            aria-expanded="false">
                            <i class="glyphicon login-icon"></i>
                        </button>
                    </div>
                    <!-- Admin Button -->
                    <!-- Facebook Logout Section -->
                    <div id="facebooklogout" runat="server">
                        <!-- Logout Section -->
                        <div id="Logout" runat="server" style="display: none;" class="btn-group pull-right">
                            <div id="logged_in_user">
                                <asp:Label ID="lbl_user_name" runat="server"></asp:Label>
                            </div>
                            <asp:Button ID="btnLogout" CssClass="btns btn btn-warning" runat="server" Text="Logout"
                                OnClick="btnLogout_Click" />
                            <button id="logged_in_user2" type="button" class="btn btn-warning dropdown-toggle"
                                data-toggle="dropdown" aria-expanded="false">
                                <i class="glyphicon login-icon"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="alertmessage" class="" style="display: none;">
            <div class="modal-header">
                <div id="Div1" onclick="hide_alertmessage();" class="close">
                    <span aria-hidden="true">×</span>
                </div>
            </div>
            <h4>
                You are not authorized to view this page.
            </h4>
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
        <!-- script to login and invite friends to facebook -->
        <script type="text/javascript">

            // Init the SDK upon load
            window.fbAsyncInit = function () {
                FB.init({ appId: '<% Response.Write(f_get_facebook_app_id());%>',
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
                                    document.getElementById('login_wait_message').style.display = 'block';
                                    //                                    document.getElementById('btn_play_now').style.display = 'block';
                                    document.getElementById('Logout').style.display = 'block';
                                    document.getElementById('lbl_user_name').innerHTML = me.name;
                                    document.getElementById('txt_fullname').value = me.name;
                                    document.getElementById('fb_user_id').value = response.authResponse.userID;

                                    //                                    document.getElementById('LoginDiv').style.display = 'none';
                                    document.getElementById('divLogin').style.display = 'none';
                                    document.getElementById('Logout').style.display = 'block';
                                    hide_login();
                                    //load_edit_profile();
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
                                            },
                                            error: function (response) {
                                                errorValue = response.responseText;
                                                alert(errorValue);
                                            }
                                        });
                                    });
                                    $(document).ready(function () {
                                        $.ajax({
                                            type: "POST",
                                            contentType: "application/json; charset=utf-8",
                                            url: "frm_facebook_canvas.aspx/f_facebook_login_called_from_javascript?uid=" + email,
                                            data: "{}",
                                            dataType: "json",
                                            success: function (data) {
                                                document.getElementById('login_wait_message').style.display = 'none';
                                                document.getElementById('btn_play_now').disabled = false;
                                                document.getElementById('btn_play_now').style.cursor = 'pointer';
                                                //                                                var role = data.d;
                                                //                                                if (role == "ADMIN") {
                                                //                                                    document.getElementById('btnAdmin').style.display = 'block';

                                                //                                                }
                                                //                                                else {
                                                //                                                   document.getElementById('btnAdmin').style.display = 'none';
                                                //                                                }
                                            }, error: function (response) {
                                                errorValue = response.responseText;
                                                alert(errorValue);
                                            }
                                        });
                                    });

                                    //                                    $(document).ready(function () {

                                    //                                        $.ajax({
                                    //                                            type: "POST",
                                    //                                            contentType: "application/json; charset=utf-8",
                                    //                                            url: "frm_facebook_canvas.aspx/f_check_user_logged_in_from_facebook_first_time?uid=" + email,
                                    //                                            data: "{}",
                                    //                                            dataType: "json",
                                    //                                            success: function (data) {
                                    //                                                var if_exists = data.d;
                                    //                                                if (if_exists == true) {
                                    //                                                    // let them login and play
                                    //                                                    //   document.getElementById('signup_notice').innerHTML = "Sign up for Playing Texas Hold’Em Poker.<br> <hr />";
                                    //                                                    var request = $.ajax({
                                    //                                                        url: "frm_user_profile_edit.aspx",
                                    //                                                        cache: false,
                                    //                                                        success: function (html) {
                                    //                                                            $("#edit_profile").append(html);
                                    //                                                        }
                                    //                                                    });
                                    //                                                    request.fail(function (jqXHR, textStatus) {
                                    //                                                        //                                                        alert("Request failed: " + textStatus);
                                    //                                                    });
                                    //                                                }
                                    //                                                else {
                                    //                                                    hide_login();
                                    //                                                    document.getElementById('signup_notice').innerHTML = "Thanks you are now connected  via facebook and can start to play.Optionally you can create a local HedgeEm password and display name.<br> <hr />";
                                    //                                                    show_fb_register();
                                    //                                                    document.getElementById('txt_fb_username').value = email;
                                    //                                                }
                                    //                                            }
                                    //                                        });
                                    //                                    });
                                }
                            })
                            //                            document.getElementById('btn_play_now').style.display = 'block';
                            document.getElementById('btnLogin').style.display = 'none';

                        } else if (response.status === 'not_authorized') {
                            // not_authorized
                            login();
                            alert('not');
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
                                document.getElementById('login_wait_message').style.display = 'block';


                                document.getElementById('fb_user_id').value = (FB.getAuthResponse() || {}).userID;


                                //                                document.getElementById('LoginDiv').style.display = 'none';
                                document.getElementById('divLogin').style.display = 'none';
                                document.getElementById('Logout').style.display = 'block';
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
                                                        },
                                                        error: function (response) {
                                                            errorValue = response.responseText;
                                                            alert(errorValue);
                                                        }
                                                    });
                                                });
                                                $(document).ready(function () {
                                                    $.ajax({
                                                        type: "POST",
                                                        contentType: "application/json; charset=utf-8",
                                                        url: "frm_facebook_canvas.aspx/f_facebook_login_called_from_javascript?uid=" + email,
                                                        data: "{}",
                                                        dataType: "json",
                                                        success: function (data) {
                                                            document.getElementById('login_wait_message').style.display = 'none';
                                                            document.getElementById('btn_play_now').disabled = false;
                                                            document.getElementById('btn_play_now').style.cursor = 'pointer';
                                                            //                                                var role = data.d;
                                                            //                                                if (role == "ADMIN") {
                                                            //                                                    document.getElementById('btnAdmin').style.display = 'block';

                                                            //                                                }
                                                            //                                                else {
                                                            //                                                   document.getElementById('btnAdmin').style.display = 'none';
                                                            //                                                }
                                                        },
                                                        error: function (response) {
                                                            errorValue = response.responseText;
                                                            alert(errorValue);
                                                        }
                                                    });
                                                });
                                                var request = $.ajax({
                                                    url: "frm_user_profile_edit.aspx",
                                                    cache: false,
                                                    success: function (html) {
                                                        $("#edit_profile").append(html);
                                                    }
                                                });
                                                request.fail(function (jqXHR, textStatus) {
                                                    //                                                    alert("Request failed: " + textStatus);
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
                        }
                        else {
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
    </div>
    <div class="bottom_bar_fixed">
        <div class="container">
            <div class="row">
                <div class=" col-sm-12 col-md-12">
                    <div class="footertxt">
                        <%--<asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" OnClick="Button1_Click"
                            Text="Button" />--%>
                        Copyright © Qeetoto Ltd 2009-2015. All rights reserved.
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
