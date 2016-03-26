<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_home_page.aspx.cs" Inherits="frm_home_page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<meta http-equiv="refresh" content="30">--%>
    <script type="text/javascript" src="../resources/javascript/hedgeem_table_javascript.js"></script>
    <!-- online scripts and css for place bet widget slider -->
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/smoothness/jquery-ui.css">
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
    <script type="text/javascript" src="https://connect.facebook.net/en_US/all.js"></script>
    <!-- link which helped us to solve touch problem in jquery - http://touchpunch.furf.com/ and its script is used below -->
    <script src="../resources/javascript/jquery.ui.touch-punch.js"></script>
    <%--<script type='text/javascript' src='https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js'></script>--%>
    <!-- script use to disable second click on deal card buttons -->
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
    <script type="text/javascript">
        function LoadTimer() {
            var timeInterval = 30000;
            var remainingSeconds = (timeInterval / 1000);
            var timeInterval = setInterval(function () {
                document.getElementById('divRemainingTime').innerHTML = --remainingSeconds;

                if (remainingSeconds <= 0) {
                    document.getElementById('divRemainingTime').innerHTML = "";
                    document.getElementById('divloadingtext').innerHTML = "Next Stage Dealing.....";
                    //alert('expired;');
                    //  window.location.href = "Webform2.aspx";
                    //  LoadTimer();
                    clearInterval(timeInterval);
                }
            }, 1000);
        }

        //        setInterval(click, 3000);
        //        function click() {
        //            PageMethods
        //        }
        //        setInterval($.ajax({
        //            type: "POST",
        //            contentType: "application/json; charset=utf-8",
        //            url: "frm_home_page.aspx/btn_deal_next_stage_Click",
        //        
        //            dataType: "json",
        //            success: function (data) {
        //                //Write functionality to display data
        //            },
        //            error: function (result) {
        //                alert("Error");
        //            }
        //        }), 3000);
    </script>
    <script type="text/javascript">
        function show_login() {
            document.getElementById('userdetails').style.display = 'block';
            document.getElementById('fade_userdetails').style.display = 'block';
            return;
        }


        function hide_login() {
            document.getElementById('userdetails').style.display = 'none';
            document.getElementById('fade_userdetails').style.display = 'none';
            return;
        }
    </script>
</head>
<body>
    <%-- <script type="text/javascript">
        setInterval(click, 3000);
        function click() {
            PageMethods.btn_deal_next_stage_Click();
        }
        </script>--%>
    <%--<script type="text/javascript">
