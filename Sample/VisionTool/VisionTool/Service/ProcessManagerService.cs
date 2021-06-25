using DevExpress.Xpf.CodeView;
using GalaSoft.MvvmLight;
using Gapotchenko.FX.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;


namespace VisionTool.Service
{
    public class ProcessManagerService
    {

        private readonly SettingConfigService settingConfigService;

        public ProcessManagerService(SettingConfigService _settingConfigService)
        {
            this.settingConfigService = _settingConfigService;

            //this.ShutDownAllChildProcess();
            //this.LoadDevice();
            //this.RunAllChildProcess();
   
        }



        public void ShutDownAllChildProcess()
        {
            
            var deviceNames = Directory.GetFiles(this.settingConfigService.ApplicationSetting.DevicePath);

            try
            {
                foreach(var process in this.Processese)
                {
                    try
                    {
                        process.Value.Kill();
                        process.Value.Dispose();

                    }catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                this.Processese.Clear();
            }


            foreach (var device in deviceNames)
            {
                var deviceName = Path.GetFileNameWithoutExtension(device);
                var processes = Process.GetProcessesByName(deviceName);
                try
                {
                    
                    
                    foreach (var process in processes)
                    {
                        var env = process.ReadEnvironmentVariables();
                        if (env["VisionParentUniqueID"] == this.settingConfigService.ApplicationSetting.ProgramUniqueID)
                        {
                            process.Kill();
                            process.WaitForExit(2000);

                        }
                    }

                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                finally
                {
                    foreach (var process in processes)
                        process.Dispose();
                    processes = null;
                }
            }

        }

        public void RunChildProcess(Device.Device device)
        {

            try
            {
                var process = this.Processese[device.Uid];

                process.Kill();
                process.Exited -= this.ProcessManagerService_Exited;
                process.Dispose();

                this.Processese.Remove(device.Uid);
                

            }catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

            }

            try
            {
                string filePath = this.settingConfigService.ApplicationSetting.DeviceMainPath;
                filePath = filePath + Path.DirectorySeparatorChar + device.DeviceName + Path.DirectorySeparatorChar + device.DeviceName + ".exe";

                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = filePath;
                info.UseShellExecute = false;
                info.Arguments = device.Uid;
                info.CreateNoWindow = true;
                info.EnvironmentVariables["VisionParentUniqueID"] = this.settingConfigService.ApplicationSetting.ProgramUniqueID;



                this.Processese[device.Uid] = Process.Start(info);

                device.IsAlive = true;
                device.Pid = this.Processese[device.Uid].Id.ToString();
                this.Processese[device.Uid].EnableRaisingEvents = true;
                this.Processese[device.Uid].Exited += ProcessManagerService_Exited;

                System.Diagnostics.Debug.WriteLine("Original affinity: " + this.Processese[device.Uid].ProcessorAffinity);
                this.Processese[device.Uid].ProcessorAffinity = (IntPtr)((1 << Environment.ProcessorCount) - 1);
                System.Diagnostics.Debug.WriteLine("After affinity: " + this.Processese[device.Uid].ProcessorAffinity);
                bool defaultSetup = device.DefaultSetup();
                if (defaultSetup == false)
                    System.Diagnostics.Debug.WriteLine("Default setup failed" + device.Uid);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void RunAllChildProcess()
        {
            bool hasError = false;
            foreach(var device in this.DeviceCollection)
            {
                try
                {
                    string filePath = this.settingConfigService.ApplicationSetting.DeviceMainPath;
                    filePath = filePath + Path.DirectorySeparatorChar + device.DeviceName + Path.DirectorySeparatorChar + device.DeviceName + ".exe";


                    ProcessStartInfo info = new ProcessStartInfo();
                    info.FileName = filePath;
                    info.UseShellExecute = false;
                    info.Arguments = device.Uid;
                    info.CreateNoWindow = true;
                    info.EnvironmentVariables["VisionParentUniqueID"] = this.settingConfigService.ApplicationSetting.ProgramUniqueID;
                    

        
                    this.Processese[device.Uid] = Process.Start(info);

                    device.IsAlive = true;
                    device.Pid = this.Processese[device.Uid].Id.ToString();
                    this.Processese[device.Uid].EnableRaisingEvents = true;
                    this.Processese[device.Uid].Exited += ProcessManagerService_Exited;
       
                    System.Diagnostics.Debug.WriteLine("Original affinity: " + this.Processese[device.Uid].ProcessorAffinity);
                    this.Processese[device.Uid].ProcessorAffinity = (IntPtr)((1 << Environment.ProcessorCount) - 1);
                    System.Diagnostics.Debug.WriteLine("After affinity: " + this.Processese[device.Uid].ProcessorAffinity);
                    bool defaultSetup = device.DefaultSetup();
                    if (defaultSetup == false)
                        System.Diagnostics.Debug.WriteLine("Default setup failed" + device.Uid);
                }
                catch(Exception e)
                {
                    hasError = true;
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }




            if (hasError)
            {
                throw new Exception("일부 디바이스가 실행되지 못했습니다.");
            }
            


        }

        private void ProcessManagerService_Exited(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Process Killed");
            Process process = sender as Process;

            foreach(var device in this.DeviceCollection)
            {
                try
                {
                    if (device.Pid == process.Id.ToString())
                    {
                        device.IsAlive = false;
                    }

                }
                catch(Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine(ex.Message);

                }
                
            }

        }

        public void ClearDevice()
        {
            try
            {
                this.ShutDownAllChildProcess();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            try
            {
                this.DeviceCollection.Clear();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            
        }


        public void SaveDevice()
        {
            try
            {
                var targetSettingPath = this.settingConfigService.ApplicationSetting.DeviceSettingPath + "Setting.devset";
                var setting = new Device.DeviceSetting()
                {
                    DeviceCollection = this.DeviceCollection
                };

                string jsonString = JsonConvert.SerializeObject(setting, Formatting.Indented);
                File.WriteAllText(targetSettingPath, jsonString, Encoding.UTF8);

            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void LoadDevice()
        {
            try
            {
                var targetSettingPath = this.settingConfigService.ApplicationSetting.DeviceSettingPath + "Setting.devset";
                var jsonContent = File.ReadAllText(targetSettingPath);
                var setting = JsonConvert.DeserializeObject<Device.DeviceSetting>(jsonContent);
                foreach(var device in this.DeviceCollection)
                {
                    device.Dispose();
                }
                this.DeviceCollection.Clear();
                this.DeviceCollection.AddRange(setting.DeviceCollection);

            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

        }


        public void ExportDevice()
        {
            try
            {
                
                var targetSettingPath = DialogHelper.SaveFile("device setting file (.devset)|*.devset");
                var setting = new Device.DeviceSetting()
                {
                    DeviceCollection = this.DeviceCollection
                };

                string jsonString = JsonConvert.SerializeObject(setting, Formatting.Indented);
                File.WriteAllText(targetSettingPath, jsonString, Encoding.UTF8);


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

        }
        
        public void ImportDevice()
        {
            var targetSettingPath = DialogHelper.OpenFile("device setting file (.devset)|*.devset");
            var jsonContent = File.ReadAllText(targetSettingPath);
            var setting = JsonConvert.DeserializeObject<Device.DeviceSetting>(jsonContent);
            foreach (var device in this.DeviceCollection)
            {
                device.Dispose();
            }
            this.DeviceCollection.Clear();
            this.DeviceCollection.AddRange(setting.DeviceCollection);
        }


        private Dictionary<string, Process> _Processese = null;
        public Dictionary<string, Process> Processese
        {
            get
            {
                _Processese ??= new Dictionary<string, Process>();
                return _Processese;
            }
        }

        public void DeleteDevice(Device.Device device)
        {
            if (device == null)
                throw new Exception("Device is null");

            try
            {
                this.DeviceCollection.Remove(device);
                device.Dispose();
                this.Processese[device.Uid].Kill();
                this.Processese[device.Uid].Exited -= ProcessManagerService_Exited;
                this.Processese[device.Uid].Dispose();
                this.Processese.Remove(device.Uid);
            }catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }


        public void AddDevice(Model.DeviceConfig config)
        {
            if (config == null)
                throw new Exception("Config is not valid");

            if (config.DeviceType == null)
                throw new Exception("Config is not valid");


            if (config.DeviceType.Length == 0)
                throw new Exception("Config is not valid");

            try
            {

                var assembly = Assembly.GetExecutingAssembly();

                Type deviceType = Type.GetType("Device." + config.DeviceType + ", Device");
                var device = Activator.CreateInstance(deviceType) as Device.Device;

                device.DeviceName = config.DeviceName;
                

                this.DeviceCollection.Add(device);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                throw e;
            }

        }


        private ObservableCollection<Device.Device> _DeviceCollection = null;
        public ObservableCollection<Device.Device> DeviceCollection
        {
            get
            {
                _DeviceCollection ??= new ObservableCollection<Device.Device>();
                return _DeviceCollection;
            }
        }
    }
}
