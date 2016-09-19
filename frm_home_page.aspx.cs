using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Media;
using System.Data;
using System.Text;
using System.Web.Services;
using HedgeEmClient;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web.Configuration;
using System.Threading;



public partial class frm_home_page : System.Web.UI.Page
{

    #region Controls
    hedgeem_control_card cc_flop_card1;
    hedgeem_control_card cc_flop_card2;
    hedgeem_control_card cc_flop_card3;
    hedgeem_control_card cc_turn_card;
    hedgeem_control_card cc_river_card;
    hedgeem_control_seats ss_seat;
    hedgeem_hand_panel[] _hedgeem_hand_panels;
    BETTING_PANEL[,] _hedgeem_betting_panels;
    #endregion

    int _xxxHCnumber_of_hands = 4;
    int _table_id;
    int _xxxHCnumber_of_seats = 1; // xxx_HC value
    Double xxxHC_a_opening_balance;
    String xxxHC_a_password = "";
    String xxxHC_a_full_name = "";

    int chk_admin_flag_value = 0;
    enum_game_state _game_state;
    double my_jackpot_fund;
    string p_flop_card1_as_short_string;
    string p_flop_card2_as_short_string;
    string p_flop_card3_as_short_string;
    string p_turn_as_short_string;
    string p_river_as_short_string;
    string p_game_id;
    enum_theme my_default_theme;

    string p_game_status_as_json_string;
    String p_odds_percent_win_or_draw_string;
    String p_odds_percent_win_string;
    String p_odds_percent_draw_string;
    String p_odds_actual_string;
    double p_odds_percent_win_or_draw_double;
    double p_odds_actual_double;
    double p_odds_margin;
    double xxxp_odds_margin_rounded;
    double p_presented_margin;

    int _int_number_of_betting_stages;
    WebClient wc = new WebClient();
    double player_funds_at_seat;
    int p_total_bets;
    string p_hand_card1;
    string p_hand_card2;
    enum_hand_in_play_status p_inplay_status;
    enum_acknowledgement_type p_ack_type;
    string _bet_value;
    int xxx_HC_seat_index = 0;
    localhost.WebService service = new localhost.WebService();
    HedgeEmLogEvent _xxx_log_event = new HedgeEmLogEvent();
    int playerid;


    //For Logging
    private static String logger_name_as_defined_in_app_config = "client." + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(logger_name_as_defined_in_app_config);
    HedgeEmGameState _global_game_state_object = new HedgeEmGameState();

    // preinit event changes the theme of the page at runtime before page loads
    protected void Page_PreInit(object sender, EventArgs e)
    {
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = "frm_home_page.aspx.cs-Page_PreInit";
        my_log_event.p_message = "Method called";
        log.Debug(my_log_event.ToString());
        //ThreadStart tsTask = new ThreadStart(TaskLoop);
        //Thread MyTask = new Thread(tsTask);
        //MyTask.Start();
        try
        {
            if (Session["theme"] == null)
            {
                my_log_event.p_message = "Warning - hard coding of theme to ONLINE (should never get here)";
                log.Warn(my_log_event.ToString());
                Session["theme"] = "ONLINE";
                this.Page.Theme = this.Page.Theme = Session["theme"].ToString();
            }
            else
            {
                // this changes the theme of the hedgeem table according to the theme selected by the user.
                this.Page.Theme = Session["theme"].ToString();
                if (this.Page.Theme == "") ;
                {
                    // xxxeh - need to throw error when string =""
                    this.Page.Theme = "ONLINE";
                }
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in Page PreInit - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = ex.Message;
            my_log.p_method_name = "Page Load";
            my_log.p_player_id = f_get_player_id();
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }

    // method to get values from SVC web service when required
    private object f_get_object_from_json_call_to_server(string endpoint, Type typeIn)
    {
        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
        my_log.p_message = String.Format("Method called with endpoint [{0}]", endpoint); ;
        my_log.p_method_name = "f_get_object_from_json_call_to_server";
        my_log.p_player_id = f_get_player_id();
        my_log.p_table_id = _table_id;
        log.Debug(my_log.ToString());
        object obj = null;
        HttpWebRequest request;
        String my_service_url = "not set";

        Boolean my_use_localhost_webservice = Convert.ToBoolean(WebConfigurationManager.AppSettings["use_localhost_webservice"]);

        my_log.p_message = String.Format("WARNING! If a exception happens in this function is does not appear to user.");
        log.Warn(my_log.ToString());

        if (my_use_localhost_webservice == true)
        {
            // Create the web request
            my_service_url = "http://localhost:59225/Service1.svc/";
            my_log.p_message = String.Format("Webservice configured for local development URI = [{0}{1}]", my_service_url, endpoint); ;

        }
        else
        {
            my_service_url = "http://devserver.hedgeem.com/Service1.svc/";
            my_log.p_message = String.Format("Webservice configured for GoDaddy Deployment URI = [{0}{1}]", my_service_url, endpoint); ;

        }

        log.Debug(my_log.ToString());
        request = WebRequest.Create(my_service_url + endpoint) as HttpWebRequest;


        // Get response
        if (request != null)
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response != null)
                {
                    Stream stream = response.GetResponseStream();

                    if (stream != null)
                    {

                        var reader = new StreamReader(stream);
                        try
                        {
                            string json = reader.ReadToEnd();
                            // Check if stream is empty, if it is throw and exception
                            if (json.Length == 0)
                            {
                                String my_error_msg = String.Format("JSON string has been returned empty for URI endpoint [{0}]", request.Address);
                                my_log.p_message = my_error_msg;
                                log.Error(my_log.ToString());
                                throw new Exception(my_error_msg);
                            }
                            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
                            if (typeIn != null)
                            {
                                var ser = new DataContractJsonSerializer(typeIn);
                                obj = ser.ReadObject(ms);
                            }
                            else
                            {
                                obj = json;
                            }
                        }

                        finally
                        {
                            reader.Close();
                        }
                    }
                }
            }

        return obj;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
        my_log.p_message = "frm_home_page.aspx.cs method called.";
        my_log.p_method_name = "Page_Load";
        my_log.p_player_id = f_get_player_id();
        my_log.p_game_id = p_game_id;
        my_log.p_table_id = _table_id;
        log.Debug(my_log.ToString());
        try
        {
            // checks if session is timed out
            if (Session.Count == 0)
            {
                Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>show_session_timeout_message();</script>");
            }
            else
            {
                my_log.p_message = "Warning: Hardcoding of table ID";
                log.Warn(my_log.ToString());
                int xxx_HC_table_id_for_shared_home_page_table = 1000;
                int my_player_id = Convert.ToInt32(Session["playerid"]);
                _table_id = xxx_HC_table_id_for_shared_home_page_table;

                if (Page.IsPostBack == false)
                {
                    try
                    {
                        _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_game_state_object/" + _table_id, typeof(HedgeEmGameState));
                        // _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_next_game_state_object/" + _table_id + "," + f_get_player_id(), typeof(HedgeEmGameState));
                        my_log.p_message = String.Format("Successfully retrieved gamestate from server. Table ID [{0}], State [{1}]", _global_game_state_object.p_table_id, _global_game_state_object.p_current_state_enum.ToString());
                        log.Debug(my_log.ToString());
                    }
                    catch (Exception ex)
                    {
                        my_log.p_message = String.Format("Error trying to get game state from server. Reason [{0}]", ex.Message);
                        log.Error(my_log.ToString());

                    }
                    if (Session["user_role"] != null)
                    {
                        string role = Session["user_role"].ToString();
                        // if user is admin, show cashier button
                        if (role == enum_user_role.ADMIN.ToString())
                        {
                            btn_cashier.Visible = true;
                        }
                    }

                    table_jackpot_container.Visible = false;

                    ScriptManager sManager = ScriptManager.GetCurrent(this.Page);

                    /*
                    //Get Image from Facebook if the user is logged in via facebook
                    string facebook_imageurl = "";
                    if (Session["Facebook_User_Id"] != "")
                    {
                        //Get Image from Facebook
                        facebook_imageurl = "https://graph.facebook.com/" + Session["Facebook_User_Id"].ToString() + "/picture";
                    }
                    if (facebook_imageurl != "")
                    {
                        // Get path to save the image
                        string pathToSave = Server.MapPath("~/resources/") + "player_avatar_" + Session["username"].ToString() + ".jpg";
                        //Check if the Image exists already
                        if (!File.Exists(pathToSave))
                        {
                            //Save the image
                            WebClient client = new WebClient();
                            client.DownloadFile(facebook_imageurl, pathToSave);
                        }
                    }
                    */

                    p_game_id = _global_game_state_object.p_game_id;
                    int my_number_of_hands = _global_game_state_object.p_number_of_hands_int;
                    enum_betting_stage my_betting_stage = f_get_current_betting_stage();
                    _game_state = _global_game_state_object.p_current_state_enum;
                    _hedgeem_hand_panels = new hedgeem_hand_panel[my_number_of_hands];
                    //   Session["sess_betting_stage_enum"] = _global_game_state_object.p_current_betting_stage_enum;
                    _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
                    _hedgeem_betting_panels = new BETTING_PANEL[_int_number_of_betting_stages, my_number_of_hands];
                    lbl_game_id.Text = String.Format("Table/Game: {0}/{1} ", _global_game_state_object.p_table_id, p_game_id);
                    // gets seat balance of the current player
                    player_funds_at_seat = _global_game_state_object._seats[0].p_player_seat_balance;

                    f_call_functions_to_render_screen();
                }
                // Dynamically contructed the Web page title so show relevant info about Server, Table and Game the player is playing. 
                //   String my_page_title = String.Format("Texas Hedge'Em Poker | Server [{0}], Table {1}], Game [{2}]","server id", p_table_name, p_table_id);
                //   this.Page.Title = my_page_title;
                //  Page.RegisterStartupScript("call deal button code", "<script>setInterval('" + btn_deal_next_stage_Click() + "',3000);</script>");
                //   btn_deal_next_stage_Click();
                /*Click on Hand_Panel to get the value of current hand via _click_hand_index  and then that value pass to hidden textbox i.e mytext, when Hand_Index_Value is shown in textbox then btn_Get_Clicked_Hand_Value method to get the value of bet that we placed i.e HOLE: £1 bet pays £4*/
                Page.RegisterStartupScript("Bet_Placed_Details", "<script>f_placebet(_click_hand_index);</script>");

                if (Session["username"] != null)
                {

                    Logout.Attributes.Add("style", "display:block!Important;");
                    Page.RegisterStartupScript("OnLoading", "<script>load_edit_profile();</script>");
                    if (Session["display_name"] != null)
                    {
                        lbl_user_name.Text = Session["display_name"].ToString();
                    }
                    LoginDiv.Attributes.Add("style", "display:none !Important;");
                    usr_image.ImageUrl = "../resources/player_avatar_" + Session["username"].ToString() + ".jpg";
                    btn_play_now.Enabled = true;
                    // check user role
                    DataTable dt = service.f_get_password_from_db(Session["username"].ToString());

                    string role = "";
                    if (dt.Rows.Count > 0)
                    {
                        string password = dt.Rows[0]["password"].ToString();
                        DataTable userdetails = service.my_user_details(Session["username"].ToString(), password);
                        role = userdetails.Rows[0]["role"].ToString();

                    }
                    if (role == enum_user_role.ADMIN.ToString())
                    {
                        btnAdmin.Visible = true;
                    }
                    Session["user_role"] = role;
                    Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('progressbar').style.display='none';</script>");
                }
            }
        }
        //catch (FaultException<Exception_Fault_Contract> ex)
        //{

        //}

        catch (Exception ex)
        {
            //string my_error_popup = "alert('Error in Page Load - " + ex.Message.ToString() + "');";
            string my_error_popup = "Major Error in Page_load";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error(my_log.ToString());
            //throw new Exception(my_error_popup); 
        }
    }

