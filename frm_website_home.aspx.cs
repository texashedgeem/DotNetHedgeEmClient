﻿using System;
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

public partial class frm_website_home : System.Web.UI.Page
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
    int xxxHC_ANON_PLAYER_ID = 100;
    string xxxHC_ANON_USERNAME = "admin@mantura.com";

    
    static string ConvertStringArrayToStringJoin(string[] array)
    {
        //
        // Use string Join to concatenate the string elements.
        //
        string result = string.Join(",", array);
        return result;
    }

    private static string p_current_json_webservice_url_base
    {
        get
        {
            return WebConfigurationManager.AppSettings["hedgeem_server_default_webservice_url"].ToString();
        }
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {
        //this.Page.Theme = "ONLINE";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        btn_play_now.Enabled = false;
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method called.";
        log.Debug(my_log_event.ToString());

        p_session_current_table_id = 1000;
        p_session_server_id = 10;

        try
        {

            string strUserAgent = Request.UserAgent.ToString().ToLower();
            bool MobileDevice = Request.Browser.IsMobileDevice;
            if (Request.Cookies["MobileDevice"] != null)
            {
                if (Request.Cookies["MobileDevice"].Value == "IgnoreMobile") { MobileDevice = false; }
            }
            else
            {
                if (strUserAgent != null)
                {
                    if (MobileDevice == true || strUserAgent.Contains("iphone") || strUserAgent.Contains("blackberry") || strUserAgent.Contains("mobile") ||
                    strUserAgent.Contains("android") || strUserAgent.Contains("windows ce") || strUserAgent.Contains("opera mini") || strUserAgent.Contains("palm"))
                    {
                        Response.Redirect("https://itunes.apple.com/gb/app/texas-hedgeem/id1018941577?mt=8");
                    }
                }
            }


            //Read more: http://www.thecodingguys.net/blog/asp-net-mobile-detection#ixzz3qoYVKpHs

            if (Request.QueryString["un"] != null && Request.QueryString["p"] != null)
            {
                string retrieved_username = Decryptdata(Request.QueryString["un"].ToString());
                string retrieved_password = Decryptdata(Request.QueryString["p"].ToString());
                string retrieved_displayname = Decryptdata(Request.QueryString["dn"].ToString());
                Session["p_session_username"] = retrieved_username;
                Session["password"] = retrieved_password;
                Session["display_name"] = retrieved_displayname;
                //  service.f_activate_user(retrieved_username);
                //    my_log_event.p_message = String.Format("Username [{0}] activated. ", retrieved_username);
                my_log_event.p_message = String.Format("User [{0}] clicked on link activation. ", retrieved_username);
                ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('Thanks for verifying your account. You can now play the game.'); if(alert){ window.location='frm_website_home.aspx';}", true);
                log.Debug(my_log_event.ToString());

            }
            if (Page.IsPostBack == false)
            {
                if (Request.QueryString["signout"] != "true")
                {
                    Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('element').style.display='block';</script>");
                    Page.RegisterStartupScript("OnLoad2", "<script> $('#element').introLoader({animation: { name: 'counterLoader', options: { exitFx: 'slideRight',ease: 'easeOutSine',style: 'fluoYellow', delayBefore: 1000, exitTime: 500, animationTime: 1000} } });</script>");

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
            }
            else
            {
                //Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('progressbar').style.display='none';</script>");
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
                    Response.Redirect("frm_website_home.aspx");
                }
                if (Session["user_role"] != "ADMIN")
                {
                    btnAdmin.Attributes.Add("style", "display:none");
                    //Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('btnAdmin').style.display='none';</script>");
                }
                if (Session["p_session_username"] != null)
                {

                    Logout.Attributes.Add("style", "display:block!Important;");
                    //Page.RegisterStartupScript("OnLoading", "<script>load_edit_profile();</script>");
                    if (Session["display_name"] != null)
                    {
                        lbl_user_name.Text = Session["display_name"].ToString();
                    }
                    //LoginDiv.Attributes.Add("style", "display:none !Important;");
                    divLogin.Attributes.Add("style", "display:none !Important;");
                    if (Session["p_session_username"] != null)
                    {
                        if (File.Exists(Server.MapPath("resources/player_avatar_" + Session["p_session_username"].ToString() + ".jpg")))
                        {
                            usr_image.ImageUrl = "../resources/player_avatar_" + Session["p_session_username"].ToString() + ".jpg";
                        }
                        else
                        {
                            usr_image.ImageUrl = "../resources/avitars/user_square.png";
                        }
                    }

                    btn_play_now.Enabled = true;

                    HedgeEmPlayer my_player = get_player_details(Session["p_session_username"].ToString());

                   
                    string role = "";
                    if (my_player != null)
                    {
                        role = my_player.p_role;

                        //my_log_event.p_message = String.Format("User/player role hardcoded to BASIC_USER");
                        //log.Debug(my_log_event.ToString());
                        my_log_event.p_message = String.Format("User/player role determined to be [{0}].", role.ToString());
                        log.Debug(my_log_event.ToString());
                    }

                    if (role == enum_user_role.ADMIN.ToString())
                    {
                        btnAdmin.Attributes.Add("style", "display:block");
                      //  Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('btnAdmin').style.display='block';</script>");
                    }
                    else
                    {
                        btnAdmin.Attributes.Add("style", "display:none");
                      //  Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('btnAdmin').style.display='none';</script>");
                    }
                    Session["user_role"] = role;
                    //// - xxx- Hardcoding value of role to BASIC_USER as need to discuss this with Simon.
                    //Session["user_role"] = "BASIC_USER";
                    Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('progressbar').style.display='none';</script>");
                }


                else
                {

                }

                //Page.RegisterStartupScript("New_User_Popup", "<script type='text/javascript'>function newPopup(url) {popupWindow = window.open(url, 'popUpWindow', 'height=220,width=1250,left=50,top=200,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes')}</script>");
                //Page.RegisterStartupScript("Update_Deposit_Amount", "<script type='text/javascript'>function depositupdatePopup(url) {popupWindow = window.open(url, 'popUpWindow', 'height=200,width=450,left=300,top=200,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes')}</script>");


                if (Session["p_session_username"] != null || Session["password"] != null)
                {
                    if (Convert.ToInt32(Session["p_session_player_id"]) != 10001)
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
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;
            string my_error_popup = "Error in Page Load of frm_website_home - " + ex.Message.ToString();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup.ToString();
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
            my_log_event.p_message = "Exception caught in Page Load of frm_website_home- " + ex.Message;
            log.Error(my_log_event.ToString(), new Exception(ex.Message));
        }
    }





    // Method to get details of Player from JSON Web Service
    public HedgeEmPlayer get_player_details(string username)
    {
        HedgeEmPlayer my_player = null;
        log.Debug("get_player_details called for getting details of user - " + username);
        string my_endpoint = "Not Set";

        try
        {
            my_endpoint = String.Format("{0}/ws_get_player_list/sessionabc123,10,10000/", p_current_json_webservice_url_base);

            List<HedgeEmPlayer> my_player_list = (List<HedgeEmPlayer>)f_get_object_from_json_call_to_server(my_endpoint, typeof(List<HedgeEmPlayer>));
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

            //string my_error_popup = "alert('Error in method [get_player_details] - " + ex.Message.ToString() + "');";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;
            string my_error_popup = "Error in method [get_player_details] - " + ex.Message.ToString();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup.ToString();
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);

            //hedgeem_control_popup_message my_popup_message = new hedgeem_control_popup_message();
            //my_popup_message.p_detailed_message_str = "";
            //my_popup_message.p_detailed_message_str = ex.Message;
            //my_popup_message.p_is_visible = true;

            ////newuser.Visible = false;

            //Place_Holder_Popup_Message.Controls.Add(my_popup_message);

            //my_popup_message.Dispose();
            
        }
        return my_player;
    }

    // Method to get details of Table from JSON Web Service
    public HedgeEmTableSummary get_table_details(string username)
    {
        HedgeEmTableSummary my_table = null;
        log.Debug("get_table_details called for getting details of table for user - " + username);
        string my_endpoint = "Not Set";
        try
        {
            string my_table_name = username;
            my_endpoint = String.Format("{0}/get_server_info/10/", p_current_json_webservice_url_base);

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
            //string my_error_popup = "alert('Error in method [get_table_details] - " + ex.Message.ToString() + "');";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;
            string my_error_popup = "Error in method [get_table_details]- " + ex.Message.ToString();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup.ToString();
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);

            log.Error("Exception in get_table_details() function, - " + ex.Message);
        }
        return my_table;
    }

    protected void btn_login_Click(object sender, EventArgs e)
    {

       
       

        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        // Log the fact this method has been called
        my_log_event.p_message = String.Format("Method [btn_login_Click] called ");
        log.Debug(my_log_event.ToString());
        string my_endpoint = "Not Set";
        // Detemine if there is a session varible set for facebook. 
        // xxx Nov 2014 Note ... Not sure why - I assmume this is to check
        // if the user has already authenticated via facebook so we can make choices about what to display
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

        // Declare vairibles that will be required later
        string my_username = "";
        string my_password = "";
        DataTable my_user_details;

        // If the user has entered a email/userid into the text box use this to set 'my_username' varible (for later use)
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
            // Attempt to retrieve a Player object from the HedgeEm Webservice using the credentials supplied.
            my_endpoint = String.Format("{0}/ws_login/{1},{2},{3},{4}/", p_current_json_webservice_url_base, p_session_id, p_session_server_id, my_username, my_password);
            HedgeEmPlayer my_player = (HedgeEmPlayer)f_get_object_from_json_call_to_server(my_endpoint, typeof(HedgeEmPlayer));

            if (my_player == null)
            {
                string my_error_msg = String.Format("Player object is null after call to Webservice to retrieve details", my_username);
                my_log_event.p_message = my_error_msg;
                log.Error(my_log_event.ToString());
                throw new Exception(my_error_msg);
            }

            if (my_player.p_player_id <= 0)
            {
                my_log_event.p_message = my_player.p_error_message;
                log.Error(my_log_event.ToString());
                throw new Exception(my_log_event.ToString());

            }
            else
            {
                // If we have got here the user has logged into the server sucessfully so we should set some session variables for ease of use later
                int my_player_id = my_player.p_player_id;
                Session["p_session_player_id"] = my_player_id;
                Session["display_name"] = my_player.p_display_name;
                Session["p_session_username"] = my_username;
                Session["password"] = my_password;
                p_session_personal_table_id = my_player.p_personal_table_id;
                if (Session["p_session_username"] != null)
                {
                    if (File.Exists(Server.MapPath("resources/player_avatar_" + Session["p_session_username"].ToString() + ".jpg")))
                    {
                        usr_image.ImageUrl = "../resources/player_avatar_" + Session["p_session_username"].ToString() + ".jpg";
                    }

                    else
                    {
                        usr_image.ImageUrl = "../resources/avitars/user_square.png";
                    }
                }
                // As the user has now logged in by HedgeEm authentication hide the Facebook login DIV
                if (Session["facebooklogin"] != null)
                {
                    //LoginDiv.Attributes.Add("style", "display:none !Important;");
                }

                // As the user has now logged show the logout button and hide the login button
                // xxx Nov 2014.  Not sure why this is on condition of facebooklogin; I separated this out from if/else
                // of above to figure this out.  I assume is because you could be logged in by facebook so still want to
                // show / hide login/logout buttons appropriately
                if (Session["facebooklogin"] == null)
                {
                    lbl_user_name.Text = Session["display_name"].ToString();
                    Logout.Attributes.Add("style", "display:block !Important;");
                    //LoginDiv.Attributes.Add("style", "display:none !Important;");
                    divLogin.Attributes.Add("style", "display:none !Important;");
                    userdetails.Attributes.Add("style", "display:none !Important;");
                    btnLogout.Visible = true;
                    btnLogout.Enabled = true;
                    Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('userdetails').style.display='none';</script>");
                }

                // As the user has now logged in Enable the 'Play Now' button
                btn_play_now.Enabled = true;

               // usr_image.ImageUrl = my_player.p_user_avitar_image_url;

                // If the 'fb_hide_logout' is set it implies xxxxxxxxxxxxxxxxxxxxx so enable the 'Play Now' button
                if (Session["fb_hide_logout"] != null)
                {
                    //    chk_fb_visibility_logout.Visible = false;
                    btn_play_now.Visible = true;

                }

                // If the 'chk_logout_hide' is set it implies xxxxxxxxxxxxxxxxxxxxx so enable the 'Play Now' button
                if (Session["chk_logout_hide"] != null)
                {

                    //   chk_fb_visibility.Visible = true;
                    btn_play_now.Visible = true;
                    btnLogout.Visible = false;
                }
                Page.RegisterStartupScript("OnLoading", "<script>load_edit_profile();</script>");
                // Response.Redirect("frm_website_home.aspx");
                Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('progressbar').style.display='none';</script>");

                string role = "";
                if (my_player != null)
                {
                    role = my_player.p_role;
                    my_log_event.p_message = String.Format("User/player role determined to be [{0}].", role.ToString());
                    log.Debug(my_log_event.ToString());
                }

                if (role == enum_user_role.ADMIN.ToString())
                {
                    btnAdmin.Attributes.Add("style", "display:block");
                }
                else
                {
                    btnAdmin.Attributes.Add("style", "display:none");
                }
            }
        }
        catch (Exception ex)
        {
            string my_error_message = "Fatal Errorin method [btn_login_Click] - " + ex.Message.ToString() + "');";

            //  Log the error and raise a new exception
            my_log_event.p_message = String.Format(
                "Fatal error in method [btn_login_Click]. Reason [{0}]",
                        ex.Message);
            log.Error(my_log_event.ToString(), new Exception(ex.Message));
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;
        
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_message.ToString();
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
        }
       
    }


    protected void f_login_anonymously(int a_player_id)
    {




        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        // Log the fact this method has been called
        my_log_event.p_message = String.Format("Method [btn_login_Click] called ");
        log.Debug(my_log_event.ToString());
        string my_endpoint = "Not Set";
        
        // Declare vairibles that will be required later
        string my_username = "";
        string my_password = "";
        DataTable my_user_details;

        
        try
        {
            // Attempt to retrieve a Player object from the HedgeEm Webservice using the credentials supplied.
            my_endpoint = String.Format("{0}/ws_login/{1},{2},{3},{4}/", p_current_json_webservice_url_base, p_session_id, p_session_server_id, my_username, my_password);
            HedgeEmPlayer my_player = (HedgeEmPlayer)f_get_object_from_json_call_to_server(my_endpoint, typeof(HedgeEmPlayer));

            if (my_player == null)
            {
                string my_error_msg = String.Format("Player object is null after call to Webservice to retrieve details", my_username);
                my_log_event.p_message = my_error_msg;
                log.Error(my_log_event.ToString());
                throw new Exception(my_error_msg);
            }

            if (my_player.p_player_id <= 0)
            {
                my_log_event.p_message = my_player.p_error_message;
                log.Error(my_log_event.ToString());
                throw new Exception(my_log_event.ToString());

            }
            else
            {
                // If we have got here the user has logged into the server sucessfully so we should set some session variables for ease of use later
                int my_player_id = my_player.p_player_id;
                Session["p_session_player_id"] = my_player_id;
                Session["display_name"] = my_player.p_display_name;
                Session["p_session_username"] = my_username;
                Session["password"] = my_password;
                Session["user_role"] = my_player.p_role;
                        
                p_session_personal_table_id = my_player.p_personal_table_id;
                if (Session["p_session_username"] != null)
                {
                    if (File.Exists(Server.MapPath("resources/player_avatar_" + Session["p_session_username"].ToString() + ".jpg")))
                    {
                        usr_image.ImageUrl = "../resources/player_avatar_" + Session["p_session_username"].ToString() + ".jpg";
                    }

                    else
                    {
                        usr_image.ImageUrl = "../resources/avitars/user_square.png";
                    }
                }
                // As the user has now logged in by HedgeEm authentication hide the Facebook login DIV
                if (Session["facebooklogin"] != null)
                {
                    //LoginDiv.Attributes.Add("style", "display:none !Important;");
                }

                // As the user has now logged show the logout button and hide the login button
                // xxx Nov 2014.  Not sure why this is on condition of facebooklogin; I separated this out from if/else
                // of above to figure this out.  I assume is because you could be logged in by facebook so still want to
                // show / hide login/logout buttons appropriately
                if (Session["facebooklogin"] == null)
                {
                    lbl_user_name.Text = Session["display_name"].ToString();
                    Logout.Attributes.Add("style", "display:block !Important;");
                    //LoginDiv.Attributes.Add("style", "display:none !Important;");
                    divLogin.Attributes.Add("style", "display:none !Important;");
                    userdetails.Attributes.Add("style", "display:none !Important;");
                    btnLogout.Visible = true;
                    btnLogout.Enabled = true;
                    Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('userdetails').style.display='none';</script>");
                }

                
                string role = "";
                if (my_player != null)
                {
                    role = my_player.p_role;
                    my_log_event.p_message = String.Format("User/player role determined to be [{0}].", role.ToString());
                    log.Debug(my_log_event.ToString());
                }

                
            }
        }
        catch (Exception ex)
        {
            string my_error_message = "Fatal Errorin method [btn_login_Click] - " + ex.Message.ToString() + "');";

            //  Log the error and raise a new exception
            my_log_event.p_message = String.Format(
                "Fatal error in method [btn_login_Click]. Reason [{0}]",
                        ex.Message);
            log.Error(my_log_event.ToString(), new Exception(ex.Message));
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_message.ToString();
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
        }

    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        _xxx_log_event.p_method_name = "btnLogout_Click";
        string my_endpoint = "Not Set";
        // Log the fact this method has been called
        _xxx_log_event.p_message = String.Format("Method [btnLogout_Click] called ");
        log.Info(_xxx_log_event.ToString());
        try
        {
            my_endpoint = String.Format("{0}/ws_logout/sessionabc123,10,{1}/", p_current_json_webservice_url_base, p_session_username);

            HedgeEmPlayer my_player = (HedgeEmPlayer)f_get_object_from_json_call_to_server(my_endpoint, typeof(HedgeEmPlayer));
            btnLogout.Visible = false;
            //btnLogin.Visible = true;
            Session.Abandon();
            Session.Contents.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Logout.Attributes.Add("style", "display:none !Important;");

            divLogin.Attributes.Add("style", "display:block !Important;");
            usr_image.ImageUrl = "../resources/avitars/user_square.png";
            btn_play_now.Enabled = false;
            Response.Redirect("frm_website_home.aspx?signout=true");
        }
        catch (Exception ex)
        {
            string my_error_popup = "Error in method [btnLogout_Click] - " + ex.Message.ToString() ;
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);

            //  Log the error and raise a new exception
            _xxx_log_event.p_message = String.Format(
                "Table [Fatal error in method [btnLogout_Click]. Reason [{0}]",
                        ex.Message);
            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_is_visible = false;

            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup.ToString();
            my_popup_message.p_is_visible = true;
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
            log.Error(_xxx_log_event.ToString(), new Exception(ex.Message));
         //   throw new Exception(_xxx_log_event.ToString());
        }
       

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="endpoint">Full path to JSON endpoint including base URL.  Example: http://devserver.hedgeem.com/Service1.svc/get_server_info/10/</param>
    /// <param name="typeIn"></param>
    /// <returns></returns>
    public static object f_get_object_from_json_call_to_server(string endpoint, Type typeIn)
    {
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_message = String.Format("Method called with endpoint [{0}]", endpoint); ;
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        object obj = null;

        //my_log.p_player_id = playerid;
        try
        {
            log.Debug(my_log_event.ToString());
            HttpWebRequest request;
            String my_service_url = "not set";

            if (endpoint == "")
            {
                my_log_event.p_message = "endpoint not set";
                throw new Exception(my_log_event.ToString());
            }

            //my_service_url = WebConfigurationManager.AppSettings["hedgeem_server_default_webservice_url"];
            my_log_event.p_message = String.Format("Webservice configured for GoDaddy Deployment URI = [{0}]", endpoint); ;



            log.Debug(my_log_event.ToString());
            //request = WebRequest.Create(my_service_url + endpoint) as HttpWebRequest;
            request = WebRequest.Create(endpoint) as HttpWebRequest;


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
                                    my_log_event.p_message = my_error_msg;
                                    log.Error(my_log_event.ToString());
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
        catch (System.Net.WebException webex)
        {
            HttpWebResponse my_error_response = webex.Response as HttpWebResponse;
            switch (my_error_response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    my_log_event.p_message = String.Format(
                       "Error (Web Exception: BadRequest). Endpoint URI probably badly formed ...\nEndpoint [{0}], Status Code [{1}]",
                              endpoint,
                               my_error_response.StatusCode
                              );
                    break;

                case HttpStatusCode.NotFound:
                    my_log_event.p_message = String.Format(
                       "Error (Web Exception: NotFound). Endpoint URI not found; Endpoint [{0}], Status Code [{1}]",
                              endpoint,
                               my_error_response.StatusCode
                              );
                    break;


                case HttpStatusCode.InternalServerError:
                    my_log_event.p_message = String.Format(
                       "Error (Web Exception: InternalServerError). It looks like the HedgeEm Webservice is down, try pasting this Endpoint into a browser to see if this is the case; Endpoint [{0}], Status Code [{1}]",
                              endpoint,
                               my_error_response.StatusCode
                              );
                    break;

                default: my_log_event.p_message = String.Format(
                       "Error unexpected status code (Web Exception of type Status Code [{0}] for Endpoint [{1}], ",
                              my_error_response.StatusCode,
                              endpoint
                              );
                    break;
            }


            throw new Exception(my_log_event.ToString());
            //return new WebException(my_log_event.ToString(), webex);
        }
        catch (Exception ex)
        {
            // Log the error and raise a new exception
            if (ex.InnerException != null)
            {
                my_log_event.p_message = String.Format(
                    "Error. Details [{0}], Inner Exception [{1}]",
                            ex.Message, ex.InnerException.Message
                           );
            }
            else
            {
                my_log_event.p_message = String.Format(
               "Error. Details [{0}]", ex.Message);

            }
            log.Error(my_log_event.ToString());
            throw new Exception(my_log_event.ToString());
            //return new Exception(my_log_event.ToString());
        }
        
    }


   
    private void f_goto_table(int a_table_id)
    {
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "Method Entered";
        my_log_event.p_server_id = 4663662;
        my_log_event.p_table_id = a_table_id;
        my_log_event.p_player_id = p_session_player_id;

        log.Debug(my_log_event.ToString());

        /* Get reference to table by this table id
         For this GUI to work we need a HedgeEMTable object that it will obtain its data from (e.g. cards to display,
         odds for each hand etc).  So create a place-holder object that we will set later. */
        string my_endpoint = "Not Set";


        try
        {
            if (p_valid_session_exists)
            {

                HedgeemThemeInfo my_hedgeem_theme_info = new HedgeemThemeInfo();
                bool xxxHC_my_get_theme_info_from_server_definition = false;
                string theme_name = enum_theme.ONLINE.ToString();

                if (xxxHC_my_get_theme_info_from_server_definition){
                    my_endpoint = String.Format("{0}/ws_get_theme_details_for_hedgeem_table/{1}/", p_current_json_webservice_url_base, a_table_id);
                    my_hedgeem_theme_info = (HedgeemThemeInfo)f_get_object_from_json_call_to_server(my_endpoint, typeof(HedgeemThemeInfo));

                    if (my_hedgeem_theme_info == null)
                    {
                        throw new Exception(String.Format("Unable to determine theme for table [{0}]", a_table_id));
                    }

                    if (my_hedgeem_theme_info._error_message != "")
                    {
                        throw new Exception(my_hedgeem_theme_info._error_message);
                    }
                    theme_name = my_hedgeem_theme_info.short_name;
                    Session["theme"] = theme_name;
                }


                // xxx Note from Simon Jan 2015 - not sure what this next line does.  If it is just a pop up it can be deleted (esp as it does not work anyway)
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", "alert('" + "Theme name -" + Session["theme"].ToString() + "');", true);

                my_log_event.p_message = String.Format("Successfully retrieved theme for table ID [{0}] Theme [{1}] so redirecting to (opening) HedgeEmTable page so ASP can render page to this theme.", a_table_id, theme_name);
                log.Debug(my_log_event.ToString());

                Response.Redirect("frm_hedgeem_table.aspx", false);
            }
            else
            {
                Page.RegisterStartupScript("OnLoad", "<script>alert('You need to be logged in to sit at this table');</script>");
            }
        }
        catch (Exception ex)
        {
            // xxx_eh error not show to user
            string my_error_popup = "alert('Error in method [f_goto_table] - " + ex.Message.ToString() + "');";
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_log_event.p_message = String.Format("Error", ex.Message);
            log.Error(my_log_event.ToString());
            //throw new Exception(my_log_event.ToString());               


            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
              my_popup_message.p_detailed_message_str = my_error_popup;
            my_popup_message.p_is_visible = true;
            
            //newuser.Visible = false;
            
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
        
            my_popup_message.Dispose();

        }
    }

    private string p_session_username
    {
        get
        {
            if (Session["p_session_username"] == null)
            {
                return "Anonymous";
            }

            if (Session["p_session_username"] == "")
            {
                return "Anonymous";
            }

            return Session["p_session_username"].ToString();
        }

        set { Session["p_session_username"] = value; }
    }


    private int p_session_current_table_id
    {
        get
        {
            int my_table_id = -1;
            try
            {
                my_table_id = Convert.ToInt32(Session["p_session_current_table_id"]);
            }
            catch (Exception e)
            {
                my_table_id = -1;
            }

            return my_table_id;
        }

        set { Session["p_session_current_table_id"] = value; }
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

    private static string p_session_id
    {
        get { return HttpContext.Current.Session.SessionID; }
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

        set { Session["p_session_player_id"] = value; }
    }


    private int p_session_server_id
    {
        get
        {
            int my_session_server_id = -1;
            try
            {
                my_session_server_id = Convert.ToInt32(Session["p_session_server_id"]);
            }
            catch (Exception e)
            {
                my_session_server_id = -1;
            }

            return my_session_server_id;
        }

        set { Session["p_session_server_id"] = value; }
    }

    /// <summary>
    /// There are a number of key session variables that need to be set after login that enbles full gameplay as listed below.
    /// If all of these are set then 'p_valid_session_exists' returns as TRUE.
    /// </summary>
    private bool p_valid_session_exists
    {

        get
        {
            if (Session["p_session_username"] == null)
            {
                return false;
            }

            if (Session["p_session_username"] == "")
            {
                return false;
            }

            if (Session["p_session_personal_table_id"] == "")
            {
                return false;
            }

            if (Session["p_session_current_table_id"] == "")
            {
                return false;
            }

            if (Session["p_session_player_id"] == "")
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// WARNING OF DUPLICACTE CODE - See btn_anon_casino_Click, btn_anon_online_Click, and btn_anon_retro_Click
    /// only difference is theme selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_anon_retro_Click(object sender, EventArgs e)
    {

        // Log that the user has clicked the 'Play now' button.
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_table_id = p_session_personal_table_id;
        my_log_event.p_server_id = p_session_server_id;
        my_log_event.p_message = String.Format("User [{0}] clicked 'Play RETRO Anonymously", p_session_username);
        log.Info(my_log_event.ToString());
        Session["theme"] = enum_theme.RETRO.ToString();

        try
        {
            f_play_anon(enum_theme.RETRO);
        }
        catch (Exception ex)
        {
            my_log_event.p_message = String.Format("Error user try to play Anonymously using RETRO Theme.  Reason [{0}]", ex.Message);
            log.Error(my_log_event.ToString());
            // Cant throw a exception here
            //throw new Exception(my_log_event.ToString());

        }
    }

    /// <summary>
    /// WARNING OF DUPLICACTE CODE - See btn_anon_casino_Click, btn_anon_online_Click, and btn_anon_retro_Click
    /// only difference is theme selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_anon_online_Click(object sender, EventArgs e)
    {
        // Log that the user has clicked the 'Play now' button.
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_table_id = p_session_personal_table_id;
        my_log_event.p_server_id = p_session_server_id;
        my_log_event.p_message = String.Format("User [{0}] clicked 'Play ONLINE Anonymously", p_session_username);
        log.Info(my_log_event.ToString());

        try
        {
            f_play_anon(enum_theme.ONLINE);
        }
        catch (Exception ex)
        {
            my_log_event.p_message = String.Format("Error user try to play Anonymously using ONLINE Theme.  Reason [{0}]", ex.Message);
            log.Error(my_log_event.ToString());
            // Cant throw a exception here
            //throw new Exception(my_log_event.ToString());

        }
    }

    /// <summary>
    /// WARNING OF DUPLICACTE CODE - See btn_anon_casino_Click, btn_anon_online_Click, and btn_anon_retro_Click
    /// only difference is theme selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_anon_casino_Click(object sender, EventArgs e)
    {
        // Log that the user has clicked the 'Play now' button.
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_table_id = p_session_personal_table_id;
        my_log_event.p_server_id = p_session_server_id;
        my_log_event.p_message = String.Format("User [{0}] clicked 'Play Casino Anonymously", p_session_username);
        log.Info(my_log_event.ToString());

        try
        {
            f_play_anon(enum_theme.CASINO);
        }
        catch (Exception ex)
        {
            my_log_event.p_message = String.Format("Error user try to play Anonymously using CASINO Theme.  Reason [{0}]", ex.Message);
            log.Error(my_log_event.ToString());
            // Cant throw a exception here
            //throw new Exception(my_log_event.ToString());
            
        }
    }


    protected void f_play_anon(enum_theme a_enum_theme)
    {

        // Log that the user has clicked the 'Play now' button.
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_table_id = p_session_personal_table_id;
        my_log_event.p_server_id = p_session_server_id;
        my_log_event.p_message = String.Format("User [{0}] click 'Play Now", p_session_username);
        log.Info(my_log_event.ToString());

        // This fucntion should not be able to be played if the player as not logged in, so test if they have 
        // a 'HedgeEm Session' has been established,  If not exit this fucntion. 
        /*if (!p_valid_session_exists)
        {
            my_log_event.p_message = String.Format("User informed the session is invalide.");
            log.Debug(my_log_event.ToString());

            //ScriptManager.RegisterStartupScript(Page, GetType(), "Alert Message", "alert('You must be logged in to Play this game.');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "Alert Message", "document.getElementById('alertmessage').style.display = 'block';", true);
            return;
        }*/


        // xxx Consider uncommenting the following after aslo considering consequence (if you do this when user clicks leave table 
        // they will leave there personal table which you may not want currently as the code currently (Dec 2014) assumes you are sitting at
        // personal table all the time.
        HedgeEmPlayer my_hedgeem_player;
        //my_hedgeem_player = f_get_free_anonymous_player();
        HedgeEmGameState my_game_state = f_sit_at_anonymous_table(a_enum_theme);
        //f_login_anonymously(1201);
        if (my_game_state.p_error_message != null)
        {
            if (my_game_state.p_error_message != "")
            {
                throw new Exception(my_game_state.p_error_message);
            }
        }


        Session["p_session_username"] = my_game_state._seats[0].p_player_name;
        Session["p_session_personal_table_id"] = my_game_state.p_table_id;
        Session["p_session_current_table_id"] = my_game_state.p_table_id;
        Session["p_session_player_id"] = my_game_state._seats[0].p_player_id;
        Session["theme"] = a_enum_theme.ToString();
        Session["role"] = "BASIC_USER";

            f_goto_table(my_game_state.p_table_id);

        // If code reaches here we asssume Table ID and PlayerID is known so sit the person at this table


    }
    

    protected void btn_play_now_Click(object sender, EventArgs e)
    {

        // Log that the user has clicked the 'Play now' button.
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_table_id = p_session_personal_table_id;
        my_log_event.p_server_id = p_session_server_id;
        my_log_event.p_message = String.Format("User [{0}] click 'Play Now", p_session_username);
        log.Info(my_log_event.ToString());

        // This fucntion should not be able to be played if the player as not logged in, so test if they have 
        // a 'HedgeEm Session' has been established,  If not exit this fucntion. 
        if (!p_valid_session_exists)
        {
            my_log_event.p_message = String.Format("User informed the must be logged in to play.");
            log.Debug(my_log_event.ToString());

            //ScriptManager.RegisterStartupScript(Page, GetType(), "Alert Message", "alert('You must be logged in to Play this game.');", true);
            ScriptManager.RegisterStartupScript(Page, GetType(), "Alert Message", "document.getElementById('alertmessage').style.display = 'block';", true);
            return;
        }


        // xxx Consider uncommenting the following after aslo considering consequence (if you do this when user clicks leave table 
        // they will leave there personal table which you may not want currently as the code currently (Dec 2014) assumes you are sitting at
        // personal table all the time.
        p_session_current_table_id = p_session_personal_table_id;

        f_goto_table(p_session_personal_table_id);

        // If code reaches here we asssume Table ID and PlayerID is known so sit the person at this table


    }
    /*
    protected void btn_play_now_DEPRICATED_Click(object sender, EventArgs e)
    {

        try
        {
            // Log that the user has clicked the 'Play now' button.
            HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
            my_log_event.p_method_name = "frm_website_home.btn_play_now_Click";
            my_log_event.p_message = String.Format("User [{0}] click 'Play Now", p_session_username);
            log.Info(my_log_event.ToString());

            HedgeEmPlayer my_player = null;
            HedgeEmTableSummary my_table_details = null;

            
            if (txt_get_fm_email_id.Text == "" || p_valid_session_exists)
            {
                my_log_event.p_message = String.Format("User informed the must be logged in to play.");
                log.Debug(my_log_event.ToString());

                ScriptManager.RegisterStartupScript(Page, GetType(), "Alert Message", "alert('You must be logged in to Play this game.');", true);
            }
            else if (txt_get_fm_email_id.Text != null && p_valid_session_exists)
            {
                my_log_event.p_message = String.Format("Attempting to get player detail from webservice using text from txt_get_fm_email_id hidden field. ");
                log.Debug(my_log_event.ToString());

                // int user_exist = service.f_get_player_details_by_username(txt_get_fm_email_id.Text);
                my_player = get_player_details(txt_get_fm_email_id.Text);
                if (my_player != null)
                {
                    p_session_username = txt_get_fm_email_id.Text;
                    my_log_event.p_message = String.Format("Retrieved player detail from webservice and used this to create session information. [{0}]. ", p_session_username);
                    log.Debug(my_log_event.ToString());
                }
                else
                {
                    my_log_event.p_message = String.Format("User informed the must register to play.");
                    log.Debug(my_log_event.ToString());
                    Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>alert('You must register to play the game.');</script>");
                }
            }

            if (p_valid_session_exists)
            {
                // this will check if the user logged in via facebook exists in our db or not. If yes then will check for the table or else will create.
                Session["Facebook_User_Id"] = fb_user_id.Value;
                String password = "";
                //DataTable dt = new DataTable();

                if (txt_get_fm_email_id.Text == "")
                {
                    my_log_event.p_message = String.Format("txt_get_fm_email_id hidden field is null so try to get info from service.");
                    log.Debug(my_log_event.ToString());
                    //dt = service.f_get_password_from_db(Session["p_session_username"].ToString());
                    my_player = get_player_details(p_session_username);
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

                    {
                        
                        if (txt_get_fm_email_id.Text == "")
                        {
                            my_table_details = get_table_details(Session["p_session_username"].ToString());
                        }
                        else
                        {
                            my_table_details = get_table_details(txt_get_fm_email_id.Text);
                        }
    
                        if (my_table_details == null)
                        {

                            playerid = my_player.p_player_id;

                            if (txt_get_fm_email_id.Text == "")
                            {
                                my_log_event.p_message = String.Format("This player does not have a HedgeEm table so create one.", p_session_username);

                                f_create_personal_hedgeem_table_for_player(playerid, p_session_username);
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
                            playerid = my_player.p_player_id;
                            Session["p_session_player_id"] = playerid;
                        }
                    }
                }
               
                if (txt_get_fm_email_id.Text == "")
                {
                    string my_username = p_session_username;
                    my_table_details = get_table_details(my_username);
                    
                    int my_table_id = my_table_details.p_table_id;

                    if (my_table_id <= 0)
                    {
                        string my_error_msg = String.Format("Invalid table ID [{0}]", my_table_id);
                        log.Error(my_error_msg);
                        throw new Exception(my_error_msg);
                    }
                    Session["p_session_personal_table_id"] = my_table_id;

                    Session["table_name"] = my_table_details.p_table_name;
                    
                    f_goto_table(my_table_id);
                }
                else
                {
                    my_table_details = get_table_details(txt_get_fm_email_id.Text);
                    Session["password"] = "hedgeem123";
                    btn_login_Click(btnLogin, new EventArgs());
                    
                    int a_table_id = my_table_details.p_table_id;
                    Session["p_session_personal_table_id"] = a_table_id;

                    Session["table_name"] = my_table_details.p_table_name;
                    
                    service.f_update_last_login_date(Session["p_session_username"].ToString());//this function is used to update last login date and time of user
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
    */

    protected string f_get_facebook_app_id()
    {
        String my_app_id = ConfigurationManager.AppSettings["hedgeem_facebook_app_id"];

        return my_app_id;
    }
    protected void btn_play_shared_table_Click(object sender, EventArgs e)
    {

        try
        {
            // Log that the user has clicked the 'Play now' button.
            HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
            my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
            my_log_event.p_message = String.Format("User [{0}] click 'Play Shared Table", Session["p_session_username"]);
            log.Info(my_log_event.ToString());

            Session["p_session_player_id"] = xxxHC_ANON_PLAYER_ID;
            Session["theme"] = "CASINO_TABLE";
            Session["Facebook_User_Id"] = "not_set";
            Session["user_role"] = "BASIC_USER";
            Session["p_session_username"] = xxxHC_ANON_USERNAME;
            Session["p_session_personal_table_id"] = 1000;
            f_goto_table(1000);
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in method [btn_play_shared_table_Click] - " + ex.Message.ToString() + "');";

            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_detailed_message_str = my_error_popup;
            my_popup_message.p_is_visible = true;

            //newuser.Visible = false;

            Place_Holder_Popup_Message.Controls.Add(my_popup_message);

            my_popup_message.Dispose();

            log.Error("Error in btn_play_shared_table_Click", new Exception(ex.Message));
      //      throw new Exception(String.Format("fish cakes:", ex.Message));
        }


    }


    protected void btnAdmin_Click(object sender, EventArgs e)
    {
        log.Info("User clicked on Admin Button");
        try
        {
            if (txt_get_fm_email_id.Text == "")
            {
                Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'> document.getElementById('alertmessage').style.display = 'block';</script>");
                //  Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>alert('You are not logged in. Please log in to view this Page.');document.getElementById('progressbar').style.display='none'; </script>");
            }
            else
            {
                DataTable get_password = new DataTable();
                DataTable user_details = new DataTable();

                string my_endpoint1 = String.Format("{0}/ws_get_password_from_db/{1}/", p_current_json_webservice_url_base,
                                                                           txt_get_fm_email_id.Text);

                get_password = (DataTable)f_get_object_from_json_call_to_server(my_endpoint1, typeof(DataTable));

              //  get_password = service.f_get_password_from_db(txt_get_fm_email_id.Text);
                if (get_password.Rows.Count == 0)
                {
                    Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'> document.getElementById('alertmessage').style.display = 'block';</script>");
                    // Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>alert('You are not authorized to view this page.');</script>");
                }
                else
                {
                    string password = get_password.Rows[0]["password"].ToString();


                  string  my_endpoint = String.Format("{0}/ws_my_user_details/{1},{2}/", p_current_json_webservice_url_base,
                                                                                    txt_get_fm_email_id.Text,
                                                                                     password);

                    user_details = (DataTable)f_get_object_from_json_call_to_server(my_endpoint, typeof(DataTable));
          
                   // user_details = service.my_user_details(txt_get_fm_email_id.Text, password);
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
                            Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'> document.getElementById('alertmessage').style.display = 'block';</script>");

                            //Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>alert('You are not authorized to view this page.');</script>");
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "Error in method [btnAdmin_Click] - " + ex.Message.ToString();
        //    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Error in btnAdmin_Click", new Exception(ex.Message));

            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_detailed_message_str = my_error_popup;
            my_popup_message.p_is_visible = true;
           
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);

            my_popup_message.Dispose();

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
            
            if (email != "")
            {
                string my_endpoint1 = String.Format("{0}/ws_get_password_from_db/{1}/", p_current_json_webservice_url_base,
                                                                                email);

              DataTable  dt = (DataTable)f_get_object_from_json_call_to_server(my_endpoint1, typeof(DataTable));
               // DataTable dt = service.f_get_password_from_db(email);

                

                if (dt.Rows.Count > 0)
                {
                    string password = dt.Rows[0]["password"].ToString();

                    string my_endpoint = String.Format("{0}/ws_my_user_details/{1},{2}/", p_current_json_webservice_url_base,
                                                                                   email,
                                                                                     password);

                    DataTable userdetails = (DataTable)f_get_object_from_json_call_to_server(my_endpoint, typeof(DataTable));

                   // DataTable userdetails = service.my_user_details(email, password);
                    role = userdetails.Rows[0]["role"].ToString();
                }
                HttpContext.Current.Session["user_role"] = role;
            }
            return role;
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in method [f_check_user_role] - " + ex.Message.ToString() + "');";
            //System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>" + my_error_popup + "</SCRIPT>");

            log.Error("Error in f_check_user_role", new Exception(ex.Message));
            return my_error_popup;
        }
       
    }

    private static int p_server_id
    {
        get
        {
            int my_server_id = Convert.ToInt32(ConfigurationManager.AppSettings["default_hedgeem_server_id"]);
            return my_server_id;
        }
    }

    /// <summary>
    /// Added by Simon on 1 Dec 2014 not really knowing what I am doing but copying existing code. Called when Facebook SDK is rendered and user is logged in.
    /// This is called by the Javascript function when it has detemined that a user is authenticated against facebook ID.  I should then use this to actually 
    /// log in to the HedgeEm Server.
    /// Method must be STATIC
    /// Last edited 4 Jan 2015 (Commented out throw new exception when user not found.
    /// I believe the only purpose of this method is to call the hedgeem webservice login function when the user authenticates with facebook.
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static string f_facebook_login_called_from_javascript()
    {
        log.Warn("WANRING - COPIED NOT USED f_check_user_role called to check the role of the user.");
        string role = "Herman";
        string email = HttpContext.Current.Request.QueryString["uid"].ToString();

        hedgeem_control_popup_message my_popup_message = new hedgeem_control_popup_message();
        my_popup_message.p_detailed_message_str = "";

        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        // Log the fact this method has been called
        my_log_event.p_message = String.Format("Method [btn_login_Click] called ");
        log.Debug(my_log_event.ToString());

        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "This message creates new fb user in database";
        string my_endpoint = "Not Set";
        try
        {
            int xxxHC_a_opening_balance = 100;
            string xxx_default_password = "hedgeem123";
            try
            {

                // Creates (registers) a new user (HedgeEmPlayer) in the HedgeEm Server.
                // Note this also creates a Personal Table for them (which the HedgeEmTable is returned in the Player object 'p_personal_table_id
                enum_authentication_method my_authentication_method = enum_authentication_method.FACEBOOK;
                enum_user_role my_user_role = enum_user_role.PAYING_USER;
                my_endpoint = String.Format("{0}/ws_register_user_and_create_personal_table/{1},{2},{3},{4},{5},{6}/",
                                                p_current_json_webservice_url_base,
                                                email,
                                                xxx_default_password,
                                                email,
                                                xxxHC_a_opening_balance,
                                                my_authentication_method,
                                                my_user_role);

                HedgeEmPlayer my_player = (HedgeEmPlayer)f_get_object_from_json_call_to_server(my_endpoint, typeof(HedgeEmPlayer));
                if (my_player.p_error_message != null)
                {
                    if (my_player.p_error_message != "")
                    {
                        throw new Exception(my_player.p_error_message);
                    }
                }



                //Page.RegisterStartupScript("OnLoad", "<script>alert('Thank you for registering, you are now logged in an can start to play HedgeEm'); if(alert){ window.location='frm_website_home.aspx';}</script>");
            }
            /*
        catch (System.Net.WebException webex)
        {
            HttpWebResponse my_error_response = webex.Response as HttpWebResponse;
            switch (my_error_response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    my_log_event.p_message = String.Format(
                       "Error (Web Exception: BadRequest). Endpoint URI probably badly formed ...\nEndpoint [{0}], Status Code [{1}]",
                              my_endpoint,
                               my_error_response.StatusCode
                              );
                    break;

                case HttpStatusCode.NotFound:
                    my_log_event.p_message = String.Format(
                       "Error (Web Exception: NotFound). Endpoint URI not found; Endpoint [{0}], Status Code [{1}]",
                              my_endpoint,
                               my_error_response.StatusCode
                              );
                    break;

                default: my_log_event.p_message = String.Format(
                       "Error unexpected status code (Web Exception of type Status Code [{0}] for Endpoint [{1}], ",
                              my_error_response.StatusCode,
                              my_endpoint
                              );
                    break;
            }
            throw new WebException(my_log_event.ToString(), webex);

        }*/
            catch (Exception ex)
            {
                string my_error_message = "Error while btn_new_fb_user_Click" + ex.Message;
                string my_error_popup = "alert('Error in btn_new_fb_user_Click - " + ex.Message.ToString() + "');";
                log.Error(my_error_message);
           throw new Exception(my_error_message);
               // System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>" + my_error_popup + "</SCRIPT>");
            }
        }
        catch (Exception ex)
        {
            string my_error_message = "Error while btn_new_fb_user_Click" + ex.Message;
            string my_error_popup = "alert('Error in creating user - " + ex.Message.ToString() + "');";
            log.Error(my_error_message);
         throw new Exception(my_error_message);
            //System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>" + my_error_popup + "</SCRIPT>");
        }


        // Detemine if there is a session varible set for facebook. 
        // xxx Nov 2014 Note ... Not sure why - I assmume this is to check
        // if the user has already authenticated via facebook so we can make choices about what to display
        //if (Session["chkfb"] != null)
        //{
        //   chk_fb_visibility.Visible = true;
        //    log.Debug(String.Format("Session variable 'chkfb' is {0}", Session["chkfb"].ToString()));
        //}
        //else
        // {
        //  chk_fb_visibility.Visible = false;
        //  chk_fb_visibility.Visible = false;
        //}

        // Declare vairibles that will be required later
        string my_username = email;
        string my_password = "";
        DataTable my_user_details;

        try
        {
            string xxxHC_password = "hedgeem123";

            // Attempt to retrieve a Player object from the HedgeEm Webservice using the credentials supplied.
            my_endpoint = String.Format("{0}/ws_login/{1},{2},{3},{4}/", p_current_json_webservice_url_base, p_session_id, p_server_id, my_username, xxxHC_password);

            // Might need to make next method static
            HedgeEmPlayer my_player = null;
            my_player = (HedgeEmPlayer)f_get_object_from_json_call_to_server(my_endpoint, typeof(HedgeEmPlayer));
            if (my_player == null)
            {
                string my_error_msg = String.Format("Player object is null after call to Webservice to retrieve details", my_username);
                my_log_event.p_message = my_error_msg;
                log.Error(my_log_event.ToString());
                throw new Exception(my_error_msg);
            }

            if (my_player.p_player_id <= 0)
            {
                my_log_event.p_message = my_player.p_error_message;
                log.Warn(my_log_event.ToString());
                //throw new Exception(my_log_event.ToString());

            }
            else
            {
                // If we have got here the user has logged into the server sucessfully so we should set some session variables for ease of use later
                int my_player_id = my_player.p_player_id;
                HttpContext.Current.Session["p_session_player_id"] = my_player_id;
                HttpContext.Current.Session["display_name"] = my_player.p_display_name;
                HttpContext.Current.Session["p_session_username"] = my_username;
                HttpContext.Current.Session["password"] = my_password;
                HttpContext.Current.Session["p_session_personal_table_id"] = my_player.p_personal_table_id;

                string my_info_msg = String.Format("Session vars set: p_session_player_id[{0}], display_name[{1}], p_session_username[{2}], password[{3}], p_session_personal_table_id[{4}], ",
                                    HttpContext.Current.Session["p_session_player_id"],
                                    HttpContext.Current.Session["display_name"],
                                    HttpContext.Current.Session["p_session_username"],
                                    HttpContext.Current.Session["password"],
                                    HttpContext.Current.Session["p_session_personal_table_id"]);

                my_log_event.p_message = my_info_msg;
                log.Info(my_log_event.ToString());


                // As the user has now logged in by HedgeEm authentication hide the Facebook login DIV
                if (HttpContext.Current.Session["facebooklogin"] != null)
                {
                    //LoginDiv.Attributes.Add("style", "display:none !Important;");
                }

                // As the user has now logged show the logout button and hide the login button
                // xxx Nov 2014.  Not sure why this is on condition of facebooklogin; I separated this out from if/else
                // of above to figure this out.  I assume is because you could be logged in by facebook so still want to
                // show / hide login/logout buttons appropriately
                if (HttpContext.Current.Session["facebooklogin"] == null)
                {
                    //lbl_user_name.Text = HttpContext.Current.Session["display_name"].ToString();
                    //Logout.Attributes.Add("style", "display:block !Important;");
                    //LoginDiv.Attributes.Add("style", "display:none !Important;");
                }

                // As the user has now logged in Enable the 'Play Now' button
                //btn_play_now.Enabled = true;



                // If the 'fb_hide_logout' is set it implies xxxxxxxxxxxxxxxxxxxxx so enable the 'Play Now' button
                if (HttpContext.Current.Session["fb_hide_logout"] != null)
                {
                    //    chk_fb_visibility_logout.Visible = false;
                    //btn_play_now.Visible = true;

                }

                // If the 'chk_logout_hide' is set it implies xxxxxxxxxxxxxxxxxxxxx so enable the 'Play Now' button
                if (HttpContext.Current.Session["chk_logout_hide"] != null)
                {

                    //   chk_fb_visibility.Visible = true;
                    //btn_play_now.Visible = true;
                    //btnLogout.Visible = false;
                }
                //Page.RegisterStartupScript("OnLoading", "<script>load_edit_profile();</script>");
                // Response.Redirect("frm_website_home.aspx");
                //Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('progressbar').style.display='none';</script>");
            }
            return role;
        }
        catch (Exception ex)
        {
            string my_error_message = "Fatal Errorin method [btn_login_Click] - " + ex.Message.ToString() + "');";

            //  Log the error and raise a new exception
            my_log_event.p_message = String.Format(
                "Fatal error in method [btn_login_Click]. Reason [{0}]",
                        ex.Message);
            log.Error(my_log_event.ToString(), new Exception(ex.Message));
            throw ex;
            //Place_Holder_Popup_Message.Controls.Add(my_popup_message);
        }
       
    }



    protected void btn_new_user_Click(object sender, EventArgs e)
    {
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "This message creates new user in database";
        log.Debug(my_log_event.ToString());
        string my_endpoint = "Not Set";

        bool user_exist;
        hedgeem_control_popup_message my_popup_message = new hedgeem_control_popup_message();
        my_popup_message.p_detailed_message_str = "";

        try
        {
            // This checks whether the entered email id exists in the database or not.
            my_endpoint = String.Format("{0}/ws_get_player_list/sessionabc123,{1},10000/", p_current_json_webservice_url_base, p_server_id);

            List<HedgeEmPlayer> my_player_list = (List<HedgeEmPlayer>)f_get_object_from_json_call_to_server(my_endpoint, typeof(List<HedgeEmPlayer>));
            user_exist = my_player_list.Exists(x => x.p_username == txt_username.Text);

            if (user_exist == false)
            {
                xxxHC_a_opening_balance = 100;
                try
                {
                    enum_authentication_method my_authentication_method = enum_authentication_method.HEDGEEM_ACCOUNT;
                    enum_user_role my_user_role = enum_user_role.PAYING_USER;
                    // Creates (registers) a new user (HedgeEmPlayer) in the HedgeEm Server.
                    // Note this also creates a Personal Table for them (which the HedgeEmTable is returned in the Player object 'p_personal_table_id
                    my_endpoint = String.Format("{0}/ws_register_user_and_create_personal_table/{1},{2},{3},{4},{5},{6}/",
                                                    p_current_json_webservice_url_base,
                                                    txt_username.Text,
                                                    txt_password.Text,
                                                    txt_full_name.Text,
                                                    xxxHC_a_opening_balance,
                                                    my_authentication_method,
                                                    my_user_role);

                    HedgeEmPlayer my_player = (HedgeEmPlayer)f_get_object_from_json_call_to_server(my_endpoint, typeof(HedgeEmPlayer));
                    if (my_player.p_error_message != null)
                    {
                        if (my_player.p_error_message != "")
                        {
                //            throw new Exception(my_player.p_error_message);




                        }
                    }

                    // If we got here we assume Player was created successfully so store Player ID and continue
                    p_session_player_id = my_player.p_player_id;


                    // login to the game
                    Session["p_session_username"] = txt_username.Text;
                    Session["password"] = txt_password.Text;
                    Session["display_name"] = txt_full_name.Text;
                    Session["p_session_personal_table_id"] = my_player.p_personal_table_id;
                    Session["p_session_username"] = my_player.p_username;
                    Session["p_session_password"] = txt_password.Text;
                    Session["p_session_display_name"] = my_player.p_display_name;

                    //sends registration mail
                    //send_user_registration_mail(txt_username.Text, txt_password.Text, txt_full_name.Text);

                    //my_popup_message.p_detailed_message_str = "Thank you for registering, you may now start to play.  Please don't forget to confirm your registration by clicking on the link in the mail we have just sent you to get your free 50 chips and benefit from extra features.";
                    //Place_Holder_Popup_Message.Controls.Add(my_popup_message);
                    HedgeemMessage my_popup_messageError = new HedgeemMessage();
                    my_popup_messageError.p_detailed_message_str = "";
                    my_popup_messageError.p_detailed_message_str = "Thank you for registering, you may now start to play.  Please don't forget to confirm your registration by clicking on the link in the mail we have just sent you to get your free 50 chips and benefit from extra features.";
                    my_popup_messageError.p_is_visible = true;

                    Place_Holder_Popup_Message.Controls.Add(my_popup_messageError);

                    my_popup_messageError.Dispose();
                   // ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('Thank you for registering. You may now start to play. Click on the link in the mail we have just sent you to get benefit from extra features.');", true);
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('Thank you for registering, you may now start to play.  Please don't forget to confirm your registration by clicking on the link in the mail we have just sent you to get your free 50 chips and benefit from extra features.'); if(alert){ window.location='frm_website_home.aspx';}", true);
                    if (Session["p_session_username"] != null)
                    {

                        Logout.Attributes.Add("style", "display:block!Important;");
                        btnLogout.Visible = true;
                        btnLogout.Enabled = true;
                        Page.RegisterStartupScript("OnLoading", "<script>load_edit_profile();</script>");
                        if (Session["display_name"] != null)
                        {
                            lbl_user_name.Text = Session["display_name"].ToString();
                        }
                        //LoginDiv.Attributes.Add("style", "display:none !Important;");
                        divLogin.Attributes.Add("style", "display:none !Important;");
                        if (Session["p_session_username"] != null)
                        {
                            if (File.Exists(Server.MapPath("resources/player_avatar_" + Session["p_session_username"].ToString() + ".jpg")))
                            {
                                usr_image.ImageUrl = "../resources/player_avatar_" + Session["p_session_username"].ToString() + ".jpg";
                            }
                            
                            else
                            {
                                usr_image.ImageUrl = "../resources/avitars/user_square.png";
                            }
                        }

                        btn_play_now.Enabled = true;



                        string role = "";
                        if (my_player != null)
                        {
                            role = my_player.p_role;

                            my_log_event.p_message = String.Format("User/player role hardcoded to BASIC_USER");
                            log.Debug(my_log_event.ToString());

                        }

                        if (role == enum_user_role.ADMIN.ToString())
                        {
                            btnAdmin.Attributes.Add("style", "display:block");
                        }

                        // - xxx- Hardcoding value of role to BASIC_USER as need to discuss this with Simon.
                        Session["user_role"] = "BASIC_USER";
                        Page.RegisterStartupScript("OnLoad", "<script>document.getElementById('progressbar').style.display='none';</script>");
                    }

                }
                catch (Exception ex)
                {
                    my_log_event.p_message = String.Format("Reason [{0}]", ex.Message);
                    HedgeemerrorPopup my_popup_messageError = new HedgeemerrorPopup();
                    my_popup_messageError.p_detailed_message_str = "";
                    my_popup_messageError.p_detailed_message_str = String.Format("Reason [{0}]", ex.Message);
                    my_popup_messageError.p_is_visible = true;

                    Place_Holder_Popup_Message.Controls.Add(my_popup_messageError);

                    my_popup_messageError.Dispose();
                    log.Error(my_log_event.ToString());
                 //   throw new Exception(my_log_event.ToString());
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
            string my_error_message = String.Format("ERROR! Unable to create user.  Reason [{0}]", ex.Message);
            //string my_error_popup = "alert('Error in creating user - " + ex.Message.ToString() + "');";
            lbl_register_status_msg.Visible = true;
            lbl_register_status_msg.Text = "ERROR! Unable to create user.  Hedge'Em Administrators have been informed.";
            log.Error("Error in creating user", new Exception(ex.Message));
            HedgeemerrorPopup my_popup_messageError = new HedgeemerrorPopup();
            my_popup_messageError.p_detailed_message_str = "";
            my_popup_messageError.p_detailed_message_str = my_error_message;
            my_popup_messageError.p_is_visible = true;

            Place_Holder_Popup_Message.Controls.Add(my_popup_messageError);

            my_popup_messageError.Dispose();
            //throw new Exception("ssssss");

        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        _xxx_log_event.p_method_name = "btnSend_Click";
        _xxx_log_event.p_message = "This button is clicked to send forgot password for the user.";
        log.Debug(_xxx_log_event.ToString());
        bool user_exist;

        string my_endpoint = "Not Set";

        try
        {

            // This checks whether the entered email id exists in the database or not.
            my_endpoint = String.Format("{0}/get_server_info/10/", p_current_json_webservice_url_base);

            List<HedgeEmPlayer> my_player_list = (List<HedgeEmPlayer>)f_get_object_from_json_call_to_server("ws_get_player_list/sessionabc123,10,10000/", typeof(List<HedgeEmPlayer>));
            user_exist = my_player_list.Exists(x => x.p_username == txt_username.Text);

            // If user exists then proceed
            if (user_exist != false)
            {
                DataTable dt = new DataTable();
                // get password of the entered username
                string my_endpoint1 = String.Format("{0}/ws_get_password_from_db/{1}/", p_current_json_webservice_url_base,
                                                                                  txt_Email.Text);

                dt = (DataTable)f_get_object_from_json_call_to_server(my_endpoint1, typeof(DataTable));

             //   dt = service.f_get_password_from_db(txt_Email.Text);
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
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeemerrorPopup my_popup_messageError = new HedgeemerrorPopup();
            my_popup_messageError.p_detailed_message_str = "";
            my_popup_messageError.p_detailed_message_str = my_error_popup;
            my_popup_messageError.p_is_visible = true;

            Place_Holder_Popup_Message.Controls.Add(my_popup_messageError);

            my_popup_messageError.Dispose();
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
            Page.RegisterStartupScript("UserMsg", "<script>alert('Password has been sent to you successfully. Check Your Mail for further instructions.');if(alert){ window.location='frm_website_home.aspx';}</script>");
        }
        catch (Exception ex)
        {
            string my_error_popup = "Error in send_forgot_password_mail - " + ex.Message.ToString();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeemerrorPopup my_popup_messageError = new HedgeemerrorPopup();
            my_popup_messageError.p_detailed_message_str = "";
            my_popup_messageError.p_detailed_message_str = my_error_popup;
            my_popup_messageError.p_is_visible = true;

            Place_Holder_Popup_Message.Controls.Add(my_popup_messageError);

            my_popup_messageError.Dispose();
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
            string my_error_popup = "Error in send_fb_user_registration_mail - " + ex.Message.ToString();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            HedgeemerrorPopup my_popup_messageError = new HedgeemerrorPopup();
            my_popup_messageError.p_detailed_message_str = "";
            my_popup_messageError.p_detailed_message_str = my_error_popup;
            my_popup_messageError.p_is_visible = true;

            Place_Holder_Popup_Message.Controls.Add(my_popup_messageError);

            my_popup_messageError.Dispose();
            log.Error("Fatal error in 'send_fb_user_registration_mail'", new Exception(ex.Message));
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
            
            if (email != "")
            {
                string my_endpoint1 = String.Format("{0}/ws_get_password_from_db/{1}/", p_current_json_webservice_url_base,
                                                                                email);

              DataTable  dt = (DataTable)f_get_object_from_json_call_to_server(my_endpoint1, typeof(DataTable));

         //       DataTable dt = service.f_get_password_from_db(email);

                if (dt.Rows.Count > 0)
                {
                    string password = dt.Rows[0]["password"].ToString();
                    if_exists = true;
                    HttpContext.Current.Session["p_session_username"] = email;
                }
                else
                {
                    if_exists = false;
                }
            }
            return if_exists;
        }
        catch (Exception ex)
        {
            string my_error_popup = "alert('Error in f_check_user_logged_in_from_facebook_first_time - " + ex.Message.ToString() + "');";
            //System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>" + my_error_popup + "</SCRIPT>");
           
            log.Error("Error in f_check_user_logged_in_from_facebook_first_time", new Exception(ex.Message));
            throw ex;
        }
        
    }

    private HedgeEmGameState f_sit_at_anonymous_table(enum_theme a_enum_theme )
    {
        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        HedgeEmGameState my_game_state = new HedgeEmGameState();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "f_sit_at_anonymous_table";
        log.Debug(_xxx_log_event.ToString());
        string my_endpoint = "Not Set";

        // Set the game mode for this call
        // xxx this should be a enum and passed as a arg
        // should get enum from client enums but game mode is not visible for some reason
        // Simon 23 Dec 2015
        string xxx_my_enum_game_mode = "AUTO";
        if (a_enum_theme == enum_theme.RETRO)
        {
            xxx_my_enum_game_mode = "FASTPLAY_FLOP";
        }

        try
        {
            try
            {
    
                // Creates (registers) a new user (HedgeEmPlayer) in the HedgeEm Server.
                // Note this also creates a Personal Table for them (which the HedgeEmTable is returned in the Player object 'p_personal_table_id
                enum_authentication_method my_authentication_method = enum_authentication_method.FACEBOOK;
            my_endpoint = String.Format("{0}/ws_sit_at_anonymous_table/{1},{2},{3},{4}/",
                                                p_current_json_webservice_url_base,
                                                "anon_session_id_123",
                                                83,
                                                a_enum_theme.ToString(),
                                                xxx_my_enum_game_mode);

            my_game_state = (HedgeEmGameState)f_get_object_from_json_call_to_server(my_endpoint, typeof(HedgeEmGameState));
                if (my_game_state.p_error_message != null)
                {
                    if (my_game_state.p_error_message != "")
                    {
                        string my_error_msg = string.Format("Failed trying call 'sit anonymously' to server, Error returned [{0}], Endpoint targeted [{1}]",my_game_state.p_error_message,my_endpoint);
                        throw new Exception(my_error_msg);
                    }
                }

                // If we got here we assume Player was created successfully so store Player ID and continue
                //p_session_player_id = my_game_state.p_player_id;


                // login to the game
                /*Session["p_session_username"] = txt_username.Text;
                Session["password"] = txt_password.Text;
                Session["display_name"] = txt_full_name.Text;
                Session["p_session_personal_table_id"] = my_game_state.p_personal_table_id;
                Session["p_session_username"] = my_game_state.p_username;
                Session["p_session_password"] = txt_password.Text;
                Session["p_session_display_name"] = my_game_state.p_display_name;


                ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('Thank you for registering,your facebook account with HedgeEm. A confirmation mail is sent to your email Id '); if(alert){ window.location='frm_website_home.aspx';}", true);
                */
                //Page.RegisterStartupScript("OnLoad", "<script>alert('Thank you for registering, you are now logged in an can start to play HedgeEm'); if(alert){ window.location='frm_website_home.aspx';}</script>");
            }
            catch (Exception ex)
            {
                string my_error_msg_summary = String.Format("Fatal error trying to sit at table anonymously");
                string my_error_msg_detail = String.Format("Error in {0} Reason [{1}]", 
                    my_log_event.p_method_name, 
                    ex.Message.ToString());

                my_log_event.p_message = my_error_msg_detail;

                log.Error(my_log_event.ToString());

                HedgeemerrorPopup my_popup_messageError = new HedgeemerrorPopup();
                my_popup_messageError.p_user_message_str = my_error_msg_summary;
                my_popup_messageError.p_detailed_message_str = my_error_msg_detail;
                my_popup_messageError.p_is_visible = true;

                Place_Holder_Popup_Message.Controls.Add(my_popup_messageError);

                my_popup_messageError.Dispose();

                //newuser.Visible = false;



            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "Error in f_get_free_anonymous_player suald - " + ex.Message.ToString();
       //     ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Error in f_get_free_anonymous_player xxxaawe", new Exception(ex.Message));

            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_detailed_message_str = my_error_popup;
            my_popup_message.p_is_visible = true;

            //newuser.Visible = false;

            Place_Holder_Popup_Message.Controls.Add(my_popup_message);

            my_popup_message.Dispose();

        }

        return my_game_state;
        
    }

    /// <summary>
    /// Created by Simon on 4th Jan 2014 - I dont think this method is called - VERY BAD DUPLICATED CODE
    /// </summary>
    private void f_create_new_user_based_on_facebook_authentication()
    {

        HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();
        my_log_event.p_method_name = System.Reflection.MethodBase.GetCurrentMethod().ToString();
        my_log_event.p_message = "This message creates new fb user in database";
        log.Debug(_xxx_log_event.ToString());
        string my_endpoint = "Not Set";
        try
        {
            xxxHC_a_opening_balance = 100;
            string xxx_default_password = "hedgeem123";
            try
            {

                // Creates (registers) a new user (HedgeEmPlayer) in the HedgeEm Server.
                // Note this also creates a Personal Table for them (which the HedgeEmTable is returned in the Player object 'p_personal_table_id
                enum_authentication_method my_authentication_method = enum_authentication_method.FACEBOOK;
                enum_user_role my_user_role = enum_user_role.PAYING_USER;
                my_endpoint = String.Format("{0}/ws_register_user_and_create_personal_table/{1},{2},{3},{4},{5},{6}/",
                                                p_current_json_webservice_url_base,
                                                txt_fb_username.Text,
                                                xxx_default_password,
                                                txt_fb_display_name.Text,
                                                xxxHC_a_opening_balance,
                                                my_authentication_method,
                                                my_user_role);

                HedgeEmPlayer my_player = (HedgeEmPlayer)f_get_object_from_json_call_to_server(my_endpoint, typeof(HedgeEmPlayer));
                if (my_player.p_error_message != null)
                {
                    if (my_player.p_error_message != "")
                    {
                        throw new Exception(my_player.p_error_message);
                    }
                }

                // If we got here we assume Player was created successfully so store Player ID and continue
                p_session_player_id = my_player.p_player_id;


                // login to the game
                Session["p_session_username"] = txt_username.Text;
                Session["password"] = txt_password.Text;
                Session["display_name"] = txt_full_name.Text;
                Session["p_session_personal_table_id"] = my_player.p_personal_table_id;
                Session["p_session_username"] = my_player.p_username;
                Session["p_session_password"] = txt_password.Text;
                Session["p_session_display_name"] = my_player.p_display_name;


                //service.f_activate_user(txt_fb_username.Text);

                //send_fb_user_registration_mail(txt_fb_username.Text, xxx_default_password, txt_fb_display_name.Text);

                //lbl_register_status_msg.Visible = true;
                //lbl_register_status_msg.Text = "Thank you for registering, Please activate your account by clicking link sent on your registered email id.";
                ScriptManager.RegisterStartupScript(Page, GetType(), "OnLoad", "alert('Thank you for registering,your facebook account with HedgeEm. A confirmation mail is sent to your email Id '); if(alert){ window.location='frm_website_home.aspx';}", true);

                //Page.RegisterStartupScript("OnLoad", "<script>alert('Thank you for registering, you are now logged in an can start to play HedgeEm'); if(alert){ window.location='frm_website_home.aspx';}</script>");
            }
            catch (Exception ex)
            {
                string my_error_popup = "Error in btn_new_fb_user_Click - " + ex.Message.ToString();
          //      ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                log.Error("Error while btn_new_fb_user_Click", new Exception(ex.Message));

                HedgeemerrorPopup my_popup_messageError = new HedgeemerrorPopup();
                my_popup_messageError.p_detailed_message_str = "";
                my_popup_messageError.p_detailed_message_str = my_error_popup;
                my_popup_messageError.p_is_visible = true;

                Place_Holder_Popup_Message.Controls.Add(my_popup_messageError);

                my_popup_messageError.Dispose();

                //newuser.Visible = false;



            }
        }
        catch (Exception ex)
        {
            string my_error_popup = "Error in creating user - " + ex.Message.ToString();
       //     ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            log.Error("Error in creating user", new Exception(ex.Message));

            HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
            my_popup_message.p_detailed_message_str = "";
            my_popup_message.p_detailed_message_str = my_error_popup;
            my_popup_message.p_is_visible = true;

            //newuser.Visible = false;

            Place_Holder_Popup_Message.Controls.Add(my_popup_message);

            my_popup_message.Dispose();

        }
    }

    protected void btn_new_fb_user_Click(object sender, EventArgs e)
    {
        f_create_new_user_based_on_facebook_authentication();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        btn_play_shared_table_Click(sender, e);
    }
}