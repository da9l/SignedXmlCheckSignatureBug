# Reproduction of SignedXml.CheckSignature bug

The sample files verifies using this [verification service](https://www.rtr.at/TKP/was_wir_tun/vertrauensdienste/Signatur/signaturpruefung/Pruefung.en.html)

|File|Verifies in Service|Verifies in SignedXml.CheckSignature|
|---|---|---|
|[signedNOT_OK_xdsNS_inSignatureNode.xml.txt](checkSignature/sampleFiles/signedNOT_OK_xdsNS_inSignatureNode.xml.txt)|Yes|?|
|[signedOK_xdsNS_inRoot.xml.txt](checkSignature/sampleFiles/signedOK_xdsNS_inRoot.xml.txt)|Yes|?|

