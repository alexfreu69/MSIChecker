using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using WindowsInstaller;
using System.Runtime.InteropServices;


namespace MSIChecker
{
    public partial class Form1 : Form
    {
        const string subkey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Installer\UserData";

        #region Externe Deklarationen       

        [StructLayout(LayoutKind.Sequential)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        [Flags]
        public enum ShellExecuteMaskFlags : uint
        {
            SEE_MASK_DEFAULT = 0x00000000,
            SEE_MASK_CLASSNAME = 0x00000001,
            SEE_MASK_CLASSKEY = 0x00000003,
            SEE_MASK_IDLIST = 0x00000004,
            SEE_MASK_INVOKEIDLIST = 0x0000000c,   // Note SEE_MASK_INVOKEIDLIST(0xC) implies SEE_MASK_IDLIST(0x04) 
            SEE_MASK_HOTKEY = 0x00000020,
            SEE_MASK_NOCLOSEPROCESS = 0x00000040,
            SEE_MASK_CONNECTNETDRV = 0x00000080,
            SEE_MASK_NOASYNC = 0x00000100,
            SEE_MASK_FLAG_DDEWAIT = SEE_MASK_NOASYNC,
            SEE_MASK_DOENVSUBST = 0x00000200,
            SEE_MASK_FLAG_NO_UI = 0x00000400,
            SEE_MASK_UNICODE = 0x00004000,
            SEE_MASK_NO_CONSOLE = 0x00008000,
            SEE_MASK_ASYNCOK = 0x00100000,
            SEE_MASK_HMONITOR = 0x00200000,
            SEE_MASK_NOZONECHECKS = 0x00800000,
            SEE_MASK_NOQUERYCLASSSTORE = 0x01000000,
            SEE_MASK_WAITFORINPUTIDLE = 0x02000000,
            SEE_MASK_FLAG_LOG_USAGE = 0x04000000,
        }

        [DllImport("shell32.dll", EntryPoint = "ShellExecute")]
        public static extern long ShellExecute(int hwnd, string cmd, string file, string param1, string param2, int swmode);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

        #endregion

        long iSumSize;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            FillGrid();
            
        }

