<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="asset-id">[asset-id]</xsl:param>
	<xsl:param name="asset-type">[asset-type]</xsl:param>
	<xsl:param name="upload-user-name">[upload-user-name]</xsl:param>
	<xsl:param name="rejector-name">[rejector-name]</xsl:param>
	<xsl:param name="rejector-comments">[rejector-comments]</xsl:param>
	<xsl:param name="url">[url]</xsl:param>

	<xsl:template name="body-area">
		<p>Hi,</p>
		<p>
			This is an automated email to inform you that an asset has been not been approved.
			<br />
			<br />
			<table border="0" cellpadding="3" cellspacing="0">
				<tr>
					<td>Ref:</td>
					<td>
						<xsl:value-of select="$asset-id" />
					</td>
				</tr>
				<tr>
					<td>Type:</td>
					<td>
						<xsl:value-of select="$asset-type" />
					</td>
				</tr>
				<tr>
					<td>Uploaded by:</td>
					<td>
						<xsl:value-of select="$upload-user-name" />
					</td>
				</tr>
			</table>
			<br />
			<br />
			<strong>Rejection details:</strong>
			<table border="0" cellpadding="3" cellspacing="0">
				<tr>
					<td>Rejected by:</td>
					<td>
						<xsl:value-of select="$rejector-name" />
					</td>
				</tr>
				<tr>
					<td>Reason:</td>
					<td>
						<xsl:value-of select="$rejector-comments" />
					</td>
				</tr>
			</table>
			<br />
			<br />
			Please click on the link below to review asset details.
			<br />
			<a href="{$url}"><xsl:value-of select="$url" /></a>
		</p>
	</xsl:template>

</xsl:stylesheet>