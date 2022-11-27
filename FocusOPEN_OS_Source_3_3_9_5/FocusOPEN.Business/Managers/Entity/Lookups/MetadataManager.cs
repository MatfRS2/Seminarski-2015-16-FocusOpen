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
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class MetadataManager
	{
		#region Public Methods

		public static void EnsureRootExists(int brandId, int groupNumber, string name, User user)
		{
			if (MetadataCache.Instance.GetList(brandId, groupNumber).Count() > 0)
				return;

			Add(name, null, brandId, groupNumber, string.Empty, string.Empty, user);
		}

		public static Metadata Add(string text, int? parentId, int brandId, int groupNumber, string externalRef, string synonyms, User user)
		{
			Metadata o = Metadata.New();
			o.Name = text;
			o.ParentMetadataId = parentId;
			o.BrandId = brandId;
			o.GroupNumber = groupNumber;
			o.ExternalRef = externalRef;
			o.Synonyms = synonyms;

			if (o.ParentMetadataId.GetValueOrDefault() <= 0)
				o.ParentMetadataId = null;

		    var sibglings = MetadataCache.Instance
		        .GetList(brandId, groupNumber)
		        .Where(m => (
                                //either we are adding to the top parent level
		                        parentId == 0 && !m.ParentMetadataId.HasValue) ||
                                //or we need all siblings with the same parent
                                m.ParentMetadataId == parentId
		        )
		        .ToList();

		    o.MetadataOrder = sibglings.Count == 0 ? 1 : sibglings.OrderBy(s => s.MetadataOrder).Last().MetadataOrder + 1;

			Validate(o);
			
			o = Metadata.Update(o);
			CacheManager.InvalidateCache("Metadata", CacheType.All);

			if (!user.IsNull)
				AuditLogManager.LogUserAction(user, AuditUserAction.AddMetadata, string.Format("Added Metadata with ID: {0}.  Name: {1}", o.MetadataId, o.Name));
			
			return o;
		}

		public static void Update(int id, string text, string externalRef, string synonyms, User user)
		{
			Metadata o = Metadata.Get(id);
			o.Name = text;
			o.ExternalRef = externalRef;
			o.Synonyms = synonyms;
			Validate(o);
			Metadata.Update(o);
			CacheManager.InvalidateCache("Metadata", CacheType.All);
			AuditLogManager.LogUserAction(user, AuditUserAction.ModifyMetadata, "Updated metadata with ID: " + id);
		}

		public static void Delete(int id)
		{
			// Get the metadata item to be deleted
			Metadata o = Metadata.Get(id);

			// Check if it's a root item, in which case we need to ensure that it's not the only root item remaining
			// as we need to disallow the deletion then (there must be one root item at all times)
			if (!o.ParentMetadataId.HasValue)
			{
				MetadataFinder finder = new MetadataFinder {BrandId = o.BrandId, GroupNumber = o.GroupNumber, ParentMetadataId = Int32.MinValue};
				int count = Metadata.GetCount(finder);

				if (count == 1)
					throw new InvalidMetadataException("This is the only root metadata item and cannot be deleted");
			}

			o.IsDeleted = true;
			Metadata.Update(o);
			CacheManager.InvalidateCache("Metadata", CacheType.All);
		}

		public static void Move(int metadataId, int parentId, User user, int order)
		{
			var metadata = Metadata.Get(metadataId);
            IList<Metadata> subMetas;

            if(parentId > 0)
            {
                var parentMetadata = Metadata.Get(parentId);
                subMetas = parentMetadata.Children;
            }else
            {//we are at root level - i.e. all metas are with parent=NULL
                //so get all siblings by brandId and group number
                subMetas = (from m in MetadataCache.Instance.GetList(metadata.BrandId.GetValueOrDefault(), metadata.GroupNumber)
                             where !m.ParentMetadataId.HasValue
                             select m).ToList();
            }

            // Reorder top level categories
            foreach (var meta in subMetas)
            {
                if ((meta.MetadataOrder >= order) && (meta.MetadataId != metadataId))
                {
                    meta.MetadataOrder += 1;
                    Metadata.Update(meta);
                }
            }
            
            metadata.ParentMetadataId = parentId;
		    metadata.MetadataOrder = order;

			if (metadata.ParentMetadataId.GetValueOrDefault() <= 0)
				metadata.ParentMetadataId = null;

			Metadata.Update(metadata);
			CacheManager.InvalidateCache("Metadata", CacheType.All);
			AuditLogManager.LogUserAction(user, AuditUserAction.ModifyMetadata, "Moved metadata with ID: " + metadataId);
		}

		#endregion

		#region Private Methods

		private static void Validate(Metadata entity)
		{
			if (entity.BrandId <= 0)
				throw new InvalidMetadataException("Brand is required");

			if (entity.GroupNumber <= 0)
				throw new InvalidMetadataException("Group Number is required");
			
			if (StringUtils.IsBlank(entity.Name))
				throw new InvalidMetadataException("Metadata name cannot be blank");

			MetadataFinder finder = new MetadataFinder {Name = entity.Name, BrandId = entity.BrandId, GroupNumber = entity.GroupNumber};
			Metadata o = Metadata.FindOne(finder);

			if (!o.IsNull)
			{
				if (entity.IsNew)
					throw new InvalidMetadataException("That item already exists");

				if (!entity.MetadataId.Equals(o.MetadataId))
					throw new InvalidMetadataException("That item already exists");
			}
		}

		#endregion
	}
}