# Reproduction of SignedXml.CheckSignature bug

This is a f# reproduction of the error described in this [dotnet/runtime Issue Report](https://github.com/dotnet/runtime/issues/21451)
It has enabled debugging of the library using TraceSource.

The sample files verifies using this [verification service](https://www.rtr.at/TKP/was_wir_tun/vertrauensdienste/Signatur/signaturpruefung/Pruefung.en.html)

| File                                                                                                                | Verifies in Service | Verifies in SignedXml.CheckSignature |
| ------------------------------------------------------------------------------------------------------------------- | ------------------- | ------------------------------------ |
| [signedNOT_OK_xdsNS_inSignatureNode.xml.txt](checkSignature/sampleFiles/signedNOT_OK_xdsNS_inSignatureNode.xml.txt) | Yes                 | No!                                  |
| [signedOK_xdsNS_inRoot.xml.txt](checkSignature/sampleFiles/signedOK_xdsNS_inRoot.xml.txt)                           | Yes                 | Yes!                                 |

## To build and run

```powershell
dotnet tool install
dotnet run --project checkSignature
```

N.B. The Debug information is written first for both signatures and then the result is printed afterwards.
