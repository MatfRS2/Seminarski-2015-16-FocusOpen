/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

namespace FocusOPEN.Data
{

	public partial class LightboxLinked
	{

        #region Private variables

        private Lightbox m_Lightbox = null;
        private User m_User = null;

        #endregion

        #region Lazy Loads

        public Lightbox Lightbox
        {
            get
            {
                if (m_Lightbox == null)
                {
                    m_Lightbox = Lightbox.Get(LightboxId);
                    m_Lightbox.IsLinked = true;
                    m_Lightbox.IsDefault = false; //linked lightboxes not currently able to be defaults
                    m_Lightbox.IsEditable = IsEditable.GetValueOrDefault(false);
                    m_isDirty = false;                 
                }

                return m_Lightbox;
            }
        }

        public User User
        {
            get
            {
                if (m_User == null)
                    m_User = User.Get(UserId);

                return (m_User);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets a linked lightbox by lightboxid and userid
        /// </summary>
        public static LightboxLinked GetLightboxLinked(int userId, int lightboxId)
        {
            LightboxLinkedFinder finder = new LightboxLinkedFinder { UserId = userId, LightboxId = lightboxId };
            return LightboxLinked.FindOne(finder);
        }


        #endregion

    }
}
