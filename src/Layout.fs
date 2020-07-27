module Layout

open Feliz
open Feliz.Router

let navBar =
    Html.nav
        [ prop.children
            [ Html.button
                [ prop.text "Users"
                  prop.onClick (fun _ -> Router.navigate ("users")) ]
              Html.button
                  [ prop.text "Dogs"
                    prop.onClick (fun _ -> Router.navigate ("dogs")) ] ] ]

let view body =
    Html.div [ navBar; body ]
