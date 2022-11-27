<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="message">[message]</xsl:param>
	<xsl:param name="registrant-name">[registration-name]</xsl:param>
	<xsl:param name="registrant-email">[registration-email]</xsl:param>
	<xsl:param name="registrant-ip-address">[registration-ip-address]</xsl:param>

	<xsl:template name="body-area">
		<p>Hi,</p>
		<p>
			A user recently tried to register, but could not due to the following reason:
			<br />
			<br />
			<em>
				<xsl:value-of select="$message" />
			</em>
			<br />
			<br />
			The user details are below:
			<br />
			<br />
			Name:
			<xsl:value-of select="$registrant-name" />
			<br />
			Email:
			<xsl:value-of select="$registrant-email" />
			<br />
			Ip Address:
			<xsl:value-of select="$registrant-ip-address" />
		</p>
		<p>
			If this user should be allowed to register, please add their email domain and/or ip address to the pre-approved list.
		</p>
	</xsl:template>

</xsl:stylesheet>