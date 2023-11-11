using Configuration;
using ConfigurationTest.Configs;

namespace ConfigurationTest.Tests;

public static class Helpers
{
	public delegate Config<BasicTestConfig> CreateBasic();
	public delegate Config<ComplexTestConfig> CreateComplex();
	public delegate Config<Configs.Outdated.BasicTestConfig> CreateOutdated();
	
	public delegate Config<BasicTestConfig> LoadBasic(string path);
	public delegate Config<ComplexTestConfig> LoadComplex(string path);

	private static void CreateTestPathIfNotExists(string path)
	{
		if (!Path.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}

	public static void CanCreateConfig(CreateBasic create)
	{
		Config<BasicTestConfig> config = create();

		Assert.NotNull(config.Data);
		Assert.Equal(Constants.BoolValue, config.Data.BoolValue);
		Assert.Equal(Constants.IntegerValue, config.Data.IntegerValue);
		Assert.Equal(Constants.FloatValue, config.Data.FloatValue);
		Assert.Equal(Constants.StringValue, config.Data.StringValue);
	}
	
	public static void CanSaveAndLoadConfigs<T>(CreateBasic create, string extension
		, bool b, int i, float f, string s)
	{
		BasicTestConfig basicConfig = new(b, i, f, s);
		Config<BasicTestConfig> expected = create();
		expected.Data = basicConfig;
		
		string filename = $"canSaveAndLoadConfigTest_{i}.{extension}";
		string basePath = Path.Combine(Path.GetTempPath(), "xunit");
		CreateTestPathIfNotExists(basePath);
		string path = Path.Combine(Path.GetTempPath(), "xunit", filename);

		expected.FilePath = path;
		expected.Save();

		Config<BasicTestConfig>? actual = (Config<BasicTestConfig>?)Activator.CreateInstance(typeof(T));
		Assert.NotNull(actual);

		actual.FilePath = path;
		actual.Load();

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
		Configs.Outdated.BasicTestConfig outdatedConfig = new(i, f, s, u);
		Config<Configs.Outdated.BasicTestConfig> expected = create();
		expected.Data = outdatedConfig;
		
		string filename = $"defaultsAreProperlyLoadedConfigTest.{extension}";
		string path = Path.Combine(Path.GetTempPath(), "xunit", filename);

		expected.Save(path);

		Config<BasicTestConfig> actual = load(path);

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
		Config<ComplexTestConfig> expected = create();
		
		string filename = $"canSaveAndLoadComplexConfigTest.{extension}";
		string path = Path.Combine(Path.GetTempPath(), "xunit", filename);

		expected.Save(path);

		Config<ComplexTestConfig> actual = load(path);

		Assert.NotNull(expected.Data);
		Assert.NotNull(actual.Data);
		Assert.Equal(expected.Data.Basic, actual.Data.Basic);
		Assert.Equal(expected.Data.Other, actual.Data.Other);
		Assert.Equal(expected.Data.DoubleValue, actual.Data.DoubleValue);
	}
}