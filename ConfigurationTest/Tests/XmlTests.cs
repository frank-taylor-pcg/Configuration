using Configuration;
using ConfigurationTest.Configs;

namespace ConfigurationTest.Tests;

public class XmlTests
{
	private static Config<BasicTestConfig> Create() =>
		new XmlConfig<BasicTestConfig>(new BasicTestConfig());
	
	private static Config<Configs.Outdated.BasicTestConfig> CreateOutdated() =>
		new XmlConfig<Configs.Outdated.BasicTestConfig>(new Configs.Outdated.BasicTestConfig());

	private static Config<BasicTestConfig> LoadConfig(string path) =>
		Config<BasicTestConfig>.LoadConfig<XmlConfig<BasicTestConfig>>(path);

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
		Helpers.CanSaveAndLoadConfigs<XmlConfig<BasicTestConfig>>(Create, "Xml", b, i, f, s);
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
		Config<ComplexTestConfig> CreateComplex() => new XmlConfig<ComplexTestConfig>();
		Config<ComplexTestConfig> LoadComplex(string path) =>
			XmlConfig<ComplexTestConfig>.LoadConfig<XmlConfig<ComplexTestConfig>>(path);
		
		Helpers.CanSaveAndLoadComplexConfigs(CreateComplex, LoadComplex, "Xml");
	}
}