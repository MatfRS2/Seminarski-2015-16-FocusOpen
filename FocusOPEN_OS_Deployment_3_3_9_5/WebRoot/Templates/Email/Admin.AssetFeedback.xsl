<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="url">[url]</xsl:param>
	<xsl:param name="uploader-first-name">[uploader-first-name]</xsl:param>
	<xsl:param name="feedback-sender-name">[feedback-sender-name]</xsl:param>
	<xsl:param name="asset-id">[asset-id]</xsl:param>
	<xsl:param name="asset-title">[asset-title]</xsl:param>
	<xsl:param name="feedback">[feedback]</xsl:param>

	<xsl:template name="body-area">
		<p>
			Dear
			<xsl:value-of select="$uploader-first-name" />
			,
		</p>
		<p>
			The following feedback was left by <em><xsl:value-of select="$feedback-sender-name" /></em> for the asset with title
			'<em><xsl:value-of select="$asset-title" /></em>' (ref: <xsl:value-of select="$asset-id" />).
			<br />
			<br />
			<em><xsl:value-of select="$feedback"/></em>
			<br />
			<br />
			Click on the link below to review the asset details and make changes:<br/>
			<a href="{$url}"><xsl:value-of select="$url" /></a>
		</p>
	</xsl:template>

</xsl:stylesheet>