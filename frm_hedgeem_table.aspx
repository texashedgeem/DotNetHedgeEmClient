<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_hedgeem_table.aspx.cs" EnableViewState="false"
    Inherits="frm_hedgeem_table" %>
<meta http-equiv="Cache-control" content="max-age=10"/> 
 <meta http-equiv="Expires" content="08 may 2015 23:59:59 GMT"/>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<head id="Head1" runat="server">
    <title>Texas Hedge'Em | Watch, Predict, Win!</title>
    <script type="text/javascript" src="resources/javascript/hedgeem_table_javascript.js"></script>
    <!-- online scripts and css for place bet widget slider -->
    <!--<link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/smoothness/jquery-ui.css">-->
    <link href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="resources/javascript/animation.js" type="text/javascript"></script>
    <link href="resources/css/css-animation.css" rel="stylesheet" type="text/css" />
    <link href="resources/css/online/animate.css" rel="stylesheet" type="text/css" />
    <!-- xxx Do not use Visual Studio themes -->
    
    <!--<link href='http://fonts.googleapis.com/css?family=Open+Sans' rel='stylesheet' type='text/css'/>-->
    <link rel="stylesheet" href="App_Themes/ONLINE/normalize.css"/>
    <link rel="stylesheet" href="App_Themes/ONLINE/demo.css"/>
    <link rel="stylesheet" href="App_Themes/ONLINE/demo.css"/>
    <!-- Pushy CSS -->
    <link rel="stylesheet" href="App_Themes/ONLINE/pushy.css"/>
    <!-- jQuery -->
        <script src="http://code.jquery.com/jquery-1.11.1.min.js" type="text/javascript"></script>


  
	  <script src="resources/javascript/v2p.js"></script>
      <script src="https://cdnjs.cloudflare.com/ajax/libs/prefixfree/1.0.7/prefixfree.min.js" type="text/javascript"></script>
  
    <!--<script type="text/javascript" src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>-->
    <!--<script type="text/javascript" src="https://connect.facebook.net/en_US/all.js"></script>-->
    <!-- link which helped us to solve touch problem in jquery - http://touchpunch.furf.com/ and its script is used below -->
    <script type="text/javascript" src="resources/javascript/jquery.ui.touch-punch.js"></script>
  
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
    <script src="App_Themes/ONLINE/pushy.min.js"></script>
    <script type="text/javascript">
jQuery(document).ready(function(){              
$(".menu-btn").click(function() {
    $(this).removeClass('.menu-btn').addClass('bb');
});
});
    </script>
    
    
 
    
    
