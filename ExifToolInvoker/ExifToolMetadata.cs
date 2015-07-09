using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Ossisoft
{
	public class ExifToolMetadata : ReadOnlyDictionary<string, string>
	{
		public ExifToolMetadata(Dictionary<string, string>  dictionary) : base(dictionary)
		{
		}

		public bool ContainsKey(ExifToolTagNames key)
		{
			return ContainsKey (key.ToString ());
		}

		public bool TryGetValueAs<T>(ExifToolTagNames key, out T value)
		{
			return TryGetValueAs<T> (key.ToString (), out value);
		}

		public bool TryGetValueAs<T>(string key, out T value)
		{
			Type t = typeof(T);
			string stringValue;
			bool success = TryGetValue (key, out stringValue);

			value = default(T);

			if (success) {
				if (t == typeof(DateTime)) {
					string[] formats = 
					{
						"yyyy:MM:dd HH:mm:ss",
						"yyyy:MM:dd HH:mm:ssK"
					};

					try {
						object objectValue = DateTime.ParseExact (stringValue, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
						value = (T)objectValue;
					} catch (Exception) {
						success = false;
					}
				} else {
					try {
						value = (T)Convert.ChangeType (stringValue, t);	
					} catch (Exception) {
						success = false;
					}
				}
			}

			return success;
		}
	}
}

