﻿using System.Runtime.Serialization;

namespace Configuration;

public abstract class Config<T>
{
	public string? FilePath { get; set; }
	public T? Data { get; set; }

	/// <summary>
	/// Saves the elements in the Data component to the assigned file
	/// </summary>
	public void Save()
	{
		// Notify the user if this occurs
		if (FilePath is null) return;
		Save(FilePath);
	}
	
	/// <summary>
	/// Populate the Data component of an existing Config&lt;T&gt; object with data from the assigned file
	/// </summary>
	public void Load()
	{
		// Notify the user if this occurs
		if (FilePath is null) return;
		Load(FilePath);
	}

	/// <summary>
	/// Save the elements in the Data component to the specified file 
	/// </summary>
	/// <param name="path">Absolute path to the config file</param>
	public abstract void Save(string path);

	/// <summary>
	/// Populate the Data component of an existing Config&lt;T&gt; object with data from the supplied file
	/// </summary>
	/// <param name="path">Absolute path to the config file</param>
	public abstract void Load(string path);

	/// <summary>
	/// Attempts to load a config from file and instantiate an object of the correct Config&lt;T&gt; type.
	/// Calls the Load() function which may throw other exceptions not listed here depending on the implementation.
	/// </summary>
	/// <param name="path">Absolute path to the config file</param>
	/// <typeparam name="TConfig">The specific derived class, i.e., XmlConfig&lt;MyConfig&gt;</typeparam>
	/// <returns></returns>
	/// <exception cref="SerializationException">Thrown if an object of the desired type can't be created</exception>
	public static TConfig LoadConfig<TConfig>(string path) where TConfig : Config<T>
	{
		TConfig? result = (TConfig?)Activator.CreateInstance(typeof(TConfig));
		if (result is null)
		{
			string name = typeof(TConfig).Name;
			string msg = string.Format(Strings.LoadConfigFailure, name);
			throw new SerializationException(msg);
		}

		result.Load(path);
		return result;
	}
}
