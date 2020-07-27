module Dogs

open Dogs.Models
open Feliz
open Thoth.Json

let dogsView dogs =
    let dogView dog =
        Html.img
            [ prop.src dog.url
              prop.className "dog-image" ]
    Html.div [ prop.children [ Html.div [ prop.children [ for dog in dogs -> dogView dog ] ] ] ]

let dogsComponent =
    React.functionComponent (fun () ->
        let (loading, setLoading) = React.useState (true)
        let (dogs, setDogs) = React.useState (List.empty<Dog>)
        let (error, setError) = React.useState ("")

        let loadDogs () =
            if not loading then setLoading true
            Fetch.fetch "https://dog.ceo/api/breeds/image/random/3" []
            |> Promise.bind (fun res -> res.text ())
            |> Promise.map (fun json -> Decode.fromString dogsDecoder json)
            |> Promise.mapResult (fun dogs ->
                setLoading false
                setDogs dogs)
            |> Promise.mapResultError setError
            |> ignore

        React.useEffect ((fun () -> loadDogs ()), [||])

        let context = React.useContext (Context.appContext)

        Html.div
            [ dogsView dogs

              if loading then
                  Html.p "Loading..."
              else
                  Html.button
                      [ prop.text "Refresh dogs"
                        prop.onClick (fun _ -> loadDogs ()) ]

              Html.p context.appUser.name

              Html.h4 error ])
