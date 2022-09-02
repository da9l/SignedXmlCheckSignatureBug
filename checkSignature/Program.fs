open System.Security.Cryptography.Xml
open System.Security.Cryptography.X509Certificates
open System.Reflection
open System.Diagnostics
open System.Xml
open System.IO
open FSharpPlus
open System.Text

let traceSource = 
    Assembly.Load("System.Security.Cryptography.Xml")
        .GetType("System.Security.Cryptography.Xml.SignedXmlDebugLog")
        .GetField("s_traceSource", (BindingFlags.Static ||| BindingFlags.NonPublic))
        .GetValue(null) :?> TraceSource
traceSource.Listeners.Add(new TextWriterTraceListener(System.Console.Out)) |> ignore
traceSource.Switch.Level <- SourceLevels.Verbose

let prependSrcDir fileName =
    Path.Combine [|__SOURCE_DIRECTORY__;fileName|]


    
type SignatureVerificationError =
    | UnableToLoadFile of exn
    | IncorrectNumberOfSignatureNodes of int
    | FailedToLoadSignature of exn
    | SignatureInvalid of string
    | Other
    
let signedFiles = 
    ["sampleFiles\signedNOT_OK_xdsNS_inSignatureNode.xml.txt";"sampleFiles\signedOK_xdsNS_inRoot.xml.txt"]
    |> List.map prependSrcDir

let readFileAsUtf8 filePath =
    File.ReadAllText(filePath,Encoding.UTF8)

let getSignatureElement (xml:XmlDocument) =
    let nodes = xml.GetElementsByTagName("Signature",SignedXml.XmlDsigNamespaceUrl)
    if nodes.Count <> 1 then
        nodes.Count |> IncorrectNumberOfSignatureNodes |> Error
    else
        nodes.[0] :?> XmlElement |> Ok


let verifySignature (xml:string) =
    // let xmlDocument = XmlDocument(PreserveWhitespace=true)
    // let signedXml =
    //     Result.protect xmlDocument.LoadXml xml
    //     |> Result.mapError UnableToLoadFile
    //     |> Result.map (fun () -> xmlDocument, (SignedXml xmlDocument))
    //     |> Result.map 
    monad {
        let xmlDocument = XmlDocument(PreserveWhitespace=true)
        let! signedXml =
            Result.protect xmlDocument.LoadXml xml
            |> Result.mapError UnableToLoadFile
            |> Result.map (fun () -> SignedXml xmlDocument)
        let! sigElement = xmlDocument |> getSignatureElement
        let! loadedSignedXml = 
            Result.protect signedXml.LoadXml sigElement
            |> Result.mapError FailedToLoadSignature
            |> Result.map (fun () -> signedXml)
        return! 
            loadedSignedXml.CheckSignature()
            |> function
            | true -> Ok "Signature valid!"
            | false -> Error <| (SignatureInvalid xml)

    }
    
signedFiles
|> List.map readFileAsUtf8
|> List.map verifySignature
|> List.map (sprintf "\n%A\n")
|> printfn "%A\n"

