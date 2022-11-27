<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="url">[url]</xsl:param>
	<xsl:param name="sender-name">[sender-name]</xsl:param>
	<xsl:param name="asset-count">[asset count]</xsl:param>
	<xsl:param name="lightbox-name">[lightbox-name]</xsl:param>
	<xsl:param name="message">[message]</xsl:param>

	<xsl:template name="body-area">
		<p>Hi,</p>
		<p>
			You have received a lightbox named '
			<xsl:value-of select="$lightbox-name" />
			' from
			<xsl:value-of select="$sender-name" />
			, containing
			<xsl:value-of select="$asset-count" />
			assets.
		</p>
		<p>
			<xsl:if test="$message != ''">
				The following message was included:
				<br />
				<br />
				&quot;
				<xsl:value-of select="$message" />
				&quot;
			</xsl:if>
		</p>
		<p>
			To view this lightbox, please go to:
			<br />
			<a href="{$url}"><xsl:value-of select="$url" /></a>
		</p>
	</xsl:template>

</xsl:stylesheet>