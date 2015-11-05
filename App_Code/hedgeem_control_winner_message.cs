using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
//using HedgeEmWinGUI;

[DefaultProperty("Text")]
    /* xxx Need to delete this cut and past after testing. [ToolboxData("<{0}:hedgeem_control_seat runat=server></{0}:hedgeem_control_seat>")]*/
    public class hedgeem_control_winner_message : WebControl
    {

        private String _winner_message_str;

        public String p_winner_message_str
        {
            get { return _winner_message_str; }
            set
            {
                _winner_message_str = value;

            }
        }
        
        

        protected override void RenderContents(HtmlTextWriter writer)
        {
            
            writer.WriteLine("");
            writer.Indent += 1;
            writer.WriteLine(String.Format("<!-- Winner Message -->"));
            writer.Indent += 1;
            writer.WriteLine("<div id='winner_message_container'>");
            writer.Indent += 1;

            // For Jackpot balance
            writer.WriteLine(String.Format("<div id ='winner_message_string' >"));
            writer.WriteLine("");
            writer.Indent += 1;
            writer.WriteLine(p_winner_message_str);
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

