'-------------------------------------------------------------------------------
' FocusOPEN Digital Asset Manager
' Delete all asset files (ie. to perform a full reset)
'-------------------------------------------------------------------------------

Option Explicit



If (WScript.Arguments.Count = 0) Then
  If (MsgBox("Are you sure", vbYesNo, "Confirm") = vbNo) Then
    WScript.Quit()
  End If
End If


Dim oFs: Set oFs = CreateObject("Scripting.FileSystemObject")
Dim oRootFolder: Set oRootFolder = oFs.GetFolder(".")

Dim oSubFolder, oFile, iCount: iCount = 0


For Each oSubFolder in oRootFolder.SubFolders
	If (oSubFolder.Name <> "Homepage") Then
		For Each oFile in oSubFolder.Files
			Call oFs.DeleteFile(oFile)
			iCount = iCount + 1
		Next
	End If
Next


If (WScript.Arguments.Count = 0) Then
  WScript.Echo("Done.  Deleted " & iCount & " files")
End If