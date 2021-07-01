# Fun.Blazor [![Nuget](https://img.shields.io/nuget/v/Fun.Blazor)](https://www.nuget.org/packages/Fun.Blazor)

This is a project to make F# developer to write blazor easier.

It is based on [bolero](https://github.com/fsbolero/Bolero) and  [Feliz.Engine](https://github.com/alfonsogarciacaro/Feliz.Engine)

[WASM side docs](https://slaveoftime.github.io/Fun.Blazor/)

[Server side docs](https://funblazor.slaveoftime.fun)



## Fun.Blazor

### Simple dsl

```fsharp
let app =
    html.div [
        attr.styles [
            style.margin 10
        ]
        html.text "Hello baisc usage 2"
    ]
```

Auto generated MudBlazor dsl

```fsharp
let app =
    mudCard.create [
        mudCard.childContent [
            mudAlert.create [
                mudAlert.icon Icons.Filled.AccessAlarm
                mudAlert.childContent "Mud alert"
            ]
        ]
    ]
```

### Create a WASM app

    * Other resources like index.html should be put under wwwroot. You can check Fun.Blazor.Docs.Wasm project for detail

```
dotnet add package Fun.Blazor
```

```fsharp
open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Fun.Blazor

let app = html.text "hello world"

let builder = WebAssemblyHostBuilder.CreateDefault(Environment.GetCommandLineArgs())
        
builder
    .AddFunBlazorNode("#app", app)
    .Services.AddFunBlazor() |> ignore
        
builder.Build().RunAsync() |> ignore
```

### Create a blazor server app

```
dotnet add package Fun.Blazor
dotnet add package Bolero.Server
```

```fsharp
open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Bolero.Server.Html
open Fun.Blazor.Docs.Server
open Fun.Blazor
open Fun.Blazor

// Currently the we need to define a class to render for server side blazor. In the future if I found some workaround this could be simpler
type Index () =
    inherit Bolero.Component()

    override this.Render() = html.text "hello world" |> html.toBolero

    static member page =
        doctypeHtml [] [
            html.html ("en", [
                html.head [
                    html.title "Fun Blazor"
                    html.baseUrl "/"
                    html.meta [ attr.charsetUtf8 ]
                    html.meta [ attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0" ]
                ]
                html.body [
                    attr.styles [ style.margin 0 ]
                    attr.childContent [
                        html.bolero rootComp<Index>
                        html.bolero Bolero.Server.Html.boleroScript
                    ]
                ]
            ])
            |> html.toBolero
        ]

Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
    .ConfigureWebHostDefaults(fun builder ->
        builder
            .ConfigureServices(fun (services: IServiceCollection) ->
                services.AddControllersWithViews() |> ignore
                services
                    .AddServerSideBlazor().Services
                    .AddBoleroHost(true, true)
                    .AddFunBlazor() |> ignore)
            .Configure(fun (application: IApplicationBuilder) ->
                application
                    .UseStaticFiles()
                    .UseRouting()
                    .UseEndpoints(fun endpoints ->
                        endpoints.MapBlazorHub() |> ignore
                        endpoints.MapFallbackToBolero(Index.page) |> ignore) |> ignore) |> ignore)
    .Build()
    .Run()
```
