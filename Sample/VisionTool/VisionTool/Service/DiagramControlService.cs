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
using VisionTool.Helper;
using System.Windows.Media;
using DevExpress.Xpf.CodeView;

namespace VisionTool.Service
{
    public class DiagramControlService
    {
        private readonly SettingConfigService settingConfigService;
        private Action<string> currentDiagramEditDate;

        public DiagramControlService(SettingConfigService _settingConfigService)
        {
            this.settingConfigService = _settingConfigService;


            this.DiagramDataType.Add("image");
            this.UpdateDiagramInfo();
        }


        public void SetCheckDiagramEditDate(Action<string> check)
        {
            this.currentDiagramEditDate += check;

        }




        public InputSnapSpot CreateInputSnapSpot()
        {
            return new InputSnapSpot("", "");
        }

        public OutputSnapSpot CreateOutputSnapSpot()
        {
            return new OutputSnapSpot("", "");
        }




        private ObservableCollection<DiagramConfig> _DiagramConfigCollection = null;
        public ObservableCollection<DiagramConfig> DiagramConfigCollection
        {
            get
            {
                _DiagramConfigCollection ??= new ObservableCollection<DiagramConfig>();
                return _DiagramConfigCollection;
            }
        }

        private ObservableCollection<Diagram> _DiagramCollection = null;
        public ObservableCollection<Diagram> DiagramCollection
        {
            get
            {
                _DiagramCollection ??= new ObservableCollection<Diagram>();
                return _DiagramCollection;
            }
        }

        private ObservableCollection<string> _DiagramDataType = null;
        public ObservableCollection<string> DiagramDataType
        {
            get
            {
                _DiagramDataType ??= new ObservableCollection<string>();
                return _DiagramDataType;
            }
        }



        private ObservableCollection<InputSnapSpot> _PreviewInputSnapSpotCollection = null;
        public ObservableCollection<InputSnapSpot> PreviewInputSnapSpotCollection
        {
            get
            {
                _PreviewInputSnapSpotCollection ??= new ObservableCollection<InputSnapSpot>();
                return _PreviewInputSnapSpotCollection;
            }
        }

        private ObservableCollection<OutputSnapSpot> _PreviewOutputSnapSpotCollection = null;
        public ObservableCollection<OutputSnapSpot> PreviewOutputSnapSpotCollection
        {
            get
            {
                _PreviewOutputSnapSpotCollection ??= new ObservableCollection<OutputSnapSpot>();
                return _PreviewOutputSnapSpotCollection;
            }
        }


        private ObservableCollection<Function> _PreviewFunctionCollection = null;
        public ObservableCollection<Function> PreviewFunctionCollection
        {
            get
            {
                _PreviewFunctionCollection ??= new ObservableCollection<Function>();
                return _PreviewFunctionCollection;
            }
        }



        public void ClearPreviewDiagram()
        {
            this.PreviewFunctionCollection.Clear();
            this.PreviewInputSnapSpotCollection.Clear();
            this.PreviewOutputSnapSpotCollection.Clear();
        }


