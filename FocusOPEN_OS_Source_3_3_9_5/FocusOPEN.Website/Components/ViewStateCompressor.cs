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
using System.IO;
using System.IO.Compression;
using System.Web.UI;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// ViewState compressor class, to shrink viewstate a bit.  This is a little
	/// server heavy, so only use on pages where big viewstate is a problem
	/// http://www.codeproject.com/aspnet/ViewStateCompression.asp
	/// </summary>
	public class ViewStateCompressor : PageStatePersister
	{
		private const string m_ViewStateHiddenFieldName = "__COMPRESSEDVIEWSTATE";
		private LosFormatter m_StateFormatter;

		public ViewStateCompressor(Page page) : base(page)
		{
		}

		private new LosFormatter StateFormatter
		{
			get
			{
				if (m_StateFormatter == null)
					m_StateFormatter = new LosFormatter();

				return m_StateFormatter;
			}
		}

		public override void Load()
		{
			string viewState = Page.Request.Form[m_ViewStateHiddenFieldName];
			byte[] bytes = Convert.FromBase64String(viewState);

			using (MemoryStream input = new MemoryStream())
			{
				input.Write(bytes, 0, bytes.Length);
				input.Position = 0;
				using (GZipStream gzip = new GZipStream(input, CompressionMode.Decompress, true))
				{
					using (MemoryStream output = new MemoryStream())
					{
						byte[] buff = new byte[65];
						int read = gzip.Read(buff, 0, buff.Length);
						while (read > 0)
						{
							output.Write(buff, 0, read);
							read = gzip.Read(buff, 0, buff.Length);
						}

						gzip.Close();
						gzip.Dispose();

						Pair pair = (Pair) StateFormatter.Deserialize(Convert.ToBase64String(output.ToArray()));
						ViewState = pair.First;
						ControlState = pair.Second;
					}
				}
			}
		}

		public override void Save()
		{
			if (ViewState != null || ControlState != null)
			{
				StringWriter writer = new StringWriter();
				StateFormatter.Serialize(writer, new Pair(ViewState, ControlState));
				byte[] bytes = Convert.FromBase64String(writer.ToString());

				using (MemoryStream output = new MemoryStream())
				{
					using (GZipStream gzip = new GZipStream(output, CompressionMode.Compress, true))
					{
						gzip.Write(bytes, 0, bytes.Length);
						gzip.Close();
						gzip.Dispose();
					}

					string viewState = Convert.ToBase64String(output.ToArray());
					ScriptManager.RegisterHiddenField(Page, m_ViewStateHiddenFieldName, viewState);
				}
			}
		}
	}
}