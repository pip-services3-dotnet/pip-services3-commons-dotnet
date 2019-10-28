using PipServices3.Commons.Reflect;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PipServices3.Commons.Data
{
	/// <summary>
	/// Helper class to extract and process search tags from objects.
	/// The search tags can be kept individually or embedded as hash tags inside text
	/// like "This text has #hash_tag that can be used for search."
	///  </summary>

	public static class TagsProcessor
	{
		private const string NORMALIZE_REGEX = "(_|#)+";
		private const string COMPRESS_REGEX = "( |_|#)+";
		private const string SPLIT_REGEX = "(,|;)+";
		private const string HASHTAG_REGEX = "#\\w+";

		/// <summary>
		/// Normalizes a tag by replacing special symbols like '_' and '#' with spaces.
		/// When tags are normalized then can be presented to user in similar shape and form.
		/// </summary>
		/// <param name="tag">the tag to normalize.</param>
		/// <returns>a normalized tag.</returns>
		public static string NormalizeTag(string tag)
		{
			return tag != null
				? Regex.Replace(tag, NORMALIZE_REGEX, " ").Trim()
				: null;
		}

		/// <summary>
		/// Compress a tag by removing special symbols like spaces, '_' and '#'
		/// and converting the tag to lower case.
		/// When tags are compressed they can be matched in search queries.
		/// </summary>
		/// <param name="tag">the tag to compress.</param>
		/// <returns>a compressed tag.</returns>
		public static string CompressTag(string tag)
		{
			return tag != null
				? Regex.Replace(tag, COMPRESS_REGEX, "").ToLower()
				: null;
		}

		/// <summary>
		///  Compares two tags using their compressed form.
		/// </summary>
		/// <param name="tag1">the first tag.</param>
		/// <param name="tag2">the second tag.</param>
		/// <returns>true if the tags are equal and false otherwise.</returns>
		public static bool EqualTags(string tag1, string tag2)
		{
			if (tag1 == null && tag2 == null)
				return true;
			if (tag1 == null || tag2 == null)
				return false;
			return CompressTag(tag1) == CompressTag(tag2);
		}


		/// <summary>
		/// Normalizes a list of tags.
		/// </summary>
		/// <param name="tags">the tags to normalize.</param>
		/// <returns>a list with normalized tags.</returns>
		public static string[] NormalizeTags(string[] tags)
		{
			return tags.Select(NormalizeTag).ToArray();
		}

		/// <summary>
		/// Normalizes a comma-separated list of tags.
		/// </summary>
		/// <param name="tagList">a comma-separated list of tags to normalize.</param>
		/// <returns>a list with normalized tags.</returns>
		public static string[] NormalizeTagList(string tagList)
		{
			var tags = Regex.Split(tagList, SPLIT_REGEX).ToList();
			// Remove separators
			for (var index = 0; index < tags.Count - 1; index++)
				tags.Splice(index + 1, 1);

			return NormalizeTags(tags.ToArray());
		}

		public static List<T> Splice<T>(this List<T> source, int index, int count)
		{
			var items = source.GetRange(index, count);
			source.RemoveRange(index, count);
			return items;
		}


		/// <summary>
		/// Compresses a list of tags.
		/// </summary>
		/// <param name="tags">the tags to compress.</param>
		/// <returns>a list with normalized tags.</returns>
		public static string[] CompressTags(string[] tags)
		{
			return tags.Select(CompressTag).ToArray();
		}


		/// <summary>
		/// Compresses a comma-separated list of tags.
		/// </summary>
		/// <param name="tagList">a comma-separated list of tags to compress.</param>
		/// <returns>a list with compressed tags.</returns>
		public static string[] CompressTagList(string tagList)
		{
			//var tags = tagList.split(this.SPLIT_REGEX);
			var tags = Regex.Split(tagList, SPLIT_REGEX);

			var tagsList = tags.ToList();
			// Remove separators
			for (var index = 0; index < tagsList.Count - 1; index++)
				tagsList.Splice(index + 1, 1);

			return CompressTags(tagsList.ToArray());
		}



		/// <summary>
		/// Extracts hash tags from a text.
		/// </summary>
		/// <param name="text">a text that contains hash tags</param>
		/// <returns>a list with extracted and compressed tags.</returns>
		public static string[] ExtractHashTags(string text)
		{
			var tags = new string[0];

			if (text != "")
			{
				string[] hashTags = (from Match match in Regex.Matches(text, HASHTAG_REGEX) select match.Value).ToArray();
				tags = CompressTags(hashTags);
			}

			return tags.Distinct().ToArray();
		}

		private static string ExtractString(dynamic field)
		{
			if (field == null) return "";

			if (field is string) return field.ToString();

			var result = "";
			if (field is Dictionary<string, string>)
			{
				foreach (string prop in field.Keys)
				{
					result += " " + ExtractString(field[prop]);
				}

				return result;
			}

			var properties = PropertyReflector.GetProperties(field);
			foreach (string prop in properties.Keys)
			{
				result += " " + ExtractString(properties[prop]);
			}
			return result;
		}

		/// <summary>
		/// Extracts hash tags from selected fields in an object.
		/// </summary>
		/// <param name="obj">an object which contains hash tags.</param>
		/// <param name="searchFields">a list of fields in the objects where to extract tags</param>
		/// <returns>a list of extracted and compressed tags.</returns>
		public static string[] ExtractHashTagsFromValue(dynamic obj, params string[] searchFields)
		{
			// Todo: Use recursive
			string[] tags = CompressTags(obj.Tags.ToArray());

			foreach (string field in searchFields)
			{
				string text = ExtractString(PropertyReflector.GetProperty(obj, field));

				if (text != "")
				{
					string[] hashTags = (from Match match in Regex.Matches(text, HASHTAG_REGEX) select match.Value).ToArray();
					tags = tags.Concat(CompressTags(hashTags).ToList()).ToArray();
				}

			}

			return tags.Distinct().ToArray();
		}
	}
}
