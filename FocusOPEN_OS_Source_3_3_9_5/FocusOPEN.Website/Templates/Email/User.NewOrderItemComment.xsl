<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<xsl:import href="Email.Template.xsl" />
	<xsl:output method="html" omit-xml-declaration="yes" />

	<xsl:param name="url">[url]</xsl:param>
	<xsl:param name="first-name">[first-name]</xsl:param>
	<xsl:param name="order-id">[order-id]</xsl:param>
	<xsl:param name="asset-id">[asset-id]</xsl:param>
	<xsl:param name="comment-text">[comment-text]</xsl:param>
	<xsl:param name="comment-date">[comment-date]</xsl:param>
	<xsl:param name="comment-user-name">[comment-user-name]</xsl:param>
	<xsl:param name="current-status">[current-status]</xsl:param>

	<xsl:template name="body-area">
		<p>
			Dear
			<xsl:value-of select="$first-name" />
			,
		</p>
		<p>
			Your order with reference '
			<xsl:value-of select="$order-id" />
			' contains the asset with
			reference '
			<xsl:value-of select="$asset-id" />
			', which requires approval before it
			can be downloaded.
			<br />
			<br />
			The user responsible for approving this request is
			<xsl:value-of select="$comment-user-name" />
			, who
			left the following comment on
			<xsl:value-of select="$comment-date" />
			.
			<br />
			<br />

			<em>
				<pre style="font-size:1.3em;margin-left:15px;">
					<xsl:value-of select="$comment-text" />
				</pre>
			</em>

			<br />

			<xsl:if test="$current-status = 'awaitingapproval'">
				This request is still awaiting approval, and more information is required before it can be approved for download.
				<br />
				<br />
				Please click on the link below to go to view your order where you can respond to this.
			</xsl:if>

			<xsl:if test="$current-status = 'approved'">
				<span style="font-weight:bold;color:green">
					This request has been approved for download.
				</span>
				<br />
				<br />
				Please click on the link below to go to view your order where you can download the requested asset.
			</xsl:if>

			<xsl:if test="$current-status = 'rejected'">
				<span style="font-weight:bold;color:red">
					This request has been denied.
				</span>
				<br />
				<br />
				Please click on the link below to go to view your order.
			</xsl:if>

			<br />
			<br />
			
			<a href="{$url}"><xsl:value-of select="$url" /></a>

		</p>
	</xsl:template>

</xsl:stylesheet>