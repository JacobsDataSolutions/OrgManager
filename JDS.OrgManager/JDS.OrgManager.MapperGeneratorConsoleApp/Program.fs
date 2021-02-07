open System
open System.Reflection
open System.IO
open JDS.OrgManager.Domain.Abstractions.Models
open JDS.OrgManager.Domain.Models
open JDS.OrgManager.Application.Abstractions.Models
open System.Linq

type MapperTypeInfo = { Name: string; Type: Type; AssignableType: Type }
type MapperClassSpec = { ClassName: string; SourceTypeName: string; DestTypeName: string; InterfaceName: string; Namespaces: string list }

[<EntryPoint>]
let main argv =
    let assignableTypes = [ typeof<IDomainEntity>; typeof<IViewModel>; typeof<IDbEntity>; typeof<IValueObject> ]
    let allTypes =
        Directory.EnumerateFiles(".", "*.dll")
        |> Seq.filter (fun name -> name.TrimStart('.', '\\').StartsWith("JDS"))
        |> Seq.map Assembly.LoadFrom
        |> Seq.map (fun a -> a.GetTypes())
        |> Seq.concat
        |> Seq.filter (fun t -> assignableTypes |> List.exists (fun t' -> t'.IsAssignableFrom(t)))
        |> Seq.map (fun t -> { Name = t.Name; Type = t; AssignableType = assignableTypes |> List.find (fun t' -> t'.IsAssignableFrom(t)) })
        |> List.ofSeq

    let typeLookup = allTypes.ToDictionary((fun t -> t.Name), id)

    let getSuffix (info: MapperTypeInfo) = info.AssignableType.Name.TrimStart('I')
    let getRawTypeName (info: MapperTypeInfo) =
        let name = if (info.Type.IsInterface && info.Name.StartsWith('I')) then info.Name.Substring(1) else info.Name
        name.Replace("Entity", "").Replace("ViewModel", "")
    let getMapperInterfaceName (first: MapperTypeInfo) (second: MapperTypeInfo) = sprintf "I%sTo%sMapper" (getSuffix first) (getSuffix second)
    let getSuggestedClassName (rootName: string) (first: MapperTypeInfo) (second: MapperTypeInfo) = sprintf "%s%sTo%sMapper" rootName (getSuffix first) (getSuffix second)

    let typePairs =
        File.ReadAllLines(argv.[1])
        |> Seq.map (fun line ->
                        let split = line.Split("->") |> Array.map (fun name -> typeLookup.[name])
                        (split.[0], split.[1])
                    )
        |> List.ofSeq

    let classSpecs =
        typePairs
        |> List.map (fun (first, second) ->
                        let rootName = (if first.Name.Length > second.Name.Length then first else second) |> getRawTypeName
                        {
                            ClassName = getSuggestedClassName rootName first second;
                            SourceTypeName = first.Name;
                            DestTypeName = second.Name;
                            InterfaceName = sprintf "%s<%s, %s>" (getMapperInterfaceName first second) first.Name second.Name;
                            Namespaces = [ first.Type.Namespace; second.Type.Namespace ]
                        }
                    )

    let allNamespaces =
        "JDS.OrgManager.Application.Abstractions.Mapping" :: (classSpecs |> List.map (fun specs -> specs.Namespaces) |> List.concat |> List.distinct |> List.sort)
        |> List.map (fun ns -> "using " + ns + ";")

    let getClassDef (spec: MapperClassSpec) =
        sprintf @"    public partial class %s : MapperBase<%s, %s>, %s
    { }" spec.ClassName spec.SourceTypeName spec.DestTypeName spec.InterfaceName

    let allClassDefs = classSpecs |> List.sortBy (fun def -> def.ClassName) |> List.map getClassDef

    let classDeclarations = String.Join("\r\n\r\n", allClassDefs)
    let namespaceDeclaration = String.Join("\r\n", allNamespaces)

    let fileText =
        sprintf @"%s

namespace JDS.OrgManager.Application.Common.Mapping
{
%s
}"          namespaceDeclaration classDeclarations

    File.WriteAllText(argv.[0], fileText)
    0