using DevExpress.Compression;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VisionTool.Helper
{
    public class FileSystemHelper
    {

        public static void CopyFiles(string _sourcePath, string _targetPath, string pattern)
        {
            try
            {
                var files = Directory.GetFiles(_sourcePath, pattern);
                foreach (var file in files)
                {
                    File.Copy(file, _targetPath + Path.GetFileName(file), true);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        

        public static void CopyFile(string _source, string _path)
        {

            try
            {
                File.Copy(_source, _path, true);
            }
            catch(Exception e)
            {
                throw e;
            }
            
        }


        public static string[] GetFiles(string _path, string _pattern)
        {
            try
            {
                return Directory.GetFiles(_path, _pattern);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static void DeleteFiles(string _path)
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(_path);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static void UnZipFile(string _filePath, string _targetPath, string _password)
        {

            try
            {
                using (ZipArchive archive = ZipArchive.Read(_filePath))
                {
                    archive.Password = _password;
                    foreach (ZipItem item in archive)
                    {
                        item.Extract(_targetPath);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
