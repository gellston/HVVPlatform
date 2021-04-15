using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using Model.DiagramProperty;

namespace VisionTool.Helper
{

    public class FileAssociation
    {
        public string Extension { get; set; }
        public string ProgId { get; set; }
        public string FileTypeDescription { get; set; }
        public string ExecutableFilePath { get; set; }
    }

    static class Extensions
    {

        // needed so that Explorer windows get refreshed after the registry is updated
        [System.Runtime.InteropServices.DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        private const int SHCNE_ASSOCCHANGED = 0x8000000;
        private const int SHCNF_FLUSH = 0x1000;

        public static List<BaseDiagramProperty> Clone(this List<BaseDiagramProperty> listToClone)
        {
            var list = new List<BaseDiagramProperty>();

            foreach (var item in listToClone)
            {
                var newItem = (BaseDiagramProperty)item.Clone();
                list.Add(newItem);

            }

            return list;
        }

        public static object DeepClone(object obj)
        {
            object objResult = null;

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }

            return objResult;
        }

        private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        public static void EnsureAssociationsSet()
        {
            var filePath = Process.GetCurrentProcess().MainModule.FileName;
            EnsureAssociationsSet(
                new FileAssociation
                {
                    Extension = ".vsjs",
                    ProgId = "HvVisionProgram",
                    FileTypeDescription = "VisionScript File",
                    ExecutableFilePath = filePath
                },
                 new FileAssociation
                 {
                     Extension = ".vsseq",
                     ProgId = "HvVisionProgram",
                     FileTypeDescription = "VisionScriptSequence File",
                     ExecutableFilePath = filePath
                 },
                 new FileAssociation
                 {
                     Extension = ".module",
                     ProgId = "HvVisionProgram",
                     FileTypeDescription = "VisionModule File",
                     ExecutableFilePath = filePath
                 },
                 new FileAssociation
                 {
                     Extension = ".diagram",
                     ProgId = "HvVisionProgram",
                     FileTypeDescription = "VisionDiagram File",
                     ExecutableFilePath = filePath
                 }
            );

        }

        public static void EnsureAssociationsSet(params FileAssociation[] associations)
        {
            bool madeChanges = false;
            foreach (var association in associations)
            {
                madeChanges |= SetAssociation(
                    association.Extension,
                    association.ProgId,
                    association.FileTypeDescription,
                    association.ExecutableFilePath);
            }

            if (madeChanges)
            {
                SHChangeNotify(SHCNE_ASSOCCHANGED, 0, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static bool SetAssociation(string extension, string progId, string fileTypeDescription, string applicationFilePath)
        {
            bool madeChanges = false;
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + extension, progId);
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + progId, fileTypeDescription);
            madeChanges |= SetKeyDefaultValue($@"Software\Classes\{progId}\shell\open\command", "\"" + applicationFilePath + "\" \"%1\"");
            return madeChanges;
        }

        private static bool SetKeyDefaultValue(string keyPath, string value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(keyPath))
            {
                if (key.GetValue(null) as string != value)
                {
                    key.SetValue(null, value);
                    return true;
                }
            }

            return false;
        }
    }
}
