using System;
using System.Web.SessionState;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components.Handlers
{
	public class AttachedFileHandler : BaseFileHandler, IReadOnlySessionState
	{
		public override void ProcessRequest()
		{
			int assetFileId = GetIdFromFilename();
			int assetId = WebUtils.GetIntRequestParam("AssetId", 0);

			if (assetFileId <= 0)
				throw new SystemException("Invalid attached file id: " + assetFileId);

			if (assetId <= 0)
				throw new SystemException("Invalid asset id: " + assetId);

			// Get the asset file
			AssetFile assetFile = AssetFile.Get(assetFileId);

			if (assetFile.AssetId != assetId)
				throw new SystemException("Asset mismatch");

			if (assetFile.IsNull)
				throw new SystemException("Asset file not found");

			if (assetFile.Asset.RestrictAttachedFiles && !EntitySecurityManager.CanUserDownloadAsset(SessionInfo.Current.User, assetFile.Asset))
				throw new SystemException("Access denied");

			// Log the download
			string notes = string.Format("Downloaded file: {0}", assetFile.Filename);
			AuditLogManager.LogAssetAction(assetFile.AssetId, SessionInfo.Current.User, AuditAssetAction.DownloadedAttachedFile, notes);
			AuditLogManager.LogUserAction(SessionInfo.Current.User, AuditUserAction.DownloadAttachedFile, notes);

			// Send the file
			SiteUtils.SendAttachedFile(assetFile);
		}
	}
}