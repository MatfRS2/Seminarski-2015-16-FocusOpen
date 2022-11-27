<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="url">[url]</xsl:param>
	<xsl:param name="order-id">[order-id]</xsl:param>
	<xsl:param name="asset-id">[asset-id]</xsl:param>
	<xsl:param name="comment-text">[comment-text]</xsl:param>
	<xsl:param name="comment-date">[comment-date]</xsl:param>
	<xsl:param name="comment-user-name">[comment-user-name]</xsl:param>

	<xsl:template name="body-area">
		<p>Hi,</p>
		<p>
			This is an automated message to inform you that a user has left further information
			about the asset with reference '
			<xsl:value-of select="$asset-id" />
			' in the order
			with reference '
			<xsl:value-of select="$order-id" />
			', which requires you approval
			before it can be downloaded.
			<br />
			<br />
			The comment was left by
			<xsl:value-of select="$comment-user-name" />
			on
			<xsl:value-of select="$comment-date" />
			.
			The comment text is below.
			<br />
			<br />

			<em>
				<pre style="font-size:1.3em;margin-left:15px;">
					<xsl:value-of select="$comment-text" />
				</pre>
			</em>

			<br />
			<br />
			Please click on the link below to go to the order detail where you can respond to this
			and approve or reject this request if required.
			<br />
			<br />

			<a href="{$url}"><xsl:value-of select="$url" /></a>
		</p>
	</xsl:template>

</xsl:stylesheet>