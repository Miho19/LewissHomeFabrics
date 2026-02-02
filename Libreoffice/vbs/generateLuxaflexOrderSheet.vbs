
Dim BlindCDATADictionary
Dim CustomerInformationCDATADictionary
Dim BlindsInformationDictionary
Dim CustomerDictionary
Dim ExcelApplication
Dim workbook

Dim TemplateFilePath
Dim TempFilePath
Dim SaveFilePath
Dim PDFSavePath

Dim DEV

Public Sub Main

    Set BlindCDATADictionary = CreateObject("Scripting.Dictionary")
    Set CustomerInformationCDATADictionary = CreateObject("Scripting.Dictionary")
    Set BlindsInformationDictionary = CreateObject("Scripting.Dictionary")
    Set CustomerDictionary = CreateObject("Scripting.Dictionary")

    Dim oArgs
    Set oArgs = WScript.Arguments

    If oArgs.Count <> 4 Then
        WScript.Echo("Wrong Argument Count")
        WScript.Quit()
    End If

    
    DEV = FALSE

    '' TemplateFilePath, TempFile.txt, workbookSavePath, PDFSavePath
    TemplateFilePath = "" & Trim(oArgs(0)) & ""
    TempFilePath = "" & Trim(oArgs(1)) & ""
    SaveFilePath = "" & Trim(oArgs(2)) & ""
    PDFSavePath = "" & Trim(oArgs(3)) & ""

    If Not DEV Then Call FilePathSanitise

    Dim FileAsText
    FileAsText=""
    Call GetFileAsText(FileAsText)
    
    If Len(FileAsText)=0 Then
        WScript.Echo("No Data Found")
        WScript.Quit()
    End If

    Call PopulateDictionaries(FileAsText)
    Call PopulateTemplateFile


End Sub

Private Sub FilePathSanitise()

    Dim FSO
	Set FSO = CreateObject("Scripting.FileSystemObject")

    If Not FSO.FileExists(TemplateFilePath) Then
        wscript.Echo "Template File does not exist." & CHR(10) & TemplateFilePath
        wscript.quit()
    End If

    If Not FSO.FileExists(TempFilePath) Then
        WScript.Echo("File does not exist:" & CHR(10) & TempFilePath)
        WScript.Quit()
    End If

    If DEV Then Exit Sub

    If FSO.FileExists(SaveFilePath) Then
        FSO.DeleteFile(SaveFilePath)
    End If

    If FSO.FileExists(PDFSavePath) Then
        FSO.DeleteFile(PDFSavePath)
    End If

End Sub

Private Sub PopulateTemplateFileCustomerInformation(workbook)
    
    Dim MainSheet
    Set MainSheet = workbook.Worksheets(CustomerDictionary.Item("sheetname"))

    Dim Keys
    Keys = CustomerInformationCDATADictionary.Keys

    Dim CDATAIndex, Value, outstring
    For Each Key in Keys
        CDATAIndex = UCASE(Trim(CustomerInformationCDATADictionary.Item(Key)))
        Value = CustomerDictionary.Item(Key)

        If CDATAIndex <> "0" Then
            MainSheet.Range(CDATAIndex).Value = Value
        End if
    Next

End Sub

Private Sub PopulateTemplateFile()

    If DEV Then
        Set ExcelApplication = GetObject(, "Excel.Application")
        ExcelApplication.Visible = True
        ExcelApplication.Workbooks.Open TemplateFilePath
    Else
        Set ExcelApplication = createObject("Excel.Application")
        ExcelApplication.Visible = False
        ExcelApplication.Workbooks.add(TemplateFilePath)
    End IF
    
    Dim workbook
    Set workbook = ExcelApplication.ActiveWorkbook

    Call PopulateTemplateFileCustomerInformation(workbook)
    Call PopulateTemplateFileBlindInformation(workbook)
    Call GeneratePDF(Workbook)

    If Not DEV Then
        workbook.saveAs SaveFilePath
        workbook.Close
        ExcelApplication.Quit
    Else
        WScript.Echo "Completed"
    End If

End Sub

Private Sub GeneratePDF(Workbook)
    workbook.Worksheets(CustomerDictionary.Item("sheetname")).Activate
    
    Dim MainSheet
    Set MainSheet = workbook.ActiveSheet

    MainSheet.Range(MainSheet.PageSetup.PrintArea).ExportAsFixedFormat 0, PDFSavePath, 0, 1, 0, , , 0

End Sub