        public void RenderFunction(ObservableCollection<InputSnapSpot> _inputSnapSpot,
                                   ObservableCollection<OutputSnapSpot> _outputSnapSpot,
                                   string _diagramName,
                                   double _diagramWidth,
                                   double _diagramHeight,
                                   Color _diagramColor)
        {



            if (_inputSnapSpot.Count() == 0)
                throw new Exception("Input node is not exists");

            if (_outputSnapSpot.Count() == 0)
                throw new Exception("Output node is not exists");


            if (_inputSnapSpot.ToList().Exists(x => x.Name.Length == 0 || x.DataType.Length == 0))
                throw new Exception("Empty input node exists");

            if (_outputSnapSpot.ToList().Exists(x => x.Name.Length == 0 || x.DataType.Length == 0))
                throw new Exception("Empty output node exists");


            if (this.DiagramConfigCollection.ToList().Exists(x => x.DiagramName == _diagramName))
                throw new Exception("Diagram name is already exists");



            var inputNodeNames = _inputSnapSpot.Select(x => x.Name).ToList();
            var distinctInputNodeNames = inputNodeNames.Distinct().ToList();


            var outputNodeNames = _outputSnapSpot.Select(x => x.Name).ToList();
            var distinctOutputNodeNames = outputNodeNames.Distinct().ToList();

            bool duplicatesInputCheck = false;
            distinctInputNodeNames.ForEach(distinctInputName =>
            {
                if (inputNodeNames.Where(inputNodeName => inputNodeName == distinctInputName).Count() > 1)
                    duplicatesInputCheck = true;
            });
            if(duplicatesInputCheck == true)
            {
                throw new Exception("Duplicated input node exists");
            }


            bool duplicatesOutputCheck = false;
            distinctOutputNodeNames.ForEach(distinctOutputName =>
            {
                if (outputNodeNames.Where(outputNodeName => outputNodeName == distinctOutputName).Count() > 1)
                    duplicatesOutputCheck = true;
            });
            if (duplicatesOutputCheck == true)
            {
                throw new Exception("Duplicated output node exists");
            }




            var function = new Function()
            {
                Name = _diagramName,
                IsNew = false
            };

            function.Location.X = 100;
            function.Location.Y = 100;
            function.Size.Width = _diagramWidth;
            function.Size.Height = _diagramHeight;

            int count = 0;
            double lastOffsetY = 0;
            foreach(var inputSnapSpot in _inputSnapSpot)
            {
                count++;
                inputSnapSpot.Parent = function;
                inputSnapSpot.Offset.X = 15;
                lastOffsetY = inputSnapSpot.Offset.Y = 25 * count;
                function.Input.Add(inputSnapSpot);
            }
            lastOffsetY += 10;
            count = 0;
            foreach(var outputSnapSpot in _outputSnapSpot)
            {
                count++;
                outputSnapSpot.Parent = function;
                outputSnapSpot.Offset.X = 15;
                outputSnapSpot.Offset.Y = lastOffsetY + 25 * count;
                function.Output.Add(outputSnapSpot);
            }

            function.Location.ValueChanged();
            function.Color = _diagramColor;


            this.PreviewInputSnapSpotCollection.Clear();
            this.PreviewOutputSnapSpotCollection.Clear();
            this.PreviewFunctionCollection.Clear();


            this.PreviewInputSnapSpotCollection.AddRange(_inputSnapSpot);
            this.PreviewOutputSnapSpotCollection.AddRange(_outputSnapSpot);

            
            this.PreviewFunctionCollection.Add(function);

        }

