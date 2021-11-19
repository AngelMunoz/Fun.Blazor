﻿[<AutoOpen>]
module Fun.Blazor.DslWatch

open System
open Bolero
open Bolero.Html


type FunBlazorHtmlEngine with
    member html.watch (store: IObservable<'T>, render: 'T -> Node, defaultValue: 'T, ?key) =
        Bolero.Node.BlazorComponent<StoreComponent<'T>>
            ([
                "DefaultValue" => defaultValue
                "Store" => store
                "RenderFn" => render
                match key with
                | Some key -> Bolero.Key key
                | None -> ()
            ]
            ,[])


    member html.watch (store: IStore<'T>, render: 'T -> Node) = html.watch (store.Observable, render, store.Current)
    member html.watch (store: IStore<'T>, render: 'T -> Node list) = html.watch (store.Observable, render >> ForEach, store.Current)
    member html.watch (key, store: IStore<'T>, render: 'T -> Node) = html.watch (store.Observable, render, store.Current, key = key)
    member html.watch (key, store: IStore<'T>, render: 'T -> Node list) = html.watch (store.Observable, render >> ForEach, store.Current, key = key)
    
    member html.watch2 (store1: IStore<'T1>, store2: IStore<'T2>, render: 'T1 -> 'T2 -> Node) =
        html.watch (store1, fun s1 ->
            html.watch (store2, fun s2 ->
                render s1 s2))

    member html.watch2 (store1: IStore<'T1>, store2: IStore<'T2>, render: 'T1 -> 'T2 -> Node list) =
        html.watch2 (store1, store2, fun s1 s2 -> render s1 s2 |> ForEach)

    member html.watch3 (store1: IStore<'T1>, store2: IStore<'T2>, store3: IStore<'T3>, render: 'T1 -> 'T2 -> 'T3 -> Node list) =
        html.watch (store1, fun s1 ->
            html.watch (store2, fun s2 ->
                html.watch (store3, fun s3 ->
                    render s1 s2 s3)))

    member html.watch3 (store1: IStore<'T1>, store2: IStore<'T2>, store3: IStore<'T3>, render: 'T1 -> 'T2 -> 'T3 -> Node) =
        html.watch3 (store1, store2, store3, fun s1 s2 s3 -> [ render s1 s2 s3 ])
