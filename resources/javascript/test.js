/**@author Konrad Gadzinowski <kgadzinowski@gmail.com>
 * 
 */
    function SupportVhVw() {

        this.setVh = function(name, vh) {

            $(window).resize( function(event) {
                scaleVh(name, vh);
            });

            scaleVh(name, vh);
        }

        this.setVw = function(name, vw) {

            $(window).resize( function(event) {
                scaleVw(name, vw);
            });

            scaleVw(name, vw);
        }
		
		 this.setMVw = function(name, vh) {

            $(window).resize( function(event) {
                scaleMVw(name, vh);
            });

            scaleMVw(name, vh);
        }
		

        var scaleVw = function(name, vw) {

            var scrWidth = jQuery('body').width();
            var px = (scrWidth * vw) / 100;
            var fontSize = jQuery(name).css('width', px + "px");
        }
		
		
		var scaleMVw = function(name, vh) {

            var scrWidth = jQuery(document).height();
            var px = (scrWidth * vh) / 100;
            var fontSize = jQuery(name).css('max-width', px + "px");
        }
		
		


        var scaleVh = function(name, vh) {

            var scrHeight = jQuery(document).height();

            var px = (scrHeight * vh) / 100;
            var fontSize = jQuery(name).css('height', px + "px");
        }
    };


