<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="url">[url]</xsl:param>
	<xsl:param name="first-name">[first-name]</xsl:param>
	<xsl:param name="order-id">[order-id]</xsl:param>

	<xsl:template name="body-area">
		<p>
			Dear
			<xsl:value-of select="$first-name" />
			,
		</p>
		<p>
			Your order with reference '
			<xsl:value-of select="$order-id" />
			' has been completed.  Please click
			on the link below to go to the download manager where you can download approved assets.
			<br />
			<br />
			<a href="{$url}"><xsl:value-of select="$url" /></a>
		</p>
	</xsl:template>

</xsl:stylesheet>