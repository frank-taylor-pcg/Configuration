using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Configuration;

public class XmlConfig<T> : Config<T>
{
	// public T? Data { get; set; }

	public XmlConfig()
	{
		Data = Activator.CreateInstance<T>();
	}

	public XmlConfig(T data)
	{
		Data = data;
	}

	private static XmlWriterSettings GetSettings()
	{
		return new XmlWriterSettings
		{
			Indent = true,
			IndentChars = Options.IndentCharacters,
			NewLineOnAttributes = true
		};
	}

	/// <summary>
	/// Attempts to save the current configuration values to a file
	/// </summary>
	/// <param name="path">Absolute path of the file for storing the configuration values</param>
	public override void Save(string path)
	{
		try
		{
			if (Data is null) throw new NullReferenceException(Strings.NullDataWarning);

			XmlSerializer xml = new(Data.GetType());
			using XmlWriter xr = XmlWriter.Create(path, GetSettings());
			xml.Serialize(xr, Data);
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
	public override void Load(string path)
	{
		try
		{
			T? result = default;
			
			XmlSerializer xml = new(typeof(T));
			using XmlReader xr = XmlReader.Create(path);
			if (xml.CanDeserialize(xr))
			{
				result = (T?)xml.Deserialize(xr);
			}

			if (result != null)
			{
				Data = result;
				return;
			}
			
			string msg = string.Format(Strings.DeserializationFailure, path, GetType().Name);
			throw new SerializationException(msg);
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