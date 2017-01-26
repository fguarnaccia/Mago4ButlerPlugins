<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl" xmlns:wix="http://schemas.microsoft.com/wix/2006/wi">
  <xsl:template match="wix:Wix">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:apply-templates select="*" />
    </xsl:copy>
  </xsl:template>
  <!-- Copy all attributes and elements to the output. -->
  <xsl:template match="@*|*">
    <xsl:copy>
      <xsl:apply-templates select="@*" />
      <xsl:apply-templates select="*" />
    </xsl:copy>
  </xsl:template>
  <xsl:output method="xml" indent="yes" />

  <xsl:key name="log-search" match="wix:Component[contains(wix:File/@Source, '.log')]" use="@Id" />
  <xsl:template match="wix:Component[key('log-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('log-search', @Id)]" />
  
  <xsl:key name="pdb-search" match="wix:Component[contains(wix:File/@Source, '.pdb')]" use="@Id" />
  <xsl:template match="wix:Component[key('pdb-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('pdb-search', @Id)]" />
  
  <xsl:key name="xml-search" match="wix:Component[contains(wix:File/@Source, '.xml')]" use="@Id" />
  <xsl:template match="wix:Component[key('xml-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('xml-search', @Id)]" />
  
  <xsl:key name="locked-search" match="wix:Component[contains(wix:File/@Source, '.locked')]" use="@Id" />
  <xsl:template match="wix:Component[key('locked-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('locked-search', @Id)]" />
  
  <xsl:key name="vshost-search" match="wix:Component[contains(wix:File/@Source, '.vshost.')]" use="@Id" />
  <xsl:template match="wix:Component[key('vshost-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('vshost-search', @Id)]" />
  
  <xsl:key name="XmlSerializers-search" match="wix:Component[contains(wix:File/@Source, '.XmlSerializers.')]" use="@Id" />
  <xsl:template match="wix:Component[key('XmlSerializers-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('XmlSerializers-search', @Id)]" />
  
  <xsl:key name="PluginExample-search" match="wix:Component[contains(wix:File/@Source, 'PluginExample')]" use="@Id" />
  <xsl:template match="wix:Component[key('PluginExample-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('PluginExample-search', @Id)]" />

  <xsl:key name="NoTbs-search" match="wix:Component[contains(wix:File/@Source, 'Microarea.Mago4Butler.NoTbs')]" use="@Id" />
  <xsl:template match="wix:Component[key('NoTbs-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('NoTbs-search', @Id)]" />

  <xsl:key name="NoCefPak-search" match="wix:Component[contains(wix:File/@Source, 'cef.pak')]" use="@Id" />
  <xsl:template match="wix:Component[key('NoCefPak-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('NoCefPak-search', @Id)]" />
  
  <xsl:key name="NoCefPakExtensions-search" match="wix:Component[contains(wix:File/@Source, 'cef_extensions.pak')]" use="@Id" />
  <xsl:template match="wix:Component[key('NoCefPakExtensions-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('NoCefPakExtensions-search', @Id)]" />
  
  <xsl:key name="Nod3dcompiler_43-search" match="wix:Component[contains(wix:File/@Source, 'd3dcompiler_43.dll')]" use="@Id" />
  <xsl:template match="wix:Component[key('Nod3dcompiler_43-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('Nod3dcompiler_43-search', @Id)]" />
  
  <xsl:key name="Nodevtools_resources-search" match="wix:Component[contains(wix:File/@Source, 'devtools_resources.pak')]" use="@Id" />
  <xsl:template match="wix:Component[key('Nodevtools_resources-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('Nodevtools_resources-search', @Id)]" />
  
  <xsl:key name="Nowidevinecdmadapter-search" match="wix:Component[contains(wix:File/@Source, 'widevinecdmadapter.dll')]" use="@Id" />
  <xsl:template match="wix:Component[key('Nowidevinecdmadapter-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('Nowidevinecdmadapter-search', @Id)]" />
  
  <xsl:template match='wix:Component'>
    <xsl:copy>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates select="*" />
    </xsl:copy>
  </xsl:template>
  <xsl:template match='wix:File'>
    <xsl:copy>
      <xsl:apply-templates select="@*"/>
      <xsl:attribute name="ProcessorArchitecture">
        <xsl:choose>
          <xsl:when test="contains(@Source, 'x64')">
            <xsl:text>x64</xsl:text>
          </xsl:when>
          <xsl:when test="contains(@Source, 'x86')">
            <xsl:text>x86</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>msil</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:apply-templates select="*" />
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>