<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="message">[message]</xsl:param>
	<xsl:param name="user-name">[user-name]</xsl:param>
	<xsl:param name="user-email">[user-email]</xsl:param>
	<xsl:param name="user-ip-address">[user-ip-address]</xsl:param>
	<xsl:param name="url">[url]</xsl:param>

	<xsl:template name="body-area">
		<p>Hi,</p>
		<p>
			A user recently tried to login, but could not due to the following reason:
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
			<xsl:value-of select="$user-name" />
			<br />
			Email:
			<xsl:value-of select="$user-email" />
			<br />
			Ip Address:
			<xsl:value-of select="$user-ip-address" />
		</p>
		<p>
			You can <a href="{$url}">edit this user's details</a> via the User Management section in the admin area.
		</p>
	</xsl:template>

</xsl:stylesheet>