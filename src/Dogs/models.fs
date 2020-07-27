module Dogs.Models

open Thoth.Json

type Dog =
    { url: string }

let dogDecoder: Decoder<Dog> =
    Decode.object (fun fields -> { url = fields.Required.Raw Decode.string })

let dogsDecoder: Decoder<Dog list> =
    Decode.object (fun fields -> fields.Required.At [ "message" ] (Decode.list dogDecoder))
