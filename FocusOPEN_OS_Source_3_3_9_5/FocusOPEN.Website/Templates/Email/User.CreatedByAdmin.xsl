<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="first-name">[first-name]</xsl:param>
	<xsl:param name="email">[email]</xsl:param>
	<xsl:param name="password">[password]</xsl:param>
	<xsl:param name="url">[url]</xsl:param>

	<xsl:template name="body-area">
		<p>
			Dear
			<xsl:value-of select="$first-name" />
			,
		</p>
		<p>
			Welcome to the
			<xsl:value-of select="$appName" />
			.  Your login details are below.
		</p>
		<p>
			<table border="0" cellpadding="4" cellspacing="0">
				<tr>
					<td>
						<strong>Username:</strong>
					</td>
					<td>
						<xsl:value-of select="$email" />
					</td>
				</tr>
				<tr>
					<td>
						<strong>Password:</strong>
					</td>
					<td>
						<xsl:value-of select="$password" />
					</td>
				</tr>
			</table>
		</p>
		<p>
			Click on the link below to confirm your email address and login:
			<br />
			<a href="{$url}"><xsl:value-of select="$url" /></a>
		</p>
	</xsl:template>

</xsl:stylesheet>