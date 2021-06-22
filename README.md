# Fun.Blazor [![Nuget](https://img.shields.io/nuget/v/Fun.Blazor)](https://www.nuget.org/packages/Fun.Blazor)

This is a project to make F# developer to write blazor easier.

It is based on [bolero](https://github.com/fsbolero/Bolero) and  [Feliz.Engine](https://github.com/alfonsogarciacaro/Feliz.Engine)

[WASM side docs](https://slaveoftime.github.io/Fun.Blazor/)

[Server side docs](https://funblazor.slaveoftime.fun)



## Fun.Blazor


### Create a WASM app

    * Other resources like index.html should be put under wwwroot. You can check Fun.Blazor.Docs.Wasm project for detail

```
dotnet add package Fun.Blazor
```

```fsharp
open System
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open MudBlazor.Services
open Fun.Blazor

let app = html.text "hello world"

let builder = WebAssemblyHostBuilder.CreateDefault(Environment.GetCommandLineArgs())
        
builder
    .AddFunBlazorNode("#app", app)
    .Services
        .AddFunBlazor()
    |> ignore
        
builder.Build().RunAsync() |> ignore
```

### Create a blazor server app

```
dotnet add package Fun.Blazor.Server
```

```fsharp
open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open MudBlazor.Services
open Fun.Blazor.Docs.Server
open Fun.Blazor
open Fun.Blazor.Server

// Currently the we need to define a class to render for server side blazor. In the future if I found some workaround this could be simpler
type Index () =
    inherit Bolero.Component()

    override this.Render() = html.text "hello world" |> html.toBolero

    static member page =
        html.doctypeHtml [
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
                        html.root<Index>()
                        html.bolero Bolero.Server.Html.boleroScript
                    ]
                ]
            ])
        ]

Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
    .ConfigureWebHostDefaults(fun builder ->
        builder
            .ConfigureServices(fun (services: IServiceCollection) ->
                services.AddControllersWithViews() |> ignore
                services
                    .AddServerSideBlazor().Services
                    .AddFunBlazorServer() |> ignore)
            .Configure(fun (application: IApplicationBuilder) ->
                application
                    .UseStaticFiles()
                    .UseRouting()
                    .UseEndpoints(fun endpoints ->
                        endpoints.MapBlazorHub() |> ignore
                        endpoints.MapFallbackToFunBlazor(Index.page) |> ignore) |> ignore) |> ignore)
    .Build()
    .Run()
```
