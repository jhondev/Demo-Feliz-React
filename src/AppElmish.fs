//module App
//
//open Fable.SimpleHttp
//open Feliz
//open Elmish
//open Thoth.Json
//
//type Dog =
//    { url: string }
//
//let dogDecoder: Decoder<Dog> =
//    Decode.object (fun fields -> { url = fields.Required.Raw Decode.string })
//
//let dogsDecoder: Decoder<Dog list> =
//    Decode.object (fun fields -> fields.Required.At [ "message" ] (Decode.list dogDecoder))
//
//type State =
//    { Count: int
//      Loading: bool
//      Dogs: List<Dog>
//      Error: string }
//
//type AsyncOperationStatus<'t> =
//    | Started
//    | Finished of 't
//
//let init () =
//    ({ Count = 0
//       Loading = false
//       Dogs = []
//       Error = "" }, Cmd.none)
//
//type Msg =
//    | GettingDogs of AsyncOperationStatus<Result<string, string>>
//    | Increment
//    | Decrement
//
//module Cmd =
//    let fromAsync (operation: Async<'msg>): Cmd<'msg> =
//        let delayedCmd (dispatch: 'msg -> unit): unit =
//            let delayedDispatch =
//                async {
//                    let! msg = operation
//                    dispatch msg }
//
//            Async.StartImmediate delayedDispatch
//
//        Cmd.ofSub delayedCmd
//
//let update (msg: Msg) (state: State) =
//    match msg with
//    | GettingDogs Started ->
//        let nextState = { state with Loading = true }
//
//        let dogsCall =
//            async {
//                let! response =
//                    Http.request "https://dog.ceo/api/breeds/image/random/3"
//                    |> Http.method GET
//                    |> Http.send
//                if response.statusCode = 200
//                then return GettingDogs(Finished(Ok response.responseText))
//                else return GettingDogs(Finished(Error "Could not load the content"))
//            }
//        nextState, Cmd.fromAsync dogsCall
//    | GettingDogs (Finished result) ->
//        match result with
//        | Ok data ->
//            let dogsResult = Decode.fromString dogsDecoder data
//            match dogsResult with
//            | Ok dogs ->
//                ({ state with
//                       Loading = false
//                       Dogs = dogs }, Cmd.none)
//            | Error error ->
//                { state with
//                      Loading = false
//                      Error = error }, Cmd.none
//        | Error error ->
//            { state with
//                  Loading = false
//                  Error = error }, Cmd.none
//    | Increment -> { state with Count = state.Count + 1 }, Cmd.none
//    | Decrement -> { state with Count = state.Count - 1 }, Cmd.none
//
//let Dogs (state: State) (dispatch: Msg -> unit) =
//    let dogView dog =
//        Html.img
//            [ prop.src dog.url
//              prop.className "dog-image" ]
//    Html.div [ prop.children [ Html.div [ prop.children [ for dog in state.Dogs -> dogView dog ] ] ] ]
//
//let render (state: State) (dispatch: Msg -> unit) =
//
//    Html.div
//        [ Html.button
//            [ prop.onClick (fun _ -> dispatch Increment)
//              prop.text "Increment edited" ]
//
//          Html.button
//              [ prop.onClick (fun _ -> dispatch Decrement)
//                prop.text "Decrement again" ]
//
//          Html.h1 state.Count
//
//          Html.h4 state.Error
//
//          Dogs state dispatch
//
//          if state.Loading then
//              Html.h1 "Loading..."
//          else
//              Html.button
//                  [ prop.text "Load dogs"
//                    prop.onClick (fun _ -> dispatch (GettingDogs Started)) ] ]
