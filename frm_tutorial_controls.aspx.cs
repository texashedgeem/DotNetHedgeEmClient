using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Data;
using System.Text;
using HedgeEmClient;



public partial class frm_tutorial_controls : System.Web.UI.Page
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


    protected void Page_PreInit(object sender, EventArgs e)
    {
        this.Page.Theme = "TUTORIAL";
        chk_show_chip_icon.Text = "45";        
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        f_redraw_page();
    }






    public void f_redraw_page()
    {
        ss_seat = new hedgeem_control_seats();
        ss_seat.p_player_name = "Simon";
        ss_seat.p_photo_image = "player_avitar_sit_here";
        ss_seat.p_balance = txt_player_balance.Text;
        ss_seat.p_player_id = 123;
        ss_seat.p_seat_index = 0;
        ss_seat.p_show_bets_info = true;

        for (int hand_index = 0; hand_index < 4; hand_index++)
        {
            HedgeEmHandPayout my_hand_payout = new HedgeEmHandPayout();
            my_hand_payout.hand_index = hand_index;
            my_hand_payout.payout_value = 444;
            ss_seat.f_add_hedgeem_hand_payout_to_list(my_hand_payout);
        }
        

        string chip_icon_resource_name = "chip_icon_0.png";
        ss_seat.p_chip_icon = "../resources/" + chip_icon_resource_name;

        if (chk_show_chip_icon.Checked == true)
        {
            ss_seat.p_balance = "puke";
            ss_seat.p_show_bets_info = true;
        }

        bool xxx_show_payout = true;
        if (xxx_show_payout)
        {
            ss_seat.p_payout += 10;
            ss_seat.p_payout = "£12.50";
        }
    
        Place_Holder_Player_Info.Controls.Add(ss_seat);
    }


    protected void chk_show_chip_icon_TextChanged(object sender, EventArgs e)
    {
        ss_seat.p_balance = "Need new functionality here";
        txt_player_balance.Text = "Need new functionality here. xxx";
    }

    protected void txt_player_balance_TextChanged(object sender, EventArgs e)
    {
        ss_seat.p_balance = txt_player_balance.Text;
    }
}