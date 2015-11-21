using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Data; // For DataTable
using System.Resources; // So I can use the resource manager to retrieve image resources programatically
using System.Runtime.InteropServices;
using System.Drawing;
using HedgeEmClient;


    [DefaultProperty("Text")]
    [ToolboxData("<{0}:hedgeem_hand_panel runat=server></{0}:hedgeem_hand_panel>")]
    public class hedgeem_hand_panel : WebControl
    {
        public hedgeem_hand_panel(int a_number_of_stages, int a_number_of_players)
        {
            _players_bets = new double[a_number_of_stages, a_number_of_players];
            _offered_odds = new double[a_number_of_stages];
            
            _number_of_players = a_number_of_players;
        }

        private static int HC_hardcoded_int_to_highlight_unset_values = -666;

                /// <summary>
        /// An array of all players bets for this specific hand and this specific stage.
        /// e.g. if the stage of the game (as recorded in this class was FLOP
        /// </summary>
        public double[,] p_players_bets
        {
            set { _players_bets = value; }
            get { return _players_bets; }
        }
        private double[,] _players_bets;

        /// <summary>
        /// An array of all offered odds for this specific hand and this specific stage.
        /// e.g. if the stage of the game (as recorded in this class was FLOP
        /// </summary>
        public double[] p_offered_odds
        {
            set { _offered_odds = value; }
            get { return _offered_odds; }
        }
        private double[] _offered_odds;

        private int _number_of_players = HC_hardcoded_int_to_highlight_unset_values;
        private int _number_of_stages = HC_hardcoded_int_to_highlight_unset_values;

        private Boolean _is_in_play;
        private Boolean _is_dead;
        private Boolean _is_dead_cert;
        private Boolean _is_winner;
        private Boolean _is_favourite;
        private Boolean _is_best_value;
        private Boolean _show_pays_amount;
        private Boolean _show_hover_highlight;
        private String _shortest_description;
        private String _description;
        private String _long_description;
        private Boolean _show_description = true;
        private int _top_shortest_description;
        private int _left_shortest_description;

        private string buttonvalue;
        private float _pays_amount;
        private String _card1;
        private String _card2;
        private int _hand_index;
        private string _show_winners;
        private String _card_as_short_string;
        static string HC_str_to_indicate_card_should_be_shown_face_down = "ZZ";
        enum_betting_stage _current_betting_stage = enum_betting_stage.NON_BETTING_STAGE;
        public enum_betting_stage p_current_betting_stage
        {
            get { return _current_betting_stage; }
            set { _current_betting_stage = value; }
        }

        public string ToString()
        {
            return String.Format("Desc[{0}], (Card1[{1}],Card2[{2}]). IsDead[{3}]",
                                    _description,
                                    _card1,
                                    _card2,
                                    _is_dead);
        }



        /// <summary>
        /// 
        /// </summary>
        /// 
        public string p_show_winners
        {
            get { return _show_winners; }
            set { _show_winners = value; }
        }
        public string p_buttonvalue
        {
            get { return buttonvalue; }
            set { buttonvalue = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String f_get_stage_as_string(enum_betting_stage a_betting_stage)
        {

                string my_stage_as_string = "";

                switch (a_betting_stage)
                {
                    case enum_betting_stage.HOLE_BETS:
                       
                        return "Hole";
                        
                    case enum_betting_stage.FLOP_BETS:
                        return "Flop";
                        
                    case enum_betting_stage.TURN_BETS:
                        return "Turn";
                        
                    case enum_betting_stage.NON_BETTING_STAGE:
                        //f_activate_deal_next_button_and_hide_others();
                        break;
                

                
            }
                return my_stage_as_string; 
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        public String p_shortest_description
        {
            get { return _shortest_description; }
            set { _shortest_description = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int p_top_shortest_description
        {
            get { return _top_shortest_description; }
            set { _top_shortest_description = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int p_left_shortest_description
        {
            get { return _left_shortest_description; }
            set { _left_shortest_description = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String p_description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String p_long_description
        {
            get { return _long_description; }
            set { _long_description = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int p_hand_index
        {
            get { return _hand_index; }
            set { _hand_index = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String p_card1
        {
            get { return _card1; }
            set
            {
                _card1 = value;
                // Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String p_card2
        {
            get { return _card2; }
            set { _card2 = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean p_is_best_value
        {
            get { return _is_best_value; }
            set { _is_best_value = value; }
        }

        public Boolean p__show_pays_amount
        {
            get { return _show_pays_amount; }
            set { _show_pays_amount = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public Boolean p_is_favourite
        {
            get { return _is_favourite; }
            set { _is_favourite = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean p_is_in_play
        {
            get { return _is_in_play; }
            set { _is_in_play = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean p_is_dead
        {
            get { return _is_dead; }
            set { _is_dead = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean p_is_dead_cert
        {
            get { return _is_dead_cert; }
            set { _is_dead_cert = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean p_is_winner
        {
            get { return _is_winner; }
            set { _is_winner = value; }
        }

        public Boolean p_show_description
        {
            get { return _show_description; }
            set { _show_description = value; }
        }


        /// <summary>
        /// A HandPanel can generate and diplay DIV tags that show all players previous bets.  If your GUI design need this information you need to set
        /// p_show_players_bets tp true;
        /// </summary>
        public Boolean p_show_players_bets
        {
            get { return _show_players_bets; }
            set { _show_players_bets = value; }
        }
        private Boolean _show_players_bets = true;

        public Boolean p_show_hover_highlight
        {
            get { return _show_hover_highlight; }
            set { _show_hover_highlight = value; }
        }

        public float p_pays_amount
        {
            get { return _pays_amount; }
            set { _pays_amount = value; }
        }


        
        
        
        
        
        

        public String p_card_as_short_string
        {
            get { return _card_as_short_string; }
            set
            {
                _card_as_short_string = value;

            }
        }



        /// <summary>
        /// Return: my_payout_for_single_bet_str
        /// Return a string that is to be use to to show how much the payout will be for a single bet.  Decimal places will only be shown if 
        /// non-zero e.g. "Pays £3",  "Pays £1.50"
        /// 
        /// Notes / Requirement
        /// The function uses generates the string that it returns from 'p_pays_amount'.  If this value is not set this function will return an
        /// empty string.
        /// </summary>
        public String p_payout_string
        {
            get
            {
                String my_payout_for_single_bet_str = "";
                // ######### current odds and previous bets
                // Display Payout for single bet.
                float my_payout_for_single_bet = 0;

                my_payout_for_single_bet = p_pays_amount;

                
                if (my_payout_for_single_bet == 0)
                {
                    my_payout_for_single_bet_str = "";
                }
                else
                {
                    if (my_payout_for_single_bet > 0)
                    {
                        my_payout_for_single_bet += 1;
                    }
                    else
                    {
                        my_payout_for_single_bet = my_payout_for_single_bet * -1;
                        my_payout_for_single_bet = 1 + (1 / my_payout_for_single_bet);
                    }

                    // Only show two decimal places if needed (i.e. if hand is a favourite and you get let that
                    // £1 payout for each £1 bet. 
                    // Note my_payout_for_single_bet will have decimal places between 1 and 2 
                    if (my_payout_for_single_bet > 1 && my_payout_for_single_bet < 2)
                    {
                        my_payout_for_single_bet_str = String.Format(" pays £{0:#0.00}", my_payout_for_single_bet);
                    }
                    else
                    {
                        my_payout_for_single_bet_str = String.Format("pays £{0:#0}", my_payout_for_single_bet);

                    }

                   

                }
                return my_payout_for_single_bet_str;
            }
        }


        /// <summary>
        /// Return: my_payout_for_single_bet_str
        /// Return a string that is to be used to to show how much the payout will be for a single bet.  Decimal places will only be shown if 
        /// non-zero e.g. "Pays £3",  "Pays £1.50"
        /// 
        /// Notes / Requirement
        /// The function uses generates the string that it returns from 'p_pays_amount'.  If this value is not set this function will return an
        /// empty string.
        /// </summary>
        public String f_get_payout_string(enum_betting_stage stage_index, int a_player_index)
        {
            // This is the string that will be returned.
            String my_payout_for_single_bet_str = "NOT SET YET";
            double my_previous_bet_payout_if_wins = 0;
                
            // Determine the odds for this hand at this stage.  
            // If zero, no bets could have been placed so return with empty payout string
            double my_offered_odds = _offered_odds[(int)stage_index];
            if (my_offered_odds == 0){
                return "";
            }

            // Determine the bets placed on this hand at this stage.  
            // If zero, no bets could have been placed so return with empty payout string
            double my_bet_double = _players_bets[(int)stage_index, 0];
            if (my_bet_double == 0){
                return "";
            }
            
            // If code gets here a bet must have been made so calculate the payout.
            // Note if the odd are -ve this implies odds are of format 1:n  instead of n:1 
            // (i.e. a favourite that has more chance of winning than of losing.
            // Display Payout for single bet.
            double my_payout_for_single_bet = 0;

            if (my_offered_odds > 0){
                my_previous_bet_payout_if_wins = (my_bet_double * my_offered_odds) + my_bet_double;
            }else{
                my_payout_for_single_bet = my_offered_odds * -1;
                my_payout_for_single_bet = 1 + (1 / my_payout_for_single_bet);
                my_previous_bet_payout_if_wins = (my_bet_double * my_payout_for_single_bet) ;
            }

            // Now that we have calucalted payout, format text to nice human friendly presentation
            string my_stage_str = f_get_stage_as_string(stage_index);

            // Calculate the payout for a single bet taking into consideration -ve odds implies favourite
            


            // Only show two decimal places if needed (i.e. if hand is a favourite and you get let that
            // £1 payout for each £1 bet. 
            // Note my_payout_for_single_bet will have decimal places between 1 and 2 
            if (my_payout_for_single_bet > 1 && my_payout_for_single_bet < 2)
            {
                my_payout_for_single_bet_str = String.Format("£{0:#0.00}", my_previous_bet_payout_if_wins);
            }
            else
            {
                my_payout_for_single_bet_str = String.Format("£{0:#0}", my_previous_bet_payout_if_wins);

            }



            
            return my_payout_for_single_bet_str;
            
        }

        public String p_card_image_filename_card1
        {
            get
            {
                #region Generate paths for card images
                // Generate card image filename
                String my_card_image_filename_card1 = HC_str_to_indicate_card_should_be_shown_face_down;
                //if (_is_dead)
                //{
                //    my_card_image_filename_card1 = "../resources/Cards/card_" + _card1 + "_left_dead.png";
                //}
                //else
                //{
                //    my_card_image_filename_card1 = "../resources/Cards/card_" + _card1 + "_left_alive.png";
                //
                //}
                my_card_image_filename_card1 = "resources/Cards/card_" + _card1 + "_middle_alive.png";

                if (_card1 == HC_str_to_indicate_card_should_be_shown_face_down)
                {
                    //my_card_image_filename_card1 = "../resources/Cards/card__back_blue1_left.png";
                    my_card_image_filename_card1 = "resources/Cards/card__back_blue1_middle.png";

                    
                }
                

                return my_card_image_filename_card1;

                #endregion Generate paths for card images
            }
        }


        public String p_card_image_filename_card2
        {
            get
            {
                #region Generate paths for card images
                // Generate card image filename
                String my_card_image_filename_card2 = HC_str_to_indicate_card_should_be_shown_face_down;
                //if (_is_dead)
                //{
                //    my_card_image_filename_card2 = "../resources/Cards/card_" + _card2 + "_right_dead.png";
                //}
                //else
                //{
                //    my_card_image_filename_card2 = "../resources/Cards/card_" + _card2 + "_right_alive.png";

                //}
                my_card_image_filename_card2 = "resources/Cards/card_" + _card2 + "_middle_alive.png";


                if (_card2 == HC_str_to_indicate_card_should_be_shown_face_down)
                {
                    //my_card_image_filename_card2 = "../resources/Cards/card__back_blue1_right.png";
                    my_card_image_filename_card2 = "resources/Cards/card__back_blue1_middle.png";
                }

                return my_card_image_filename_card2;

                #endregion Generate paths for card images
            }
        }

        private void f_render_cards(HtmlTextWriter writer)
        {
            string my_dead_card_class = "";
            if (_is_dead) {
                 my_dead_card_class = " dead_opacity ";
            }
            writer.WriteLine(String.Format("<div id='hxxxxxxxxxxxx{0}' class='{1}'", _hand_index, my_dead_card_class)); 
          
            
         
            #region Generate the HTML to display Card 1
            writer.WriteLine("");
            writer.WriteLine("<!-- Hand " + _hand_index + ", Card 1 -->");
            writer.WriteLine("<div id='h" + _hand_index + "c1' onclick='javascript:f_placebet(" + _hand_index + ");' class='card_left' runat='server'>");
            writer.Indent += 1;
            writer.WriteLine(String.Format("<div id='card_{0}'></div>", p_card1));
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "animated flip");
            //
            // xxx Consider commenting this next block out when using sprites in all themes to improve performance.
            writer.AddAttribute(HtmlTextWriterAttribute.Src, p_card_image_filename_card1);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();


            writer.Indent -= 1;
            writer.WriteLine("");
            writer.WriteLine("</div>");
            writer.WriteLine("");
            #endregion

            #region Generate the HTML to display Card 2
            writer.WriteLine("");
            writer.WriteLine("<!-- Hand " + _hand_index + ", Card 2 -->");
            writer.WriteLine("<div id='h" + _hand_index + "c2' onclick='javascript:f_placebet(" + _hand_index + ");' class='card_right ' runat='server'>");
            writer.Indent += 1;
            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "animated flip");

            writer.WriteLine(String.Format("<div id='card_{0}'></div>", p_card2));
            
            writer.AddAttribute(HtmlTextWriterAttribute.Src, p_card_image_filename_card2);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.Indent -= 1;
            writer.WriteLine("");
            writer.WriteLine("</div>");
            writer.WriteLine("");
            #endregion Card 2

            writer.WriteLine("</div>");
        }

        private void f_render_previous_placed_bets(HtmlTextWriter writer)
        {

            // Only render this part of the HTML if the developer has set the parameter to show previously placed bets.
            if (_show_players_bets)
            {
                // Create the opening 'container' DIV for all previous bets 
                writer.WriteLine("<!-- Previous bets-->");
                writer.WriteLine("<div id='h" + _hand_index + "_previous_bets' class='previous_placed_bets_container'>");
                writer.Indent += 1;

                // For each stage and for each player/seat render the previous bets
                for (enum_betting_stage stage_index = enum_betting_stage.HOLE_BETS; stage_index <= enum_betting_stage.TURN_BETS; stage_index++)
                {
                    int xxx_HC_seat_index = 0;
                    double my_bet_double = _players_bets[(int)stage_index, xxx_HC_seat_index];
                    
                    // Only render when previous bets have been placed
                    if (my_bet_double > 0)
                    {
                     
                        string my_stage_str = f_get_stage_as_string(stage_index);

                        String my_previous_bet_payout_if_wins_str = f_get_payout_string(stage_index, xxx_HC_seat_index);

                        string my_class_str = String.Format("previous_placed_bet_for_any_stage previous_placed_bet_for_{0}_stage", my_stage_str);
                        writer.WriteLine(String.Format("<div id='h{0}_previous_bets' class='{1}' >", _hand_index, my_class_str));
                        writer.Indent += 1;

                        // If the hand is dead only show how much the player has bet not how much they could have won.
                        if (p_is_dead){
                            writer.WriteLine("<span class='lost_bet'>");
                            writer.WriteLine(String.Format("{0}: £{1} bet", my_stage_str, my_bet_double, my_previous_bet_payout_if_wins_str));
                            writer.WriteLine("</span>");
                        }
                        else
                        {
                            // The hand is still alive so display the bet and payout value
                            writer.WriteLine(String.Format("{0} £{1} bet pays {2}", my_stage_str, my_bet_double, my_previous_bet_payout_if_wins_str));
                            
                            // Give the player the opportunity to cancel current bets for any bets place on current stage (before next stage is dealt)
                            if (p_current_betting_stage == stage_index)
                            {
                                writer.WriteLine(String.Format("<div id='s{0}h{1}p{2}_cancel_bet' class='cancel_bet_button' onclick='javascript:f_cancel_bet(" + _hand_index + ");'>CANCEL</div>", my_stage_str, p_hand_index, 0));
                            }
                            else
                            {
                                writer.WriteLine(String.Format("<div id='s{0}h{1}p{2}_secured_bet' class='secured_bet_button'>Secured</div>", my_stage_str, p_hand_index, 0));
                            
                            }
                        } 
                        
                        writer.WriteLine("");
                        writer.Indent -= 1;
                        writer.WriteLine("</div>");
                    }
                }

                // Close the opening 'container' DIV for all previous bets 
                writer.Indent -= 1;
                writer.WriteLine("</div>");
                writer.WriteLine("");

            }
        }

        private void f_render_status_icons(HtmlTextWriter writer)
        {
            #region Generate the HTML that shows the hand's status icons
            writer.WriteLine("");
            writer.WriteLine("<div id='status_icons_container' onclick='javascript:f_placebet(" + _hand_index + ");'>");

            // Can't lose Indicator
            #region display_for_cant_lose_scenario
            if (_is_dead_cert && !_is_winner && !_is_dead)
            {
                writer.WriteLine("<!-- Can't loose icon -->");
                writer.WriteLine("<div id='h" + _hand_index + "icl' class='hand_status_icon icon_cant_lose'>");
                writer.Indent += 1;
                writer.AddAttribute(HtmlTextWriterAttribute.Src, "resources/icons/icon_cant_lose1.png");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.WriteLine("");
                writer.Indent -= 1;
                writer.WriteLine("</div>");
                writer.WriteLine("");

            }
            #endregion display_for_cant_lose_scenario

            // Favourite Indicator
            #region _is_favourite
            if (_is_favourite && !_is_winner && !_is_dead)
            {
                writer.WriteLine("<!-- Favourite icon -->");
                //writer.WriteLine("<div id='h" + _hand_index + "ifav' class='icon_favourite'>");
                writer.WriteLine("<div id='h" + _hand_index + "ifav' class='hand_status_icon icon_favourite'>");
                writer.Indent += 1;
                writer.AddAttribute(HtmlTextWriterAttribute.Src, "resources/icons/icon_favourite5.png");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.WriteLine("");
                writer.Indent -= 1;
                writer.WriteLine("</div>");
                writer.WriteLine("");
            }
            #endregion _is_favourite

            // Can't is Dead Indicator
            #region _is_best_value
            if (_is_best_value && !_is_winner && !_is_dead)
            {
                writer.WriteLine("<!-- Best Value Indicator icon -->");
                writer.WriteLine("<div id='h" + _hand_index + "ibv' class='hand_status_icon icon_best_value'>");
                writer.Indent += 1;
                writer.AddAttribute(HtmlTextWriterAttribute.Src, "resources/icons/icon_best_value1.png");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.WriteLine("");
                writer.Indent -= 1;
                writer.WriteLine("</div>");
                writer.WriteLine("");
            }
            #endregion _is_best_value

            writer.WriteLine("");
            writer.WriteLine("</div>");
            #endregion status_icons
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        private void f_render_hand_description(HtmlTextWriter writer)
        {
            if (!_is_dead) {  
                writer.WriteLine("");
                writer.Write(String.Format("<div id='payoutdescription_h{0}' class='payoutdescription'> {1}</div>",
                    _hand_index,
                    _description));
                writer.Indent -= 1;
                
                writer.WriteLine("");
                writer.Write(String.Format("<div id='payout_value_h{0}' class='payoutvalue'> {1}</div>",
                    _hand_index,
                    p_payout_string));
                writer.Indent -= 1;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        private void f_render_payoutvalue(HtmlTextWriter writer)
        {
            if (!_is_dead)
            {
                writer.WriteLine("");
                writer.Write(String.Format("<div id='payout_value_h{0}' class='payoutvalue'> {1}</div>",
                    _hand_index,
                    p_payout_string));
                writer.Indent -= 1;
            }
        }

        private String f_generate_onclick_code()
        {
            String my_onclick_event;

            if (_current_betting_stage == enum_betting_stage.NON_BETTING_STAGE || p_is_dead || p_is_dead_cert || p_is_winner || !p_is_in_play)
            {
                my_onclick_event = "";
            }
            else
            {
                my_onclick_event = (String.Format("onclick='javascript:f_placebet({0});'",_hand_index));
            }

            return my_onclick_event;
        }
            

        protected override void RenderContents(HtmlTextWriter writer)
        {
           

            #region Generate HTML for Hand Panel container opening DIV
            writer.Indent += 1;
            writer.WriteLine("");
            writer.WriteLine("");
            writer.WriteLine(String.Format("<!-- ### Hand {0} ###-->", _hand_index));
            writer.WriteLine(String.Format("<div id='hedgeem_control_hand_panel_hand_{0}' class='hedgeem_control_hand_panel' {1}>", _hand_index, f_generate_onclick_code()));
            writer.Indent += 1;
            #endregion
            
            f_render_cards(writer);
            
            #region Draw the components of the hand panel for when the hand is 'Dead'
            if (_is_dead)
            {
                writer.WriteLine("<!-- Dead Hand Indicator icon -->");
                writer.WriteLine("<div id='h" + _hand_index + "idead' class='icon_dead'>");
                writer.Indent += 1;
                writer.WriteLine("");
                writer.Indent -= 1;
                writer.WriteLine("</div>");
                writer.WriteLine("");
            }
            #endregion

            f_render_status_icons(writer);
            f_render_previous_placed_bets(writer);
            
            #region Generate the HTML that indicates the hand is a winner (if it is).
            if (_is_winner)
            {
                writer.WriteLine("<!-- Winner icon -->");
                writer.WriteLine("<div id='h" + _hand_index + "iwin' class='icon_winner'>");
                // Commented out but Simon 9th Aug 2014 as image presented via css
                //writer.Indent += 1;
                //writer.AddAttribute(HtmlTextWriterAttribute.Src, "../resources/icons/icon_winning_hand_indicator1.png");
                //writer.RenderBeginTag(HtmlTextWriterTag.Img);
                //writer.RenderEndTag();
                ///writer.WriteLine("");
                //writer.Indent -= 1;
                writer.WriteLine("</div>");
                writer.WriteLine("");
            }
            #endregion _is_winner

            #region Generate the HTML that shows the 'Hand Description' Block
            if (_show_description)
            {
                #region Generate the HTML to display the hand description container opening DIVS
                writer.WriteLine("");
                writer.WriteLine("<!-- Hand " + _hand_index + ", Hand Panel -->");
                writer.WriteLine("<div id='h" + _hand_index + "_payout' onclick='javascript:f_placebet(" + _hand_index + ");' class='hedgeem_control_hand_panel_info_background'>");
                writer.Indent += 1;
                #endregion

                f_render_hand_description(writer);
                //f_render_payoutvalue(writer);
                    
                #region Generate the HTML to display the hand description container closing DIVS
                writer.WriteLine("");
                writer.WriteLine("</div>");

                base.RenderContents(writer);
                writer.Indent -= 1;
                #endregion
            }
            #endregion if_show_description
                     
            #region Generate HTML for Hand Panel container closing DIV
            writer.WriteLine("</div>");
            writer.Indent -= 1;
            #endregion
        }


        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }
    }

