using Configuration;
using ConfigurationTest.Configs;

namespace ConfigurationTest.Tests;

public class TomlTests
{
	private static Config<BasicConfig> Create() =>
		new TomlConfig<BasicConfig>(new BasicConfig());
	
	private static Config<Configs.Outdated.BasicConfig> CreateOutdated() =>
		new TomlConfig<Configs.Outdated.BasicConfig>(new Configs.Outdated.BasicConfig());

	private static Config<BasicConfig> LoadConfig(string path) =>
		Config<BasicConfig>.LoadConfig<TomlConfig<BasicConfig>>(path);

	[Fact]
	public void CanCreateTomlConfig()
	{
		Helpers.CanCreateConfig(Create);
	}
	
	[Theory]
	[InlineData(true, 1, 2f, "3")]
	[InlineData(false, 4, 5f, "6")]
	[InlineData(true, 7, 8f, "9")]
	public void CanLoadAndSaveTomlConfigs(bool b, int i, float f, string s)
	{
		Helpers.CanSaveAndLoadConfigs<TomlConfig<BasicConfig>>(Create, "Toml", b, i, f, s);
	}
	
	// Doesn't handle missing properties, so defaults aren't loaded
	[Theory]
	[InlineData(1, 2f, "3", 13)]
	[InlineData(4, 5f, "6", 17)]
	[InlineData(7, 8f, "9", 19)]
	public void TomlDefaultsAreProperlyLoaded(int i, float f, string s, ushort u)
	{
		Helpers.DefaultsAreProperlyLoaded(CreateOutdated, LoadConfig, "Toml", i, f, s, u);
	}

	// Doesn't handle Guids, so with my current configs - this fails
	[Fact]
	public void CanSaveAndLoadComplexTomlConfigs()
	{
		Config<ComplexConfig> CreateComplex() => new TomlConfig<ComplexConfig>();
		Config<ComplexConfig> LoadComplex(string path) =>
			TomlConfig<ComplexConfig>.LoadConfig<TomlConfig<ComplexConfig>>(path);
		
		Helpers.CanSaveAndLoadComplexConfigs(CreateComplex, LoadComplex, "Toml");
	}
}