using DevExpress.Compression;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Model;
using OpenCvSharp;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using VisionTool.Helper;
using System.Windows.Media;
using DevExpress.Xpf.CodeView;
using Model.DiagramProperty;
using System.Reflection;

namespace VisionTool.Service
{
    public class DiagramControlService
    {
        private readonly SettingConfigService settingConfigService;
        private Action<string> currentDiagramEditDate;

        private Action<string> currentDiagramName;
        private Action<string> currentDiagramComment;
        private Action<Color> currentDiagramColor;
        private Action<int> currentDiagramVersion;
        private Action<ImageSource> currentDiagramImage = null;
        private Action<string> currentDiagramImagePath;
        private Action<string> currentDiagramScript;
        private Action<string> currentDiagramWriter;
        private Action<string> currentDiagramCategory;

        public DiagramControlService(SettingConfigService _settingConfigService)
        {


            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };


            this.settingConfigService = _settingConfigService;




            this.UpdateDiagramInfo();
        }


        public void SetCallbackDiagramEditDate(Action<string> check)
        {
            this.currentDiagramEditDate += check;

        }

        public void SetCallbackDiagramComment(Action<string> check)
        {
            this.currentDiagramComment += check;

        }
        public void SetCallbackDiagramScript(Action<string> check)
        {
            this.currentDiagramScript += check;

        }


        public void SetCallbackDiagramVersion(Action<int> check)
        {
            this.currentDiagramVersion += check;
        }

        public void SetCallbackDiagramColor(Action<Color> check)
        {
            this.currentDiagramColor += check;
        }

        public void SetCallbackDiagramImage(Action<ImageSource> check)
        {
            this.currentDiagramImage += check;
        }
        
        public void SetCallbackDiagramName(Action<string> check)
        {
            this.currentDiagramName += check;
        }

        public void SetCallbackDiagramImagePath(Action<string> check)
        {
            this.currentDiagramImagePath += check;
        }

        public void SetCallbackDiagramWriter(Action<string> check)
        {
            this.currentDiagramWriter += check;
        }

        public void SetCallbackDiagramCategory(Action<string> check)
        {
            this.currentDiagramCategory += check;
        }

        


        private InputSnapSpot CreateInputSnapSpot(BaseDiagramProperty property)
        {
            if (property == null) return null;
            var cloneProperty = (BaseDiagramProperty)property.Clone();

            return new InputSnapSpot("", "")
            {
                DiagramProperty = cloneProperty
            };
        }

        private OutputSnapSpot CreateOutputSnapSpot()
        {
            return new OutputSnapSpot("", "");
        }

