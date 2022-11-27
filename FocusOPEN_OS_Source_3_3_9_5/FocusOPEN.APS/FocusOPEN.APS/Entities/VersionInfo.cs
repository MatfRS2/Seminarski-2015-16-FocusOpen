using System;
using System.Runtime.Serialization;

namespace FocusOPEN.APS
{
	[DataContract]
	public class VersionInfo
	{
		[DataMember]
		public int Major { get; set; }

		[DataMember]
		public int Minor { get; set; }

		[DataMember]
		public int Revision { get; set; }

		[DataMember]
		public int Build { get; set; }

		[DataMember]
		public DateTime CreateDate { get; set; }

		[DataMember]
		public DateTime LastModifiedDate { get; set; }
	}
}