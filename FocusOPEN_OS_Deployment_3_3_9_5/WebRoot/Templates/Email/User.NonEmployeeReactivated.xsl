<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="user-id">[user-id]</xsl:param>
	<xsl:param name="first-name">[first-name]</xsl:param>
	<xsl:param name="last-name">[last-name]</xsl:param>

	<xsl:template name="body-area">
		<p>
			Dear
			<xsl:value-of select="$first-name" />
			,
		</p>
		<p>
			Your account has been reactivated.  Please login using the link below.
			<br />
			<br />
			<xsl:variable name="url"><xsl:value-of select="$appPath" />Login.aspx</xsl:variable>
			<a href="{$url}"><xsl:value-of select="$url" /></a>
		</p>
	</xsl:template>

</xsl:stylesheet>