using static SPPClient.Management;

namespace SPPClient
{
    internal partial class KeyManager
    {
        #region "WMI Define"

        private const string ProductClass = "SoftwareLicensingProduct";
        private const string ServiceClass = "SoftwareLicensingService";
        private const string ProductIsPrimarySkuSelectClause = "ID, ApplicationId, PartialProductKey, LicenseIsAddon, Description, Name";
        private const string PartialProductKeyNonNullWhereClause = "PartialProductKey <> null";
        private const string KMSClientLookupClause = "KeyManagementServiceMachine, KeyManagementServicePort, KeyManagementServiceLookupDomain";
        private const string EmptyWhereClause = "";
        private const string WindowsAppId = "55c92734-d682-4d71-983e-d6ec3f16059f";
        private const string SLKeyPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform";
        private const string SLKeyPath32 = @"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform";

        private readonly ManagementScope WMIObject = new(@"\\localhost\root\cimv2");

        public KeyManager()
        {
            Connect();
        }

        private void Connect()
        {
            WMIObject.Connect();
        }

        private ManagementObject GetServiceObject(string strQuery)
        {
            foreach (ManagementObject objService in GetObjectCollection("SELECT " + strQuery + " FROM " + ServiceClass))
            {
                return objService; //这里仿照slmgr.vbs
            }
            return null;
        }

        private ManagementObjectCollection GetProductCollection(string strQuery, string strWhere = "")
        {
            var strWhereLink = "";
            if (strWhere != "")
            {
                strWhereLink = " WHERE " + strWhere;
            }
            var colCollection = GetObjectCollection("SELECT " + strQuery + " FROM " + ProductClass + strWhereLink);
            if (colCollection.Count != 0)
            {
                return colCollection;
            }
            return null;
        }

        private ManagementObjectCollection GetObjectCollection(string query)
        {
            ManagementObjectSearcher searcher = new(WMIObject, GetObjectQuery(query));
            return searcher.Get();
        }

        #endregion "WMI Define"
        #region"LOCAL BASE API"

        private int GetIsPrimaryWindowsSKU(ManagementObject objProduct)
        {
            var iPrimarySku = 0;
            if ((objProduct.GetProperty<string>("ApplicationId").ToLower() == WindowsAppId) & (objProduct.GetProperty<string>("PartialProductKey") != ""))
            {
                try
                {
                    var bIsAddOn = objProduct.GetProperty<bool>("LicenseIsAddon");
                    if (bIsAddOn)
                    {
                        iPrimarySku = 0;
                    }
                    else
                    {
                        iPrimarySku = 1;
                    }
                }
                catch
                {
                    var objDesc = objProduct.GetProperty<string>("Description");
                    if (IsKmsClient(objDesc) | IsKmsServer(objDesc))
                    {
                        iPrimarySku = 1;
                    }
                    else
                    {
                        iPrimarySku = 2;
                    }
                }
            }
            return iPrimarySku;
        }

        private static bool CheckProductForCommand(ManagementObject objProduct, string strActivationID)
        {
            var bCheckProductForCommand = false;

            if ((strActivationID == "") & (objProduct.GetProperty<string>("ApplicationId").ToLower() == WindowsAppId) & (objProduct.GetProperty<bool>("LicenseIsAddon") == false))
            {
                bCheckProductForCommand = true;
            }
            if (objProduct.GetProperty<string>("ID").ToLower() == strActivationID)
            {
                bCheckProductForCommand = true;
            }
            return bCheckProductForCommand;
        }

        public bool GetLicenseStatus()
        {
            var objService = GetServiceObject("Version");
            objService.Method("RefreshLicenseStatus");
            foreach (ManagementObject objProduct in GetProductCollection(ProductIsPrimarySkuSelectClause + ", LicenseStatus", PartialProductKeyNonNullWhereClause))
            {
                var bCheckProductForCommand = CheckProductForCommand(objProduct, "");
                if (bCheckProductForCommand)
                {
                    var strDescription = objProduct.GetProperty<string>("Description");
                    var licenseStatus = objProduct.GetProperty<uint>("LicenseStatus") == 1;
                    if (strDescription.ToUpper().IndexOf("WINDOWS") != -1)
                    {
                        return licenseStatus;
                    }
                }
            }
            return false;
        }

