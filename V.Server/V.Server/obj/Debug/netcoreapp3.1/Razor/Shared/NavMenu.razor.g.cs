#pragma checksum "C:\projects\Vitkovice\V.Server\V.Server\Shared\NavMenu.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ecc3e42740a9f7cc9b0db24c7a4350efc5b94042"
// <auto-generated/>
#pragma warning disable 1591
namespace V.Server.Shared
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\projects\Vitkovice\V.Server\V.Server\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\projects\Vitkovice\V.Server\V.Server\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\projects\Vitkovice\V.Server\V.Server\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\projects\Vitkovice\V.Server\V.Server\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\projects\Vitkovice\V.Server\V.Server\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\projects\Vitkovice\V.Server\V.Server\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\projects\Vitkovice\V.Server\V.Server\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\projects\Vitkovice\V.Server\V.Server\_Imports.razor"
using V.Server;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\projects\Vitkovice\V.Server\V.Server\_Imports.razor"
using V.Server.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "C:\projects\Vitkovice\V.Server\V.Server\_Imports.razor"
using DevExpress.Blazor;

#line default
#line hidden
#nullable disable
    public partial class NavMenu : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenComponent<DevExpress.Blazor.DxTreeView>(0);
            __builder.AddAttribute(1, "AllowSelectNodes", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 1 "C:\projects\Vitkovice\V.Server\V.Server\Shared\NavMenu.razor"
                              true

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(2, "CssClass", "app-sidebar");
            __builder.AddAttribute(3, "Nodes", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(4, "\r\n        ");
                __builder2.OpenComponent<DevExpress.Blazor.DxTreeViewNode>(5);
                __builder2.AddAttribute(6, "NavigateUrl", "");
                __builder2.AddAttribute(7, "Text", "Overview");
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(8, "\r\n        ");
                __builder2.OpenComponent<DevExpress.Blazor.DxTreeViewNode>(9);
                __builder2.AddAttribute(10, "NavigateUrl", "datagrid");
                __builder2.AddAttribute(11, "Text", "Data Grid");
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(12, "\r\n    ");
            }
            ));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
