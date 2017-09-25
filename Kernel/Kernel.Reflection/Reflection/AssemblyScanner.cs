namespace Kernel.Reflection
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	/// <summary>
	/// Scans for available assemblies and excludes irrelevant onces
	/// </summary>
	public class AssemblyScanner
	{
		private static Lazy<IEnumerable<Assembly>> scannableAssemblies = new Lazy<IEnumerable<Assembly>>(new Func<IEnumerable<Assembly>>(AssemblyScanner.GetScannableAssemblies));
		
		public static IEnumerable<Assembly> ScannableAssemblies { get { return AssemblyScanner.scannableAssemblies.Value; } }
		
		private static IEnumerable<Assembly> GetScannableAssemblies()
		{
			var results = new List<Assembly>();

			var appDomain = AppDomain.CurrentDomain.GetAssemblies();

			results.AddRange(appDomain);

			//Get all files defined by GetFileSearchPatternsToUse. 
			var filesToScan = ScanDirectoryForAssemblyFiles();

			foreach (var assemblyFile in filesToScan)
			{
				//Excludes any file from obj folder
				if (assemblyFile.Directory.FullName.Contains("\\obj\\"))
					continue;

				ScanAssembly(assemblyFile.FullName, results);
			}

			return results;
		}

		private static void ScanAssembly(string assemblyPath, IList<Assembly> results)
		{
			Assembly assembly;

			//Get compilation mode. See Image class for more info
			var compilationMode = Image.GetCompilationMode(assemblyPath);

			//Ignore native
			if (compilationMode == CompilationMode.NativeOrInvalid)
			{
				//log here
				//LoggerManager.WriteInformationToEventLog(string.Format("Assembly {0} is skipped as it is a native or invalid assembly", assemblyPath));

				return;
			}

			//Ignore 32bit assembly in 64bits environment
			if (!Environment.Is64BitProcess && compilationMode == CompilationMode.CLRx64)
			{
				//log here
				//LoggerManager.WriteInformationToEventLog(string.Format("Assembly {0} is skipped as it is a CLR64 assembly", assemblyPath));

				return;
			}

			try
			{
				//Ignore runtime assemblies
				if (IsRuntimeAssembly(assemblyPath))
				{
					//log here
					//LoggerManager.WriteInformationToEventLog(string.Format("Assembly {0} is skipped as it is a runtime assembly", assemblyPath));

					return;
				}

				assembly = Assembly.LoadFrom(assemblyPath);

				if (results.Contains(assembly))
				{
					return;
				}
			}
			catch (BadImageFormatException ex)
			{
				//log here
				//LoggerManager.WriteExceptionToEventLog(ex);

				return;
			}

			results.Add(assembly);
		}

		/// <summary>
		/// Scan application domain base folder and sub folders for file with extensions defined in pattern.
		/// </summary>
		/// <param name="scanSubFolders">true to search sub folders otherwise false</param>
		/// <returns>Files to be scan</returns>
		private static IEnumerable<FileInfo> ScanDirectoryForAssemblyFiles(bool scanSubFolders = true)
		{
			var baseDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

			var searchOption = scanSubFolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			return GetFileSearchPatternsToUse()
				.SelectMany(extension => baseDir.GetFiles(extension, searchOption));
		}

		/// <summary>
		/// Determines if the given assembly is runtime assembly. Could be refactored and provider with runtime assemblies to be injected
		/// </summary>
		/// <param name="assemblyPath"></param>
		/// <returns></returns>
		internal static bool IsRuntimeAssembly(string assemblyPath)
		{
			var publicKeyToken = AssemblyName.GetAssemblyName(assemblyPath).GetPublicKeyToken();

			var lowerInvariant = BitConverter.ToString(publicKeyToken).Replace("-", "").ToLowerInvariant();

			//System
			if (lowerInvariant == "b77a5c561934e089")
			{
				return true;
			}

			//Web
			if (lowerInvariant == "b03f5f7f11d50a3a")
			{
				return true;
			}

			//patterns and practices
			if (lowerInvariant == "31bf3856ad364e35")
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Dedfines the file extensions to be included in the scan. Default is *.dll. Could be made configurable.
		/// </summary>
		/// <returns></returns>
		private static IEnumerable<string> GetFileSearchPatternsToUse()
		{
			yield return "*.dll";
		}
	}
}