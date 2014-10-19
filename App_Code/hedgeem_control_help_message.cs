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
using HedgeEmClient;

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:hedgeem_control_seat runat=server></{0}:hedgeem_control_seat>")]
    public class hedgeem_control_help_message : WebControl
    {

        
        /// <summary>
        /// 
        /// </summary>
        public String p_help_title
        {
            get { return _help_title; }
            set
            {
                _help_title = value;

            }
        }
        private String _help_title;
        
        /// <summary>
        /// 
        /// </summary>
        public Double p_help_description
        {
            get { return _help_description; }
            set
            {
                _help_description = value;

            }
        }
        private Double _help_description;

        /// <summary>
        /// 
        /// </summary>
        public enum_game_state p_enum_game_state
        {
            set { _enum_game_state = value; }
            get { return _enum_game_state; }
        }
        private enum_game_state _enum_game_state;
        

        protected override void RenderContents(HtmlTextWriter writer)
        {
            
            writer.WriteLine("");
            // Help Container
            writer.WriteLine(String.Format("<!-- Help -->"));
            writer.WriteLine("<div id='help_container'>");
                writer.Indent += 1;

                // Help Title
                writer.WriteLine(String.Format("<div id ='help_title' class='help_title'>"));
                writer.WriteLine("");
                    writer.Indent += 1;
                    writer.WriteLine(p_help_title);
                    writer.WriteLine("");
                    writer.Indent -= 1;
                writer.WriteLine("</div>");
                writer.WriteLine("");

                // Help Description
                writer.WriteLine(String.Format("<div id ='help_description' class='help_description'>"));
                writer.WriteLine("");
                    writer.Indent += 1;
                    writer.WriteLine(p_help_description);
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





