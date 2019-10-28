using System.Collections.Generic;
using Xunit;

namespace PipServices3.Commons.Data
{
	public class TagsProcessorTest
	{
		[Fact]
		public void TestNormalizeTags()
		{
			var tag = TagsProcessor.NormalizeTag("  A_b#c ");
			Assert.Equal("A b c", tag);

			var tags = TagsProcessor.NormalizeTags(new[] { "  A_b#c ", "d__E f" });
			Assert.Equal(2, tags.Length);
			Assert.Equal("A b c", tags[0]);
			Assert.Equal("d E f", tags[1]);

			tags = TagsProcessor.NormalizeTagList("  A_b#c ,d__E f;;");
			Assert.Equal(3, tags.Length);
			Assert.Equal("A b c", tags[0]);
			Assert.Equal("d E f", tags[1]);

		}

		[Fact]
		public void TestCompressTags()
		{
			var tag = TagsProcessor.CompressTag("  A_b#c ");
			Assert.Equal("abc", tag);

			var tags = TagsProcessor.CompressTags(new[] { "  A_b#c ", "d__E f" });
			Assert.Equal(2, tags.Length);
			Assert.Equal("abc", tags[0]);
			Assert.Equal("def", tags[1]);

			tags = TagsProcessor.CompressTagList("  A_b#c ,d__E f;;");
			Assert.Equal(3, tags.Length);

			Assert.Equal("abc", tags[0]);
			Assert.Equal("def", tags[1]);

		}

		[Fact]
		public void TestExtractHashTags()
		{
			var tags = TagsProcessor.ExtractHashTags("  #Tag_1  #TAG2#tag3 ");
			Assert.Equal(3, tags.Length);
			Assert.Equal("tag1", tags[0]);
			Assert.Equal("tag2", tags[1]);
			Assert.Equal("tag3", tags[2]);
		}

		public class Test1
		{
			public List<string> Tags = new List<string> { "Tag 1", "tag_2", "TAG3" };
			public string Name = "Text with tag1 #Tag1";
			public string Description = "Text with #tag_2,#tag3-#tag4 and #TAG__5";
		}

		public class NameTest
		{
			public string Short = "Just a name";
			public string Full = "Text with tag1 #Tag1";
		}
		public class DescriptionTest
		{
			public string En = "Text with #tag_2,#tag4 and #TAG__5";
			public string Ru = "Текст с #tag_2,#tag3 и #TAG__5";
		}
		public class Test2
		{
			public List<string> Tags = new List<string> { "Tag 1", "tag_2", "TAG3" };
			public NameTest Name = new NameTest();
			public DescriptionTest Description = new DescriptionTest();
		}
		public class Test3
		{
			public List<string> Tags = new List<string> { "Tag 1", "tag_2", "TAG3" };
			public Dictionary<string, string> Name = new Dictionary<string, string> { { "Text with tag1", "#Tag1" } };
			public Dictionary<string, string> Description = new Dictionary<string, string> { { "Text with", " #tag_2,#tag3-#tag4 " }, { "Text with2", " and #TAG__5" } };
		}
		public class Test4
		{
			public List<string> Tags = new List<string> { "Tag 1", "tag_2", "TAG3" };
			public List<string> Name = new List<string> { "Text with tag1", "#Tag1" };
			public List<string> Description = new List<string> { "Text with #tag_2,#tag3-#tag4", " and #TAG__5" };
		}



		[Fact]
		public void TestExtractHashTagsFromValue()
		{
			var tags = TagsProcessor.ExtractHashTagsFromValue(new TagsProcessorTest.Test1(), "Name", "Description");

			var exp = new[] { "tag1", "tag2", "tag3", "tag4", "tag5" };
			Assert.Equal(exp.Length, tags.Length);
			for (int i = 0; i < exp.Length; i++) Assert.Equal(exp[i], tags[i]);
		}

		[Fact]
		public void TestExtractHashTagsFromObject()
		{
			var tags = TagsProcessor.ExtractHashTagsFromValue(new TagsProcessorTest.Test2(), "Name", "Description");

			var exp = new[] { "tag1", "tag2", "tag3", "tag4", "tag5" };
			Assert.Equal(exp.Length, tags.Length);
			for (int i = 0; i < exp.Length; i++) Assert.Equal(exp[i], tags[i]);

		}

		[Fact]
		public void TestExtractHashTagsFromDictionary()
		{
			var tags = TagsProcessor.ExtractHashTagsFromValue(new TagsProcessorTest.Test3(), "Name", "Description");

			var exp = new[] { "tag1", "tag2", "tag3", "tag4", "tag5" };
			Assert.Equal(exp.Length, tags.Length);
			for (int i = 0; i < exp.Length; i++) Assert.Equal(exp[i], tags[i]);

		}

		[Fact]
		public void TestExtractHashTagsFromList()
		{
			var tags = TagsProcessor.ExtractHashTagsFromValue(new TagsProcessorTest.Test3(), "Name", "Description");

			var exp = new[] { "tag1", "tag2", "tag3", "tag4", "tag5" };
			Assert.Equal(exp.Length, tags.Length);
			for (int i = 0; i < exp.Length; i++) Assert.Equal(exp[i], tags[i]);

		}
	}
}
