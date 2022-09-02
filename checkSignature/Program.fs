open System.Security.Cryptography.Xml
open System.Security.Cryptography.X509Certificates
open System.Reflection
open System.Diagnostics
open System.Xml

let traceSource = 
    Assembly.Load("System.Security.Cryptography.Xml")
        .GetType("System.Security.Cryptography.Xml.SignedXmlDebugLog")
        .GetField("s_traceSource", (BindingFlags.Static ||| BindingFlags.NonPublic))
        .GetValue(null) :?> TraceSource
traceSource.Listeners.Add(new TextWriterTraceListener(System.Console.Out)) |> ignore
traceSource.Switch.Level <- SourceLevels.Verbose


let xmlDocument = XmlDocument(PreserveWhiteSpace=true)