        private void FillGrid()
        {

            List<string> aFiles = new List<string>();
            grid.Rows.Clear();
            iSumSize = 0;
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(subkey))
            {
                foreach (string user in rk.GetSubKeyNames())
                {
                    foreach (string product in rk.OpenSubKey(user+@"\Products").GetSubKeyNames())
                    {
                        string sProduct, sGUID, sFile;
                        sGUID = KeyName2GUID(product);
                        sProduct = GetProductName(user, product);
                        sFile = GetMSIFileName(user, product);
                        long iSize = GetSize(sFile);
                        iSumSize += iSize;
                        grid.Rows.Add(new object[] { sProduct, "MSI", product, sGUID, sFile, iSize,GetStatus(sFile),user});
                        aFiles.Add(sFile); 

                        try
                        {
                            foreach (string patch in rk.OpenSubKey(user + @"\Products\" + product + @"\Patches").GetSubKeyNames())
                            {
                                string sPatch, sPatchGUID, sPatchFile;
                                int iUninstallable;
                                sPatch = GetPatchDisplayName(user, product, patch);
                                sPatchGUID = KeyName2GUID(patch);
                                sPatchFile = GetMSPFileName(user, patch);
                                try
                                {
                                    iUninstallable = (int)Registry.GetValue(@"HKEY_LOCAL_MACHINE\" + subkey + @"\" + user + @"\Products\" + product + @"\Patches\" + patch, "Uninstallable", 0);
                                }
                                catch (Exception)
                                {
                                    iUninstallable = 0;
                                }

                                if (!aFiles.Contains(sPatchFile))
                                {
                                    aFiles.Add(sPatchFile);
                                    long iPatchSize = GetSize(sPatchFile);
                                    iSumSize += iPatchSize;
                                    grid.Rows.Add(new object[] { sPatch, "MSP", patch, sPatchGUID, sPatchFile, iPatchSize, GetStatus(sPatchFile),user,sGUID, iUninstallable });
                                }
                            }

                        }
                        catch (Exception) { }
                    }

                }
            }

            ScanInstallerFiles(aFiles);
            lblSize.Text = iSumSize.ToString("N0");

            SortOrder so = grid.SortOrder;
            DataGridViewColumn col = grid.SortedColumn;
            if (col != null)
            {
                ListSortDirection lsd = ListSortDirection.Ascending;
                if (so.Equals(SortOrder.Descending))
                {
                    lsd = ListSortDirection.Descending;
                }
                grid.Sort(col, lsd);
            }
        }

        private void ScanInstallerFiles(List<string> lstRegistry)
        {
            const string sInstaller= @"C:\Windows\Installer\";
            // .var list3 = list1.Except(list2).ToList();
            List<string> lstFound=System.IO.Directory.EnumerateFiles(sInstaller, "*.ms?").ToList().ConvertAll(d => d.ToLower());
            foreach (string itm in lstFound.Except(lstRegistry))
            {
                string sGUID = null;
                string sProduct = null;
                string sType = System.IO.Path.GetExtension(itm).ToUpper().Remove(0, 1);
                if (sType == "MSI")
                {
                    sGUID = GetProductGUIDFromMSI(itm,out sProduct);

                }
                else
                {
                    sGUID = GetPatchGUIDFromMSP(itm);
                }
                long iSize = GetSize(itm);
                iSumSize += iSize;
                grid.Rows.Add(new object[] { sProduct, sType, GUID2KeyName(sGUID), sGUID, itm, iSize, "#ORPHANED#","" });
            }
        }

        private long GetSize(string itm)
        {
            if (itm != null && itm != "")
            {
                try
                {
                    return (long)new System.IO.FileInfo(itm).Length;
                }
                catch (Exception)
                {
                    return 0;
                }
                
            }
            else
            {
                return 0;
            }
            
        }

        private string GetProductNameFromMSI(string itm)
        {
            throw new NotImplementedException();
        }

        private string GetPatchGUIDFromMSP(string itm)
        {
            /*
             PatchXMLData = MsiExtractPatchXMLData(PatchPath)

            Dim xmlMsiPatch
            Set xmlMsiPatch = LoadXML(PatchXMLData)

            Dim PatchCode
            PatchCode = xmlMsiPatch.Attributes.GetNamedItem("PatchGUID").Text

            GetPatchGUIDFromMSP = PatchCode

            Dim ProductCode
            ProductCode = xmlMsiPatch.getElementsByTagName("TargetProductCode").item(0).text

            Function MsiExtractPatchXMLData(szPatchPath)
	        Dim Installer
	        Set Installer = CreateObject("WindowsInstaller.Installer")
	        Dim szXMLData
	        szXMLData =Installer.ExtractPatchXMLData(szPatchPath)

	        MsiExtractPatchXMLData = szXMLData
            End Function

            Function LoadXML(XmlData)
	        Dim xmlDoc : Set xmlDoc = CreateObject("Microsoft.XMLDOM")
	        xmlDoc.async = False

	        If Not xmlDoc.LoadXml(XmlData) Then
		        Dim oErr : Set oErr = xmlDoc.parseError
		        Dim sErrMsg : sErrMsg = "XML Parsing Error. File: " & oErr.url & " Reason : " & oErr.reason & _
					        " Line: " & oErr.line & ", Character: " & _
					        oErr.linepos & ", Text: " & oErr.srcText
		        WScript.Echo "[" & Time & "]: " & sErrMsg
		        WScript.Quit
	        End If

	        Set LoadXML = xmlDoc.documentElement
            End Function

            ProductCode = xmlMsiPatch.getElementsByTagName("TargetProductCode").item(0).text
            */

            Type classType = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            Object installerClassObject = Activator.CreateInstance(classType);
            Installer installer = (Installer)installerClassObject;
            try
            {
                string PatchXMLData = installer.ExtractPatchXMLData(itm);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(PatchXMLData);
                // MessageBox.Show(doc.GetElementsByTagName("TargetProductCode").Item(0).Value);
                // MessageBox.Show(PatchXMLData);
                return doc.DocumentElement.Attributes.GetNamedItem("PatchGUID").Value;
            }
            catch (Exception ex)
            {
                return "#"+ex.Message+"#";
            }

        }

        private string QueryDB(Database msiDatabase, string strProperty)
                    {
                        string result = null;
                        /*
                        string sql = "SELECT Property, Value FROM Property";
                        WindowsInstaller.View view = msiDatabase.OpenView(sql);
                        view.Execute(null);

                        Record record = view.Fetch();
                        do
                        {
                            String value = record.get_StringData(1);
                            if (String.Compare(value, strProperty, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                result = record.get_StringData(2);
                            }
                            record = view.Fetch();
                        } while (record != null);
                        */
            string sql = "SELECT Value FROM Property WHERE Property = '" + strProperty + "'";
            WindowsInstaller.View view = msiDatabase.OpenView(sql);
            view.Execute(null);
            Record record = view.Fetch();
            if (record != null)
            {
                result = record.get_StringData(1);
            }

            view.Close();
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(view);
            return result;
        }

        private string GetProductGUIDFromMSI(string itm, out string sName)
        {
            /*
             CONST MSIDBOPEN_READONLY=0

            On Error Resume Next
            GetMSIProductCode=""
            If msifile = "" Then Exit Function

            Dim FS, TS, DB, View, Rec

            err.Clear
            Set DB = msi.OpenDatabase(msifile,MSIDBOPEN_READONLY)
            If Err.number <> 0 Then 
               GetProductGUIDFromMSI=CheckError
               Exit Function
            End If

            Set View = DB.OpenView("Select `Value` From Property WHERE `Property` ='ProductCode'")
            View.Execute
            Set Rec = View.Fetch
            If Not Rec Is Nothing Then
             GetProductGUIDFromMSI=Rec.StringData(1)
            End If
            */
            //const int MSIDBOPEN_READONLY = 0;
            if (itm == null)
            {
                sName = "";
                return "";
            }

            Type classType = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            Object installerClassObject = Activator.CreateInstance(classType);
            Installer installer = (Installer)installerClassObject;
            Database msiDatabase;
            try
            {
                msiDatabase = installer.OpenDatabase(itm, MsiOpenDatabaseMode.msiOpenDatabaseModeReadOnly);
                string s = QueryDB(msiDatabase, "ProductCode");
                sName = QueryDB(msiDatabase, "ProductName");
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(msiDatabase);
                return s;
            }
            catch (Exception)
            {
                sName = "#MSIERROR#";
                return "#MSIERROR#";
            }
            

        }

        private string GetPatchDisplayName(string user, string product, string patch)
        {
            try
            {
                return (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\" + subkey + @"\" + user + @"\Products\" + product + @"\Patches\" + patch, "DisplayName", "");
            }
            catch (Exception)
            {
                return "#ERROR#";
            }
        }

        private string GetMSPFileName(string user, string patch)
        {
            try
            {
                string s= (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\" + subkey + @"\" + user + @"\Patches\" + patch, "LocalPackage", "#MISSING#");
                if (s == null)
                {
                    s = "";
                }
                return s.ToLower();
            }
            catch (Exception)
            {
                return "#ERROR#";
            }
        }

        private string GetStatus(string sFile)
        {
            if (sFile == null)
            {
                return "#INVALID#";
            }

            if (System.IO.File.Exists(sFile))
            {
                return "OK";
            }
            else
            {
                return "#MISSING#";
            }
        }

        private string GetMSIFileName(string user, string product)
        {
            try
            {
                string s = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\" + subkey + @"\" + user + @"\Products\" + product + @"\InstallProperties", "LocalPackage", "#MISSING#");
                if (s == null)
                {
                    s = "";
                }
                return s.ToLower();
            }
            catch (Exception)
            {
                return "#ERROR#";
            }
        }

        private string GetProductName(string user, string product)
        {
            try
            {
                return (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\" + subkey + @"\" + user + @"\Products\" + product+ @"\InstallProperties", "DisplayName", "#MISSING#");
            }
            catch (Exception)
            {
                return "#ERROR#";
            }
        }

        private string KeyName2GUID(string product)
        {
            if (product.Length != 32)
            {
                return product;
            }
            else
            {
                string sRemain = RevertString(product.Substring(20, 2)) + RevertString(product.Substring(22, 2)) + RevertString(product.Substring(24, 2)) + RevertString(product.Substring(26, 2)) + RevertString(product.Substring(28, 2)) + RevertString(product.Substring(30, 2));
                return "{" + RevertString(product.Substring(0,8)) + "-" + RevertString(product.Substring(8, 4)) + "-" + RevertString(product.Substring(12, 4)) + "-" + RevertString(product.Substring(16, 2)) + RevertString(product.Substring(18, 2)) + "-" + sRemain + "}";
            }
        }

        private string GUID2KeyName(string product)
        {
            if (product.Length != 38)
            {
                return product;
            }
            else
            {
                string s = RevertString(product.Substring(1, 8));
                s += RevertString(product.Substring(10, 4));
                s += RevertString(product.Substring(15, 4));
                s += RevertString(product.Substring(20, 2));
                s += RevertString(product.Substring(22, 2));
                s += RevertString(product.Substring(25, 2));
                s += RevertString(product.Substring(27, 2));
                s += RevertString(product.Substring(29, 2));
                s += RevertString(product.Substring(31, 2));
                s += RevertString(product.Substring(33, 2));
                s += RevertString(product.Substring(35, 2));
                return s;
            }
        }

        public string RevertString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private void itmSearch_Click(object sender, EventArgs e)
        {
            string sURL=(string)grid.SelectedCells[3].Value;
            Process.Start("https://www.google.de/search?q=" + sURL);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            grid.Columns[5].ValueType = typeof(long);
            grid.Columns[9].ValueType = typeof(int);
        }

        private void itmProperties_Click(object sender, EventArgs e)
        {
            string sFile = (string)grid.SelectedCells[4].Value;
            //long ret=ShellExecute(0, "properties", sFile, "", "", 5);
            //MessageBox.Show(ret.ToString());
            if (sFile=="")
            {
                return;
            }
            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = sFile;
            info.nShow = (int)ShowCommands.SW_SHOW;
            info.fMask = (uint)ShellExecuteMaskFlags.SEE_MASK_INVOKEIDLIST;
            ShellExecuteEx(ref info);
        }

        private void itmUninstall_Click(object sender, EventArgs e)
        {
            string sGUID = (string)grid.SelectedCells[3].Value;
            string sParentGUID = (string)grid.SelectedCells[8].Value;
            Process pid;
            if (sGUID != "")
            {
                switch ((string)grid.SelectedCells[1].Value)
                {
                    case "MSI":
                        pid = Process.Start(System.IO.Path.Combine(Environment.SystemDirectory, "msiexec.exe"), @"/X" + sGUID);
                        pid.WaitForExit();
                        break;
                    case "MSP":
                        if ((int)grid.SelectedCells[9].Value == 1)
                        {
                            pid = Process.Start(System.IO.Path.Combine(Environment.SystemDirectory, "msiexec.exe"), @"/package " + sParentGUID +@" /uninstall " + sGUID);
                            pid.WaitForExit();
                        }
                        break;
                    default:
                        break;
                }
                FillGrid();


            }
            
        }

        private void itmRemove_Click(object sender, EventArgs e)
        {
            string sFile = (string)grid.SelectedCells[4].Value;
            if (sFile != "")
            {
                try
                {
                    System.IO.File.Delete(sFile);
                    FillGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                 }
            }
        }

        private void itmRegistry_Click(object sender, EventArgs e)
        {
            string sReg = (string)grid.SelectedCells[2].Value;
            if (sReg != "")
            {
                string sType = (string)grid.SelectedCells[1].Value;
                string sUser = (string)grid.SelectedCells[7].Value;
                if (sType=="MSI")
                {
                    try
                    {
                        Registry.LocalMachine.DeleteSubKeyTree(subkey + @"\" + sUser +@"\Products\" + sReg);
                        FillGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
