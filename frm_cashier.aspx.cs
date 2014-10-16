using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class frm_cashier : System.Web.UI.Page
{
    int tableid;
    int playerid;
    int seat_id=0;
    //seat table
    hedgeem_control_seats ss_seat=new hedgeem_control_seats();

    //For Logging
    private static String logger_name_as_defined_in_app_config = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(logger_name_as_defined_in_app_config);

    //webservice object 
    localhost.WebService service = new localhost.WebService();
    protected void Page_Load(object sender, EventArgs e)
    {

        //log event object is created 
        //HedgeEmLogEvent my_log_event = new HedgeEmLogEvent();  xxx - this does not work Kiran/Simon - 25th March 2014
        //my_log_event.p_method_name = "page_load_frm_cashier";
        log.Info("Page_Load of Cashier Page is called");
        try
        {
            tableid = service.get_table_id(Session["table_name"].ToString());
            playerid = service.get_player_id(Session["username"].ToString(), Session["password"].ToString());

            if (Session.Count == 0)
            {
                Page.RegisterStartupScript("Alert Message", "<script type='text/javascript'>show_session_timeout_message();</script>");
            }
            else
            {
                lbl_account_balance.Text = service.f_get_player_account_balance(playerid).ToString();
                
                lbl_seat_balance.Text = Math.Round(service.f_get_player_seat_balance(tableid, 0, playerid), 2).ToString();
            }
        }
        catch(Exception ex)
        {
           //my_log_event.p_message = "Exception caught - " + ex.Message;
           //log.Error(_xxx_log_event.ToString());
           //throw new Exception(String.Format("page_load_frm_cashier", ex.Message));
            log.Error("Error in Page Load of Cashier Page", new Exception(ex.Message));
        }
    }
    protected void txt_tranfer_balance_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btn_transfer_Click(object sender, EventArgs e)
    {

        //log event object is created 
        //HedgeEmLogEvent my_log_event = new HedgeEmLogEvent(); xxx - this does not work Kiran/Simon - 25th March 2014
        //my_log_event.p_method_name = "btn_transfer_Click";
        log.Info("btn_transfer_Click of Cashier Page is called");

        try
        {

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
            //my_log_event.p_message = "Exception caught - " + ex.Message;
            //log.Error(_xxx_log_event.ToString());
            //throw new Exception(String.Format("btn_transfer_Click ", ex.Message));
        }

    }
    protected void btn_deposit_Click(object sender, EventArgs e)
    {


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
             log.Error("Error inbtn_deposit_Click of Cashier Page", new Exception(ex.Message));
         }

    }
}