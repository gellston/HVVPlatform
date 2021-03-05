using DevExpress.Compression;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VisionTool.Model;
using OpenCvSharp;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;

namespace VisionTool.Service
{
    public class DiagramPackageService
    {
        private readonly SettingConfigService settingConfigService;

        public DiagramPackageService(SettingConfigService _settingConfigService)
        {
            this.settingConfigService = _settingConfigService;
        }

        public ObservableCollection<DiagramConfig> LoadAllDiagramConfig(string _path, string _imagePath)
        {


            ObservableCollection<DiagramConfig> configCollection = new ObservableCollection<DiagramConfig>();

            try
            {
                var configs = Directory.GetFiles(_path, "*.json");
                foreach (var config in configs)
                {
                    var jsonContent = File.ReadAllText(config);
                    var module = JsonConvert.DeserializeObject<DiagramConfig>(jsonContent);

                    try{
                        Mat image = new Mat(_imagePath + module.DiagramImageName);
                        image = image.Resize(new Size(100, 100));
                        module.DiagramImage = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(image);
                        
                    }
                    catch(Exception e)
                    {
                       
                    }
                    configCollection.Add(module);

                    
                }

            }
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
            {
                return false;
            }


            return true;
        }

        public bool CreateDiagramPackage(string _diagramName,
                                        string _diagramModifyDate,
                                        string _diagramWriter,
                                        int _diagramVersion,
                                        string _diagramComment,
                                        Function _function,
                                        List<InputSnapSpot> _inputCollection,
                                        List<OutputSnapSpot> _outputCollection,
                                        string _diagramTargetPath,
                                        string _diagramImagePath,
                                        double _diagramWidth,
                                        double _diagramHeight,
                                        string _diagramTempPackagePath,
                                        string _password)
        {
            try
            {
                if (_diagramName.Length == 0) return false;
                if (_diagramModifyDate.Length == 0) return false;
                if (_diagramVersion < 1) return false;
                if (_diagramTargetPath.Length == 0) return false;
                if (_diagramTempPackagePath.Length == 0) return false;
                if (_function == null) return false;

                if (Directory.Exists(_diagramTargetPath) == false) return false;
                if (Directory.Exists(_diagramTempPackagePath) == false) return false;



                var regexItem = new Regex("^[a-zA-Z0-9 ]*$");

                if (regexItem.IsMatch(_diagramName) == false) return false;



                System.IO.DirectoryInfo di = new DirectoryInfo(_diagramTempPackagePath);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                
                try
                {
                    var imagePath = _diagramTempPackagePath + _diagramName + ".jpg";
                    Mat image = new Mat(_diagramImagePath);
                    image = image.Resize(new Size(_diagramWidth, _diagramHeight));
                    image.ImWrite(imagePath);
                }
                catch(Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
                

                //var dependentDLL = _moduleTempPackagePath + "dependent" + Path.DirectorySeparatorChar;
                //Directory.CreateDirectory(dependentDLL);
                //foreach (var file in _dependentCollection)
                //{
                //    var targetFilePath = dependentDLL + file.FileName;
                //    File.Copy(file.FilePath, targetFilePath);
                //}

                //var mainDLLPath = _moduleTempPackagePath + _moduleName + ".dll";
                //File.Copy(_moduleMainPath, mainDLLPath);



                DiagramConfig config = new DiagramConfig()
                {
                    DiagramVersion = _diagramVersion,
                    DiagramComment = _diagramComment,
                    DiagramName = _diagramName,
                    DiagramWriter = _diagramWriter,
                    DiagramModifyDate = _diagramModifyDate,
                    DiagramImageName = _diagramName + ".jpg",
                    FunctionInfo = _function,
                    InputSnapSpotCollection = _inputCollection,
                    OutputSnapSpotCollection = _outputCollection

                };

                


                var targetConfigPath = _diagramTempPackagePath + _diagramName + ".json";

                //var jsonString = JsonSerializer.Serialize(config);
                string jsonString = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(targetConfigPath, jsonString);


                var targetZipPath = _diagramTargetPath + _diagramName + ".diagram";
                var compressFiles = Directory.GetFiles(_diagramTempPackagePath);

                string zipFileName = targetZipPath;
                string sourceDir = _diagramTempPackagePath;
                string password = _password;
                EncryptionType encryptionType = EncryptionType.Aes256;
                using (ZipArchive archive = new ZipArchive())
                {
                    archive.Password = password;
                    archive.EncryptionType = encryptionType;


                    archive.AddDirectory(_diagramTempPackagePath, "/");


                    archive.Save(zipFileName);
                }


            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

    }
}
