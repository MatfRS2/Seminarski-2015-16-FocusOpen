<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="uploader-name" />

	<xsl:template name="body-area">
		<p>Hi,</p>
		<p>
			This email is to inform you that the user '
			<xsl:value-of select="$uploader-name" />
			' recently tried to upload some assets which already exist in the system,
			but they do not have access to the original assets.  The asset details are listed below.
		</p>
		<ul>
			<xsl:for-each select="//Asset">
				<li>
					<a><xsl:attribute name="href"><xsl:value-of select="$appPath" />Go.ashx/RAD/AID<xsl:value-of select="@AssetId" />/</xsl:attribute><xsl:value-of select="@AssetId" />:<xsl:value-of select="@Filename" /></a>
				</li>
			</xsl:for-each>
		</ul>
		<p>
			Click on a filename above to view the asset details and make changes.
		</p>
	</xsl:template>

</xsl:stylesheet>