        public void ImportDiagram()
        {
            try
            {
                var path = DialogHelper.OpenFile("diagram file (.diagram)|*.diagram");
                var fileName = Path.GetFileName(path);

                var targetPath = settingConfigService.ApplicationSetting.DiagramPath + fileName;

                if (File.Exists(targetPath) == true)
                {
                    if (DialogHelper.ShowConfirmMessage("다이어그램이 존재합니다. 덮어쓰시겠습니까?") == false)
                    {
                        return;
                    }

                    
                }
                File.Copy(path, targetPath, true);
                this.UpdateDiagramInfo();

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void DeleteDiagram(DiagramConfig config)
        {
            try
            {
                if (config == null) return;
                var diagramPath = this.settingConfigService.ApplicationSetting.DiagramPath + config.DiagramName + ".diagram";
                File.Delete(diagramPath);


                
            }catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public void ImportDiagram(string path)
        {
            try
            {
                var fileName = Path.GetFileName(path);

                var targetPath = settingConfigService.ApplicationSetting.DiagramPath + fileName;

                if(File.Exists(targetPath) == true)
                {
                    if(DialogHelper.ShowConfirmMessage("다이어그램이 존재합니다. 덮어쓰시겠습니까?") == false)
                    {
                        return;
                    }

                    
                }
                File.Copy(path, targetPath, true);
                this.UpdateDiagramInfo();

            }catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }


        public void AddInputSnapSpot(BaseDiagramProperty property)
        {
            var input = this.CreateInputSnapSpot(property);
            if (input == null) return;
            this.InputSnapSpotCollection.Add(input);

        }


        public void AddOutputSnapSpot()
        {
            this.OutputSnapSpotCollection.Add(this.CreateOutputSnapSpot());
        }

        public void AddImageFromPath()
        {

            try
            {
                var path = DialogHelper.OpenFile("Image File (.jpg)|*.jpg");
                var image = Helper.ImageHelper.LoadImageFromPath(path, 100, 100);
                this.currentDiagramImage.Invoke(image);
                this.currentDiagramImagePath.Invoke(path);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void RemoveOutputSnapSpot(OutputSnapSpot output)
        {
            if (output != null)
                this.OutputSnapSpotCollection.Remove(output);
        }
        public void RemoveInputSnapSpot(InputSnapSpot input)
        {
            if (input != null)
                this.InputSnapSpotCollection.Remove(input);
        }

        public void RemovePreviewFunctionProperty(BaseDiagramProperty property)
        {
            if (property != null)
                this.PreviewFunctionPropertyCollection.Remove(property);
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

        private ObservableCollection<BaseDiagramProperty> _DiagramPropertyDataType = null;
        public ObservableCollection<BaseDiagramProperty> DiagramPropertyDataType
        {
            get
            {
                _DiagramPropertyDataType ??= new ObservableCollection<BaseDiagramProperty>();
                return _DiagramPropertyDataType;
            }
        }


        private ObservableCollection<string> _DiagramCategory = null;
        public ObservableCollection<string> DiagramCategory
        {
            get
            {
                _DiagramCategory ??= new ObservableCollection<string>();
                return _DiagramCategory;
            }
        }




        private ObservableCollection<InputSnapSpot> _InputSnapSpotCollection = null;
        public ObservableCollection<InputSnapSpot> InputSnapSpotCollection
        {
            get
            {
                _InputSnapSpotCollection ??= new ObservableCollection<InputSnapSpot>();
                return _InputSnapSpotCollection;
            }
        }

        private ObservableCollection<OutputSnapSpot> _OutputSnapSpotCollection = null;
        public ObservableCollection<OutputSnapSpot> OutputSnapSpotCollection
        {
            get
            {
                _OutputSnapSpotCollection ??= new ObservableCollection<OutputSnapSpot>();
                return _OutputSnapSpotCollection;
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

        private ObservableCollection<BaseDiagramProperty> _PreviewFunctionPropertyCollection = null;
        public ObservableCollection<BaseDiagramProperty> PreviewFunctionPropertyCollection
        {
            get
            {
                _PreviewFunctionPropertyCollection ??= new ObservableCollection<BaseDiagramProperty>();
                return _PreviewFunctionPropertyCollection;
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
            this.PreviewFunctionPropertyCollection.Clear();



            this.currentDiagramColor.Invoke(Color.FromRgb(255, 255, 255));
            this.currentDiagramComment.Invoke("");
            this.currentDiagramVersion.Invoke(1);
            this.currentDiagramEditDate.Invoke("");
            this.currentDiagramName.Invoke("");
            this.currentDiagramImage.Invoke(null);


            this.InputSnapSpotCollection.Clear();
            this.OutputSnapSpotCollection.Clear();

        }

        public void LoadFunctionInfoFromConfig(DiagramConfig config)
        {

            this.currentDiagramComment.Invoke(config.DiagramComment);
            this.currentDiagramImage.Invoke(config.DiagramImage);
            this.currentDiagramEditDate.Invoke(config.DiagramModifyDate);
            this.currentDiagramImagePath.Invoke(this.settingConfigService.ApplicationSetting.DiagramImagePath +
                                                config.DiagramImageName);

            this.currentDiagramName.Invoke(config.DiagramName);
            this.currentDiagramScript.Invoke(config.DiagramScript);
            this.currentDiagramWriter.Invoke(config.DiagramWriter);
            this.currentDiagramVersion.Invoke(config.DiagramVersion);
            this.currentDiagramColor.Invoke(config.FunctionInfo.Color);
            this.currentDiagramCategory.Invoke(config.DiagramCategory);
            this.InputSnapSpotCollection.Clear();
            this.OutputSnapSpotCollection.Clear();


            this.InputSnapSpotCollection.AddRange(config.InputSnapSpotCollection);
            this.OutputSnapSpotCollection.AddRange(config.OutputSnapSpotCollection);

        }




        public void RenderFunction(double _canvasWidth,
                                   string _diagramName,
                                   Color _diagramColor)
        {



            if ((this.InputSnapSpotCollection.Count() + this.OutputSnapSpotCollection.Count() + PreviewFunctionPropertyCollection.Count()) == 0)
                throw new Exception("Node and property is not exists");

            //if (_outputSnapSpot.Count() == 0)
            //    throw new Exception("Output node is not exists");


            if (this.InputSnapSpotCollection.ToList().Exists(x => x.Name.Length == 0 || x.DataType.Length == 0))
                throw new Exception("Empty input node exists");

            if (this.OutputSnapSpotCollection.ToList().Exists(x => x.Name.Length == 0 || x.DataType.Length == 0))
                throw new Exception("Empty output node exists");


            //if (this.DiagramConfigCollection.ToList().Exists(x => x.DiagramName == _diagramName))
            //    throw new Exception("Diagram name is already exists");

            if (_diagramName.Length == 0)
                throw new Exception("Name is not exists");



            var inputNodeNames = this.InputSnapSpotCollection.Select(x => x.Name).ToList();
            var distinctInputNodeNames = inputNodeNames.Distinct().ToList();


            var outputNodeNames = this.OutputSnapSpotCollection.Select(x => x.Name).ToList();
            var distinctOutputNodeNames = outputNodeNames.Distinct().ToList();


            //var functionProperties = this.PreviewFunctionPropertyCollection.Select(x => x.Name).ToList();
            //var distinctFunctionProperties = functionProperties.Distinct().ToList();

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


            /*
            bool duplicatesFunctionPropertiesCheck = false;
            distinctFunctionProperties.ForEach(distinctFunctionName =>
            {
                if (functionProperties.Where(outputNodeName => outputNodeName == distinctFunctionName).Count() > 1)
                    duplicatesOutputCheck = true;
            });
            if (duplicatesFunctionPropertiesCheck == true)
            {
                throw new Exception("Duplicated function property exists");
            }*/




            var function = new Function()
            {
                Name = _diagramName,
                IsNew = false
            };

            function.Location.X = 100;
            function.Location.Y = 100;
            function.Size.Width = 0;
            function.Size.Height = 0;

            int count = 0;
            double lastOffsetY = 0;
            foreach(var inputSnapSpot in this.InputSnapSpotCollection)
            {
                count++;
                inputSnapSpot.Parent = function;
                inputSnapSpot.Offset.X = 15;
                lastOffsetY = inputSnapSpot.Offset.Y = 25 * count;
                function.Input.Add(inputSnapSpot);
            }
            lastOffsetY += 10;
            count = 0;
            foreach(var outputSnapSpot in this.OutputSnapSpotCollection)
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


            this.PreviewInputSnapSpotCollection.AddRange(this.InputSnapSpotCollection);
            this.PreviewOutputSnapSpotCollection.AddRange(this.OutputSnapSpotCollection);

            function.Location.X = _canvasWidth * 0.3;
            function.Location.Y = 10;
            function.Size.Width = _canvasWidth * 0.4;

            var functionHeight = 30 + (function.Input.Count() + function.Output.Count()) * 30;
            function.Size.Height = functionHeight;


            this.PreviewFunctionCollection.Add(function);

        }


        public void UpdateDiagramInfo()
        {
            this.DiagramConfigCollection.Clear();
            this.DiagramCollection.Clear();

            this.DiagramDataType.Clear();
            this.DiagramCategory.Clear();
            this.DiagramDataType.AddRange(this.settingConfigService.ApplicationSetting.DiagramDataTypeCollection);
            this.DiagramCategory.AddRange(this.settingConfigService.ApplicationSetting.DiagramCategoryCollection);

            this.DiagramPropertyDataType.Clear();
            //this.DiagramPropertyDataType.Add(new EmptyDiagramProperty());
            //this.DiagramPropertyDataType.Add(new BoolDiagramProperty());
            //this.DiagramPropertyDataType.Add(new ThresholdDiagramProperty());


            foreach(var propertyName in this.settingConfigService.ApplicationSetting.DiagramPropertyDataTypeCollection)
            {
                try
                {

                    var assembly = Assembly.GetExecutingAssembly();

                    Type propertyType = Type.GetType("Model.DiagramProperty." + propertyName + ", Model");
                    var property = Activator.CreateInstance(propertyType);
                    this.DiagramPropertyDataType.Add((BaseDiagramProperty)property);
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

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
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

            var configs = Directory.GetFiles(this.settingConfigService.ApplicationSetting.DiagramConfigPath, "*.json");
            foreach (var config in configs)
            {
                try
                {
                    var jsonContent = File.ReadAllText(config, Encoding.UTF8);
                    var module = JsonConvert.DeserializeObject<DiagramConfig>(jsonContent);


                    try
                    {
                        Mat image = new Mat(this.settingConfigService.ApplicationSetting.DiagramImagePath + module.DiagramImageName);
                        image = image.Resize(new Size(100, 100));
                        module.DiagramImage = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(image);
                    }
                    catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        module.DiagramImage = WpfSvgRenderer.CreateImageSource(SvgImageHelper.CreateImage(new Uri("pack://application:,,,/DevExpress.Images.v20.2;component/SvgImages/Dashboards/ShowWeightedLegendNone.svg")), 1d, null, null, true);
                    }

                    this.DiagramConfigCollection.Add(module);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }

        public bool CheckDiagramExists(string _diagramName)
        {
            if (this.DiagramConfigCollection.ToList().Exists(x => x.DiagramName == _diagramName) == true)
                return true;
            else return false;
        }

        public void CreateDiagramPackage(string _diagramName,
                                         string _diagramWriter,
                                         int _diagramVersion,
                                         string _diagramComment,
                                         string _diagramScript,
                                         string _diagramCategory,
                                         string _diagramImagePath,
                                         double _diagramWidth,
                                         double _diagramHeight,
                                         bool _isOverwrite = false)
        {
            try
            {
                if (_diagramName.Length == 0) throw new Exception("Diagram is not correct");
                if (_diagramVersion < 1) throw new Exception("Diagram version is not correct");
                if (this.PreviewFunctionCollection.Count == 0) throw new Exception("Function info is not correct");
                if (_diagramCategory == null) throw new Exception("Diagram category is not set");
                if (_diagramCategory.Length == 0) throw new Exception("Diagram category is not set");

                var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
                if (regexItem.IsMatch(_diagramName) == false) new Exception("It contains special characters in diagram name");

                if (this.DiagramConfigCollection.ToList().Exists(x => x.DiagramName == _diagramName) && _isOverwrite == false)
                    throw new Exception("Diagram name is already exists");


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
                    System.Diagnostics.Debug.WriteLine(e.Message);
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
                    DiagramCategory = _diagramCategory,
                    DiagramImageName = _diagramName + ".jpg",
                    FunctionInfo = this.PreviewFunctionCollection[0],
                    InputSnapSpotCollection = this.PreviewInputSnapSpotCollection.ToList(),
                    OutputSnapSpotCollection = this.PreviewOutputSnapSpotCollection.ToList(),
                    DiagramScript = _diagramScript

                };

                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                var targetConfigPath = this.settingConfigService.TempDiagramPackagePath + _diagramName + ".json";
                string jsonString = JsonConvert.SerializeObject(config, Formatting.Indented);
                 File.WriteAllText(targetConfigPath, jsonString, Encoding.UTF8);


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
