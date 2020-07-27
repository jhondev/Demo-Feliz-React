module Context

open Feliz
open Models

type ContextState =
    { appName: string
      appUser: User
      changeUser: User -> Unit }

let initialState =
    { appName = "Dogs App"
      appUser =
          { name = ""
            email = "" }
      changeUser = fun _ -> () }

let appContext = React.createContext (defaultValue = initialState)
