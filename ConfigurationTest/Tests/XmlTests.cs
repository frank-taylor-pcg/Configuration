using Configuration;
using ConfigurationTest.Configs;

namespace ConfigurationTest.Tests;

public class XmlTests
{
	private static Config<BasicConfig> Create() =>
		new XmlConfig<BasicConfig>(new BasicConfig());
	
	private static Config<Configs.Outdated.BasicConfig> CreateOutdated() =>
		new XmlConfig<Configs.Outdated.BasicConfig>(new Configs.Outdated.BasicConfig());

	private static Config<BasicConfig> LoadConfig(string path) =>
		Config<BasicConfig>.LoadConfig<XmlConfig<BasicConfig>>(path);

	[Fact]
	public void CanCreateXmlConfig()
	{
		Helpers.CanCreateConfig(Create);
	}
	
	[Theory]
	[InlineData(true, 1, 2f, "3")]
	[InlineData(false, 4, 5f, "6")]
	[InlineData(true, 7, 8f, "9")]
	public void CanLoadAndSaveXmlConfigs(bool b, int i, float f, string s)
	{
		Helpers.CanSaveAndLoadConfigs<XmlConfig<BasicConfig>>(Create, "Xml", b, i, f, s);
	}
	
	[Theory]
	[InlineData(1, 2f, "3", 13)]
	[InlineData(4, 5f, "6", 17)]
	[InlineData(7, 8f, "9", 19)]
	public void XmlDefaultsAreProperlyLoaded(int i, float f, string s, ushort u)
	{
		Helpers.DefaultsAreProperlyLoaded(CreateOutdated, LoadConfig, "Xml", i, f, s, u);
	}

	[Fact]
	public void CanSaveAndLoadComplexXmlConfigs()
	{
		Config<ComplexConfig> CreateComplex() => new XmlConfig<ComplexConfig>();
		Config<ComplexConfig> LoadComplex(string path) =>
			XmlConfig<ComplexConfig>.LoadConfig<XmlConfig<ComplexConfig>>(path);
		
		Helpers.CanSaveAndLoadComplexConfigs(CreateComplex, LoadComplex, "Xml");
	}
}