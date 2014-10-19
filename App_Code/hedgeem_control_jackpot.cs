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
using System.IO;
//using HedgeEmWinGUI;
using System.Resources;

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:hedgeem_control_seat runat=server></{0}:hedgeem_control_seat>")]
    public class hedgeem_control_jackpot : WebControl
    {

        private String _bg_image;
        private Double _jackpot_balance;
        
        public Double p_jackpot_balance
        {
            get { return _jackpot_balance; }
            set
            {
                _jackpot_balance = value;

            }
        }
        
        

        protected override void RenderContents(HtmlTextWriter writer)
        {
            
            writer.WriteLine("");
            writer.Indent += 1;
            writer.WriteLine(String.Format("<!-- Jackpot -->"));
            writer.Indent += 1;
            writer.WriteLine("<div id='hedgeem_jackpot'>");
            writer.Indent += 1;

            // For Jackpot balance
            writer.WriteLine(String.Format("<div id ='jackpot_balance' class='displayed_currency'>"));
            writer.WriteLine("");
            writer.Indent += 1;
            writer.WriteLine(p_jackpot_balance);
            writer.WriteLine("");
            writer.Indent -= 1;
            writer.WriteLine("</div>");
            writer.WriteLine("");


            // For Player_Image
            /*
            writer.WriteLine(String.Format("<DIV id ='player_avitar_{0}' class='player_image'>", "xxx_should_be_seat_id"));
            writer.Indent += 1;
            writer.WriteLine(String.Format("<img src='{0}'>", player_image_filename));
            writer.Indent -= 1;
            writer.WriteLine("</DIV>");
            */

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
        

    }





