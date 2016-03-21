using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Ossisoft
{
    public class ExifToolMetadata : ReadOnlyDictionary<string, string>
    {
        public ExifToolMetadata(Dictionary<string, string> dictionary) : base(dictionary)
        {
        }

        public bool TryGetValueAs<TValue>(string key, out TValue value)
        {
            Type t = typeof(TValue);
            string stringValue;
            bool success = TryGetValue(key, out stringValue);

            value = default(TValue);

            if (success)
            {
                if (t == typeof(DateTime))
                {
                    string[] formats =
                    {
                        "yyyy:MM:dd HH:mm:ss",
                        "yyyy:MM:dd HH:mm:ssK"
                    };

                    try
                    {
                        object objectValue = DateTime.ParseExact(stringValue, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
                        value = (TValue)objectValue;
                    }
                    catch (Exception)
                    {
                        success = false;
                    }
                }
                else {
                    try
                    {
                        value = (TValue)Convert.ChangeType(stringValue, t);
                    }
                    catch (Exception)
                    {
                        success = false;
                    }
                }
            }

            return success;
        }
    }
}

