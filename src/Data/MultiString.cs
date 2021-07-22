using PipServices3.Commons.Convert;
using System;
using System.Collections.Generic;
using System.Text;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// An object that contains string translations for multiple languages.
    /// Language keys use two-letter codes like: 'en', 'sp', 'de', 'ru', 'fr', 'pr'.
    /// When translation for specified language does not exists it defaults to English('en').
    /// When English does not exists it falls back to the first defined language.
    /// </summary>
    /// <example>
    /// <code>
    /// var values = MultiString.FromTuples(
    ///    "en", "Hello World!",
    ///    "ru", "Привет мир!"
    /// );
    ///
    /// var value1 = values.Get('ru'); // Result: "Привет мир!"
    /// var value2 = values.Get('pt'); // Result: "Hello World!"
    /// </code>
    /// </example>
    public class MultiString: Dictionary<string, string>
    {
        /// <summary>
        /// Creates a new MultiString object and initializes it with values.
        /// </summary>
        /// <param name="map">a map with language-text pairs.</param>
        public MultiString(Dictionary<string, string> map = null)
        {
            if (map != null)
                this.Append(map);
        }

        /// <summary>
        /// Gets a string translation by specified language.
        /// When language is not found it defaults to English('en').
        /// When English is not found it takes the first value.
        /// </summary>
        /// <param name="language">a language two-symbol code.</param>
        /// <returns>a translation for the specified language or default translation.</returns>
        public string Get(string language)
        {
            // Get specified language
            var value = this.ContainsKey(language) ? this[language]: null;

            // Default to english
            if (value == null)
                value = this.ContainsKey("en") ? this["en"] : null;

            // Default to the first property
            if (value == null)
            {
                foreach(string lang in this.Keys)
                {
                    if (this.ContainsKey(lang))
                    {
                        value = this[lang];
                        break;
                    } 
                }
            }

            return value;
        }

        /// <summary>
        /// Gets all languages stored in this MultiString object.
        /// </summary>
        /// <returns>a list with language codes. </returns>
        public List<string> GetLanguages()
        {
            var languages = new List<string>();

            foreach(var key in this.Keys)
            {
                if (this.ContainsKey(key))
                    languages.Add(key);
            }

            return languages;
        }

        /// <summary>
        /// Puts a new translation for the specified language.
        /// </summary>
        /// <param name="language">a language two-symbol code.</param>
        /// <param name="value">a new translation for the specified language.</param>
        public void Put(string language, dynamic value)
        {
            this[language] = StringConverter.ToNullableString(value);
        }

        /// <summary>
        /// Removes translation for the specified language.
        /// </summary>
        /// <param name="language">a language two-symbol code.</param>
        public new void Remove(string language)
        {
            base.Remove(language);
        }

        /// <summary>
        /// Appends a map with language-translation pairs.
        /// </summary>
        /// <param name="map">the map with language-translation pairs.</param>
        public void Append(Dictionary<string, string> map)
        {
            if (map == null) return;

            foreach (var key in this.Keys)
            {
                var value = map[key];
                if(map.ContainsKey(key))
                    this[key] = StringConverter.ToNullableString(value);
            }
        }

        /// <summary>
        /// Clears all translations from this MultiString object.
        /// </summary>
        public new void Clear()
        {
            base.Clear();
        }

        /// <summary>
        /// Returns the number of translations stored in this MultiString object.
        /// </summary>
        /// <returns>the number of translations.</returns>
        public int Length()
        {
            return this.Keys.Count;
        }

        /// <summary>
        /// Creates a new MultiString object from a value that contains language-translation pairs.
        /// </summary>
        /// <param name="value">the value to initialize MultiString.</param>
        /// <returns>a MultiString object.</returns>
        /// See <see cref="StringValueMap"/>
        public static MultiString FromValue(Dictionary<string, string> value)
        {
            return new MultiString(value);
        }

        /// <summary>
        /// Creates a new MultiString object from language-translation pairs (tuples).
        /// </summary>
        /// <param name="tuples">an array that contains language-translation tuples.</param>
        /// <returns>a MultiString Object.</returns>
        /// See <see cref="FromTuplesArray"/>
        public static MultiString FromTuples(params string[] tuples)
        {
            return MultiString.FromTuplesArray(tuples);
        }

        /// <summary>
        /// Creates a new MultiString object from language-translation pairs (tuples) specified as array.
        /// </summary>
        /// <param name="tuples">an array that contains language-translation tuples.</param>
        /// <returns>a MultiString Object.</returns>
        public static MultiString FromTuplesArray(string[] tuples)
        {
            var result = new MultiString();
            if (tuples == null || tuples.Length == 0)
                return result;

            for (int index = 0; index<tuples.Length; index+=2 )
            {
                if (index + 1 >= tuples.Length) break;

                var name = StringConverter.ToString(tuples[index]);
                var value = StringConverter.ToNullableString(tuples[index + 1]);

                result[name] = value;
            }

            return result;
        }


    }
}
