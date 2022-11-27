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
using System.Linq;
using System.Text.RegularExpressions;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using Daydream.Data;
using log4net;
using System.Collections.Generic;

namespace FocusOPEN.Business
{
	public class LightboxManager : BaseCLOManager
    {

        #region Properties

        /// <summary>
        /// Maximum number of CC emails allowed when sending a lightbox
        /// </summary>
        public static int MaxNumberCCEmails { get; set; }
        
        #endregion


        #region Static Methods

        private static void ValidateLightboxName(string name)
		{
			if (StringUtils.IsBlank(name))
				throw new InvalidLightboxException("name is required");

			if (name.Length > m_MaxLightboxNameLength)
				throw new InvalidLightboxException(string.Format("name cannot exceed {0} characters", m_MaxLightboxNameLength));

			if (name.Contains("default"))
				throw new InvalidLightboxException("you cannot use the word 'default' in a lightbox name");
		}

		public static string StripSenderName(string lightboxName)
		{
			return Regex.Replace(lightboxName, @"\s\[[^\]]+]", string.Empty, RegexOptions.IgnoreCase);
		}

		#endregion

		#region Events

		/// <summary>
		/// Fired when something happens to the lightbox list
		/// eg. Lightbox is added, removed, renamed, made default
		/// </summary>
		public event EventHandler LightboxListChanged;
		public static event LightboxSentEventHandler LightboxSentToUser;

		private void OnLightboxListChanged()
		{
			m_Lightboxes = null;

			if (LightboxListChanged != null)
				LightboxListChanged(this, EventArgs.Empty);
		}

		#endregion

		#region Private variables

		private static readonly ILog m_Logger = LogManager.GetLogger(typeof(LightboxManager));
		private EntityList<Lightbox> m_Lightboxes = null;
		private const int m_MaxLightboxNameLength = 40;

		#endregion

		#region Constructor

