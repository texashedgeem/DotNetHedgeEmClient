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
    [ToolboxData("<{0}:hedgeem_card_control runat=server></{0}:hedgeem_card_control>")]
    public class hedgeem_control_card : WebControl
    {


        private float _scaling_factor;
        private String _card_two_character_string;
        private Boolean _is_dead;
        private Boolean _is_face_up;
        private enum_card_position _enum_card_position;
        static string HC_str_to_indicate_card_should_be_shown_face_down = "ZZ";

        /// <summary>
        /// Default constructor
        /// </summary>
        public hedgeem_control_card()
        {
            p_scaling_factor = 1F;
            p_card_two_character_string = "";
            p_is_dead = false;
            p_is_face_up = true;
            p_enum_card_position = enum_card_position.MIDDLE;

        }

        private String _card_as_short_string;

        private String _cssclass;
        /// <summary>
        /// 
        /// </summary>
        public String p_card_as_short_string
        {
            get { return _card_as_short_string; }
            set
            {
                _card_as_short_string = value;
                // Invalidate();
            }
        }
        public String p_cssclass
        {
            get { return _cssclass; }
            set
            {
                _cssclass = value;
                // Invalidate();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public enum_card_position p_enum_card_position
        {
            get { return _enum_card_position; }
            set { _enum_card_position = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String p_card_two_character_string
        {
            get { return _card_two_character_string; }
            set { _card_two_character_string = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public float p_scaling_factor
        {
            get { return _scaling_factor; }
            set { _scaling_factor = value; }
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
        public Boolean p_is_face_up
        {
            get { return _is_face_up; }
            set { _is_face_up = value; }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {

            writer.Indent += 1;
            String my_card_image_filename = f_get_card_resource_name(_card_as_short_string, p_enum_card_position);
            writer.WriteLine("");
            writer.WriteLine("<!-- Card " + p_card_as_short_string + " -->");
            //writer.WriteLine("<div class='card_left_flop'>");
            writer.WriteLine(String.Format("<div id='card_{0}' class='{1}'>", p_card_as_short_string, p_cssclass));
            writer.Indent += 1;
            writer.AddAttribute(HtmlTextWriterAttribute.Src, my_card_image_filename);
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.Indent -= 1;
            writer.WriteLine("");
            writer.WriteLine("</div>");
            writer.WriteLine("");
            base.RenderContents(writer);
            writer.Indent -= 1;

        // unfinished development - remove block commenting to see what it does
        /*
            // See http://css3.bradshawenterprises.com/flip/
            writer.Indent += 1;
            writer.WriteLine("");
            writer.WriteLine("<!-- Hidden facedown card to support card flipping animation through css -->");
            writer.WriteLine(String.Format("<div class='back face center'>"));
        writer.WriteLine(String.Format("<div id='card_{0}' class='{1}'>", p_card_as_short_string, p_cssclass));
        writer.Indent += 1;
        writer.AddAttribute(HtmlTextWriterAttribute.Src, my_card_image_filename);
        writer.RenderBeginTag(HtmlTextWriterTag.Img);
        writer.RenderEndTag();
        writer.Indent -= 1;
        writer.WriteLine("");
        writer.WriteLine("</div>");
            writer.WriteLine("");
            writer.WriteLine("</div>");
            writer.WriteLine("");
            base.RenderContents(writer);
            writer.Indent -= 1;
        */

    }
    protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }



        private String f_get_card_resource_name(String a_card_string, enum_card_position a_card_position)
        {
            // If called with an empty string return the card 'back'

            string my_card_position_token = a_card_position.ToString().ToLower();
            if (a_card_string == HC_str_to_indicate_card_should_be_shown_face_down)
            {
                //return global::HedgeEmWinGUI.Properties.Resources.card__back_blue1_left;

                if (a_card_position == enum_card_position.LEFT) {
                    return "../resources/Cards/card__back_blue1_left.png";
                }

                if (a_card_position == enum_card_position.MIDDLE)
                {
                    return "../resources/Cards/card__back_blue1_middle.png";
                }

                if (a_card_position == enum_card_position.RIGHT)
                {
                    return "../resources/Cards/card__back_blue1_right.png";
                }
            }


            // Dynamically construct the string that will be used to retrive the Image for the card
            String my_card_character_string = a_card_string;
            String card_resource_name = "card_" + my_card_character_string;

            _is_dead = false;
            // Choose whether to use the 'Dead' (greyed out) or 'Alive' version of the card.
            if (_is_dead)
            {
                card_resource_name = "card_" + my_card_character_string + "_" + my_card_position_token + "_dead";
            }
            else
            {
                card_resource_name = "card_" + my_card_character_string + "_"  + my_card_position_token + "_alive";
            }

           // Try to retrieve the card image
            card_resource_name = card_resource_name.ToLower();
            
            // Return the image if you can find
            return "../resources/Cards/" + card_resource_name + ".png";
        }

    }

