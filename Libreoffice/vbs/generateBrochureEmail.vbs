	Set oArgs = wscript.arguments
	
	If Not oArgs.count >= 3 Then
		wscript.echo "Wrong Argument Count"
		wscript.Quit
	End If

	Dim FSO
	Set FSO = CreateObject("Scripting.FileSystemObject")

	Dim attachmentCollection
	Set attachmentCollection = CreateObject("Scripting.Dictionary")


	For i=3 To oArgs.count - 1
		pathstring = "" & Trim(oArgs(i)) & ""
		if not FSO.FileExists(pathstring) Then
			wscript.echo "Attachment (" & i & ") does not exist"
			wscript.Quit
		end if
		attachmentCollection.add i, pathstring
	Next	


	

	
	Dim OutLookObject
	Set OutLookObject = CreateObject("Outlook.Application")

	If OutLookObject is nothing Then
		wscript.echo "Failed to load outlook."
		wscript.Quit
	End if
 
	Dim objMailItem
	Set objMailItem = OutLookObject.createitem(0)

	for each key in attachmentCollection.Keys
		objMailItem.Attachments.Add attachmentCollection.Item(key)
	Next
	
	Dim bodyFile
	Dim bodyTS
	

		

	set bodyFile = FSO.getFile(oArgs(0))
	
	set bodyTS = bodyFile.OpenAsTextStream(1, -2)
	
	bodyFile = bodyTS.readAll

	bodyTS.Close

	
	
	objMailItem.Subject = oArgs(1)
	objMailItem.Recipients.Add oArgs(2)
	objMailItem.BodyFormat = 2
	objMailItem.display
	
	

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
	
	

	