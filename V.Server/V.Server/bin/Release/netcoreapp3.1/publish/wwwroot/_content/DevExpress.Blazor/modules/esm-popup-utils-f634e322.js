import"./esm-chunk-d81494b9.js";import{g as t}from"./esm-chunk-a2731447.js";import{changeDom as e,setStyles as i,toggleCssClass as o,clearStyles as n,getCurrentStyleSheet as r,elementHasCssClass as s,getClassName as l}from"./esm-dom-utils-0e4190ff.js";const h="\\s*matrix\\(\\s*"+[0,0,0,0,0,0].map((function(){return"(\\-?\\d+\\.?\\d*)"})).join(",\\s*")+"\\)\\s*";function a(t){let e=0;if(null!=t&&""!==t)try{const i=t.indexOf("px");i>-1&&(e=parseFloat(t.substr(0,i)))}catch(t){}return Math.ceil(e)}function f(t){const e=new RegExp(h).exec(t.transform);return e?{left:parseInt(e[5]),top:parseInt(e[6])}:{left:0,top:0}}function d(t,e,i){t.transform="matrix(1, 0, 0, 1, "+e+", "+i+")"}function m(t,e,i){const o=t.getBoundingClientRect(),n={left:e(o.left),top:e(o.top),right:i(o.right),bottom:i(o.bottom)};return n.width=n.right-n.left,n.height=n.bottom-n.top,n}function c(t){return m(t,Math.floor,Math.ceil)}function g(t){return m(t,Math.ceil,Math.floor)}class u{constructor(t,e){this.key=t,this.info=e}checkMargin(){return!0}allowScroll(){return"height"===this.info.size}canApplyToElement(t){return l(t).indexOf("dxbs-align-"+this.key)>-1}getRange(t){const e=this.getTargetBox(t)[this.info.to],i=e+this.info.sizeM*(t.elementBox[this.info.size]+(this.checkMargin()?t.margin:0));return{from:Math.min(e,i),to:Math.max(e,i),windowOverflow:0}}getTargetBox(t){return null}validate(t,e){const i=e[this.info.size];return t.windowOverflow=Math.abs(Math.min(0,t.from-i.from)+Math.min(0,i.to-t.to)),t.validTo=Math.min(t.to,i.to),t.validFrom=Math.max(t.from,i.from),0===t.windowOverflow}applyRange(e,i){i.appliedModifierKeys[this.info.size]=this.key;const o="width"===this.info.size?"left":"top",n=i.styles;let r=e.from;this.allowScroll()&&e.windowOverflow>0&&(i.limitBox.scroll.width||(i.limitBox.scroll.width=!0,i.limitBox.width.to-=t()),i.isScrollable&&(n["max-height"]=i.height-e.windowOverflow+"px",i.width+=t(),i.elementBox.width+=t(),r=e.validFrom)),n.width=i.width+"px",this.checkMargin()&&(r+=Math.max(0,this.info.sizeM)*i.margin),i.elementBox[o]+=r,d(n,i.elementBox.left,i.elementBox.top)}dockElementToTarget(t){const e=this.getRange(t);this.dockElementToTargetInternal(e,t)||this.applyRange(e,t)}dockElementToTargetInternal(t,e){}}class p extends u{constructor(t,e,i){super(t,e,i),this.oppositePointName=i||null}getTargetBox(t){return t.targetBox.outer}getOppositePoint(){return this._oppositePoint||(this._oppositePoint=x.filter(function(t){return t.key===this.oppositePointName}.bind(this))[0])}dockElementToTargetInternal(t,e){if(this.validate(t,e.limitBox))this.applyRange(t,e);else{const i=this.getOppositePoint(),o=i.getRange(e);if(!(i.validate(o,e.limitBox)||o.windowOverflow<t.windowOverflow))return!1;i.applyRange(o,e)}return!0}}class w extends u{checkMargin(){return!1}getTargetBox(t){return t.targetBox.inner}dockElementToTargetInternal(t,e){return this.validate(t,e.limitBox),!1}validate(t,e){const i=Math.min(t.from,Math.max(0,t.to-e[this.info.size].to));return i>0&&(t.from-=i,t.to-=i),super.validate(t,e)}}const x=[new p("above",{to:"top",from:"bottom",size:"height",sizeM:-1},"below"),new p("below",{to:"bottom",from:"top",size:"height",sizeM:1},"above"),new w("top-sides",{to:"top",from:"top",size:"height",sizeM:1}),new w("bottom-sides",{to:"bottom",from:"bottom",size:"height",sizeM:-1}),new p("outside-left",{to:"left",from:"right",size:"width",sizeM:-1},"outside-right"),new p("outside-right",{to:"right",from:"left",size:"width",sizeM:1},"outside-left"),new w("left-sides",{to:"left",from:"left",size:"width",sizeM:1}),new w("right-sides",{to:"right",from:"right",size:"width",sizeM:-1})];function M(t,o,n,l){return e((function(){const e=function(t,e,i){const o=r(),n=c(t),l=g(e),h=t.ownerDocument.documentElement,d={isScrollable:s(t,"dxbs-scrollable"),specifiedOffsetModifiers:x.filter((function(e){return e.canApplyToElement(t)})),margin:a(o.marginTop),width:i?Math.max(i.width,Math.ceil(t.offsetWidth)):Math.ceil(t.offsetWidth),height:Math.ceil(t.offsetHeight),appliedModifierKeys:{height:null,width:null}},m=f(o),u=t.classList.contains("visually-hidden")?l.left:n.left;var p,w,M,b;d.elementBox={left:p=m.left-u,top:w=m.top-n.top,right:p+(M=n.width),bottom:w+(b=n.height),width:M,height:b},d.targetBox={outer:c(e),inner:g(e)},d.limitBox={scroll:{width:h.clientWidth<window.innerWidth,height:h.clientHeight<window.innerHeight},width:{from:0,to:h.clientWidth},height:{from:0,to:h.clientHeight}},d.styles={};const z=(t.getAttribute("data-popup-align")||i&&i.align).split(/\s+/);return d.offsetModifiers=x.filter((function(t){return z.some((function(e){return t.key===e}))})),d}(t,o,n);for(let t=0;t<e.offsetModifiers.length;t++)e.offsetModifiers[t].dockElementToTarget(e);l(e),i(t,e.styles)}))}function b(t){s(t,"show")?(t.isDockedElementHidden&&delete t.isDockedElementHidden,n(t),o(t,"show",!1)):t.isDockedElementHidden&&delete t.isDockedElementHidden}function z(t,e,i){null!==e&&(M(t,e,{align:i},()=>{}),o(t,"show",!0),n(t))}function B(t){return parseFloat(window.getComputedStyle(t,null).getPropertyValue("padding-right"))}function k(){return window.innerWidth-document.body.getBoundingClientRect().width}export{B as getElementPaddingRight,k as getScrollbarWidth,b as hide,f as parseTranslateInfo,z as show,d as translatePosition};