</head>
<body>
    <script language="javascript" type="text/javascript">
        var sessionTimeoutWarning = 
	"<%= System.Configuration.ConfigurationSettings.AppSettings
	["SessionWarning"].ToString()%>";
        var sessionTimeout = "<%= Session.Timeout %>";
        var timeOnPageLoad = new Date();
 
        //For warning
        setTimeout('SessionWarning()', parseInt(sessionTimeoutWarning) * 60 * 1000);
        //To redirect to the welcome page
        setTimeout('RedirectToWelcomePage()',parseInt(sessionTimeout) * 60 * 1000);

        //Session Warning
        function SessionWarning() {
            //minutes left for expiry
            var minutesForExpiry =  (parseInt(sessionTimeout) - 
				parseInt(sessionTimeoutWarning));
            var message = "Your session will expire in another " + minutesForExpiry + 
			" mins! Please Save the data before the session expires";
            alert(message);
            var currentTime = new Date();
            //time for expiry
            var timeForExpiry = timeOnPageLoad.setMinutes(timeOnPageLoad.getMinutes() 
				+ parseInt(sessionTimeout)); 

            //Current time is greater than the expiry time
            if(Date.parse(currentTime) > timeForExpiry)
            {
               show_session_timeout_message();
            }
        }

        //Session timeout
        function RedirectToWelcomePage(){
           show_session_timeout_message();
        }
    </script>
    
 
    <audio id="sound_bet" src="../resources/waves/Bet-5.mp3" preload="auto"></audio>
    <audio id="sound_fold" src="../resources/waves/Fold-3.wav" preload="auto"></audio>
    <audio id="sound_deal" src="../resources/waves/Deal-6.mp3" preload="auto"></audio>
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
    <!--<script>        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = "https://connect.facebook.net/en_US/all.js#xfbml=1&appId=119665184821685";
            fjs.parentNode.insertBefore(js, fjs);
        } (document, 'script', 'facebook-jssdk'));
    </script>-->
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true"
        ScriptMode="Release" LoadScriptsBeforeUI="true">
    </asp:ToolkitScriptManager>
    <!-- Pushy Menu -->
      <asp:HiddenField ID="hdimage" runat="server" />
    <nav class="pushy pushy-left">
            <ul>
            
             <li>
               
               <a href="#"> 
                  <div id="playrealbutton"> 
                     <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
                     <ContentTemplate>
                        <asp:Button ID="btn_play_for_real" runat="server" Text="Play for Real" OnClientClick=
                        "show_play_for_real_text();" OnClick="btn_play_for_real_Click" value="Play for Real" />
                     </ContentTemplate>
                     </asp:UpdatePanel>
                  </div>    
               </a>  
                                    
                 </li>
             
                <li>
                   <a href="#"> 
                      <div id="menu_exit_button"> 
                         <asp:UpdatePanel ID="UpdatePanel3" runat="server" RenderMode="Inline">
                         <ContentTemplate>
                            <asp:Button 
                                ID="Button1" 
                                runat="server" 
                                Text="Exit" 
                                OnClick="btn_leave_table_Click" 
                                value="Leave Table" />
                         </ContentTemplate>
                         </asp:UpdatePanel>
                      </div>    
                   </a>  
                </li>
             
                <li>
                   
                    <!-- <div id='command_buttons_container'>-->
                
                    <!-- Lobby button -->
                    <div id='btn_lobby'>
                        <asp:ImageButton 
                            ID="btnLobby" 
                            runat="server" 
                            ImageUrl="../resources/buttons/btn_lobby.png"
                            OnClick="btnLobby_Click" />
                    </div>
                 </li>
             
                <!-- Help button -->
                <li>
                    <div id='btn_help'>
                       <asp:ImageButton 
                           ID="btn_help_asp" 
                           runat="server" 
                           ImageUrl="../resources/buttons/btn_help.png"
                           OnClick="btnLobby_Click" />
                    </div>
                 </li>
                 
                 <!-- Rules button -->
                 <li>  
                     <a href="#">
                         <div id='btn_rules' class="online" onClick="show_rules();">
                             Rules
                         </div>
                     </a>
                 </li>   
                             
                  <!-- Get Chips button -->
                 <li>
                    <a href="#">  
                        <div id='btn_get_chips' 
                            onClick="show_get_chips();">
                            Get Chips
                       </div>
                    </a>
                  </li>

                <li> <!-- Cashier button -->
                    <a href="#">  
                       <div id='btn_cashier' 
                           onClick="btn_cashier_Click();" 
                           runat="server">
                           Cashier
                       </div>
                    </a>
                </li>
               
                  <!-- Show Admin Button (Show odds for all hands)-->
                    <div id='btn_show_admin'>
                       <asp:ImageButton ID="btn_Show_Admin_Flag" runat="server"
                           ImageUrl="../resources/buttons/btn_debug.png"
                           Text="Show Admin" 
                           OnClick="btn_Show_Admin_Flag_Click" 
                           Visible="false" />
                    </div>
                
                     <!-- Hide Admin Button (Show odds for all hands)-->
                    <div id='btn_hide_admin'>
                       <asp:ImageButton ID="btn_Hide_Admin_Flag" runat="server"
                           ImageUrl="../resources/buttons/btn_debug.png"
                           Text="Hide Admin" 
                           Visible="false" 
                           OnClick="btn_Hide_Admin_Flag_Click" />
                    </div>

               <!--  </div> commented out this div 2 Dec 2015-->
                           
               </li>
               
                            
               
              
            </ul>
        </nav> <div id="toggle"> <div class="menu-btn">
        </div></div>
    <!-- Site Overlay -->
    <div class="site-overlay">
    </div>
    <div id="container" class="containerr">
      
        <div class="topbar_fixed">
         
             
                    <div id="social_media_container">
                        <div class="alignment">
                            <!-- Facebook Invite Friends -->
                            <a href="#" onClick="Facebook_Invite_Friends()" class="fb-invite"></a>
                            <!-- Twitter -->
                            <!-- Commented out by Simon as platform.twitter does not resolve
                            <div id="twitter_container">
                                <a href="https://twitter.com/TexasHedgeEm" class="twitter-follow-button" data-show-count="false"
                                    data-show-screen-name="false">Follow @TexasHedgeEm</a>
                                <script>                                    !function (d, s, id) { var js, fjs = d.getElementsByTagName(s)[0], p = /^http:/.test(d.location) ? 'http' : 'https'; if (!d.getElementById(id)) { js = d.createElement(s); js.id = id; js.src = p + '://platform.twitter.com/widgets.js'; fjs.parentNode.insertBefore(js, fjs); } } (document, 'script', 'twitter-wjs');</script>
                            </div>
                                -->
                            <!-- Facebook Like Button -->
                            <div class="fb-like" data-href="https://apps.facebook.com/texashedgeem" data-layout="button_count"
                                data-action="like" data-show-faces="false" data-share="false">
                            </div>
                            <li class="ext"><a href="#">
                                <div id="Logout" runat="server" class="btn-group login">
                                    <asp:Label ID="lbl_user_name" runat="server" Text=""></asp:Label>
                                    <asp:Button ID="btnLogout" runat="server" Text="Exit" OnClick="btn_leave_table_Click" />
                                   
                                </div>
                            </a></li>
                        </div>
                    </div>
            
           
        </div>
      
            <div id="hedgeem_table_container" class="hedgeem">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <!-- 'Background to everything on the HedgeEm Table main display - typically the Casino carpet -->
                        <div class="scaling">
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
                            <div id="table_logo">
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
                            <div id="table_jackpot_container" runat="server" visible="true">
                                <asp:PlaceHolder ID="Place_Holder_Table_Jackpot" runat="server"></asp:PlaceHolder>
                            </div>
                            <asp:PlaceHolder ID="Place_Holder_Winner_Message" runat="server"></asp:PlaceHolder>
                            <!-- Play for real button -->
                            
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
                            <!-- Cashier Div-->
                            <div id="cashier_div" class="white">
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
                            <!-- plaece again-->
                            <!-- buttons panel-->
                            <div class="buttons_panel">
                              
                                    <!-- 19-->
                                   
                                    <!-- 19-->
                                    <!-- Deal Buttons Container -->
                                    <div id='btn_deal_buttons_container'>
                                        <!-- Deal HOLE Button -->
                                        <asp:Button ID="btnDealHole" runat="server" OnClick="btn_deal_next_stage_Click" OnClientClick="javascript:disableButton(this,3000);"
                                                Text="Deal Hole" value="Deal Hole" />
                                        <!-- Deal FLOP Button -->
                                            <asp:Button ID="btnDealFlop" runat="server" class="plokplok" OnClick="btn_deal_next_stage_Click" OnClientClick="javascript:disableButton(this,3000);"
                                              
                                                Text="Deal Flop" value="Deal Flop" />
                                            <!-- Deal TURN Button -->
                                            <asp:Button ID="btnDealTurn" runat="server" OnClick="btn_deal_next_stage_Click" OnClientClick="javascript:disableButton(this,3000);"
                                                Text="Deal Turn" value="Deal Turn" />
                                            <!-- Deal RIVER Button -->
                                            <asp:Button ID="btnDealRiver" runat="server" OnClientClick="javascript:disableButton(this,3000);"
                                                Text="Deal River" OnClick="btn_deal_next_stage_Click" value="Deal River" />
                                            <!-- Deal Next Game Button -->
                                            <asp:Button ID="btnNextGame" runat="server" Text="Next Game" OnClick="btn_deal_next_stage_Click"
                                                OnClientClick=" disableButton(this,3000);" value="Next Game" />
                                        
                                          <!--Do NOT delete this empty DIV is is used and will break app Simon Nov 2015 - Ask Jatin why he added this-->
                                        <!--<div class="dealbtnset">
                                        
                                                   
                                            </div>-->
                                  
                                    <!-- End of Command buttons content -->
                                </div>

                                <!-- Player Content -->
                                        
                                <!--<div>  Do not add this Div added by Simon Nov 2015 to connect with what appeared to be orphanned div below -->
                                <asp:UpdatePanel ID="updPanl_to_avoid_Postback" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                            <script type="text/javascript">
                                                function hide_rm() {
                                                    document.getElementById('hide_rm').style.display = 'none';
                                                    document.getElementById('rm').style.display = 'none';
                                                }
    </script>
                                                <!-- Player_Info -->
                                                <div id="Player_Info" runat="server">
                                                    <asp:PlaceHolder ID="Place_Holder_Player_Info" runat="server"></asp:PlaceHolder>
                                                </div>
                                                <!-- Odds_bets -->

                                                <!-- Commented out by Simon Nov 2015 - need to ask Webguruz what this is for
                                                <div id="Place_Holder_Odds_bets_sample" runat="server">
                                                    <asp:PlaceHolder ID="Place_Holder_Odds_bets" runat="server"></asp:PlaceHolder>
                                                </div>
                                                </div>-->
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        
                            </div>
                            <!-- buttons panel-->
                            <!-- Error Message content -->
                            <div id="error_message" class="white_content">
                                <div class="modal-header">
                                    <div id="hide_rules" onClick="javascript:closediv();" class="close">
                                        <span aria-hidden="true">×</span>
                                    </div>
                                </div>
                                <div id='short_description' runat="server">
                                </div>
                                <div id='long_description' runat="server">
                                </div>
                            </div>
                           
                    </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- Rules Div -->
                <div id="content" class="div_rules_content white_content modal-content animated bounceInDown">
                    <div class="modal-header">
                        <div id="hide_rules" onClick="hide_rules();" class="close">
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
                <!-- Play for real pop up div -->
                <div id="play_for_real_text" class="div_rules_content white_content">
                    <div class="modal-header">
                        <div id="hide_play_for_real_text" onClick="hide_play_for_real_text();" class="close">
                            <span aria-hidden="true">×</span>
                        </div>
                    </div>
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
                        <asp:Button ID="btn_play_for_real_deposit_pledge" CssClass="btn btn-success" runat="server"
                            Text="Submit" ValidationGroup="play_for_real" OnClick="btn_play_for_real_deposit_pledge_Click" />
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
                <div id="get_chips" class="div_rules_content white_content modal-content animated bounceInDown">
                    <div class="modal-header">
                        <div id="hide_get_chips" onClick="hide_get_chips();" class="close">
                            <span aria-hidden="true">×</span>
                        </div>
                        <h4 id="myModalLabel" class="modal-title">
                            Get Chips</h4>
                    </div>
                    <p>
                        Hedge'Em is a free to play game!</p>
                    <p>
                        Please help us keep it that way by doing any of the following to get your free chips.</p>
                    <div id='option_visit_these_sites' class="alternatives_section_heading">
                        Visit one of our favourite sites ...
                    </div>
                    <div>
                    <iframe style="width:120px;height:240px;" marginwidth="0" marginheight="0" scrolling="no" frameborder="0" src="//ws-eu.amazon-adsystem.com/widgets/q?ServiceVersion=20070822&OneJS=1&Operation=GetAdHtml&MarketPlace=GB&source=ac&ref=qf_sp_asin_til&ad_type=product_link&tracking_id=teheem01-21&marketplace=amazon&region=GB&placement=B019PQPYVY&asins=B019PQPYVY&linkId=&show_border=true&link_opens_in_new_window=true">
