open System
open System.IO
open System.Reflection
open System.Linq
open System.Diagnostics

[<EntryPoint>]
let main argv =
    let restrictedPropertyNames = [ "name"; "length"; "prototype" ]
    printfn "Generating typescript constants..."
    //argv |> Array.iter (fun arg -> printfn "%s" arg)
    let outputFilePath = @"ClientApp\src\app\shared\"
    let types = [(typedefof<int>, "number"); (typedefof<string>, "string")]
    let toCamelCase (str: string) =
        if String.IsNullOrWhiteSpace(str) then ""
        else
            let chars = str.ToCharArray();
            chars.[0] <- Char.ToLower(chars.[0])
            new string(chars)
    let toTsType (typ: Type) = List.find(fun t -> (fst t) = typ) types |> snd
    let makeSafeName (name: string) = if List.exists (fun n -> n = name) restrictedPropertyNames then "_" + name else name
    let getValue (field: FieldInfo) =
        let value = field.GetValue(null)
        if (field.FieldType = typedefof<string>) then sprintf "\"%A\"" value else sprintf "%A" value

    Assembly
        .LoadFrom(argv.[0] + "JDS.OrgManager.Application.dll")
        .GetTypes()
        |> Seq.filter(fun t ->
                            t.IsClass &&
                            t.IsAbstract &&
                            t.IsSealed)
        |> Seq.map (fun t -> t.Name, t.GetFields(BindingFlags.Public ||| BindingFlags.Static ||| BindingFlags.FlattenHierarchy)
                                        |> Array.filter(fun field -> field.IsLiteral && not <| field.IsInitOnly))
        |> Seq.filter (fun (_, fields) -> not <| Array.isEmpty fields)
        |> Seq.map (fun (name, fields) ->
                        let tsFileName = argv.[1] + outputFilePath + name.ToLower() + ".ts"

                        let consts =
                            fields
                            |> Array.map (fun field -> sprintf "    static readonly %s: %s = %s;" (field.Name |> toCamelCase |> makeSafeName) (toTsType field.FieldType) (getValue field))

                        (tsFileName, sprintf @"
export class %s {
%s
}"                          name (String.Join("\r\n", consts)))
                    )
        |> Seq.iter (fun (tsFileName, tsFileContents) -> File.WriteAllText(tsFileName, tsFileContents))
    printfn "Done generating typescript constants."
    0 // return an integer exit code