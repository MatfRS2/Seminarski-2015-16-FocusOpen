<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="url">[url]</xsl:param>
	<xsl:param name="first-name">[first-name]</xsl:param>
	<xsl:param name="asset-id">[asset-id]</xsl:param>
	<xsl:param name="asset-title">[asset-title]</xsl:param>
	<xsl:param name="feedback">[feedback]</xsl:param>

	<xsl:template name="body-area">
		<p>
			Dear
			<xsl:value-of select="$first-name" />
			,
		</p>
		<p>
			Thank you for your feedback for the asset with title '<em><xsl:value-of select="$asset-title" /></em>' (ref: <xsl:value-of select="$asset-id" />).
			<br />
			<br />
			Your message is included below for your records.
			<br />
			<br />
			<em><xsl:value-of select="$feedback"/></em>
		</p>
	</xsl:template>

</xsl:stylesheet>