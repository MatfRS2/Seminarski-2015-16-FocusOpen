<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="url">[url]</xsl:param>

	<xsl:template name="body-area">
		<p>Hi,</p>
		<p>
			This is an automated message to inform you that a user has requested an asset which
			requires your approval.  Please click on the link below to view further details,
			and approve or deny this request.
			<br />
			<br />
			<a href="{$url}"><xsl:value-of select="$url" /></a>
		</p>
	</xsl:template>

</xsl:stylesheet>