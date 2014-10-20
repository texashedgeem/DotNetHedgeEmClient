using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.Services;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;
using HedgeEmClient;
using System.Runtime.Serialization.Json;
using System.Net;

public partial class frm_facebook_canvas : System.Web.UI.Page
{
    private static String logger_name_as_defined_in_app_config = "client." +
    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(logger_name_as_defined_in_app_config);
    //Comment added by jaspreet
    String my_username = "";
    int playerid;
    Double xxxHC_a_opening_balance;
    String xxxHC_a_password = "";
    String xxxHC_a_full_name = "";
    //This should not be a global variable [_xxx_log_event]
    HedgeEmLogEvent _xxx_log_event = new HedgeEmLogEvent();
    string website_url = WebConfigurationManager.AppSettings["WebsiteUrl"].ToString();

    localhost.WebService service = new localhost.WebService();

    static string ConvertStringArrayToStringJoin(string[] array)
    {
        //
        // Use string Join to concatenate the string elements.
        //
        string result = string.Join(",", array);
        return result;
    }

    // preinit event changes the theme of the page at runtime before page loads
    protected void Page_PreInit(object sender, EventArgs e)
    {
        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
        my_log.p_method_name = "Page_PreInit";
        my_log.p_player_id = playerid;
        my_log.p_table_id = 666;
        my_log.p_message = "Method called.";
        log.Debug(my_log.ToString());
        try
        {
            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();
            this.Page.Theme = "ONLINE";
            // If the session has expired we would have lost critical session state so re-direct the users to the home page.
            if (Session.Count == 0)
            {
                my_log.p_message = "No session detected";
                log.Warn(my_log.ToString());
                this.Page.Theme = "ONLINE";
                //ScriptManager.RegisterStartupScript(Page, GetType(), "SessionTimeOutMsg", "show_session_timeout_message();", true);
                // Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>show_session_timeout_message();</script>");
            }
            else
            {
                if (Session["theme"] != null)
                {
                    // this changes the theme of the hedgeem table according to the theme selected by the user.
                    this.Page.Theme = Session["theme"].ToString();

                    if (this.Page.Theme == "")
                    {
                        // xxxeh - need to throw error when string =""
                        this.Page.Theme = "ONLINE";
                    }


                }
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in Page PreInit - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_log = new HedgeEmLogEvent();
            my_log.p_message = ex.Message;
            my_log.p_method_name = "Page Load";
            my_log.p_player_id = playerid;
            my_log.p_table_id = 666;
            log.Error(my_log.ToString());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = "frm_facebook_canvas.Page_Load";
        my_log_event.p_message = "Method called.";
        log.Debug(my_log_event.ToString());
        try
        {
            if (Request.QueryString["un"] != null && Request.QueryString["p"] != null)
            {
                string retrieved_username = Decryptdata(Request.QueryString["un"].ToString());
                string retrieved_password = Decryptdata(Request.QueryString["p"].ToString());
                string retrieved_displayname = Decryptdata(Request.QueryString["dn"].ToString());
                Session["username"] = retrieved_username;
                Session["password"] = retrieved_password;
                Session["display_name"] = retrieved_displayname;
                //  service.f_activate_user(retrieved_username);
                //    my_log_event.p_message = String.Format("Username [{0}] activated. ", retrieved_username);
                my_log_event.p_message = String.Format("User [{0}] clicked on link activation. ", retrieved_username);
                ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('Thanks for verifying your account. You can now play the game.'); if(alert){ window.location='frm_facebook_canvas.aspx';}", true);
                log.Debug(my_log_event.ToString());

            }
            if (Page.IsPostBack == false)
            {
                //#region load_all_images_at_once
                ////Get Path of Images
                //var path = Server.MapPath("~/resources/cards/");
                //string[] images = Directory.GetFiles(path, "*.png");
                //List<string> get_list_of_images = new List<string>();
                //foreach (string image in images)
                //{
                //    string name = Path.GetFileName(image);
                //    // Add image names in list
                //    get_list_of_images.Add("'resources/cards/" + name + "'");
                //}
                //// convert list to array
                //string[] array_of_images = get_list_of_images.ToArray();
                //// convert array to string along with joining with ','
                //string images_ = ConvertStringArrayToStringJoin(array_of_images);

                //// script to call the Progress Bar while images are loading
                //Page.RegisterStartupScript("Loading", "<script type='text/javascript'> var myImages = [" + images_ + "]; progressBar(myImages.length); </script>");

                //// script to load all images
                //Page.RegisterStartupScript("Prefetch Images", "<script type='text/javascript'> var myImages = [" + images_ + "]; for (var i = 0; i <= myImages.length; i++) { var img = new Image();  img.src = myImages[i];}</script>");
                //#endregion load_all_images_at_once

            }
            else
            {
                Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('progressbar').style.display='none';</script>");
            }

            if (Request.QueryString["r"] != null)
            {
                my_log_event.p_message = String.Format("Session Timedout message shown to user.");
                log.Debug(my_log_event.ToString());
                Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>show_session_timeout_message();</script>");
            }
            else
            {
                if (usr_image.ImageUrl == "")
                {
                    usr_image.ImageUrl = "../resources/avitars/player_avitar_sit_here.JPG";
                    my_log_event.p_message = String.Format("Player avitar image not found so showing default sit here..");
                    log.Debug(my_log_event.ToString());
                }

                if (Session["fb_hide_logout"] != null)
                {
                    //   chk_fb_visibility_logout.Visible = false;
                    btn_play_now.Visible = true;
                }

                if (Request.QueryString["action"] != null)
                {
                    Response.Clear();
                    Session.Abandon();
                    Response.Redirect("frm_facebook_canvas.aspx");
                }

                if (Session["username"] != null)
                {

                    Logout.Attributes.Add("style", "display:block!Important;");
                    Page.RegisterStartupScript("OnLoading", "<script>load_edit_profile();</script>");
                    if (Session["display_name"] != null)
                    {
                        lbl_user_name.Text = Session["display_name"].ToString();
                    }
                    LoginDiv.Attributes.Add("style", "display:none !Important;");
                    if (Session["username"] != null)
                    {
                        if (File.Exists(Server.MapPath("resources/player_avatar_" + Session["username"].ToString() + ".jpg")))
                        {
                            usr_image.ImageUrl = "../resources/player_avatar_" + Session["username"].ToString() + ".jpg";
                        }
                        else
                        {
                            usr_image.ImageUrl = "../resources/avitars/user_square.png";
                        }
                    }

                    btn_play_now.Enabled = true;

                    HedgeEmPlayer my_player = get_player_details(Session["username"].ToString());

                    // check user role
                    //DataTable dt = service.f_get_password_from_db(Session["username"].ToString());

                    //string role = "";
                    //if (dt.Rows.Count > 0)
                    //{
                    //    string password = dt.Rows[0]["password"].ToString();
                    //    DataTable userdetails = service.my_user_details(Session["username"].ToString(), password);
                    //    role = userdetails.Rows[0]["role"].ToString();
                    //    my_log_event.p_message = String.Format("User/player role determined to be [{0}].", role.ToString());
                    //    log.Debug(my_log_event.ToString());
                    //}
                    string role = "";
                    if (my_player != null)
                    {
                        role = my_player.p_role;

                        my_log_event.p_message = String.Format("User/player role hardcoded to BASIC_USER");
                        log.Debug(my_log_event.ToString());
                        //my_log_event.p_message = String.Format("User/player role determined to be [{0}].", role.ToString());
                        //log.Debug(my_log_event.ToString());
                    }

                    if (role == enum_user_role.ADMIN.ToString())
                    {
                        btnAdmin.Visible = true;
                    }
                    //  Session["user_role"] = role;
                    // - xxx- Hardcoding value of role to BASIC_USER as need to discuss this with Simon.
                    Session["user_role"] = "BASIC_USER";
                    Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('progressbar').style.display='none';</script>");
                }


                else
                {

                }

                //Page.RegisterStartupScript("New_User_Popup", "<script type='text/javascript'>function newPopup(url) {popupWindow = window.open(url, 'popUpWindow', 'height=220,width=1250,left=50,top=200,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes')}</script>");
                //Page.RegisterStartupScript("Update_Deposit_Amount", "<script type='text/javascript'>function depositupdatePopup(url) {popupWindow = window.open(url, 'popUpWindow', 'height=200,width=450,left=300,top=200,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes')}</script>");


                if (Session["username"] != null || Session["password"] != null)
                {
                    if (Convert.ToInt32(Session["playerid"]) != 10001)
                    {
                        deposit_update.Visible = true;
                        deposit_update.Attributes.Add("style", "margin:0 0 -25px !Important");
                    }
                }
                if (Session["chk_logout_hide"] != null)
                {

                    //  chk_fb_visibility.Visible = true;
                    btn_play_now.Visible = true;
                    btnLogout.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in Page Load of frm_facebook_canvas - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);

            my_log_event.p_message = "Exception caught in Page Load of frm_facebook_canvas- " + ex.Message;
            log.Error(my_log_event.ToString(), new Exception(ex.Message));
        }
    }

    // Method to get details of Player from JSON Web Service
    public HedgeEmPlayer get_player_details(string username)
    {
        HedgeEmPlayer my_player = null;
        log.Debug("get_player_details called for getting details of user - " + username);
        try
        {
            List<HedgeEmPlayer> my_player_list = (List<HedgeEmPlayer>)f_get_object_from_json_call_to_server("ws_get_player_list/sessionabc123,10,10000/", typeof(List<HedgeEmPlayer>));
            foreach (HedgeEmPlayer player in my_player_list)
            {
                if (player.p_username == username)
                {
                    my_player = player;
                    return my_player;
                }
                else
                {
                    my_player = null;
                }
            }

        }
        catch (Exception ex)
        {
            log.Error("Exception in get_player_details() function, - " + ex.Message);
        }
        return my_player;
    }

    // Method to get details of Table from JSON Web Service
    public HedgeEmTableSummary get_table_details(string username)
    {
        HedgeEmTableSummary my_table = null;
        log.Debug("get_table_details called for getting details of table for user - " + username);
        try
        {
            string my_table_name = username + "_Personal";
            List<HedgeEmTableSummary> my_table_list = (List<HedgeEmTableSummary>)f_get_object_from_json_call_to_server("ws_get_hedgeem_table_list/sessionabc123,10,10000/", typeof(List<HedgeEmTableSummary>));
            foreach (HedgeEmTableSummary table in my_table_list)
            {
                if (table.p_table_name == my_table_name)
                {
                    my_table = table;
                    return my_table;
                }
                else
                {
                    my_table = null;
                }
            }

        }
        catch (Exception ex)
        {
            log.Error("Exception in get_table_details() function, - " + ex.Message);
        }
        return my_table;
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
            HedgeEmPlayer my_player = (HedgeEmPlayer)f_get_object_from_json_call_to_server("ws_login/sessionabc123,10," + my_username + "," + my_password + "/", typeof(HedgeEmPlayer));
            if (my_player == null)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "show_error_message();", true);
            }
            else
            {
                int my_player_id = my_player.p_player_id;
                Session["playerid"] = my_player_id;
                Session["display_name"] = my_player.p_display_name;


                if (my_player_id == 10001)
                {
                    deposit_update.Visible = true;
                }
                else
                {
                    deposit_update.Visible = true;
                    deposit_update.Attributes.Add("style", "margin:0 0 -25px !Important");
                }
                Session["username"] = my_username;
                Session["password"] = my_password;
                //service.f_update_last_login_date(Session["username"].ToString());
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
                    if (File.Exists(Server.MapPath("resources/player_avatar_" + Session["username"].ToString() + ".jpg")))
                    {
                        usr_image.ImageUrl = "../resources/player_avatar_" + Session["username"].ToString() + ".jpg";
                    }
                    else
                    {
                        usr_image.ImageUrl = "../resources/avitars/user_square.png";
                    }
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
            HedgeEmPlayer my_player = (HedgeEmPlayer)f_get_object_from_json_call_to_server("ws_logout/sessionabc123,10," + Session["username"].ToString() + "/", typeof(HedgeEmPlayer));
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

    private object f_get_object_from_json_call_to_server(string endpoint, Type typeIn)
    {
        HedgeEmLogEvent my_log = new HedgeEmLogEvent();
        my_log.p_message = String.Format("Method called with endpoint [{0}]", endpoint); ;
        my_log.p_method_name = "f_get_object_from_json_call_to_server";
        my_log.p_player_id = playerid;

        log.Debug(my_log.ToString());
        object obj = null;
        HttpWebRequest request;
        String my_service_url = "not set";


        my_log.p_message = String.Format("WARNING! If a exception happens in this function is does not appear to user.");
        log.Warn(my_log.ToString());



        my_service_url = WebConfigurationManager.AppSettings["hedgeem_server_default_webservice_url"];
        //my_service_url = "http://devserver.hedgeem.com/Service1.svc/";
        my_log.p_message = String.Format("Webservice configured for GoDaddy Deployment URI = [{0}{1}]", my_service_url, endpoint); ;



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
            
            int a_table_id = my_hedgeem_table_summary.p_table_id;
            HedgeEmGenericAcknowledgement my_generic_ack = new HedgeEmGenericAcknowledgement();
            my_generic_ack = (HedgeEmGenericAcknowledgement)f_get_object_from_json_call_to_server("ws_sit_at_table/" + xxx_my_HC_session_id_not_used_yet + "," +
                                                10 + "," +
                                                a_table_id + "," +
                                                my_HC_seat_id + "," +
                                                player_id, typeof(HedgeEmGenericAcknowledgement));
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

            HedgeEmPlayer my_player = null;
            HedgeEmTableSummary my_table_details = null;

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

                // int user_exist = service.f_get_player_details_by_username(txt_get_fm_email_id.Text);
                my_player = get_player_details(txt_get_fm_email_id.Text);
                if (my_player != null)
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
                //DataTable dt = new DataTable();

                if (txt_get_fm_email_id.Text == "")
                {
                    my_log_event.p_message = String.Format("txt_get_fm_email_id hidden field is null so try to get info from service.");
                    log.Debug(my_log_event.ToString());
                    //dt = service.f_get_password_from_db(Session["username"].ToString());
                    my_player = get_player_details(Session["username"].ToString());
                }
                else
                {
                    my_log_event.p_message = String.Format("txt_get_fm_email_id hidden field is [{0}] so try to get info from service using this value", txt_get_fm_email_id.Text);
                    log.Debug(my_log_event.ToString());
                    // dt = service.f_get_password_from_db(txt_get_fm_email_id.Text);

                    my_player = get_player_details(txt_get_fm_email_id.Text);
                    Session["chkfb"] = "yes_facebook";
                }
                // If user exists in our db then will go into this
                if (my_player != null)
                {
                    my_log_event.p_message = String.Format("Webservice has record of user so now see if they have a HedgeEm Table", txt_get_fm_email_id.Text);
                    log.Debug(my_log_event.ToString());

                    //  password = dt.Rows[0]["password"].ToString();
                    // if (password != null)
                    {
                        //  Session["password"] = password.ToString();
                        // Check for table exists or not
                        // int chk_hedgeem_table_exits_or_not;

                        if (txt_get_fm_email_id.Text == "")
                        {
                            my_table_details = get_table_details(Session["username"].ToString());
                            // chk_hedgeem_table_exits_or_not = service.f_get_table_id_from_db(Session["username"].ToString());
                        }
                        else
                        {
                            my_table_details = get_table_details(txt_get_fm_email_id.Text);
                            // chk_hedgeem_table_exits_or_not = service.f_get_table_id_from_db(txt_get_fm_email_id.Text);
                        }


                        if (my_table_details == null)
                        {
                            playerid = my_player.p_player_id;

                            if (txt_get_fm_email_id.Text == "")
                            {
                                my_log_event.p_message = String.Format("This player does not have a HedgeEm table so create one.", Session["username"].ToString());

                                f_create_personal_hedgeem_table_for_player(playerid, Session["username"].ToString());
                            }
                            else
                            {
                                my_log_event.p_message = String.Format("This player does not have a HedgeEm table so create one.", txt_get_fm_email_id.Text);

                                f_create_personal_hedgeem_table_for_player(playerid, txt_get_fm_email_id.Text);
                            }
                            log.Debug(my_log_event.ToString());
                        }
                        else
                        {

                            //playerid = service.get_player_id(Session["username"].ToString(), password);
                            playerid = my_player.p_player_id;
                            Session["playerid"] = playerid;
                        }
                    }
                }

                if (txt_get_fm_email_id.Text == "")
                {
                    string my_username = Session["username"].ToString();
                    my_table_details = get_table_details(my_username);
                    //int my_table_id = service.f_get_table_id_from_db(my_username);
                    int my_table_id = my_table_details.p_table_id;

                    if (my_table_id <= 0)
                    {
                        string my_error_msg = String.Format("Invalid table ID [{0}]", my_table_id);
                        log.Error(my_error_msg);
                        throw new Exception(my_error_msg);
                    }
                    Session["tableid"] = my_table_id;

                    //Session["table_name"] = service.get_table_name(my_table_id);
                    Session["table_name"] = my_table_details.p_table_name;
                    //  Session["table_name"] = Session["username"].ToString();
                    f_goto_table(my_table_id);
                }
                else
                {
                    my_table_details = get_table_details(txt_get_fm_email_id.Text);
                    Session["password"] = "hedgeem123";
                    btnLogin_Click(btnLogin, new EventArgs());
                    //int a_table_id = service.f_get_table_id_from_db(txt_get_fm_email_id.Text);
                    int a_table_id = my_table_details.p_table_id;
                    Session["tableid"] = a_table_id;

                    Session["table_name"] = my_table_details.p_table_name;
                    //  Session["table_name"] = txt_get_fm_email_id.Text;
                    //service.f_update_last_login_date(Session["username"].ToString());//this function is used to update last login date and time of user
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
    protected void btn_cashier_Click(object sender, ImageClickEventArgs e)
    {
        //   ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>window.open('frm_cashier.aspx', 'popUpWindow', 'height=300,width=550,left=300,top=200,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes');</script>'");
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

    protected void btn_new_user_Click(object sender, EventArgs e)
    {
        _xxx_log_event.p_method_name = "btn_new_user_Click";
        _xxx_log_event.p_message = "This message creates new user in database";
        log.Debug(_xxx_log_event.ToString());
        bool user_exist;
        try
        {
            // This checks whether the entered email id exists in the database or not.
            List<HedgeEmPlayer> my_player_list = (List<HedgeEmPlayer>)f_get_object_from_json_call_to_server("ws_get_player_list/sessionabc123,10,10000/", typeof(List<HedgeEmPlayer>));
            user_exist = my_player_list.Exists(x => x.p_username == txt_username.Text);

            if (user_exist == false)
            {
                xxxHC_a_opening_balance = 100;
                try
                {
                    // creates new user in database
                    HedgeEmPlayer my_player = (HedgeEmPlayer)f_get_object_from_json_call_to_server("ws_create_hedgeem_player/" + txt_username.Text + "," + txt_password.Text + "," + txt_full_name.Text + "," + xxxHC_a_opening_balance, typeof(HedgeEmPlayer));
                    //  int player_id = service.f_create_player(txt_username.Text, txt_password.Text, txt_full_name.Text, xxxHC_a_opening_balance);
                    int player_id = my_player.p_player_id;

                    // creates table for the new user created above
                    f_create_personal_hedgeem_table_for_player(player_id, txt_username.Text);

                    // login to the game
                    Session["username"] = txt_username.Text;
                    Session["password"] = txt_password.Text;
                    Session["display_name"] = txt_full_name.Text;

                    //sends registration mail
                    send_user_registration_mail(txt_username.Text, txt_password.Text, txt_full_name.Text);

                    ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('Thank you for registering. You may now start to play. Click on the link in the mail we have just sent you to get benefit from extra features.'); if(alert){ window.location='frm_facebook_canvas.aspx';}", true);
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('Thank you for registering, you may now start to play.  Please don't forget to confirm your registration by clicking on the link in the mail we have just sent you to get your free 50 chips and benefit from extra features.'); if(alert){ window.location='frm_facebook_canvas.aspx';}", true);
                }
                catch (Exception ex)
                {
                    string my_error_popup = "alert('Error in method [btn_new_user_Click] - " + ex.Message.ToString() + "');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                    log.Error("Error while btn_new_user_Click", new Exception(ex.Message));
                }
            }

            else
            {
                lbl_register_status_msg.Visible = true;
                lbl_register_status_msg.Text = "This User Already exists. Please try again or Login to continue.";
                //ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('This User Already exists. Please try again or Login to continue.');", true);
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in creating user - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Error in creating user", new Exception(ex.Message));
        }
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        _xxx_log_event.p_method_name = "btnSend_Click";
        _xxx_log_event.p_message = "This button is clicked to send forgot password for the user.";
        log.Debug(_xxx_log_event.ToString());
        bool user_exist;
        try
        {

            // This checks whether the entered email id exists in the database or not.
            List<HedgeEmPlayer> my_player_list = (List<HedgeEmPlayer>)f_get_object_from_json_call_to_server("ws_get_player_list/sessionabc123,10,10000/", typeof(List<HedgeEmPlayer>));
            user_exist = my_player_list.Exists(x => x.p_username == txt_username.Text);

            // If user exists then proceed
            if (user_exist != false)
            {
                DataTable dt = new DataTable();
                // get password of the entered username
                dt = service.f_get_password_from_db(txt_Email.Text);
                string password = "";
                if (dt.Rows.Count > 0)
                {
                    password = dt.Rows[0]["password"].ToString();
                }

                // send email method is called with email and password of the user
                send_forgot_password_mail(txt_Email.Text, password);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('This Email doesnot exists. Please try again with valid Email Id.'); show_forgot_password();", true);

            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in btnSend_Click - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Fatal error in 'btnSend_Click'", new Exception(ex.Message));
        }
    }
    public void send_forgot_password_mail(string email_id, string password)
    {
        _xxx_log_event.p_method_name = "send_forgot_password_mail";
        _xxx_log_event.p_message = "Method to send Email of Forgotten Password";
        log.Debug(_xxx_log_event.ToString());
        try
        {
            // code to send password through mail
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential("hedgeem@gmail.com", "Plok23712");
            smtp.Port = 25;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;

            MailMessage message = new MailMessage();
            message.From = new MailAddress("hedgeem@gmail.com");
            message.To.Add(email_id);
            message.Subject = "Forgot Password Mail";
            message.Body = "As requested, your details are :- <br/><br/> Username - " + email_id + " <br/> Password - " + password + "";
            // Send mail copy to Simon
            message.CC.Add("simon.hewins@mantura.com");
            message.IsBodyHtml = true;
            smtp.Send(message);
            Page.RegisterStartupScript("UserMsg", "<script>alert('Password has been sent to you successfully. Check Your Mail for further instructions.');if(alert){ window.location='frm_facebook_canvas.aspx';}</script>");
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in send_forgot_password_mail - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Fatal error in 'send_forgot_password_mail'", new Exception(ex.Message));
        }
    }
    public void send_fb_user_registration_mail(string email_id, string password, string display_name)
    {
        _xxx_log_event.p_method_name = "send_fb_user_registration_mail";
        _xxx_log_event.p_message = "Method to send Email of User Registration";
        log.Debug(_xxx_log_event.ToString());
        try
        {
            // Send mail to user
            string encrypted_email = Encryptdata(email_id);
            string encrypted_password = Encryptdata(password);
            string encrypted_displayname = Encryptdata(display_name);

            // code to send facebook user registration mail
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential("hedgeem@gmail.com", "Plok23712");
            smtp.Port = 25;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;

            MailMessage message = new MailMessage();
            message.From = new MailAddress("hedgeem@gmail.com");
            message.To.Add(email_id);
            message.Subject = "Successfully Registered - Hedgeem Poker";
            message.Body = "Hi " + display_name + ",<br> Thank you for registering your facebook account with HedgeEm us can now log in at any time with your Facebook account alternatively you can login with you Hedgeem Player name <br> Email Id-" + email_id + " <br/> Password - " + password + "<br/> Username - " + display_name + ".";
            // Send mail copy to Simon
            message.CC.Add("simon.hewins@mantura.com");
            message.IsBodyHtml = true;
            smtp.Send(message);
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in send_fb_user_registration_mail - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Fatal error in 'send_fb_user_registration_mail'", new Exception(ex.Message));
        }
    }
    public void send_user_registration_mail(string email_id, string password, string display_name)
    {
        _xxx_log_event.p_method_name = "send_user_registration_mail";
        _xxx_log_event.p_message = "Method to send Email of User Registration";
        log.Debug(_xxx_log_event.ToString());
        try
        {
            string encrypted_email = Encryptdata(email_id);
            string encrypted_password = Encryptdata(password);
            string encrypted_displayname = Encryptdata(display_name);
            // code to send user registration mail
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential("hedgeem@gmail.com", "Plok23712");
            smtp.Port = 25;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;

            MailMessage message = new MailMessage();
            message.From = new MailAddress("hedgeem@gmail.com");
            message.To.Add(email_id);
            message.Subject = "Successfully Registered - Hedgeem Poker";
            message.Body = "Hi " + display_name + "You have been successfully registered to play Texas Hedge'Em Poker, your details are :- <br/><br/> Username - " + email_id + " <br/> Password - " + password + "<br/> Please click on the following link to activate your account :- <br/><a href='" + website_url + "?un=" + encrypted_email + "&p=" + encrypted_password + "&dn=" + encrypted_displayname + "'> " + website_url + "?un=" + encrypted_email + "&p=" + encrypted_password + "&dn=" + encrypted_displayname + "</a>";

            // Send mail copy to Simon
            message.CC.Add("simon.hewins@mantura.com");
            message.IsBodyHtml = true;
            smtp.Send(message);
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in send_user_registration_mail - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Fatal error in 'send_user_registration_mail'", new Exception(ex.Message));
        }
    }

    private string Encryptdata(string data)
    {
        string strmsg = string.Empty;
        byte[] encode = new byte[data.Length];
        encode = Encoding.UTF8.GetBytes(data);
        strmsg = Convert.ToBase64String(encode);
        return strmsg;
    }

    private string Decryptdata(string encrypt_data)
    {
        string decryptpwd = string.Empty;
        UTF8Encoding encodepwd = new UTF8Encoding();
        Decoder Decode = encodepwd.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(encrypt_data);
        int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        char[] decoded_char = new char[charCount];
        Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        decryptpwd = new String(decoded_char);
        return decryptpwd;
    }


    // Web Method to check the user is logged in from facebook for the first time

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

    protected void btn_new_fb_user_Click(object sender, EventArgs e)
    {
        _xxx_log_event.p_method_name = "btn_new_fb_user_Click";
        _xxx_log_event.p_message = "This message creates new fb user in database";
        log.Debug(_xxx_log_event.ToString());
        int user_exist;
        try
        {
            // This checks whether the entered email id exists in the database or not.
            // xxx nasty code.  esp as the function of ws_get_player_list may change in the future to NOT return all player
            List<HedgeEmPlayer> my_player_list = (List<HedgeEmPlayer>)f_get_object_from_json_call_to_server("ws_get_player_list/sessionabc123,10,10000/", typeof(List<HedgeEmPlayer>));
            bool my_does_user_exist = my_player_list.Exists(x => x.p_username == txt_fb_username.Text);

            if (my_does_user_exist == false)
            {
                xxxHC_a_opening_balance = 100;
                string xxx_default_password = "hedgeem123";
                try
                {
                    // creates new user in database
                    HedgeEmPlayer my_player = (HedgeEmPlayer)f_get_object_from_json_call_to_server("ws_create_hedgeem_player/" + txt_fb_username.Text + "," + xxx_default_password + "," + txt_fb_display_name.Text + "," + xxxHC_a_opening_balance, typeof(HedgeEmPlayer));
                    int player_id = my_player.p_player_id;
                    //int player_id = service.f_create_player(txt_fb_username.Text, xxx_default_password, txt_fb_display_name.Text, xxxHC_a_opening_balance);
                    service.f_activate_user(txt_fb_username.Text);
                    // login to the game
                    Session["username"] = txt_username.Text;
                    Session["password"] = txt_password.Text;
                    Session["display_name"] = txt_full_name.Text;
                    // creates table for the new user created above
                    f_create_personal_hedgeem_table_for_player(player_id, txt_fb_username.Text);
                    send_fb_user_registration_mail(txt_fb_username.Text, xxx_default_password, txt_fb_display_name.Text);

                    //lbl_register_status_msg.Visible = true;
                    //lbl_register_status_msg.Text = "Thank you for registering, Please activate your account by clicking link sent on your registered email id.";
                    ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('Thank you for registering,your facebook account with HedgeEm. A confirmation mail is sent to your email Id '); if(alert){ window.location='frm_facebook_canvas.aspx';}", true);

                    //Page.RegisterStartupScript("OnLoad", "<script>alert('Thank you for registering, you are now logged in an can start to play HedgeEm'); if(alert){ window.location='frm_facebook_canvas.aspx';}</script>");
                }
                catch (Exception ex)
                {
                    string my_error_popup = "alert('Error in btn_new_fb_user_Click - " + ex.Message.ToString() + "');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                    log.Error("Error while btn_new_fb_user_Click", new Exception(ex.Message));
                }
            }
            else
            {
                lbl_register_fb_status_msg.Visible = true;
                lbl_register_fb_status_msg.Text = "This User Already exists. Please try again or Login to continue.";
                //ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('This User Already exists. Please try again or Login to continue.');", true);
                // Page.RegisterStartupScript("OnLoad", "<script>alert('This User Already exists. Please try again or Login to continue.');</script>");
            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in creating user - " + ex.Message.ToString() + "');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Error in creating user", new Exception(ex.Message));
        }
    }
}