//get a hold of the timers
    var iddleTimeoutWarning = null;
    var iddleTimeout = null;
 
    //this function will automatically be called by ASP.NET AJAX when page is loaded and partial postbacks complete
    function pageLoad() {
 
        //clear out any old timers from previous postbacks
        if (iddleTimeoutWarning != null)
            clearTimeout(iddleTimeoutWarning);
        if (iddleTimeout != null)
            clearTimeout(iddleTimeout);
        //read time from web.config
        var millisecTimeOutWarning = <%= int.Parse(System.Configuration.ConfigurationManager.AppSettings["SessionTimeoutWarning"]) * 60 * 1000 %>;
        var millisecTimeOut = <%= int.Parse(System.Configuration.ConfigurationManager.AppSettings["SessionTimeout"]) * 60 * 1000 %>;
 
        //set a timeout to display warning if user has been inactive
        iddleTimeoutWarning = setTimeout("DisplayIddleWarning()", millisecTimeOutWarning);
        iddleTimeout = setTimeout("TimeoutPage()", millisecTimeOut);
    }
 
    function DisplayIddleWarning() {
      //  alert("Your session is about to expire due to inactivity.");
      show_session_timeout_message();
    }
 
    function TimeoutPage() {
        //refresh page for this sample, we could redirect to another page that has code to clear out session variables
        //location.reload();
    }
    </script>--%>
    <audio id="sound_bet" src="../resources/waves/Bet-4.wav" preload="auto"></audio>
    <audio id="sound_fold" src="../resources/waves/Fold-3.wav" preload="auto"></audio>
    <audio id="sound_deal" src="../resources/waves/Deal-4.wav" preload="auto"></audio>
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
    <script>        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "https://connect.facebook.net/en_US/all.js#xfbml=1&appId=119665184821685";
            fjs.parentNode.insertBefore(js, fjs);
        } (document, 'script', 'facebook-jssdk'));
    </script>
    <form id="form1" runat="server">
    <div id="wrapper">
        <img src="../resources/backgrounds/HedgeEmTableComposite_800x600.png" />
        <div id="hedgeem_table_container">
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
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
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true"
                ScriptMode="Release" LoadScriptsBeforeUI="true">
            </asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick">
                    </asp:Timer>
                    <!-- 'Background to everything on the HedgeEm Table main display - typically the Casino carpet -->
                    <div id="hedgeem_table_backdrop">
                    </div>
                    <div id="hedgeem_table_physical_table">
                    </div>
                    <!-- Backdrop for game information -->
                    <div id="hedgeem_table_info_bar_background">
                    </div>
                    <!-- Show the current game id -->
                    <div id="hedgeem_game_id">
                        <asp:Label ID="lbl_game_id" runat="server"></asp:Label>
                    </div>
                    <!-- Image to show Casino logo -->
                    handpamer
                    <div id="table_logo">
                    </div>
                    <div id="divRemainingTime" style="display: block; padding: 0; position: relative;
                        top: 36%; width: 13%; font-size: 50px; color: White; z-index: 10; left: 46%;"
                        runat="server">
                    </div>
                    <div id="divloadingtext" style="display: block; padding: 0; position: relative; top: 36%;
                        width: 25%; font-size: 20px; color: White; z-index: 10; left: 40%;" runat="server">
                    </div>
                    <asp:TextBox ID="txt_json_response" Visible="false" runat="server" TextMode="MultiLine"
                        CssClass="json_txt"></asp:TextBox>
                    <!-- Image to show house chips -->
                    <div id="house_chips">
                    </div>
                    <asp:PlaceHolder ID="Place_Holder_Placed_Bets" runat="server"></asp:PlaceHolder>
                    <!-- Bet Slider -->
                    <div id="bet_slider">
                        <asp:PlaceHolder ID="Place_Holder_Bet_Slider" runat="server"></asp:PlaceHolder>
                    </div>
                    <!-- Table Jackpot -->
                    <div id="table_jackpot_container" runat="server">
                        <asp:PlaceHolder ID="Place_Holder_Table_Jackpot" runat="server"></asp:PlaceHolder>
                    </div>
                    <asp:PlaceHolder ID="Place_Holder_Winner_Message" runat="server"></asp:PlaceHolder>
                    <!-- Play for real button -->
                    <%-- <div id="btn_play_for_real" onclick="show_play_for_real_text();">
                    </div>--%>
                    <asp:Button ID="btn_play_for_real" runat="server" Text="" OnClientClick="show_play_for_real_text();"
                        OnClick="btn_play_for_real_Click" />
                    <!-- Board Cards -->
                    <div id="board_cards" class="board_cards_container">
                        <!-- Flop_Cards_Description -->
                        <div id="Flop_Cards">
                            <asp:PlaceHolder ID="Place_Holder_Flop_Cards" runat="server"></asp:PlaceHolder>
                        </div>
                        <!-- Turn_Cards_Description -->
                        <div id="Turn_Cards">
                            <asp:PlaceHolder ID="Place_Holder_Turn_Cards" runat="server"></asp:PlaceHolder>
                        </div>
                        <!-- River_Cards_Description -->
                        <div id="River_Cards">
                            <asp:PlaceHolder ID="Place_Holder_River_Cards" runat="server"></asp:PlaceHolder>
                        </div>
                    </div>
                    <!-- Hands -->
                    <div id="hand_panels_container" class="hand_panels_container" runat="server">
                        <asp:PlaceHolder ID="Place_Holder_Hand_Panel" runat="server"></asp:PlaceHolder>
                    </div>
                    <!-- Betting panels container -->
                    <div id="betting_panels_container">
                        <asp:PlaceHolder ID="Place_Holder_Betting_Panel" runat="server"></asp:PlaceHolder>
                    </div>
                    <!-- xxx I do not like this code.  Lets try to recode in more elegant way -->
                    <div id="hidden_controls">
                        <!-- This button calls javascript 'btn_Get_Clicked_Hand_Value_Click' function when pressed-->
                        <asp:Button ID="btn_hidden_control_to_place_bet" Text="btn_hidden_control_to_place_bet.text: Not set yet"
                            OnClick="btn_Get_Clicked_Hand_Value_Click" value="btn_hidden_control_to_place_bet.value: Not set yet"
                            runat="server" />
                        <asp:Button ID="btn_hidden_control_to_cancel_bet" Text="btn_hidden_control_to_cancel_bet.text: Not set yet"
                            OnClick="btn_cancel_bets_for_this_hand_and_stage_Click" value="btn_hidden_control_to_cancel_bet.value: Not set yet"
                            OnShiftClick="" runat="server" />
                        <input type="text" id="btn_hidden_control_temp_store_for_hand_index" text="btn_hidden_control_temp_store_for_hand_index.text: Not set yet"
                            value="btn_hidden_control_temp_store_for_hand_index.value: Not set yet" runat="server" />
                        <asp:Button ID="btn_Get_Clicked_Player_Id" runat="server" Text="Get_Player_Id" OnClick="btn_Get_Clicked_Player_Id_Click" />
                        <input type="text" id="mytext_player_Id" runat="server" value="" />
                        <input type="text" id="mytext_player_Id_hedgeem" runat="server" value="" />
                        <asp:HiddenField ID="hdn_table_id" runat="server" />
                        <asp:Button ID="btn_get_chips_add" runat="server" Text="btn_get_chips_add" OnClick="btn_get_chips_add_Click"
                            value="btn_get_chips_add.value: Not set yet" />
                    </div>
                    <!-- Deal Buttons Container -->
                    <%--<div id='btn_deal_buttons_container'>
                        <!-- Deal HOLE Button -->
                        <asp:ImageButton ID="btnDealHole" runat="server" OnClick="btn_deal_next_stage_Click"
                            OnClientClick="javascript: play_multi_sound('sound_deal'); disableButton(this,3000);"
                            onmouseover="javascript:return ChangeImage('mouseoverImage');" onmouseout="javascript:return ChangeImage('mouseoutImage');"
                            Text="Deal Hole" ImageUrl="../resources/buttons/btn_deal_hole.png" />
                        <!-- Deal FLOP Button -->
                        <asp:ImageButton ID="btnDealFlop" runat="server" OnClick="btn_deal_next_stage_Click"
                            OnClientClick="javascript: play_multi_sound('sound_deal'); disableButton(this,3000);"
                            onmouseover="javascript:return ChangeImageFlop('mouseoverImage');" onmouseout="javascript:return ChangeImageFlop('mouseoutImage');"
                            Text="Deal Flop" ImageUrl="../resources/buttons/btn_deal_flop.png" />
                        <!-- Deal TURN Button -->
                        <asp:ImageButton ID="btnDealTurn" runat="server" OnClick="btn_deal_next_stage_Click"
                            OnClientClick="javascript: play_multi_sound('sound_deal'); disableButton(this,3000);"
                            onmouseover="javascript:return ChangeImageTurn('mouseoverImage');" onmouseout="javascript:return ChangeImageTurn('mouseoutImage');"
                            Text="Deal Turn" ImageUrl="../resources/buttons/btn_deal_turn.png" />
                        <!-- Deal RIVER Button -->
                        <asp:ImageButton ID="btnDealRiver" runat="server" onmouseover="javascript:return ChangeImageRiver('mouseoverImage');"
                            OnClientClick="javascript: play_multi_sound('sound_deal'); disableButton(this,3000);"
                            onmouseout="javascript:return ChangeImageRiver('mouseoutImage');" Text="Deal River"
                            ImageUrl="~/resources/buttons/btn_deal_river.png" OnClick="btn_deal_next_stage_Click" />
                        <!-- Deal Next Game Button -->
                        <asp:ImageButton ID="btnNextGame" runat="server" Text="Next Game" ImageUrl="../resources/buttons/btn_deal_next_game.png"
                            OnClick="btn_deal_next_stage_Click" OnClientClick=" disableButton(this,3000); call();" />
                        <%--<asp:Button ID="btn_auto_deal" runat="server" OnClick="btn_deal_next_stage_Click" />
                    </div>--%>
                    <!-- Player Content -->
                    <asp:UpdatePanel ID="updPanl_to_avoid_Postback" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <!-- Player_Info -->
                            <div id="Player_Info" runat="server">
                                <asp:PlaceHolder ID="Place_Holder_Player_Info" runat="server"></asp:PlaceHolder>
                            </div>
                            <!-- Odds_bets -->
                            <div id="Place_Holder_Odds_bets_sample" runat="server">
                                <asp:PlaceHolder ID="Place_Holder_Odds_bets" runat="server"></asp:PlaceHolder>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--Command buttons content -->
                    <div id='command_buttons_container'>
                        <!-- Lobby button -->
                        <div id='btn_lobby'>
                            <asp:ImageButton ID="btnLobby" runat="server" ImageUrl="../resources/buttons/btn_lobby.png"
                                OnClick="btnLobby_Click" />
                        </div>
                        <!-- Help button -->
                        <div id='btn_help'>
                            <asp:ImageButton ID="btn_help" runat="server" ImageUrl="../resources/buttons/btn_help.png"
                                OnClick="btnLobby_Click" />
                        </div>
                        <!-- Rules button -->
                        <div id='btn_rules' onclick="show_rules();">
                            <%-- <asp:ImageButton ID="btn_rules" runat="server" 
                        ImageUrl="../resources/buttons/btn_rules.png" onclick="btn_rules_Click" />--%>
                        </div>
                        <!-- Get Chips button -->
                        <div id='btn_get_chips' onclick="show_get_chips();">
                            Get Chips
                        </div>
                        <!-- Cashier button -->
                        <div id='btn_cashier' onclick="btn_cashier_Click();" runat="server" visible="false">
                        </div>
                        <!-- Show Admin Button (Show odds for all hands)-->
                        <div id='btn_show_admin'>
                            <asp:ImageButton ID="btn_Show_Admin_Flag" runat="server" ImageUrl="../resources/buttons/btn_debug.png"
                                Text="Show Admin" OnClick="btn_Show_Admin_Flag_Click" Visible="false" />
                        </div>
                        <!-- Hide Admin Button (Show odds for all hands)-->
                        <div id='btn_hide_admin'>
                            <asp:ImageButton ID="btn_Hide_Admin_Flag" runat="server" ImageUrl="../resources/buttons/btn_debug.png"
                                Text="Hide Admin" Visible="false" OnClick="btn_Hide_Admin_Flag_Click" />
                        </div>
                    </div>
                    <!-- Error Message content -->
                    <div id="error_message" class="white_content">
                        <div id='short_description' runat="server">
                        </div>
                        <div id='long_description' runat="server">
                        </div>
                    </div>
                    <div id="fade" class="black_overlay" onclick="javascript:closediv();">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- Rules Div -->
            <div id="content" class="div_rules_content">
                <div id="hide_rules" onclick="hide_rules();">
                </div>
                <h3>
                    Rules</h3>
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
            <!-- Play for real pop up div -->
            <div id="play_for_real_text" class="div_rules_content">
                <p>
                    Thank you for enjoying our game so much you would like to play for real. Unfortunately
                    we are currently not licenced for to play for real money. Please come back soon
                    and hopefully we will be.</p>
                <p>
                    Help us bring this forward ...</p>
                <p>
                    Please let us know how much you anticipate you wpuld bring to a Hedge'Em table when
                    playing with real money.</p>
                <p class="play_for_real_deposit_pledge">
                    £
                    <asp:TextBox ID="txt_play_for_real_deposit_pledge" runat="server"></asp:TextBox>
                    <asp:Button ID="btn_play_for_real_deposit_pledge" runat="server" Text="Submit" ValidationGroup="play_for_real"
                        OnClick="btn_play_for_real_deposit_pledge_Click" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                        ValidationGroup="play_for_real" ControlToValidate="txt_play_for_real_deposit_pledge"></asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Enter Valid Amount"
                        ValidationGroup="play_for_real" ControlToValidate="txt_play_for_real_deposit_pledge"
                        Type="Double"></asp:RangeValidator></p>
                <p>
                    Regards</p>
                <p>
                    The Hedge'Em team.
                </p>
            </div>
            <!-- Get Chips Div -->
            <div id="get_chips" class="div_rules_content">
                <div id="hide_get_chips" onclick="hide_get_chips();">
                </div>
                <h4>
                    <p>
                        Hedge'Em is a free to play game!</p>
                    <p>
                        Please help us keep it that way by doing any of the following to get your free chips.<p>
                            <div id='option_visit_these_sites' class="alternatives_section_heading">
                                Visit one of our favourite sites ...
                            </div>
                            <!-- Fulltilt -->
                            <div id='action_item_fulltilt' class="get_chips_content_divs" onclick='ai_fultilt_click();'>
                                <div id='ai_fult_tilt_image' class="get_chips_image">
                                    <img src="../resources/buttons/full_tilt.png" width="100%" height="10%" />
                                </div>
                                <div id='ai_full_tilt_description'>
                                    Fulltilt
                                </div>
                            </div>
                            <!-- BetFair -->
                            <div id='action_item_betfair' class="get_chips_content_divs" onclick='ai_betfair_click();'>
                                <div id='ai_BetFair_image' class="get_chips_image">
                                    <img src="../resources/buttons/bet_fair.png" width="100%" height="10%" />
                                </div>
                                <div id='ai_BetFair_description'>
                                    BetFair
                                </div>
                            </div>
                            <div id='option_share' class="alternatives_section_heading">
                                <p>
                                    Or share our game with others ...</p>
                            </div>
                            <!-- Recommend to Facebook -->
                            <div id='action_item_recommend_facebook' class="get_chips_content_divs" onclick='ai_fb_recommend_click();'>
                                <div id='ai_recommend_facebook_image' class="get_chips_image">
                                    <img src="../resources/buttons/fb_recommend.jpg" width="100%" height="10%" />
                                </div>
                                <div id='ai_recommend_facebook_description'>
                                    Recommend to a friend on Facebook
                                </div>
                            </div>
                            <!-- Share with Facebook -->
                            <div id='action_item_share_facebook' class="get_chips_content_divs" onclick='ai_fb_share_click();'>
                                <div id='ai_share_facebook_image' class="get_chips_image">
                                    <img src="../resources/buttons/fb_share.jpg" width="100%" height="10%" />
                                </div>
                                <div id='ai_share_facebook_description'>
                                    Share with friends on Facebook
                                </div>
                            </div>
                            <!-- PayPal -->
                            <div id='option_donate' class="alternatives_section_heading">
                                <p>
                                    Alternatively, please make a small donation ...</p>
                            </div>
                            <div id='action_item_paypal' class="get_chips_content_divs">
                                <div id='ai_PayPal_image' class="get_chips_image">
                                    <input type="hidden" name="cmd" value="_s-xclick">
                                    <input type="hidden" name="hosted_button_id" value="FMBK6L8C6FFZQ">
                                    <asp:ImageButton ID="btnDonateNow" runat="server" ImageUrl="../resources/buttons/btn_donate_paypal.gif"
                                        PostBackUrl="https://www.paypal.com/cgi-bin/webscr" />
                                </div>
                                <div id='ai_PayPal_description'>
                                    Donate via PayPal
                                </div>
                            </div>
            </div>
            <!-- session timeout message pop up div -->
            <div id="session_timeout_message">
                <p>
                    Your Session to sit on this table has expired.</p>
                <p>
                    Please Login again to continue.</p>
                <input type="button" onclick="javascript:window.location='frm_facebook_canvas.aspx';"
                    value="Proceed" />
            </div>
            <!-- Place bet widget for hand 0 -->
            <div id="hedgeem_bet_slider1">
                <p class="bet_payout_description">
                    <label for="amount">
                        Bet £</label>
                    <input type="text" id="amount1" runat="server" class="amount" />
                    pays £
                    <input type="text" id="derived_payout_header1" class="amount" />
                </p>
                <div class="bet_slider">
                    <p class="bet_left">
                        £1</p>
                    <div id="slider-range-min1">
                    </div>
                    <p class="bet_right">
                        All-in</p>
                </div>
                <br />
                <asp:UpdatePanel ID="update_panel_to_Stop_page_refresh_on_betting1" runat="server">
                    <ContentTemplate>
                        <div class="bet_buttons">
                            <asp:Button ID="btn_Bet" CssClass="place_bet_button" runat="server" OnClick="btn_Bet_Click"
                                OnClientClick="hide_bet_popup1();" />
                            <input type="button" class="cancel_place_bet_button" onclick="hide_bet_popup1();" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- Place bet widget for hand 1 -->
            <div id="hedgeem_bet_slider2">
                <p class="bet_payout_description">
                    <label for="amount">
                        Bet £</label>
                    <input type="text" id="amount2" runat="server" class="amount" />
                    pays £
                    <input type="text" id="derived_payout_header2" class="amount" />
                </p>
                <div class="bet_slider">
                    <p class="bet_left">
                        £1</p>
                    <div id="slider-range-min2">
                    </div>
                    <p class="bet_right">
                        All-in</p>
                </div>
                <br />
                <asp:UpdatePanel ID="update_panel_to_Stop_page_refresh_on_betting2" runat="server">
                    <ContentTemplate>
                        <div class="bet_buttons">
                            <asp:Button ID="Button2" runat="server" CssClass="place_bet_button" OnClick="btn_Bet_Click"
                                OnClientClick="hide_bet_popup2();" />
                            <input type="button" class="cancel_place_bet_button" onclick="hide_bet_popup2();" /></div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- Place bet widget for hand 2 -->
            <div id="hedgeem_bet_slider3">
                <p class="bet_payout_description">
                    <label for="amount">
                        Bet £</label>
                    <input type="text" id="amount3" runat="server" class="amount" />
                    pays £
                    <input type="text" id="derived_payout_header3" class="amount" />
                </p>
                <div class="bet_slider">
                    <p class="bet_left">
                        £1</p>
                    <div id="slider-range-min3">
                    </div>
                    <p class="bet_right">
                        All-in</p>
                </div>
                <br />
                <asp:UpdatePanel ID="update_panel_to_Stop_page_refresh_on_betting3" runat="server">
                    <ContentTemplate>
                        <div class="bet_buttons">
                            <asp:Button ID="Button4" runat="server" CssClass="place_bet_button" OnClick="btn_Bet_Click"
                                OnClientClick="hide_bet_popup3();" />
                            <input type="button" class="cancel_place_bet_button" onclick="hide_bet_popup3();" /></div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <!-- Place bet widget for hand 3 -->
            <div id="hedgeem_bet_slider4">
                <p class="bet_payout_description">
                    <label for="amount">
                        Bet £</label>
                    <input type="text" id="amount4" runat="server" class="amount" />
                    pays £
                    <input type="text" id="derived_payout_header4" class="amount" />
                </p>
                <div class="bet_slider">
                    <p class="bet_left">
                        £1</p>
                    <div id="slider-range-min4">
                    </div>
                    <p class="bet_right">
                        All-in</p>
                </div>
                <br />
                <asp:UpdatePanel ID="update_panel_to_Stop_page_refresh_on_betting4" runat="server">
                    <ContentTemplate>
                        <div class="bet_buttons">
                            <asp:Button ID="Button6" runat="server" CssClass="place_bet_button" OnClick="btn_Bet_Click"
                                OnClientClick="hide_bet_popup4();" />
                            <input type="button" class="cancel_place_bet_button" onclick="hide_bet_popup4();" /></div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="10">
        <ProgressTemplate>
            <div class="loading" align="center">
                <img src="../resources/loading.gif" alt="" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
    <div id="fade_rules" class="green_overlay" onclick="hide_rules();">
    </div>
    <div id="fade_get_chips" class="green_overlay" onclick="hide_get_chips();">
    </div>
    <div id="fade_play_for_real" class="green_overlay" onclick="hide_play_for_real_text();">
    </div>
    <div id="fade_session_timeout_message" class="fade_overlay">
    </div>
    <div id="fade_userdetails" class="black_overlay" onclick="hide_login();">
    </div>
    <div id="fade_hedgeem_bet_slider" class="no_opaque_overlay" onclick="hide_bet_popup();">
    </div>
    <!-- script to login and invite friends to facebook -->
    <script type="text/javascript">

        // Init the SDK upon load
        window.fbAsyncInit = function () {
            FB.init({
                appId: '1436611759889065', //Development App
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
                                        url: "frm_home_page.aspx/f_check_user_role?uid=" + email,
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
                                        url: "frm_home_page.aspx/f_check_user_logged_in_from_facebook_first_time?uid=" + email,
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
                                    url: "frm_home_page.aspx/f_check_user_logged_in_from_facebook_first_time?uid=" + email,
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
