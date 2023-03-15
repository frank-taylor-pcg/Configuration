namespace Configuration;

internal static class Strings
{
	internal const string NullDataWarning = "Config.Data cannot be null";

	internal const string NullPathWarning = "File path cannot be null";

	internal static readonly string SaveFailure =
		$"The following error occurred when attempting to call {0}:{Environment.NewLine}{1}";
	
	internal const string LoadFailure = "Failed to load from file.  Loading defaults";

	internal static readonly string DeserializationFailure =
		$"Failed to deserialize contents of {0} into object of type {1}";

	internal static readonly string LoadConfigFailure =
		$"Failed to create instance of object type {0} in during call to LoadConfig";
}