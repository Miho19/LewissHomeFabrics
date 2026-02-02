	Set oArgs = wscript.arguments
	
	If Not oArgs.count = 4 Then
		wscript.echo "Wrong Argument Count"
		wscript.Quit
	End If

	Dim FSO
	Set FSO = CreateObject("Scripting.FileSystemObject")

	pathstring = "" & Trim(oArgs(3)) & ""

	if not FSO.FileExists(pathstring) Then
		wscript.echo "Attachment does not exist"
		wscript.Quit
	end if

	Dim BodyFilePath
	BodyFilePath = "" & Trim(oArgs(0)) & ""
	If Not FSO.FileExists(BodyFilePath) Then
		WScript.Echo "Body File Not Found"
		WScript.Quit
	End If

	
	Dim OutLookObject
	Set OutLookObject = CreateObject("Outlook.Application")

	If OutLookObject is nothing Then
		wscript.echo "Failed to load outlook."
		wscript.Quit
	End if
 
	Dim objMailItem
	Set objMailItem = OutLookObject.createitem(0)

	
	
	Dim bodyFile
	Dim bodyTS
	Dim bodyText

	set bodyFile = FSO.getFile(BodyFilePath)
	
	set bodyTS = bodyFile.OpenAsTextStream(1, -2)
	
	bodyFile = bodyTS.readAll

	bodyTS.Close

	
	objMailItem.Subject = oArgs(1)
	objMailItem.Recipients.Add oArgs(2)

	
	objMailItem.Attachments.Add pathstring
	objMailItem.Attachments.Add "K:\Data\Process\Attachments\Kinetics Art in Motion.pdf"
	objMailItem.Attachments.Add "K:\Data\Process\Attachments\Lewis's Terms and Conditions.pdf"
	objMailItem.Attachments.Add "K:\Data\Info\Email Templates\Email Tags\KineticsEmailTag.jpg", 1, 0
	objMailItem.Attachments.Add "K:\Data\Info\Email Templates\Email Tags\Facebook.png", 1, 0
	objMailItem.Attachments.Add "K:\Data\Info\Email Templates\Email Tags\Instagram.png", 1, 0


	objMailItem.HTMLBody = bodyFile
	objMailItem.Display

	Wscript.Sleep 1000

	Set FSO = Nothing
	Set objMailItem = Nothing
	Set OutLookObject = Nothing



	

	