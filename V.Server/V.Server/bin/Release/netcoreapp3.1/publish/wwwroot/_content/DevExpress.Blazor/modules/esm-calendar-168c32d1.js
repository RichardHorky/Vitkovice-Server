import"./esm-chunk-d81494b9.js";import"./esm-chunk-a2731447.js";import{ensureElement as t,getParentByClassName as e}from"./esm-dom-utils-0e4190ff.js";import{p as n}from"./esm-chunk-9c16a801.js";import{d as o}from"./esm-chunk-f1e43abb.js";import{L as i,h as c,u as s}from"./esm-chunk-1b6abd73.js";import{initFocusHidingEvents as a}from"./esm-focus-utils-b9e104a7.js";import{T as r}from"./esm-chunk-66f169d2.js";function l(e,o,l,h){if(e=t(e)){if(e.dayCells=void 0,function(t){i.attachEventToElement(t,"touchstart",v),i.attachEventToElement(t,"touchmove",p),i.attachEventToElement(t,"touchend",E),i.attachLongTabEventToElement(t,y),i.attachEventToElement(t,"mousedown",m),i.attachEventToElement(t,"mousemove",g),i.attachEventToElement(t,"mouseup",S)}(e),a(e.querySelector(".card-header.btn-group")),!l){o.style.width="",o.style.height="";const t=o.getBoundingClientRect();o.style.width=t.width+"px",o.style.height=t.height+"px"}return Promise.resolve("ok")}function f(t,n){const o=(e.dayCells||(e.dayCells=e.querySelectorAll(".dxbs-day")),e.dayCells);for(let e,i=0;e=o[i];i++){const o=e.getBoundingClientRect();if(o.top<=n&&n<o.bottom&&o.left<=t&&t<o.right)return e}}function m(t){if(c("TouchStart"))return;const n=d(t.srcElement);n&&(e.inSelectionMode=!0,e.firstDate=u(n))}function g(t){e.inSelectionMode&&(e.throttledDrag||(e.throttledDrag=r.throttle((function(t){const n=f(t.clientX,t.clientY);if(n){const o=u(n);if(!o||!e.firstDate)return;e.lastDate?h.invokeMethodAsync("ChangeSelectionRange",o):(h.invokeMethodAsync("StartSelectionRange",e.firstDate,!t.ctrlKey),e.lastDate=o)}}),20)),e.throttledDrag(t))}function S(t){e.inSelectionMode&&e.lastDate&&h.invokeMethodAsync("EndSelectionRange",!1),e.firstDate=void 0,e.lastDate=void 0,e.inSelectionMode=!1}function v(t){s((function(){}),"TouchStart",300,!0)}function y(t){const n=d(t.srcElement);if(n){e.inSelectionMode=!0;const t=u(n);e.lastDate=t,h.invokeMethodAsync("StartSelectionRange",t,!1)}}function p(t){if(!e.inSelectionMode)return;const o=f(t.touches[0].clientX,t.touches[0].clientY);if(o){const t=u(o);t&&e.lastDate-t!=0&&h.invokeMethodAsync("ChangeSelectionRange",t)}n(t)}function E(t){e.inSelectionMode&&(h.invokeMethodAsync("EndSelectionRange",!0),n(t)),e.inSelectionMode=!1}}function u(t){return new Date(parseInt(t.getAttribute("data-date")))}function d(t){return e(t,"dxbs-day")}function h(e){if(e=t(e))return o(e),o(e.querySelector(".card-header.btn-group")),Promise.resolve("ok")}const f={init:l,dispose:h};export default f;export{h as dispose,l as init};
