<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="inviter-name">[inviter-name]</xsl:param>
	<xsl:param name="inviter-message">[inviter-message]</xsl:param>
	<xsl:param name="asset-id">[asset-id]</xsl:param>
	<xsl:param name="asset-type">[asset-type]</xsl:param>
	<xsl:param name="upload-user-name">[upload-user-name]</xsl:param>
	<xsl:param name="url">[url]</xsl:param>

	<xsl:template name="body-area">
		<p>Hi,</p>
		<p>
			You have been invited by
			<xsl:value-of select="$inviter-name" />
			to comment on an asset waiting to be approved.
			<br />
			<br />

			<strong>
				Message from
				<xsl:value-of select="$inviter-name" />
				:
			</strong>
			<br />
			<br />
			<xsl:value-of select="$inviter-message" />

			<br />
			<br />

			<strong>Asset details:</strong>
			<br />
			<br />
			Reference:
			<xsl:value-of select="$asset-id" />
			<br />
			Asset Type:
			<xsl:value-of select="$asset-type" />
			<br />
			Uploaded by:
			<xsl:value-of select="$upload-user-name" />

			<br />
			<br />
			Please click on the link below to review asset details and add your comments.
			<br />
			<a href="{$url}"><xsl:value-of select="$url" /></a>
		</p>
	</xsl:template>

</xsl:stylesheet>