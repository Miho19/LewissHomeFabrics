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

	set bodyFile = FSO.getFile(oArgs(0))
	
	set bodyTS = bodyFile.OpenAsTextStream(1, -2)
	
	bodyFile = bodyTS.readAll

	bodyTS.Close

	
	objMailItem.Subject = oArgs(1)
	objMailItem.Recipients.Add oArgs(2)
	objMailItem.Attachments.Add pathstring
	objMailItem.BodyFormat = 2
	objMailItem.Display


	CurrentBody = objMailItem.HTMLBody
	Dim startdiv
	Dim endBody

	dim apos 

	apos = instr(1, CurrentBody,"<a") 	
	
	startdivpos = instr(instr(1, CurrentBody, "<div"), currentBody, ">")
	startdiv = mid(CurrentBody, 1, startdivpos)
	
	endBody = mid(currentBody, apos, Len(CurrentBody))		
	objMailItem.HTMLBody = ""
	objMailItem.HTMLBody = startDiv & bodyFile & endBody
	
	
	