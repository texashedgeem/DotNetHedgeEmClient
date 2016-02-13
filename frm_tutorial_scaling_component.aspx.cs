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
using System.Drawing;
using System.Drawing.Imaging;



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

    }


    protected void Page_Load(object sender, EventArgs e)
    {

    }

  
}