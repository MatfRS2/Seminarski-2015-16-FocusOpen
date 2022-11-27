/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class UserLightboxDropDownList : AbstractDictionaryDropDownList
	{
		#region Accessors

		public bool ShowPublicLightboxes { get; set; }

        public bool HideLinkedLightboxes { get; set; }

		#endregion

		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
			Dictionary<Int32, String> ht = new Dictionary<int, string>();

			foreach (Lightbox lb in ContextInfo.LightboxManager.UserLightboxes)
			{
                // Filter out linked lightboxes if required

                if (!lb.IsLinked || !HideLinkedLightboxes)
                {
                    StringBuilder sb = new StringBuilder(lb.Name);

                    JoinableList jList = new JoinableList(", ");

                    if (lb.IsDefault)
                        jList.Add("default");

                    if (lb.IsPublic)
                        jList.Add("public");

                    if (lb.IsLinked)
                        jList.Add("linked");

                    if (jList.Count > 0)
                        sb.AppendFormat(" ({0})", jList);

                    ht[lb.LightboxId.GetValueOrDefault()] = sb.ToString();
                }
			}

			if (ShowPublicLightboxes)
			{
				// Initialise finder to get public lightboxes
				LightboxFinder finder = new LightboxFinder {IsPublic = true};

				// Non-superadmins should only see public lightboxes which are assigned
				// to a brand to which they are also assigned (so a user cannot see lightboxes
				// assigned to brands to which they do not have access).
				if (SessionInfo.Current.User.UserRole != UserRole.SuperAdministrator && BrandManager.IsMultipleBrandMode)
				{
					finder.BrandIdList.Add(0);
					finder.BrandIdList.AddRange(SessionInfo.Current.User.Brands.Select(b => b.BrandId.GetValueOrDefault()));
				}

				// Get the lightboxes
				List<Lightbox> lightboxList = Lightbox.FindMany(finder);

				foreach (Lightbox lb in lightboxList)
				{
					int lightboxId = lb.LightboxId.GetValueOrDefault();

					// Check that the lightbox isn't already in the list
					// (In case the current user has public lightboxes)
					if (!ht.ContainsKey(lightboxId))
					{
						string lightboxName = string.Format("{0} (public)", lb.Name);
						ht.Add(lightboxId, lightboxName);
					}
				}
			}

			return ht;
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			SafeSelectValue(ContextInfo.LightboxManager.GetDefaultLightbox().LightboxId.ToString());
		}
	}
}