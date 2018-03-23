;( function( window ) {
	
    'use strict';
    function classReg(className) {
        return new RegExp("(^|\\s+)" + className + "(\\s+|$)");
    }

    var hasClass, addClass, removeClass;

    if ('classList' in document.documentElement) {
        hasClass = function (elem, c) {
            return elem.classList.contains(c);
        };
        addClass = function (elem, c) {
            elem.classList.add(c);
        };
        removeClass = function (elem, c) {
            elem.classList.remove(c);
        };
    }
    else {
        hasClass = function (elem, c) {
            return classReg(c).test(elem.className);
        };
        addClass = function (elem, c) {
            if (!hasClass(elem, c)) {
                elem.className = elem.className + ' ' + c;
            }
        };
        removeClass = function (elem, c) {
            elem.className = elem.className.replace(classReg(c), ' ');
        };
    }

    function toggleClass(elem, c) {
        var fn = hasClass(elem, c) ? removeClass : addClass;
        fn(elem, c);
    }

    var classie = {
        hasClass: hasClass,
        addClass: addClass,
        removeClass: removeClass,
        toggleClass: toggleClass,
        has: hasClass,
        add: addClass,
        remove: removeClass,
        toggle: toggleClass
    };

    if (typeof define === 'function' && define.amd) {
        define(classie);
    } else {
        window.classie = classie;
    }

	function extend( a, b ) {
		for( var key in b ) { 
			if( b.hasOwnProperty( key ) ) {
				a[key] = b[key];
			}
		}
		return a;
	}

	function hasParent( e, id ) {
		if (!e) return false;
		var el = e.target||e.srcElement||e||false;
		while (el && el.id != id) {
			el = el.parentNode||false;
		}
		return (el!==false);
	}

	function getLevelDepth( e, id, waypoint, cnt ) {
		cnt = cnt || 0;
		if ( e.id.indexOf( id ) >= 0 ) return cnt;
		if( classie.has( e, waypoint ) ) {
			++cnt;
		}
		return e.parentNode && getLevelDepth( e.parentNode, id, waypoint, cnt );
	}

	function mobilecheck() {
		var check = false;
		(function(a){if(/(android|ipad|playbook|silk|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(a)||/1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(a.substr(0,4)))check = true})(navigator.userAgent||navigator.vendor||window.opera);
		return check;
	}

	function closest(e, classname) {
		if( classie.has( e, classname ) ) {
			return e;
		}
		return e.parentNode && closest( e.parentNode, classname );
	}

	function mlPushMenu( el, trigger, options ) {	
		this.el = el;
		this.trigger = trigger;
		this.options = extend( this.defaults, options );
		this.support = Modernizr.csstransforms3d;
		if( this.support ) {
			this._init();
		}
	}

	mlPushMenu.prototype = {
		defaults : {
			type : 'overlap', // overlap || cover
			levelSpacing : 40,
			backClass: 'menuBack'
		},
		_init : function() {
			this.open = false;
			this.level = 0;
			this.wrapper = document.getElementById('divMainMenu');
			this.scroller = document.getElementById('scroller');
			this.levels = Array.prototype.slice.call(this.el.querySelectorAll('div.menuLevel'));
			var self = this;
			this.levels.forEach(function (el, i) { el.setAttribute('data-level', getLevelDepth(el, self.el.id, 'menuLevel')); });
			this.menuItems = Array.prototype.slice.call( this.el.querySelectorAll( 'li' ) );
			this.levelBack = Array.prototype.slice.call( this.el.querySelectorAll( '.' + this.options.backClass ) );
			this.eventtype = mobilecheck() ? 'touchstart' : 'click';
			classie.add( this.el, 'menu-' + this.options.type );
			this._initEvents();
		},
		_initEvents : function() {
			var self = this;

			var bodyClickFn = function( el ) {
				self._resetMenu();
				el.removeEventListener( self.eventtype, bodyClickFn );
			};

			this.trigger.addEventListener( this.eventtype, function( ev ) {
				ev.stopPropagation();
				ev.preventDefault();
				if( self.open ) {
					self._resetMenu();
				}
				else {
					self._openMenu();
					document.addEventListener( self.eventtype, function( ev ) {
						if( self.open && !hasParent( ev.target, self.el.id ) ) {
							bodyClickFn( this );
						}
					} );
				}
			} );

			this.menuItems.forEach( function( el, i ) {
			    var subLevel = el.querySelector('div.menuLevel');
				if( subLevel ) {
					el.querySelector( 'a' ).addEventListener( self.eventtype, function( ev ) {
						ev.preventDefault();
						var level = closest(el, 'menuLevel').getAttribute('data-level');
						if( self.level <= level ) {
							ev.stopPropagation();
							classie.add(closest(el, 'menuLevel'), 'menuLevel-overlay');
							self._openMenu( subLevel );
						}
					} );
				}
			} );

			this.levels.forEach( function( el, i ) {
				el.addEventListener( self.eventtype, function( ev ) {
					ev.stopPropagation();
					var level = el.getAttribute( 'data-level' );
					if( self.level > level ) {
						self.level = level;
						self._closeMenu();
					}
				} );
			} );

			this.levelBack.forEach( function( el, i ) {
				el.addEventListener( self.eventtype, function( ev ) {
					ev.preventDefault();
					var level = closest(el, 'menuLevel').getAttribute('data-level');
					if( self.level <= level ) {
						ev.stopPropagation();
						self.level = closest(el, 'menuLevel').getAttribute('data-level') - 1;
						self.level === 0 ? self._resetMenu() : self._closeMenu();
					}
				} );
			} );	
		},
		_openMenu : function( subLevel ) {
			++this.level;
			$(".divMainMenu").css("width", "85%");
			$(".divMainMenu").css("z-index", "1");
			var levelFactor = ( this.level - 1 ) * this.options.levelSpacing,
				translateVal = this.options.type === 'overlap' ? this.el.offsetWidth + levelFactor : this.el.offsetWidth;
			
			this._setTransform('translate3d(' + translateVal + 'px,0,0)');
		    $('#scroller').css({
		        transform: "translate3d(" +translateVal +"px, 0, 0)"
		    });

			if( subLevel ) {
				this._setTransform( '', subLevel );
				for( var i = 0, len = this.levels.length; i < len; ++i ) {
					var levelEl = this.levels[i];
					if (levelEl != subLevel && !classie.has(levelEl, 'menuLevel-open')) {
						this._setTransform( 'translate3d(-100%,0,0) translate3d(' + -1*levelFactor + 'px,0,0)', levelEl );
					}
				}
			}
			if( this.level === 1 ) {
			    classie.add(this.wrapper, 'menuPushed');
				this.open = true;
			}
			classie.add(subLevel || this.levels[0], 'menuLevel-open');
		    classie.add(trigger, 'active');
		},
		_resetMenu: function () {
		    $(".divMainMenu").css("width", "0%");
		    this._setTransform('translate3d(0,0,0)');
		    $('#scroller').css({
		        transform: "translate3d(0, 0, 0)"
		    });
		    $("#scroller").css({ "transform": "" });
			this.level = 0;
			classie.remove(this.wrapper, 'menuPushed');
			classie.remove(trigger, 'active');
			this._toggleLevels();
			this.open = false;
		},
		_closeMenu : function() {
			var translateVal = this.options.type === 'overlap' ? this.el.offsetWidth + ( this.level - 1 ) * this.options.levelSpacing : this.el.offsetWidth;
			this._setTransform('translate3d(' + translateVal + 'px,0,0)');
			$('#scroller').css({
			    transform: "translate3d(" + translateVal + "px, 0, 0)"
			});
			$("#scroller").css({ "transform": "" });
			this._toggleLevels();
		},
		_setTransform : function( val, el ) {
			el = el || this.wrapper;
			el.style.WebkitTransform = val;
			el.style.MozTransform = val;
			el.style.transform = val;
		},
		_toggleLevels : function() {
			for( var i = 0, len = this.levels.length; i < len; ++i ) {
				var levelEl = this.levels[i];
				if( levelEl.getAttribute( 'data-level' ) >= this.level + 1 ) {
				    classie.remove(levelEl, 'menuLevel-open');
				    classie.remove(levelEl, 'menuLevel-overlay');
				}
				else if( Number( levelEl.getAttribute( 'data-level' ) ) == this.level ) {
				    classie.remove(levelEl, 'menuLevel-overlay');
				}
			}
		}
	}
    
	window.mlPushMenu = mlPushMenu;
})(window);
