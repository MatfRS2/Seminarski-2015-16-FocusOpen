<?xml version="1.0" encoding="utf-8"?>

<!DOCTYPE xsl:stylesheet  [
	<!ENTITY nbsp   "&#160;">
	<!ENTITY copy   "&#169;">
	<!ENTITY reg    "&#174;">
	<!ENTITY trade  "&#8482;">
	<!ENTITY mdash  "&#8212;">
	<!ENTITY ldquo  "&#8220;">
	<!ENTITY rdquo  "&#8221;"> 
	<!ENTITY pound  "&#163;">
	<!ENTITY yen    "&#165;">
	<!ENTITY euro   "&#8364;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="html" encoding="utf-8" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />

	<xsl:param name="appPath">../</xsl:param>
	<xsl:param name="orgName">../</xsl:param>
	<xsl:param name="appName">../</xsl:param>

	<xsl:template match="/">
		<html xmlns="http://www.w3.org/1999/xhtml">
			<head>
				<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
				<title>
					<xsl:value-of select="$appName" />
				</title>
				<style type="text/css">
					html,body{height:100%;width:100%;margin:0px;padding:0px;}
					body{background:#fff;font-size:11px;font-family:arial,helvetica,sans-serif;}
					td{font-size:11px;font-family:arial,helvetica,sans-serif;}
				</style>
			</head>
			<body bgcolor="#ffffff" text="#000000" topmargin="0" leftmargin="0">
				<style type="text/css">
					html,body{height:100%;width:100%;margin:0px;padding:0px;}
					body{background:#fff;font-size:11px;font-family:arial,helvetica,sans-serif;}
					td{font-size:11px;font-family:arial,helvetica,sans-serif;}
				</style>
				<xsl:call-template name="body-area" />
				<br />
				Regards,
				<br />
				<xsl:value-of select="$appName" />
				<br />
				<br />
				<p>
					<em>
						This is an automated message.  Please do not reply.
					</em>
				</p>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>