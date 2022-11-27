<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="first-name">[first-name]</xsl:param>
	<xsl:param name="ftp-messages">[ftp-messages]</xsl:param>

	<xsl:template name="body-area">
		<p>
			Dear
			<xsl:value-of select="$first-name" />
			,
		</p>
		<p>
			The assets selected for transfer to FTP have been transferred sucessfully. The FTP log is attached.
			<br />
			<br />
			If you have any trouble retrieving these asset files, you can restart the download by logging into your account
			and using the download manager.
		</p>
	</xsl:template>

</xsl:stylesheet>