		public LightboxManager(User user)
			: base(user)
		{
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Gets all user lightboxes
		/// </summary>
		public EntityList<Lightbox> UserLightboxes
		{
			get
			{
				if (m_Lightboxes == null)
				{
					LightboxFinder lightboxFinder = new LightboxFinder();

					if (User.UserRole == UserRole.SuperAdministrator)
					{
						lightboxFinder.UserIdOrPublic = User.UserId.GetValueOrDefault(-1);
					}
					else
					{
						lightboxFinder.UserId = User.UserId.GetValueOrDefault(-1);
					}

					lightboxFinder.SortExpressions.Add(new AscendingSort(Lightbox.Columns.Name));
					m_Lightboxes = Lightbox.FindMany(lightboxFinder);


                    //add any linked lightboxes
                    LightboxLinkedFinder lightboxLinkedFinder = new LightboxLinkedFinder {UserId = User.UserId.GetValueOrDefault(-1)};
                    EntityList<LightboxLinked> lightboxLinks = LightboxLinked.FindMany(lightboxLinkedFinder);

                    if(lightboxLinks.Count > 0){
                        //get only linkedLightboxes that are not disabled and not expired
                        var validLinkedLightboxes = from l in lightboxLinks where l.Disabled.GetValueOrDefault(false) == false && l.ExpiryDate.GetValueOrDefault(DateTime.MaxValue) > DateTime.Now select l.Lightbox;

                        if (validLinkedLightboxes.Count() > 0)
                        {
                            //add to the user's list
                            m_Lightboxes.AddRange(validLinkedLightboxes.ToArray());

                            //resort list by name
                            m_Lightboxes.Sort((a, b) => a.Name.CompareTo(b.Name));
                        }
                    }
				}              

				return m_Lightboxes;
			}
		}

		#endregion

		public void SaveLightbox(Lightbox lb)
		{
			ValidateLightboxName(lb.Name);

			if (lb.Summary.Length > 160)
				throw new InvalidLightboxException("summary cannot exceed 160 characters");

			if (lb.Notes.Length > 500)
				throw new InvalidLightboxException("notes cannot exceed 500 characters");

			if (lb.IsPublic && lb.Brands.Count == 0)
				throw new InvalidLightboxException("public lightboxes must be assigned to at least one brand");

			if (lb.CreateDate == DateTime.MinValue)
				throw new InvalidLightboxException("system error - invalid createdate");

			Lightbox.Update(lb);

			OnLightboxListChanged();
		}

		#region Lightbox Retrieval

		/// <summary>
		/// Gets the default lightbox for the user.
		/// If the user does not have a default lightbox, but have lightboxes, the first lightbox
		/// will be made the default.  If the user has no lightboxes, returns a null lightbox
		/// </summary>
		public Lightbox GetDefaultLightbox()
		{
			// Try and find the default lightbox and return it
			foreach (Lightbox lb in UserLightboxes)
				if (lb.IsDefault)
					return lb;

			// Otherwise, there's no default lightbox, so
			// make the first one default that's not linked and return it
			if (UserLightboxes.Count > 0)
			{
                Lightbox lb = UserLightboxes.FirstOrDefault(l => l.IsLinked == false);

                if (lb != null)
                {
                    lb.IsDefault = true;                
                    Lightbox.Update(lb);
                    return lb;
                }
                
			}

			return Lightbox.Empty;
		}


		/// <summary>
		/// Gets the lightbox with the specified ID
		/// </summary>
		public Lightbox GetLightboxById(int lightboxId)
		{
			foreach (Lightbox lb in UserLightboxes)
				if (lb.LightboxId == lightboxId)
					return lb;

			return Lightbox.Get(lightboxId);
		}

		#endregion

		#region Asset Stuff - Checks, Add, Remove

		public bool LightboxContainsAsset(Lightbox lightbox, int assetId)
		{
			return lightbox.GetLightboxAssetList().Any(lba => lba.AssetId == assetId);
		}


        /// <summary>
        /// creates a new lightbox asset and adds this to the lightbox with the specified ID, if it does not contain it already
        /// </summary>
        public void AddAssetToLightbox(int lightboxId, int assetId)
        {
            LightboxAsset lba = LightboxAsset.New();
            lba.LightboxId = lightboxId;
            lba.AssetId = assetId;

            AddAssetToLightbox(lightboxId, lba);
        }



		/// <summary>
		/// Adds a copy of the lightbox asset to the lightbox with the specified ID, if it does not contain the asset
		/// </summary>
		public void AddAssetToLightbox(int lightboxId, LightboxAsset lba)
		{
			Lightbox lb = GetLightboxById(lightboxId);
            AddAssetToLightbox(lb, lba);
		}

        /// <summary>
        /// Adds a copy of the lightbox asset to the lightbox, if it does not contain the asset
        /// </summary>
        public void AddAssetToLightbox(Lightbox lightbox, LightboxAsset lightboxAsset)
        {
            if (!lightbox.IsNull && !lightboxAsset.IsNull)
            {
                if (EntitySecurityManager.CanManageLightbox(User, lightbox))
                {
                    if (!LightboxContainsAsset(lightbox, lightboxAsset.AssetId))
                    {
                        LightboxAsset lba = LightboxAsset.New();
                        lba.LightboxId = lightbox.LightboxId.GetValueOrDefault();
                        lba.AssetId = lightboxAsset.AssetId;
                        lba.Notes = lightboxAsset.Notes;
                        lba.CreateDate = DateTime.Now;
                        LightboxAsset.Update(lba);

                        AuditLogManager.LogAssetAction(lightboxAsset.AssetId, User, AuditAssetAction.AddedToLightbox);
                        AuditLogManager.LogUserAction(User, AuditUserAction.AddToLightbox, string.Format("Added AssetId: {0} to LightboxId: {1}", lightboxAsset.AssetId, lightbox.LightboxId.GetValueOrDefault()));
                    }
                }
                else
                {
                    m_Logger.DebugFormat("User: {0} (UserId: {1}) tried to add AssetId: {2} to LightboxId: {3} but couldn't due to insufficient permissions to manage ths lightbox", User.FullName, User.UserId, lightboxAsset.AssetId, lightbox.LightboxId.GetValueOrDefault());
                }
            }
        }




		public void RemoveAssetFromLightbox(int lightboxId, int assetId)
		{
			RemoveAssetFromLightbox(lightboxId, assetId, string.Empty);
		}

		public void RemoveAssetFromLightbox(int lightboxId, int assetId, string additionalNotes)
		{
			Lightbox lb = GetLightboxById(lightboxId);

			if (EntitySecurityManager.CanManageLightbox(User, lb))
			{
				if (LightboxContainsAsset(lb, assetId))
				{
					foreach (LightboxAsset lba in lb.GetLightboxAssetList())
					{
						if (lba.AssetId == assetId)
						{
							LightboxAsset.Delete(lba.LightboxAssetId);

							AuditLogManager.LogAssetAction(assetId, User, AuditAssetAction.RemovedFromLightbox);

							string notes = string.Format("Removed AssetId: {0} from LightboxId: {1}", assetId, lightboxId);

							if (!StringUtils.IsBlank(additionalNotes))
								notes += string.Format(". {0}", additionalNotes);

							AuditLogManager.LogUserAction(User, AuditUserAction.RemoveFromLightbox, notes);
						}
					}
				}
			}
			else
			{
				m_Logger.DebugFormat("User: {0} (UserId: {1}) tried to remove AssetId: {2} from LightboxId: {3} but couldn't due to insufficient permissions to manage ths lightbox", User.FullName, User.UserId, assetId, lightboxId);
			}
		}


        /// <summary>
        /// Reorders the lightbox asset in the list with the new order number.
        /// </summary>
        public void ReorderLightboxAsset(int lightboxId, int assetId, int newOrderIndex)
        {
            LightboxAssetFinder finder = new LightboxAssetFinder { LightboxId = lightboxId };
            EntityList<LightboxAsset> lbAssets = LightboxAsset.FindMany(finder);

            //get sorted list of lightbox assets
            var sortedList = (from lba in lbAssets orderby lba.OrderNumber.GetValueOrDefault(9999),lba.LightboxAssetId select lba).ToList();

            //make sure newOrderIndex is within the list's bounds
            if (newOrderIndex >= sortedList.Count())
                newOrderIndex = (sortedList.Count() - 1);
            else if (newOrderIndex < 0)
                newOrderIndex = 0;

            //re-insert item into correct place within the sorted list
            int iCurrentIndex = sortedList.FindIndex(lba => lba.AssetId == assetId);

            if (iCurrentIndex >= 0) //check asset was found
            {
                LightboxAsset lightboxAsset = sortedList[iCurrentIndex];
                sortedList.RemoveAt(iCurrentIndex);
                sortedList.Insert(newOrderIndex, lightboxAsset);

                //update lightbox assets with their new positions
                for (int i = 0; i < sortedList.Count(); i++)
                {
                    LightboxAsset lba = sortedList[i];
                    lba.OrderNumber = i;
                    LightboxAsset.Update(lba);
                }
            }          
        }



		#endregion

		#region Lightbox Stuff

		public Lightbox CreateLightbox(string name, bool isDefault)
		{
			Lightbox lb = Lightbox.New();
			lb.UserId = User.UserId.GetValueOrDefault();
			lb.Name = name;
			lb.IsDefault = false;
			lb.CreateDate = DateTime.Now;

			SaveLightbox(lb);

			if (isDefault)
				SetDefaultLightbox(lb.LightboxId.GetValueOrDefault());

			AuditLogManager.LogUserAction(User, AuditUserAction.AddLightbox, string.Format("Created lightbox: {0} (LightboxId: {1})", lb.Name, lb.LightboxId));

			return lb;
		}

		public void RemoveLightbox(int lightboxId)
		{
			// Ensure that we have more than one lightbox
			if (UserLightboxes.Count == 1)
				throw new InvalidLightboxException("cannot remove only lightbox");

			// Get the lightbox
			Lightbox lb = GetLightboxById(lightboxId);

			// Default lightboxes cannot be removed
			if (lb.IsDefault)
				throw new InvalidLightboxException("cannot remove default lightbox");


            if (lb.IsLinked)
            {
                 //remove linked lightbox
                LightboxLinked lightboxLinked = LightboxLinked.GetLightboxLinked(User.UserId.GetValueOrDefault(-1),lightboxId);

                if (!lightboxLinked.IsNull)
                {
                    LightboxLinked.Delete(lightboxLinked.LightboxLinkedId);
                }

                // Update audit log
                AuditLogManager.LogUserAction(User, AuditUserAction.RemoveLightbox, string.Format("Removed linked lightbox: {0} (LightboxLinkedId: {1})", lb.Name, lightboxLinked.LightboxLinkedId));
            }
            else
            {
                // Non-superadmins can only remove their own lightboxes
                if (User.UserRole != UserRole.SuperAdministrator && lb.UserId != User.UserId.GetValueOrDefault())
                    throw new InvalidLightboxException("cannot remove lightbox not created by you");

                // Delete it
                Lightbox.Delete(lb.LightboxId);
                
                // Update audit log
                AuditLogManager.LogUserAction(User, AuditUserAction.RemoveLightbox, string.Format("Removed lightbox: {0} (LightboxId: {1})", lb.Name, lb.LightboxId));
            }

			// Fire event
			OnLightboxListChanged();
		}

		public void MergeLightbox(int sourceLightboxId, int targetLightboxId, bool removeSource)
		{
			if (sourceLightboxId == targetLightboxId)
				throw new InvalidLightboxException("source and target lightbox cannot be the same");

			Lightbox source = GetLightboxById(sourceLightboxId);
            Lightbox target = GetLightboxById(targetLightboxId);

            if (source.IsLinked)
                throw new InvalidLightboxException("cannot use linked lightboxes as the source");

            if(target.IsLinked)
                throw new InvalidLightboxException("cannot use linked lightboxes as the target");

			if (removeSource)
			{
				if (source.IsDefault)
					throw new InvalidLightboxException("source lightbox cannot removed as it is the default lightbox");

				if (!EntitySecurityManager.CanManageLightbox(User, source))
					throw new InvalidLightboxException("source lightbox cannot removed as you do not have permission");
			}

			foreach (LightboxAsset lba in source.GetLightboxAssetList())
                AddAssetToLightbox(targetLightboxId, lba);

			if (removeSource)
				RemoveLightbox(sourceLightboxId);
		}

		public Lightbox DuplicateLightbox(int lightboxId, string newName)
		{
			return DuplicateLightbox(lightboxId, newName, User.UserId.GetValueOrDefault());
		}

		public void RenameLightbox(int lightboxId, string newName)
		{
            Lightbox lb = GetLightboxById(lightboxId);

            if (!lb.IsEditable)
                throw new InvalidLightboxException("you are not allowed to edit this lightbox");

			ValidateLightboxName(newName);

			if (IsDuplicateName(newName, User.UserId.GetValueOrDefault(-1)))
				throw new InvalidLightboxException("A lightbox with that name already exists");
		
			lb.Name = newName;

			SaveLightbox(lb);
		}

		/// <summary>
		/// Sends a lightbox to a user
		/// </summary>
		/// <param name="lightboxId">The ID of the lightbox to send</param>
		/// <param name="subject">The subject of the email</param>
		/// <param name="message">The message to include in the email</param>
		/// <param name="recipient">The email address of the recipient</param>
		/// <param name="cc">The email address of the CC recipient (optional)</param>
		/// <param name="expiryDate">The date which the lightbox should expire (if sent to a non-registered user)</param>
		/// <param name="downloadLinks"></param>
		public void SendLightbox(int lightboxId, string subject, string message, string recipient, string cc, DateTime? expiryDate, bool? downloadLinks, bool? linked, bool? editable)
		{
			ErrorList errors = new ErrorList();
			Lightbox lightbox = GetLightboxById(lightboxId);

			User recipientUser = User.Empty;


            if (lightbox.IsLinked && !lightbox.IsEditable)
                errors.Add("you are not allowed to send this linked lightbox");

			if (StringUtils.IsBlank(subject))
				errors.Add("no subject entered");

			if (subject.Length > 150)
				errors.Add("subject length cannot exceed 150 characters");

			if (!StringUtils.IsEmail(recipient))
			{
				errors.Add("no recipient email address entered");
			}
			else if (recipient.Length > 150)
			{
				errors.Add("recipient email address cannot exceed 150 characters");
			}
			else
			{
				recipientUser = User.GetByEmail(recipient);

				if (!recipientUser.IsNull)
				{
                    string errorMsg;
                    if (!ValidateLightboxSendForUser(recipientUser, lightbox, linked, out errorMsg))
                    {
                        errors.Add(errorMsg);
                    }  
				}
			}

            //get collection of cc emails and associated user objects
            Dictionary<string, User> ccUsers = new Dictionary<string,User>();

            if (!StringUtils.IsBlank(cc))
            {
                //get valid email addresses from cc string (all lower case)
                string[] ccEmails;

                //SplitEmails returns false if there are any invalid
                //addresses contained in the cc string - currently no action
                //is taken 
                StringUtils.SplitEmails(cc, out ccEmails);

                foreach (string address in ccEmails)
                {
                    //check that address has not been added already
                    //and that cc user is not the same as the main recipient
                    if (!ccUsers.ContainsKey(address))
                    {                           
                        //validate address
                        if (address == recipient.ToLower())
                        {
                            errors.Add("recipient and cc email cannot be the same");
                            break;
                        }
                        else if (cc.Length > 150)
				        {
					        errors.Add("cc email address cannot exceed 150 characters");
                            break;
                        }else{
                            //find if a registered user associated with the address
                            User user = User.GetByEmail(address);

                            if (!user.IsNull)
                            {
                                //registered user - make sure lightbox is valid to send to them
                                string errorMsg;
                                if (ValidateLightboxSendForUser(user, lightbox, linked, out errorMsg))
                                {
                                    ccUsers.Add(address, user);
                                }
                                else
                                {
                                    errors.Add(errorMsg);
                                }                         
                            }
                            else
                            {
                                //unregistered user
                                ccUsers.Add(address, User.Empty);
                            }
                        }
                    }
                }                   
            }


            if (ccUsers.Count > MaxNumberCCEmails)           
                errors.Add("exceeded maximum number of CC email addresses");

			if (message.Length > 400)
				errors.Add("message cannot be more than 400 characters");

			if (expiryDate.HasValue && expiryDate.Value <= DateTime.Now.Date)
				errors.Add("expiry date must be after today");

			if (lightbox.GetAssetList().Count == 0)
				errors.Add("cannot send empty lightbox");

			if (lightbox.IsPublic && lightbox.UserId != User.UserId.GetValueOrDefault())
				errors.Add("cannot send public lightbox");

			// Drop out with error if any validation errors occured
			if (errors.Count > 0)
				throw new InvalidLightboxException(errors);

			// First send the lightbox to the recipient
			SendLightboxToUser(lightbox, recipientUser, recipient, subject, message, expiryDate, downloadLinks, linked, editable);

			// Then send it to the CC user, if one is specified
            foreach(KeyValuePair<string,User> user in ccUsers)
            {
                SendLightboxToUser(lightbox, user.Value, user.Key, subject, message, expiryDate, downloadLinks, linked, editable);
            }
				
		}

		public void SetDefaultLightbox(int lightboxId)
		{
			foreach (Lightbox lightbox in UserLightboxes)
			{
                if (!lightbox.IsLinked) //don't update linked lightboxes
                {
                    lightbox.IsDefault = (lightbox.LightboxId.GetValueOrDefault() == lightboxId);
                    Lightbox.Update(lightbox);
                }              
			}

			OnLightboxListChanged();
		}

		#endregion

		#region Private Methods

		private void SendLightboxToUser(Lightbox lightbox, User user, string recipient, string subject, string message, DateTime? expiryDate, bool? downloadLinks, bool? linked, bool? editable)
		{
			m_Logger.Debug("SendLightboxToUser - start");

			int? createdLightboxId = null;
            int? linkedLightboxId = null;

			if (!user.IsNull)
			{
                //check to see if should link the lightbox or
                //make a new copy of it
                if (linked.GetValueOrDefault(false))
                {
                    // create new linked lightbox 
                    LightboxLinked linkedLightbox = LightboxLinked.New();
                    linkedLightbox.UserId = user.UserId.GetValueOrDefault();
                    linkedLightbox.LightboxId = lightbox.LightboxId.GetValueOrDefault();
                    linkedLightbox.IsEditable = editable;
                    linkedLightbox.ExpiryDate = expiryDate;
                    LightboxLinked.Update(linkedLightbox);

                    linkedLightboxId = linkedLightbox.LightboxLinkedId;
                }
                else
                {
                    // copying lightbox
                    string lightboxName = lightbox.Name;
                    string newLightboxName = GetLightboxNameForSending(lightboxName);

                    Lightbox createdLightbox = DuplicateLightbox(lightbox.LightboxId.GetValueOrDefault(), newLightboxName, user.UserId.GetValueOrDefault());
                    createdLightboxId = createdLightbox.LightboxId.GetValueOrDefault();
                }
			}

			LightboxSent lbs = LightboxSent.New();
			lbs.LightboxId = lightbox.LightboxId.GetValueOrDefault();
			lbs.CreatedLightboxId = createdLightboxId;
			lbs.SenderId = User.UserId.GetValueOrDefault();
			lbs.RecipientEmail = recipient;
			lbs.Subject = subject;
			lbs.Message = message;
			lbs.DateSent = DateTime.Now;
			lbs.ExpiryDate = expiryDate;
            lbs.DownloadLinks = downloadLinks;
            lbs.LightboxLinkedId = linkedLightboxId;
           
			
			if (lbs.RecipientEmail.Length > 150)
				throw new SystemException("Recipient email cannot exceed 150 characters");

			if (lbs.Subject.Length > 150)
				throw new SystemException("Subject cannot exceed 150 characters");

			if (lbs.Message.Length > 500)
				throw new SystemException("Message cannot exceed 500 characters");
			LightboxSent.Update(lbs);

			if (LightboxSentToUser != null)
				LightboxSentToUser(this, new LightboxSentEventArgs(lbs));

			AuditLogManager.LogUserAction(User, AuditUserAction.SendLightbox, string.Format("Sent LightboxId: {0} to: {1}", lightbox.LightboxId, recipient));

			m_Logger.Debug("SendLightboxToUser - end");
		}


        /// <summary>
        /// Performs validation to ensure lightbox is appropriate to be sent to user.
        /// </summary>
        private bool ValidateLightboxSendForUser(User user, Lightbox lightbox, bool? linked, out string errorMsg)
        {
            errorMsg = string.Empty;

            if (linked.GetValueOrDefault(false))
            {
                //linking to the lightbox so check not already linked
                if(IsLightboxLinked(lightbox.LightboxId.GetValueOrDefault(),user.UserId.GetValueOrDefault()))
                {
                    errorMsg = "recipient " + user.Email + " is already linked to this lightbox";
                    return false;
                }
            }
            else
            {
                // copying the lightbox so check for duplicate name

                string newLightboxName = GetLightboxNameForSending(lightbox.Name);

                if (IsDuplicateName(newLightboxName, user.UserId.GetValueOrDefault()))
                {
                    errorMsg = "recipient " + user.Email + " already has a lightbox with the same name";
                    return false;
                }
            }

            return true;
        }



		private string GetLightboxNameForSending(string lightboxName)
		{
			// Remove any existing user name
			lightboxName = StripSenderName(lightboxName);

			// New suffix
			string newSuffix = " [" + User.FullName + "]";

			// Create the new lightbox name from the name + sender
			string newLightboxName = string.Concat(lightboxName, newSuffix);

			// Ensure new name doesn't exceed limit
			if (newLightboxName.Length > m_MaxLightboxNameLength)
			{
				int newLength = m_MaxLightboxNameLength - newSuffix.Length;
				lightboxName = lightboxName.Substring(0, newLength);
				newLightboxName = string.Concat(lightboxName, newSuffix);
			}

			return newLightboxName;
		}

		private static bool IsDuplicateName(string newName, int userId)
		{
			LightboxFinder finder = new LightboxFinder {UserId = userId, Name = newName};
			int count = Lightbox.GetCount(finder);
			return (count > 0);
		}

        private static bool IsLightboxLinked(int lightboxId, int userId)
        {
            LightboxLinked lightboxLinked = LightboxLinked.GetLightboxLinked(userId, lightboxId);
            return (!lightboxLinked.IsNull && lightboxLinked.ExpiryDate.GetValueOrDefault(DateTime.MaxValue) > DateTime.Now);
        }


		private Lightbox DuplicateLightbox(int lightboxId, string newName, int targetUserId)
		{
			ValidateLightboxName(newName);

			if (IsDuplicateName(newName, targetUserId))
			{
				string message = (targetUserId == User.UserId.GetValueOrDefault()) ? "A lightbox with that name already exists" : "User already has a lightbox with that name";
				throw new InvalidLightboxException(message);
			}

			Lightbox lightbox = GetLightboxById(lightboxId);

			Lightbox newLightbox = Lightbox.New();
			newLightbox.UserId = targetUserId;
			newLightbox.Name = newName;
			newLightbox.Summary = lightbox.Summary;
			newLightbox.Notes = lightbox.Notes;
			newLightbox.IsDefault = false;
			newLightbox.CreateDate = DateTime.Now;

			SaveLightbox(newLightbox);

			foreach (LightboxAsset lba in lightbox.GetLightboxAssetList())
			{
				LightboxAsset newlba = LightboxAsset.New();

				newlba.LightboxId = newLightbox.LightboxId.GetValueOrDefault();
				newlba.AssetId = lba.AssetId;
				newlba.Notes = lba.Notes;
				newlba.CreateDate = DateTime.Now;

				LightboxAsset.Update(newlba);
			}

			return newLightbox;
		}

		#endregion

		public static void EnsureUserHasDefaultLightbox(User user)
		{
			if (user.IsNew || user.IsNull)
				return;

			LightboxFinder finder = new LightboxFinder {UserId = user.UserId.GetValueOrDefault(), IsDefault = true};
			int count = Lightbox.GetCount(finder);

			if (count == 0)
			{
				Lightbox lb = Lightbox.New();
				lb.UserId = user.UserId.GetValueOrDefault();
				lb.Name = "My Assets";
				lb.IsDefault = true;
				lb.CreateDate = DateTime.Now;
				Lightbox.Update(lb);

				AuditLogManager.LogUserAction(user, AuditUserAction.AddLightbox, "System created default lightbox as there were no other ligthboxes");
			}
		}

		public bool ContainsLightbox(int lightboxId)
		{
			return UserLightboxes.Any(lb => lb.LightboxId.Equals(lightboxId));
		}
	}
}