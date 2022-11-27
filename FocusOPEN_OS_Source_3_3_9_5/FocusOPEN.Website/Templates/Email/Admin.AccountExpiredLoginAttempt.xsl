<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="user-name">[user-name]</xsl:param>
	<xsl:param name="user-email">[user-email]</xsl:param>
	<xsl:param name="url">[url]</xsl:param>

	<xsl:template name="body-area">
		<p>Hi,</p>
		<p>
			A user recently tried to login, but could not due to their account being expired.
			<br />
			<br />
			The user details are below:
			<br />
			<br />
			Name:
			<xsl:value-of select="$user-name" />
			<br />
			Email:
			<xsl:value-of select="$user-email" />
			<br />
		</p>
		<p>
			To edit this user's details, click on the link below:
			<br />
			<a href="{$url}"><xsl:value-of select="$url" /></a>
		</p>
	</xsl:template>

</xsl:stylesheet>