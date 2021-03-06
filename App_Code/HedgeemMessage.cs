﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

/// <summary>
/// Summary description for HedgeemMessage
/// </summary>
 [DefaultProperty("Text")]
public class HedgeemMessage:WebControl
{
   
     /// <summary>
        /// Detailed message (Typically only show to administrators. 
        /// </summary>
        public String p_detailed_message_str
        {
            get { return _detailed_message_str; }
            set
            {
                _detailed_message_str = value;

            }
        }
        private String _detailed_message_str;


        public bool p_is_visible
        {
            get { return _is_visible; }
            set
            {
                _is_visible = value;

            }
        }
        private bool _is_visible = false;


        /// <summary>
        /// Summary message intended for end user. Does not contain detailed technical information or stack traces.
        /// Example Message.  "Sorry something has gone wrong with this operation.  The HedgeEm administrators have been notified."
        /// </summary>
        public String p_user_message_str
        {
            get { return _user_message_str; }
            set
            {
                _user_message_str = value;

            }
        }
        private String _user_message_str = "";




        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (p_is_visible)
            {
                writer.WriteLine("");
                writer.Indent += 1;
                writer.WriteLine(String.Format("<!-- Pop-up Message -->"));
                writer.Indent += 1;
                writer.WriteLine("<div id='popup_message_container'>");
                writer.Indent += 1;
                writer.WriteLine(String.Format("<div id ='hide_login' class='close' onclick='onclose()' >"));
                writer.WriteLine(String.Format("<span aria-hidden='true'>×</span>"));
                writer.WriteLine("</div>");
                // For User message
                writer.WriteLine(String.Format("<div id ='user_message_string' onclick='Facebook_Invite_Friends()'>"));
                writer.WriteLine("");
                writer.Indent += 1;
                writer.WriteLine(p_user_message_str);
                writer.WriteLine("");
                writer.Indent -= 1;
                writer.WriteLine("</div>");
                writer.WriteLine("");


                // For Detailed message
                writer.WriteLine(String.Format("<div id ='detailed_message_string' >"));
                writer.WriteLine("");
                writer.Indent += 1;
                writer.WriteLine(p_detailed_message_str);
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
        }
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }
    
}