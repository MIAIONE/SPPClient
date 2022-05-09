using HWIDEx;
using System.Security.AccessControl;

internal class Program
{
    #region struct

    public enum ALG_ID : uint
    {
        CALG_MD5 = 0x8003,
        CALG_RSA = 0x800C
    }

    public enum HashParameters
    {
        HP_ALGID = 0x0001,
        HP_HASHVAL = 0x2,
        HP_HASHSIZE = 0x0004
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RTL_OSVERSIONINFOEX
    {
        public uint dwOSVersionInfoSize;
        public uint dwMajorVersion;
        public uint dwMinorVersion;
        public uint dwBuildNumber;
        public uint dwPlatformId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szCSDVersion;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FILE_TIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        public Int16 wYear;
        public Int16 wMonth;
        public Int16 wDayOfWeek;
        public Int16 wDay;
        public Int16 wHour;
        public Int16 wMinute;
        public Int16 wSecond;
        public Int16 wMilliseconds;
    }

    #endregion struct

    #region pinvoke
    [DllImport("Kernel32.dll")]
    private static extern bool Wow64DisableWow64FsRedirection(bool Wow64DisableRedirection);

    [DllImport("Ntdll.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    private static extern int RtlGetVersion(out RTL_OSVERSIONINFOEX osversion);

    [DllImport("slc.dll", EntryPoint = "SLGetWindowsInformationDWORD", CharSet = CharSet.Auto)]
    private static extern int SLGetWindowsInformationDWORD(string pwszValueName, ref int pdwValue);

    [DllImport("kernel32.dll")]
    private static extern void GetSystemTime(ref SYSTEMTIME lpSystemTime);

    [DllImport("kernel32.dll")]
    private static extern void GetSystemTimeAsFileTime(ref FILE_TIME lpSystemTimeAsFileTime);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool FileTimeToSystemTime(ref FILE_TIME lpFileTime, out SYSTEMTIME lpSystemTime);

    [DllImport("AdvApi32.dll", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CryptAcquireContextW(out IntPtr providerContext, [MarshalAs(UnmanagedType.LPWStr)] string container, [MarshalAs(UnmanagedType.LPWStr)] string provider, int providerType, uint flags);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool CryptCreateHash(IntPtr hProv, uint algId, IntPtr hKey, uint dwFlags, ref IntPtr phHash);

    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool CryptDestroyHash(IntPtr hHash);

    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool CryptDestroyKey(IntPtr phKey);

    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool CryptHashData(IntPtr hHash, byte[] pbData, uint dataLen, uint flags);

    [DllImport("Advapi32.dll", EntryPoint = "CryptReleaseContext", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool CryptReleaseContext(IntPtr hProv, Int32 dwFlags);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool CryptGetHashParam(IntPtr hHash, uint dwParam, Byte[] pbData, ref uint pdwDataLen, uint dwFlags);

    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr memset(byte[] dest, int c, int count);

    #endregion pinvoke

    #region define

    //private static bool KMS38 = false;

    private static Dictionary<string, Tuple<int, int, string, string>> UpdateProductKey = new Dictionary<string, Tuple<int, int, string, string>>()
    {
        {"43TBQ-NH92J-XKTM7-KT3KK-P39PB", new Tuple<int, int, string, string>(125, 17763, "EnterpriseS","Microsoft.Windows.125.X21-83233_8wekyb3d8bbwe")},
        {"NK96Y-D9CD8-W44CQ-R8YTK-DYJWX", new Tuple<int, int, string, string>(125, 14393, "EnterpriseS","Microsoft.Windows.125.X21-05035_8wekyb3d8bbwe")},
        {"FWN7H-PF93Q-4GGP8-M8RF3-MDWWW", new Tuple<int, int, string, string>(125, 10240, "EnterpriseS","Microsoft.Windows.125.X19-99617_8wekyb3d8bbwe")},
        {"QPM6N-7J2WJ-P88HH-P3YRH-YY74H", new Tuple<int, int, string, string>(191, 19044, "IoTEnterpriseS","Microsoft.Windows.191.X21-99682_8wekyb3d8bbwe")},
        {"M33WV-NHY3C-R7FPM-BQGPT-239PG", new Tuple<int, int, string, string>(126, 17763, "EnterpriseSN","Microsoft.Windows.126.X21-83264_8wekyb3d8bbwe")},
        {"2DBW3-N2PJG-MVHW3-G7TDK-9HKR4", new Tuple<int, int, string, string>(126, 14393, "EnterpriseSN","Microsoft.Windows.126.X21-04922_8wekyb3d8bbwe")},
        {"8V8WN-3GXBH-2TCMG-XHRX3-9766K", new Tuple<int, int, string, string>(126, 10240, "EnterpriseSN","Microsoft.Windows.126.X19-98770_8wekyb3d8bbwe")},
        {"XQQYW-NFFMW-XJPBH-K8732-CKFFD", new Tuple<int, int, string, string>(188, 19044, "IoTEnterprise","Microsoft.Windows.188.X21-99378_8wekyb3d8bbwe")},
        {"YTMG3-N6DKC-DKB77-7M9GH-8HVX7", new Tuple<int, int, string, string>(101, 0, "Core","Microsoft.Windows.101.X19-98868_8wekyb3d8bbwe")},
        {"N2434-X9D7W-8PF6X-8DV9T-8TYMD", new Tuple<int, int, string, string>(99, 0, "CoreCountrySpecific","Microsoft.Windows.99.X19-99652_8wekyb3d8bbwe")},
        {"4CPRK-NM3K3-X6XXQ-RXX86-WXCHW", new Tuple<int, int, string, string>(98, 0, "CoreN","Microsoft.Windows.98.X19-98877_8wekyb3d8bbwe")},
        {"BT79Q-G7N6G-PGBYW-4YWX6-6F4BT", new Tuple<int, int, string, string>(100, 0, "CoreSingleLanguage","Microsoft.Windows.100.X19-99661_8wekyb3d8bbwe")},
        {"YNMGQ-8RYV3-4PGQ3-C8XTP-7CFBY", new Tuple<int, int, string, string>(121, 0, "Education","Microsoft.Windows.121.X19-98886_8wekyb3d8bbwe")},
        {"84NGF-MHBT6-FXBX8-QWJK7-DRR8H", new Tuple<int, int, string, string>(122, 0, "EducationN","Microsoft.Windows.122.X19-98892_8wekyb3d8bbwe")},
        {"XGVPP-NMH47-7TTHJ-W3FW7-8HV2C", new Tuple<int, int, string, string>(4, 0, "Enterprise","Microsoft.Windows.4.X19-99683_8wekyb3d8bbwe")},
        {"WGGHN-J84D6-QYCPR-T7PJ7-X766F", new Tuple<int, int, string, string>(27, 0, "EnterpriseN","Microsoft.Windows.27.X19-98747_8wekyb3d8bbwe")},
        {"3V6Q6-NQXCX-V8YXR-9QCYV-QPFCT", new Tuple<int, int, string, string>(171, 0, "EnterpriseG","Microsoft.Windows.27.X19-98746_8wekyb3d8bbwe")},
        {"FW7NV-4T673-HF4VX-9X4MM-B4H4T", new Tuple<int, int, string, string>(172, 0, "EnterpriseGN","Microsoft.Windows.172.X21-24709_8wekyb3d8bbwe")},
        {"VK7JG-NPHTM-C97JM-9MPGT-3V66T", new Tuple<int, int, string, string>(48, 0, "Professional","Microsoft.Windows.48.X19-98841_8wekyb3d8bbwe")},
        {"2B87N-8KFHP-DKV6R-Y2C8J-PKCKT", new Tuple<int, int, string, string>(49, 0, "ProfessionalN","Microsoft.Windows.49.X19-98859_8wekyb3d8bbwe")},
        {"8PTT6-RNW4C-6V7J2-C2D3X-MHBPB", new Tuple<int, int, string, string>(164, 0, "ProfessionalEducation","Microsoft.Windows.164.X21-04955_8wekyb3d8bbwe")},
        {"GJTYN-HDMQY-FRR76-HVGC7-QPF8P", new Tuple<int, int, string, string>(165, 0, "ProfessionalEducationN","Microsoft.Windows.165.X21-04956_8wekyb3d8bbwe")},
        {"DXG7C-N36C4-C4HTG-X4T3X-2YV77", new Tuple<int, int, string, string>(161, 0, "ProfessionalWorkstation","Microsoft.Windows.161.X21-43626_8wekyb3d8bbwe")},
        {"WYPNQ-8C467-V2W6J-TX4WX-WT2RQ", new Tuple<int, int, string, string>(162, 0, "ProfessionalWorkstationN","Microsoft.Windows.162.X21-43644_8wekyb3d8bbwe")}
    };
    /*
    private static Dictionary<string, Tuple<int, int, string>> KMSProductKey = new Dictionary<string, Tuple<int, int, string>>()
 {
     {"M7XTQ-FN8P6-TTKYV-9D4CC-J462D", new Tuple<int, int, string>(125, 17763, "EnterpriseS")},
     {"DCPHK-NFMTC-H88MJ-PFHPY-QJ4BJ", new Tuple<int, int, string>(125, 14393, "EnterpriseS")},
     {"WNMTR-4C88C-JK8YV-HQ7T2-76DF9", new Tuple<int, int, string>(125, 10240, "EnterpriseS")},
     {"92NFX-8DJQP-P6BBQ-THF9C-7CG2H", new Tuple<int, int, string>(126, 17763, "EnterpriseSN")},
     {"QFFDN-GRT3P-VKWWX-X7T3R-8B639", new Tuple<int, int, string>(126, 14393, "EnterpriseSN")},
     {"2F77B-TNFGY-69QQF-B8YKP-D69TJ", new Tuple<int, int, string>(126, 10240, "EnterpriseSN")},
     {"TX9XD-98N7V-6WMQ6-BX7FG-H8Q99", new Tuple<int, int, string>(101, 0, "Core")},
     {"PVMJN-6DFY6-9CCP6-7BKTT-D3WVR", new Tuple<int, int, string>(99, 0, "CoreCountrySpecific")},
     {"3KHY7-WNT83-DGQKR-F7HPR-844BM", new Tuple<int, int, string>(98, 0, "CoreN")},
     {"7HNRX-D7KGG-3K4RQ-4WPJ4-YTDFH", new Tuple<int, int, string>(100, 0, "CoreSingleLanguage")},
     {"NW6C2-QMPVW-D7KKK-3GKT6-VCFB2", new Tuple<int, int, string>(121, 0, "Education")},
     {"2WH4N-8QGBV-H22JP-CT43Q-MDWWJ", new Tuple<int, int, string>(122, 0, "EducationN")},
     {"NPPR9-FWDCX-D2C8J-H872K-2YT43", new Tuple<int, int, string>(4, 0, "Enterprise")},
     {"DPH2V-TTNVB-4X9Q3-TJR4H-KHJW4", new Tuple<int, int, string>(27, 0, "EnterpriseN")},
     {"YYVX9-NTFWV-6MDM3-9PT4T-4M68B", new Tuple<int, int, string>(171, 0, "EnterpriseG")},
     {"44RPN-FTY23-9VTTB-MP9BX-T84FV", new Tuple<int, int, string>(172, 0, "EnterpriseGN")},
     {"W269N-WFGWX-YVC9B-4J6C9-T83GX", new Tuple<int, int, string>(48, 0, "Professional")},
     {"MH37W-N47XK-V7XM9-C7227-GCQG9", new Tuple<int, int, string>(49, 0, "ProfessionalN")},
     {"6TP4R-GNPTD-KYYHQ-7B7DP-J447Y", new Tuple<int, int, string>(164, 0, "ProfessionalEducation")},
     {"YVWGF-BXNMC-HTQYQ-CPQ99-66QFC", new Tuple<int, int, string>(165, 0, "ProfessionalEducationN")},
     {"NRG8B-VKK3Q-CXVCJ-9G2XF-6Q84J", new Tuple<int, int, string>(161, 0, "ProfessionalWorkstation")},
     {"9FNHH-K3HBT-3W4TD-6383H-6XYWF", new Tuple<int, int, string>(162, 0, "ProfessionalWorkstationN")},
     {"VDYBN-27WPP-V4HQT-9VMD4-VMK7H", new Tuple<int, int, string>(7, 20348, "ServerStandard")},
     {"N69G4-B89J2-4G8F4-WWYCC-J464C", new Tuple<int, int, string>(7, 17763, "ServerStandard")},
     {"WC2BQ-8NRM3-FDDYY-2BFGV-KHKQY", new Tuple<int, int, string>(7, 0, "ServerStandard")},
     {"67KN8-4FYJW-2487Q-MQ2J7-4C4RG", new Tuple<int, int, string>(13, 20348, "ServerStandardCore")},
     {"N2KJX-J94YW-TQVFB-DG9YT-724CC", new Tuple<int, int, string>(13, 17763, "ServerStandardCore")},
     {"PTXN8-JFHJM-4WC78-MPCBR-9W4KR", new Tuple<int, int, string>(13, 0, "ServerStandardCore")},
     {"WX4NM-KYWYW-QJJR4-XV3QB-6VM33", new Tuple<int, int, string>(8, 20348, "ServerDatacenter")},
     {"WMDGN-G9PQG-XVVXX-R3X43-63DFG", new Tuple<int, int, string>(8, 17763, "ServerDatacenter")},
     {"CB7KF-BWN84-R7R2Y-793K2-8XDDG", new Tuple<int, int, string>(8, 0, "ServerDatacenter")},
     {"QFND9-D3Y9C-J3KKY-6RPVP-2DPYV", new Tuple<int, int, string>(12, 20348, "ServerDatacenterCore")},
     {"6NMRW-2C8FM-D24W7-TQWMY-CWH2D", new Tuple<int, int, string>(12, 17763, "ServerDatacenterCore")},
     {"2HXDN-KRXHB-GPYC7-YCKFJ-7FVDG", new Tuple<int, int, string>(12, 0, "ServerDatacenterCore")},
     {"6N379-GGTMK-23C6M-XVVTC-CKFRQ", new Tuple<int, int, string>(52, 20348, "ServerSolution")},
     {"WVDHN-86M7X-466P6-VHXV7-YY726", new Tuple<int, int, string>(52, 17763, "ServerSolution")},
     {"FDNH6-VW9RW-BXPJ7-4XTYG-239TB", new Tuple<int, int, string>(52, 0, "ServerSolution")},
     {"TM8T6-9NJWV-PC26Y-RFYXH-YY723", new Tuple<int, int, string>(53, 0, "ServerSolutionCore")}
 };
    */
    private static Dictionary<string, string> SkuList = new Dictionary<string, string>()
   {
       {"1", "Ultimate"},
       {"2", "HomeBasic"},
       {"3", "HomePremium"},
       {"4", "Enterprise"},
       {"5", "HomeBasicN"},
       {"6", "Business"},
       {"7", "ServerStandard"},
       {"8", "ServerDatacenter"},
       {"9", "ServerSBSStandard"},
       {"10", "ServerEnterprise"},
       {"11", "Starter"},
       {"12", "ServerDatacenterCore"},
       {"13", "ServerStandardCore"},
       {"14", "ServerEnterpriseCore"},
       {"15", "ServerEnterpriseIA64"},
       {"16", "BusinessN"},
       {"17", "ServerWeb"},
       {"18", "ServerComputeCluster"},
       {"19", "ServerHomeStandard"},
       {"20", "ServerStorageExpress"},
       {"21", "ServerStorageStandard"},
       {"22", "ServerStorageWorkgroup"},
       {"23", "ServerStorageEnterprise"},
       {"24", "ServerWinSB"},
       {"25", "ServerSBSPremium"},
       {"26", "HomePremiumN"},
       {"27", "EnterpriseN"},
       {"28", "UltimateN"},
       {"29", "ServerWebCore"},
       {"30", "ServerMediumBusinessManagement"},
       {"31", "ServerMediumBusinessSecurity"},
       {"32", "ServerMediumBusinessMessaging"},
       {"33", "ServerWinFoundation"},
       {"34", "ServerHomePremium"},
       {"35", "ServerWinSBV"},
       {"36", "ServerStandardV"},
       {"37", "ServerDatacenterV"},
       {"38", "ServerEnterpriseV"},
       {"39", "ServerDatacenterVCore"},
       {"40", "ServerStandardVCore"},
       {"41", "ServerEnterpriseVCore"},
       {"42", "ServerHyperCore"},
       {"43", "ServerStorageExpressCore"},
       {"44", "ServerStorageStandardCore"},
       {"45", "ServerStorageWorkgroupCore"},
       {"46", "ServerStorageEnterpriseCore"},
       {"47", "StarterN"},
       {"48", "Professional"},
       {"49", "ProfessionalN"},
       {"50", "ServerSolution"},
       {"51", "ServerForSBSolutions"},
       {"52", "ServerSolutionsPremium"},
       {"53", "ServerSolutionsPremiumCore"},
       {"54", "ServerSolutionEM"},
       {"55", "ServerForSBSolutionsEM"},
       {"56", "ServerEmbeddedSolution"},
       {"57", "ServerEmbeddedSolutionCore"},
       {"58", "ProfessionalEmbedded"},
       {"59", "ServerEssentialManagement"},
       {"60", "ServerEssentialAdditional"},
       {"61", "ServerEssentialManagementSvc"},
       {"62", "ServerEssentialAdditionalSvc"},
       {"63", "ServerSBSPremiumCore"},
       {"64", "ServerHPC"},
       {"65", "Embedded"},
       {"66", "StarterE"},
       {"67", "HomeBasicE"},
       {"68", "HomePremiumE"},
       {"69", "ProfessionalE"},
       {"70", "EnterpriseE"},
       {"71", "UltimateE"},
       {"72", "EnterpriseEval"},
       {"74", "Prerelease"},
       {"76", "ServerMultiPointStandard"},
       {"77", "ServerMultiPointPremium"},
       {"79", "ServerStandardEval"},
       {"80", "ServerDatacenterEval"},
       {"84", "EnterpriseNEval"},
       {"85", "EmbeddedAutomotive"},
       {"86", "EmbeddedIndustryA"},
       {"87", "ThinPC"},
       {"88", "EmbeddedA"},
       {"89", "EmbeddedIndustry"},
       {"90", "EmbeddedE"},
       {"91", "EmbeddedIndustryE"},
       {"92", "EmbeddedIndustryAE"},
       {"93", "ProfessionalPlus"},
       {"95", "ServerStorageWorkgroupEval"},
       {"96", "ServerStorageStandardEval"},
       {"97", "CoreARM"},
       {"98", "CoreN"},
       {"99", "CoreCountrySpecific"},
       {"100", "CoreSingleLanguage"},
       {"101", "Core"},
       {"103", "ProfessionalWMC"},
       {"105", "EmbeddedIndustryEval"},
       {"106", "EmbeddedIndustryEEval"},
       {"107", "EmbeddedEval"},
       {"108", "EmbeddedEEval"},
       {"109", "ServerNano"},
       {"110", "ServerCloudStorage"},
       {"111", "CoreConnected"},
       {"112", "ProfessionalStudent"},
       {"113", "CoreConnectedN"},
       {"114", "ProfessionalStudentN"},
       {"115", "CoreConnectedSingleLanguage"},
       {"116", "CoreConnectedCountrySpecific"},
       {"117", "ConnectedCar"},
       {"118", "IndustryHandheld"},
       {"119", "PPIPro"},
       {"120", "ServerARM64"},
       {"121", "Education"},
       {"122", "EducationN"},
       {"123", "IoTUAP"},
       {"124", "ServerCloudHostInfrastructure"},
       {"125", "EnterpriseS"},
       {"126", "EnterpriseSN"},
       {"127", "ProfessionalS"},
       {"128", "ProfessionalSN"},
       {"129", "EnterpriseSEval"},
       {"130", "EnterpriseSNEval"},
       {"131", "IoTUAPCommercial"},
       {"133", "MobileEnterprise"},
       {"135", "Holographic"},
       {"136", "HolographicBusiness"},
       {"138", "ProfessionalSingleLanguage"},
       {"139", "ProfessionalCountrySpecific"},
       {"140", "EnterpriseSubscription"},
       {"141", "EnterpriseSubscriptionN"},
       {"143", "ServerDatacenterNano"},
       {"144", "ServerStandardNano"},
       {"145", "ServerDatacenterACor"},
       {"146", "ServerStandardACor"},
       {"147", "ServerDatacenterWSCor"},
       {"148", "ServerStandardWSCor"},
       {"149", "UtilityVM"},
       {"159", "ServerDatacenterEvalCor"},
       {"160", "ServerStandardEvalCor"},
       {"161", "ProfessionalWorkstation"},
       {"162", "ProfessionalWorkstationN"},
       {"164", "ProfessionalEducation"},
       {"165", "ProfessionalEducationN"},
       {"168", "ServerAzureCor"},
       {"169", "ServerAzureNano"},
       {"171", "EnterpriseG"},
       {"172", "EnterpriseGN"},
       {"175", "ServerRdsh"},
       {"178", "Cloud"},
       {"179", "CloudN"},
       {"180", "HubOS"},
       {"182", "OneCoreUpdateOS"},
       {"183", "CloudE"},
       {"184", "Andromeda"},
       {"185", "IoTOS"},
       {"186", "CloudEN"},
       {"187", "IoTEdgeOS"},
       {"188", "IoTEnterprise"},
       {"189", "Lite"},
       {"191", "IoTEnterpriseS"},
       {"202", "CloudEditionN"},
       {"203", "CloudEdition"},
       {"406", "ServerAzureStackHCICor"},
       {"407", "ServerTurbine"},
       {"408", "ServerTurbineCor"}
   };

    #endregion define


    private static readonly string slmgr = "/nologo " + "\"" + Environment.SystemDirectory + "\\slmgr.vbs"+"\"";
    public static void Main(string[] args)
    {
        //Console.WriteLine(Environment.SystemDirectory);
        Activation();
        
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
    private static void Activation()
    {
        Wow64DisableWow64FsRedirection(false);
        string VersionString = GetOsVersion();
        if (Version.Parse(NativeOsVersion().ToString()).Major < 10)
        {
            Console.WriteLine("当前系统不受支持，仅适用于Windows 10及以上操作系统");
            return;
        }
         
        RegistryKey skipx = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform", true);
        if (skipx != null)
        {
            skipx.SetValue("SkipRearm", 1, RegistryValueKind.DWord);
            skipx.SetValue("ClipSvcStart", 1,RegistryValueKind.DWord);

        }
         
        using (ServiceController sc = new("sppsvc"))
        {
             
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, new(0, 1, 0));
                Console.WriteLine("正在停止服务");
            } 
            sc.Close();
        };
 
         if(Directory.Exists(Environment.SystemDirectory + @"\spp\store\2.0\cache") == false)
        {
            Directory.CreateDirectory(Environment.SystemDirectory + @"\spp\store\2.0\cache");
        }
        AddSecurityControll2Folder(Environment.SystemDirectory + @"\spp");
        AddSecurityControll2Folder(Environment.SystemDirectory + @"\spp\store");
        AddSecurityControll2Folder(Environment.SystemDirectory + @"\spp\store\2.0");
        AddSecurityControll2Folder(Environment.SystemDirectory + @"\spp\store\2.0\cache");

        var tokensdat = Environment.SystemDirectory + @"\spp\store\2.0\tokens.dat";
        var datadat = Environment.SystemDirectory + @"\spp\store\2.0\data.dat";
        var cachedat = Environment.SystemDirectory + @"\spp\store\2.0\cache\cache.dat";

        if (File.Exists(tokensdat))
        {
            AddSecurityControll2File(tokensdat);
            File.Delete(tokensdat);
            Console.WriteLine("删除" + tokensdat);
        }
         
        if (File.Exists(datadat))
        {
            AddSecurityControll2File(datadat);
            File.Delete(datadat);
            Console.WriteLine("删除" + datadat);
        }
         
        if (File.Exists(cachedat))
        {
            AddSecurityControll2File(cachedat);
            File.Delete(cachedat);
            Console.WriteLine("删除" + cachedat);
        }
       
        using (ServiceController sc = new("sppsvc"))
        {
            
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running,new(0,1,0));
                Console.WriteLine("正在启动服务");
            } 
            sc.Close();
        };
        CScript(slmgr + " -upk");
        CScript(slmgr + " -cpky");
        Console.WriteLine("正在卸载密钥");
        CScript(slmgr + " -ckms");
        Console.WriteLine("正在清除KMS");
        CScript(slmgr + " -rearm");
        Console.WriteLine("正在重置授权状态");

        byte[] DST = HWID.HwidGetCurrentEx();
        string Base64String1 = HWID.HwidCreateBlock(DST, DST[0]);
        string SessionId = VersionString + ";" + "Hwid=" + Base64String1 + ";";
        Console.WriteLine("正在收集硬件信息");
        string EditionID = "";
       
        Console.WriteLine("正在设置系统授权到升级模式");
        int SKU = 0;
        RegistryKey regkey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, (Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32)).OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", true);
        if (regkey != null)
        {
            string EditionName = regkey.GetValue("EditionID").ToString();
            SKU = int.Parse(SkuList.Where((x) => x.Value.ToString() == EditionName).Select((y) => y.Key.ToString()).ToList()[0]);
            EditionID = EditionName;
        }
        RegistryKey TokenRootRegKey = Registry.LocalMachine.CreateSubKey(@"SYSTEM\Tokens", true);
        TokenRootRegKey.SetValue("Channel", "Retail", RegistryValueKind.String);
        RegistryKey subKey = TokenRootRegKey.CreateSubKey("Kernel", true);
        subKey.SetValue("Kernel-ProductInfo", SKU, RegistryValueKind.DWord);
        subKey.SetValue("Security-SPP-GenuineLocalStatus", 1, RegistryValueKind.DWord);
        //int szRes = SLGetWindowsInformationDWORD("Kernel-ProductInfo", ref EditionID);
        //if (szRes != 0)
        //{
        //    EditionID = SKU;
        //}
         
        /*
        if (KMS38)
        {
            string ProductKeys = KMSProductKey.Where((x) => x.Value.Item1 == EditionID && NativeOsVersion().Build >= x.Value.Item2).Select((y) => y.Key).ToList()[0];
            if (string.IsNullOrEmpty(ProductKeys)) return;
            SessionId = SessionId + "GVLKExp=" + FileTime2SystemTime() + ";DownlevelGenuineState=1;";
        }
         */
        string ProductKeys = UpdateProductKey.Where((x) => x.Value.Item1 == SKU && NativeOsVersion().Build >= x.Value.Item2).Select((y) => y.Key).ToList()[0];
        
        //Console.WriteLine(ProductKeys);
        if (string.IsNullOrEmpty(ProductKeys))
        {
            Console.WriteLine("当前系统不在支持范围内，正在退出");
            return;
        }
        CScript(slmgr + " -ipk " + ProductKeys);
        Console.WriteLine("正在安装密钥:"+ ProductKeys);
        string pfn = UpdateProductKey[ProductKeys].Item4;
        SessionId = SessionId + "Pfn=" + pfn + ";DownlevelGenuineState=1;";

        byte[] bytesessionid = System.Text.Encoding.Unicode.GetBytes(SessionId);
        bytesessionid = bytesessionid.Concat(new byte[] { 0, 0 }).ToArray();
        string base64string2 = Convert.ToBase64String(bytesessionid);
        SessionId = "SessionId=" + base64string2 + ";" + UtcTimeToIso8601();
        //SessionId = "SessionId=TwBTAE0AYQBqAG8AcgBWAGUAcgBzAGkAbwBuAD0AMQAwADsATwBTAE0AaQBuAG8AcgBWAGUAcgBzAGkAbwBuAD0AMAA7AE8AUwBQAGwAYQB0AGYAbwByAG0ASQBkAD0AMgA7AFAAUAA9ADAAOwBIAHcAaQBkAD0AYgBRAEEAQQBBAEIATQBBAFEAZwBBAEEAQQBBAEEAQQBBAFEAQQBDAEEAQQBJAEEAQQB3AEEARQBBAEEAQQBBAEIAZwBBAEIAQQBBAEUAQQBhAEwANwA4AEcAWQBJAFoAZwBqAEsAbwArAEoAbwBZAGQAaQB1AFEATgBnADcAVABHAE4AYQB3AFcAaABlAE4ARABvADUAZgA4AGEAUQBPADMAaQA1AGMAVQBKAHAAYwBwAEcARABXADgAUgBEAHYARABBAEEAQwBBAEEARQBCAEEAQQBJAEYAQQBBAE0AQgBBAEEAUQBDAEEAQQBZAEIAQQBBAGcASABBAEEAawBEAEEAQQBvAEIAQQBBAHcASABBAEEAQQBBAEEAQQBBAEEAQQBBAD0APQA7AFAAZgBuAD0ATQBpAGMAcgBvAHMAbwBmAHQALgBXAGkAbgBkAG8AdwBzAC4ANAA4AC4AWAAxADkALQA5ADgAOAA0ADEAXwA4AHcAZQBrAHkAYgAzAGQAOABiAGIAdwBlADsARABvAHcAbgBsAGUAdgBlAGwARwBlAG4AdQBpAG4AZQBTAHQAYQB0AGUAPQAxADsAAAA=;TimeStampClient=2022-05-05T00:00:00Z";
        byte[] hashArray = ComputeHashEx(SessionId);
        //Debug.Print(hashArray.Length.ToString() + Environment.NewLine + BitConverter.ToString(hashArray).Replace("-", " "));
        byte[] Dst = VRSA.SignPKCS(hashArray);
        Console.WriteLine("正在签发数字许可证");
        if (Dst != null)
        {
            string base64string3 = System.Convert.ToBase64String(Dst);
            var tokenxml = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\tokens.xrm-ms";
            SaveData(SessionId, base64string3, tokenxml);
            Console.WriteLine("保存数字许可证到"+ tokenxml);
            //SaveData(SessionId, base64string3, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microsoft\\Windows\\ClipSVC\\GenuineTicket\\GenuineTicket.xml");
            Run("clipup.exe", "-v -o -altto " + @"""" + tokenxml + @"""");
            Console.WriteLine("正在安装数字许可证");
            CScript(slmgr + " -ato");
            Console.WriteLine("正在更新授权状态");
            Registry.LocalMachine.DeleteSubKeyTree(@"SYSTEM\Tokens", false);
            Console.WriteLine("请重新启动系统，以完成激活");
        }
        else
        {
            Console.WriteLine("签发许可证失败");
        } 
    }

    #region function
    /// <summary>
    /// 为文件添加users，everyone用户组的完全控制权限
    /// </summary>
    /// <param name="filePath"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
    private  static void AddSecurityControll2File(string filePath)
    {
        try
        {
            //获取文件信息
            FileInfo fileInfo = new(filePath);
            //获得该文件的访问权限
            System.Security.AccessControl.FileSecurity fileSecurity = fileInfo.GetAccessControl();
            //添加ereryone用户组的访问权限规则 完全控制权限
            //fileSecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
            //添加Users用户组的访问权限规则 完全控制权限
            //fileSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
            //sppsvc
            fileSecurity.AddAccessRule(new FileSystemAccessRule("sppsvc", FileSystemRights.FullControl, AccessControlType.Allow));

            //设置访问权限
            fileInfo.SetAccessControl(fileSecurity);
        }
        catch { }
    }


    /// <summary>
    ///为文件夹添加users，everyone用户组的完全控制权限
    /// </summary>
    /// <param name="dirPath"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:验证平台兼容性", Justification = "<挂起>")]
    private static void AddSecurityControll2Folder(string dirPath)
    {
        try
        {
            //获取文件夹信息
            DirectoryInfo dir = new(dirPath);
            //获得该文件夹的所有访问权限
            DirectorySecurity dirSecurity = dir.GetAccessControl(AccessControlSections.All);
            //设定文件ACL继承
            InheritanceFlags inherits = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;

            //添加ereryone用户组的访问权限规则 完全控制权限
            FileSystemAccessRule everyoneFileSystemAccessRule = new("Everyone", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            //添加Users用户组的访问权限规则 完全控制权限
            FileSystemAccessRule usersFileSystemAccessRule = new("Users", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            //sppsvc
            FileSystemAccessRule NTSVCFileSystemAccessRule = new("sppsvc", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);


            bool isModified = false;
            //dirSecurity.ModifyAccessRule(AccessControlModification.Add, everyoneFileSystemAccessRule, out isModified);
            //dirSecurity.ModifyAccessRule(AccessControlModification.Add, usersFileSystemAccessRule, out isModified);
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, NTSVCFileSystemAccessRule, out isModified);
            //设置访问权限
            dir.SetAccessControl(dirSecurity);
        }
        catch { }
    }
    private static string CScript(string var)
    {
             
        try
        {
            Process process = new();
            process.StartInfo.FileName = "CScript.exe";
            process.StartInfo.Arguments = var;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            process.Start();
            process.WaitForExit(60000);
            string end = process.StandardOutput.ReadToEnd();
            process.Close();
            
            return end.Trim();
        }
        catch
        {
            return string.Empty;
        }
    }
    private static string Run(string path, string var = "")
    {

        try
        {
            Process process = new();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = var;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            process.Start();
            process.WaitForExit(60000);
            string end = process.StandardOutput.ReadToEnd();
            process.Close();

            return end.Trim();
        }
        catch
        {
            return string.Empty;
        }
    }
    private static void SaveData(string SessionId, string Base64String3, string szPath)
    {
        if (string.IsNullOrEmpty(Base64String3) || string.IsNullOrEmpty(SessionId))
        {
            return;
        }
        string pubkey = "BgIAAACkAABSU0ExAAgAAAEAAQARq+V11k+dvHMCaLWVCaSbeQNlOdWTLkkl0hdMh5V3YhLU2R4h0Jd+7k7qfZ4aIo4ussduwGgmyDRikj5L2R77GG2ciHk4i8siK8qg7frOU0KT5rEks3qVj38C3dS1wS6D67shBFrxPlOEP8+JlelgP7Gxmwdao7NF4LXZ3+KdbJ//9jkmN8iAOP0N2XzW0/cJp9P1q6hE7eeqc/3Qn3zMr0q1Dx7vstN98oV17hNYCwumOxxS1rH+3n7ap2JKRSelo8Jvi214jZLBL+hOtYaGpxs7zIL3ofpoaYy5g7pc/DaTvyfpJho5634jK7dXVFMpzJZMn9w0F/3rkquk0Amm";
        using (XmlTextWriter writer = new(szPath, Encoding.UTF8))
        {
            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 4;
            writer.WriteStartElement("genuineAuthorization", "http://www.microsoft.com/DRM/SL/GenuineAuthorization/1.0");
            writer.WriteStartElement("version");
            writer.WriteString("1.0");
            writer.WriteEndElement();
            writer.WriteStartElement("genuineProperties");
            writer.WriteAttributeString("origin", "sppclient");
            writer.WriteStartElement("properties");
            writer.WriteString("SessionId=" + SessionId);
            writer.WriteEndElement();
            writer.WriteStartElement("signatures");
            writer.WriteStartElement("signature");
            writer.WriteAttributeString("name", "downlevelGTkey");
            writer.WriteAttributeString("method", "rsa-sha256");
            writer.WriteAttributeString("key", pubkey);
            writer.WriteString(Base64String3);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }

    private static string GetOsVersion()
    {
        RTL_OSVERSIONINFOEX osVersionInfo = new RTL_OSVERSIONINFOEX();
        osVersionInfo.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osVersionInfo);
        int status = RtlGetVersion(out osVersionInfo);
        if (status != 0)
        {
            return "";
        }
        return "OSMajorVersion=" + osVersionInfo.dwMajorVersion + ";OSMinorVersion=" + osVersionInfo.dwMinorVersion + ";OSPlatformId=" + osVersionInfo.dwPlatformId + ";PP=0";
    }

    private static Version NativeOsVersion()
    {
        RTL_OSVERSIONINFOEX osVersionInfo = new RTL_OSVERSIONINFOEX();
        osVersionInfo.dwOSVersionInfoSize = (uint)Marshal.SizeOf(typeof(RTL_OSVERSIONINFOEX));
        int status = RtlGetVersion(out osVersionInfo);
        if (status != 0)
        {
            return Environment.OSVersion.Version;
        }
        return new Version((int)osVersionInfo.dwMajorVersion, (int)osVersionInfo.dwMinorVersion, (int)osVersionInfo.dwBuildNumber);
    }

    private static string FileTime2SystemTime()
    {
        FILE_TIME SystemTimeAsFileTime = new FILE_TIME();
        GetSystemTimeAsFileTime(ref SystemTimeAsFileTime);
        SYSTEMTIME SysTime = new SYSTEMTIME();
        FILE_TIME filetime = new FILE_TIME();
        filetime.dwLowDateTime = SystemTimeAsFileTime.dwLowDateTime;
        if (FileTimeToSystemTime(ref filetime, out SysTime))
        {
            var timestampclient = SysTime.wYear.ToString("0000") + "-" + SysTime.wMonth.ToString("00") + "-" + SysTime.wDay.ToString("00") + "T" + SysTime.wHour.ToString("00") + ":" + SysTime.wMinute.ToString("00") + ":" + SysTime.wSecond.ToString("00") + "Z";
            return timestampclient.Replace(timestampclient.Substring(0, 10), "2038-01-19");
        }
        return "";
    }

    private static string UtcTimeToIso8601()
    {
        string timestampclient = "";
        SYSTEMTIME SysTime = new SYSTEMTIME();
        GetSystemTime(ref SysTime);
        //timestampclient = SysTime.wYear.ToString("0000") + "-" + SysTime.wMonth.ToString("00") + "-" + SysTime.wDay.ToString("00") + "T" + SysTime.wHour.ToString("00") + ":" + SysTime.wMinute.ToString("00") + ":" + SysTime.wSecond.ToString("00") + "Z";
        timestampclient = SysTime.wYear.ToString("0000") + "-" + SysTime.wMonth.ToString("00") + "-" + SysTime.wDay.ToString("00") + "T" + "00:00:00Z";
        if (string.IsNullOrEmpty(timestampclient))
        {
            return string.Empty;
        }
        return "TimeStampClient=" + timestampclient;
    }

    private static byte[] ComputeHashEx(string szSessionId)
    {
        IntPtr phProv = IntPtr.Zero;
        if (CryptAcquireContextW(out phProv, null, "Microsoft Enhanced RSA and AES Cryptographic Provider", 0x18, 0xF0000020))
        {
            byte[] pbDatas = new byte[4];
            byte[] pNewData = ComputeHash(phProv, szSessionId, pbDatas);
            pbDatas = new byte[pNewData[0]];
            byte[] pbNewData = ComputeHash(phProv, szSessionId, pbDatas);
            return pbNewData;
        }
        return null;
    }

    private static byte[] ComputeHash(IntPtr phProv, string SessionId, byte[] pbDatas)
    {
        byte[] ReturnBytes = null;
        IntPtr hHash = new IntPtr();
        byte[] pbData = new byte[4];
        if (CryptCreateHash(phProv, (uint)ALG_ID.CALG_RSA, IntPtr.Zero, 0, ref hHash))
        {
            uint pdwDataLen = 4;
            if (CryptGetHashParam(hHash, 4, pbData, ref pdwDataLen, 0))
            {
                if (!(pbDatas.Length == 4))
                {
                    byte[] pbBuffer = Encoding.ASCII.GetBytes(SessionId);
                    if (CryptHashData(hHash, pbBuffer, (uint)pbBuffer.Length, 0))
                    {
                        pdwDataLen = (uint)pbDatas.Length;
                        if (CryptGetHashParam(hHash, (uint)HashParameters.HP_HASHVAL, pbDatas, ref pdwDataLen, 0))
                        {
                            ReturnBytes = pbDatas;
                        }
                    }
                }
                else
                {
                    ReturnBytes = pbData;
                }
            }
        }
        CryptDestroyHash(hHash);
        return ReturnBytes;
    }

    #endregion function
}