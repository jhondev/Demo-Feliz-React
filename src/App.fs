module App

open Browser.Dom
open Fable.Core.JsInterop
open Feliz
open Feliz.Router
open Models

importAll "../styles/main.scss"

let routerComponent =
    React.functionComponent (fun _ ->
        let (currentUrl, setUrl) = React.useState (Router.currentUrl ())

        let currentPage =
            match currentUrl with
            | [] -> Home.view
            | [ "users" ] -> Users.usersComponent ()
            | [ "dogs" ] -> Dogs.dogsComponent ()
            | _ -> Html.h1 "Not found"

        let layout = Layout.view currentPage

        React.router
            [ router.onUrlChanged (fun url -> setUrl url)
              router.children layout ])

let AppComponent =
    React.functionComponent (fun _ ->
        let appUser =
            { name = "user name"
              email = "user@email.com" }

        let (user, setUser) = React.useState (appUser)

        let contextValue =
            { Context.initialState with
                  appUser = user
                  changeUser = setUser }

        React.contextProvider (Context.appContext, contextValue, routerComponent ()))

ReactDOM.render (AppComponent, document.getElementById "root")
