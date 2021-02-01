import"./esm-chunk-d81494b9.js";import"./esm-chunk-a2731447.js";import{ensureElement as e,RequestAnimationFrame as t,addClassNameToElement as i,getLeftRightPaddings as s}from"./esm-dom-utils-0e4190ff.js";import{g as n}from"./esm-chunk-9c16a801.js";import{T as o}from"./esm-chunk-1b6abd73.js";import{K as l}from"./esm-chunk-808bf349.js";import{ensureAccentColorStyle as h}from"./esm-dx-style-helper-e7dc0266.js";function r(e){window.dxAccessibilityHelper||(window.dxAccessibilityHelper=new d),window.dxAccessibilityHelper.sendMessageToAssistiveTechnology(e)}class d{constructor(){this._helperElement=null}get helperElement(){return null==this._helperElement&&(this._helperElement=this.createHelperElement()),this._helperElement}createHelperElement(){const e=document.createElement("DIV");return e.className="dxAIFE dxAIFME",e.setAttribute("role","note"),e.setAttribute("aria-live","assertive"),document.documentElement.appendChild(e),e}sendMessageToAssistiveTechnology(e){this.helperElement.innerHTML=e,setTimeout(()=>{this.helperElement.innerHTML=""},300)}}const a={Disabled:0,NextColumn:1,Component:2},m=0,u=1,c=2,E=30;class p{constructor(e,t,i,s){this._mainElement=e,this._nextHeaderCellElement=e.nextElementSibling,this._blazorComponent=s,this._gridResizeProxy=t,this._mode=i,this._resizeParameters={},this._resizeAnchor=null,this._lastWidth=null,this._lastNextWidth=null,this._timeoutId=null,this._leftMouseXBoundary=null,this._rightMouseXBoundary=null}get mainElement(){return this._mainElement}get nextHeaderCellElement(){return this._nextHeaderCellElement}get hasNextColumn(){return!!this._nextHeaderCellElement}get hasColumnId(){return this.mainElement.hasAttribute("data-dxdg-column-id")}get gridResizeProxy(){return this._gridResizeProxy}get blazorComponent(){return this._blazorComponent}get resizeAnchor(){return this._resizeAnchor}get caption(){return this.mainElement.innerText}get isResizeAllowed(){return this._mode!==a.Disabled&&this.hasColumnId&&(this.hasNextColumn||this._mode===a.Component)}get step(){let e=Math.ceil((this._resizeParameters.maxWidth-this._resizeParameters.minWidth)/100);return e>5&&(e=5),e}initialize(){this.createResizeAnchor(),M().initializeHeaderCellEvents(this),this.initializeEvents()}initializeEvents(){this.isResizeAllowed&&(this.mainElement.addEventListener("focus",this.onFocus.bind(this)),this.mainElement.addEventListener("keydown",this.onKeyDown.bind(this)),this.mainElement.addEventListener("keyup",this.onKeyUp.bind(this)))}onFocus(e){n(e)===this.mainElement&&(this.onFocusCore(),this._lastWidth=this.mainElement.offsetWidth,this._lastNextWidth=this.hasNextColumn?this.nextHeaderCellElement.offsetWidth:0,this.updateResizeParameters())}onKeyDown(e){if(n(e)===this.mainElement)switch(e.keyCode){case l.Left:this.onKeyResize(-this.step),e.stopPropagation(),e.preventDefault();break;case l.Right:this.onKeyResize(this.step),e.stopPropagation(),e.preventDefault()}}onKeyResize(e){let i=1500;this.hasNextColumn&&(i=this._lastWidth+this._lastNextWidth);const s=this._lastWidth+e;t(function(){this.setWidth(s,i,e)}.bind(this)),this._timeoutId&&clearTimeout(this._timeoutId)}onKeyUp(e){if(n(e)!==this.mainElement)return;const t=e.keyCode;t===l.Left&&t===l.Right&&(this._timeoutId&&clearTimeout(this._timeoutId),this._timeoutId=setTimeout(()=>this.invokeSetWidth(),500))}updateResizeParameters(){const e=this.mainElement.offsetWidth,t=this.isResizeAllowed;let i=1500;this._mode===a.NextColumn&&(i=e+(this.hasNextColumn?this.nextHeaderCellElement.offsetWidth:0)-30),this._resizeParameters={allowResize:t,minWidth:t?30:e,currentWidth:e,maxWidth:t?i:e}}update(e){const t=this.isResizeAllowed;this._nextHeaderCellElement=this.mainElement.nextElementSibling,this._mode=e,this.isResizeAllowed?t||(this.mainElement.appendChild(this._resizeAnchor),M().initializeHeaderCellEvents(this)):t&&(this.mainElement.removeChild(this._resizeAnchor),M().removeHeaderCellEvents(this))}createResizeAnchor(){const e=document.createElement("div");i(e,"dxColumnResizeAnchor"),e.dxResizableHeaderCell=this,this._resizeAnchor=e,this.mainElement.appendChild(e)}onMouseDown(){this.setMouseBoundaries(),this.onFocusCore()}setMouseBoundaries(){const e=this.mainElement.getBoundingClientRect(),t=Math.min(30,e.width);if(this._leftMouseXBoundary=e.left+t-10,this._mode===a.NextColumn){const t=this.nextHeaderCellElement.getBoundingClientRect();this._rightMouseXBoundary=e.right+t.width-30}}removeMouseBoundaries(){this._leftMouseXBoundary=null,this._rightMouseXBoundary=null}getResizeType(e){return e<=this._leftMouseXBoundary?u:this._rightMouseXBoundary&&e>=this._rightMouseXBoundary?c:m}onFocusCore(){this.isResizeAllowed&&(this.ensureWidthSyncronized(),r(`Width is ${this.mainElement.offsetWidth} pixels. Use arrow keys to resize.`))}onDragResizeAnchor(e,t){if(!this.isResizeAllowed)return;const i=this.getResizeType(t),s=this.mainElement.offsetWidth,n=this.getNewWidth(s,e,i);this.setWidth(n,this.hasNextColumn?s+this.nextHeaderCellElement.offsetWidth:null,e)}getNewWidth(e,t,i){switch(i){case u:return Math.min(30,e);case c:return e+this.nextHeaderCellElement.offsetWidth-30;default:return e+t}}onDragResizeAnchorStop(){this.removeMouseBoundaries(),this.invokeSetWidth()}setWidth(e,t,i){if(!this.isWidthChanged(e)||!this.isValidWidth(e,t,i))return!1;const n=(o=this.mainElement,"border-box"===window.getComputedStyle(o,null).getPropertyValue("box-sizing")?0:s(o));var o;const l=t-(e-=n)-n;return this._lastWidth=e,this._lastNextWidth=l,this.gridResizeProxy.setWidth(this._mode,e,l),!0}isWidthChanged(e){return e!==this._lastWidth}invokeSetWidth(){this._lastWidth&&this._lastNextWidth&&(this.gridResizeProxy.resetWidth(),M().lockResize(),this.blazorComponent.invokeMethodAsync("SetColumnWidths",f(this.mainElement),this._lastWidth,f(this.nextHeaderCellElement),this._lastNextWidth).then(()=>{this._lastWidth=null,this._lastNextWidth=null,M().unlockResize()}))}isValidWidth(e,t,i){let s=e>=30||i>0;return s&&this._mode!==a.Component&&(s=e<=t-30),s}ensureWidthSyncronized(){if(this.isWidthSyncronized())return;const e=this.gridResizeProxy;this.blazorComponent.invokeMethodAsync("GetCellCache").then(t=>{e.syncronizeWidth(t)})}isWidthSyncronized(){let e=!1;const t=this.gridResizeProxy.getColElementInlineWidth();if(0===t)e=!0;else{e=t===this.mainElement.offsetWidth}return e}}class z{constructor(e,t){this.mainElement=e,this.update(t)}get colElements(){return R(this._colElements,this._resizeElements.colElements)}get elementsToSetComponentWidth(){return R(this._elementsToSetComponentWidth,this._resizeElements.elementsToSetComponentWidth)}get elementsToSyncWidth(){return R(this._elementsToSyncWidth,this._resizeElements.elementsToSyncWidth)}get elementsToCheckScroll(){return R(this._elementsToCheckScroll,this._resizeElements.elementsToCheckScroll)}get elementsToReset(){return R(this._elementsToReset,this._resizeElements.elementsToReset)}update(e){this._resizeElements=e,this._colElements=null,this._elementsToSetComponentWidth=null,this._elementsToSyncWidth=null,this._elementsToCheckScroll=null,this._elementsToReset=null}setWidth(e,t,i){r(t+" pixels"),this.setComponentWidth(t),this.colElements.forEach(s=>{s.style.width=w(t),e===a.NextColumn&&(s.nextElementSibling.style.width=w(i))})}setComponentWidth(e){if(!this.elementsToSetComponentWidth)return;if(this.isScrollExists())return;const t=e-this.mainElement.offsetWidth,i=[];this.elementsToSetComponentWidth.forEach(e=>i.push(new _(e,e.offsetWidth+t))),this.elementsToSyncWidth&&i.push(new _(this.elementsToSyncWidth[0],this.elementsToSyncWidth[1].offsetWidth+t)),i.forEach(e=>e.apply())}isScrollExists(){const e=this.elementsToCheckScroll;if(!e)return;return e[0].clientWidth<e[1].clientWidth}getColElementInlineWidth(){let e=0;const t=this.colElements;for(let i=0;i<t.length;i++){const s=t[i].style.width;if(""!==s){e=parseInt(s);break}}return e}syncronizeWidth(i){t((function(){const t=i.map((function(t){const i=e(t[0]);return i?{width:i.offsetWidth,cols:v(t[1])}:null})).filter(e=>e);for(let e=0;e<t.length;e++){const i=t[e].cols;for(let s=0;s<i.length;s++)i[s].style.width=w(t[e].width)}}))}resetWidth(){this.elementsToReset&&!this.isScrollExists()&&this.elementsToReset.forEach(e=>{e.style.width=null})}}class _{constructor(e,t){this._element=e,this._width=t}apply(){this._element.style.width=w(this._width)}}class g{constructor(){this._pageX=-1,this._resizeLock=0,this._resizableHeaderCell=null}get pageX(){return this._pageX}set pageX(e){this._pageX=e}get resizableHeaderCell(){return this._resizableHeaderCell}initializeHeaderCellEvents(e){this.initializeMouseDown(e.resizeAnchor)}initializeMouseDown(e){e.addEventListener(o.touchMouseDownEventName,W)}initializeMouseMove(){document.addEventListener(o.touchMouseMoveEventName,x)}initializeMouseUp(){document.addEventListener(o.touchMouseUpEventName,y)}removeHeaderCellEvents(e){e.resizeAnchor.removeEventListener(o.touchMouseDownEventName,W)}onMouseDown(e,t){this._resizableHeaderCell=e,e.onMouseDown(),this.pageX=t,this.initializeMouseMove(),this.initializeMouseUp()}lockResize(){this._resizeLock++}unlockResize(){this._resizeLock--}isResizeLocked(){return this._resizeLock>0}invalidateState(){this._pageX=-1,this._resizableHeaderCell=null}}function f(e){if(!e)return(-1).toString();return e.getAttribute("data-dxdg-column-id").split("|")[0]}function C(e){return Math.round(e.pageX||e.changedTouches&&e.changedTouches[0].pageX||0)}function W(e){const t=M(),i=n(e).dxResizableHeaderCell;if(i&&i.isResizeAllowed){const s=C(e);t.onMouseDown(i,s)}}function x(e){const i=M();if(i.isResizeLocked()||-1===i.pageX)return;const s=C(e),n=s-i.pageX;if(i.pageX=s,0!==n){const e=i.resizableHeaderCell;e&&t((function(){e.onDragResizeAnchor(n,s)}))}}function y(){const e=M();e.resizableHeaderCell.onDragResizeAnchorStop(),e.invalidateState(),document.removeEventListener(o.touchMouseMoveEventName,x),document.removeEventListener(o.touchMouseUpEventName,y)}function w(e){return e+"px"}function R(e,t){return t?(e||(e=v(t)),e):null}function v(e){return e.map(e=>document.getElementById(e))}function M(){return null===window.dxResizeEventManagerInstance&&(window.dxResizeEventManagerInstance=new g),window.dxResizeEventManagerInstance}window.dxResizeEventManagerInstance=null;const T=new Map;function b(t,i,s,n){if(!(t=e(t)))return;let o=T.get(t);if(o)o.update(s),o.gridResizeProxy.update(i);else{const e=new z(t,i);o=new p(t,e,s,n),o.isResizeAllowed&&(o.initialize(),T.set(t,o))}return h(),Promise.resolve("ok")}const S={initResizeColumn:b};export default S;export{a as ColumnResizeMode,b as initResizeColumn,E as minColumnWidth};
