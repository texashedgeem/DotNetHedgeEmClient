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
    public class hedgeem_control_last_game_dump : WebControl
    {

        private String _bg_image;
        

        /// <summary>
        /// 
        /// </summary>
        public String p_game_dump_as_string
        {
            get { return _game_dump_as_string; }
            set
            {
                _game_dump_as_string = value;

            }
        }
        private String _game_dump_as_string;
        
        

        protected override void RenderContents(HtmlTextWriter writer)
        {
            
            writer.WriteLine("");
            writer.Indent += 1;
            writer.WriteLine(String.Format("<!-- Game Dump -->"));
            writer.Indent += 1;
            writer.WriteLine("<div id='hedgeem_last_game_dump'>");
            writer.Indent += 1;

            // For Jackpot balance
            writer.WriteLine(String.Format("<div id ='hedgeem_last_game_dump_string' class='last_game_dump_string'>"));
            writer.WriteLine("");
            writer.Indent += 1;
            writer.WriteLine(p_game_dump_as_string);
            writer.WriteLine("");
            writer.Indent -= 1;
            writer.WriteLine("</div>");
            writer.WriteLine("");


            

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





