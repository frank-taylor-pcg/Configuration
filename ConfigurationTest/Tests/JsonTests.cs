using Configuration;
using ConfigurationTest.Configs;

namespace ConfigurationTest.Tests;

public class JsonTests
{
	private static Config<BasicTestConfig> Create() =>
		new JsonConfig<BasicTestConfig>(new BasicTestConfig());
	
	private static Config<Configs.Outdated.BasicTestConfig> CreateOutdated() =>
		new JsonConfig<Configs.Outdated.BasicTestConfig>(new Configs.Outdated.BasicTestConfig());

	private static Config<BasicTestConfig> LoadConfig(string path) =>
		Config<BasicTestConfig>.LoadConfig<JsonConfig<BasicTestConfig>>(path);

	[Fact]
	public void CanCreateJsonConfig()
	{
		Helpers.CanCreateConfig(Create);
	}
	
	[Theory]
	[InlineData(true, 1, 2f, "3")]
	[InlineData(false, 4, 5f, "6")]
	[InlineData(true, 7, 8f, "9")]
	public void CanLoadAndSaveJsonConfigs(bool b, int i, float f, string s)
	{
		Helpers.CanSaveAndLoadConfigs<JsonConfig<BasicTestConfig>>(Create, "Json", b, i, f, s);
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
		Config<ComplexTestConfig> CreateComplex() => new JsonConfig<ComplexTestConfig>();
		Config<ComplexTestConfig> LoadComplex(string path) =>
			JsonConfig<ComplexTestConfig>.LoadConfig<JsonConfig<ComplexTestConfig>>(path);
		
		Helpers.CanSaveAndLoadComplexConfigs(CreateComplex, LoadComplex, "Json");
	}
}