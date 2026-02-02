const cdoBasic=1
schema = "http://schemas.microsoft.com/cdo/configuration/"
Set objMessage = CreateObject("CDO.Message")

Set oArgs = wscript.arguments

If Not oArgs.count = 2 Then
	wscript.echo "usage: wscript generateOrderEmail.vbs <subject> <ORDER PDF PATH>"
	wscript.Quit
End If


Dim CCArray
'' CCArray = Array("processing@lewiss.co.nz", "joshua.april@lewiss.co.nz")
CCArray = Array()

Dim subject
Dim pdfPath

subject = oArgs(0)
pdfPath = "" & Trim(oArgs(1)) & ""

With objMessage
    .From = "joshua.april@lewiss.co.nz"
    .To = "joshua.april@lewiss.co.nz"
    .CC = Join(CCArray, ",") 
    .Subject = subject 
    .AddAttachment pdfPath
    .CreateMHTMLBody "K:\Data\Process\Development\Email\LuxaflexOrder.htm"
    With .Configuration.Fields
            .Item (schema & "sendusing") = 2
            .Item (schema & "smtpserver") = "lewiss-co-nz.mail.protection.outlook.com"
            .Item (schema & "smtpserverport") = 25
    End With
    .Configuration.Fields.Update
    .Send
End With