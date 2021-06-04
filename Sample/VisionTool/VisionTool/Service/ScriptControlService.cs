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
using Model;

namespace VisionTool.Service
{
    public class ScriptControlService
    {
        private readonly SettingConfigService settingConfigService;
        private readonly HV.V1.Interpreter interpreter;
        private Action<bool> isRunningAction;
        private Action<string> currentFPSAction;
        private Action<string> currentExecutiontimeAction;
        private Action<int> currentErrorLineAction;

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







        ~ScriptControlService()
        {
            this.interpreter.TraceEvent -= Trace;
        }

        private void Trace(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.ScriptLogCollection.Insert(0, new Log("스크립트", text));
            });
            Thread.Sleep(1);
        }




        public void SaveScript(Script context)
        {
            try
            {
                string filePath = DialogHelper.SaveFile("Script File (.vsjs)|*.vsjs");
                context.FilePath = filePath;
                context.FileName = Path.GetFileName(filePath);
                File.WriteAllText(filePath, context.ScriptContent, Encoding.UTF8);

            }catch(Exception e)
            {
                throw e;
            }
        }

        public ObservableCollection<Script> GetScriptsFromFolder()
        {

            try
            {
                ObservableCollection<Script> collection = new ObservableCollection<Script>();

                var path = DialogHelper.OpenFolder();
                var files = Directory.GetFiles(path, "*.vsjs");

                foreach (var file in files)
                {
                    var script = new Script(Path.GetFileName(file), File.ReadAllText(file, Encoding.UTF8), file);
                    collection.Add(script);
                }

                return collection;

            }
            catch(Exception e)
            {
                throw e;
            }

        }

        public Script GetScriptFromPath()
        {
            try
            {
                var path = DialogHelper.OpenFile("Script File (.vsjs)|*.vsjs");
                var context = File.ReadAllText(path, Encoding.UTF8);
                Script script = new Script(Path.GetFileName(path), context, path);
                return script;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void RemoveScript(Script model)
        {
            if (model == null) return;
            this.ScriptCollection.Remove(model);
        }
        
        private Script CreateNewScript()
        {
            return new Script("new.vsjs", "/* Be the god of coding */", "");
        }


        public void LoadScriptFromPath()
        {

            try
            {
                var script = this.GetScriptFromPath();
                this.ScriptCollection.Add(script);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }

        }

        public void AddNewScript()
        {
            this.ScriptCollection.Add(this.CreateNewScript());
        }


        public void LoadScriptFromPath(string path)
        {
            try
            {

                var fileName = Path.GetFileName(path);
                var content = File.ReadAllText(path, Encoding.UTF8);

                this.ScriptCollection.Add(new Script(fileName, content, path));

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }

        }

        public void ClearNativeModules()
        {
            this.NativeModuleCollection.Clear();
          
            

            foreach(var _object in this.GlobalCollection)
            {
                _object.Dispose();
            }

            this.GlobalCollection.Clear();


            foreach(var _object in this.interpreter.GlobalObjects)
            {
                _object.Value.Dispose();
            }
            this.interpreter.GlobalObjects.Clear();



        }

        public bool ContinuousRunScript(string context)
        {
            if (this.IsRunningScript == true) return false;
            if (context == null) return false;

            this.IsRunningScript = true;

            this.interpreter.SetModulePath(this.settingConfigService.ApplicationSetting.ModuleMainPath);
            this.ScriptLogCollection.Clear();
            Task.Run(async () =>
            {
                var currentFps = 0;
                //var stacked_time = 0.0;

                var startTime = DateTime.Now;
                while (IsRunningScript)
                {
                    //await Task.Delay(10);
                    //var watch = System.Diagnostics.Stopwatch.StartNew();
                    var startStepTime = DateTime.Now;
                    try
                    {

                        this.interpreter.RunScript(context);

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.GlobalCollection.Clear();
                            this.GlobalCollection.AddRange(this.interpreter.GlobalObjects.Values.ToList());


                            this.GlobalNames.Clear();
                            this.GlobalNames.AddRange(this.interpreter.GlobalNames.ToList());


                            this.NativeModuleCollection.Clear();
                            this.NativeModuleCollection.AddRange(this.interpreter.NativeModules.Values.ToList());

                            
                            


                            foreach (var result in this.ResultObjectCollection)
                            {
                                foreach (var globalName in this.interpreter.GlobalNames)
                                {
                                    if (result.Name == globalName)
                                        result.Data = this.interpreter.GlobalObjects[globalName];
                                }
                            }

                        }, DispatcherPriority.Send);
                    }
                    catch (HV.V1.ScriptError e)
                    {

                        System.Diagnostics.Debug.WriteLine(e.Message);
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            string errorContent = string.Format("Error Line:{0}, Column({1},{2})\n{3}", e.Line(), e.StartColumn(), e.EndColumn(), e.Message);
                            this.ScriptLogCollection.Add(new Log("Error", errorContent));
                            this.currentErrorLineAction.Invoke(e.Line());
                            DialogHelper.ShowToastErrorMessage("스크립트 에러 메세지", errorContent);
                            
                        }, DispatcherPriority.Send);

                        break;
                    }

                    var endTime = DateTime.Now;
                    var measureSecond = (endTime - startTime).TotalMilliseconds;
                    var measureTaktTime = (endTime - startStepTime).TotalMilliseconds;
                    currentFps++;
                    if (measureSecond > 1000)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.currentFPSAction.Invoke(currentFps.ToString("F2"));
                            this.currentExecutiontimeAction.Invoke(measureTaktTime.ToString() + " ms");
                        }, DispatcherPriority.Send);
                        currentFps = 0;
                        startTime = DateTime.Now;

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
            this.ScriptLogCollection.Clear();
            this.interpreter.SetModulePath(this.settingConfigService.ApplicationSetting.ModuleMainPath);
            Task.Run(() =>
            {
                try
                {
                    this.interpreter.RunScript(context);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        this.GlobalCollection.Clear();
                        this.GlobalCollection.AddRange(this.interpreter.GlobalObjects.Values);

                        this.GlobalNames.Clear();
                        this.GlobalNames.AddRange(this.interpreter.GlobalNames.ToList());


                        this.NativeModuleCollection.Clear();
                        this.NativeModuleCollection.AddRange(this.interpreter.NativeModules.Values);


                        foreach(var result in this.ResultObjectCollection)
                        {
                            foreach(var globalObject in this.GlobalCollection)
                            {
                                if (result.Name == globalObject.Name)
                                    result.Data = globalObject;
                            }
                        }

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
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                    }, DispatcherPriority.Send);

                }
                catch(HV.V1.ScriptError e)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        string errorContent = string.Format("Error Line:{0}, Column({1},{2})\n{3}", e.Line(), e.StartColumn(), e.EndColumn(), e.Message);
                        this.ScriptLogCollection.Add(new Log("Error", errorContent));
                        this.currentErrorLineAction.Invoke(e.Line());
                        DialogHelper.ShowToastErrorMessage("스크립트 에러 메세지", errorContent);
                    }, DispatcherPriority.Send);
                }

                this.IsRunningScript = false;
            });

            return true;
        }

        private ObservableCollection<Log> _ScriptLogCollection = new ObservableCollection<Log>();
        public ObservableCollection<Log> ScriptLogCollection
        {
            get
            {
                return _ScriptLogCollection;
            }
        }

        private ObservableCollection<Script> _ScriptCollection = new ObservableCollection<Script>();
        public ObservableCollection<Script> ScriptCollection
        {
            get
            {

                return _ScriptCollection;
            }
            //set => Set(ref _ScriptCollection, value);
        }

        private ObservableCollection<HV.V1.NativeModule> _NativeModuleCollection = new ObservableCollection<HV.V1.NativeModule>();
        public ObservableCollection<HV.V1.NativeModule> NativeModuleCollection
        {
            get
            {
                return _NativeModuleCollection;
            }
        }

        private ObservableCollection<HV.V1.Object> _GlobalCollection = new ObservableCollection<HV.V1.Object>();
        public ObservableCollection<HV.V1.Object> GlobalCollection
        {
            get
            {
                return _GlobalCollection;
            }
        }


        private ObservableCollection<string> _GlobalNames = new ObservableCollection<string>();
        public ObservableCollection<string> GlobalNames
        {
            get
            {
                return _GlobalNames;
            }
        }


        public void SetCallbackRunning(Action<bool> check)
        {
            this.isRunningAction += check;

        }

        public void SetCallbackCurrentExecutionTime(Action<string> check)
        {
            this.currentExecutiontimeAction += check;
        }

        public void SetCallbackCurrentFPS(Action<string> check)
        {
            this.currentFPSAction += check;
        }

        public void SetCallbackCurrentErrorLine(Action<int> check)
        {
            this.currentErrorLineAction += check;
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


        private ObservableCollection<ResultObject> _ResultObjectCollection = null;
        public ObservableCollection<ResultObject> ResultObjectCollection
        {
            get
            {
                _ResultObjectCollection ??= new ObservableCollection<ResultObject>();
                return _ResultObjectCollection;
            }
        }

        public void ClearResultObject()
        {
            foreach(var result in this.ResultObjectCollection)
            {
                result.Data.Dispose();
            }

            this.ResultObjectCollection.Clear();
        }


        public void AddResultObject(string name)
        {
            if(this.interpreter.GlobalNames.Contains(name) == true)
            {

                var globalObject = this.interpreter.GlobalObjects[name];

                this.ResultObjectCollection.Add(new ResultObject()
                {
                    Name = name,
                    Data = globalObject
                });
            }
        }

        //public void AddResultObject(HV.V1.Object _object)
        //{
        //    if(this.interpreter.GlobalNames.Contains(_object.Name) == true)
        //    {
        //        this.ResultObjectCollection.Add(new ResultObject()
        //        {
        //            Name = _object.Name,
        //            Data = _object
        //        });
        //    }
        //}
    }
}