    protected void btnAdmin_Click(object sender, EventArgs e)
    {
        log.Info("User clicked on Admin Button");
        try
        {
            if (txt_get_fm_email_id.Text == "")
            {
                Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>alert('You are not logged in. Please log in to view this Page.');document.getElementById('progressbar').style.display='none'; </script>");
            }
            else
            {
                DataTable get_password = new DataTable();
                DataTable user_details = new DataTable();

                get_password = service.f_get_password_from_db(txt_get_fm_email_id.Text);
                if (get_password.Rows.Count == 0)
                {
                    Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>alert('You are not authorized to view this page.');</script>");
                }
                else
                {
                    string password = get_password.Rows[0]["password"].ToString();

                    user_details = service.my_user_details(txt_get_fm_email_id.Text, password);
                    // Check you got only one record back, if not throw appropriate exception
                    if (user_details.Rows.Count < 1)
                    {
                        throw new Exception(String.Format("Username '{0}' not found. Username or password incorrect.", txt_get_fm_email_id.Text));
                    }
                    else if (user_details.Rows.Count > 1)
                    {
                        throw new Exception(String.Format("Multiple users found for username '{0}'.  This indicates something is wrong so cancelling operation..", txt_get_fm_email_id.Text));
                    }
                    else
                    {
                        enum_user_role role = (enum_user_role)Enum.Parse(typeof(enum_user_role), user_details.Rows[0]["role"].ToString());
                        if (role == enum_user_role.ADMIN)
                        {
                            Response.Redirect("http://serveradmin.hedgeem.com");
                        }
                        else
                        {
                            Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>alert('You are not authorized to view this page.');</script>");
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in method [btnAdmin_Click] - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Error in btnAdmin_Click", new Exception(ex.Message));
        }
    }

    #region Sound_Play_On_Click_Of_Deal_Button
    // This function plays sound on the click of deal button
    private void PlaySound()
    {
        log.Debug("PlaySound method of Hedgeem Table is called");
        try
        {
            SoundPlayer sd = new SoundPlayer();
            sd.SoundLocation = Server.MapPath("~/resources/waves/Deal-4.wav");
            sd.Play();
            sd.Play();
            sd.Play();
            sd.Play();
            sd.Play();
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in PlaySound method - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = ex.Message;
            my_log.p_method_name = "PlaySound";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }
    #endregion Sound_Play_On_Click_Of_Deal_Button

    private int f_get_player_id()
    {
        return 10000;
    }

    //#region f_activate_deal_hole_button_and_hide_others
    //private void f_activate_deal_hole_button_and_hide_others()
    //{
    //    /*----- This function only shown Deal Display Random others are invisible------*/
    //    btnDealHole.Visible = true;
    //    btnDealFlop.Visible = false;
    //    btnDealTurn.Visible = false;
    //    btnDealRiver.Visible = false;
    //    btnNextGame.Visible = false;
    //}
    //#endregion

    //#region f_activate_deal_flop_button_and_hide_others
    //private void f_activate_deal_flop_button_and_hide_others()
    //{
    //    /*----- This function only shown Deal Flop others are invisible------*/
    //    btnDealHole.Visible = false;
    //    btnDealFlop.Visible = true;
    //    btnDealTurn.Visible = false;
    //    btnDealRiver.Visible = false;
    //    btnNextGame.Visible = false;
    //    btnLobby.Visible = true;
    //}
    //#endregion

    //#region f_activate_deal_turn_button_and_hide_others
    //private void f_activate_deal_turn_button_and_hide_others()
    //{
    //    /*----- This function only shown Deal Turn others are invisible------*/
    //    btnDealHole.Visible = false;
    //    btnDealFlop.Visible = false;
    //    btnDealTurn.Visible = true;
    //    btnDealRiver.Visible = false;
    //    btnNextGame.Visible = false;
    //    btnLobby.Visible = true;
    //}
    //#endregion Visible_Deal_Turn_Buttons

    //#region f_activate_deal_river_button_and_hide_others
    //private void f_activate_deal_river_button_and_hide_others()
    //{
    //    /*----- This function only shown Deal River others are invisible------*/
    //    btnDealHole.Visible = false;
    //    btnDealFlop.Visible = false;
    //    btnDealTurn.Visible = false;
    //    btnDealRiver.Visible = true;
    //    btnLobby.Visible = true;
    //}
    //#endregion Visible_Deal_River_Buttons

    //#region f_activate_deal_next_button_and_hide_others
    //private void f_activate_deal_next_button_and_hide_others()
    //{
    //    /*----- This function only shown Deal Next others are invisible------*/
    //    btnDealHole.Visible = false;
    //    btnDealFlop.Visible = false;
    //    btnDealTurn.Visible = false;
    //    btnDealRiver.Visible = false;
    //    btnNextGame.Visible = true;
    //}
    //#endregion Visible_Deal_Next_Buttons

    /// Context / Background reading
    /// ----------------------------
    /// You must understand the following before understanding this function
    /// + What the HedgeEmHandStageInfo class is and what its purpose is.
    /// 
    /// History
    /// -----------
    /// Original Author: Simon Hewins Jul 2014
    /// Last edit:       Simon Hewins Aug 2014
    /// 
    /// Description
    /// -----------
    /// 
    /// Gets a LIST of HedgeEmHandStageInfo objects for any given hand (index) at any given stage.
    /// </summary>
    /// <param name="a_enum_game_state"></param>
    /// <param name="a_hand_index"></param>
    /// <returns></returns>
    public List<HedgeEmBet> f_get_previous_bets_for_stage_and_hand_player_list(enum_betting_stage a_enum_betting_stage, int a_hand_index, int a_player_id)
    {
        /// xxx hack until Bet contains player id
        a_player_id = 0;

        List<HedgeEmBet> my_previous_bets_list = new List<HedgeEmBet>();
        try
        {
            my_previous_bets_list = (from handsstage_objects in _global_game_state_object.p_recorded_bets
                                     where handsstage_objects.p_enum_betting_stage == a_enum_betting_stage
                         && handsstage_objects.p_hand_index == a_hand_index
                         && handsstage_objects.p_seat_index == a_player_id
                                     select handsstage_objects).ToList();
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_get_previous_bets_for_stage_and_hand" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_get_hand_stage_info_object_for_stage_and_hand function " + ex.Message;
            my_log.p_method_name = "f_get_hand_stage_info_object_for_stage_and_hand";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
        return my_previous_bets_list;
    }

    /// Context / Background reading
    /// ----------------------------
    /// You must understand the following before understanding this function
    /// + What the HedgeEmHandStageInfo class is and what its purpose is.
    /// 
    /// History
    /// -----------
    /// Original Author: Simon Hewins Jul 2014
    /// Last edit:       Simon Hewins Aug 2014
    /// 
    /// Description
    /// -----------
    /// 
    /// Gets a LIST of HedgeEmHandStageInfo objects for any given hand (index) at any given stage.
    /// </summary>
    /// <param name="a_enum_game_state"></param>
    /// <param name="a_hand_index"></param>
    /// <returns></returns>
    public HedgeEmBet f_get_previous_bets_for_stage_and_hand_player(enum_betting_stage a_enum_betting_stage, int a_hand_index, int a_player_id)
    {
        /// xxx hack until Bet contains player id
        a_player_id = 0;

        List<HedgeEmBet> my_previous_bets_list = new List<HedgeEmBet>();

        HedgeEmBet myHedgeEmBet = null;

        try
        {
            my_previous_bets_list = (from handsstage_objects in _global_game_state_object.p_recorded_bets
                                     where handsstage_objects.p_enum_betting_stage == a_enum_betting_stage
                         && handsstage_objects.p_hand_index == a_hand_index
                         && handsstage_objects.p_seat_index == a_player_id
                                     select handsstage_objects).ToList();

            if (my_previous_bets_list.Count > 1)
            {
                String my_err_msg = String.Format("Expected only one 'Bet' for state [{0}], hand [{1}] object got [{1}] ",
                    a_enum_betting_stage.ToString(),
                    a_hand_index,
                    my_previous_bets_list.Count);

                throw new Exception(my_err_msg);
            }
            myHedgeEmBet = my_previous_bets_list[0];

        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_get_previous_bets_for_stage_and_hand" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_get_hand_stage_info_object_for_stage_and_hand function " + ex.Message;
            my_log.p_method_name = "f_get_hand_stage_info_object_for_stage_and_hand";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
        return myHedgeEmBet;
    }

    private enum_game_state f_convert_hedgeem_stage_to_state(enum_betting_stage a_betting_stage)
    {
        enum_game_state my_enum_game_state_HACK = enum_game_state.UNCHANGED;
        switch (a_betting_stage)
        {
            case enum_betting_stage.HOLE_BETS:
                my_enum_game_state_HACK = enum_game_state.STATUS_HOLE;
                break;
            case enum_betting_stage.FLOP_BETS:
                my_enum_game_state_HACK = enum_game_state.STATUS_FLOP;
                break;
            case enum_betting_stage.TURN_BETS:
                my_enum_game_state_HACK = enum_game_state.STATUS_TURN;
                break;

            default:
                break;
        }

        return my_enum_game_state_HACK;
    }


    /// Context / Background reading
    /// ----------------------------
    /// You must understand the following before understanding this function
    /// + What the HedgeEmHandStageInfo class is and what its purpose is.
    /// 
    /// History
    /// -----------
    /// Original Author: Simon Hewins Jul 2014
    /// Last edit:       Simon Hewins Aug 2014
    /// 
    /// Description
    /// -----------
    /// 
    /// Gets a LIST of HedgeEmHandStageInfo objects for any given hand (index) at any given stage.
    /// </summary>
    /// <param name="a_enum_game_state"></param>
    /// <param name="a_hand_index"></param>
    /// <returns></returns>
    public double f_get_total_previous_bets_for_stage_and_hand_player(enum_betting_stage a_enum_betting_stage, int a_hand_index, int a_player_id)
    {
        /// xxx hack until Bet contains player id
        a_player_id = 0;

        List<HedgeEmBet> my_previous_bets_list = new List<HedgeEmBet>();

        HedgeEmBet myHedgeEmBet = null;
        double my_bet_total = -666;

        try
        {
            my_previous_bets_list = (from handsstage_objects in _global_game_state_object.p_recorded_bets
                                     where handsstage_objects.p_enum_betting_stage == a_enum_betting_stage
                         && handsstage_objects.p_hand_index == a_hand_index
                         && handsstage_objects.p_seat_index == a_player_id
                                     select handsstage_objects).ToList();

            my_bet_total = (from handsstage_objects in _global_game_state_object.p_recorded_bets
                            where handsstage_objects.p_enum_betting_stage == a_enum_betting_stage
                && handsstage_objects.p_hand_index == a_hand_index
                && handsstage_objects.p_seat_index == a_player_id
                            select handsstage_objects.p_bet_amount).Sum();

            /*if (my_previous_bets_list.Count > 1)
            {
                String my_err_msg = String.Format("Expected only one 'Bet' for state [{0}], hand [{1}] object got [{1}] ",
                    a_enum_betting_stage.ToString(),
                    a_hand_index,
                    my_previous_bets_list.Count);

                throw new Exception(my_err_msg);
            }*/

            //myHedgeEmBet = my_previous_bets_list[0];

        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_get_previous_bets_for_stage_and_hand" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_get_hand_stage_info_object_for_stage_and_hand function " + ex.Message;
            my_log.p_method_name = "f_get_hand_stage_info_object_for_stage_and_hand";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
        return my_bet_total;
    }

    /// <summary>
    /// Context / Background reading
    /// ----------------------------
    /// You must understand the following before understanding this function
    /// + What the HedgeEmHandStageInfo class is and what its purpose is.
    /// 
    /// History
    /// -----------
    /// Original Author: Simon Hewins Jul 2014
    /// Last edit:       Simon Hewins Aug 2014
    /// 
    /// Description
    /// -----------
    /// 
    /// Gets a LIST of HedgeEmHandStageInfo objects for any given hand (index) at any given stage.
    /// </summary>
    /// <param name="a_enum_game_state"></param>
    /// <param name="a_hand_index"></param>
    /// <returns></returns>
    public HedgeEmHandStageInfo f_get_hand_stage_info_object_for_stage_and_hand(enum_game_state a_enum_game_state, int a_hand_index)
    {
        List<HedgeEmHandStageInfo> myHedgeEmHandStageInfoList = new List<HedgeEmHandStageInfo>();
        HedgeEmHandStageInfo myHedgeEmHandStageInfo = null;
        try
        {
            // Return without searching if not a valid state where a hand_stage_info object is expected.
            if (!(a_enum_game_state == enum_game_state.STATUS_HOLE || a_enum_game_state == enum_game_state.STATUS_FLOP || a_enum_game_state == enum_game_state.STATUS_TURN || a_enum_game_state == enum_game_state.STATUS_RIVER))
            {
                return myHedgeEmHandStageInfo;
            }
            myHedgeEmHandStageInfoList = (from handsstage_objects in _global_game_state_object.p_hand_stage_info_list
                                          where handsstage_objects.p_enum_game_state == a_enum_game_state
                             && handsstage_objects.p_hand_index == a_hand_index
                                          select handsstage_objects).ToList();

            if (myHedgeEmHandStageInfoList.Count > 1)
            {
                String my_err_msg = String.Format("Expected only one 'HandStatusInfo' for state [{0}], hand [{1}] object got [{1}] ",
                    a_enum_game_state.ToString(),
                    a_hand_index,
                    myHedgeEmHandStageInfoList.Count);

                throw new Exception(my_err_msg);
            }
            myHedgeEmHandStageInfo = myHedgeEmHandStageInfoList[0];

        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_get_hand_stage_info_object_for_stage_and_hand" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_get_hand_stage_info_object_for_stage_and_hand function " + ex.Message;
            my_log.p_method_name = "f_get_hand_stage_info_object_for_stage_and_hand";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
            throw new Exception(my_error_popup);
        }
        return myHedgeEmHandStageInfo;
    }


    /// <summary>
    /// Context / Background reading
    /// ----------------------------
    /// You must understand the following before understanding this function
    /// + What the HedgeEmHandStageInfo class is and what its purpose is.
    /// 
    /// History
    /// -----------
    /// Original Author: Simon Hewins Jul 2014
    /// Last edit:       Simon Hewins Aug 2014
    /// 
    /// Description
    /// -----------
    /// 
    /// Gets a LIST of HedgeEmHandStageInfo objects for any given hand (index) at any given stage.
    /// </summary>
    /// <param name="a_enum_game_state"></param>
    /// <param name="a_hand_index"></param>
    /// <returns></returns>
    public List<HedgeEmHandStageInfo> f_get_hand_stage_info_object_for_stage_and_hand_list(enum_game_state a_enum_game_state, int a_hand_index)
    {
        List<HedgeEmHandStageInfo> myHedgeEmHandStageInfo = new List<HedgeEmHandStageInfo>();
        try
        {
            myHedgeEmHandStageInfo = (from handsstage_objects in _global_game_state_object.p_hand_stage_info_list
                                      where handsstage_objects.p_enum_game_state == a_enum_game_state
                         && handsstage_objects.p_hand_index == a_hand_index
                                      select handsstage_objects).ToList();
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_get_hand_stage_info_object_for_stage_and_hand" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_get_hand_stage_info_object_for_stage_and_hand function " + ex.Message;
            my_log.p_method_name = "f_get_hand_stage_info_object_for_stage_and_hand";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
        return myHedgeEmHandStageInfo;
    }


    #region f_place_bet
    //This function is used to Maintain Hand Panel Values when Bet is Placed on a Hand.
    public void f_place_bet()
    {
        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
        my_log.p_message = String.Format("Method called. Player [{0}], Table [{1}], Game [{2}]", f_get_player_id(), _table_id, p_game_id);
        my_log.p_method_name = "frm_hedgeem_table.f_place_bet()";
        my_log.p_player_id = f_get_player_id();
        my_log.p_game_id = p_game_id;
        my_log.p_table_id = _table_id;

        enum_betting_stage my_betting_stage = (enum_betting_stage)Session["sess_betting_stage_enum"];


        log.Debug(my_log.ToString());
        /* Get value of Selected_Hand_Panel for bet from textbox and save it in a variable */
        try
        {
            // Get the hand_index from the hidden control
            int handindexbet = Convert.ToInt32(btn_hidden_control_temp_store_for_hand_index.Value);
            DC_bet_acknowledgement my_bet_ack;

            // xxx_HC hardcode the bet value to be £1
            int bet_amount = 1;
            enum_betting_stage my_betting_stage_enum = (enum_betting_stage)Session["sess_betting_stage_enum"];
            my_bet_ack = f_place_bet(my_betting_stage_enum, handindexbet, bet_amount);
            if (my_bet_ack.p_ack_or_nak != enum_acknowledgement_type.ACK.ToString())
            {
                string short_desc = "Bet not accepted because.  Reason: ";
                //To show error description that why bet is not accepted inside div
                short_description.InnerHtml = short_desc + my_bet_ack.p_nak_reason + " Player Id: " + f_get_player_id() + " Stage: " + my_betting_stage;
                ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "document.getElementById('error_message').style.display = 'block';document.getElementById('fade').style.display = 'block';", true);
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in frm.hedgeem_table.cs.f_place_bet" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_place_bet function " + ex.Message;
            my_log.p_method_name = "f_place_bet";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }
    #endregion f_place_bet

    #region f_update_hedgeem_control_buttons_with_info_from_server
    private void f_update_hedgeem_control_buttons_with_info_from_server()
    {
        log.Debug("f_update_hedgeem_control_buttons_with_info_from_server called");
        // Determine the new state of the game and display the appropriate buttons
        try
        {
            switch (_game_state)
            {
                case enum_game_state.STATUS_START:
                    //btnDealHole.Visible = true;
                    //btnDealFlop.Visible = false;
                    //btnDealTurn.Visible = false;
                    //btnDealRiver.Visible = false;
                    //btnNextGame.Visible = false;
                    f_clear_players_bets_lablels();
                    break;

                case enum_game_state.STATUS_HOLE:
                    //btnDealHole.Visible = false;
                    //btnDealFlop.Visible = true;
                    //btnDealTurn.Visible = false;
                    //btnDealRiver.Visible = false;
                    //btnNextGame.Visible = false;
                    break;

                case enum_game_state.STATUS_FLOP:
                    //btnDealHole.Visible = false;
                    //btnDealFlop.Visible = false;
                    //btnDealTurn.Visible = true;
                    //btnDealRiver.Visible = false;
                    //btnNextGame.Visible = false;
                    break;

                case enum_game_state.STATUS_TURN:
                    //btnDealHole.Visible = false;
                    //btnDealFlop.Visible = false;
                    //btnDealTurn.Visible = false;
                    //btnDealRiver.Visible = true;
                    //btnNextGame.Visible = false;
                    break;

                case enum_game_state.STATUS_RIVER:
                    //btnDealHole.Visible = false;
                    //btnDealFlop.Visible = false;
                    //btnDealTurn.Visible = false;
                    //btnDealRiver.Visible = false;
                    //btnNextGame.Visible = true;
                    hedgeem_control_winner_message my_winner_message = new hedgeem_control_winner_message();
                    my_winner_message.p_winner_message_str = f_calculate_winnings();
                    if (my_winner_message.p_winner_message_str != "")
                    {
                        Place_Holder_Winner_Message.Controls.Add(my_winner_message);
                    }
                    break;

                default:
                    //btnDealHole.Visible = false;
                    //btnDealFlop.Visible = false;
                    //btnDealTurn.Visible = false;
                    //btnDealRiver.Visible = false;
                    //btnNextGame.Visible = true;
                    break;
            }



        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_update_hedgeem_control_buttons_with_info_from_server" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_update_hedgeem_control_buttons_with_info_from_server function " + ex.Message;
            my_log.p_method_name = "f_update_hedgeem_control_buttons_with_info_from_server";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }

    }
    #endregion

    #region f_update_hedgeem_control_board_cards_with_info_from_server
    /// <summary>
    /// This method is used to get the values of all Flop Cards when 
    /// someone click on Deal Buttons except Hole Button. Then save 
    /// that value of Flop Card to Session.
    /// Also get CSS value from session as described below. */
    /// </summary>
    private void f_update_hedgeem_control_board_cards_with_info_from_server()
    {
        try
        {
            log.Debug("f_update_hedgeem_control_board_cards_with_info_from_server called");

            // Create the FLOP card instances
            cc_flop_card1 = new hedgeem_control_card();
            cc_flop_card2 = new hedgeem_control_card();
            cc_flop_card3 = new hedgeem_control_card();
            // Get reference to the Application scoped Hedge'Em control that models the 'Turn' card
            // xxx NOTE the only reason the TURN card is coded diffently from other board cards was to prove that
            // it could be coded as an application scoped control.  I (Simon) do not know which method is best.
            // Logically there should only ever be one instance of a TURN card at any given table so I was thinking 
            // it would be best to instantiate only once.
            hedgeem_control_card my_turn_card = new hedgeem_control_card();
            hedgeem_control_card my_river_card = new hedgeem_control_card();
            
            // Note expect card as short string to be something like ac (ace of clubs), 6d (six of diamonds etc)
            // If ZZ us returned this implies the card is to be shown face down.
            p_flop_card1_as_short_string = _global_game_state_object.p_flop_card1_string;
            p_flop_card2_as_short_string = _global_game_state_object.p_flop_card2_string;
            p_flop_card3_as_short_string = _global_game_state_object.p_flop_card3_string;
            p_turn_as_short_string = _global_game_state_object.p_turn_card_string;
            p_river_as_short_string = _global_game_state_object.p_river_card_string;

            // Set the FLOP, TURN and RIVER card values using values specified by server
            // E.g. if the Server thinks the FLOP card in 'Six of clubs' (6c) set GUI control to 6c
            cc_flop_card1.p_card_as_short_string = p_flop_card1_as_short_string;
            cc_flop_card2.p_card_as_short_string = p_flop_card2_as_short_string;
            cc_flop_card3.p_card_as_short_string = p_flop_card3_as_short_string;

            my_turn_card.p_card_as_short_string = p_turn_as_short_string;
            my_river_card.p_card_as_short_string = p_river_as_short_string;

            // Set the style attribute for the Board card (e.g. whether they are position in left,right or middle position)
            cc_flop_card1.p_enum_card_position = enum_card_position.MIDDLE;
            cc_flop_card2.p_enum_card_position = enum_card_position.MIDDLE;
            cc_flop_card3.p_enum_card_position = enum_card_position.MIDDLE;
            my_turn_card.p_enum_card_position = enum_card_position.MIDDLE;
            my_river_card.p_enum_card_position = enum_card_position.MIDDLE;
            cc_flop_card1.p_cssclass = "card_left_flop";
            cc_flop_card2.p_cssclass = "card_middle_flop";
            cc_flop_card3.p_cssclass = "card_right_flop";
            my_turn_card.p_cssclass = "card_middle_turn";
            my_river_card.p_cssclass = "card_middle_river";

            Place_Holder_Flop_Cards.Controls.Add(cc_flop_card1);
            Place_Holder_Flop_Cards.Controls.Add(cc_flop_card2);
            Place_Holder_Flop_Cards.Controls.Add(cc_flop_card3);
            Place_Holder_Turn_Cards.Controls.Add(my_turn_card);
            Place_Holder_River_Cards.Controls.Add(my_river_card);
            

            lbl_game_id.Text = String.Format("Table/Game: {0}/{1} ", _global_game_state_object.p_table_id, p_game_id);

            // Control to show placed bets.  Only show during betting stages HOLE, FLOP and TURN and at RIVER/PAYOUT stage.

            if (_game_state >= enum_game_state.STATUS_HOLE && _game_state <= enum_game_state.STATUS_RIVER)
            {
                hedgeem_control_placed_bets my_control_placed_bets = new hedgeem_control_placed_bets();

                int count = _global_game_state_object._recorded_bets.Count;
                int bets = 0;
                foreach (HedgeEmBet hedgeem_bets in _global_game_state_object._recorded_bets)
                {
                    bets = (int)hedgeem_bets.p_bet_amount;
                }
                p_total_bets = bets * count;
                my_control_placed_bets.p_placed_bets = p_total_bets;
                Place_Holder_Placed_Bets.Controls.Add(my_control_placed_bets);
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_update_hedgeem_control_board_cards_with_info_from_server function " + ex.Message;
            my_log.p_method_name = "f_update_hedgeem_control_board_cards_with_info_from_server";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }
    #endregion f_update_hedgeem_control_flop_cards_with_info_from_server



    #region f_update_hedgeem_control_jackpot_with_info_from_server
    /// <summary>
    /// This method is used to get the values of all Flop Cards when 
    /// someone click on Deal Buttons except Hole Button. Then save 
    /// that value of Flop Card to Session.
    /// Also get CSS value from session as described below. */
    /// </summary>
    private void f_update_hedgeem_control_jackpot_with_info_from_server()
    {
        try
        {
            log.Debug("f_update_hedgeem_control_jackpot_with_info_from_server called");

            hedgeem_control_jackpot my_control_jackpot = new hedgeem_control_jackpot();
            my_control_jackpot.p_jackpot_balance = my_jackpot_fund;

            Place_Holder_Table_Jackpot.Controls.Add(my_control_jackpot);
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_update_hedgeem_control_board_cards_with_info_from_server function " + ex.Message;
            my_log.p_method_name = "f_update_hedgeem_control_board_cards_with_info_from_server";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }
    #endregion f_update_hedgeem_control_jackpot_with_info_from_server


    private int f_get_number_of_seats()
    {
        int my_number_of_seats = _global_game_state_object.p_number_of_seats_int;
        return my_number_of_seats;

    }

    #region f_update_hedgeem_control_bet_slider_with_info_from_server
    // This function is used to update the bet slider control with the latest information it has received from the server.
    protected void f_update_hedgeem_control_bet_slider_with_info_from_server()
    {
        try
        {
            log.Debug("f_update_hedgeem_control_bet_slider_with_info_from_server called.  XXX Currently this is a stub function that does nothing.");
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_update_hedgeem_control_bet_slider_with_info_from_server" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Error in f_update_hedgeem_control_bet_slider_with_info_from_server", new Exception(ex.Message));
        }
    }
    #endregion f_update_hedgeem_control_bet_slider_with_info_from_server

    #region f_update_hedgeem_control_betting_panels_with_info_from_server
    //  This function is used to maintain the value of bet placed on each time page is refreshed.
    protected void f_update_hedgeem_control_betting_panels_with_info_from_server()
    {
        /* Get value of Selected_Hand_Panel for bet from textbox and save it in a variable */
        try
        {
            log.Debug("f_update_hedgeem_control_betting_panels_with_info_from_server called");
            // xxxHC value of theme
            my_default_theme = enum_theme.ONLINE;
            if (my_default_theme == enum_theme.ONLINE)
            {
                // Do not show the betting panels 
                // return;
            }
            else
            {
                // Update the display
                string best_odds_token = "";
                //For each betting stage and each hand
                for (enum_betting_stage stage_index = enum_betting_stage.HOLE_BETS; stage_index <= enum_betting_stage.TURN_BETS; stage_index++)
                {

                    for (int hand_index = 0; hand_index < _xxxHCnumber_of_hands; hand_index++)
                    {
                        // xxx should really use Doubles here 
                        _hedgeem_betting_panels[(int)stage_index, hand_index] = new BETTING_PANEL(f_get_number_of_seats());
                        if (_global_game_state_object._hands.Count() != 0)
                        {
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_enum_betting_stage = stage_index;
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_percent_draw = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_percent_draw_string;
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_percent_win_or_draw = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_percent_win_or_draw_string;
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_actual = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_actual_string;
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_margin = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_margin_double.ToString();
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_percent_win = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_percent_win_string;
                            if (_global_game_state_object.p_hand_stage_info_list[hand_index].p_is_recommended_hand_to_bet_on_for_best_value_odds == true)
                            {
                                best_odds_token = " *";
                            }
                            else
                            {
                                best_odds_token = "";
                            }
                            double offered_odds = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_margin_rounded_double;

                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odd_margin_rounded = offered_odds;
                            String prefix;
                            if (offered_odds < 0)
                            {
                                prefix = "1/";
                                offered_odds = offered_odds * -1;
                            }
                            else
                            {
                                prefix = "";
                            }
                            if (offered_odds == 0)
                            {

                            }
                            else
                            {

                            }
                        }

                        //// #####################################
                        //// Display the chips for each player seated at the table (where bets have been placed)
                        for (int seat_index = 0; seat_index < f_get_number_of_seats(); seat_index++)
                        {
                            if (_global_game_state_object._seats.Count() == 0)
                            {
                                string chip_icon_resource_name = "chip_icon_seat_" + seat_index.ToString();
                            }
                            // if a bet has been place (i.e. recorded bets for stage, seat and hand combination is > 0)
                            // make the chip icon for the relevant seat visible and update the chip value to match the bet placed
                            //if (my_hedgeem_table._bets[(int)stage_index, seat_index, hand_index] > 0)
                            // xxx hardcode to always execute
                            try
                            {
                                if (true)
                                {
                                    if (_global_game_state_object._recorded_bets.Count() != 0)
                                    {
                                        // Update bet value on 'chip' and show it.  eg. set to 5 for $5 bet
                                        double bet_value_double = _global_game_state_object._recorded_bets[seat_index].p_bet_amount;
                                        _hedgeem_betting_panels[(int)stage_index, hand_index].p_players_bets[seat_index] = bet_value_double;
                                        Place_Holder_Betting_Panel.Controls.Add(_hedgeem_betting_panels[(int)stage_index, hand_index]);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                string my_error_popup = "alert('Error in f_update_hedgeem_control_betting_panels_with_info_from_server " + e.Message.ToString() + "');";
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                                HedgeEmLogEvent my_log = new HedgeEmLogEvent();
                                my_log.p_message = "Exception caught in f_update_hedgeem_control_betting_panels_with_info_from_server function " + e.Message;
                                my_log.p_method_name = "f_update_hedgeem_control_betting_panels_with_info_from_server";
                                my_log.p_player_id = f_get_player_id();
                                my_log.p_game_id = p_game_id;
                                my_log.p_table_id = _table_id;
                                log.Error(my_log.ToString());
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_update_hedgeem_control_betting_panels_with_info_from_server" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_update_hedgeem_control_betting_panels_with_info_from_server function " + ex.Message;
            my_log.p_method_name = "f_update_hedgeem_control_betting_panels_with_info_from_server";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }
    #endregion f_update_hedgeem_control_betting_panels_with_info_from_server

    #region f_update_hedgeem_control_hand_panels_with_info_from_server
    /// <summary>
    /// Method is used to get/maintain values i.e Pays Description , Bet Amount value etc of all cards 
    /// as well as bets values along with their CSS.
    /// Show the Icon Status according to the current stage of Game.
    /// Also check which Card is Winner or Dead
    /// </summary>
    /// <returns></returns>
    private void f_update_hedgeem_control_hand_panels_with_info_from_server()
    {
        log.Debug("f_update_hedgeem_control_hand_panels_with_info_from_server called");
        try
        {
            for (int hand_index = 0; hand_index < _xxxHCnumber_of_hands; hand_index++)
            {
                // call to get HedgeEmHandStageInfo for given hand index at given stage
                HedgeEmHandStageInfo my_hedgeem_hand_stage_info = f_get_hand_stage_info_object_for_stage_and_hand(_global_game_state_object.p_current_state_enum, hand_index);

                if (_global_game_state_object._hands.Count() == 0)
                {
                    p_hand_card1 = "ZZ";
                    p_hand_card2 = "ZZ";
                }

                else
                {
                    p_hand_card1 = _global_game_state_object._hands[hand_index].Substring(0, 2);

                    p_hand_card2 = _global_game_state_object._hands[hand_index].Substring(2, 2);
                }
                // Create a new HedgeEm Control that will be used to display the status of one Hand in the Web page.
                int my_num_stages = _int_number_of_betting_stages;
                int my_num_seats = f_get_number_of_seats();

                _hedgeem_hand_panels[hand_index] = new hedgeem_hand_panel(my_num_stages, my_num_seats);
                // Hand_Index
                _hedgeem_hand_panels[hand_index].p_hand_index = hand_index;

                // Card 1
                _hedgeem_hand_panels[hand_index].p_card1 = p_hand_card1;

                // Card 2 
                _hedgeem_hand_panels[hand_index].p_card2 = p_hand_card2;

                _hedgeem_hand_panels[hand_index].p_current_betting_stage = f_get_current_betting_stage();

                // xxx Need to investigate difference between .p_winning_hands and .p_is_hand_a_winner_at_this_stage;

                //_hedgeem_hand_panels[hand_index].p_is_winner = my_hedgeem_hand_stage_info.p_is_hand_a_winner_at_this_stage;
                //_hedgeem_hand_panels[hand_index].p_is_winner = my_hedgeem_hand_stage_info.p_winning_hands;
                //_hedgeem_hand_panels[hand_index].p_is_dead = my_hedgeem_hand_stage_info.p
                /*
                 * bool winning_hands = Convert.ToBoolean(f_get_object_from_json_call_to_server("get_winning_hands/" + _table_id.ToString() + "," + hand_index.ToString(), null));
                if (_game_state == enum_game_state.STATUS_RIVER)
                {
                    _hedgeem_hand_panels[hand_index].p_is_winner = winning_hands;
                    _hedgeem_hand_panels[hand_index].p_is_dead = !winning_hands;
                }*/

                /* Will show the Status_icon i.e can't lose icon, best value icon, favourite icon etc according to current stage 
                 * but except the Non_Betting_Stage */

                enum_betting_stage my_betting_stage = f_get_current_betting_stage();

                if (my_betting_stage == enum_betting_stage.NON_BETTING_STAGE)
                {
                    if (_game_state == enum_game_state.STATUS_RIVER)
                    {
                        {
                            // xxx Need to investigate difference between .p_winning_hands and .p_is_hand_a_winner_at_this_stage;

                            bool my_is_winner = _hedgeem_hand_panels[hand_index].p_is_winner = my_hedgeem_hand_stage_info.p_winning_hands;
                            bool my_is_winner2 = _hedgeem_hand_panels[hand_index].p_is_winner = my_hedgeem_hand_stage_info.p_is_hand_a_winner_at_this_stage;

                            _hedgeem_hand_panels[hand_index].p_is_winner = my_is_winner2;
                            _hedgeem_hand_panels[hand_index].p_is_dead = !my_is_winner2;
                            _hedgeem_hand_panels[hand_index].p_description = my_hedgeem_hand_stage_info.p_hand_description_short;
                        }
                    }
                    else
                    {
                        // Hand description 
                        int my_hand_number = hand_index + 1;
                        _hedgeem_hand_panels[hand_index].p_description = "Hand " + my_hand_number.ToString();
                    }

                }
                else
                {
                    //Show can't lose icon
                    bool is_this_hand_going_to_win = my_hedgeem_hand_stage_info.p_is_hand_that_cant_lose_at_this_stage;
                    _hedgeem_hand_panels[hand_index].p_is_dead_cert = is_this_hand_going_to_win;
                    p_inplay_status = my_hedgeem_hand_stage_info.p_hand_inplay_status;

                    //Show best value icon
                    //bool is_this_hand_best_value = p_recommended_hand_to_bet_on_for_best_value_odds[hand_index * xxx_HC_int_number_of_betting_stages + (int)_betting_stage];
                    bool is_this_hand_best_value = my_hedgeem_hand_stage_info.p_is_recommended_hand_to_bet_on_for_best_value_odds;
                    _hedgeem_hand_panels[hand_index].p_is_best_value = is_this_hand_best_value;

                    //Show favourite
                    bool is_this_hand_favourite = my_hedgeem_hand_stage_info.p_is_hand_that_is_favourite;
                    _hedgeem_hand_panels[hand_index].p_is_favourite = is_this_hand_favourite;

                    // Hand description 
                    _hedgeem_hand_panels[hand_index].p_description = my_hedgeem_hand_stage_info.p_hand_description_short;

                    // PAYS_Amount
                    _hedgeem_hand_panels[hand_index].p_pays_amount = (float)my_hedgeem_hand_stage_info.p_odds_margin_rounded_double;

                    string payout_string;
                    string[] new_payout_string;
                    float new_payout_value = 0;
                    if (_hedgeem_hand_panels[hand_index].p_payout_string == "")
                    {
                        new_payout_value = 0;
                    }
                    else
                    {
                        payout_string = _hedgeem_hand_panels[hand_index].p_payout_string;
                        new_payout_string = payout_string.Split('£');
                        new_payout_value = float.Parse(new_payout_string[1]);
                    }

                    // Gets value of pay out amount in session for all hands which will be used in place bet widget to show pay amount
                    Session["pays_amount_" + hand_index] = new_payout_value;
                    enum_hand_in_play_status my_hand_status = p_inplay_status;
                    if (my_hand_status == enum_hand_in_play_status.IN_PLAY_DEAD)
                    {
                        _hedgeem_hand_panels[hand_index].p_is_dead = true;
                    }
                    else
                    {
                        _hedgeem_hand_panels[hand_index].p_is_dead = false;
                    }

                }
                // Add the created control to the Webpage (HedgeEmTable.aspx) for display to the user.
                _hedgeem_hand_panels[hand_index].ID = "auto_div_hand_id" + hand_index;
                _hedgeem_hand_panels[hand_index].CssClass = "auto_div_hand_id_class";
                Place_Holder_Hand_Panel.Controls.Add(_hedgeem_hand_panels[hand_index]);
            }
        }

        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_update_hedgeem_control_hand_panels_with_info_from_server" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_update_hedgeem_control_hand_panels_with_info_from_server function " + ex.Message;
            my_log.p_method_name = "f_update_hedgeem_control_hand_panels_with_info_from_server";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
            // xxxeh - this error does not pop up
            throw new Exception(my_error_popup);
        }

    }
    #endregion f_update_hedgeem_control_hand_panels_with_info_from_server

    /// <summary>
    /// A 'hedgeem_control_hand_panel' is a GUI component that displays any one Hand in a HedgeEm game and all the graphical components that 
    /// are related to it (see documentation on hedgeem_controls_**** and hedgeem_control_handpanel).  This function updates the part of the
    /// control that displays PREVIOUS and CURRENT BETS.  
    /// 
    /// When this function should be called.
    /// ------------------------------------
    /// This function should be called only after the global object that describes the state of a hedgeEm game has been updated by a call to the 
    /// HedgeEm server.
    /// 
    /// 
    /// </summary>
    private void f_update_hedgeem_control_hand_panels_with_info_from_server_previous_bets()
    {
        try
        {
            log.Debug("f_update_hedgeem_control_hand_panels_with_info_from_server_previous_bets called");


            //For each betting stage and each hand and each seat ...
            for (enum_betting_stage stage_index = enum_betting_stage.HOLE_BETS; stage_index <= enum_betting_stage.TURN_BETS; stage_index++)
            {
                for (int hand_index = 0; hand_index < _xxxHCnumber_of_hands; hand_index++)
                {
                    for (int seat_index = 0; seat_index < f_get_player_id(); seat_index++)
                    {
                        // ... update each GUI component with information about previous bets.
                        // by 
                        // 1. Detemining if a bet has been place for hand and stage (xxx and later seat), and ... 
                        // 2. If it has get the odds presented at this hand and stage

                        // 1. Detemining if a bet has been placed for hand and stage (xxx and later seat), and ... 
                        double my_bet_total_for_stage_and_hand = f_get_total_previous_bets_for_stage_and_hand_player(stage_index, hand_index, seat_index);

                        // If a bet has been placed (i.e. recorded bets for stage, seat and hand combination is > 0)
                        // make the chip icon for the relevant seat visible and update the chip value to match the bet placed
                        try
                        {
                            if (my_bet_total_for_stage_and_hand > 0)
                            {

                                enum_game_state my_hack_game_state = f_convert_hedgeem_stage_to_state(stage_index);

                                // Get reference the list that contains all of the known status of Hands at each stage 
                                HedgeEmHandStageInfo my_hedgeem_hand_stage_info = f_get_hand_stage_info_object_for_stage_and_hand(my_hack_game_state, hand_index);

                                string chip_icon_resource_name = "chip_icon_seat_" + seat_index.ToString();

                                // Update bet value on 'chip' and show it.  eg. set to 5 for $5 bet
                                string bet_value = my_bet_total_for_stage_and_hand.ToString();

                                if (my_hedgeem_hand_stage_info != null)
                                {
                                    double my_odds_at_stage = my_hedgeem_hand_stage_info.p_odds_margin_rounded_double;
                                    // Players bets xxx currently hardcoded to one seat/player
                                    int xxx_HC_player_index = 0;
                                    _hedgeem_hand_panels[hand_index].p_players_bets[(int)stage_index, xxx_HC_player_index] = my_bet_total_for_stage_and_hand;
                                    _hedgeem_hand_panels[hand_index].p_offered_odds[(int)stage_index] = my_odds_at_stage;

                                }

                            }
                        }

                        catch (Exception e)
                        {
                            string my_error_message = String.Format("Error in f_update_hedgeem_control_hand_panels_with_info_from_server_previous_bets. Reason {0}", e.Message);
                            string my_error_popup = "alert('Error in f_update_hedgeem_control_hand_panels_with_info_from_server_previous_bets " + e.Message.ToString() + "');";
                            // xxeh exception message not shown to user
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
                            my_log.p_message = "Exception caught in f_update_hedgeem_control_hand_panels_with_info_from_server_previous_bets function " + e.Message;
                            my_log.p_method_name = "f_update_hedgeem_control_hand_panels_with_info_from_server_previous_bets";
                            my_log.p_player_id = f_get_player_id();
                            my_log.p_game_id = p_game_id;
                            my_log.p_table_id = _table_id;
                            log.Error(my_log.ToString());

                            throw new Exception(my_error_message);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_update_hedgeem_control_hand_panels_with_info_from_server_previous_bets" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_update_hedgeem_control_hand_panels_with_info_from_server_previous_bets function " + ex.Message;
            my_log.p_method_name = "f_update_hedgeem_control_hand_panels_with_info_from_server_previous_bets";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
            throw new Exception(my_error_popup);
        }
    }

    #region f_clear_players_bets_lablels
    // this function clears players bets labels
    private void f_clear_players_bets_lablels()
    {
        try
        {
            log.Debug("f_clear_players_bets_lablels called");
            int no_of_betting_stages = _int_number_of_betting_stages;
            for (int handindex = 0; handindex < _xxxHCnumber_of_hands; handindex++)
            {
                for (int stageindex = 0; stageindex < no_of_betting_stages; stageindex++)
                {
                    if (_hedgeem_betting_panels[stageindex, handindex] != null)
                    {
                        _hedgeem_betting_panels[stageindex, handindex].p_enum_panel_display_status =
                            enum_hand_in_play_status.IN_NON_BETTING_STAGE;
                    }

                    for (int player_index = 0; player_index < f_get_player_id(); player_index++)
                    {
                        if (_hedgeem_betting_panels[stageindex, handindex] != null)
                        {
                            _hedgeem_betting_panels[stageindex, handindex].p_players_bets[player_index] = 0;
                        }
                        if (_hedgeem_hand_panels[handindex] != null)
                        {
                            _hedgeem_hand_panels[handindex].p_players_bets[stageindex, player_index] = 0;
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_clear_players_bets_lablels - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_clear_players_bets_lablels function " + ex.Message;
            my_log.p_method_name = "f_clear_players_bets_lablels";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }
    #endregion f_clear_players_bets_lablels

    #region f_update_labels
    // this function update labels
    public void f_update_labels()
    {
        string best_odds_token = "";
        log.Debug("f_update_labels called");
        try
        {
            log.Info("Call f_update_seat_panels_with_info_from_servers in f_update_labels");
            //f_update_seat_panels_with_info_from_servers();
        }
        catch (Exception e)
        {
            string err_message = string.Format("Unable to update seat panels method f_update_labels \nReason {}", e.Message);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", err_message, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_update_labels function - Unable to update seat panels method in f_update_labels " + e.Message;
            my_log.p_method_name = "f_update_labels";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }

        //For each betting stage and each hand
        for (enum_betting_stage stage_index = enum_betting_stage.HOLE_BETS; stage_index <= enum_betting_stage.TURN_BETS; stage_index++)
        {

            for (int hand_index = 0; hand_index < _xxxHCnumber_of_hands; hand_index++)
            {
                // xxx should really use Doubles here 
                _hedgeem_betting_panels[(int)stage_index, hand_index] = new BETTING_PANEL(f_get_player_id());
                _hedgeem_betting_panels[(int)stage_index, hand_index] = new BETTING_PANEL(f_get_player_id());
                _hedgeem_betting_panels[(int)stage_index, hand_index].p_enum_betting_stage = stage_index;
                _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_percent_draw = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_percent_draw_string;
                _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_percent_win_or_draw = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_percent_win_or_draw_string;
                _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_actual = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_actual_string;
                _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_margin = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_margin_double.ToString();
                _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_percent_win = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_percent_win_string;
                if (_global_game_state_object.p_hand_stage_info_list[hand_index].p_is_recommended_hand_to_bet_on_for_best_value_odds == true)
                {
                    best_odds_token = " *";
                }
                else
                {
                    best_odds_token = "";
                }
                double offered_odds = _global_game_state_object.p_hand_stage_info_list[hand_index].p_odds_margin_rounded_double;
                _hedgeem_betting_panels[(int)stage_index, hand_index].p_odd_margin_rounded = offered_odds;
                String prefix;
                if (offered_odds < 0)
                {
                    prefix = "1/";
                    offered_odds = offered_odds * -1;
                }
                else
                {
                    prefix = "";
                }
                if (offered_odds == 0)
                {

                }
                else
                {

                }
                //// #####################################
                //// Display the chips for each player seated at the table (where bets have been placed)
                for (int seat_index = 0; seat_index < f_get_player_id(); seat_index++)
                {
                    string chip_icon_resource_name = "chip_icon_seat_" + seat_index.ToString();
                    try
                    {
                        // if a bet has been place (i.e. recorded bets for stage, seat and hand combination is > 0)
                        // make the chip icon for the relevant seat visible and update the chip value to match the bet placed
                        //if (my_hedgeem_table._bets[(int)stage_index, seat_index, hand_index] > 0)
                        if (true)
                        {
                            // Update bet value on 'chip' and show it.  eg. set to 5 for $5 bet
                            string bet_value = _global_game_state_object._recorded_bets[seat_index].p_bet_amount.ToString();
                            double bet_value_double = _global_game_state_object._recorded_bets[hand_index].p_bet_amount;
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_players_bets[seat_index] = bet_value_double;
                            Place_Holder_Betting_Panel.Controls.Add(_hedgeem_betting_panels[(int)stage_index, hand_index]);
                        }
                        else
                        {
                            /* ----------------Hide the chip as no bets placed------------------*/
                            //_hedgeem_control_betting_panel.p_visible = "none";
                        }

                    }
                    catch (Exception e)
                    {
                        string my_error_popup = "alert('Error in f_update_labels" + e.Message.ToString() + "');";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
                        my_log.p_message = "Exception caught in f_update_labels function " + e.Message;
                        my_log.p_method_name = "f_update_labels";
                        my_log.p_player_id = f_get_player_id();
                        my_log.p_game_id = p_game_id;
                        my_log.p_table_id = _table_id;
                        log.Error(my_log.ToString());
                    }
                }
            }
        }
    }
    #endregion f_update_labels

    #region f_place_bet
    DC_bet_acknowledgement f_place_bet(enum_betting_stage a_betting_stage, int a_hand_index, double a_amount)
    {
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
        my_log.p_message = "";
        my_log.p_method_name = "f_place_bet";
        my_log.p_player_id = f_get_player_id();
        my_log.p_game_id = p_game_id;
        my_log.p_table_id = _table_id;

        // Create a new 'Bet Acknowledgement' object that will be used to Ackknowledge the success (ACK) or failure (NACK) of the placed bet.
        DC_bet_acknowledgement my_ack = null;

        try
        {
            // Log the entry to this method
            my_log.p_message = String.Format("f_place_bet called. Args Stage[{0}], Hand [{1}], Amount[{2}]", a_betting_stage.ToString(), a_hand_index, a_amount);
            log.Debug(my_log.p_message);

            // Call the Hedge'Em Webservices (via helper function) to place the bet.
            string place_bet_endpoint = String.Format("ws_place_bet/{0},{1},{2},{3},{4}",
                                                        _table_id.ToString(),
                                                        a_hand_index.ToString(),
                                                        a_amount.ToString(),
                                                        a_betting_stage.ToString(),
                                                        f_get_player_id().ToString());
            my_ack = (DC_bet_acknowledgement)f_get_object_from_json_call_to_server(place_bet_endpoint, typeof(DC_bet_acknowledgement));

            // Return the Acknowledgement message to the caller to indicate success or failure.
            return my_ack;
        }
        catch (Exception e)
        {
            string strScript = "alert('Unable to place bet. Reason..." + e.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", strScript, true);
            my_log.p_message = "Exception caught in f_place_bet function " + e.Message;

            log.Error(my_log.ToString());
            return my_ack;
        }
    }
    #endregion f_place_bet

    #region Get_Bet_Value
    // This function is used to get value of clicked hand
    protected void btn_Get_Clicked_Hand_Value_Click(object sender, EventArgs e)
    {
        try
        {
            log.Debug("f_place_bet is called in btn_Get_Clicked_Hand_Value_Click ");
            // calls place bet function
            f_place_bet();
            log.Debug("f_call_function_to_render_screen is called in btn_Get_Clicked_Hand_Value_Click");

            _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_game_state_object/" + _table_id, typeof(HedgeEmGameState));
            p_game_id = _global_game_state_object.p_game_id;
            int my_number_of_hands = _global_game_state_object.p_number_of_hands_int;
            enum_betting_stage my_betting_stage = f_get_current_betting_stage();
            _game_state = _global_game_state_object.p_current_state_enum;
            _hedgeem_hand_panels = new hedgeem_hand_panel[my_number_of_hands];
            Session["sess_betting_stage_enum"] = _global_game_state_object.p_current_betting_stage_enum;
            _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
            _hedgeem_betting_panels = new BETTING_PANEL[_int_number_of_betting_stages, my_number_of_hands];
            lbl_game_id.Text = String.Format("Table/Game: {0}/{1} ", _global_game_state_object.p_table_id, p_game_id);
            f_call_functions_to_render_screen();
        }
        catch (Exception ex)
        {
            string my_error_message = String.Format("Error in f_update_hedgeem_control_hand_panels_with_info_from_server_previous_bets. Reason {0}", ex.Message);

            string my_error_popup = "alert('Error in btn_Get_Clicked_Hand_Value_Click" + ex.Message.ToString() + "');";
            // xxxeh popup does not show
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in btn_Get_Clicked_Hand_Value_Click function " + ex.Message;
            my_log.p_method_name = "btn_Get_Clicked_Hand_Value_Click";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
            //throw new Exception(my_error_popup);
        }
    }
    #endregion Get_Bet_Value

    #region Controls_for_Betting_Panels_and_Board_Cards
    private void Controls_for_Betting_Panels_and_Board_Cards()
    {
        cc_flop_card1 = new hedgeem_control_card();
        cc_flop_card2 = new hedgeem_control_card();
        cc_flop_card3 = new hedgeem_control_card();
        cc_turn_card = new hedgeem_control_card();
        cc_river_card = new hedgeem_control_card();
        cc_flop_card1.p_enum_card_position = enum_card_position.LEFT;
        cc_flop_card2.p_enum_card_position = enum_card_position.MIDDLE;
        cc_flop_card3.p_enum_card_position = enum_card_position.RIGHT;
        cc_turn_card.p_enum_card_position = enum_card_position.MIDDLE;
        cc_river_card.p_enum_card_position = enum_card_position.MIDDLE;
        ss_seat = new hedgeem_control_seats();
    }
    #endregion

    #region f_createControl_Bettinf_update_seat_panels_with_info_from_serverrid
    /// Defines/Displays all the places where players can place bets.  This is essentially
    /// a x*y grid where x is the number of betting stages and y is the number of hands that can be bet on
    /// See also: f_displayControl_Bettinf_update_seat_panels_with_info_from_serverrid
    public void f_createControl_Bettinf_update_seat_panels_with_info_from_serverrid()
    {
        for (int stageindex = 0; stageindex < _int_number_of_betting_stages; stageindex++)
        {
            for (int handindex = 0; handindex < _xxxHCnumber_of_hands; handindex++)
            {
                _hedgeem_betting_panels[stageindex, handindex] = new BETTING_PANEL(f_get_player_id());
            }
        }
    }
    #endregion f_createControl_Bettinf_update_seat_panels_with_info_from_serverrid

    #region f_update_seat_panels_with_info_from_server
    private void xxx_not_usedf_update_seat_panels_with_info_from_servers()
    {
        int my_seat_id = -1;
        int my_seat_player_id = -1;
        string my_seat_player_name = "Empty Seat";
        double my_player_funds_at_seat = 0;
        try
        {
            // For each seat at this table sit a player from the knoww list of current players
            for (int seat_index = 0; seat_index < f_get_player_id(); seat_index++)
            {
                ss_seat = new hedgeem_control_seats();
                my_seat_id = _global_game_state_object._seats[seat_index].p_seat_id;
                my_seat_player_id = _global_game_state_object._seats[seat_index].p_player_id;
                my_seat_player_name = _global_game_state_object._seats[seat_index].p_player_name;
                my_player_funds_at_seat = _global_game_state_object._seats[seat_index].p_player_seat_balance;

                // Test if no-one is sitting at seat (as indicated by seat_player_id = -1)
                if (my_seat_player_id == -1)
                {
                    ss_seat.p_player_name = "";
                    ss_seat.p_photo_image = "";
                    ss_seat.p_balance = "0";
                    ss_seat.p_player_id = my_seat_player_id;
                    ss_seat.p_seat_index = my_seat_id;
                    Session["seat_id"] = my_seat_id;
                    //string chip_icon_resource_name = String.Format("player_avatar_sit_here.JPG");
                    string chip_icon_resource_name = "";
                    ss_seat.p_chip_icon = "../resources/" + chip_icon_resource_name;
                    Place_Holder_Player_Info.Controls.Add(ss_seat);
                }
                else
                {
                    string player_seat_balance_text = String.Format("£{0:#0.00}", my_player_funds_at_seat);
                    //player_seat_balance_text = (9000 + _global_game_state_object.test()).ToString();
                    string avatar_image_name = "";
                    try
                    {
                        //avatar_image_name = "player_avatar_" + Session["username"].ToString();
                        avatar_image_name = "player_avatar_" + my_seat_player_name;

                    }
                    catch (Exception ex)
                    {
                        string my_error_popup = "alert('" + ex.Message.ToString() + "');";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
                        my_log.p_message = "Exception caught in f_update_seat_panels_with_info_from_servers function " + ex.Message;
                        my_log.p_method_name = "f_update_seat_panels_with_info_from_servers";
                        my_log.p_player_id = f_get_player_id();
                        my_log.p_game_id = p_game_id;
                        my_log.p_table_id = _table_id;
                        log.Error(my_log.ToString());
                    }
                    ss_seat.p_player_name = Session["username"].ToString();
                    ss_seat.p_photo_image = avatar_image_name;
                    ss_seat.p_balance = player_seat_balance_text;
                    ss_seat.p_seat_index = my_seat_id;
                    ss_seat.p_player_id = my_seat_player_id;
                    string chip_icon_resource_name = String.Format("chip_icon_{0}.png", seat_index);
                    ss_seat.p_chip_icon = "../resources/" + chip_icon_resource_name;
                    Place_Holder_Player_Info.Controls.Add(ss_seat);
                }
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_update_seat_panels_with_info_from_servers function " + ex.Message;
            my_log.p_method_name = "f_update_seat_panels_with_info_from_servers";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }
    #endregion

    protected void btnLobby_Click(object sender, ImageClickEventArgs e)
    {
        Session["fb_hide_logout"] = "fb_hide_logout";
        Response.Redirect("frm_facebook_canvas.aspx");
    }

    #region f_call_functions_to_render_screen
    /// <summary>
    /// This function can be called at any time to redraw the Web page with updated state from the Hedge'Em Server.
    /// 
    /// A Hedge'Em Table web page is comprised the following graphical components (Controls):
    /// 
    /// + Hand Panels:      One for each hand dealt - shows the card, hand description, status icons 
    ///                     and other information related to a specifc Hand.
    ///                     
    /// + Betting Panels:   (Optional) One for each hand and for each stage of the game.  Betting panels are typically
    ///                     when the 'style' is CASINO mode to allow bets to be 'placed on a table' and for multiple 
    ///                     player games.
    ///                     
    /// + Seats:            One for each seat at the HedgeEm table.  Seats are almost synomous with Players - i.e. 
    ///                     A player sits a seat. A seat control has details about the player sitting at the seat. E.g.
    ///                     The players funds at this seat, thier name, their avitar etc.
    /// 
    /// + Jackpot:          (Optional) A control that shows the current state of the Jackpot. 
    /// 
    /// + Deal Buttons:     These are button that the user can press to procede the game to next game stage.
    /// 
    /// + Action Buttons:   These are buttons to do any other action (e.g. Go to Cashier, go to Lobby etc
    /// 
    /// + Bet Slider:       (Optional) A 'slider' control that allows the user to select how much they want to bet on a given
    ///                     hand.
    /// 
    /// All this function does is call out other functions to do the update.
    /// 
    /// </summary>
    private void f_call_functions_to_render_screen()
    {
        f_update_hedgeem_control_hand_panels_with_info_from_server();
        f_update_hedgeem_control_board_cards_with_info_from_server();
        f_update_hedgeem_control_seat_with_info_from_server();
        f_update_hedgeem_control_jackpot_with_info_from_server();
        f_update_hedgeem_control_betting_panels_with_info_from_server();
        f_update_hedgeem_control_bet_slider_with_info_from_server();
        f_update_hedgeem_control_hand_panels_with_info_from_server_previous_bets();
        f_update_hedgeem_control_buttons_with_info_from_server();
        f_update_game_id();
    }
    #endregion f_call_functions_to_render_screen

    protected void btn_Get_Clicked_Player_Id_Click(object sender, EventArgs e)
    {
        log.Debug("btn_Get_Clicked_Player_Id_Click called");
        HedgeEmPlayer a_player = (HedgeEmPlayer)Session["value"];
        if (Convert.ToInt32(mytext_player_Id_hedgeem.Value) > 0)
        {
            if (a_player != null)
            {
                //f_update_seat_panels_with_info_from_servers();
            }
        }
        else
        {
            try
            {
                Session["current_player_seat_id"] = mytext_player_Id.Value;
                //   int seat_index_if_seated = my_hedgeem_table.f_find_seat_number_for_player_id(f_get_player_id());
                int seat_index_if_seated = 0;
                if (seat_index_if_seated >= 0)
                {
                    String msz = String.Format("Already seated at seat {0}, you are not permitted to sit at more that one seat.", seat_index_if_seated + 1);
                    string Message;
                    Message = "alert('" + msz + "')";
                    //  ScriptManager.RegisterStartupScript(updPanl_to_avoid_Postback, updPanl_to_avoid_Postback.GetType(), "Not_Seated", Message, true);
                    for (int seat_index = 0; seat_index < f_get_player_id(); seat_index++)
                    {
                        ss_seat = new hedgeem_control_seats();
                        ss_seat.p_player_name = Session["ss_seat.p_player_name_" + seat_index].ToString();
                        ss_seat.p_photo_image = Session["ss_seat.p_photo_image_" + seat_index].ToString();
                        ss_seat.p_balance = Session["ss_seat.p_balance_" + seat_index].ToString();
                        ss_seat.p_seat_index = Convert.ToInt32(Session["ss_seat.p_seat_index_" + seat_index]);
                        ss_seat.p_player_id = Convert.ToInt32(Session["ss_seat.p_player_id_" + seat_index]);
                        ss_seat.p_chip_icon = Session["ss_seat.p_chip_icon_" + seat_index].ToString();
                        ss_seat.p_back_color = Session["ss_seat.p_back_color_" + seat_index].ToString();
                        mytext_player_Id.Value = Session["mytext_player_Id"].ToString();
                        mytext_player_Id_hedgeem.Value = Session["mytext_player_Id_hedgeem"].ToString();
                        Place_Holder_Player_Info.Controls.Add(ss_seat);
                    }
                }
                else
                {
                    f_update_hedgeem_control_seat_with_info_from_server();
                    //  ScriptManager.RegisterStartupScript(this.updPanl_to_avoid_Postback, this.updPanl_to_avoid_Postback.GetType(), "Open Sit Here Form", "window.open('frm_sit_here.aspx','','height=200,width=300,left=300,top=200,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes','_blank');", true);
                }
            }
            catch (Exception ex)
            {
                string my_error_popup = "alert('" + ex.Message.ToString() + "');";

                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                log.Error("Error in btn_Get_Clicked_Player_Id_Click", new Exception(ex.Message));
                HedgeEmLogEvent my_log = new HedgeEmLogEvent();
                my_log.p_message = "Exception caught in btn_Get_Clicked_Player_Id_Click function " + ex.Message;
                my_log.p_method_name = "btn_Get_Clicked_Player_Id_Click";
                my_log.p_player_id = f_get_player_id();
                my_log.p_game_id = p_game_id;
                my_log.p_table_id = _table_id;
                log.Error(my_log.ToString());
            }
        }

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {

        _xxx_log_event.p_method_name = "btnLogin_Click";
        // Log the fact this method has been called
        _xxx_log_event.p_message = String.Format("Method [btnLogin_Click] called ");
        log.Debug(_xxx_log_event.ToString());
        if (Session["chkfb"] != null)
        {
            //   chk_fb_visibility.Visible = true;
            log.Debug(String.Format("Session variable 'chkfb' is {0}", Session["chkfb"].ToString()));
        }
        else
        {
            //  chk_fb_visibility.Visible = false;
            //  chk_fb_visibility.Visible = false;
        }

        string my_username = "";
        string my_password = "";
        DataTable my_user_details;

        if (txt_get_fm_email_id.Text != "")
        {
            my_username = txt_get_fm_email_id.Text;
            my_password = Session["password"].ToString();
            Session["facebooklogin"] = "true";
        }
        else
        {
            my_username = txtUsername.Text;
            my_password = txtPassword.Text;
        }
        try
        {
            /* Check the user login details (username and password) are valid by querying the database
              and if they are populate a DataTable object (my_user_details) with the details of the user.
              Details can be found from the column heading in the 'player' table in the db such as
              player_id, fullname, balance etc. */

            my_user_details = service.my_user_details(my_username, my_password);
            // Check you got only one record back, if not throw appropriate exception
            if (my_user_details.Rows.Count < 1)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "show_error_message();", true);
                // lbl_login_status_msg.Text = "Username " + my_username + " not found. Username or password incorrect.";
                //Page.RegisterStartupScript("OnLoad", "<script>show_error_message();</script>");

                //throw new Exception(String.Format("Username '{0}' not found. Username or password incorrect.", my_username));
            }
            else if (my_user_details.Rows.Count > 1)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "show_error_message();", true);

                //  lbl_login_status_msg.Text = "Multiple users found for Username= "+ my_username +" .This indicates something is wrong so cancelling operation";

                // Page.RegisterStartupScript("OnLoad", "<script>show_error_message();</script>");
                // throw new Exception(String.Format("Multiple users found for username '{0}'.  This indicates something is wrong so cancelling operation..", my_username));
            }
            else
            {
                //  Read the player details: player_id and fullname
                DataRow row = my_user_details.Rows[0];
                int my_player_id = Convert.ToInt32(row["player_id"]);
                Session["playerid"] = my_player_id;
                Session["display_name"] = row["fullname"].ToString();


                //if (my_player_id == 10001)
                //{
                //    deposit_update.Visible = true;
                //}
                //else
                //{
                //    deposit_update.Visible = true;
                //    deposit_update.Attributes.Add("style", "margin:0 0 -25px !Important");
                //}
                Session["username"] = my_username;
                Session["password"] = my_password;
                service.f_update_last_login_date(Session["username"].ToString());
                if (Session["facebooklogin"] != null)
                {
                    LoginDiv.Attributes.Add("style", "display:none !Important;");
                }
                else
                {
                    lbl_user_name.Text = Session["display_name"].ToString();
                    Logout.Attributes.Add("style", "display:block !Important;");
                    LoginDiv.Attributes.Add("style", "display:none !Important;");
                }
                btn_play_now.Enabled = true;
                if (Session["username"] != null)
                {
                    usr_image.ImageUrl = "../resources/player_avatar_" + Session["username"].ToString() + ".jpg";
                }

                if (Session["fb_hide_logout"] != null)
                {
                    //    chk_fb_visibility_logout.Visible = false;
                    btn_play_now.Visible = true;

                }
                if (Session["chk_logout_hide"] != null)
                {

                    //   chk_fb_visibility.Visible = true;
                    btn_play_now.Visible = true;
                    btnLogout.Visible = false;
                }
                Page.RegisterStartupScript("OnLoading", "<script>load_edit_profile();</script>");
                // Response.Redirect("frm_facebook_canvas.aspx");
                Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('progressbar').style.display='none';</script>");
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in method [btnLogin_Click] - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);

            //  Log the error and raise a new exception
            _xxx_log_event.p_message = String.Format(
                "Table [Fatal error in method [btnLogin_Click]. Reason [{0}]",
                        ex.Message);
            log.Error(_xxx_log_event.ToString(), new Exception(ex.Message));
            throw new Exception(_xxx_log_event.ToString());
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        _xxx_log_event.p_method_name = "btnLogout_Click";
        // Log the fact this method has been called
        _xxx_log_event.p_message = String.Format("Method [btnLogout_Click] called ");
        log.Info(_xxx_log_event.ToString());
        try
        {
            btnLogout.Visible = false;
            //btnLogin.Visible = true;
            Session.Abandon();
            Session.Contents.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in method [btnLogout_Click] - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);

            //  Log the error and raise a new exception
            _xxx_log_event.p_message = String.Format(
                "Table [Fatal error in method [btnLogout_Click]. Reason [{0}]",
                        ex.Message);
            log.Error(_xxx_log_event.ToString(), new Exception(ex.Message));
            throw new Exception(_xxx_log_event.ToString());
        }
        Response.Redirect("frm_facebook_canvas.aspx");

    }

    protected void btn_play_now_Click(object sender, EventArgs e)
    {

        //HedgeEmLogEvent _xxx_log_event = new HedgeEmLogEvent();
        //if (Session["username"] == null)
        //{
        //    log.Info("[Anonymous user] clicked on btn_play_now");
        //    Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>alert('You must be logged in to Play this game.');document.getElementById('progressbar').style.display='none'; </script>");
        //}
        //else
        try
        {
            // Log that the user has clicked the 'Play now' button.
            HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
            my_log_event.p_method_name = "frm_facebook_canvas.btn_play_now_Click";
            my_log_event.p_message = String.Format("User [{0}] click 'Play Now", Session["username"]);
            log.Info(my_log_event.ToString());


            if (txt_get_fm_email_id.Text == "" && Session["username"] == null)
            {
                my_log_event.p_message = String.Format("User informed the must be logged in to play.");
                log.Debug(my_log_event.ToString());

                ScriptManager.RegisterStartupScript(Page, GetType(), "Alert Message", "alert('You must be logged in to Play this game.');", true);
            }
            else if (txt_get_fm_email_id.Text != null && Session["username"] == null)
            {
                my_log_event.p_message = String.Format("Attempting to get player detail from webservice using text from txt_get_fm_email_id hidden field. ");
                log.Debug(my_log_event.ToString());

                int user_exist = service.f_get_player_details_by_username(txt_get_fm_email_id.Text);
                if (user_exist == 1)
                {

                    Session["username"] = txt_get_fm_email_id.Text;
                    my_log_event.p_message = String.Format("Retrieved player detail from webservice and used this to create session information. [{0}]. ", Session["username"]);
                    log.Debug(my_log_event.ToString());

                }
                else
                {
                    my_log_event.p_message = String.Format("User informed the must register to play.");
                    log.Debug(my_log_event.ToString());
                    Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>alert('You must register to play the game.');</script>");
                }
            }

            if (Session["username"] != null)
            {
                // this will check if the user logged in via facebook exists in our db or not. If yes then will check for the table or else will create.
                Session["Facebook_User_Id"] = fb_user_id.Value;
                String password = "";
                DataTable dt = new DataTable();

                if (txt_get_fm_email_id.Text == "")
                {
                    my_log_event.p_message = String.Format("txt_get_fm_email_id hidden field is null so try to get info from service.");
                    log.Debug(my_log_event.ToString());
                    dt = service.f_get_password_from_db(Session["username"].ToString());
                }
                else
                {
                    my_log_event.p_message = String.Format("txt_get_fm_email_id hidden field is [{0}] so try to get info from service using this value", txt_get_fm_email_id.Text);
                    log.Debug(my_log_event.ToString());
                    dt = service.f_get_password_from_db(txt_get_fm_email_id.Text);
                    Session["chkfb"] = "yes_facebook";
                }
                // If user exists in our db then will go into this
                if (dt.Rows.Count > 0)
                {
                    my_log_event.p_message = String.Format("Webservice has record of user so now see if they have a HedgeEm Table", txt_get_fm_email_id.Text);
                    log.Debug(my_log_event.ToString());

                    password = dt.Rows[0]["password"].ToString();
                    if (password != null)
                    {
                        Session["password"] = password.ToString();
                        // Check for table exists or not
                        int chk_hedgeem_table_exits_or_not;

                        if (txt_get_fm_email_id.Text == "")
                        {
                            chk_hedgeem_table_exits_or_not = service.f_get_table_id_from_db(Session["username"].ToString());
                        }
                        else
                        {
                            chk_hedgeem_table_exits_or_not = service.f_get_table_id_from_db(txt_get_fm_email_id.Text);
                        }


                        if (chk_hedgeem_table_exits_or_not == 0)
                        {

                            if (txt_get_fm_email_id.Text == "")
                            {
                                playerid = service.get_player_id(Session["username"].ToString(), password);
                                Session["playerid"] = playerid;
                            }
                            else
                            {
                                playerid = service.get_player_id(txt_get_fm_email_id.Text, password);
                                Session["playerid"] = playerid;
                            }

                            my_log_event.p_message = String.Format("This player does not have a HedgeEm table so create one.", txt_get_fm_email_id.Text);
                            log.Debug(my_log_event.ToString());

                            f_create_personal_hedgeem_table_for_player(playerid, txt_get_fm_email_id.Text);
                        }
                        else
                        {

                            playerid = service.get_player_id(Session["username"].ToString(), password);
                            Session["playerid"] = playerid;
                        }
                    }
                }
                else
                {
                    //try
                    //{
                    //    // this will create the user in our database as well as its table.
                    //    my_username = txt_get_fm_email_id.Text;
                    //    xxxHC_a_opening_balance = 100;
                    //    xxxHC_a_password = "password";
                    //    xxxHC_a_full_name = txt_fullname.Text;
                    //    Session["chk_logout_hide"] = "chk_logout_hide";
                    //    try
                    //    {
                    //        int player_id = service.f_create_player(txt_get_fm_email_id.Text, xxxHC_a_password, xxxHC_a_full_name, xxxHC_a_opening_balance);



                    //        Session["username"] = my_username;
                    //        Session["password"] = xxxHC_a_password;

                    //        f_create_personal_hedgeem_table_for_player(player_id, my_username);

                    //    }
                    //    catch (Exception exx)
                    //    {
                    //        log.Error("Creation of new User failed", new Exception(exx.Message));
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    log.Error("Fatal error in 'btn_play_now_Click'", new Exception(ex.Message));
                    //}
                }
                if (txt_get_fm_email_id.Text == "")
                {
                    string my_username = Session["username"].ToString();
                    int my_table_id = service.f_get_table_id_from_db(my_username);

                    if (my_table_id <= 0)
                    {
                        string my_error_msg = String.Format("Invalid table ID [{0}]", my_table_id);
                        log.Error(my_error_msg);
                        throw new Exception(my_error_msg);
                    }

                    Session["tableid"] = my_table_id;

                    Session["table_name"] = service.get_table_name(my_table_id);
                    //  Session["table_name"] = Session["username"].ToString();
                    f_goto_table(my_table_id);
                }
                else
                {
                    btnLogin_Click(btnLogin, new EventArgs());
                    int a_table_id = service.f_get_table_id_from_db(txt_get_fm_email_id.Text);
                    Session["tableid"] = a_table_id;

                    Session["table_name"] = service.get_table_name(a_table_id);
                    //  Session["table_name"] = txt_get_fm_email_id.Text;
                    service.f_update_last_login_date(Session["username"].ToString());//this function is used to update last login date and time of user
                    f_goto_table(a_table_id);

                }
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in method [btn_play_now_Click] - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Error in btn_play_now_Click", new Exception(ex.Message));
        }


    }

    private void f_create_personal_hedgeem_table_for_player(int player_id, string username)
    {

        //HedgeEmLogEvent _xxx_log_event = new HedgeEmLogEvent();
        //my_log_event.p_method_name = "f_create_personal_hedgeem_table_for_player";
        log.Debug("f_create_personal_hedgeem_table_for_player is called");
        try
        {

            // Define the parameters that you wish to set when you create this Hedge'Em table
            Double my_target_rtp = 97;
            int my_number_of_hands_to_be_played_at_this_table = 4;
            int my_number_of_seats_at_this_table = 1;
            int my_initial_investment = 10000; // xxx this value needs to be revisited.
            String my_table_name = "";
            my_table_name = String.Format("{0}_Personal", username);

            bool my_is_jackpot_enabled = false;
            enum_theme my_default_theme = enum_theme.ONLINE;

            // See if a table exist for this test
            // _current_table_object_string = service.f_get_hedgeem_table_by_table_name(my_table_name);

            // If a table does NOT exist, create one
            // if (_current_table_object_string == "")
            {
                // Create a new table based on the simulation parameters defined above.
                //_current_table_object = (HedgeEMTable)
                int xxx_my_HC_session_id_not_used_yet = 666;
                int my_HC_seat_id = 0;
                int my_buy_in = Convert.ToInt32(25);
                HedgeEmTableSummary my_hedgeem_table_summary = new HedgeEmTableSummary();
                my_hedgeem_table_summary = (HedgeEmTableSummary)f_get_object_from_json_call_to_server("ws_create_hedgeem_table/" + my_target_rtp + "," +
                                               my_number_of_hands_to_be_played_at_this_table + "," +
                                               my_number_of_seats_at_this_table + "," +
                                               my_initial_investment + "," +
                                               my_table_name + "," +
                                               my_is_jackpot_enabled + "," +
                                               my_default_theme + "," + (int)xxxHC_a_opening_balance + "," + player_id + "," + xxx_my_HC_session_id_not_used_yet + "," + my_HC_seat_id + "," + my_buy_in, typeof(HedgeEmTableSummary));
                //    service.f_create_table(my_target_rtp,
                //                                   my_number_of_hands_to_be_played_at_this_table,
                //                                   my_number_of_seats_at_this_table,
                //                                   my_initial_investment,
                //                                   my_table_name,
                //                                   my_is_jackpot_enabled,
                //                                   my_default_theme, (int)xxxHC_a_opening_balance, player_id, xxx_my_HC_session_id_not_used_yet, my_HC_seat_id, my_buy_in);
                int a_table_id = my_hedgeem_table_summary.p_table_id;
                HedgeEmGenericAcknowledgement my_generic_ack = new HedgeEmGenericAcknowledgement();
                my_generic_ack = (HedgeEmGenericAcknowledgement)f_get_object_from_json_call_to_server("ws_sit_at_table/" + xxx_my_HC_session_id_not_used_yet + "," +
                                                   10 + "," +
                                                   a_table_id + "," +
                                                   my_HC_seat_id + "," +
                                                   player_id, typeof(HedgeEmGenericAcknowledgement));

            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in method [f_create_personal_hedgeem_table_for_player] - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Error in f_create_personal_hedgeem_table_for_player", new Exception(ex.Message));
            //my_log_event.p_message = "Exception caught - " + ex.Message;
            //log.Error(_xxx_log_event.ToString());

        }
    }

    private void f_goto_table(int a_table_id)
    {

        /* Get reference to table by this table id
         For this GUI to work we need a HedgeEMTable object that it will obtain its data from (e.g. cards to display,
         odds for each hand etc).  So create a place-holder object that we will set later. */


        log.Debug("f_goto_table is called");
        try
        {
            if (Session["username"] != null)
            {

                //string theme_name = service.get_theme_name(a_table_id);
                HedgeemThemeInfo my_hedgeem_theme_info = new HedgeemThemeInfo();
                my_hedgeem_theme_info = (HedgeemThemeInfo)f_get_object_from_json_call_to_server("ws_get_theme_details_for_hedgeem_table/" + a_table_id, typeof(HedgeemThemeInfo));
                string theme_name = my_hedgeem_theme_info.short_name;
                Session["theme"] = theme_name;

                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('" + "Theme name -" + Session["theme"].ToString() + "');", true);
                /* HACK xxx.  This is a hack to ensure that the 'style' is set correctly when
                 starting from the lobby window.  I have to call the menu item as I only want to set the background
                 once - this is also a hack */
                EventArgs ea = new EventArgs();

                Response.Redirect("frm_hedgeem_table.aspx", false);
            }
            else
            {
                Page.RegisterStartupScript("OnLoad", "<script>alert('You need to be logged in to sit at this table');</script>");
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in method [f_goto_table] - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Error in f_goto_table", new Exception(ex.Message));
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_game_state_object/1000", typeof(HedgeEmGameState));
        p_game_id = _global_game_state_object.p_game_id;
        int my_number_of_hands = _global_game_state_object.p_number_of_hands_int;
        enum_betting_stage my_betting_stage = f_get_current_betting_stage();
        Session["sess_betting_stage_enum"] = _global_game_state_object.p_current_betting_stage_enum;
        _game_state = _global_game_state_object.p_current_state_enum;
        _hedgeem_hand_panels = new hedgeem_hand_panel[my_number_of_hands];
        _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
        _hedgeem_betting_panels = new BETTING_PANEL[_int_number_of_betting_stages, my_number_of_hands];
        player_funds_at_seat = _global_game_state_object._seats[0].p_player_seat_balance;
        // call to the method to render the screen
        f_call_functions_to_render_screen();
        //img_countdown.Visible = false;
        // btn_deal_next_stage_Click();
        //img_countdown.Visible = true;
        //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "LoadTimer();", true);
        // ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "LoadTimer();", true);
    }

    public void f_update_hedgeem_control_seat_with_info_from_server()
    {
        log.Debug("f_update_hedgeem_control_seat_with_info_from_server called");
        try
        {
            int my_seat_id = -1;
            int my_seat_player_id = -1;
            string my_seat_player_name = "Empty Seat";
            double my_player_funds_at_seat = 0;
            _table_id = 1000;
            // For each seat at this table sit a player from the knoww list of current players
            int xxxHC_number_of_seats = 6;
            for (int seat_index = 0; seat_index < xxxHC_number_of_seats; seat_index++)
            {
                ss_seat = new hedgeem_control_seats();
                if (_global_game_state_object._seats.Count() != 0)
                {
                    my_seat_id = _global_game_state_object._seats[seat_index].p_seat_id;
                    my_seat_player_id = _global_game_state_object._seats[seat_index].p_player_id;
                    my_seat_player_name = _global_game_state_object._seats[seat_index].p_player_name;
                    my_player_funds_at_seat = _global_game_state_object._seats[seat_index].p_player_seat_balance;
                }
                if (my_seat_player_id == -1)
                {
                    ss_seat.p_player_name = "";
                    ss_seat.p_photo_image = "player_avitar_sit_here";
                    ss_seat.p_balance = "0";
                    ss_seat.p_player_id = my_seat_player_id;
                    ss_seat.p_seat_index = my_seat_id;
                    string chip_icon_resource_name = String.Format("chip_icon_{0}.png", seat_index);
                    ss_seat.p_chip_icon = "../resources/" + chip_icon_resource_name;
                    // Highlight the seat the current player is sitting at.
                    if (mytext_player_Id.Value != "")
                    {
                        if (Convert.ToInt32(mytext_player_Id_hedgeem.Value) == Convert.ToInt32(my_seat_player_id))
                        {
                            ss_seat.p_back_color = "Black";
                        }
                        else
                        {
                            ss_seat.p_back_color = "Black";
                        }
                    }
                    else
                    {
                        ss_seat.p_back_color = "Black";
                    }
                    Place_Holder_Player_Info.Controls.Add(ss_seat);
                }
                else
                {
                    string player_seat_balance_text = String.Format("£{0:#0.00}", my_player_funds_at_seat);
                    string avatar_image_name = "";
                    try
                    {
                        //avatar_image_name = "player_avatar_" + Session["username"].ToString();
                        avatar_image_name = "player_avatar_" + my_seat_player_name;

                    }
                    catch (Exception ex)
                    {
                        string my_error_popup = "alert('Error in f_update_hedgeem_control_seat_with_info_from_server" + ex.Message.ToString() + "');";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
                        my_log.p_message = "Exception caught in f_update_hedgeem_control_seat_with_info_from_server function " + ex.Message;
                        my_log.p_method_name = "f_update_hedgeem_control_seat_with_info_from_server";
                        my_log.p_player_id = f_get_player_id();
                        my_log.p_game_id = p_game_id;
                        my_log.p_table_id = _table_id;
                        log.Error(my_log.ToString());
                    }
                    //ss_seat.p_player_name = Session["username"].ToString();
                    ss_seat.p_photo_image = avatar_image_name;
                    ss_seat.p_balance = player_seat_balance_text;
                    ss_seat.p_seat_index = my_seat_id;
                    ss_seat.p_player_id = my_seat_player_id;
                    string chip_icon_resource_name = String.Format("chip_icon_{0}.png", seat_index);
                    ss_seat.p_chip_icon = "../resources/" + chip_icon_resource_name;
                    // Highlight the seat the current player is sitting at.
                    if (mytext_player_Id.Value != "")
                    {
                        if (Convert.ToInt32(mytext_player_Id_hedgeem.Value) == Convert.ToInt32(my_seat_player_id))
                        {
                            ss_seat.p_back_color = "Orange";
                        }
                        else
                        {
                            ss_seat.p_back_color = "Black";
                        }
                    }
                    else if (f_get_player_id() == my_seat_player_id)
                    {
                        ss_seat.p_back_color = "Orange";
                    }
                    else
                    {
                        ss_seat.p_back_color = "Black";
                    }
                    Place_Holder_Player_Info.Controls.Add(ss_seat);
                }
                Session["ss_seat.p_player_name_" + seat_index] = ss_seat.p_player_name;
                Session["ss_seat.p_photo_image_" + seat_index] = ss_seat.p_photo_image;
                Session["ss_seat.p_balance_" + seat_index] = ss_seat.p_balance;
                Session["ss_seat.p_seat_index_" + seat_index] = ss_seat.p_seat_index;
                Session["ss_seat.p_player_id_" + seat_index] = ss_seat.p_player_id;
                Session["ss_seat.p_chip_icon_" + seat_index] = ss_seat.p_chip_icon;
                Session["ss_seat.p_back_color_" + seat_index] = ss_seat.p_back_color;
                Session["mytext_player_Id"] = mytext_player_Id.Value;
                Session["mytext_player_Id_hedgeem"] = mytext_player_Id_hedgeem.Value;
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_update_hedgeem_control_seat_with_info_from_server - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_update_hedgeem_control_seat_with_info_from_server function " + ex.Message;
            my_log.p_method_name = "f_update_hedgeem_control_seat_with_info_from_server";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }

    protected void lnlLobbyPage_Click(object sender, EventArgs e)
    {
        Response.Redirect("Lobby.aspx");
    }

    // This function calculate the total winnings in the game.
    private String f_calculate_winnings()
    {
        log.Debug("f_calculate_winnings called");
        String my_complete_win_string = "";
        try
        {
            StringBuilder my_winnings_for_each_stage_SB = new StringBuilder();
            Double my_total_winning = 0;
            double my_winnings_from_this_bet = 0;
            //For each winning hand ...
            #region for_all_hands
            for (int current_hand = 0; current_hand < _xxxHCnumber_of_hands; current_hand++)
            {
                // If this is a winning hand then for each player playing ...
                if (_global_game_state_object.p_hand_stage_info_list[current_hand].p_is_hand_a_winner_at_this_stage == true)
                {

                    //Calulate the winnings (if any) for any bet placed on each of the stages ...
                    for (int seat_index = 0; seat_index < f_get_player_id(); seat_index++)
                    {
                        //For each betting stage (POCKETS, FLOP, and TURN) ...
                        for (int stage_index = 0; stage_index < _int_number_of_betting_stages; stage_index++)
                        {

                            string str_betting_stage = "HOLE bet";
                            int game_state = 6;
                            switch (stage_index)
                            {
                                case 0:
                                    str_betting_stage = "HOLE bet";
                                    game_state = 6;
                                    break;
                                case 1:
                                    str_betting_stage = "FLOP bet";
                                    game_state = 10;
                                    break;
                                case 2:
                                    str_betting_stage = "TURN bet";
                                    game_state = 11;
                                    break;

                                default:
                                    break;
                            }
                            HedgeEmHandStageInfo my_hedgeem_hand_stage_info = f_get_hand_stage_info_object_for_stage_and_hand((enum_game_state)game_state, current_hand);
                            if (my_hedgeem_hand_stage_info == null)
                            {
                                throw new Exception("my_hedgeem_hand_stage_info is null in f_calculate winnings");
                            }
                            else
                            {
                                double my_bet_total = f_get_total_previous_bets_for_stage_and_hand_player((enum_betting_stage)stage_index, current_hand, f_get_player_id());

                                if (Math.Sign(my_hedgeem_hand_stage_info.p_odds_margin_rounded_double) == 1)
                                {
                                    my_winnings_from_this_bet = Convert.ToDouble(my_bet_total)
                                                                 * my_hedgeem_hand_stage_info.p_odds_margin_rounded_double;
                                }
                                else
                                {
                                    if (my_hedgeem_hand_stage_info.p_odds_margin_rounded_double != 0)
                                    {
                                        my_winnings_from_this_bet = my_bet_total
                                                                     / my_hedgeem_hand_stage_info.p_odds_margin_rounded_double;
                                        my_winnings_from_this_bet = Math.Abs(my_winnings_from_this_bet);
                                    }
                                }
                                if (my_bet_total != 0)
                                {
                                    double my_initial_bet_for_stage_hand = my_bet_total;
                                    double my_total_winnings_for_stage_hand = my_winnings_from_this_bet + my_initial_bet_for_stage_hand;
                                    my_total_winning = my_total_winning + my_total_winnings_for_stage_hand;
                                    /// xxx hack to not show message box on Online version
                                    my_winnings_for_each_stage_SB.AppendLine();
                                    my_winnings_for_each_stage_SB.AppendFormat("£{0:#0.00} from {1}.",
                                        my_total_winnings_for_stage_hand,
                                        str_betting_stage);
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            if (my_total_winning > 0)
            {
                my_complete_win_string = String.Format("You won £{0:#0.00}", my_total_winning);
            }
            else
            {
                my_complete_win_string = "";
            }

            return my_complete_win_string;
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_calculate_winnings - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_calculate_winnings function " + ex.Message;
            my_log.p_method_name = "f_calculate_winnings";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
            return my_complete_win_string;
        }
    }

    #region f_play_deal_cards_sound_effect
    /// <summary>
    /// xxx this sound should be played on client side javascript not serverside as it introduces unecessary delay
    /// </summary>
    private void f_play_deal_cards_sound_effect()
    {
        SoundPlayer sd = new SoundPlayer();
        sd.SoundLocation = Server.MapPath("~/resources/waves/Deal-4.wav");
        sd.Play();
        sd.Play();
        sd.Play();
        sd.Play();
        sd.Play();
    }
    #endregion Sound_Play_On_Click_Of_Deal_Button

    protected void btn_play_for_real_deposit_pledge_Click(object sender, EventArgs e)
    {
        HedgeEmLogEvent _xxx_log_event = new HedgeEmLogEvent();
        _xxx_log_event.p_method_name = "btn_play_for_real_deposit_pledge_Click";
        log.Info("[" + Session["username"].ToString() + "] clicked on Play for Real Deposit Pledge");
        try
        {
            // This will save the value of pledge amount in the database.
            play_for_real_deposit_pledge _play_for_real_deposit_pledge = new play_for_real_deposit_pledge
            {
                p_playerid = f_get_player_id(),
                p_play_for_real_amount = Convert.ToDouble(txt_play_for_real_deposit_pledge.Text)
            };

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(play_for_real_deposit_pledge));
            MemoryStream mem = new MemoryStream();
            ser.WriteObject(mem, _play_for_real_deposit_pledge);
            string data =
                Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
            WebClient webClient = new WebClient();
            webClient.Headers["Content-type"] = "application/json";
            webClient.Encoding = Encoding.UTF8;
            webClient.UploadString("http://devserver.hedgeem.com/Service1.svc/f_set_play_for_real_deposit_pledge", data);
            webClient.UploadString("http://localhost:59225/Service1.svc/f_set_play_for_real_deposit_pledge", data);
            // Empties the value of textbox
            txt_play_for_real_deposit_pledge.Text = "";
            _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_game_state_object/" + _table_id, typeof(HedgeEmGameState));
            p_game_id = _global_game_state_object.p_game_id;
            int my_number_of_hands = _global_game_state_object.p_number_of_hands_int;
            enum_betting_stage my_betting_stage = f_get_current_betting_stage();
            _game_state = _global_game_state_object.p_current_state_enum;
            _hedgeem_hand_panels = new hedgeem_hand_panel[my_number_of_hands];
            _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
            _hedgeem_betting_panels = new BETTING_PANEL[_int_number_of_betting_stages, my_number_of_hands];
            f_update_game_id();
            // Method which calls the functions to render the screen.
            f_call_functions_to_render_screen();
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in btn_play_for_real_deposit_pledge_Click - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in btn_play_for_real_deposit_pledge_Click function " + ex.Message;
            my_log.p_method_name = "btn_play_for_real_deposit_pledge_Click";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }

    private void f_update_game_id()
    {
        lbl_game_id.Text = String.Format("Table/Game: {0}/{1}. Session ID [{2}] ", _global_game_state_object.p_table_id, p_game_id, Session.SessionID);
    }

    protected void btn_play_for_real_Click(object sender, EventArgs e)
    {
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = "btn_play_for_real_Click";
        log.Info("[" + Session["username"].ToString() + "] cliked on Play for Real button.");
        try
        {
            // This will get the current value of count that how many times user have clicked on the button, from the database.       
            int play_for_real_count = Convert.ToInt32(f_get_object_from_json_call_to_server("f_get_play_for_real_count/" + f_get_player_id().ToString(), null));

            // This will increment the value of count.
            play_for_real _play_for_real = new play_for_real
            {
                p_playerid = f_get_player_id(),
                p_play_for_real_count = play_for_real_count
            };

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(play_for_real));
            MemoryStream mem = new MemoryStream();
            ser.WriteObject(mem, _play_for_real);
            string data =
                Encoding.UTF8.GetString(mem.ToArray(), 0, (int)mem.Length);
            WebClient webClient = new WebClient();
            webClient.Headers["Content-type"] = "application/json";
            webClient.Encoding = Encoding.UTF8;
            webClient.UploadString("http://devserver.hedgeem.com/Service1.svc/f_set_play_for_real_count", data);
            webClient.UploadString("http://localhost:59225/Service1.svc/f_set_play_for_real_count", data);

            _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_game_state_object/" + _table_id, typeof(HedgeEmGameState));
            p_game_id = _global_game_state_object.p_game_id;
            int my_number_of_hands = _global_game_state_object.p_number_of_hands_int;
            enum_betting_stage my_betting_stage = f_get_current_betting_stage();
            _game_state = _global_game_state_object.p_current_state_enum;
            _hedgeem_hand_panels = new hedgeem_hand_panel[my_number_of_hands];
            _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
            _hedgeem_betting_panels = new BETTING_PANEL[_int_number_of_betting_stages, my_number_of_hands];
            lbl_game_id.Text = String.Format("Table/Game: {0}/{1} ", _global_game_state_object.p_table_id, p_game_id);
            // Method which calls the functions to render the screen.
            f_call_functions_to_render_screen();
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in btn_play_for_real_Click - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in btn_play_for_real_Click function " + ex.Message;
            my_log.p_method_name = "btn_play_for_real_Click";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }

    //xxx commenting out the bet slider code and reverting back the old single bet place. NOw this function is not in use
    protected void btn_Bet_Click(object sender, EventArgs e)
    {
        HedgeEmBetAcknowledgement my_bet_ack = new HedgeEmBetAcknowledgement();
        log.Info("[" + Session["username"].ToString() + "] placed a bet");
        /* Get value of Selected_Hand_Panel for bet from textbox and save it in a variable */
        // xxx need to document this
        try
        {
            //  Get the hand_index from the hidden control
            int handindexbet = Convert.ToInt32(btn_hidden_control_temp_store_for_hand_index.Value);
            double bet_amount;
            if (handindexbet == 0)
            {
                bet_amount = double.Parse(amount1.Value);
            }
            else if (handindexbet == 1)
            {
                bet_amount = double.Parse(amount2.Value);
            }
            else if (handindexbet == 2)
            {
                bet_amount = double.Parse(amount3.Value);
            }
            else
            {
                bet_amount = double.Parse(amount4.Value);
            }

            double rounded_bet_amount = Math.Round(bet_amount, 2, MidpointRounding.AwayFromZero);
            DC_bet_acknowledgement enum_my_bet_ack;

            enum_betting_stage my_betting_stage = f_get_current_betting_stage();
            enum_my_bet_ack = f_place_bet(my_betting_stage,
               handindexbet, rounded_bet_amount);

            if (enum_my_bet_ack.p_ack_or_nak != enum_acknowledgement_type.ACK.ToString())
            {
                string short_desc = "Bet not accepted because.  Reason: ";
                string long_desc = my_bet_ack.p_ack_description;
                //To show error description that why bet is not accepted inside div
                short_description.InnerHtml = short_desc;
                long_description.InnerHtml = long_desc;
                ScriptManager.RegisterStartupScript(Page, GetType(), "JsStatus", "document.getElementById('error_message').style.display = 'block';document.getElementById('fade').style.display = 'block';", true);
            }
            f_place_bet();
            _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_game_state_object/" + _table_id, typeof(HedgeEmGameState));
            p_game_id = _global_game_state_object.p_game_id;
            int my_number_of_hands = _global_game_state_object.p_number_of_hands_int;
            _game_state = _global_game_state_object.p_current_state_enum;
            _hedgeem_hand_panels = new hedgeem_hand_panel[my_number_of_hands];
            _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
            _hedgeem_betting_panels = new BETTING_PANEL[_int_number_of_betting_stages, my_number_of_hands];
            lbl_game_id.Text = String.Format("Table/Game: {0}/{1} ", _global_game_state_object.p_table_id, p_game_id);
            f_call_functions_to_render_screen();
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in btn_Bet_Click" + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in btn_Bet_Click function " + ex.Message;
            my_log.p_method_name = "btn_Bet_Click";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }

    // This cancels the bet placed by the user.
    public void btn_cancel_bets_for_this_hand_and_stage_Click(object sender, EventArgs e)
    {
        log.Info("[" + Session["username"].ToString() + "] cancelled the bet");
        try
        {
            // Get the hand_index from the hidden control
            int handindexbet = Convert.ToInt32(btn_hidden_control_temp_store_for_hand_index.Value);
            int xxx_HC_seat_index = 0;
            // Call webservice svc function to cancel the bet placed
            f_get_object_from_json_call_to_server("f_cancel_bets_for_this_hand_and_stage/" + _table_id.ToString() + "," + f_get_player_id().ToString() + "," + xxx_HC_seat_index.ToString() + "," + handindexbet.ToString(), null);

            _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_game_state_object/" + _table_id, typeof(HedgeEmGameState));
            p_game_id = _global_game_state_object.p_game_id;
            int my_number_of_hands = _global_game_state_object.p_number_of_hands_int;
            enum_betting_stage my_betting_stage = f_get_current_betting_stage();
            _game_state = _global_game_state_object.p_current_state_enum;
            _hedgeem_hand_panels = new hedgeem_hand_panel[my_number_of_hands];
            _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
            _hedgeem_betting_panels = new BETTING_PANEL[_int_number_of_betting_stages, my_number_of_hands];
            lbl_game_id.Text = String.Format("Table/Game: {0}/{1} ", _global_game_state_object.p_table_id, p_game_id);
            // Method to render the screen
            f_call_functions_to_render_screen();
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_cancel_bets_for_this_hand_and_stage" + ex.Message.ToString() + "');";
            ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('" + ex.Message.ToString() + "');", true);
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_cancel_bets_for_this_hand_and_stage function " + ex.Message;
            my_log.p_method_name = "f_cancel_bets_for_this_hand_and_stage";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }


    /// <summary>
    /// Get the current betting stage for the table.  Assume we already have this and it is 'cached' in session.
    /// If not get it from HedgeEmServer
    /// </summary>
    /// <returns></returns>
    enum_betting_stage f_get_current_betting_stage()
    {
        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
        enum_betting_stage my_enum_betting_stage = enum_betting_stage.NON_BETTING_STAGE;

        // Test if the current betting stage is stored in session, if it is use this, if not try to retrieve from server.
        if (Session["sess_betting_stage_enum"] != null)
        {
            try
            {
                my_enum_betting_stage = (enum_betting_stage)Session["sess_betting_stage_enum"];
            }
            catch (Exception e)
            {
                throw new Exception("Fatal error trying to get current betting stage from session" + e.Message);
            }
        }
        else
        {
            try
            {
                _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_game_state_object/" + _table_id, typeof(HedgeEmGameState));
                my_log.p_message = String.Format("Successfully retrieved gamestate from server. Table ID [{0}], State [{1}]", _global_game_state_object.p_table_id, _global_game_state_object.p_current_state_enum.ToString());
                log.Debug(my_log.ToString());
                my_enum_betting_stage = _global_game_state_object.p_current_betting_stage_enum;
            }
            catch (Exception ex)
            {
                my_log.p_message = String.Format("Error trying to get game state from server. Reason [{0}]", ex.Message);
                log.Error(my_log.ToString());
                return enum_betting_stage.NON_BETTING_STAGE;
            }

        }

        return my_enum_betting_stage;

    }

    #region btn_deal_next_stage_Click
    /// <summary>
    /// This is the primary co-ordinating function initiates the transistion of a Hedge'Em game from one
    /// state to the next.  This function is called when a player/user clicks on and of the 'Deal' buttons
    /// (e.g. 'Deal Hole', 'Deal Flop', 'Deal Next' etc).  
    /// 
    /// If you imagine the in the real physical world it would be like the player at a black-jack table
    /// telling the dealer to deal the next card. 
    /// 
    /// Upon calling of this function two things happen:
    /// 
    ///    1. An instruction is sent to the server to tell it to progress the game to the next stage.  (e.g. 
    ///       (if the game is currently in 'HOLE' state, then progress to 'FLOP' state.
    ///       
    ///    2. An internal call to is made to tell the webpage to update its display based on the change in state
    ///       as read from the server post change to new state.  Eg. If the Hedge'Em game now enters the 'TURN'
    ///       stage then show/enable the 'Deal River' button and update the Odd that each hand has of winning. 
    ///       
    /// xxx ... Note: in most games (and how this is currently (Dec 2013) coded there will only be 
    /// one player per table so the game will change to the next state each time this function is called.
    /// In future the game will not progress to the next state until all players at the same table have 
    /// issued the same instruction or the dealer/server decided to do on a time-delay basis after each stage.
    /// </summary>
    /// <returns></returns>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 

    public void btn_deal_next_stage_Click()
    {


        //   ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('called');", true);
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = "frm_home_page.btn_deal_next_stage_Click";
        my_log_event.p_method_name = "btn_deal_next_stage_Click";
        if (Session["username"] == null)
        {
            my_log_event.p_message = "Hardcoded username to Simon";
            log.Warn(my_log_event.ToString());
            Session["username"] = "Simon";
        }
        string my_username = Session["username"].ToString();

        log.Info("[" + my_username + "] clicked Deal Next Stage button");
        try
        {
            if (Session.Count == 0)
            {
                log.Info("Session timed out for user with player id = " + f_get_player_id());
                ScriptManager.RegisterStartupScript(Page, GetType(), "SessionTimeOutMsg", "show_session_timeout_message();", true);
            }
            else
            {
                int xxxHC_my_player_ID = 10000;
                // call to webservice to get next state object
                _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_next_game_state_object/" + _table_id + "," + f_get_player_id(), typeof(HedgeEmGameState));
                p_game_id = _global_game_state_object.p_game_id;
                int my_number_of_hands = _global_game_state_object.p_number_of_hands_int;
                enum_betting_stage my_betting_stage = f_get_current_betting_stage();
                Session["sess_betting_stage_enum"] = _global_game_state_object.p_current_betting_stage_enum;
                _game_state = _global_game_state_object.p_current_state_enum;
                _hedgeem_hand_panels = new hedgeem_hand_panel[my_number_of_hands];
                _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
                _hedgeem_betting_panels = new BETTING_PANEL[_int_number_of_betting_stages, my_number_of_hands];
                player_funds_at_seat = _global_game_state_object._seats[0].p_player_seat_balance;
                // call to the method to render the screen
                f_call_functions_to_render_screen();
            }
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert ('hi');", true);
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in btn_deal_next_stage_Click - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert ('" + ex.Message.ToString() + "')", true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = String.Format("Exception caught in btn_deal_next_stage_Click function. Reason [{0}] ", ex.Message);
            my_log.p_method_name = "btn_deal_next_stage_Click";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
            //throw new Exception("xxx This exception is not being caught");
        }
        //Response.Write("<META HTTP-EQUIV=Refresh CONTENT='30; URL='>");
    }

    //public void TaskLoop()
    //{
    //    // In this example, task will repeat in infinite loop
    //    // You can additional parameter if you want to have an option 
    //    // to stop the task from some page
    //    while (true)
    //    {
    //        // Execute scheduled task
    //        btn_deal_next_stage_Click();

    //        // Wait for certain time interval
    //        // System.Threading.Timer(callback, null, next5am - DateTime.Now, TimeSpan.FromHours(24));
    //        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30));
    //    }
    //}

    //protected void btn_deal_next_stage_Click(object sender, ImageClickEventArgs e)
    //{
    //    HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
    //    my_log_event.p_method_name = "frm_home_page.btn_deal_next_stage_Click";
    //    my_log_event.p_method_name = "btn_deal_next_stage_Click";
    //    if (Session["username"] == null)
    //    {
    //        my_log_event.p_message = "Hardcoded username to Simon";
    //        log.Warn(my_log_event.ToString());
    //        Session["username"] = "Simon";
    //    }
    //    string my_username = Session["username"].ToString();

    //    log.Info("[" + my_username + "] clicked Deal Next Stage button");
    //    try
    //    {
    //        if (Session.Count == 0)
    //        {
    //            log.Info("Session timed out for user with player id = " + f_get_player_id());
    //            ScriptManager.RegisterStartupScript(Page, GetType(), "SessionTimeOutMsg", "show_session_timeout_message();", true);
    //        }
    //        else
    //        {
    //            int xxxHC_my_player_ID = 10000;
    //            // call to webservice to get next state object
    //            _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_next_game_state_object/" + _table_id + "," + f_get_player_id(), typeof(HedgeEmGameState));
    //            game_id = _global_game_state_object.p_game_id;
    //            int my_number_of_hands = _global_game_state_object.p_number_of_hands_int;
    //            enum_betting_stage my_betting_stage = f_get_current_betting_stage();
    //            Session["sess_betting_stage_enum"] = _global_game_state_object.p_current_betting_stage_enum;
    //            _game_state = _global_game_state_object.p_current_state_enum;
    //            _hedgeem_hand_panels = new hedgeem_hand_panel[my_number_of_hands];
    //            _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
    //            _hedgeem_betting_panels = new BETTING_PANEL[_int_number_of_betting_stages, my_number_of_hands];
    //            player_funds_at_seat = _global_game_state_object._seats[0].p_player_seat_balance;
    //            // call to the method to render the screen
    //            f_call_functions_to_render_screen();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string my_error_popup = "alert('Error in btn_deal_next_stage_Click - " + ex.Message.ToString() + "');";
    //        ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert ('plokerror')", true);
    //        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
    //        my_log.p_message = String.Format("Exception caught in btn_deal_next_stage_Click function. Reason [{0}] ", ex.Message);
    //        my_log.p_method_name = "btn_deal_next_stage_Click";
    //        my_log.p_player_id = f_get_player_id();
    //        my_log.p_game_id = game_id;
    //        my_log.p_table_id = _table_id;
    //        log.Error(my_log.ToString());
    //        //throw new Exception("xxx This exception is not being caught");
    //    }
    //}
    #endregion btn_deal_next_stage_Click

    #region AltA_Show_Functionality
    protected void btn_Show_Admin_Flag_Click(object sender, EventArgs e)
    {
        /* This method is used to Reset/Show Admin Flag Vlaues depend on Session value either 
         * it will be shown or reset as described below */
        if (Convert.ToInt32(Session["Check_AltA"]) == 1)
        {
            btn_Show_Admin_Flag.Visible = false;
            btn_Hide_Admin_Flag.Visible = true;
        }
        if (Session["Check_AltA"] == null)
        {
            Session["Check_AltA"] = "0";
            btn_Hide_Admin_Flag.Visible = true;
            btn_Show_Admin_Flag.Visible = false;
        }

    }
    #endregion AltA_Show_Functionality

    #region AltA_Hide_Functionality
    /// <summary>
    ///  This method is used to Hide Admin Flag Vlaues depend on Session value either 
    /// it will be shown or not as described below 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Hide_Admin_Flag_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Convert.ToInt32(Session["Check_AltA"]) == 1)
            {
                btn_Hide_Admin_Flag.Visible = false;
                btn_Show_Admin_Flag.Visible = true;
                Session["Check_AltA"] = null;
            }
            else
            {
                Session["Check_AltA"] = null;
                btn_Hide_Admin_Flag.Visible = false;
                btn_Show_Admin_Flag.Visible = true;
            }
        }

        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in AltA_Hide_Functionality " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
        }

    }
    #endregion AltA_Hide_Functionality

    // This method add 25 chips amount to user's seat balance if a user clicks on get chips button
    protected void btn_get_chips_add_Click(object sender, EventArgs e)
    {
        HedgeEmLogEvent my_log = new HedgeEmLogEvent();

        try
        {
            my_log.p_message = "Method called ";
            my_log.p_method_name = "btn_get_chips_add_Click";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Debug(my_log.ToString());
            log.Info("[" + Session["username"].ToString() + "] clicked btn_get_chips");
            int xxx_HC_seat_id = 0;
            double my_get_chips_top_up_amount = Convert.ToDouble(WebConfigurationManager.AppSettings["get_chips_default_amount"]);


            seat_balance_update my_seat_balance_update = new seat_balance_update
            {
                p_playerid = f_get_player_id(),
                p_tableid = _table_id,
                p_seatid = xxx_HC_seat_id,
                p_balance = my_get_chips_top_up_amount
            };


            int my_server_id = 10;
            // Call the Hedge'Em Webservices (via helper function) to place the bet.
            string get_chips_endpoint = String.Format("ws_top_up_chips_at_table/{0},{1},{2},{3},{4}",
                                                        my_server_id,
                                                        _table_id,
                                                        xxx_HC_seat_id,
                                                        my_get_chips_top_up_amount,
                                                        f_get_player_id());
            my_seat_balance_update = (seat_balance_update)f_get_object_from_json_call_to_server(get_chips_endpoint, typeof(seat_balance_update));


            // gets clicked link value from session
            string clicked_name = Session["name"].ToString();

            //logs the entry of clicked link
            log.Info("[" + Session["username"].ToString() + "] Selected [" + clicked_name + "] option to get chips.");
            /*_global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_game_state_object/" + _table_id, typeof(HedgeEmGameState));
            game_id = _global_game_state_object.p_game_id;
            int my_number_of_hands =_global_game_state_object.p_number_of_hands_int;
            
            enum_betting_stage my_betting_stage = f_get_current_betting_stage();
            _game_state = _global_game_state_object.p_current_state_enum;
            _hedgeem_hand_panels = new hedgeem_hand_panel[my_number_of_hands];
            _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
            _hedgeem_betting_panels = new BETTING_PANEL[_int_number_of_betting_stages, my_number_of_hands];
            lbl_game_id.Text = String.Format("Table/Game: {0}/{1} ", _global_game_state_object.p_table_id, game_id);
            */
            //f_call_functions_to_render_screen();
            ScriptManager.RegisterStartupScript(Page, GetType(), "", "alert('Congratulations £25 have been added to your seat amount........!');", true);
        }
        catch (Exception ex)
        {
            // xxxeh this exeception does not show to users
            string my_error_popup = "alert('Error in btn_get_chips_add_Click - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_log.p_message = String.Format("Exception caught in btn_get_chips_add_Click function. Reason [{0}] ", ex.Message);
            my_log.p_method_name = "btn_get_chips_add_Click";
            my_log.p_player_id = f_get_player_id();
            my_log.p_game_id = p_game_id;
            my_log.p_table_id = _table_id;
            log.Error(my_log.ToString());
        }
    }

    // Method use to get the clicked link by user to get free chips
    [WebMethod(EnableSession = true)]
    public static string get_clicked_name(string name)
    {
        try
        {
            Page objp = new Page();
            // sets value of clicked link to session
            objp.Session["name"] = name;
        }
        catch (Exception ex)
        {
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in get_clicked_name function " + ex.Message;
            my_log.p_method_name = "get_clicked_name";
            log.Error(my_log.ToString());
        }
        return name;
    }

    [WebMethod(EnableSession = true)]
    public static bool f_check_user_logged_in_from_facebook_first_time()
    {
        log.Debug("f_check_user_logged_in_from_facebook_first_time called to check the role of the user.");
        bool if_exists = false;
        try
        {
            // Fetch the email from Javascript through query string
            string email = HttpContext.Current.Request.QueryString["uid"].ToString();
            localhost.WebService service = new localhost.WebService();

            if (email != "")
            {
                DataTable dt = service.f_get_password_from_db(email);

                if (dt.Rows.Count > 0)
                {
                    string password = dt.Rows[0]["password"].ToString();
                    if_exists = true;
                    HttpContext.Current.Session["username"] = email;
                }
                else
                {
                    if_exists = false;
                }
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_check_user_logged_in_from_facebook_first_time - " + ex.Message.ToString() + "');";
            System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>" + my_error_popup + "</SCRIPT>");
            log.Error("Error in f_check_user_logged_in_from_facebook_first_time", new Exception(ex.Message));
        }
        return if_exists;
    }

    // Web Method to check the role of the logged in user. Called when Facebook SDK is rendered and user is logged in.
    [WebMethod(EnableSession = true)]
    public static string f_check_user_role()
    {
        log.Debug("f_check_user_role called to check the role of the user.");
        string role = "";
        try
        {
            // Fetch the email from Javascript through query string
            string email = HttpContext.Current.Request.QueryString["uid"].ToString();
            localhost.WebService service = new localhost.WebService();

            if (email != "")
            {
                DataTable dt = service.f_get_password_from_db(email);

                if (dt.Rows.Count > 0)
                {
                    string password = dt.Rows[0]["password"].ToString();
                    DataTable userdetails = service.my_user_details(email, password);
                    role = userdetails.Rows[0]["role"].ToString();
                }
                HttpContext.Current.Session["user_role"] = role;
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in method [f_check_user_role] - " + ex.Message.ToString() + "');";
            System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>" + my_error_popup + "</SCRIPT>");

            log.Error("Error in f_check_user_role", new Exception(ex.Message));
        }
        return role;
    }
}