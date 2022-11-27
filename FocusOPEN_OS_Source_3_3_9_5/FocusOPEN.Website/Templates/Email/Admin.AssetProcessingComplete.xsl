<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="first-name">[first-name]</xsl:param>
	<xsl:param name="asset-filename">[asset-filename]</xsl:param>
	<xsl:param name="asset-id">[asset-id]</xsl:param>
	<xsl:param name="url">[url]</xsl:param>

	<xsl:template name="body-area">
		<p>
			Dear
			<xsl:value-of select="$first-name" />
			,
		</p>
		<p>
			This is an automated email to inform you that the asset with filename '
			<xsl:value-of select="$asset-filename" />
			' (reference:
			<xsl:value-of select="$asset-id" />
			)
			has been processed successfully.
		</p>
		<p>
			Click on the link below to view or edit asset details.
			<br />
			<a href="{$url}"><xsl:value-of select="$url" /></a>
		</p>
	</xsl:template>

</xsl:stylesheet>