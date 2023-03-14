using Configuration;
using ConfigurationTest.Configs;

namespace ConfigurationTest.Tests;

public class JsonTests
{
	private static Config<BasicConfig> Create() =>
		new JsonConfig<BasicConfig>(new BasicConfig());
	
	private static Config<Configs.Outdated.BasicConfig> CreateOutdated() =>
		new JsonConfig<Configs.Outdated.BasicConfig>(new Configs.Outdated.BasicConfig());

	private static Config<BasicConfig> LoadConfig(string path) =>
		Config<BasicConfig>.LoadConfig<JsonConfig<BasicConfig>>(path);

	[Fact]
	public void CanCreateJsonConfig()
	{
		Helpers.CanCreateConfig(Create);
	}
	
	[Theory]
	[InlineData('z', 1, 2f, "3")]
	[InlineData('y', 4, 5f, "6")]
	[InlineData('x', 7, 8f, "9")]
	public void CanLoadAndSaveJsonConfigs(char c, int i, float f, string s)
	{
		Helpers.CanSaveAndLoadConfigs<JsonConfig<BasicConfig>>(Create, "Json", c, i, f, s);
	}
	
	[Theory]
	[InlineData(1, 2f, "3", 13)]
	[InlineData(4, 5f, "6", 17)]
	[InlineData(7, 8f, "9", 19)]
	public void JsonDefaultsAreProperlyLoaded(int i, float f, string s, ushort u)
	{
		Helpers.DefaultsAreProperlyLoaded(CreateOutdated, LoadConfig, "Json", i, f, s, u);
	}

	[Fact]
	public void CanSaveAndLoadComplexJsonConfigs()
	{
		Config<ComplexConfig> CreateComplex() => new JsonConfig<ComplexConfig>();
		Config<ComplexConfig> LoadComplex(string path) =>
			JsonConfig<ComplexConfig>.LoadConfig<JsonConfig<ComplexConfig>>(path);
		
		Helpers.CanSaveAndLoadComplexConfigs(CreateComplex, LoadComplex, "Json");
	}
}