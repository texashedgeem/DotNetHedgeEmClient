using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Resources;

using HedgeEmClient;

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:hedgeem_control_seat runat=server></{0}:hedgeem_control_seat>")]
    public class hedgeem_control_seats : WebControl
    {

        private String _bg_image;
        private String _photo_image;
        private String _player_name;
        private String _chip_icon;
        private String _balance;
        private Double _balance_double;
        private Int32 _seat_index;
        private int _player_id;
        private int _number_of_hands;

        private String _back_color;
        bool _show_bets_info = false;
        private List<HedgeEmHandPayout> _hand_payout_list = new List<HedgeEmHandPayout>();
        
        public void f_set_payout_for_hand(int a_hand_index){
            for (int hand_index = 0; hand_index < _number_of_hands; hand_index++)
            {
                HedgeEmHandPayout my_seat = new HedgeEmHandPayout();
                //f_add_hedgeem_seat_to_list(my_seat);
            }
        }

        public void f_add_hedgeem_hand_payout_to_list(HedgeEmHandPayout a_hedgeem_hand_payout)
        {
            // Record the odds
            _hand_payout_list.Add(a_hedgeem_hand_payout);
        }


        public bool p_show_bets_info
        {
            get { return _show_bets_info; }
            set
            {
                _show_bets_info = value;

            }
        }

        public String p_bg_image
        {
            get { return _bg_image; }
            set
            {
                _bg_image = value;

            }
        }
        public String p_photo_image
        {
            get { return _photo_image; }
            set
            {
                _photo_image = value;

            }
        }
        public String p_player_name
        {
            get { return _player_name; }
            set
            {
                _player_name = value;

            }
        }
        public String p_chip_icon
        {
            get { return _chip_icon; }
            set
            {
                _chip_icon = value;

            }
        }
        public String p_balance
        {
            get {
                return _balance; }
            set
            {
                _balance = value;

            }
        }
        public Double p_balance_double
        {
            get { return _balance_double; }
            set
            {
                _balance_double = value;

            }
        }
        public int p_seat_index
        {
            get { return _seat_index; }
            set
            {
                _seat_index = value;
            }
        }



        string _total_bet_label = "Total Bet";
        public string p_total_bet_label
        {
            get { return _total_bet_label; }
            set
            {
                _total_bet_label = value;
            }
        }


        double _total_bet_amount = 666;
        public double p_total_bet_amount
        {
            get { return _total_bet_amount; }
            set
            {
                _total_bet_amount = value;
            }
        }

        string _hand_bet_label = "Hand 0" ;
        public string p_hand_bet_label
        {
            get { return _hand_bet_label; }
            set
            {
                _hand_bet_label = value;
            }
        }

        double _hand_bet_value = 18;
        public double p_hand_bet_value
        {
            get { return _hand_bet_value; }
            set
            {
                _hand_bet_value = value;
            }
        }

        string _payout_label = "Secured Payouts";
        public string p_payout_label
        {
            get { return _payout_label; }
            set
            {
                _payout_label = value;
            }
        }

        string _payout = "Payout not set";
        public string p_payout
        {
            get { return _payout; }
            set
            {
                _payout = value;
            }
        }


        public int p_player_id
        {
            get { return _player_id; }
            set
            {
                _player_id = value;

            }

        }
        public String p_back_color
        {
            get { return _back_color; }
            set
            {
                _back_color = value;

            }
        }


        protected override void RenderContents(HtmlTextWriter writer)
        {
            String player_image_filename = f_get_player_resource_name(_photo_image);

            writer.WriteLine("");
            writer.Indent += 1;
            writer.WriteLine(String.Format("<!-- Seat [{0}], Player [{1}] -->", _seat_index, _player_name));
            writer.Indent += 1;
            writer.WriteLine(String.Format("<div id='hedgeem_seat_container_{0}' onclick='javascript:f_get_player_id({0},{1})' class='hedgeem_seat_container'>", p_seat_index, p_player_id));
            writer.Indent += 1;

            // For Player_Name
            writer.WriteLine(String.Format("<DIV id ='player_{0}' class='player_name'>", _player_name));
            writer.WriteLine("");
            writer.Indent += 1;
            writer.WriteLine(_player_name);
            writer.WriteLine("");
            writer.Indent -= 1;
            writer.WriteLine("</DIV>");
            writer.WriteLine("");


            // For Player_Image
            writer.WriteLine(String.Format("<DIV id ='player_avitar_{0}' class='player_image'>", "xxx_should_be_seat_id"));
            writer.Indent += 1;
            writer.WriteLine(String.Format("<img src='{0}'>", player_image_filename));
            writer.Indent -= 1;
            writer.WriteLine("</DIV>");


            //For Chip_Image_Icon
            writer.WriteLine(String.Format("<DIV id ='player_chip_icon_{0}' class='chip_image' >", "xxx_should_be_seat_id"));
            writer.Indent += 1;
            writer.WriteLine(String.Format("<img src='{0}'>", _chip_icon));
            writer.Indent -= 1;
            writer.WriteLine("</DIV>");


            // For Player_Balance
            writer.WriteLine(String.Format("<DIV id ='player_balance_{0}' class='player_balance'>", _player_id));
            writer.Indent += 1;
            writer.WriteLine(_balance);
            writer.Indent -= 1;
            writer.WriteLine("</DIV>");

            // In some Themes/Skins you may want to show information about the bets placed by a player; particulary if 
            // you are displaying a multi-seated table. If you do then render the following html.
            if (_show_bets_info)
            {
                // For Player_Payout
                writer.WriteLine(String.Format("<DIV id ='payout_container{0}' class='player_payout_container'>", _seat_index));
                writer.Indent += 1;

                writer.WriteLine(String.Format("<DIV id ='total_payout_container{0}' class='hand_payout_total_container'>", _seat_index));
                writer.Indent += 1;

                    // Total bet label
                    writer.WriteLine(String.Format("<DIV id ='total_bet{0}' class='total_bet_label'>", _player_id));
                    writer.Indent += 1;
                    writer.WriteLine(p_total_bet_label);
                    writer.Indent -= 1;
                    writer.WriteLine("</DIV>");

                    // Total bet value
                    writer.WriteLine(String.Format("<DIV id ='total_bet_amount{0}' class='total_bet_amount'>", "xxx_should_be_seat_id"));
                    writer.Indent += 1;
                    writer.WriteLine(p_total_bet_amount.ToString());
                    writer.Indent -= 1;
                    writer.WriteLine("</DIV>");

                writer.Indent -= 1;
                writer.WriteLine("</DIV>");

                // Payout value
                if (true){
                    writer.WriteLine(String.Format("<DIV id ='hand_payouts_container{0}' class='hand_payouts_container'>", _player_id));
                    writer.Indent += 1;
                    foreach (HedgeEmHandPayout my_hand_payout in _hand_payout_list)
                    {
                        if (my_hand_payout.payout_value > 0)
                        {
                            writer.WriteLine(String.Format("<DIV id ='payhand_container{0}' class='hand_payout_container'>", my_hand_payout.hand_index));
                            writer.Indent += 1;
                            
                                writer.WriteLine(String.Format("<DIV id ='payhand_label{0}' class='hand_payout_label'>", my_hand_payout.hand_index));
                                writer.Indent += 1;
                                writer.WriteLine("Hand " + (my_hand_payout.hand_index + 1) + ": ");
                                writer.Indent -= 1;
                                writer.WriteLine("</DIV>");

                                writer.WriteLine(String.Format("<DIV id ='payhand{0}' class='hand_payout_value'>", my_hand_payout.hand_index));
                                writer.Indent += 1;
                                writer.WriteLine(p_payout);
                                writer.Indent -= 1;
                                writer.WriteLine("</DIV>");
                            writer.Indent -= 1;
                            writer.WriteLine("</DIV>");

                        }
                    }
                    writer.Indent -= 1;
                    writer.WriteLine("</DIV>");

                }
                    
                //for (int hand_index = 0; hand_index < 4; hand_index ++ )
                      
                writer.Indent -= 1; // Close payout container indent
                writer.WriteLine("");
                writer.WriteLine("</div>");
            }

            writer.Indent -= 1; // Close container indent
            writer.WriteLine("");
            writer.WriteLine("</div>");
        
            writer.WriteLine("");

            base.RenderContents(writer);
        }
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }
        private String f_get_player_resource_name(String a_player_name_string)
        {

            String player_resource_name = a_player_name_string;

            // Return the image if you can find
            return "../resources/" + player_resource_name + ".jpg";
        }

    }


