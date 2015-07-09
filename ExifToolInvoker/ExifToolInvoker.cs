﻿using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Ossisoft
{
	public class ExifToolInvoker
	{
		private string exifToolPath;

		public static string ExifToolPath = "exiftool";

		public ExifToolInvoker () : this(ExifToolPath)
		{
		}

		public ExifToolInvoker(string exifToolPath)
		{
			this.exifToolPath = exifToolPath;
		}

		public Process Invoke(string arguments)
		{
			var proc = new Process {
				StartInfo = new ProcessStartInfo {
					FileName = exifToolPath,
					Arguments = arguments,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true
				}
			};

			proc.Start();

			return proc;
		}

		public ExifToolMetadata GetMetadata(string filePath)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string> ();

			var proc = Invoke (string.Format ("-s {0}", filePath));

			while (!proc.StandardOutput.EndOfStream) {
				string line = proc.StandardOutput.ReadLine();

				string[] kvp = line.Split (new char[] { ':' }, 2);

				if (kvp.Length == 2)
				{
					string key = kvp [0].Trim ();
					string value = kvp [1].Trim ();

					if (!dictionary.ContainsKey (key)) {
						dictionary.Add (key, value);
					}
				}
			}

			return new ExifToolMetadata(dictionary);
		}
	}
}

