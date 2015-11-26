using System;
using System.IO;
using System.Web.Services.Discovery;

namespace WebServiceDiscoveryTest
{
	class MainClass
	{
		/// <summary>
		/// On Mono 4.0 the program will output:
		/// 
		/// TempConvert.disco
		/// TempConvert.wsdl
		/// 
		/// On Mono 4.2.1 an exception is thrown.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main (string[] args)
		{
			try {
				string directory = Path.GetDirectoryName (typeof(MainClass).Assembly.Location);
				GenerateReferenceMap (directory);
				ReadReferenceMap (directory);
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}
		}

		static void GenerateReferenceMap (string directory)
		{
			string url = "http://www.w3schools.com/WebServices/TempConvert.asmx";

			var protocol = new DiscoveryClientProtocol ();
			protocol.DiscoverAny (url);
			protocol.ResolveAll ();

			protocol.WriteAll (directory, "Reference.map");
		}

		/// <summary>
		/// protocol.ReadAll fails on Mono 4.2 but not on Mono 4.0 since the
		/// generated Reference.map file contains an incorrect filename.
		/// </summary>
		static void ReadReferenceMap (string directory)
		{
			var protocol = new DiscoveryClientProtocol ();
			var results = protocol.ReadAll (Path.Combine (directory, "Reference.map"));
			foreach (DiscoveryClientResult result in results) {
				Console.WriteLine (result.Filename);
			}
		}
	}
}
