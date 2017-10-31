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
  
  <xsl:key name="AutmaticUpdates-search" match="wix:Component[contains(wix:File/@Source, 'Microarea.Mago4Butler.AutomaticUpdates')]" use="@Id" />
  <xsl:template match="wix:Component[key('AutmaticUpdates-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('AutmaticUpdates-search', @Id)]" />

  <xsl:key name="Dustman-search" match="wix:Component[contains(wix:File/@Source, 'Microarea.Mago4Butler.DustmanButler')]" use="@Id" />
  <xsl:template match="wix:Component[key('Dustman-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('Dustman-search', @Id)]" />

  <xsl:key name="Telemetry-search" match="wix:Component[contains(wix:File/@Source, 'Microarea.Mago4Butler.Telemetry')]" use="@Id" />
  <xsl:template match="wix:Component[key('Telemetry-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('Telemetry-search', @Id)]" />

  <xsl:key name="Reporter-search" match="wix:Component[contains(wix:File/@Source, 'Microarea.Mago4Butler.Reporter')]" use="@Id" />
  <xsl:template match="wix:Component[key('Reporter-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('Reporter-search', @Id)]" />

  <xsl:key name="PluginExample-search" match="wix:Component[contains(wix:File/@Source, 'PluginExample')]" use="@Id" />
  <xsl:template match="wix:Component[key('PluginExample-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('PluginExample-search', @Id)]" />

  <xsl:key name="OtherPlugins-search" match="wix:Component[contains(wix:File/@Source, 'plgn')]" use="@Id" />
  <xsl:template match="wix:Component[key('OtherPlugins-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('OtherPlugins-search', @Id)]" />

  <xsl:key name="NoCefPak-search" match="wix:Component[contains(wix:File/@Source, '.pak')]" use="@Id" />
  <xsl:template match="wix:Component[key('NoCefPak-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('NoCefPak-search', @Id)]" />

  <xsl:key name="NoCefBin-search" match="wix:Component[contains(wix:File/@Source, '.bin')]" use="@Id" />
  <xsl:template match="wix:Component[key('NoCefBin-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('NoCefBin-search', @Id)]" />

  <xsl:key name="NoCefDat-search" match="wix:Component[contains(wix:File/@Source, '.dat')]" use="@Id" />
  <xsl:template match="wix:Component[key('NoCefDat-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('NoCefDat-search', @Id)]" />

  <xsl:key name="NoCefSharp-search" match="wix:Component[contains(wix:File/@Source, 'CefSharp')]" use="@Id" />
  <xsl:template match="wix:Component[key('NoCefSharp-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('NoCefSharp-search', @Id)]" />

  <xsl:key name="Nod3dcompiler-search" match="wix:Component[contains(wix:File/@Source, 'd3dcompiler')]" use="@Id" />
  <xsl:template match="wix:Component[key('Nod3dcompiler-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('Nod3dcompiler-search', @Id)]" />

  <xsl:key name="Nolibcef-search" match="wix:Component[contains(wix:File/@Source, 'libcef.dll')]" use="@Id" />
  <xsl:template match="wix:Component[key('Nolibcef-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('Nolibcef-search', @Id)]" />

  <xsl:key name="NolibEGL-search" match="wix:Component[contains(wix:File/@Source, 'libEGL.dll')]" use="@Id" />
  <xsl:template match="wix:Component[key('NolibEGL-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('NolibEGL-search', @Id)]" />

  <xsl:key name="NolibGLESv2-search" match="wix:Component[contains(wix:File/@Source, 'libGLESv2.dll')]" use="@Id" />
  <xsl:template match="wix:Component[key('NolibGLESv2-search', @Id)]" />
  <xsl:template match="wix:ComponentRef[key('NolibGLESv2-search', @Id)]" />
  
  
  
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