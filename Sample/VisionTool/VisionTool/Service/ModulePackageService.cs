
using DevExpress.Compression;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using VisionTool.Model;


namespace VisionTool.Service
{
    public class ModulePackageService
    {

        public ModulePackageService()
        {

        }


        public ObservableCollection<ModuleConfig> LoadAllModuleConfig(string _path)
        {


            ObservableCollection<ModuleConfig> configCollection = new ObservableCollection<ModuleConfig>();

            try
            {
                var configs = Directory.GetFiles(_path, "*.json");
                foreach (var config in configs)
                {
                    var jsonContent = File.ReadAllText(config);
                    var module = JsonSerializer.Deserialize<ModuleConfig>(jsonContent);
                    configCollection.Add(module);
                }

            }
            catch(Exception e)
            {
                throw e;
            }


            return configCollection;
        }


        public bool DeleteAllFiles(string _moduleTempPackagePath)
        {
            if (_moduleTempPackagePath.Length == 0) return false;
            if (Directory.Exists(_moduleTempPackagePath) == false) return false;

            System.IO.DirectoryInfo di = new DirectoryInfo(_moduleTempPackagePath);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            return true;
        }

        public bool CopyAllFiles(string _source, string _target)
        {

            try
            {
          
                if (!Directory.Exists(_target))
                {
                    Directory.CreateDirectory(_target);
                }
                foreach (var srcPath in Directory.GetFiles(_source))
                {
                    //Copy the file from sourcepath and place into mentioned target path, 
                    //Overwrite the file if same file is exist in target path
                    File.Copy(srcPath, srcPath.Replace(_source, _target), true);
                }

            }
            catch(Exception e)
            {
                return false;
            }

            return true;
        }


        public bool UnzipModule(string _sourceModule, string targetPath, string password)
        {
            if (File.Exists(_sourceModule) == false) return false;
            if (targetPath.Length == 0) return false;
            if (Directory.Exists(targetPath) == false) return false;

            string pathToZipArchive = _sourceModule;
            string pathToExtract = targetPath;

            try
            {
                using (ZipArchive archive = ZipArchive.Read(pathToZipArchive))
                {
                    archive.Password = password;
                    foreach (ZipItem item in archive)
                    {
                        item.Extract(pathToExtract);
                    }
                }
            }
            catch(Exception e)
            {
                return false;
            }
            

            return true;
        }

        public bool CreateModulePackage(string _moduleName, 
                                        string _moduleModifyDate, 
                                        string _moduleMainPath, 
                                        int _moduleVersion,
                                        string _moduleComment, 
                                        ObservableCollection<DependentDLL> _dependentCollection, 
                                        string _moduleTargetPath,
                                        string _moduleTempPackagePath,
                                        string _password)
        {
            try
            {
                if (_moduleName.Length == 0) return false;
                if (_moduleModifyDate.Length == 0) return false;
                if (_moduleVersion < 1) return false;
                if (_moduleTargetPath.Length == 0) return false;
                if (_moduleTempPackagePath.Length == 0) return false;
                if (_moduleMainPath.Length == 0) return false;

                if (Directory.Exists(_moduleTargetPath) == false) return false;
                if (Directory.Exists(_moduleTempPackagePath) == false) return false;



                var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

                if (regexItem.IsMatch(_moduleName) == false) return false;



                System.IO.DirectoryInfo di = new DirectoryInfo(_moduleTempPackagePath);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }


                var dependentDLL = _moduleTempPackagePath + "dependent" + Path.DirectorySeparatorChar;
                Directory.CreateDirectory(dependentDLL);
                foreach (var file in _dependentCollection)
                {
                    var targetFilePath = dependentDLL + file.FileName;
                    File.Copy(file.FilePath, targetFilePath);
                }

                var mainDLLPath = _moduleTempPackagePath + _moduleName + ".dll";
                File.Copy(_moduleMainPath, mainDLLPath);


                ModuleConfig config = new ModuleConfig()
                {
                    ModuleComment = _moduleComment,
                    ModuleName = _moduleName,
                    ModuleVersion = _moduleVersion,
                    ModuleModifyDate = _moduleModifyDate
                };


                var targetConfigPath = _moduleTempPackagePath + _moduleName + ".json";

                var jsonString = JsonSerializer.Serialize(config);
                File.WriteAllText(targetConfigPath, jsonString);


                var targetZipPath = _moduleTargetPath + _moduleName + ".module";
                var compressFiles = Directory.GetFiles(_moduleTempPackagePath);

                string zipFileName = targetZipPath;
                string sourceDir = _moduleTempPackagePath;
                string password = _password;
                EncryptionType encryptionType = EncryptionType.Aes256;
                using (ZipArchive archive = new ZipArchive())
                {
                    archive.Password = password;
                    archive.EncryptionType = encryptionType;


                    archive.AddDirectory(_moduleTempPackagePath, "/");


                    archive.Save(zipFileName);
                }

                
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
