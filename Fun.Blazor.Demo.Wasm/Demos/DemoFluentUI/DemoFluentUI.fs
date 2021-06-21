﻿[<AutoOpen>]
module Fun.Blazor.Demo.Wasm.DemoFluentUI.Demo

open Fun.Blazor
open Fun.Blazor.Demo.Wasm.Components


let private rootDir = "Demos/DemoFluentUI"


let demoFluentUI =
    html.div [
        simplePage
            "https://www.fast.design/"
            "Fast FluentUI"
            "The adaptive interface system for modern web experiences"
            "Interfaces built with FAST adapt to your design system and can be used with any modern UI Framework by leveraging industry standard Web Components."
            [
                demoContainer "Skeleton" $"{rootDir}/SkeletonDemo" skeletonDemo
            ]
        html.script [
            attr.src "https://unpkg.com/@fluentui/web-components"
            attr.type' "module"
        ]
    ]
