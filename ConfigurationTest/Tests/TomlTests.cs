using Configuration;
using ConfigurationTest.Configs;

namespace ConfigurationTest.Tests;

public class TomlTests
{
	private static Config<BasicTestConfig> Create() =>
		new TomlConfig<BasicTestConfig>(new BasicTestConfig());
	
	private static Config<Configs.Outdated.BasicTestConfig> CreateOutdated() =>
		new TomlConfig<Configs.Outdated.BasicTestConfig>(new Configs.Outdated.BasicTestConfig());

	private static Config<BasicTestConfig> LoadConfig(string path) =>
		Config<BasicTestConfig>.LoadConfig<TomlConfig<BasicTestConfig>>(path);

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
		Helpers.CanSaveAndLoadConfigs<TomlConfig<BasicTestConfig>>(Create, "Toml", b, i, f, s);
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
		Config<ComplexTestConfig> CreateComplex() => new TomlConfig<ComplexTestConfig>();
		Config<ComplexTestConfig> LoadComplex(string path) =>
			TomlConfig<ComplexTestConfig>.LoadConfig<TomlConfig<ComplexTestConfig>>(path);
		
		Helpers.CanSaveAndLoadComplexConfigs(CreateComplex, LoadComplex, "Toml");
	}
}