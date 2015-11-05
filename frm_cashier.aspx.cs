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

public partial class frm_cashier : System.Web.UI.Page
{
    int tableid;
    int playerid;
    int seat_id=0;
    string xxxHC_Session_id = "sessionabc123";
    int xxxHC_ANON_PLAYER_ID = 100;
    string xxxHC_ANON_USERNAME = "admin@mantura.com";
   
    //seat table
    hedgeem_control_seats ss_seat=new hedgeem_control_seats();

    //For Logging
    private static String logger_name_as_defined_in_app_config = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(logger_name_as_defined_in_app_config);

    //webservice object 
    localhost.WebService service = new localhost.WebService();
    protected void Page_Load(object sender, EventArgs e)
    {
        HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
        my_popup_message.p_detailed_message_str = "";
        my_popup_message.p_is_visible = false;
        //log event object is created 
        //HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();  xxx - this does not work Kiran/Simon - 25th March 2014
        //my_log_event.p_method_name = "page_load_frm_cashier";
        log.Info("Page_Load of Cashier Page is called");


        try
        {
            if (!p_valid_session_exists)
            {
                string my_error_popup = "Invalid session - so this operation cannot proceed.";
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                my_popup_message.p_detailed_message_str = my_error_popup.ToString();
                Place_Holder_Popup_Message.Controls.Add(my_popup_message);
               // throw new Exception("Invalid session - so this operation cannot proceed.");
            }
           
            tableid = p_session_personal_table_id;
            playerid = p_session_player_id;

            if (Request.QueryString["seat_id"] != null)
            {
                p_session_current_seat_id = Convert.ToInt32(Request.QueryString["seat_id"]);
            }

            if (Session.Count == 0)
            {
                Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>show_session_timeout_message();</script>");
            }
            else
            {
               // lbl_account_balance.Text = service.f_get_player_account_balance(playerid).ToString();

                string my_endpoint = String.Format("{0}/ws_get_player_account_balance/{1}",
                                                                               playerid);

                lbl_account_balance.Text = f_get_object_from_json_call_to_server(my_endpoint, typeof(Double)).ToString();
        


                
                lbl_seat_balance.Text = Math.Round(service.f_get_player_seat_balance(tableid, 0, playerid), 2).ToString();
            }
        }
        catch(Exception ex)
        {
           //my_log_event.p_message = "Exception caught - " + ex.Message;
           //log.Error(_xxx_log_event.ToString());
           //throw new Exception(String.Format("page_load_frm_cashier", ex.Message));
            log.Error("Error in Page Load of Cashier Page", new Exception(ex.Message));
          //  throw new Exception(String.Format("page_load_frm_cashier", ex.Message));

            
         //  my_log_event.p_message = String.Format("Reason [{0}]", ex.Message);
            string my_error_popup = "Error in Page Load of Cashier Page - " + ex.Message.ToString();
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
                    my_popup_message.p_detailed_message_str = my_error_popup.ToString();
                    Place_Holder_Popup_Message.Controls.Add(my_popup_message);
            
        }
    }
    protected void txt_tranfer_balance_TextChanged(object sender, EventArgs e)
    {

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

        //set { Session["p_session_username"] = value; }
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

        //set { Session["p_session_personal_table_id"] = value; }
    }

