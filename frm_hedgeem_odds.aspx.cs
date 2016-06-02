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
using System.Web.Caching;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;


class RequestState
{
    public HttpWebRequest request = null;
    public HttpWebResponse response = null;
}
public partial class frm_hedgeem_odds : System.Web.UI.Page
{

    #region Controls
    hedgeem_control_card cc_flop_card1;
    hedgeem_control_card cc_flop_card2;
    hedgeem_control_card cc_flop_card3;
    hedgeem_control_card cc_turn_card;
    hedgeem_control_card cc_river_card;
    hedgeem_control_jackpot my_control_jackpot;
    hedgeem_control_seats ss_seat;
    hedgeem_hand_panel[] _hedgeem_hand_panels;
    BETTING_PANEL[,] _hedgeem_betting_panels;
    #endregion

    int number_of_hands;
    //int playerid;
    //int number_of_seats = 1; // xxx_HC value

    int chk_admin_flag_value = 0;
    enum_game_state _game_state;
    double my_jackpot_fund;
    string p_flop_card1_as_short_string;
    string p_flop_card2_as_short_string;
    string p_flop_card3_as_short_string;
    string p_turn_as_short_string;
    string p_river_as_short_string;
    int game_id;
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

    //For Logging
    private static String logger_name_as_defined_in_app_config = "client." + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(logger_name_as_defined_in_app_config);
    HedgeEmGameState _global_game_state_object = new HedgeEmGameState();

    string xxxHC_Session_id = "sessionabc123";
    int xxxHC_ANON_PLAYER_ID = 100;
    string xxxHC_ANON_USERNAME = "admin@mantura.com";


    //[DataContract]
    //public class Exception_Fault_Contract
    //{
    //    [DataMember]
    //    public String Message { get; set; }

    //    [DataMember]

    //    public String Description { get; set; }
    //}

