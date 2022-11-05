﻿// hot-reload
module Fun.Blazor.Docs.Wasm.Demos.CustomElementDemo

open Fun.Blazor


let private count =
    html.inject (fun (store: IShareStore) ->
        let count = store.CreateCVal("count", 1)
        div {
            adaptiview () {
                let! count, setCount = count.WithSetter()
                button {
                    onclick (fun _ -> setCount (count + 1))
                    $"count = {count}"
                }
            }
        }
    )


let entry = div {
    p { "Below will render two custome elements which will powered by blazor" }
    CustomElt.customElt count
    CustomElt.customElt count
}
