using System.Diagnostics;
using System.Runtime.Serialization;
using Tomlyn;
using Tomlyn.Syntax;

namespace Configuration;

public class TomlConfig<T> : Config<T> where T : class, new()
{
	public TomlConfig()
	{
		Data = Activator.CreateInstance<T>();
	}

	public TomlConfig(T data)
	{
		Data = data;
	}

	public override void Save(string path)
	{
		try
		{
			if (Data is null) throw new NullReferenceException(Strings.NullDataWarning);

			string result = Toml.FromModel(Data);
			File.WriteAllText(path, result);
		}
		catch (Exception ex)
		{
			string msg = string.Format(Strings.SaveFailure, GetType().Name, ex);
			Debug.WriteLine(msg);

			if (Options.ShouldRethrowExceptions)
				throw;
		}
	}

	public override void Load(string path)
	{
		try
		{
			string contents = File.ReadAllText(path);
			Toml.TryToModel(contents, out T? result, out DiagnosticsBag? diagnostics);
			DumpDiagnosticsMessages(diagnostics);

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

	private static void DumpDiagnosticsMessages(DiagnosticsBag? diagnostics)
	{
		if (diagnostics is null) return;
		foreach (DiagnosticMessage dm in diagnostics)
		{
			Debug.WriteLine($"[{dm.Kind}] {dm.Message} ({dm.Span})");
		}
	}
}