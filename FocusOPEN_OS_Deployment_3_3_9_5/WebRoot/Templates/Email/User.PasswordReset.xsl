<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="first-name">[first-name]</xsl:param>
	<xsl:param name="email">[email]</xsl:param>
	<xsl:param name="password">[password]</xsl:param>

	<xsl:template name="body-area">
		<p>
			Dear
			<xsl:value-of select="$first-name" />
			,
		</p>
		<p>
			Your password for the
			<xsl:value-of select="$appName" />
			has been reset and is below:
		</p>
		<p>
			<em>
				<xsl:value-of select="$password" />
			</em>
		</p>
		<p>
			<em>
				This password is case sensitive and can only be used once.  You will be required to change it to something else the next time you login.
			</em>
		</p>
	</xsl:template>

</xsl:stylesheet>