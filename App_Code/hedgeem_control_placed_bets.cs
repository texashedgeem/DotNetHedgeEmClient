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
//using HedgeEmWinGUI;
using System.Resources;

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:hedgeem_control_placed_bets runat=server></{0}:hedgeem_control_placed_bets>")]
    public class hedgeem_control_placed_bets : WebControl
    {

        private String _bg_image;
        private Double _placed_bets;

        public Double p_placed_bets
        {
            get { return _placed_bets; }
            set
            {
                _placed_bets = value;

            }
        }
        
        

        protected override void RenderContents(HtmlTextWriter writer)
        {

            if (p_placed_bets > 0)
            {
                writer.WriteLine("");
                writer.Indent += 1;
                writer.WriteLine(String.Format("<!-- Placed Bets -->"));
                writer.Indent += 1;
                writer.WriteLine("<div id='placed_bets_container'>");
                writer.Indent += 1;

                // For PLaced bets total 
                writer.WriteLine(String.Format("<div id ='placed_bets' class='displayed_currency'>"));
                writer.WriteLine("");
                writer.Indent += 1;
                writer.WriteLine(String.Format("Total bets £{0}",p_placed_bets));
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
        }
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }
        

    }

