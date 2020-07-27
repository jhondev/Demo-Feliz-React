module Users

open Feliz

open System

// http://www.fssnip.net/7UY/title/Random-string-generator
let ranStr n =
    let r = Random()

    let chars =
        Array.concat
            ([ [| 'a' .. 'z' |]
               [| 'A' .. 'Z' |]
               [| '0' .. '9' |] ])

    let sz = Array.length chars
    String(Array.init n (fun _ -> chars.[r.Next sz]))

let usersComponent =
    React.functionComponent (fun () ->
        let context = React.useContext (Context.appContext)
        Html.div
            [ Html.h1 "Users page"
              Html.p context.appUser.name
              Html.p context.appUser.email

              Html.button
                  [ prop.text "Change user name"
                    prop.onClick (fun _ ->
                        context.changeUser ({ context.appUser with name = (sprintf "new name: %s" (ranStr 5)) })) ] ])