        public bool InstallLicense(string licFilePath)
        {
            try
            {
                if (File.Exists(licFilePath))
                {
                    var LicenseData = File.ReadAllText(licFilePath);
                    var objService = GetServiceObject("Version");
                    objService.Method("InstallLicense", LicenseData);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static List<string> GetAllXrmMSFiles(string rootPath)
        {
            return Directory.EnumerateFiles(rootPath, "*.xrm-ms", SearchOption.AllDirectories).ToList();
        }

        private ManagementObject GetKmsClientObjectByActivationID(string strActivationID = "")
        {
            try
            {
                strActivationID = strActivationID.ToLower();
                ManagementObject objTarget = null;
                if (strActivationID == "")
                {
                    objTarget = GetServiceObject("Version, " + KMSClientLookupClause);
                }
                else
                {
                    foreach (ManagementObject objProduct in GetProductCollection("ID, " + KMSClientLookupClause, EmptyWhereClause))
                    {
                        if (objProduct.GetProperty<string>("ID") == strActivationID)
                        {
                            objTarget = objProduct;
                            break;
                        }
                    }
                }
                return objTarget;
            }
            catch
            {
                return null;
            }
        }

        private bool SetKms(string Name = "", string Port = "", string strActivationID = "")
        {
            try
            {
                var objTarget = GetKmsClientObjectByActivationID(strActivationID);
                if (objTarget != null)
                {
                    if (Name != "")
                    {
                        objTarget.Method("SetKeyManagementServiceMachine", Name);
                    }
                    else
                    {
                        objTarget.Method("ClearKeyManagementServiceMachine");
                    }
                    if (Port != "")
                    {
                        objTarget.Method("SetKeyManagementServicePort", long.Parse(Port));
                    }
                    else
                    {
                        objTarget.Method("ClearKeyManagementServicePort");
                    }
                    if (objTarget.GetProperty<string>("KeyManagementServiceMachine") != "")
                    {
                        Console.WriteLine("警告：在设置KMS服务器时发现已经存在KMS域服务器，现在的设定将覆盖域服务器的相关设定，当前KMS服务器（" + Name + ":" + Port + "）将被用于激活");
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool ClearKmsLookupDomain(string strActivationID = "")
        {
            try
            {
                var objTarget = GetKmsClientObjectByActivationID(strActivationID);
                if (objTarget != null)
                {
                    objTarget.Method("ClearKeyManagementServiceLookupDomain");
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool SetHostCachingDisable(bool boolHostCaching)
        {
            try
            {
                var objService = GetServiceObject("Version");
                objService.Method("DisableKeyManagementServiceHostCaching", boolHostCaching);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
        #region"PUBLIC API"

        public bool InstallProductKey(string key)
        {
            try
            {
                var bIsKms = false;
                var objService = GetServiceObject("Version");
                var strVersion = objService.GetProperty<string>("Version");
                objService.Method("InstallProductKey", key);
                objService.Method("RefreshLicenseStatus");
                foreach (ManagementObject objProduct in GetProductCollection(ProductIsPrimarySkuSelectClause, PartialProductKeyNonNullWhereClause))
                {
                    var strDescription = objProduct.GetProperty<string>("Description");
                    var iIsPrimaryWindowsSku = GetIsPrimaryWindowsSKU(objProduct);
                    if (iIsPrimaryWindowsSku == 2)
                    {
                        Console.WriteLine("警告：当前KEY对应SKU未知，可能安装失败！");
                    }
                    if (IsKmsServer(strDescription))
                    {
                        bIsKms = true;
                        break;
                    }
                }
                if (bIsKms)
                {
                    if (ExistsRegistry(Registry.LocalMachine, SLKeyPath))
                    {
                        if (SetRegistryValue(Registry.LocalMachine, SLKeyPath, "KeyManagementServiceVersion", strVersion, RegistryValueKind.String) == false)
                        {
                            return false;
                        }
                    }
                    if (ExistsRegistry(Registry.LocalMachine, SLKeyPath32))
                    {
                        if (SetRegistryValue(Registry.LocalMachine, SLKeyPath32, "KeyManagementServiceVersion", strVersion, RegistryValueKind.String) == false)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    if (ExistsRegistry(Registry.LocalMachine, SLKeyPath))
                    {
                        if (DeleteRegistryValue(Registry.LocalMachine, SLKeyPath, "KeyManagementServiceVersion") == false)
                        {
                            return false;
                        }
                    }
                    if (ExistsRegistry(Registry.LocalMachine, SLKeyPath32))
                    {
                        if (DeleteRegistryValue(Registry.LocalMachine, SLKeyPath, "KeyManagementServiceVersion") == false)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UninstallProductKey(string strActivationID = "")
        {
            try
            {
                strActivationID = strActivationID.ToLower();
                var kmsServerFound = false;
                var uninstallDone = false;
                var objService = GetServiceObject("Version");
                var strVersion = objService.GetProperty<string>("Version");
                foreach (ManagementObject objProduct in GetProductCollection(ProductIsPrimarySkuSelectClause + ", ProductKeyID", PartialProductKeyNonNullWhereClause))
                {
                    var strDescription = objProduct.GetProperty<string>("Description");
                    var bCheckProductForCommand = CheckProductForCommand(objProduct, strActivationID);
                    if (bCheckProductForCommand)
                    {
                        var iIsPrimaryWindowsSku = GetIsPrimaryWindowsSKU(objProduct);
                        if (iIsPrimaryWindowsSku == 2)
                        {
                            Console.WriteLine("警告：当前KEY对应SKU未知，可能安装失败！");
                        }
                        objProduct.Method("UninstallProductKey");
                        objService.Method("RefreshLicenseStatus");
                        if ((strActivationID != "") | (iIsPrimaryWindowsSku == 1))
                        {
                            uninstallDone = true;
                        }
                    }
                    else if (IsKmsServer(strDescription))
                    {
                        kmsServerFound = true;
                    }
                    if ((kmsServerFound == true) & (uninstallDone == true))
                    {
                        break;
                    }
                }
                if (kmsServerFound)
                {
                    if (ExistsRegistry(Registry.LocalMachine, SLKeyPath))
                    {
                        if (SetRegistryValue(Registry.LocalMachine, SLKeyPath, "KeyManagementServiceVersion", strVersion, RegistryValueKind.String) == false)
                        {
                            return false;
                        }
                    }
                    if (ExistsRegistry(Registry.LocalMachine, SLKeyPath32))
                    {
                        if (SetRegistryValue(Registry.LocalMachine, SLKeyPath32, "KeyManagementServiceVersion", strVersion, RegistryValueKind.String) == false)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (ExistsRegistry(Registry.LocalMachine, SLKeyPath))
                    {
                        if (DeleteRegistryValue(Registry.LocalMachine, SLKeyPath, "KeyManagementServiceVersion") == false)
                        {
                            return false;
                        }
                    }
                    if (ExistsRegistry(Registry.LocalMachine, SLKeyPath32))
                    {
                        if (DeleteRegistryValue(Registry.LocalMachine, SLKeyPath, "KeyManagementServiceVersion") == false)
                        {
                            return false;
                        }
                    }
                }
                return uninstallDone;
            }
            catch
            {
                return false;
            }
        }

        public bool ActivateProduct(string strActivationID = "")
        {
            try
            {
                strActivationID = strActivationID.ToLower();
                var bFoundAtLeastOneKey = false;
                var objService = GetServiceObject("Version");
                foreach (ManagementObject objProduct in GetProductCollection(ProductIsPrimarySkuSelectClause + ", LicenseStatus, VLActivationTypeEnabled", PartialProductKeyNonNullWhereClause))
                {
                    var bCheckProductForCommand = CheckProductForCommand(objProduct, strActivationID);
                    if (bCheckProductForCommand)
                    {
                        var iIsPrimaryWindowsSku = GetIsPrimaryWindowsSKU(objProduct);
                        if ((iIsPrimaryWindowsSku == 2) & (strActivationID == ""))
                        {
                            Console.WriteLine("警告：当前KEY对应SKU未知，可能安装失败！");
                        }
                        //Console.WriteLine(objProduct.GetProperty<object>("VLActivationTypeEnabled").ToString());
                        if (objProduct.GetProperty<object>("VLActivationTypeEnabled").ToString() == "3") //无故抛出uint2int异常
                        {
                            Console.WriteLine("错误：此系统已配置为仅限基于令牌的激活，当前激活仅支持零售模式、KMS，请尝试更改激活类型设置");
                            return false;
                        }
                        var strDescription = objProduct.GetProperty<string>("Description");
                        if ((IsMAK(strDescription) == false) | (objProduct.GetProperty<uint>("LicenseStatus") != 1))
                        {
                            objProduct.Method("Activate");
                            objService.Method("RefreshLicenseStatus");
                        }
                        bFoundAtLeastOneKey = true;
                        if ((strActivationID != "") | (iIsPrimaryWindowsSku == 1))
                        {
                            break;
                        }
                    }
                }
                return bFoundAtLeastOneKey;
            }
            catch
            {
                return false;
            }
        }

        public bool ClearPKeyFromRegistry()
        {
            try
            {
                var objService = GetServiceObject("Version");
                objService.Method("ClearProductKeyFromRegistry");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ReinstallLicenses()
        {
            try
            {
                var strOemFolder = Environment.SystemDirectory + @"\oem";
                var strSppTokensFolder = Environment.SystemDirectory + @"\spp\tokens";
                foreach (string sppfile in GetAllXrmMSFiles(strSppTokensFolder))
                {
                    InstallLicense(sppfile);
                }
                foreach (string oemfile in GetAllXrmMSFiles(strOemFolder))
                {
                    InstallLicense(oemfile);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RearmWindows()
        {
            try
            {
                var objService = GetServiceObject("Version");
                objService.Method("ReArmWindows");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ClearKms()
        {
            return SetKms() & ClearKmsLookupDomain();
        }

        public bool ClearKmsServerCache()
        {
            return SetHostCachingDisable(true);
        }

        public enum VLActivationType : int
        {
            Any,
            ActiveDirectory,
            KMS,
            TokenBased
        }

        public bool SetVLActivationType(VLActivationType intType = VLActivationType.Any, string strActivationID = "")
        {
            try
            {
                var objTarget = GetKmsClientObjectByActivationID(strActivationID);
                if (objTarget != null)
                {
                    if (intType != VLActivationType.Any)
                    {
                        objTarget.Method("SetVLActivationTypeEnabled", intType);
                    }
                    else
                    {
                        objTarget.Method("ClearVLActivationTypeEnabled");
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }

    internal static class Management
    {
        public static void Method(this ManagementObject obj, string methodName, params object[] objParams)
        {
            if (obj != null)
            {
                //var watcher = GetOperationObserver(methodName);
                obj.InvokeMethod(methodName, objParams);
            }
        }

        public static T GetProperty<T>(this ManagementObject obj, string propertyName)
        {
            if (obj != null)
            {
                return (T)obj.GetPropertyValue(propertyName);
            }
            return default;
        }

        public static ObjectQuery GetObjectQuery(string query)
        {
            return new ObjectQuery(query);
        }

        public static ManagementOperationObserver GetOperationObserver(string methodName)
        {
            var result = new ManagementOperationObserver();
            var signStr = "[" + methodName + "]:";
            result.Completed += new CompletedEventHandler((o, c) =>
            {
                switch (c.Status)
                {
                    case ManagementStatus.Failed:
                        Console.WriteLine(signStr + "失败");
                        break;

                    case ManagementStatus.AccessDenied:
                        Console.WriteLine(signStr + "操作被禁止");
                        break;

                    case ManagementStatus.PrivilegeNotHeld:
                        Console.WriteLine(signStr + "权限不足");
                        break;

                    case ManagementStatus.ServerTooBusy:
                        Console.WriteLine(signStr + "服务正忙");
                        break;

                    case ManagementStatus.Timedout:
                        Console.WriteLine(signStr + "操作超时");
                        break;

                    case ManagementStatus.Unexpected:
                        Console.WriteLine(signStr + "未知错误");
                        break;

                    case ManagementStatus.ProviderFailure:
                        Console.WriteLine(signStr + "服务操作失败");
                        break;

                    case ManagementStatus.InvalidParameter:
                        Console.WriteLine(signStr + "无效参数");
                        break;

                    case ManagementStatus.InitializationFailure:
                        Console.WriteLine(signStr + "初始化错误");
                        break;

                    case ManagementStatus.IllegalNull:
                        Console.WriteLine(signStr + "非法引用");
                        break;

                    case ManagementStatus.IllegalOperation:
                        Console.WriteLine(signStr + "非法操作");
                        break;

                    case ManagementStatus.NoError:
                        Console.WriteLine(signStr + "成功");
                        break;

                    default:
                        Console.WriteLine(signStr + "未知错误");
                        break;
                }
            });
            return result;
        }

        public static bool IsKmsClient(string strDescription)
        {
            if (strDescription.ToUpper().IndexOf("VOLUME_KMSCLIENT") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsKmsServer(string strDescription)
        {
            if (IsKmsClient(strDescription))
            {
                return false;
            }
            else
            {
                if (strDescription.ToUpper().IndexOf("VOLUME_KMS") != -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool IsMAK(string strDescription)
        {
            if (strDescription.ToUpper().IndexOf("MAK") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool SetRegistryValue(RegistryKey registry, string path, string name, object value, RegistryValueKind valueKind)
        {
            try
            {
                var tree = registry.CreateSubKey(path, true);
                tree.SetValue(name, value, valueKind);
                tree.Close();
                registry.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T GetetRegistryValue<T>(RegistryKey registry, string path, string name)
        {
            try
            {
                var tree = registry.CreateSubKey(path, true);
                var result = tree.GetValue(name, default);
                tree.Close();
                registry.Close();
                return (T)result;
            }
            catch
            {
                return default;
            }
        }

        public static bool DeleteRegistryValue(RegistryKey registry, string path, string name)
        {
            try
            {
                var tree = registry.CreateSubKey(path, true);
                tree.DeleteValue(name, false);
                tree.Close();
                registry.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ExistsRegistry(RegistryKey registry, string path)
        {
            try
            {
                registry.OpenSubKey(path, true);
                registry.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}