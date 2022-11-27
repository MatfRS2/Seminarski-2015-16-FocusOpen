'-------------------------------------------------------------------------------
' FocusOPEN Digital Asset Manager (TM)
' Temp folder cleanup script
'-------------------------------------------------------------------------------

Option Explicit



'Maximum number of hours to keep files/folders
Const MAX_HOURS_TO_KEEP = 48

'Initialise objects
Dim oFs: Set oFs = CreateObject("Scripting.FileSystemObject")
Dim oRootFolder: Set oRootFolder = oFs.GetFolder(".\Temp")

'Declare variables
Dim oSubFolder, oFile
Dim iFileCount: iFileCount = 0
Dim iFolderCount: iFolderCount = 0

'Remove any old files in the root temp folder
Call CleanFiles(oRootFolder)

'Remove any old files in subfolders
For Each oSubFolder in oRootFolder.SubFolders
  If (oSubFolder.Name <> "Homepage") Then
    Call CleanFiles(oSubFolder)
  End IF
Next

'Now delete any empty subfolders
For Each oSubFolder in oRootFolder.SubFolders
	If (oSubFolder.Files.Count = 0) Then
		If CheckDelete(oSubFolder) Then
			iFolderCount = iFolderCount + 1
		End If
	End If
Next

If (WScript.Arguments.Count = 0) Then
	WScript.Echo ("All done. Deleted " & iFileCount & " files, " & iFolderCount & " folders")
End If




'-------------------------------------------------------------------------------
'Helper Methods
'-------------------------------------------------------------------------------
Sub CleanFiles(oFolder)
	For Each oFile in oFolder.Files
		If CheckDelete(oFile) Then
			iFileCount = iFileCount+1
		End If
	Next
End Sub
'-------------------------------------------------------------------------------
Function CheckDelete(oFileFolder)
	If (DateDiff("h", oFileFolder.DateLastModified, Now) > MAX_HOURS_TO_KEEP) Then
		Call oFileFolder.Delete()
		CheckDelete = True
	Else
    CheckDelete = False
	End If	
End Function
'-------------------------------------------------------------------------------