        public void UpdateDiagramInfo()
        {
            this.DiagramConfigCollection.Clear();
            this.DiagramCollection.Clear();

            FileSystemHelper.DeleteFiles(this.settingConfigService.ApplicationSetting.DiagramConfigPath);

            var diagrams = FileSystemHelper.GetFiles(this.settingConfigService.ApplicationSetting.DiagramPath, "*.diagram");
            foreach(var diagram in diagrams)
            {
                try
                {
                    FileSystemHelper.DeleteFiles(this.settingConfigService.TempDiagramPackagePath);
                    FileSystemHelper.UnZipFile(diagram, this.settingConfigService.TempDiagramPackagePath, this.settingConfigService.SecurityPassword);
                    FileSystemHelper.CopyFiles(this.settingConfigService.TempDiagramPackagePath, this.settingConfigService.ApplicationSetting.DiagramConfigPath, "*.json");
                    FileSystemHelper.CopyFiles(this.settingConfigService.TempDiagramPackagePath, this.settingConfigService.ApplicationSetting.DiagramImagePath, "*.jpg");

                    this.DiagramCollection.Add(new Diagram(diagram, Path.GetFileName(diagram)));

                }
                catch(Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }

            var configs = Directory.GetFiles(this.settingConfigService.ApplicationSetting.DiagramConfigPath, "*.json");
            foreach (var config in configs)
            {
                try
                {
                    var jsonContent = File.ReadAllText(config);
                    var module = JsonConvert.DeserializeObject<DiagramConfig>(jsonContent);


                    try
                    {
                        Mat image = new Mat(this.settingConfigService.ApplicationSetting.DiagramImagePath + module.DiagramImageName);
                        image = image.Resize(new Size(100, 100));
                        module.DiagramImage = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(image);
                    }
                    catch(Exception e)
                    {
                        System.Console.WriteLine(e.Message);
                        module.DiagramImage = WpfSvgRenderer.CreateImageSource(SvgImageHelper.CreateImage(new Uri("pack://application:,,,/DevExpress.Images.v20.2;component/SvgImages/Dashboards/ShowWeightedLegendNone.svg")), 1d, null, null, true);
                    }



                    this.DiagramConfigCollection.Add(module);
                   
                    



                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }
            }
        }



        public void CreateDiagramPackage(string _diagramName,
                                        string _diagramWriter,
                                        int _diagramVersion,
                                        string _diagramComment,
                                        Function _function,
                                        ObservableCollection<InputSnapSpot> _inputCollection,
                                        ObservableCollection<OutputSnapSpot> _outputCollection,
                                        string _diagramImagePath,
                                        double _diagramWidth,
                                        double _diagramHeight)
        {
            try
            {
                if (_diagramName.Length == 0) throw new Exception("Diagram is not correct");
                //if (_diagramModifyDate.Length == 0) throw new Exception("Diagram modification date is not correct");
                if (_diagramVersion < 1) throw new Exception("Diagram version is not correct");
                if (_function == null) throw new Exception("Function info is not correct");


                var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
                if (regexItem.IsMatch(_diagramName) == false) new Exception("It contains special characters in diagram name");


                FileSystemHelper.DeleteFiles(this.settingConfigService.TempDiagramPackagePath);

                
                try
                {
                    var imagePath = this.settingConfigService.TempDiagramPackagePath + _diagramName + ".jpg";
                    Mat image = new Mat(_diagramImagePath);
                    image = image.Resize(new Size(_diagramWidth, _diagramHeight));
                    image.ImWrite(imagePath);
                }
                catch(Exception e)
                {
                    System.Console.WriteLine(e.Message);
                }

                var currentDate = DateTime.Now.ToString("yyyy-MM-HH hh:mm:ss");
                currentDiagramEditDate.Invoke(currentDate);

                DiagramConfig config = new DiagramConfig()
                {
                    DiagramVersion = _diagramVersion,
                    DiagramComment = _diagramComment,
                    DiagramName = _diagramName,
                    DiagramWriter = _diagramWriter,
                    DiagramModifyDate = currentDate,
                    DiagramImageName = _diagramName + ".jpg",
                    FunctionInfo = _function,
                    InputSnapSpotCollection = _inputCollection.ToList(),
                    OutputSnapSpotCollection = _outputCollection.ToList()

                };

                


                var targetConfigPath = this.settingConfigService.TempDiagramPackagePath + _diagramName + ".json";
                string jsonString = JsonConvert.SerializeObject(config, Formatting.Indented);
                 File.WriteAllText(targetConfigPath, jsonString);


                var targetZipPath = this.settingConfigService.ApplicationSetting.DiagramPath + _diagramName + ".diagram";

                EncryptionType encryptionType = EncryptionType.Aes256;
                using (ZipArchive archive = new ZipArchive())
                {
                    archive.Password = this.settingConfigService.SecurityPassword;
                    archive.EncryptionType = encryptionType;
                    archive.AddDirectory(this.settingConfigService.TempDiagramPackagePath, "/");
                    archive.Save(targetZipPath);
                }


            }
            catch (Exception e)
            {
                throw e;
            }
          
        }

    }
}
