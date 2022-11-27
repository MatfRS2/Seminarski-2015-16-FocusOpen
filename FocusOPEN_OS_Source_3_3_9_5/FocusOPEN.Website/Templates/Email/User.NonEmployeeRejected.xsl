<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="user-id">[user-id]</xsl:param>
	<xsl:param name="first-name">[first-name]</xsl:param>
	<xsl:param name="last-name">[last-name]</xsl:param>
	<xsl:param name="notes">[notes]</xsl:param>

	<xsl:template name="body-area">
		<p>
			Dear
			<xsl:value-of select="$first-name" />
			,
		</p>
		<p>
			You recently registered on the
			<xsl:value-of select="$appName" />
			.  Unfortunately, your request for access to the
			<xsl:value-of select="$appName" />
			has been denied.  The reason is given below:
			<br />
			<br />
			<em>
				<xsl:value-of select="$notes" />
			</em>
		</p>
	</xsl:template>

</xsl:stylesheet>