</iframe>
                    </div>
                    <!-- Fulltilt -->
                    <div id='action_item_fulltilt' class="get_chips_content_divs" onclick='ai_fultilt_click();'>
                        <div id='ai_fult_tilt_image' class="get_chips_image">
                            <img src="../resources/buttons/full_tilt.png" />
                        </div>
                        <div id='ai_full_tilt_description'>
                            Fulltilt
                        </div>
                    </div>
                    <!-- BetFair -->
                    <div id='action_item_betfair' class="get_chips_content_divs" onclick='ai_betfair_click();'>
                        <div id='ai_BetFair_image' class="get_chips_image">
                            <img src="../resources/buttons/bet_fair.png" />
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
                            <img style="padding-right: 7px" class="fbr" src="../resources/buttons/fb_recommend.jpg" />
                        </div>
                        <div id='ai_recommend_facebook_description'>
                            Recommend to a friend on Facebook
                        </div>
                    </div>
                    <!-- Share with Facebook -->
                    <div id='action_item_share_facebook' class="get_chips_content_divs" onclick='ai_fb_share_click();'>
                        <div id='ai_share_facebook_image' class="get_chips_image">
                            <img class="fbr" src="../resources/buttons/fb_share.jpg" />
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
                    <input class="btn btn-success" type="button" onClick="javascript:window.location='frm_website_home.aspx';"
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
                                <input type="button" class="cancel_place_bet_button" onClick="hide_bet_popup1();" />
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
                                <input type="button" class="cancel_place_bet_button" onClick="hide_bet_popup2();" /></div>
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
                                <input type="button" class="cancel_place_bet_button" onClick="hide_bet_popup3();" /></div>
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
                                <input type="button" class="cancel_place_bet_button" onClick="hide_bet_popup4();" /></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
               
            </div>
      <div id="popup_message">
                <script type="text/javascript" >
                    function onclose() {
                        document.getElementById('hide_edit_profile').style.display = 'none';
                        document.getElementById('popup_message').style.display = 'none';
                    }

                </script>
                    <asp:PlaceHolder ID="Place_Holder_Popup_Message" runat="server"></asp:PlaceHolder>
                </div>
      
        </div>
        
   
  
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
            DisplayAfter="10">
            <ProgressTemplate>
                <div class="loading" align="center">
                    <img src="../resources/loading.gif" alt="" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div id="fade_rules" class="green_overlay" onClick="hide_rules();">
        </div>
        <div id="fade_get_chips" class="green_overlay" onClick="hide_get_chips();">
        </div>
        <div id="fade_play_for_real" class="green_overlay" onClick="hide_play_for_real_text();">
        </div>
        <div id="fade_session_timeout_message" class="fade_overlay">
        </div>
        <div id="fade_hedgeem_bet_slider" class="no_opaque_overlay" onClick="hide_bet_popup();">
        </div>
    </form>
    </div>
    <script>
        $(".menu-btn").click(function () {
            $(this).toggleClass("highlight");

        });

        $(".site-overlay").click(function () {
            $(".menu-btn").removeClass("highlight");

        });
    </script>

  
