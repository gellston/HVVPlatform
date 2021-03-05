using DevExpress.Xpf.CodeView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using VisionTool.Model;

namespace VisionTool.Service
{
    public class ScriptControlService
    {
        private readonly SettingConfigService settingConfigService;
        private readonly HV.V1.Interpreter interpreter;
        private Action<bool> isRunningAction;
        private Action<string> currentFPSAction;
        private Action<string> currentExecutiontimeAction;

        public ScriptControlService(SettingConfigService _settingConfigService)
        {
            this.settingConfigService = _settingConfigService;

            HV.V1.Interpreter.InitV8StartupData(this.settingConfigService.CurrentApplicationPath);
            HV.V1.Interpreter.InitV8Platform();
            if (HV.V1.Interpreter.InitV8Engine() == false)
            {
                throw new Exception("V8 engine init failed");
            }
            HV.V1.Interpreter.SetV8Flag("--use_strict");
            HV.V1.Interpreter.SetV8Flag("--max_old_space_size=8192");
            HV.V1.Interpreter.SetV8Flag("--expose_gc");

            this.interpreter = new HV.V1.Interpreter();
            this.interpreter.TraceEvent += Trace;
        }

        private void Trace(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.ScriptLogCollection.Insert(0, new Log()
                {
                    Type = "스크립트",
                    Content = text
                });
            });
            Thread.Sleep(1);
        }

        public void SaveScript(Script context)
        {
            try
            {
                string filePath = DialogHelper.SaveFile("Script File (.js)|*.js");
                context.FilePath = filePath;
                context.FileName = Path.GetFileName(filePath);
                File.WriteAllText(filePath, context.ScriptContent, Encoding.UTF8);

            }catch(Exception e)
            {

            }
        }

        public ObservableCollection<Script> GetScriptsFromFolder()
        {
            ObservableCollection<Script> collection = new ObservableCollection<Script>();

            try
            {
                var path = DialogHelper.OpenFolder();
                var files = Directory.GetFiles(path, "*.js");
                foreach (var file in files)
                {
                    collection.Add(new Script()
                    {
                        FileName = Path.GetFileName(file),
                        ScriptContent = File.ReadAllText(file, Encoding.UTF8),
                        FilePath = file
                    });
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            return collection;
        }

        public Script GetScriptFromPath()
        {
            Script script = new Script();
            try
            {
                var path = DialogHelper.OpenFile("Script File (.js)|*.js");
                var context = File.ReadAllText(path, Encoding.UTF8);
                script.ScriptContent = context;
                script.FileName = Path.GetFileName(path);
                script.FilePath = path;

            }catch(Exception e)
            {

            }

            return script;
        }
        
        public Script CreateNewScript()
        {
            return new Script()
            {
                FileName = "new.js",
                ScriptContent = "/* Be the god of coding */",
                FilePath = ""
            };
        }

        public bool ContinuousRunScript(string context)
        {
            if (this.IsRunningScript == true) return false;
            if (context == null) return false;

            this.IsRunningScript = true;

            this.interpreter.SetModulePath(this.settingConfigService.ApplicationSetting.ModuleMainPath);

            Task.Run(async () =>
            {
                var currentFps = 0;
                var stacked_time = 0.0;

                while (IsRunningScript)
                {
                    await Task.Delay(10);
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    try
                    {

                        this.interpreter.RunScript(context);

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.GlobalCollection.Clear();
                            this.GlobalCollection.AddRange(this.interpreter.GlobalObjects.Values.ToList());

                            this.NativeModuleCollection.Clear();
                            this.NativeModuleCollection.AddRange(this.interpreter.NativeModules.Values.ToList());

                            //if (this._isTracking == false) return;
                            //try
                            //{
                            //    var image = this.interpreter.GlobalObjects.Values.ToList().Where((_object) =>
                            //    {
                            //        return _object.Name.Contains(this._trackingName) && _object.Type.Contains(this._trackingType);
                            //    }).First();

                            //    var hvImage = new HV.V1.Image(image);
                            //    var width = hvImage.Width;
                            //    var height = hvImage.Height;
                            //    var stride = hvImage.Stride;
                            //    var size = hvImage.Size;
                            //    if (TrackingImagePresenter == null || TrackingImagePresenter.Width != width || TrackingImagePresenter.Height != height)
                            //    {
                            //        TrackingImagePresenter = new WriteableBitmap(width, height, 96, 96, PixelFormats.Gray8, null);
                            //        TrackingImagePresenter.Freeze();
                            //    }
                            //    TrackingImagePresenter.WritePixels(new System.Windows.Int32Rect(0, 0, width, height), hvImage.Ptr(), size, stride);
                            //}
                            //catch (Exception e)
                            //{

                            //}
                        }, DispatcherPriority.Send);
                    }
                    catch (HV.V1.ScriptError e)
                    {

                        System.Console.WriteLine(e.Message);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            string errorContent = string.Format("Error Line:{0}, Column({1},{2})\n{3}", e.Line(), e.StartColumn(), e.EndColumn(), e.Message);
                            this.ScriptLogCollection.Add(new Log()
                            {
                                Type = "Error",
                                Content = errorContent
                            });
                            DialogHelper.ShowToastErrorMessage("스크립트 에러 메세지", errorContent);
                        }, DispatcherPriority.Send);

                        break;
                    }

                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    stacked_time += elapsedMs;
                    currentFps++;
                    if (stacked_time > 1000)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.currentFPSAction.Invoke(currentFps.ToString("F2"));
                            this.currentExecutiontimeAction.Invoke(elapsedMs.ToString() + " ms");
                        }, DispatcherPriority.Send);
                        currentFps = 0;
                        stacked_time = 0;
                    }
                }

                this.IsRunningScript = false;
            });
            return true;
        }

        public bool RunScript(string context)
        {
            if (this.IsRunningScript == true) return false;
            if (context == null) return false;

            this.IsRunningScript = true;

            Task.Run(() =>
            {
                try
                {
                    this.interpreter.RunScript(context);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        this.GlobalCollection.Clear();
                        this.GlobalCollection.AddRange(this.interpreter.GlobalObjects.Values);

                        this.NativeModuleCollection.Clear();
                        this.NativeModuleCollection.AddRange(this.interpreter.NativeModules.Values);

                        if (this.IsRunningScript == false) return;
                        try
                        {
                            //var image = this.interpreter.GlobalObjects.Values.ToList().Where((_object) =>
                            //{
                            //    return _object.Name.Contains(this._trackingName) && _object.Type.Contains(this._trackingType);
                            //}).First();

                            //var hvImage = new HV.V1.Image(image);
                            //var width = hvImage.Width;
                            //var height = hvImage.Height;
                            //var stride = hvImage.Stride;
                            //var size = hvImage.Size;
                            //if (TrackingImagePresenter == null || TrackingImagePresenter.Width != width || TrackingImagePresenter.Height != height)
                            //{
                            //    TrackingImagePresenter = new WriteableBitmap(width, height, 96, 96, PixelFormats.Gray8, null);
                            //}
                            //TrackingImagePresenter.WritePixels(new System.Windows.Int32Rect(0, 0, width, height), hvImage.Ptr(), size, stride);
                        }
                        catch (Exception e)
                        {

                        }
                    }, DispatcherPriority.Send);

                }
                catch(HV.V1.ScriptError e)
                {

                }

                this.IsRunningScript = false;
            });

            

            return true;
        }

        private ObservableCollection<Log> _ScriptLogCollection = null;
        public ObservableCollection<Log> ScriptLogCollection
        {
            get
            {
                _ScriptLogCollection ??= new ObservableCollection<Log>();
                return _ScriptLogCollection;
            }
        }

        private ObservableCollection<HV.V1.NativeModule> _NativeModuleCollection = null;
        public ObservableCollection<HV.V1.NativeModule> NativeModuleCollection
        {
            get
            {
                _NativeModuleCollection ??= new ObservableCollection<HV.V1.NativeModule>();
                return _NativeModuleCollection;
            }
        }

        private ObservableCollection<HV.V1.Object> _GlobalCollection = null;
        public ObservableCollection<HV.V1.Object> GlobalCollection
        {
            get
            {
                if (_GlobalCollection == null)
                {
                    _GlobalCollection = new ObservableCollection<HV.V1.Object>();
                }
                return _GlobalCollection;
            }
        }

        public void SetCheckRunning(Action<bool> check)
        {
            this.isRunningAction += check;

        }

        public void SetCheckCurrentExecutionTime(Action<string> check)
        {
            this.currentExecutiontimeAction += check;
        }

        public void SetCheckCurrentFPS(Action<string> check)
        {
            this.currentFPSAction += check;
        }


        private bool _IsRunningScript = false;
        public bool IsRunningScript {
            get => _IsRunningScript;
            set
            {
                
                this._IsRunningScript = value;
                this.isRunningAction.Invoke(value);
            }
        }

        public void StopScriptRunning()
        {
            this.interpreter.Terminate();
            this.IsRunningScript = false;
        }
    }
}
