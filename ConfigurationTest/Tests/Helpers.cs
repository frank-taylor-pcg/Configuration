using Configuration;
using ConfigurationTest.Configs;

namespace ConfigurationTest.Tests;

public static class Helpers
{
	public delegate Config<BasicConfig> CreateBasic();
	public delegate Config<ComplexConfig> CreateComplex();
	public delegate Config<Configs.Outdated.BasicConfig> CreateOutdated();
	
	public delegate Config<BasicConfig> LoadBasic(string path);
	public delegate Config<ComplexConfig> LoadComplex(string path);

	public static void CanCreateConfig(CreateBasic create)
	{
		Config<BasicConfig> config = create();

		Assert.NotNull(config.Data);
		Assert.Equal(Constants.BoolValue, config.Data.BoolValue);
		Assert.Equal(Constants.IntegerValue, config.Data.IntegerValue);
		Assert.Equal(Constants.FloatValue, config.Data.FloatValue);
		Assert.Equal(Constants.StringValue, config.Data.StringValue);
	}
	
	public static void CanSaveAndLoadConfigs<T>(CreateBasic create, string extension
		, bool b, int i, float f, string s)
	{
		BasicConfig basicConfig = new(b, i, f, s);
		Config<BasicConfig> expected = create();
		expected.Data = basicConfig;
		
		string filename = $"canSaveAndLoadConfigTest_{i}.{extension}";
		string path = Path.Combine(Path.GetTempPath(), "xunit", filename);

		expected.Save(path);

		Config<BasicConfig>? actual = (Config<BasicConfig>?)Activator.CreateInstance(typeof(T));
		Assert.NotNull(actual);
		
		actual.Load(path);

		Assert.NotNull(expected.Data);
		Assert.NotNull(actual.Data);
		Assert.Equal(expected.Data.BoolValue, actual.Data.BoolValue);
		Assert.Equal(expected.Data.FloatValue, actual.Data.FloatValue);
		Assert.Equal(expected.Data.IntegerValue, actual.Data.IntegerValue);
		Assert.Equal(expected.Data.StringValue, actual.Data.StringValue);
	}

	public static void DefaultsAreProperlyLoaded(CreateOutdated create, LoadBasic load, string extension,
		int i, float f, string s, ushort u)
	{
		Configs.Outdated.BasicConfig outdatedConfig = new(i, f, s, u);
		Config<Configs.Outdated.BasicConfig> expected = create();
		expected.Data = outdatedConfig;
		
		string filename = $"defaultsAreProperlyLoadedConfigTest.{extension}";
		string path = Path.Combine(Path.GetTempPath(), "xunit", filename);

		expected.Save(path);

		Config<BasicConfig> actual = load(path);

		Assert.NotNull(expected.Data);
		Assert.NotNull(actual.Data);
		// The character value was not included in the PartialConfig so verify that the default was loaded
		Assert.Equal(Constants.BoolValue, actual.Data.BoolValue);
		Assert.Equal(expected.Data.FloatValue, actual.Data.FloatValue);
		Assert.Equal(expected.Data.IntegerValue, actual.Data.IntegerValue);
		Assert.Equal(expected.Data.StringValue, actual.Data.StringValue);
	}

	//, Guid id, object? obj, long l)
	public static void CanSaveAndLoadComplexConfigs(CreateComplex create, LoadComplex load, string extension)
	{
		Config<ComplexConfig> expected = create();
		
		string filename = $"canSaveAndLoadComplexConfigTest.{extension}";
		string path = Path.Combine(Path.GetTempPath(), "xunit", filename);

		expected.Save(path);

		Config<ComplexConfig> actual = load(path);

		Assert.NotNull(expected.Data);
		Assert.NotNull(actual.Data);
		Assert.Equal(expected.Data.Basic, actual.Data.Basic);
		Assert.Equal(expected.Data.Other, actual.Data.Other);
		Assert.Equal(expected.Data.DoubleValue, actual.Data.DoubleValue);
	}
}