    private int p_session_current_seat_id
    {
        get
        {
            int my_seat_id = -1;
            try
            {
                my_seat_id = Convert.ToInt32(Session["p_session_current_seat_id"]);
            }
            catch (Exception e)
            {
                my_seat_id = -1;
            }

            return my_seat_id;
        }

        set { Session["p_session_current_seat_id"] = value; }
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

        //set { Session["p_session_personal_table_id"] = value; }
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

        //set { Session["p_session_player_id"] = value; }
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

            if (Session["p_session_player_id"] == "")
            {
                return false;
            }

            return true;
        }
    }

    protected void btn_transfer_Click(object sender, EventArgs e)
    {
        HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
        my_popup_message.p_detailed_message_str = "";
        my_popup_message.p_is_visible = false;
        //log event object is created 
        //HedgeEmLogEvent my_log_event = new HedgeEmLogEvent(); xxx - this does not work Kiran/Simon - 25th March 2014
        //my_log_event.p_method_name = "btn_transfer_Click";
        log.Info("btn_transfer_Click of Cashier Page is called");

        try
        {
            //if (! p_valid_session_exists_global_asax_IS_THIS_RIGHT_PLACE_xxx){
            if (!p_valid_session_exists)
            {
                throw new Exception("Invalid session - so this operation cannot proceed.");
            }
            double acc_balance = Convert.ToDouble(lbl_account_balance.Text);
            double balance_to_transfer = Convert.ToDouble(txt_tranfer_balance.Text);
            double final_player_balance = 0.0, final_seat_balance = 0;
            final_player_balance = acc_balance - balance_to_transfer;
            final_seat_balance = Math.Round(Convert.ToDouble(lbl_seat_balance.Text) + balance_to_transfer, 2);
            if (final_player_balance < 0)
            {
                lbl_msg.Visible = true;
                lbl_msg.Text = "Enter the Correct Amoount";
            }
            else
            {
                service.f_set_player_balance(playerid, final_player_balance);
                service.f_update_seat_table_with_seat_id(playerid, tableid, final_seat_balance, seat_id);
                lbl_msg.Text = "updated successfully";
                lbl_account_balance.Text = Math.Round(service.f_get_player_account_balance(playerid), 2).ToString();
                lbl_seat_balance.Text = Math.Round(service.f_get_player_seat_balance(tableid, 0, playerid), 2).ToString();
                txt_tranfer_balance.Text = "0";
                ss_seat.p_balance =service.f_get_player_seat_balance(tableid, 0, playerid).ToString();

            }
        }
        catch(Exception ex)
        {
            log.Error("Error in btn_transfer_Click of Cashier Page", new Exception(ex.Message));
            string my_error_popup = "Error in btn_transfer_Click of Cashier Page" + ex.Message.ToString();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup.ToString();
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
            //my_log_event.p_message = "Exception caught - " + ex.Message;
            //log.Error(_xxx_log_event.ToString());
            //throw new Exception(String.Format("btn_transfer_Click ", ex.Message));
        }

    }
    protected void btn_deposit_Click(object sender, EventArgs e)
    {
        HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
        my_popup_message.p_detailed_message_str = "";
        my_popup_message.p_is_visible = false;

        //log event object is created 
        //HedgeEmLogEvent my_log_event = new HedgeEmLogEvent(); xxx - this does not work Kiran/Simon - 25th March 2014
        //my_log_event.p_method_name = "btn_deposit_Click"; 
        log.Info("btn_deposit_Click of Cashier Page is called");

        double acc_balance = Convert.ToDouble(lbl_account_balance.Text);
        double amount_to_deposit=Convert.ToDouble(txt_deposit.Text);
         double final_player_balance = 0.0;

         try
         {

             if (amount_to_deposit <= 0)
             {
                 lbl_msg.Visible = true;
                 lbl_deposit_status.Text = "Enter the Amoount greater than 0";
                 lbl_deposit_status.Visible = true;
             }
             else
             {
                 final_player_balance = acc_balance + amount_to_deposit;
                 service.f_set_player_balance(playerid, final_player_balance);

                 lbl_deposit_status.Text = "Deposited successfully";
                 lbl_deposit_status.Visible = true;
                 lbl_account_balance.Text = Math.Round(service.f_get_player_account_balance(playerid), 2).ToString();
                 lbl_seat_balance.Text = Math.Round(service.f_get_player_seat_balance(tableid, 0, playerid), 2).ToString();
                 txt_deposit.Text = "0";
             }
         }
         catch(Exception ex)
         {
             //my_log_event.p_message = "Exception caught - " + ex.Message;
             //log.Error(_xxx_log_event.ToString());
             //throw new Exception(String.Format("btn_deposit_Click", ex.Message));
             string my_error_popup = "Error in method [page_load_frm_cashier] - " + ex.Message.ToString();
             //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
             my_popup_message.p_detailed_message_str = my_error_popup.ToString();
             Place_Holder_Popup_Message.Controls.Add(my_popup_message);
             log.Error("Error inbtn_deposit_Click of Cashier Page", new Exception(ex.Message));
         }

    }

    protected void btn_sit_here_Click(object sender, EventArgs e)
    {

        HedgeemerrorPopup my_popup_message = new HedgeemerrorPopup();
        my_popup_message.p_detailed_message_str = "";
        my_popup_message.p_is_visible = false;
        //log event object is created 
        //HedgeEmLogEvent my_log_event = new HedgeEmLogEvent(); xxx - this does not work Kiran/Simon - 25th March 2014
        //my_log_event.p_method_name = "btn_sit_here_Click"; 
        log.Info("btn_sit_here_Click of Cashier Page is called");

        double acc_balance = 0;
        if (lbl_account_balance.Text != "")
        {
            acc_balance = Convert.ToDouble(lbl_account_balance.Text);
        }
        
        
        double amount_to_deposit = 0;
        if (txt_deposit.Text != "")
        {
            amount_to_deposit = Convert.ToDouble(txt_deposit.Text);
        }
        
        double final_player_balance = 0.0;

        try
        {

            if (amount_to_deposit <= 0)
            {
                lbl_msg.Visible = true;
                lbl_deposit_status.Text = "Enter the Amoount greater than 0";
                lbl_deposit_status.Visible = true;
            }
            else
            {
                final_player_balance = acc_balance + amount_to_deposit;
                service.f_set_player_balance(playerid, final_player_balance);

                lbl_deposit_status.Text = "Deposited successfully";
                lbl_deposit_status.Visible = true;
                lbl_account_balance.Text = Math.Round(service.f_get_player_account_balance(playerid), 2).ToString();
                lbl_seat_balance.Text = Math.Round(service.f_get_player_seat_balance(tableid, 0, playerid), 2).ToString();
                txt_deposit.Text = "0";

                int xxx_my_HC_session_id_not_used_yet = 666999;
                int my_HC_seat_id = 0;
                HedgeEmGenericAcknowledgement my_generic_ack = new HedgeEmGenericAcknowledgement();
                my_generic_ack = (HedgeEmGenericAcknowledgement)f_get_object_from_json_call_to_server("ws_sit_at_table_new/" + p_session_current_table_id + "," +
                                                   10 + "," +
                                                   p_session_current_table_id + "," +
                                                   p_session_current_seat_id + "," +
                                                   p_session_player_id + "," +
                                                   amount_to_deposit, typeof(HedgeEmGenericAcknowledgement));
                if (my_generic_ack.p_error_message != null)
                {
                    if (my_generic_ack.p_error_message != "")
                    {
                        throw new Exception(my_generic_ack.p_error_message);
                    }
                }

                //my_log.p_message = String.Format("Successfully retrieved gamestate from server. Table ID [{0}], State [{1}]", _global_game_state_object.p_table_id, _global_game_state_object.p_current_state_enum.ToString());
                //log.Debug(my_log.ToString());
                 
 

                
            }
        }
        catch (Exception ex)
        {
            //my_log_event.p_message = "Exception caught - " + ex.Message;
            //log.Error(_xxx_log_event.ToString());
            string my_error_popup = "Error in btn_sit_here_Click of Cashier Page " + ex.Message.ToString();
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", my_error_popup, true);
            my_popup_message.p_detailed_message_str = my_error_popup.ToString();
            Place_Holder_Popup_Message.Controls.Add(my_popup_message);
            log.Error("Error in btn_sit_here_Click of Cashier Page", new Exception(ex.Message));
        }

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


}