</body>

  
            
            <script> 
 /**
/**
 * Polyfill for VW, VH, VM, VMIN units. Handles int and float values, both positive and negative.
 * @depends StyleFix from -prefix-free http://leaverou.github.com/prefixfree/
 * @author Lea Verou, Xandor Schiefer, Viktor Bezdek
 */
(function() {
 
    // dependence je prefixfree.js
    if (!window.StyleFix) {
        console.warn('Viewport units requires prefixfree.js.');
        return;
    }
 
    // check capabilities
    var dummy = document.createElement('_').style,
        units = ['vw', 'vh', 'vm', 'vmin'].filter(function(unit) {
            dummy.width = '';
            dummy.width = '10' + unit;
            return !dummy.width;
        });
 
    if (!units.length) {
        return;
    }
 
    // parse int & float values
    // pattern info - http://regex101.com/r/oO9fA9/3
    var searchPattern = RegExp('(-?[a-z]+(-[a-z]+)*\\s*:\\s*)(-?[0-9.]+)(' + units.join('|') + ')\\b(\\s*;.\\s*(-?[a-z]+(-[a-z]+)*\\s*:\\s*)\\b([0-9]*\\.?[0-9]+)(px)\\b)?', 'gi');
 
    // register CSS processing function
    StyleFix.register(function(css) {
        var w = innerWidth,
            h = innerHeight,
            m = Math.min(w, h);
        return css.replace(searchPattern,
            function(match, property, sub_property, num, unit) {
                console.log('match');
                return property + num + unit + ';' + property + ((num * (unit == 'vw' ? w : unit == 'vh' ? h : m) / 100) + 'px');
            }
        );
    });
 
    // throttled function to update view after orientation change
    var handleOrientationChange = (function() {
        'use strict';
 
        var timeWindow = 200; // ms throttle time
        var lastExecution = new Date((new Date()).getTime() - timeWindow);
 
        var handleOrientationChange = function() {
            // process style fix
            StyleFix.process();
        };
 
        return function() {
            if ((lastExecution.getTime() + timeWindow) <= (new Date()).getTime()) {
                lastExecution = new Date();
                return handleOrientationChange.apply(this, arguments);
            }
        };
    }());
 
    // handle viewport resize and orientation change
    window.addEventListener('resize', handleOrientationChange, false);
    window.addEventListener('orientationchange', handleOrientationChange, false);
 
})();

</script>
            
            
       <script type="text/javascript" src="resources/javascript/tokenizer.js"></script>
         <script type="text/javascript" src="resources/javascript/parser.js"></script>
        <script type="text/javascript" src="resources/javascript/vminpoly.js"></script> 
    
</html>
