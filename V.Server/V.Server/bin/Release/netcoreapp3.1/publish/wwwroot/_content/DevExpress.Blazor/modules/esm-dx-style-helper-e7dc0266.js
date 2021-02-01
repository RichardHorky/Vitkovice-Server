import{B as e}from"./esm-chunk-d81494b9.js";import{g as t}from"./esm-chunk-a2731447.js";import{toPx as s}from"./esm-dom-utils-0e4190ff.js";let i=null;class n{constructor(e,t){this._name=e,this._value=t}get name(){return this._name}get value(){return this._value}}class l{constructor(e,t){this._selectors=e,this._rules=t}get selectors(){return this._selectors}get rules(){return this._rules}toString(e){let t=this.rules.join("\n");for(let s=0;s<e.length;s++){const i=e[s];t=t.replace(new RegExp(""+i.name,"g"),i.value)}return`${this.selectors.join(",\n")} { ${t} }`}}class r{constructor(){this._rules=[],this._dummyContainer=null,this._ieCssStyles=[],this.initialize()}get styleElement(){return null==this._styleElement&&(this._styleElement=this.createStyleElement()),this._styleElement}createStyleElement(){const e=document.createElement("STYLE");return document.head.appendChild(e),e}initialize(){this.initializeIeCssStyles(),this.initializeDummyElements(),this.initializeRules(),this.updateStyleElement(),this.removeDummyElements()}getAccentColor(){const e=this._dummyContainer.querySelector("a");return window.getComputedStyle(e).color}getAccentShadowColor(e){const t=e.replace("rgb","").replace("(","").replace(")","").split(",").map(e=>e.trim());t.push(".25");return`rgba(${t.join(",")})`}updateStyleElement(){e.IE?this.updateIEStyleElement():this.updateStyleElementCore()}updateIEStyleElement(){const e=this._ieCssStyles.map(e=>e.toString(this._rules)).join("\n");this.styleElement.innerHTML=e}updateStyleElementCore(){const e=this._rules.map(e=>`${e.name}: ${e.value}`).join(";\n");this.styleElement.innerHTML=`:root{ ${e} }`}update(){this._rules=[],this.initialize()}initializeIeCssStyles(){}initializeDummyElements(){}initializeRules(){}removeDummyElements(){}}class o extends r{initializeIeCssStyles(){this._ieCssStyles=[new l(["th:focus .dxColumnResizeAnchor"],["box-shadow: 0 0 0 1px --dx-accent-shadow-color;"]),new l(["th:focus .dxColumnResizeAnchor::after"],["border-left: 1px solid --dx-accent-color;","border-right: 1px solid --dx-accent-color;"]),new l([".table th:focus:before"],["box-shadow: 0 0 0 2px --dx-accent-color;"])]}initializeDummyElements(){const e=this.createDummyContainer(),t=this.createDummyLink();e.appendChild(t),document.documentElement.appendChild(e),this._dummyContainer=e}createDummyContainer(){const e=document.createElement("DIV");return e.style.top=s(-1e4),e.style.left=s(-1e4),e.className="dxAIFE",e.setAttribute("aria-hidden",!0),e}createDummyLink(){const e=document.createElement("A");return e.innerHTML="test",e.href="javascript:;",e}removeDummyElements(){document.documentElement.removeChild(this._dummyContainer),this._dummyContainer=null}initializeRules(){const e=this.getAccentColor();this._rules.push(new n("--dx-accent-color",e)),this._rules.push(new n("--dx-accent-shadow-color",this.getAccentShadowColor(e)))}}class a extends r{initializeIeCssStyles(){this._ieCssStyles=[new l([".dxbs-gridview.dxbs-has-vertical-scrollbar.dxbs-vertical-scrollbar-visible > .card > .dxgvHSDC.dxbs-scrollbar-padding"],["padding-right: --dx-scrollbar-width;"])]}initializeRules(){this._rules.push(new n("--dx-scrollbar-width",s(t())))}}function c(){window.dxAccentColorStyle||(window.dxAccentColorStyle=new o)}function m(){void(i||(i=new a)),i.update()}const u={ensureAccentColorStyle:c};export default u;export{c as ensureAccentColorStyle,m as updateScrollbarStyle};
