namespace Configuration;

public static class Options
{
	/// <summary>
	/// The characters to use when indenting the configuration file
	/// </summary>
	// ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
	public static string IndentCharacters { get; set; } = "\t";
	
	/// <summary>
	/// Set this to false if you want exceptions to be reported to the Debug console, but otherwise ignored.
	/// </summary>
	// ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
	public static bool ShouldRethrowExceptions { get; set; } = true;
}