Private Sub GetFileAsText(FileAsText)
   	Dim FSO
	Set FSO = CreateObject("Scripting.FileSystemObject")

    Dim File
    Set File = FSO.GetFile(TempFilePath)

    Dim TextStream
    Set TextStream = File.OpenAsTextStream(1, -2)

    FileAsText = TextStream.ReadAll()
    TextStream.Close()

End Sub

Private Sub PopulateDictionaryWithValues(Dictionary, ValueString)
    Dim StringSplit,ValueSplit, Key, Value
    StringSplit = Split(ValueString, ";")

    For i = 0 TO UBound(StringSplit)
        ValueSplit = Split(StringSplit(i), "=", 2)
        key = Trim(Replace(ValueSplit(0), CHR(13), ""))
        Value = Trim(Replace(ValueSplit(1), CHR(13), ""))
        Dictionary.Add Key, Value
    Next
End Sub

Private Sub PopulateDictionaries(FileAsText)

    Dim SplitArray
    SplitArray = Split(FileAsText, CHR(10))
    
    Dim BlindCDATAString, CustomerCDATAString, CustomerInformationString, BlindInformationString
    BlindCDATAString = SplitArray(1)
    CustomerCDATAString = SplitArray(0)
    CustomerInformationString = SplitArray(2)
    
    Call PopulateDictionaryWithValues(CustomerInformationCDATADictionary, CustomerCDATAString)
    Call PopulateDictionaryWithValues(BlindCDATADictionary, BlindCDATAString)
    Call PopulateDictionaryWithValues(CustomerDictionary, CustomerInformationString)
    Call PopulateBlindInformationDictionary(BlindsInformationDictionary, SplitArray)


End Sub

Private Sub PopulateBlindInformationDictionary(Dictionary, SplitArray)
    Dim Key, ValueString, SplitValue
    For i = 3 To UBound(SplitArray)
        SplitValue = Split(SplitArray(i), ":", 2)
        Key = SplitValue(0)
        ValueString = SplitValue(1)
        Dictionary.Add Key, ValueString
    Next

End Sub


Private Sub PrintDictionary(Dictionary)
    Dim Keys, Value, outputString
    If Not Dictionary.Count > 0 Then Exit Sub

    Keys = Dictionary.Keys

    For Each Key in Keys
        Value = Dictionary.Item(Key)
        outputString = outputString & Key & ":" & Value & CHR(10)
    Next

    outputString = LEFT(outputString, Len(outputString) - 1)
    Wscript.Echo(outputString)
End Sub

Private Sub PopulateTemplateFileBlindInformation(workbook)
    
    workbook.Worksheets(CustomerDictionary.Item("sheetname")).Activate
    
    Dim MainSheet
    Set MainSheet = workbook.ActiveSheet

    Dim BlindDictionary
    Set BlindDictionary = CreateObject("Scripting.Dictionary")

    Dim Range
    Set Range = MainSheet.Range(CustomerDictionary.Item("blindrange"))

    Dim RowIndexes
    RowIndexes = BlindsInformationDictionary.Keys

    Dim CurrentBlindString
    For Each Row in RowIndexes
        CurrentBlindString = BlindsInformationDictionary.Item(Row)
        Call ConvertBlindStringToDictionary(BlindDictionary, CurrentBlindString)
        Call AddBlindInformationToRow(Range, Row, BlindDictionary)
    Next


End Sub


Private Sub ConvertBlindStringToDictionary(Dictionary, BlindString)
    Dictionary.RemoveAll()
    
    Dim SplitArray
    SplitArray = Split(BlindString, ";")

    Dim Key, Value, StringValue, SplitString
    For i = 0 TO UBOUND(SplitArray)
        StringValue = TRIM(Replace(SplitArray(i), CHR(13), ""))
        SplitString = Split(StringValue, "=")
        Key = SplitString(0)
        Value = SplitString(1)
        Dictionary.Add Key, Value
    Next
    
End Sub

Private Sub AddBlindInformationToRow(Range, Row, BlindDictionary)
    Row = CINT(Row) + 1
    Dim Keys
    Keys = BlindCDATADictionary.Keys
    Dim outstring
    outstring = ""
    Dim CellIndex, Value
    For Each Key in Keys
        CellIndex = CINT(BlindCDATADictionary.Item(Key)) + 1
        Value = BlindDictionary.Item(Key)

        Range.Cells(Row, CellIndex).Value = Value

    Next

End Sub






Main()
