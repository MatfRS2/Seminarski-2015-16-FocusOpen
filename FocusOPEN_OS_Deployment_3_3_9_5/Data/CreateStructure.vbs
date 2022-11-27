'-------------------------------------------------------------------------------
' FocusOPEN Digital Asset Manager
' Create Data Folder Structure
'-------------------------------------------------------------------------------

Option Explicit



Dim oFs: Set oFs = CreateObject("Scripting.FileSystemObject")

'Create folders
if (Not oFs.FolderExists("AssetBitmapGroups")) Then Call oFs.CreateFolder("AssetBitmapGroups")
if (Not oFs.FolderExists("AssetFiles")) Then Call oFs.CreateFolder("AssetFiles")
if (Not oFs.FolderExists("AssetFilesZipped")) Then Call oFs.CreateFolder("AssetFilesZipped")
if (Not oFs.FolderExists("AssetPreviews")) Then Call oFs.CreateFolder("AssetPreviews")
if (Not oFs.FolderExists("AssetThumbnails")) Then Call oFs.CreateFolder("AssetThumbnails")
if (Not oFs.FolderExists("CachedAssetFiles")) Then Call oFs.CreateFolder("CachedAssetFiles")
if (Not oFs.FolderExists("Homepage")) Then Call oFs.CreateFolder("Homepage")
if (Not oFs.FolderExists("Temp")) Then Call oFs.CreateFolder("Temp")

SetACLS()

Set oFs = Nothing

WScript.Echo("Done creating folders")

Sub SetACLS()
	
	Dim currentDirectory: currentDirectory = Left(WScript.ScriptFullName, (Len(WScript.ScriptFullName))-(Len(WScript.ScriptName))-1)
	Dim aclCommand: aclCommand = "%COMSPEC% /c Echo Y| cacls " & Chr(34) & currentDirectory & Chr(34) & " /T /G Everyone:F"
	
	Dim oShell: Set oShell = CreateObject("WSCript.Shell") 
	Call oShell.Run(aclCommand, 0, False)
	Set oShell = Nothing
	
End Sub