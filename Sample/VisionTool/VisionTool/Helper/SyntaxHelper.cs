using ActiproSoftware.Text;
using ActiproSoftware.Text.Implementation;
using ActiproSoftware.Windows.Controls.SyntaxEditor.IntelliPrompt;
using ActiproSoftware.Windows.Controls.SyntaxEditor.IntelliPrompt.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace VisionTool.Helper
{
    public class SyntaxHelper
    {
		public const string DefinitionPath = "VisionTool.Language.";
		public const string SnippetsPath = "VisionTool.Snippets.";

		public static ISyntaxLanguage LoadLanguageDefinitionFromResourceStream(string filename)
		{
			string path = DefinitionPath + filename;
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
			{
				if (stream != null)
				{
					SyntaxLanguageDefinitionSerializer serializer = new SyntaxLanguageDefinitionSerializer();
					return serializer.LoadFromStream(stream);
				}
				else
					return SyntaxLanguage.PlainText;
			}
		}

		public static ICodeSnippetFolder LoadSampleJavascriptCodeSnippetsFromResources()
		{
            // NOTE: If you have file system access, the static CodeSnippetFolder.LoadFrom(path) method easily
            //       loads snippets within a specified file path and should be used instead

            string[] rootPaths = new string[] {
                SnippetsPath + "JavascriptFor.snippet",
                SnippetsPath + "JavascriptWhile.snippet",
            };
 
			ICodeSnippetFolder rootFolder = LoadCodeSnippetFolderFromResources("Root", rootPaths);
			return rootFolder;
		}


		private static ICodeSnippetFolder LoadCodeSnippetFolderFromResources(string folderName, string[] paths)
		{
			ICodeSnippetFolder folder = new CodeSnippetFolder(folderName);
			CodeSnippetSerializer serializer = new CodeSnippetSerializer();

			foreach (string path in paths)
			{
				using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path))
				{
					if (stream != null)
					{
						IEnumerable<ICodeSnippet> snippets = serializer.LoadFromStream(stream);
						if (snippets != null)
						{
							foreach (ICodeSnippet snippet in snippets)
								folder.Items.Add(snippet);
						}
					}
				}
			}

			return folder;
		}
	}
}
