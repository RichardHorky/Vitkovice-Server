import{B as e}from"./esm-chunk-d81494b9.js";import{g as t}from"./esm-chunk-a2731447.js";import{findParentBySelector as o,getParentByClassName as n,getDocumentScrollTop as l,getDocumentScrollLeft as i,elementIsInDOM as r,querySelectorFromRoot as s,ensureElement as c,addClassNameToElement as u,unsubscribeElement as d,getLeftRightBordersAndPaddingsSummaryValue as a,getCurrentStyleSheet as f,subscribeElementContentSize as p,subscribeElementVerticalScrollBarVisibility as h,removeClassNameFromElement as g,subscribeElementVerticalScrollBarWidth as m,removeSelection as b,subscribeElementContentWidth as y,elementHasCssClass as S,RequestAnimationFrame as H,getTopBottomBordersAndPaddingsSummaryValue as x,toPx as T}from"./esm-dom-utils-0e4190ff.js";import{a as w,d as v,i as I,g as B}from"./esm-chunk-9c16a801.js";import{d as E,r as V}from"./esm-chunk-f1e43abb.js";import{T as q}from"./esm-chunk-1b6abd73.js";import"./esm-chunk-808bf349.js";import{minColumnWidth as N,ColumnResizeMode as M}from"./esm-grid-column-resize-81963b84.js";import{updateScrollbarStyle as R}from"./esm-dx-style-helper-e7dc0266.js";const C=".dxbs-table td.table-active",L=".dropdown-item.active",D=".dropdown-item.focused",k={GroupPanelHead:"gph",ColumnHead:"ch"};function W(e){if(!e.hasAttribute("data-dxdg-draggable-id"))return null;const t=e.getAttribute("data-dxdg-column-id").split("|"),o=!(t.length>2)||"1"===t[2],n=t.length>1?k[t[1]]:k.ColumnHead,l=n===k.ColumnHead?parseInt(t[0]):-1,i=n===k.GroupPanelHead?parseInt(t[0]):-1,r=e.previousElementSibling;return{columnVisibleIndex:l,groupVisibleIndex:i,columnHeadType:n,canBeGrouped:o,needInsertBeforeToo:!r||!W(r),element:e}}function A(e){return"[data-dxdg-draggable-id="+e+"]"}function P(e,t,o){var n,l,i;n=e,l=G(t,"clientX")-o.left,i=G(t,"clientY")-o.top,void(n.style.transform=["translate(",Math.round(l),"px, ",Math.round(i),"px)"].join(""))}function G(e,t){return void 0!==e[t]?e[t]:void 0!==e.touches?e.touches[0][t]:0}function _(t,r,s,c){const u=G(t,"clientX"),d=G(t,"clientY"),a=t.target;if(s){const e=o(t.target,"th");if(e&&u>=e.getBoundingClientRect().right-N)return}let f=!1;const p=function(e){const t=Math.abs(u-G(e,"clientX"))>10,s=Math.abs(d-G(e,"clientY"))>10;return(t||s)&&(f=!0,h(),function(e,t,r,s){const c=A(r),u=o(t,c);if(!u)return;const d=n(u,"dxbs-gridview").getBoundingClientRect(),a=W(u),f=i(),p=l();let h={left:0,top:0};const g=function(e,t){const o=[],n=A(t),l=document.querySelectorAll(n);let i=!1,r=!1;for(let e=0;e<l.length;e++){const t=l[e],n=t.getBoundingClientRect(),s=W(t),c=s.columnVisibleIndex,u=s.groupVisibleIndex,d=s.columnHeadType;d===k.GroupPanelHead?i=!0:d===k.ColumnHead&&(r=!0),s.needInsertBeforeToo&&o.push(new j(t,n.left,n.top,n.bottom,c,u,d,!0,!1)),o.push(new j(t,n.right,n.top,n.bottom,c,u,d,!1,!1))}if(i||r){if(!i){const e=document.querySelector("[data-dxdg-drag-group-panel="+t+"]");if(e){const t=e.getBoundingClientRect();o.push(new j(e,t.left,t.top,t.bottom,-1,0,k.GroupPanelHead,!1,!0,!0))}}if(!r){const e=document.querySelector("[data-dxdg-drag-head-row="+t+"]");if(e){const t=e.getBoundingClientRect();o.push(new j(e,t.right,t.top,t.bottom,-1,-1,k.ColumnHead,!1,!0,!0))}}}return o}(0,r),m=function(e,t){let o=e.cloneNode(!0);const n=e.getBoundingClientRect(),l={left:G(t,"clientX")-n.left,top:G(t,"clientY")-n.top};if("DIV"!==o.tagName){const t=document.createElement("DIV"),l=window.getComputedStyle(e);t.innerHTML=o.innerHTML,t.className="card "+e.className,t.style.width=n.width+"px",t.style.height=n.height+"px",t.style.paddingTop=l.paddingTop,t.style.paddingBottom=l.paddingBottom,t.style.paddingLeft=l.paddingLeft,t.style.paddingRight=l.paddingRight,o=t}else o.style.width=n.width+"px",o.style.height=n.height+"px";o.className=o.className+" dx-dragging-state",document.body.appendChild(o);const i=o.getBoundingClientRect();return{dragElement:o,offsetFromMouse:{left:i.left+l.left,top:i.top+l.top}}}(u,e),b=m.dragElement,y=m.offsetFromMouse;P(b,e,y);let S=!0,H=null;const x=function(e){S&&(b.style.visibility="visible",S=!1);return P(b,e,{left:y.left+h.left,top:y.top+h.top}),H=function(e,t,o,n,r,s){!function(e){const t=A(e),o=document.querySelectorAll("div.dxgv-target-marks"+t);for(let e=0;e<o.length;e++){const t=o[e];t.parentNode.removeChild(t)}}(t);let c=null;const u=[],d=G(n,"clientX"),a=G(n,"clientY");for(let t=0;t<e.length;t++){const n=e[t];if(n.columnHeadType===k.GroupPanelHead&&!o.canBeGrouped)continue;if(n.top+r.top<=a&&a<=n.bottom+r.top){if(n.wholeRowIsRarget){c=n;break}u.push({distance:Math.abs(Math.abs(n.x+r.left)-Math.abs(d)),target:n})}}if(null==c){let e=1e6;for(const t in u)e>u[t].distance&&X(d,o,u[t].target)&&(e=u[t].distance,c=u[t].target)}null==c||O(o,c)||c.x>=s.left&&c.x<=s.right&&!function(e,t){const o=document.createElement("DIV");o.className="dxgv-target-marks",o.dataset.dxdgDraggableId=t,o.style.top=e.top+(e.docScrollTop-l())+l()-1-1+"px",o.style.height=e.bottom-e.top+2+"px",o.style.left=e.x+(e.docScrollLeft-i())+i()+"px",o.innerHTML=["<span class='oi oi-arrow-bottom' aria-hidden='true'></span>","<div style='height:",o.style.height,"'></div>","<span class='oi oi-arrow-top' aria-hidden='true'></span>"].join(""),document.body.appendChild(o),e.mark=o}(c,t);return c}(g,r,a,e,h,d),e.preventDefault(),!1},T=function(){if(H&&!O(a,H)){s.invokeMethodAsync("OnGridColumnHeadDragNDrop",a.columnHeadType===k.GroupPanelHead?a.groupVisibleIndex:a.columnVisibleIndex,a.columnHeadType,H.columnHeadType===k.GroupPanelHead?H.groupVisibleIndex:H.columnVisibleIndex,H.columnHeadType,H.insertBefore),H.mark&&H.mark.parentNode.removeChild(H.mark)}v(document,q.touchMouseMoveEventName,x),v(document,q.touchMouseUpEventName,T),v(window,"scroll",I),b.parentNode.removeChild(b)},I=function(){h={left:f-i(),top:p-l()}};w(document,q.touchMouseMoveEventName,x),w(document,q.touchMouseUpEventName,T),w(window,"scroll",I)}(e,a,r,c)),e.preventDefault(),!1},h=function(){v(document,q.touchMouseMoveEventName,p),v(document,q.touchMouseUpEventName,h),!f&&a&&e.WebKitTouchUI&&a.click()};return w(document,q.touchMouseMoveEventName,p),w(document,q.touchMouseUpEventName,h),t.preventDefault(),a.focus(),!1}function j(e,t,o,n,r,s,c,u,d){this.element=e,this.x=t,this.top=o,this.bottom=n,this.columnVisibleIndex=r,this.groupVisibleIndex=s,this.columnHeadType=c,this.insertBefore=u,this.wholeRowIsRarget=d,this.docScrollTop=l(),this.docScrollLeft=i()}function O(e,t){function o(e,t){return t.groupVisibleIndex===e.groupVisibleIndex||t.groupVisibleIndex===e.groupVisibleIndex-1&&!t.insertBefore}function n(e,t){return t.columnVisibleIndex===e.columnVisibleIndex||t.columnVisibleIndex===e.columnVisibleIndex-1&&!t.insertBefore}if(t.columnHeadType===e.columnHeadType&&e.columnHeadType===k.GroupPanelHead&&o(e,t))return!0;if(t.columnHeadType===e.columnHeadType&&e.columnHeadType===k.ColumnHead&&n(e,t))return!0;if(e.columnHeadType===k.GroupPanelHead&&t.columnHeadType===k.ColumnHead&&-1!==e.columnVisibleIndex&&n(e,t))return!0;return!(e.columnHeadType!==k.ColumnHead||t.columnHeadType!==k.GroupPanelHead||-1===e.groupVisibleIndex||!o(e,t))}function X(e,t,o){const n=t.element.getBoundingClientRect();if(O(t,o)&&(e<n.left||e>n.right))return!1;if(o.x<n.left){if(e>n.right)return!1}else if(e<n.left)return!1;return!0}k[1]=k.ColumnHead,k[0]=k.GroupPanelHead;class z{constructor(e,t,o){this._itemHeight=e,this._scrollTop=t,this._scrollHeight=o}get itemHeight(){return this._itemHeight}get scrollTop(){return this._scrollTop}get scrollHeight(){return this._scrollHeight}isEqual(e){return this.itemHeight===e.itemHeight&&this.scrollTop===e.scrollTop&&this.scrollHeight===e.scrollHeight}}function U(e){if(!r(e))return;let t=function(e){let t=e.querySelector("*[id$='_LB']");t||(t=e.parentNode.querySelector("*[id$='_LB']"));t||(t=e);if(t){let e=t.querySelector(L);if(e||(e=t.querySelector(C)),e)return e.parentNode}return null}(e);if(t||(t=function(e){let t=null;s(e,(function(o){t=e.querySelector("*"+o+" > *[id$='_LB']")})),t||s(e.parentNode,(function(o){t=e.parentNode.querySelector("*"+o+" > *[id$='_LB']")}));const o=t?t.querySelector(C):null;return o?o.parentNode:null}(e)),t){const o=e;let n=t.offsetTop;if("TABLE"===t.offsetParent.tagName){const e=t.offsetParent.previousElementSibling;e&&(n+=e.clientHeight)}const l=o.scrollTop+o.clientHeight<n+t.offsetHeight;o.scrollTop>n&&(o.scrollTop=n),l&&(o.scrollTop=n-(o.clientHeight-t.offsetHeight))}}function Y(e){if(!r(e))return;const t=function(e){let t=e.querySelector("*[id$='_LB']");t||(t=e.parentNode.querySelector("*[id$='_LB']"));t||(t=e);if(t){const e=t.querySelector(D);if(e)return"TR"===e.tagName?e:e.parentNode}return null}(e);if(t){let o=e.querySelector(".dxgvCSD");o||(o=e);const n=o.scrollTop+o.clientHeight<t.offsetTop+t.offsetHeight;o.scrollTop>t.offsetTop&&(o.scrollTop=t.offsetTop),n&&(o.scrollTop=t.offsetTop-(o.clientHeight-t.offsetHeight))}}function F(e){void 0===e.dataset.virtualScrollLock&&(e.dataset.virtualScrollLock=0)}function $(e){F(e),e.dataset.virtualScrollLock++}function K(e){F(e),e.dataset.virtualScrollLock--}function J(e,t,o,n,l,i,r){o.isHorizontalScrolling&&!function(e,t,o){if(t.scrollLeft===o.scrollLeft)return;const n=B(e);n===t?o.scrollLeft=t.scrollLeft:n===o&&setTimeout(()=>t.scrollLeft=o.scrollLeft,0)}(e,n,l),o.isVirtualScrolling&&function(e,t,o,n,l){if(i=o,F(i),i.dataset.virtualScrollLock>0)return;var i;!function(e,t,o,n,l){let i=!0;o.dataset.prevScrollTop?i=o.dataset.prevScrollTop!==o.scrollTop.toString():o.dataset.prevScrollTop=o.scrollTop;Q(o),i&&(o.dataset.OnScrollTimerId=setTimeout((function(){!function(e,t,o,n,l){const i=Z(t),r=ne(t,l),s=r.scrollTop,c=r.scrollBottom,u=n.clientHeight>0&&c>o.offsetHeight+i.offsetHeight;(o.clientHeight>0&&s<o.offsetHeight||u)&&!function(e,t,o){if(e.dxScrollStateCache&&e.dxScrollStateCache.isEqual(o))return;e.dxScrollStateCache=o,t.DxRestoreScrollTop=t.scrollTop,$(t),e.invokeMethodAsync("OnGridVirtualScroll",o.itemHeight,o.scrollTop,o.scrollHeight).then(o=>{!function(e,t){fe(e.mainElement,e,t)}(o,e),K(t)}).catch(le)}(e,t,r.requestScrollState)}(e,o,n,l,!1),t.needInternalSettings&&re(t)}),200),function(e){const t=0===e.scrollTop,o=e.scrollHeight-e.scrollTop===e.clientHeight;e.classList.remove("dx-scrolling"),t||o||e.classList.add("dx-scrolling")}(o))}(e,t,o,n,l)}(t,o,n,i,r)}function Q(e){e.dataset.OnScrollTimerId&&(clearTimeout(e.dataset.OnScrollTimerId),delete e.dataset.OnScrollTimerId)}function Z(e){const t=e.querySelector("table.dxbs-table"),o=e.classList.contains("dxbs-listbox")?e.querySelector("ul"):null;return t||o}function ee(e,t,o,n,l){const i=Z(t),r=function(e,t){const o=te(e);return{itemHeight:o,spacerTop:t.virtualScrollingOptions.itemsAbove*o,spacerBelow:t.virtualScrollingOptions.itemsBelow*o}}(i,l);o.style.height=r.spacerTop+"px",n.style.height=r.spacerBelow+"px",function(e,t,o,n){if(e.clientHeight>t.offsetHeight)return;e.scrollTop<o.clientHeight&&(e.scrollTop=o.clientHeight+1);e.scrollTop+e.clientHeight>o.clientHeight+t.offsetHeight&&(e.scrollTop=o.clientHeight+t.offsetHeight-e.clientHeight-1)}(t,i,o)}function te(e){const t=function(e){let t=[];return e.dataset.tempUniqueId="tempUniqueId","TABLE"===e.tagName?t=e.parentNode.querySelectorAll("TABLE[data-temp-unique-id] > TBODY > TR"):"UL"===e.tagName&&(t=e.parentNode.querySelectorAll("ul[data-temp-unique-id] > li")),delete e.dataset.tempUniqueId,t}(e),o={};for(let e=0;e<t.length;e++){const n=t[e];o[n.offsetHeight]=o[n.offsetHeight]?o[n.offsetHeight]+1:1}let n=0,l=0;for(const e in o)o[e]>l&&(l=o[e],n=e);return Number.parseInt(n)}function oe(e){return null!==Z(e)}function ne(e,t){let o=300,n=300;t&&(o=0,n=0);const l=Z(e);let i=e.scrollTop-o;i<0&&(i=0);const r=e.scrollTop+e.clientHeight+n;let s=i-o;s<0&&(s=0);const c=r-s+n,u=te(l);return{scrollTop:i,scrollBottom:r,requestScrollState:new z(u,s,c)}}function le(e){e&&e.mainElement&&ge(e.mainElement)}function ie(e){return function(){if(!e.ownerStyleSheet)return;const t=e.parentStyleSheet,o=Array.prototype.indexOf.call(t.cssRules,e);o>-1&&t.deleteRule(o)}}function re(e){const t=e.elementsStorage,o=c(e.mainElement).parentElement;if(!o)return;const n=c(t.scrollElement),l=c(t.scrollHeaderElement),i=[],r=window.getComputedStyle(o);if(r){if(!n.style.maxHeight)if(e.isDropDown)n.style.maxHeight=ue(r,l)+"px";else{const e=p(o,e=>{n.style.maxHeight=e.height-l.offsetHeight+"px"}),t=p(l,e=>{n.style.maxHeight=o.clientHeight-(e.height+2*x(l))+"px"});i.push((function(){d(e),d(t)})),n.style.maxHeight=o.clientHeight-l.offsetHeight+"px"}if(e.isDropDown&&2!==e.dropDownWidthMode){const t=ce(n,l,o,e.mainElement,e.dropDownWidthMode,r),s=n.querySelector("table");if(!s)return;V(s,(function(){t&&t()}));const c=p(s,t=>{E(s);const i=ce(n,l,o,e.mainElement,e.dropDownWidthMode,r);V(s,(function(){i&&i()}))});i.push((function(){d(c),E(s)}))}return i.length>0?function(){i.forEach(e=>e())}:null}}function se(e,t,o){t&&!o?u(e,"dxbs-vertical-scrollbar-visible"):g(e,"dxbs-vertical-scrollbar-visible")}function ce(e,o,l,i,r,s){function u(e,t){const o=e.querySelector(t);return o?o.children:null}let d=null;const a=e.querySelector("table"),p=o.querySelector("table");if(!a||!p)return;const h=u(a,"tbody>tr"),g=h&&1===h.length&&a.querySelector("tr.dxbs-empty-data-row"),m=u(p,"colgroup"),b=u(a,"colgroup");l.dataset.calculated&&ae(m,b);let y=0;if(g){p.style.width="auto";const e=window.getComputedStyle(p).width;p.removeAttribute("style"),y=parseFloat(e,10)}else{const e=u(p,"thead>tr");if(!(m&&e&&h&&b))return;a.style.width=p.style.width="auto";const t=[];for(let o=0;o<m.length;o++)if(m[o].style.width)if(-1!==m[o].style.width.indexOf("%"))t.push(o);else{const e=f(),t=c(i).getAttribute("data-dxdg-id");let n=null;e&&(e.insertRule("[data-dxdg-id='"+t+"'] table tr>td:nth-child("+(o+1)+"), [data-dxdg-id='"+t+"'] table tr>th:nth-child("+(o+1)+") { max-width:"+m[o].style.width+"; }",e.cssRules.length),n=e.cssRules[e.cssRules.length-1],y+=parseFloat(m[o].style.width,10)),d=ie(n)}else m[o].dataset.autoWidth=!0,y+=de(m[o],b[o],e[o],h[o]);if(t.length>0)for(let o=0;o<t.length;o++)y+=de(m[o],b[o],e[o],h[o]);a.removeAttribute("style"),p.removeAttribute("style")}if(0===r||1===r){const i=n(l,"dx-blazor-multicolumn-editor");if(!i)return;const c=parseInt(s.borderRightWidth,10)+parseInt(s.borderLeftWidth,10),u=y+(e.querySelector("table").offsetHeight>ue(s,o)?t():0)+c+1;0===r&&i.offsetWidth>=u?(ae(m,b),l.style.width=i.offsetWidth+"px"):l.style.width=u+"px"}return l.dataset.calculated=!0,d}function ue(e,t){const o=parseInt(e.borderTopWidth,10)+parseInt(e.borderBottomWidth,10);return parseInt(e.maxHeight,10)-o-t.offsetHeight}function de(e,t,o,n){const l=o.offsetWidth,i=n.offsetWidth,r=l>i?l:i;return t.style.width=e.style.width=r+"px",r}function ae(e,t){for(let o=0;o<e.length;o++)if(e[o].dataset.autoWidth&&(e[o].style.width=null,t)){const e=t.item(o);e&&(e.style.width=null)}}function fe(e,o,l){const i=c(e);if(!i)return;const r=l,s=o.isMultipleSelectionEnabled,p=o.scrollToSelectedItemRequested,g=o.elementsStorage;let y=c(g.scrollElement),S=c(g.scrollHeaderElement),H=c(g.virtualScrollSpacerTopElement),x=c(g.virtualScrollSpacerBottomElement);E(i);let T=null,N=null,C=null,L=null,D=null;o.needInternalSettings&&(D=re(o)),y=c(y);let k=null;if(y){if(S=c(S),$(y),o.isVirtualScrolling&&(H=c(H),x=c(x)),o.isVirtualScrolling||o.isVerticalScrolling){u(i,"dxbs-has-vertical-scrollbar"),se(i,y.scrollHeight>y.clientHeight,o.isHorizontalScrolling);o.isFirstRender&&o.isAutoVerticalScrollBarMode&&(i.disposeVerticalScrollBarSubscriber=function(e,t,o){const n=h(t,t=>{se(e,t,o)});return function(){d(n)}}(i,y,o.isHorizontalScrolling)),o.isFirstRender&&(i.disposeVerticalScrollBarWidthSubscriber=function(){const e=m(()=>R());return function(){d(e)}}())}if(o.isVirtualScrolling&&(ee(0,y,H,x,o),p?U(y):y.DxRestoreScrollTop&&(y.scrollTop=y.DxRestoreScrollTop,delete y.DxRestoreScrollTop)),k=function(e){const t=e.querySelectorAll(".btn.btn-toggle");let o=0;for(let e=0;e<t.length;e++){const n=t[e],l=n.offsetWidth+a(n.parentNode);if(l>0){o=l;break}}e&&e.style.setProperty("--button-w",o+"px");const n=f(),l=e.getAttribute("data-dxdg-id");let i=null;return n&&(n.insertRule("[data-dxdg-id='"+l+"'] > col.dxbs-grid-cols-togglebtn { width:"+o+"px !important; }",n.cssRules.length),i=n.cssRules[n.cssRules.length-1]),ie(i)}(i),T=function(e){return J(e,r,o,y,S,H,x)},w(y,"scroll",T),S&&w(S,"scroll",T),K(y),function(e){return!e.needInternalSettings&&(e.isHorizontalScrolling||e.isVerticalScrolling&&e.columnResizeMode!==M.Component)}(o)){const e=c(g.rootElement);N=function(){!function(e,o,n,l){let i,r;o&&($(o),i=o.style.overflowX,o.style.overflowX="hidden",o.style.width="0"),n&&(r=n.style.overflowX,n.style.overflowX="hidden",n.style.width="0");const s=e.clientWidth;if(o&&(s&&(o.style.width=s+"px"),o.style.overflowX=i),n){const e=function(e,t){return(t.isVerticalScrolling||t.isVirtualScrolling)&&(e.clientHeight<e.scrollHeight||"scroll"===e.style.overflowY)}(o,l)?t():0;s&&(n.style.width=s-e+"px"),n.style.overflowX=r}o&&K(o)}(e,y,S,o)},N(),w(window,"resize",N)}}function W(e){!function(e){if(!s||!e.shiftKey||!I(e))return;const t=B(e);n(t,"dxbs-data-row")&&b()}(e)}w(i,"mousedown",W);let A=null,P=null;const G=o.columnResizeMode!==M.Disabled;if(o.isColumnDragEnabled){const e=i.dataset.dxdgId;e&&(A=i.querySelector("[data-dxdg-drag-head-row='"+e+"']"),A&&(C=function(t){return _(t,e,G,r)},w(A,q.touchMouseDownEventName,C)),P=i.querySelector("[data-dxdg-gp='"+e+"']"),P&&(L=function(t){return _(t,e,G,r)},w(P,q.touchMouseDownEventName,L)))}return V(i,(function(){k&&k(),D&&D(),T&&(v(y,"scroll",J),S&&v(S,"scroll",J)),N&&v(window,"resize",N),y&&Q(y),C&&A&&v(A,q.touchMouseDownEventName,C),L&&P&&v(P,q.touchMouseDownEventName,L),v(i,"mousedown",W)})),Promise.resolve("ok")}function pe(e,t,o){const n=y(e,(e,o)=>{!function(e,t,o){e.style.width=T(t),e.style.height=T(o),u(e,"dxbs-edit-form-bounding-rect")}(t,e,o)},o);return function(){d(n)}}function he(e,t){if(!(e=document.getElementById(e)))return;const o=n(e,"dxbs-table"),l=o&&o.parentNode,i=e.querySelector(".dxbs-grid-fl");return t?e.disposeWidthSubscriber=pe(l,e,i):(me(e),function(e){S(e,"dxbs-edit-form-bounding-rect")&&H((function(){g(e,"dxbs-edit-form-bounding-rect"),e.style.width=null,e.style.height=null}))}(e)),Promise.resolve("ok")}function ge(e){return(e=c(e))&&function(e){E(e),function(e){e.disposeVerticalScrollBarSubscriber&&(e.disposeVerticalScrollBarSubscriber(),delete e.disposeVerticalScrollBarSubscriber)}(e),me(e),function(e){e.disposeVerticalScrollBarWidthSubscriber&&(e.disposeVerticalScrollBarWidthSubscriber(),delete e.disposeVerticalScrollBarWidthSubscriber)}(e)}(e),Promise.resolve("ok")}function me(e){e.disposeWidthSubscriber&&(e.disposeWidthSubscriber(),delete e.disposeWidthSubscriber)}const be={init:fe,dispose:ge,configureEditRow:he};export default be;export{oe as HasParametersForVirtualScrollingRequest,he as configureEditRow,ge as dispose,ne as getParametersForVirtualScrollingRequest,fe as init,Y as scrollToFocusedItem,U as scrollToSelectedItem};
