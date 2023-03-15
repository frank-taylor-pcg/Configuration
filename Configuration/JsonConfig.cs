using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Configuration;

public class JsonConfig<T> : Config<T>
{
	public JsonConfig()
	{
	}

	public JsonConfig(T data) : base(data)
	{
	}

	private static JsonSerializerSettings GetSettings()
	{
		return new JsonSerializerSettings
		{
			Formatting = Formatting.Indented
		};
	}

	/// <summary>
	/// Attempts to save the current configuration values to a file
	/// </summary>
	/// <param name="path">Absolute path of the file for storing the configuration values</param>
	public override void Save(string? path)
	{
		try
		{
			if (Data is null) throw new NullReferenceException(Strings.NullDataWarning);
			if (path is null) throw new NullReferenceException(Strings.NullPathWarning);

			string content = JsonConvert.SerializeObject(Data, GetSettings());
			File.WriteAllText(path, content);
		}
		catch (Exception ex)
		{
			string msg = string.Format(Strings.SaveFailure, GetType().Name, ex);
			Debug.WriteLine(msg);

			if (Options.ShouldRethrowExceptions)
				throw;
		}
	}

	/// <summary>
	/// Attempts to load the stored configuration values
	/// </summary>
	/// <param name="path">Absolute path of the file for retrieving the configuration values</param>
	public override void Load(string? path)
	{
		try
		{
			if (path is null) throw new NullReferenceException(Strings.NullPathWarning);

			string fileContent = File.ReadAllText(path);
			T? result = JsonConvert.DeserializeObject<T>(fileContent);

			if (result is null)
			{
				string msg = string.Format(Strings.DeserializationFailure, path, GetType().Name);
				throw new SerializationException(msg);
			}

			Data = result;
		}
		catch
		{
			Debug.WriteLine(Strings.LoadFailure);
			Data = Activator.CreateInstance<T>();

			if (Options.ShouldRethrowExceptions)
				throw;
		}
	}
}