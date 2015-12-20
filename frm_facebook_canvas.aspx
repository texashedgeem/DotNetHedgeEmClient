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
</head>

<body>
    <!-- This shows a splash screen until page has loaded -->
    <div id="element" class="introLoading" style="display: none;">
    </div>
    
    <!-- Not sure what this does, try deleting and see -->
    <div id="fb-root">
    </div>
    
    <!-- Not sure what this does, try deleting and see -->
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

                
                
        
                <div id="popup_message">
                    <script type="text/javascript">
                        function onclose() {
                            document.getElementById('hide_edit_profile').style.display = 'none';
                            document.getElementById('popup_message').style.display = 'none';
                        }
                    </script>
                    <asp:PlaceHolder ID="Place_Holder_Popup_Message" runat="server"></asp:PlaceHolder>
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
        
                
                <!-- error message div -->
                <div id="error_message" class="white_content modal-content">
                    <p>
                        The username/password you have entered is incorrect.</p>
                    <input type="button" onclick="hide_error_message();" value="OK" />
                </div>
                
                <!-- User Details & Play Now -->
                <div class="button_toolbar">
                     
                    <p id="login_wait_message" style="display: none;">
                            Please wait for a while, we are preparing your table.</p>
                                                
                    <div id="btn_deal_buttons_container" class="online">
                        
                        <!-- Rules button -->
                        <!--
                        <div id='btn_rules' class="online" onclick="show_rules();">
                        </div>
                        -->
                        
                        <!-- Play Classic (Online) Mode -->
                        <asp:Button ID="btn_play_anon" Enabled="true" runat="server" OnClick="btn_anon_online_Click"
                            OnClientClick="javascript: play_multi_sound('sound_deal');" Text="Play" CssClass="play_now_enabled btn btn-success" />
                        
                        <!-- Play Casino Mode -->
                        <!--
                        <asp:Button ID="btn_play_retro" Enabled="true" runat="server" OnClick="btn_anon_retro_Click"
                            OnClientClick="javascript: play_multi_sound('sound_deal');" Text="Play 'Retro'" CssClass="play_now_enabled btn btn-success" />
                        -->
                        
                        <!-- Play Casino Mode -->
                        <!--
                        <asp:Button ID="btn_play_casino" Enabled="true" runat="server" OnClick="btn_anon_casino_Click"
                            OnClientClick="javascript: play_multi_sound('sound_deal');" Text="Play 'Casino'" CssClass="play_now_enabled btn btn-success" />
                         -->
                     </div>
                    
                </div>
        
                <div id="alertmessage" class="" style="display: none;">
            <div class="modal-header">
                <div id="Div1" onclick="hide_alertmessage();" class="close">
                    <span aria-hidden="true">×</span>
                </div>
            </div>
            
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
                        Copyright © Qeetoto Ltd 2009-2016. All rights reserved.
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
