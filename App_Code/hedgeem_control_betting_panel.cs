using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data; // For DataTable

using HedgeEmClient;
using System.Resources; // So I can use the resource manager to retrieve image resources programatically

using System.Runtime.InteropServices;

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:hedgeem_control_betting_panel runat=server></{0}:hedgeem_control_betting_panel>")]

    /// <summary>
    /// 
    /// The custom control is responsible for generating a block of HTML that represents a Hedge'Em betting panel.
    /// The html it produces is dynamic and is generated based on the value of the class properties.
    /// Example outputs are provided below.
    /// 
    /// EG 1. A betting panel whose current offered odds is 20:1, 
    ///       Player 1 has placed a £2 bet on hand 1 and the FLOP stage.
    ///       PLayer 4 has placed a £5 bet on hand 1 at the HOLE stage, and a further £10 bet on hand 2; No other players have placed a bet.
    ///       PLayer 4 has placed a £10 bet on hand 2 at the FLOP stage, and a further £10 bet on hand 2; No other players have placed a bet.
    ///       
    /// <DIV ID="betting_panel_container_hand1_stage_FLOP">
    ///     <DIV ID="betting_panel_offered_odds_hand1_stage_FLOP">  20</DIV>
    ///     <DIV ID="betting_panel_player_1_bets_hand1_stage_FLOP">  2</DIV>
    ///     <DIV ID="betting_panel_player_4_bets_hand1_stage_HOLE">  5</DIV>
    ///     <DIV ID="betting_panel_player_4_bets_hand2_stage_FLOP"> 10</DIV>
    /// </DIV>    
    /// 
    /// 
    /// </summary>
    public class BETTING_PANEL : WebControl
    {
        float _width_scaling_factor = 1F;
        float _height_scaling_factor = 1F;
        bool _is_visible = true;
        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <summary>
        /// Constructor
        /// </summary>
        public BETTING_PANEL(int number_of_players)
        {
            _players_bets = new Double[number_of_players];
            _number_of_players = number_of_players;
        }

        /// <summary>
        /// The betting panel (area on a Hedge'Em (TM) table) where players place their bets for any given hand/stage combination
        /// can show the current offered odds for hand/stage.  This varible defines whether or not this is shown.
        /// </summary>
        public bool p_show_offered_odds
        {
            set { _show_offered_odds = value; }
            get { return _show_offered_odds; }
        }
        private bool _show_offered_odds = true;
        /// <summary>
        /// _odd_margin_rounded: The '_odds_margin' calculated above is often not an whole number or a number that is easy
        /// for players to relate to when dealing with odds.  Because of this we round the odds.  
        /// For example 1:9.5 may become 1:9   1:666 may be rounded to 1:500
        /// </summary>
        public double p_odd_margin_rounded
        {
            set { _odd_margin_rounded = value; }
            get { return _odd_margin_rounded; }
        }
        private double _odd_margin_rounded;
        /// <summary>
        /// xxx see also enum_hand_in_play_status - as they may be redundant code as I have tried this twice on to occassions and
        /// forgot about the former - enum_hand_in_play_status - when i did this - enum_betting_panel_display_state- on the 1st feb
        /// this is currently not used but should be.
        /// 
        /// </summary>
        public enum_hand_in_play_status p_enum_panel_display_status
        {
            set { _enum_panel_display_status = value; }
            get { return _enum_panel_display_status; }
        }
        private enum_hand_in_play_status _enum_panel_display_status = enum_hand_in_play_status.UNDEFINED;
        /// <summary>
        /// _odds_percent_win: A label to show the actual measured percentage chance a hand has of Winning.
        /// This is before house margin and rounding is considered.
        /// (Eg before the river card is dealt there may be 36 cards left to deal, if only 9 of these will
        /// make a hand win then _odds_percent_win will be set to 9/36 = 0.25  (25%)
        /// /// </summary>
        public String p_odds_percent_win
        {
            set { _odds_percent_win = value; }
            get { return _odds_percent_win; }
        }
        private String _odds_percent_win;
        /// <summary>
        /// _odds_percent_draw: A label to show the actual measured percentage chance a hand has of Drawing.
        /// This is before house margin and rounding is considered.
        /// (Eg before the river card is dealt there may be 36 cards left to deal, if 18 of these will
        /// result in a draw then _odds_percent_draw will be set to 18/36 = 0.5  (50%)
        /// /// </summary>
        public String p_odds_percent_draw
        {
            set { _odds_percent_draw = value; }
            get { return _odds_percent_draw; }
        }
        private String _odds_percent_draw;
        /// <summary>
        /// _odds_percent_win_or_draw: A label to show the actual measured percentage chance a hand has of Winning OR Drawing.
        /// This is before house margin and rounding is considered.
        /// (Eg before the river card is dealt there may be 36 cards left to deal, if 6 of these will
        /// result in a a Win and 6 will result in a Draw then _odds_percent_draw will be set to (6+6)/36 = 0.33  (33%)
        /// /// </summary>
        public String p_odds_percent_win_or_draw
        {
            set { _odds_percent_win_or_draw = value; }
            get { return _odds_percent_win_or_draw; }
        }
        private String _odds_percent_win_or_draw;
        /// <summary>
        /// _odds_raw: This is _odds_percent_win_or_draw expressed in odds format.
        /// It represents the odds that should be offered to the players if the want to bet on a hand an recieve a payout 
        /// calculated on these odds if the hand wins.
        /// /// </summary>
        public String p_odds_actual
        {
            set { _odds_actual = value; }
            get { return _odds_actual; }
        }
        private String _odds_actual;
        /// <summary>
        /// _odds_margin: This is _odds_raw * a percentage (where the percentage is to give the house an advantage).  
        /// E.g. If the house calculates the odds of 1:10 (representing that it should pay £10 for every £1 bet) it may decide 
        /// to reduce its payout by 5% and only payout £9.50.  
        /// So in this example _odds_margin = 10 * 0.05 = 1:9.5
        /// </summary>
        public String p_odds_margin
        {
            set { _odds_margin = value; }
            get { return _odds_margin; }
        }
        private String _odds_margin;
        /// <summary>
        /// p_odds_presented_margin:  This represents the difference (as a percentage) between what the player is actually offered
        /// in odds versus what they should be getting.  
        /// For example.  If the actual odd of an event occuring was 1:10 and the house introduced a 5% advantage to bring this 
        /// down to 1:9.5 and then rounded to 1:9.  Then the actual margin would be 1 - 9/10 = 0.1 = 10%.
        /// </summary>
        public double p_odds_presented_margin
        {
            set { _odds_presented_margin = value; }
            get { return _odds_presented_margin; }
        }
        public Double _odds_presented_margin;
        int _number_of_players = 0;

        public bool p_show_admin_info
        {
            set { _show_admin_info = value; }
            get { return _show_admin_info; }
        }
        public bool _show_admin_info = false;
        /// <summary>
        /// An array of all players bets for this specific hand and this specific stage.
        /// e.g. if the stage of the game (as recorded in this class was FLOP
        /// </summary>
        public double[] p_players_bets
        {
            set { _players_bets = value; }
            get { return _players_bets; }
        }
        private double[] _players_bets;

        public double p_players_betss
        {
            set { _players_betss = value; }
            get { return _players_betss; }
        }
        private double _players_betss;


        public string p_visible
        {
            set { _visible = value; }
            get { return _visible; }

        }
        private string _visible;


        public enum_hand_in_play_status p_panel_display_status
        {
            set { _enum_panel_display_status = value; }
            get { return _enum_panel_display_status; }
        }
        public enum_betting_stage p_enum_betting_stage
        {
            set { _enum_betting_stage = value; }
            get { return _enum_betting_stage; }
        }
        private enum_betting_stage _enum_betting_stage = enum_betting_stage.NON_BETTING_STAGE;
        public enum_theme p_enum_theme
        {
            set { _enum_theme = value; }
            get { return _enum_theme; }
        }
        private enum_theme _enum_theme = enum_theme.CASINO_TABLE;
        public int p_hand_index
        {
            set { _hand_index = value; }
            get { return _hand_index; }
        }
        private int _hand_index;
        public string p_betting_panel_image_background
        {
            set { _betting_panel_image_background = value; }
            get { return _betting_panel_image_background; }
        }

        private string _betting_panel_image_background;

        public string p_betting_panel_chip_background
        {
            set { _betting_panel_chip_background = value; }
            get { return _betting_panel_chip_background; }
        }

        private string _betting_panel_chip_background;

        public string p_betting_panel_chip_value
        {
            set { _betting_panel_chip_value = value; }
            get { return _betting_panel_chip_value; }
        }

        private string _betting_panel_chip_value;

        public string p_betting_panel_value
        {
            set { _betting_panel_value = value; }
            get { return _betting_panel_value; }
        }

        private string _betting_panel_value;


        public string p_Odds_css_value
        {
            set { _Odds_css_value = value; }
            get { return _Odds_css_value; }
        }

        public string p_betting_stage_as_string
        {
            get
            {
                // Generate a human/player readable string that represent the current stage of the game (HOLE, FLOP, TURN or RIVER etc)
                string str_betting_stage = "";
                switch (_enum_betting_stage)
                {
                    case enum_betting_stage.HOLE_BETS:
                        str_betting_stage = "HOLE:";
                        break;
                    case enum_betting_stage.FLOP_BETS:
                        str_betting_stage = "FLOP:";
                        break;
                    case enum_betting_stage.TURN_BETS:
                        str_betting_stage = "TURN:";
                        break;

                    default:
                        break;
                }
                return str_betting_stage;
            }
        }

        private string _Odds_css_value;

        private void f_render_detailed_stats(HtmlTextWriter a_html_writer)
        {


            a_html_writer.WriteLine("");
            a_html_writer.WriteLine(String.Format("<!-- Betting Panel detailed stats: Stage[{0}], Hand [{1}]-->", p_Odds_css_value, _hand_index));
            a_html_writer.WriteLine("<div id='player_odds_bet'>");

            // Presented Odds (PO)
            String PO = String.Format("PO:{0}", p_odd_margin_rounded);
            a_html_writer.AddAttribute(HtmlTextWriterAttribute.Class, p_Odds_css_value, false);
            a_html_writer.AddStyleAttribute("COLOR", "Yellow");
            a_html_writer.RenderBeginTag("label");
            a_html_writer.Write(PO);
            a_html_writer.RenderEndTag();
            a_html_writer.WriteBreak();

            // Margin Odds (MO)
            String MO = String.Format("MO:{0}", p_odds_margin);
            a_html_writer.WriteLine("");
            a_html_writer.AddAttribute(HtmlTextWriterAttribute.Class, p_Odds_css_value, false);
            a_html_writer.AddStyleAttribute("COLOR", "Cyan");
            a_html_writer.RenderBeginTag("label");
            a_html_writer.Write(MO);
            a_html_writer.RenderEndTag();
            a_html_writer.WriteBreak();

            // Actual Odds  (AO)
            String AO = String.Format("AO:{0}", p_odds_actual);
            a_html_writer.WriteLine("");
            a_html_writer.AddAttribute(HtmlTextWriterAttribute.Class, p_Odds_css_value, false);
            a_html_writer.AddStyleAttribute("COLOR", "Pink");
            a_html_writer.RenderBeginTag("label");
            a_html_writer.Write(AO);
            a_html_writer.RenderEndTag();
            a_html_writer.WriteBreak();

            // Percentage Total (PC)
            String PC = String.Format("Pc:{0}", p_odds_percent_win_or_draw);
            a_html_writer.WriteLine("");
            a_html_writer.AddAttribute(HtmlTextWriterAttribute.Class, p_Odds_css_value, false);
            a_html_writer.AddStyleAttribute("COLOR", "Orange");
            a_html_writer.RenderBeginTag("label");
            a_html_writer.Write(PC);
            a_html_writer.RenderEndTag();
            a_html_writer.WriteBreak();

            String W = String.Format("W:{0}", p_odds_percent_win);
            // Percentage Win (W)
            a_html_writer.WriteLine("");
            a_html_writer.AddAttribute(HtmlTextWriterAttribute.Class, p_Odds_css_value, false);
            a_html_writer.AddStyleAttribute("COLOR", "Orange");
            a_html_writer.RenderBeginTag("label");
            a_html_writer.Write(W);
            a_html_writer.RenderEndTag();
            a_html_writer.WriteBreak();

            // Percentage Draw (D)
            String D = String.Format("D:{0}", p_odds_percent_draw);
            a_html_writer.WriteLine("");
            a_html_writer.AddAttribute(HtmlTextWriterAttribute.Class, p_Odds_css_value, false);
            a_html_writer.AddStyleAttribute("COLOR", "Orange");
            a_html_writer.RenderBeginTag("label");
            a_html_writer.Write(D);
            a_html_writer.RenderEndTag();


            a_html_writer.WriteLine("</div>");
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.WriteLine(String.Format("<div id='betpanel_hand{0}_stage{1}' class='generic_betting_panel_container generic_{1}_betting_panel_container' >", _hand_index, _enum_betting_stage.ToString()));
            writer.Indent += 1;
            #region show_admin
            if (_show_admin_info)
            {
                f_render_detailed_stats(writer);
            }
            #endregion show_admin

            #region if_casino


            //Live Test
            if (p_enum_theme == enum_theme.CASINO_TABLE)
            {

                int number_of_players = _number_of_players;
                #region for_each_player
                for (int player_index = 0; player_index < number_of_players; player_index++)
                {
                    double my_bet_double = p_players_bets[player_index];
                    // double my_bet_double = _players_betss;
                    string my_chip_id = String.Format("chip_{0}", player_index);
                    string chip_icon_resource_name = "chip_icon_" + player_index.ToString();

                    if (my_bet_double > 0)
                    {
                        String my_bet = String.Format("{0}", p_players_bets[player_index]);

                        // Show the Bet_Chip
                        writer.WriteLine(String.Format("<div id='chip_hand{0}_stage{1}' class='{2}' >", 
                            _hand_index, 
                            _enum_betting_stage.ToString(), 
                            chip_icon_resource_name)
                            );

                        writer.Indent += 1;
                            writer.WriteLine(String.Format("<div id='bet_value{0}' class='{1} bet_value'>", _hand_index, p_betting_panel_chip_value));
                            writer.Indent += 1;
                                writer.WriteLine(my_bet);
                            writer.Indent -= 1;
                            writer.WriteLine("</div>");
                        writer.Indent -= 1;
                            
                        writer.WriteLine("</div>");

                    }

                }
                #endregion for_each_player
            }
            else
            {
                int number_of_players = _number_of_players;
                //int number_of_players = 1;

                #region for_each_player2
                for (int player_index = 0; player_index < number_of_players; player_index++)
                {
                    // double my_bet_double = 1.0;
                    // double my_bet_double = _players_betss;
                    double my_bet_double = p_players_bets[player_index];
                    // p_players_bets[0] = 1.0;
                    string my_chip_id = String.Format("chip_{0}", player_index);
                    string chip_icon_resource_name = "chip_icon_seat_" + player_index.ToString();


                    if (my_bet_double > 0)
                    {
                      
                        Double my_payout_for_this_bet = (p_odd_margin_rounded * my_bet_double) + my_bet_double;
                        String my_previous_bet_payout_if_wins_str;
                        String my_bet_placed_on_hand_at_stage_str = String.Format("fff");
                        Double my_previous_bet_payout_if_wins = 0;

                        my_previous_bet_payout_if_wins = (p_players_bets[0] * p_odd_margin_rounded) + p_players_bets[0];
                        //
                        // Show payout for bet
                        //my_previous_bet_payout_if_wins_str = String.Format("£{0:#0.00} {1}", my_previous_bet_payout_if_wins, str_betting_stage);
                        // xxx NEED code here to change format ... If integer do not show .00
                        Double my_previous_bet_payout_if_wins_floor = Math.Floor(my_payout_for_this_bet);
                        Double my_is_int = my_payout_for_this_bet - my_previous_bet_payout_if_wins_floor;
                        if (p_odd_margin_rounded < 0)
                        {
                            my_previous_bet_payout_if_wins_str = String.Format(
                                "{0} £{1} bet pays £{2:#0.00} ",
                                p_betting_stage_as_string,
                                p_players_bets[0],
                                my_previous_bet_payout_if_wins
                                );
                        }
                        else
                        {
                            //my_previous_bet_payout_if_wins_str = String.Format("pays £{0:#0}", my_previous_bet_payout_if_wins, str_betting_stage);
                            my_previous_bet_payout_if_wins_str = String.Format(
                                "{0} £{1} bet pays £{2:#0} ",
                                p_betting_stage_as_string,
                                p_players_bets[0],
                                my_previous_bet_payout_if_wins);

                        }

                        //

                        //String my_bet = String.Format("P{0}: £{1}", player_index, p_players_bets[player_index].ToString());
                        my_payout_for_this_bet = (p_odd_margin_rounded * my_bet_double) + my_bet_double;
                        //String my_bet = String.Format("£{0} bet pays £{1}", p_players_bets[player_index].ToString(), my_payout_for_this_bet);
                        String my_bet = my_previous_bet_payout_if_wins_str;

                        string my_betting_panel_image_background = "../resources/backgrounds/hand_indicator_backgroud.png";

                        // Betting panel container
                        writer.WriteLine("");
                        writer.WriteLine("<div id='betting_panel_container_egg'  style='display:" + _visible + "'>");
                        writer.WriteLine("");
                        writer.Indent += 1;


                        writer.AddAttribute(HtmlTextWriterAttribute.Class, _betting_panel_image_background, false);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, my_betting_panel_image_background);
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);
                        writer.RenderEndTag();



                        writer.WriteLine("<!-- Players bets -->");
                        writer.WriteLine("<div id='player_z_bet'>");
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, _betting_panel_value, false);
                        writer.RenderBeginTag("label");
                        writer.Write(my_bet);
                        writer.RenderEndTag();
                        writer.WriteLine("</div>");


                        writer.Indent -= 1;
                        writer.WriteLine("</div>");
                    }
                    else
                    {
                        break;
                    }
                    writer.WriteLine("</div>");
                }
                #endregion for_each_player2
            }
            #endregion 
            writer.Indent -= 1;
            
            writer.WriteLine("</div>");
            // base.RenderContents(writer);
        }
    }
   