    // preinit event changes the theme of the page at runtime before page loads
    protected void Page_PreInit(object sender, EventArgs e)
              {
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = "Page_PreInit";

        my_log_event.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
        my_log_event.p_table_id = p_session_personal_table_id;
        my_log_event.p_message = "Method called.";
        log.Debug(my_log_event.ToString());
        try
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(24));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            // If the session has expired we would have lost critical session state so re-direct the users to the home page.
            if (Session.Count == 0)
            {
                my_log_event.p_message = "No session detected";
                log.Warn(my_log_event.ToString());
                ScriptManager.RegisterStartupScript(Page, GetType(), "SessionTimeOutMsg", "show_session_timeout_message();", true);
                // Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>show_session_timeout_message();</script>");
            }
            else
            {
                my_log_event.p_message = "Attempting to set theme.";
                log.Debug(my_log_event.ToString());
                f_set_theme();
                
            }
            /* This use to work but does not now as Header is null
            String Theme = Session["theme"].ToString();
            Page.Header.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("/resources/css/" + Theme + "/hedgeem_table.css") + "\" />"));
            Page.Header.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("/resources/css/" + Theme + "/hedgeem_board_cards.css") + "\" />"));
            Page.Header.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("/resources/css/" + Theme + "/hedgeem_hand.css") + "\" />"));
            Page.Header.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("/resources/css/" + Theme + "/hedgeem_seat.css") + "\" />"));
            Page.Header.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("/resources/css/" + Theme + "/hedgeem_buttons.css") + "\" />"));
            Page.Header.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("/resources/css/" + Theme + "/hedgeem_betting_panels.css") + "\" />"));
            Page.Header.Controls.Add(new LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("/resources/css/" + Theme + "/hedgeem_hidden_controls.css") + "\" />"));
            */
        }
        catch (Exception ex)
        {
            string my_error_popup = "Error in Page PreInit - " + ex.Message.ToString();
            
            my_log_event = new HedgeEmLogEvent();
            my_log_event.p_message = ex.Message;
            my_log_event.p_method_name = "Page Load";
            my_log_event.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
            my_log_event.p_table_id = p_session_personal_table_id;
            log.Error(my_log_event.ToString());
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup.ToString();
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
        }
    }

    private int p_session_personal_table_id
    {
        get
        {
            int my_table_id = -1;
            try
            {
                my_table_id = Convert.ToInt32(Session["p_session_personal_table_id"]);
            }
            catch (Exception e)
            {
                my_table_id = -1;
            }

            return my_table_id;
        }

        set { Session["p_session_personal_table_id"] = value; }
    }

    private void f_set_theme()
    {

        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());

        if (Session["theme"] != null)
        {
            // this changes the theme of the hedgeem table according to the theme selected by the user.
            this.Page.Theme = Session["theme"].ToString();
            my_log_event.p_table_id = p_session_personal_table_id;
            log.Debug(my_log_event.ToString());

            if (this.Page.Theme == "")
            {
                // xxxeh - need to throw error when string =""
                this.Page.Theme = "ONLINE";
                my_log_event.p_message = "Warning. Session varible for 'theme' is blank so HARDCODED to ONLINE";
                log.Warn(my_log_event.ToString());
            }
        }
        else
        {
            this.Page.Theme = enum_theme.WSOP.ToString();
            my_log_event.p_message = "Unable to determine Session as no session variable for 'theme' - HARDCODING TO ONLINE THEME. ";
            log.Warn(my_log_event.ToString());
        }

        
    }

    private enum_theme f_get_current_theme_as_enum()
    {
        enum_theme my_theme = enum_theme.ONLINE;
        String my_theme_string = "NOT_SET";
        if (Session["theme"] != null)
        {
            if (this.Page.Theme != "")
            {

                my_theme_string = Session["theme"].ToString();
                my_theme = f_convert_theme_string_to_enum(my_theme_string);
            }
        }

        return my_theme;
    }

    private void f_update_game_id()
    {
        int my_game_id = _global_game_state_object.p_game_id;
        int my_player_id = Convert.ToInt32(Session["p_session_player_id"]);
        lbl_game_id.Text = String.Format("Table/Game: {0}/{1}. PlayerID [{2}] Session ID [{3}] ", _global_game_state_object.p_table_id, my_game_id, my_player_id, Session.SessionID);
    }
   
 
    
    private object f_get_object_from_json_call_to_server(string endpoint, Type typeIn)
    {
        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
        my_log.p_message = String.Format("Method called with endpoint [{0}]", endpoint); ;
        my_log.p_method_name = "f_get_object_from_json_call_to_server";
        my_log.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
        my_log.p_table_id = p_session_personal_table_id;
        log.Debug(my_log.ToString());
        object obj = null;
        HttpWebRequest request;
        String my_service_url = "not set";


        my_log.p_message = String.Format("WARNING! If a exception happens in this function is does not appear to user.");
        log.Warn(my_log.ToString());
        //throw new Exception("Test is this still used.2");


        my_service_url = WebConfigurationManager.AppSettings["hedgeem_server_default_webservice_url"];
        my_log.p_message = String.Format("Hedgeem Webservice URI = [{0}{1}]", my_service_url, endpoint);

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
    static string ConvertStringArrayToStringJoin(string[] array)
    {
        //
        // Use string Join to concatenate the string elements.
        //
        string result = string.Join(",", array);
        return result;
    }

    

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.RequestType;

        string my_cards = "adahqd2cks3cjdjc6h4h3h2s2d";

        if (Request.QueryString["cards"] != null) { 
            my_cards = Request.QueryString["cards"].ToString();
        }

        {
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "frm_hedgeem_odds.aspx.cs method called.";
            my_log.p_method_name = "Page_Load";
            my_log.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
            my_log.p_game_id = game_id;
            my_log.p_table_id = 1609;
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
                    int my_player_id = Convert.ToInt32(Session["p_session_player_id"]);
                    p_session_personal_table_id = Convert.ToInt32(Session["p_session_personal_table_id"]);

                    //f_preload_images();

                    bool ispostback = Page.IsAsync;

                    if (Page.IsPostBack == false)
                    {
                        try
                        {

                            _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server(String.Format("ws_recalculate_using_these_cards/1611,{0}/",my_cards), typeof(HedgeEmGameState));
                            if (_global_game_state_object.p_error_message != null)
                            {
                                if (_global_game_state_object.p_error_message != "")
                                {
                                    throw new Exception(_global_game_state_object.p_error_message);
                                }
                            }

                            my_log.p_message = String.Format("Successfully retrieved gamestate from server. Table ID [{0}], State [{1}]", _global_game_state_object.p_table_id, _global_game_state_object.p_current_state_enum.ToString());
                            log.Debug(my_log.ToString());
                        }
                        catch (Exception ex)
                        {
                            my_log.p_message = String.Format("Error trying to get game state from server. Reason [{0}]", ex.Message);
                            log.Error(my_log.ToString());
                            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
                            my_popup_message.p_detailed_message_str = "";
                            my_popup_message.p_is_visible = false;

                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                            my_popup_message.p_detailed_message_str = String.Format("Error trying to get game state from server. Reason [{0}]", ex.Message);
                            my_popup_message.p_is_visible = true;
                            Place_Holder_Popup_Message.Controls.Add(my_popup_message);

                        }
                        
                        game_id = _global_game_state_object.p_game_id;
                        number_of_hands = _global_game_state_object.p_number_of_hands_int;
                        enum_betting_stage my_betting_stage = f_get_current_betting_stage();
                        _game_state = _global_game_state_object.p_current_state_enum;
                        _hedgeem_hand_panels = new hedgeem_hand_panel[number_of_hands];
                        Session["sess_betting_stage_enum"] = _global_game_state_object.p_current_betting_stage_enum;
                        _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
                        _hedgeem_betting_panels = new BETTING_PANEL[_int_number_of_betting_stages, number_of_hands];
                        lbl_game_id.Text = String.Format("Table/Game: {0}/{1} ", _global_game_state_object.p_table_id, game_id);
                        
                        f_call_functions_to_render_screen();
                    }
             }
            }
            

            catch (Exception ex)
            {
                //string my_error_popup = "alert('Error in Page Load - " + ex.Message.ToString() + "');";
                string my_error_popup = String.Format("Fatal Error in frm_hedgeem_odds.aspx.cs Page Load. Reason [{0}]", ex.Message);
                my_log.p_message = String.Format("Fatal Error in frm_hedgeem_odds.aspx.cs Page Load. Reason [{0}]", ex.Message);
                // xxxeh This exception does not show to users
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
                my_popup_message.p_detailed_message_str = "";
                my_popup_message.p_is_visible = false;

                //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                my_popup_message.p_detailed_message_str = my_error_popup;
                my_popup_message.p_is_visible = true;
                Place_Holder_Popup_Message.Controls.Add(my_popup_message);
                log.Error(my_log.ToString());
                //throw new Exception(my_error_popup); 
         
            }
        }
        
    }

   
    
    
    private enum_theme f_convert_theme_string_to_enum(string a_theme_as_string)
    {
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());

        enum_theme my_enum_theme = enum_theme.ONLINE;
        switch (a_theme_as_string)
        {
            case "CASINO":
                my_enum_theme = enum_theme.CASINO;
                break;
            case "TEST":
                my_enum_theme = enum_theme.TEST;
                break;
            case "ONLINE":
                my_enum_theme = enum_theme.ONLINE;
                break;
            case "RESPONSIVE":
                my_enum_theme = enum_theme.RESPONSIVE;
                break;
            case "MOBILE":
                my_enum_theme = enum_theme.MOBILE;
                break;
            case "GAMEPLAY_STATUS":
                my_enum_theme = enum_theme.GAMEPLAY_STATUS;
                break;
            case "RETRO":
                my_enum_theme = enum_theme.RETRO;
                break;
            case "LEO_VEGAS":
                my_enum_theme = enum_theme.LEO_VEGAS;
                break;
            case "WSOP":
                my_enum_theme = enum_theme.WSOP;
                break;
            case "HORSERACE":
                my_enum_theme = enum_theme.HORSERACE;
                break;

            default:
                break;
        }

        return my_enum_theme;
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
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());

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

            if (myHedgeEmHandStageInfoList.Count > 0)
            {
                myHedgeEmHandStageInfo = myHedgeEmHandStageInfoList[0];
            } else
            {
                my_log_event.p_message = String.Format("Unusual circumstance.  Did not find any 'HandStatusInfo' for GameState [{0}], Hand Index[{1}] ", a_enum_game_state.ToString(), a_hand_index);
                my_log_event.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
                my_log_event.p_game_id = game_id;
                my_log_event.p_table_id = p_session_personal_table_id;
                log.Warn(my_log_event.ToString());

            }

        }
        catch (Exception ex)
        {
            //  ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_log_event.p_message = String.Format("Exception caught [{0}] ", ex.Message);
            my_log_event.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
            my_log_event.p_game_id = game_id;
            my_log_event.p_table_id = p_session_personal_table_id;
            log.Error(my_log_event.ToString());
            
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_log_event.p_message;
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
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
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());

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
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_log_event.p_message = String.Format("Exception caught [{0}] ", ex.Message);
            my_log_event.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
            my_log_event.p_game_id = game_id;
            my_log_event.p_table_id = p_session_personal_table_id;
            log.Error(my_log_event.ToString());
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_log_event.p_message;
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
        }
        return myHedgeEmHandStageInfo;
    }


    
    #region f_update_hedgeem_control_board_cards_with_info_from_server
    /// <summary>
    /// This method is used to get the values of all Flop Cards when 
    /// someone click on Deal Buttons except Hole Button. Then save 
    /// that value of Flop Card to Session.
    /// Also get CSS value from session as described below. */
    /// </summary>
    private void f_update_hedgeem_control_board_cards_with_info_from_server()
    {
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());

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


           

            lbl_game_id.Text = String.Format("Table/Game: {0}/{1} ", _global_game_state_object.p_table_id, game_id);

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
            string my_error_popup =  ex.Message.ToString();
            //  ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_log_event.p_message = "Exception caught in f_update_hedgeem_control_board_cards_with_info_from_server function " + ex.Message;
            my_log_event.p_method_name = "f_update_hedgeem_control_board_cards_with_info_from_server";
            my_log_event.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
            my_log_event.p_game_id = game_id;
            my_log_event.p_table_id = p_session_personal_table_id;
            log.Error(my_log_event.ToString());
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup;
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
        }
    }
    #endregion f_update_hedgeem_control_flop_cards_with_info_from_server

    
    #region f_update_hedgeem_control_betting_panels_with_info_from_server
    //  This function is used to maintain the value of bet placed on each time page is refreshed.
    protected void f_update_hedgeem_control_betting_panels_with_info_from_server()
    {
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());

        /* Get value of Selected_Hand_Panel for bet from textbox and save it in a variable */
        try
        {
            log.Debug("f_update_hedgeem_control_betting_panels_with_info_from_server called");
            // xxxHC value of theme
            enum_theme my_theme = f_get_current_theme_as_enum();
            enum_betting_stage my_current_betting_stage = _global_game_state_object.p_current_betting_stage_enum;
            enum_game_state my_current_game_stage_state_hack = f_convert_hedgeem_stage_to_state(my_current_betting_stage);
            enum_game_state my_current_game_state2 = _global_game_state_object.p_current_state_enum;
            int my_number_of_seats = _global_game_state_object.p_number_of_seats_int;


            // Only show the betting panels for CASINO, MOBILE, LEO_VEGAS, or WSOP Styles
            if (!(my_theme == enum_theme.CASINO || my_theme == enum_theme.MOBILE || my_theme == enum_theme.WSOP || my_theme == enum_theme.RETRO || my_theme == enum_theme.LEO_VEGAS || my_theme == enum_theme.GAMEPLAY_STATUS))
            {
                // Do not show the betting panels
                // return;
            }
            else
            {
                //For each betting stage and each hand ...
                for (enum_betting_stage stage_index = enum_betting_stage.HOLE_BETS; stage_index <= enum_betting_stage.TURN_BETS; stage_index++)
                {
                    enum_game_state my_game_state = f_convert_hedgeem_stage_to_state(stage_index);

                    for (int hand_index = 0; hand_index < number_of_hands; hand_index++)
                    {
                        // ... retrieve the HandStageInfo object, then ... 
                        HedgeEmHandStageInfo my_hand_stage_info;
                        my_hand_stage_info = f_get_hand_stage_info_object_for_stage_and_hand(my_game_state, hand_index);
                        if (my_hand_stage_info == null)
                        {
                            //string my_error_msg = String.Format("HedgeEmHandStageInfo object null for stage[0], hand_index[{1}]",my_game_state.ToString(),hand_index);
                            //throw new Exception(my_error_msg);
                            // xxx hack to create a empty object when no data returned from server (Simon 30 Jan 2016
                            my_hand_stage_info = new HedgeEmHandStageInfo(my_game_state, hand_index);
                        }

                        // Create a 'HedgeEmControl' to display the BettingPanel for this Betting stage and hand.
                        _hedgeem_betting_panels[(int)stage_index, hand_index] = new BETTING_PANEL(my_number_of_seats);
                        Place_Holder_Betting_Panel.Controls.Add(_hedgeem_betting_panels[(int)stage_index, hand_index]);
                        _hedgeem_betting_panels[(int)stage_index, hand_index].p_hand_index = hand_index;

                        if (_global_game_state_object._hands.Count() != 0)
                        {
                            // ... update the corresponding Betting Panel with info from server
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_enum_betting_stage = stage_index;
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_percent_draw = my_hand_stage_info.p_odds_percent_draw_string;
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_percent_win_or_draw = my_hand_stage_info.p_odds_percent_win_or_draw_string;
                            // xxx WARN .p_odds_actual_string is not being set as of Nov 2014 hence calling double.ToString();
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_actual = my_hand_stage_info.p_odds_actual_double.ToString();
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_margin = my_hand_stage_info.p_odds_margin_double.ToString();

                            if (my_current_game_state2 == enum_game_state.STATUS_RIVER && my_hand_stage_info.p_is_hand_a_winner_at_this_stage)
                            {
                                _hedgeem_betting_panels[(int)stage_index, hand_index].p_enum_panel_display_status = enum_hand_in_play_status.IN_PLAY_WINNER;
                            }
                            if (my_current_game_state2 == enum_game_state.STATUS_RIVER && !my_hand_stage_info.p_is_hand_a_winner_at_this_stage)
                            {
                                _hedgeem_betting_panels[(int)stage_index, hand_index].p_enum_panel_display_status = enum_hand_in_play_status.IN_NON_BETTING_STAGE;
                            }

                            if (my_hand_stage_info.p_enum_game_state < my_current_game_stage_state_hack && my_current_game_state2 != enum_game_state.STATUS_RIVER)
                            {
                                _hedgeem_betting_panels[(int)stage_index, hand_index].p_enum_panel_display_status = enum_hand_in_play_status.IN_PLAY_PREVIOUS_BETTING_STAGE_NOT_ACTIVE;
                            }

                            if (my_hand_stage_info.p_enum_game_state == my_current_game_stage_state_hack && my_current_game_state2 != enum_game_state.STATUS_RIVER)
                            {
                                _hedgeem_betting_panels[(int)stage_index, hand_index].p_enum_panel_display_status = my_hand_stage_info.p_hand_inplay_status;
                            }

                            double my_offered_odds_double = my_hand_stage_info.p_odds_margin_rounded_double;
                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odd_margin_rounded = my_offered_odds_double;

                            String prefix;
                            string my_offered_odds_string = "";
                            if (my_offered_odds_double < 0)
                            {
                                prefix = "1/";
                                my_offered_odds_double = my_offered_odds_double * -1;
                                my_offered_odds_string = prefix + my_offered_odds_double.ToString();
                            }
                            else
                            {
                                prefix = "";
                                my_offered_odds_string = prefix + my_offered_odds_double.ToString();
                            }
                            if (my_offered_odds_double == 0)
                            {

                                my_offered_odds_string = "X";
                            }


                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_payout_string = my_offered_odds_string;


                            // xxx HC to true to show admin
                            //string role = Session["user_role"].ToString();
                            string role = "ADMIN";
                            // if user is admin, show cashier button
                            if (role == enum_user_role.ADMIN.ToString())
                            {
                                _hedgeem_betting_panels[(int)stage_index, hand_index]._show_admin_info = true;
                            }
                            else
                            {
                                _hedgeem_betting_panels[(int)stage_index, hand_index]._show_admin_info = false;
                            }

                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odds_percent_win = my_hand_stage_info.p_odds_percent_win_string;

                            double offered_odds = my_hand_stage_info.p_odds_margin_rounded_double;

                            _hedgeem_betting_panels[(int)stage_index, hand_index].p_odd_margin_rounded = offered_odds;

                        }

                        }

                }
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "Error in f_update_hedgeem_control_betting_panels_with_info_from_server" + ex.Message.ToString();
            //my_error_popup = "Bollocks";
           // ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup;
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
            my_log_event.p_message = "Exception caught in f_update_hedgeem_control_betting_panels_with_info_from_server function " + ex.Message;
            my_log_event.p_method_name = "f_update_hedgeem_control_betting_panels_with_info_from_server";
            my_log_event.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
            my_log_event.p_game_id = game_id;
            my_log_event.p_table_id = p_session_personal_table_id;
            log.Error(my_log_event.ToString());
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
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());

        log.Debug("f_update_hedgeem_control_hand_panels_with_info_from_server called");
        int my_number_of_seats = _global_game_state_object.p_number_of_seats_int;
        try
        {
            for (int hand_index = 0; hand_index < number_of_hands; hand_index++)
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

                _hedgeem_hand_panels[hand_index] = new hedgeem_hand_panel(my_num_stages, my_number_of_seats);
                // Hand_Index
                _hedgeem_hand_panels[hand_index].p_hand_index = hand_index;

                // Card 1
                _hedgeem_hand_panels[hand_index].p_card1 = p_hand_card1;

                // Card 2 
                _hedgeem_hand_panels[hand_index].p_card2 = p_hand_card2;

                _hedgeem_hand_panels[hand_index].p_current_betting_stage = f_get_current_betting_stage();

               

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
            string my_error_popup = "Error in f_update_hedgeem_control_hand_panels_with_info_from_server" + ex.Message.ToString();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup;
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
            my_log_event.p_message = "Exception caught in f_update_hedgeem_control_hand_panels_with_info_from_server function " + ex.Message;
            my_log_event.p_method_name = "f_update_hedgeem_control_hand_panels_with_info_from_server";
            my_log_event.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
            my_log_event.p_game_id = game_id;
            my_log_event.p_table_id = p_session_personal_table_id;
            log.Error(my_log_event.ToString());
            // xxxeh - this error does not pop up
            throw new Exception(my_error_popup);
        }

    }
    #endregion f_update_hedgeem_control_hand_panels_with_info_from_server

    
    #region f_update_labels
    // this function update labels
    public void f_update_labels()
    {
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());

        string best_odds_token = "";
        log.Debug("f_update_labels called");
        int my_number_of_seats = _global_game_state_object.p_number_of_seats_int;

        try
        {
            log.Info("Call f_update_seat_panels_with_info_from_servers in f_update_labels");
            f_update_seat_panels_with_info_from_server_NOT_USED();
        }
        catch (Exception e)
        {
            string err_message = string.Format("Unable to update seat panels method f_update_labels \nReason {}", e.Message);
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", err_message, true);
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = err_message;
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
            my_log_event.p_message = "Exception caught in f_update_labels function - Unable to update seat panels method in f_update_labels " + e.Message;
            my_log_event.p_method_name = "f_update_labels";
            my_log_event.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
            my_log_event.p_game_id = game_id;
            my_log_event.p_table_id = p_session_personal_table_id;
            log.Error(my_log_event.ToString());
        }

        //For each betting stage and each hand
        for (enum_betting_stage stage_index = enum_betting_stage.HOLE_BETS; stage_index <= enum_betting_stage.TURN_BETS; stage_index++)
        {

            for (int hand_index = 0; hand_index < number_of_hands; hand_index++)
            {
                // xxx should really use Doubles here 
                _hedgeem_betting_panels[(int)stage_index, hand_index] = new BETTING_PANEL(my_number_of_seats);
                _hedgeem_betting_panels[(int)stage_index, hand_index] = new BETTING_PANEL(my_number_of_seats);
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
                for (int seat_index = 0; seat_index < my_number_of_seats; seat_index++)
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
                        string my_error_popup = "Error in f_update_labels" + e.Message.ToString();
                       // ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                        HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
                        my_popup_message.p_detailed_message_str = "";
                        my_popup_message.p_is_visible = false;

                        //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                        my_popup_message.p_detailed_message_str = my_error_popup;
                        my_popup_message.p_is_visible = true;
                        Place_Holder_Popup_Message.Controls.Add(my_popup_message);
                        my_log_event.p_message = "Exception caught in f_update_labels function " + e.Message;
                        my_log_event.p_method_name = "f_update_labels";
                        my_log_event.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
                        my_log_event.p_game_id = game_id;
                        my_log_event.p_table_id = p_session_personal_table_id;
                        log.Error(my_log_event.ToString());
                    }
                }
            }
        }
    }
    #endregion f_update_labels

    
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
        int my_number_of_seats = _global_game_state_object.p_number_of_seats_int;

        for (int stageindex = 0; stageindex < _int_number_of_betting_stages; stageindex++)
        {
            for (int handindex = 0; handindex < number_of_hands; handindex++)
            {
                _hedgeem_betting_panels[stageindex, handindex] = new BETTING_PANEL(my_number_of_seats);
            }
        }
    }
    #endregion f_createControl_Bettinf_update_seat_panels_with_info_from_serverrid

    #region f_update_seat_panels_with_info_from_server
    private void f_update_seat_panels_with_info_from_server_NOT_USED()
    {
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());
        my_log_event.p_game_id = game_id;

        try
        {
            int my_number_of_seats = _global_game_state_object.p_number_of_seats_int;

            // For each seat at this table sit a player from the knoww list of current players
            for (int seat_index = 0; seat_index < my_number_of_seats; seat_index++)
            {
                // Create a HedgeEm Seat object to use to display player/seat information for all players seated at this table
                ss_seat = new hedgeem_control_seats();
                Place_Holder_Player_Info.Controls.Add(ss_seat);

                // Determine what values need to be stored for this player/seat  (read most from the object returned from the HedgeEmServer)
                int my_seat_id = _global_game_state_object._seats[seat_index].p_seat_id;
                int my_seat_player_id = _global_game_state_object._seats[seat_index].p_player_id;
                string my_seat_player_name = _global_game_state_object._seats[seat_index].p_player_name;
                double my_player_funds_at_seat = _global_game_state_object._seats[seat_index].p_player_seat_balance;
                string player_seat_balance_text = String.Format("£{0:#0.00}", my_player_funds_at_seat);
                string avatar_image_name = _global_game_state_object._seats[seat_index].p_user_avitar_image_url;
                string chip_icon_resource_name = String.Format("../resources/chips/chip_icon_{0}.png", seat_index);

                //Update the HedgeEm seat values with the information derived above
                ss_seat.p_player_name = my_seat_player_name;
                ss_seat.p_photo_image = avatar_image_name;
                ss_seat.p_balance = player_seat_balance_text;
                ss_seat.p_seat_index = my_seat_id;
                ss_seat.p_player_id = my_seat_player_id;
                ss_seat.p_chip_icon = chip_icon_resource_name;
            }

        }
        catch (Exception ex)
        {
            string my_error_popup = ex.Message.ToString();
           // ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup;
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
            my_log_event.p_message = "Exception caught in f_update_seat_panels_with_info_from_servers function " + ex.Message;
            my_log_event.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
            my_log_event.p_game_id = game_id;
            my_log_event.p_table_id = p_session_personal_table_id;
            log.Error(my_log_event.ToString());
        }
    }
    #endregion

    protected void btnLobby_Click(object sender, ImageClickEventArgs e)
    {
        Session["fb_hide_logout"] = "fb_hide_logout";
        Response.Redirect("frm_website_home.aspx");
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
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());

        f_update_hedgeem_control_hand_panels_with_info_from_server();
        f_update_hedgeem_control_board_cards_with_info_from_server();
        f_update_hedgeem_control_betting_panels_with_info_from_server();
        f_update_game_id();
    }
    #endregion f_call_functions_to_render_screen

    
    
   
    /// <summary>
    /// Get the current betting stage for the table.  Assume we already have this and it is 'cached' in session.
    /// If not get it from HedgeEmServer
    /// </summary>
    /// <returns></returns>
    enum_betting_stage f_get_current_betting_stage()
    {
        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());

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
                _global_game_state_object = (HedgeEmGameState)f_get_object_from_json_call_to_server("get_game_state_object/" + p_session_personal_table_id, typeof(HedgeEmGameState));
                my_log_event.p_message = String.Format("Successfully retrieved gamestate from server. Table ID [{0}], State [{1}]", _global_game_state_object.p_table_id, _global_game_state_object.p_current_state_enum.ToString());
                log.Debug(my_log_event.ToString());
                my_enum_betting_stage = _global_game_state_object.p_current_betting_stage_enum;
            }
            catch (Exception ex)
            {
                my_log_event.p_message = String.Format("Error trying to get game state from server. Reason [{0}]", ex.Message);
                log.Error(my_log_event.ToString());
                return enum_betting_stage.NON_BETTING_STAGE;
            }

        }

        return my_enum_betting_stage;

    }

    
    


    [WebMethod]
    public string[] f_update_hedgeem_control_hand_panels()
    {

        // Create a 'log event' object to audit execution
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered.";
        my_log_event.p_player_id = p_session_player_id;
        //my_log_event.p_table_id = p_session_personal_table_id;
        log.Debug(my_log_event.ToString());


        log.Debug("f_update_hedgeem_control_hand_panels_with_info_from_server called");
        List<string> details = new List<string>();
        HedgeEmGameState _global_game_state_object = new HedgeEmGameState();
        int p_session_personal_table_id = 2058;



        object obj = null;
        HttpWebRequest request;
        String my_service_url = "not set";
        my_service_url = WebConfigurationManager.AppSettings["hedgeem_server_default_webservice_url"];

        request = WebRequest.Create(my_service_url + "get_game_state_object/2058") as HttpWebRequest;


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
                                throw new Exception(my_error_msg);
                            }
                            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
                            if (typeof(HedgeEmGameState) != null)
                            {
                                var ser = new DataContractJsonSerializer(typeof(HedgeEmGameState));
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


        _global_game_state_object = (HedgeEmGameState)obj;



        int my_number_of_seats = _global_game_state_object.p_number_of_seats_int;
        try
        {
            string p_hand_card1 = "ZZ";
            string p_hand_card2 = "ZZ";
            //ArrayList list = new ArrayList();
            //int number_of_hands = _global_game_state_object.p_number_of_hands_int;
            int number_of_hands = 4;
            for (int hand_index = 0; hand_index < number_of_hands; hand_index++)
            {
                // call to get HedgeEmHandStageInfo for given hand index at given stage
                //  HedgeEmHandStageInfo my_hedgeem_hand_stage_info = f_get_hand_stage_info_object_for_stage_and_hand(_global_game_state_object.p_current_state_enum, hand_index);

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
                int _int_number_of_betting_stages = _global_game_state_object.p_number_of_betting_stages_int;
                int my_num_stages = _int_number_of_betting_stages;
                hedgeem_hand_panel[] _hedgeem_hand_panels;

                _hedgeem_hand_panels = new hedgeem_hand_panel[number_of_hands];

                _hedgeem_hand_panels[hand_index] = new hedgeem_hand_panel(my_num_stages, my_number_of_seats);
                //// Hand_Index
                _hedgeem_hand_panels[hand_index].p_hand_index = hand_index;

                // Card 1
                _hedgeem_hand_panels[hand_index].p_card1 = p_hand_card1;

                // Card 2 
                _hedgeem_hand_panels[hand_index].p_card2 = p_hand_card2;

                string st = _hedgeem_hand_panels[hand_index].p_card_image_filename_card1;
                string st1 = _hedgeem_hand_panels[hand_index].p_card_image_filename_card2;

                //string st = p_hand_card1;
                //string st1 = p_hand_card2;

                details.Add(st);
                details.Add(st1);

                //list.Add(st);
                //list.Add(st1);

                // Add the created control to the Webpage (HedgeEmTable.aspx) for display to the user.
                //_hedgeem_hand_panels[hand_index].ID = "auto_div_hand_id" + hand_index;
                //_hedgeem_hand_panels[hand_index].CssClass = "auto_div_hand_id_class";
                // Place_Holder_Hand_Panel.Controls.Add(_hedgeem_hand_panels[hand_index]);

            }

        }


        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_update_hedgeem_control_hand_panels_with_info_from_server" + ex.Message.ToString() + "');";
            //   ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeEmLogEvent my_log = new HedgeEmLogEvent();
            my_log.p_message = "Exception caught in f_update_hedgeem_control_hand_panels_with_info_from_server function " + ex.Message;
            my_log.p_method_name = "f_update_hedgeem_control_hand_panels_with_info_from_server";
            // my_log.p_player_id = Convert.ToInt32(Session["p_session_player_id"]);
            // my_log.p_game_id = game_id;
            //       my_log.p_table_id = p_session_personal_table_id;
            log.Error(my_log.ToString());
            // xxxeh - this error does not pop up
            throw new Exception(my_error_popup);

        }
        // JavaScriptSerializer TheSerializer = new JavaScriptSerializer();
        // var TheJson = TheSerializer.Serialize(details);
        return details.ToArray();
    }


    private int p_session_player_id
    {
        get
        {
            int my_player_id = -1;
            try
            {
                my_player_id = Convert.ToInt32(Session["p_session_player_id"]);
            }
            catch (Exception e)
            {
                my_player_id = -1;
            }

            return my_player_id;
        }

        //set { Session["p_session_player_id"] = value; }
    }
}