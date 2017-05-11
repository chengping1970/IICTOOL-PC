using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace IICTool
{
    public partial class I2CTool : Form
    {
        [DllImport("hid.dll")]
        public static extern void HidD_GetHidGuid(ref Guid HidGuid);
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, uint Enumerator, IntPtr HwndParent, DIGCF Flags);
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, UInt32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid interfaceClassGuid;
            public int flags;
            public int reserved;
        }
        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData,
            int deviceInterfaceDetailDataSize, ref int requiredSize, SP_DEVINFO_DATA deviceInfoData);
        [StructLayout(LayoutKind.Sequential)]
        public class SP_DEVINFO_DATA
        {
            public int cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
            public Guid classGuid = Guid.Empty; // temp
            public int devInst = 0; // dumy
            public int reserved = 0;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            internal int cbSize;
            internal short devicePath;
        }
        public enum DIGCF
        {
            DIGCF_DEFAULT = 0x1,
            DIGCF_PRESENT = 0x2,
            DIGCF_ALLCLASSES = 0x4,
            DIGCF_PROFILE = 0x8,
            DIGCF_DEVICEINTERFACE = 0x10
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int CreateFile(
            string lpFileName,                            // file name
            uint dwDesiredAccess,                        // access mode
            uint dwShareMode,                            // share mode
            uint lpSecurityAttributes,                    // SD
            uint dwCreationDisposition,                    // how to create
            uint dwFlagsAndAttributes,                    // file attributes
            uint hTemplateFile                            // handle to template file
            );
        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern bool ReadFile
            (
                int hFile,
                byte[] lpBuffer,
                uint nNumberOfBytesToRead,
                ref uint lpNumberOfBytesRead,
                IntPtr lpOverlapped
            );
        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern bool WriteFile
            (
                int hFile,
                byte[] lpBuffer,
                uint nNumberOfBytesToRead,
                ref uint lpNumberOfBytesRead,
                IntPtr lpOverlapped
            );
        [DllImport("kernel32.dll")]
        static public extern int CloseHandle(int hObject);
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const int OPEN_EXISTING = 3;
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern IntPtr SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);
        [DllImport("hid.dll")]
        private static extern Boolean HidD_GetAttributes(int hObject, out HID_ATTRIBUTES attributes);
        public struct HID_ATTRIBUTES
        {
            public int Size;
            public ushort VendorID;
            public ushort ProductID;
            public ushort VersionNumber;
        }

        public string HIDdevicePathName;

        public bool DisplayMessage = false;
        public SerialPort SelectUart;
        public byte[] SendBuffer = new byte[256 + 64];
        public byte[] ReceiveBuffer = new byte[256 + 64];
        SynchronizationContext _syncContext = null;

        public I2CTool()
        {
            InitializeComponent();
            this.Width = this.Width - 250;
            button13.Text = ">>>";

            SelectUart = new SerialPort();
            RegData.Rows.Add(new object[] { ("00").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("10").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("20").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("30").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("40").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("50").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("60").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("70").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("80").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("90").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("A0").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("B0").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("C0").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("D0").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("E0").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.Rows.Add(new object[] { ("F0").ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" });
            RegData.EnableHeadersVisualStyles = false;
            RegData.Columns[0].DefaultCellStyle.BackColor = Color.Yellow;
            RegData.CurrentCell = RegData[1, 0];
            SlaveAddress.Text = "BA";
            SUBAddress.Text = "00";
            RegValue.Text = "00";
            textBox3.Text = "00";
            textBox4.Text = "100";
            RegData.SelectionChanged += new EventHandler(RegData_SelectionChanged);
            Thread ScanDevice = new Thread(new ThreadStart(ScandeviceThread));
            ScanDevice.IsBackground = true;
            ScanDevice.Start();
            _syncContext = SynchronizationContext.Current;
        }


        private void comboBox1_click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ;
        }

        private void SetTextBox1Text(object text)
        {
            textBox1.Text = text.ToString();
        }

        private void SetTextBox1Color(object text)
        {
            if (text.Equals("0"))
                textBox1.BackColor = Color.Red;
            else
                textBox1.BackColor = Color.Cyan;
        }
        private void SetRadio1(object text)
        {
            if (text.Equals("0"))
            {
                radioButton1.Enabled = false;
                radioButton1.Checked = false;
            }
            else
            {
                radioButton1.Enabled = true;
            }
        }
        private void SearchUart(object text)
        {
            int i, n, id;

            n = cbSerial.Items.Count;
            id = cbSerial.SelectedIndex;
            cbSerial.Items.Clear();
            string[] portList = System.IO.Ports.SerialPort.GetPortNames();
            for (i = 0; i < portList.Length; i++)
            {
                string name = portList[i];
                cbSerial.Items.Add(name);
            }
            if (i != 0)
            {
                if (cbSerial.Items.Count != n)
                {
                    cbSerial.SelectedIndex = 0;
                }
                else
                {
                    cbSerial.SelectedIndex = id;
                }
                cbSerial.BackColor = Color.Cyan;
                radioButton2.Enabled = true;
            }
            else
            {
                cbSerial.Items.Add("No UART!");
                cbSerial.BackColor = Color.Red;
                cbSerial.SelectedIndex = 0;
                radioButton2.Checked = false;
                radioButton2.Enabled = false;
            }
        }
        private void ScandeviceThread()
        {
            while (true)
            {
                uint index = 0;
                bool result1, result2;
                bool FindHid = false;
                int HidHandle = -1;
                Guid guidHID = Guid.Empty;
                IntPtr hDevInfo;

                HidD_GetHidGuid(ref guidHID);
                hDevInfo = SetupDiGetClassDevs(ref guidHID, 0, IntPtr.Zero, DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE);
                int bufferSize = 0;
                ArrayList HIDUSBAddress = new ArrayList();
                SP_DEVICE_INTERFACE_DATA DeviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
                DeviceInterfaceData.cbSize = Marshal.SizeOf(DeviceInterfaceData);
                do
                {
                    result1 = SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref guidHID, index, ref DeviceInterfaceData);
                    index++;
                    if (!result1)
                    {
                        break;
                    }
                    SP_DEVINFO_DATA strtInterfaceData = new SP_DEVINFO_DATA();
                    result2 = SetupDiGetDeviceInterfaceDetail(hDevInfo, ref DeviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, strtInterfaceData);
                    IntPtr detailDataBuffer = Marshal.AllocHGlobal(bufferSize);
                    SP_DEVICE_INTERFACE_DETAIL_DATA detailData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                    detailData.cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA));
                    Marshal.StructureToPtr(detailData, detailDataBuffer, false);
                    result2 = SetupDiGetDeviceInterfaceDetail(hDevInfo, ref DeviceInterfaceData, detailDataBuffer, bufferSize, ref bufferSize, strtInterfaceData);
                    if (result2 == false)
                    {
                        Marshal.FreeHGlobal(detailDataBuffer);
                        continue;
                    }
                    IntPtr pdevicePathName = (IntPtr)((int)detailDataBuffer + 4);
                    string devicePathName = Marshal.PtrToStringAuto(pdevicePathName);
                    HIDUSBAddress.Add(devicePathName);
                    HidHandle = CreateFile(devicePathName,
                                            GENERIC_READ | GENERIC_WRITE,
                                            FILE_SHARE_READ | FILE_SHARE_WRITE,
                                            0,
                                            OPEN_EXISTING,
                                            0,
                                            0);
                    if (HidHandle == -1)
                    {
                        Marshal.FreeHGlobal(detailDataBuffer);
                        continue;
                    }
                    HID_ATTRIBUTES HidAttr = new HID_ATTRIBUTES();
                    result2 = HidD_GetAttributes(HidHandle, out HidAttr);
                    if (result2 == false)
                    {
                        Marshal.FreeHGlobal(detailDataBuffer);
                        CloseHandle(HidHandle);
                        continue;
                    }
                    if (HidAttr.VendorID == 0x0483 && HidAttr.ProductID == 0x3410)
                    {
                        FindHid = true;
                        HIDdevicePathName = devicePathName;
                        _syncContext.Post(SetTextBox1Text, "FIND DEVICE!");
                        _syncContext.Post(SetTextBox1Color, "1");
                        _syncContext.Post(SetRadio1, "1");
                    }
                    Marshal.FreeHGlobal(detailDataBuffer);
                    CloseHandle(HidHandle);
                }
                while (result1);
                SetupDiDestroyDeviceInfoList(hDevInfo);
                if (!FindHid)
                {
                    _syncContext.Post(SetTextBox1Text, "NO DEVICE!");
                    _syncContext.Post(SetTextBox1Color, "0");
                    _syncContext.Post(SetRadio1, "0");
                }

                _syncContext.Post(SearchUart, "0");

                Thread.Sleep(1000);
            }
        }

        private void Value_TextChanged(object sender, EventArgs e)
        {
            int i;
            try
            {
                i = Convert.ToInt32(RegValue.Text, 16);
            }
            catch
            {
                RegValue.Text = "00";
                return;
            }

            if ((i & 0x01) == 0x01)
            {
                cbBIT0.Checked = true;
            }
            else
            {
                cbBIT0.Checked = false;
            }
            if ((i & 0x02) == 0x02)
            {
                cbBIT1.Checked = true;
            }
            else
            {
                cbBIT1.Checked = false;
            }
            if ((i & 0x04) == 0x04)
            {
                cbBIT2.Checked = true;
            }
            else
            {
                cbBIT2.Checked = false;
            }
            if ((i & 0x08) == 0x08)
            {
                cbBIT3.Checked = true;
            }
            else
            {
                cbBIT3.Checked = false;
            }
            if ((i & 0x10) == 0x10)
            {
                cbBIT4.Checked = true;
            }
            else
            {
                cbBIT4.Checked = false;
            }
            if ((i & 0x20) == 0x20)
            {
                cbBIT5.Checked = true;
            }
            else
            {
                cbBIT5.Checked = false;
            }
            if ((i & 0x40) == 0x40)
            {
                cbBIT6.Checked = true;
            }
            else
            {
                cbBIT6.Checked = false;
            }
            if ((i & 0x80) == 0x80)
            {
                cbBIT7.Checked = true;
            }
            else
            {
                cbBIT7.Checked = false;
            }
            RegData.CurrentCell.Value = RegValue.Text;
        }

        private void RegData_SelectionChanged(object sender, EventArgs e)
        {
            int col = RegData.CurrentCell.ColumnIndex;
            int row = RegData.CurrentCell.RowIndex;

            if (col == 0)
            {
                col = 1;
                RegData.CurrentCell = RegData[1, row];
            }
            int pos = row * 16 + col - 1;

            SUBAddress.Text = Convert.ToString(pos, 16);
            RegValue.Text = RegData[col, row].Value.ToString();

        }

        private void Bitcheck_change()
        {
            int i = 0;

            if (cbBIT7.Checked)
            {
                i += 0x80;
            }
            if (cbBIT6.Checked)
            {
                i += 0x40;
            }
            if (cbBIT5.Checked)
            {
                i += 0x20;
            }
            if (cbBIT4.Checked)
            {
                i += 0x10;
            }
            if (cbBIT3.Checked)
            {
                i += 0x08;
            }
            if (cbBIT2.Checked)
            {
                i += 0x04;
            }
            if (cbBIT1.Checked)
            {
                i += 0x02;
            }
            if (cbBIT0.Checked)
            {
                i += 0x01;
            }

            string s = i.ToString("X2");
            RegValue.Text = s;
        }

        private void cbBIT7_CheckedChanged(object sender, EventArgs e)
        {
            //Bitcheck_change();
        }

        private void cbBIT6_CheckedChanged(object sender, EventArgs e)
        {
            //Bitcheck_change();
        }

        private void cbBIT5_CheckedChanged(object sender, EventArgs e)
        {
            //Bitcheck_change();
        }

        private void cbBIT4_CheckedChanged(object sender, EventArgs e)
        {
            //Bitcheck_change();
        }

        private void cbBIT3_CheckedChanged(object sender, EventArgs e)
        {
            //Bitcheck_change();
        }

        private void cbBIT2_CheckedChanged(object sender, EventArgs e)
        {
            //Bitcheck_change();
        }

        private void cbBIT1_CheckedChanged(object sender, EventArgs e)
        {
            //Bitcheck_change();
        }

        private void cbBIT0_CheckedChanged(object sender, EventArgs e)
        {
            //Bitcheck_change();
        }

        private void cbBIT_click(object sender, EventArgs e)
        {
            Bitcheck_change();
            Write_Register((byte)Convert.ToInt32(SlaveAddress.Text, 16), (byte)Convert.ToInt32(SUBAddress.Text, 16), (byte)Convert.ToInt32(RegValue.Text, 16));
        }

        private bool Read_Register(byte SlaveAddr, byte SUBAddr, ref byte value)
        {
            ReadAll.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            if (radioButton1.Checked)
            {
                int retry = 0;
                uint ReadNumber = 0;
                uint WriteNumber = 0;
                bool result;
                int HidHandle = CreateFile(HIDdevicePathName,
                                        GENERIC_READ | GENERIC_WRITE,
                                        FILE_SHARE_READ | FILE_SHARE_WRITE,
                                        0,
                                        OPEN_EXISTING,
                                        0,
                                        0);
                SendBuffer[0] = 0;
                SendBuffer[1] = 0;
                SendBuffer[2] = SlaveAddr;
                SendBuffer[3] = SUBAddr;
                SendBuffer[4] = 0;
                SendBuffer[5] = 1;
                result = WriteFile(HidHandle, SendBuffer, 65, ref WriteNumber, IntPtr.Zero);
                do
                {
                    result = ReadFile(HidHandle, ReceiveBuffer, 65, ref ReadNumber, IntPtr.Zero);
                    Thread.Sleep(10);
                    retry++;
                    if (retry > 10)
                    {
                        break;
                    }
                }
                while (!result);
                if (ReceiveBuffer[1] == 0xFF)
                {
                    MessageBox.Show("IIC read fail!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    CloseHandle(HidHandle);
                    return false;
                }
                else
                {
                    value = ReceiveBuffer[6];
                }
                CloseHandle(HidHandle);
            }
            else if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 5000;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return false;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 1;
                SendBuffer[3] = SlaveAddr;
                SendBuffer[4] = SUBAddr;
                SendBuffer[5] = Convert.ToByte(textBox3.Text, 10);
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(50);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 8);
                }
                catch
                {
                    MessageBox.Show("UART timeout!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    SelectUart.Close();
                    return false;
                }
                int checmsum = 0x100 - ((byte)(ReceiveBuffer[3] + ReceiveBuffer[4] + ReceiveBuffer[5] + ReceiveBuffer[6]));
                if (ReceiveBuffer[2] == 0xFF)
                {
                    MessageBox.Show("IIC read fail!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    SelectUart.Close();
                    return false;
                }
                else if ((((byte)checmsum) != ReceiveBuffer[7]) || ReceiveBuffer[2] == 0xFE)
                {
                    MessageBox.Show("Communication error!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    SelectUart.Close();
                    return false;
                }
                else
                {
                    value = ReceiveBuffer[6];
                }
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select port!", "Error");
                ReadAll.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                SelectUart.Close();
                return false;
            }
            ReadAll.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte reg_value = 0;
            Read_Register((byte)Convert.ToInt32(SlaveAddress.Text, 16), (byte)Convert.ToInt32(SUBAddress.Text, 16), ref reg_value);
            RegValue.Text = reg_value.ToString("X2");
        }

        private bool Write_Register(byte SlaveAddr, byte SUBAddr, byte value)
        {
            ReadAll.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            if (radioButton1.Checked)
            {
                int retry = 0;
                uint ReadNumber = 0;
                uint WriteNumber = 0;
                bool result;
                int HidHandle = CreateFile(HIDdevicePathName,
                                        GENERIC_READ | GENERIC_WRITE,
                                        FILE_SHARE_READ | FILE_SHARE_WRITE,
                                        0,
                                        OPEN_EXISTING,
                                        0,
                                        0);
                SendBuffer[0] = 0;
                SendBuffer[1] = 1;
                SendBuffer[2] = SlaveAddr;
                SendBuffer[3] = SUBAddr;
                SendBuffer[4] = 0;
                SendBuffer[5] = 1;
                SendBuffer[6] = value;
                result = WriteFile(HidHandle, SendBuffer, 65, ref WriteNumber, IntPtr.Zero);
                do
                {
                    result = ReadFile(HidHandle, ReceiveBuffer, 65, ref ReadNumber, IntPtr.Zero);
                    Thread.Sleep(10);
                    retry++;
                    if (retry > 10)
                    {
                        break;
                    }
                }
                while (!result);
                if (ReceiveBuffer[1] == 0xFF)
                {
                    MessageBox.Show("IIC write fail!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    CloseHandle(HidHandle);
                    return false;
                }
                CloseHandle(HidHandle);
            }
            else if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 5000;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return false;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 0;
                SendBuffer[3] = SlaveAddr;
                SendBuffer[4] = SUBAddr;
                SendBuffer[5] = Convert.ToByte(textBox3.Text, 10);
                SendBuffer[6] = value;
                SendBuffer[7] = (byte)(0x100 - ((byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6])));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(50);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 8);
                }
                catch
                {
                    MessageBox.Show("UART timeout!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    SelectUart.Close();
                    return false;
                }
                int checmsum = 0x100 - ((byte)(ReceiveBuffer[3] + ReceiveBuffer[4] + ReceiveBuffer[5] + ReceiveBuffer[6]));
                if (ReceiveBuffer[2] == 0xFF)
                {
                    MessageBox.Show("IIC write fail!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    SelectUart.Close();
                    return false;
                }
                else if ((((byte)checmsum) != ReceiveBuffer[7]) || ReceiveBuffer[2] == 0xFE)
                {
                    MessageBox.Show("Communication error!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    SelectUart.Close();
                    return false;
                }
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select port!", "Error");
                ReadAll.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                SelectUart.Close();
                return false;
            }
            ReadAll.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            return true;
        }

        private bool Write_Two_Register(byte SlaveAddr, byte SUBAddr, byte value1, byte value2)
        {
            ReadAll.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            if (radioButton1.Checked)
            {
                int retry = 0;
                uint ReadNumber = 0;
                uint WriteNumber = 0;
                bool result;
                int HidHandle = CreateFile(HIDdevicePathName,
                                        GENERIC_READ | GENERIC_WRITE,
                                        FILE_SHARE_READ | FILE_SHARE_WRITE,
                                        0,
                                        OPEN_EXISTING,
                                        0,
                                        0);
                SendBuffer[0] = 0;
                SendBuffer[1] = 1;
                SendBuffer[2] = SlaveAddr;
                SendBuffer[3] = SUBAddr;
                SendBuffer[4] = 0;
                SendBuffer[5] = 2;
                SendBuffer[6] = value1;
                SendBuffer[7] = value2;
                result = WriteFile(HidHandle, SendBuffer, 65, ref WriteNumber, IntPtr.Zero);
                do
                {
                    result = ReadFile(HidHandle, ReceiveBuffer, 65, ref ReadNumber, IntPtr.Zero);
                    Thread.Sleep(10);
                    retry++;
                    if (retry > 10)
                    {
                        break;
                    }
                }
                while (!result);
                if (ReceiveBuffer[1] == 0xFF)
                {
                    MessageBox.Show("IIC write fail!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    CloseHandle(HidHandle);
                    return false;
                }
                CloseHandle(HidHandle);
            }
            else if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 5000;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return false;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 5;
                SendBuffer[3] = SlaveAddr;
                SendBuffer[4] = SUBAddr;
                SendBuffer[5] = value1;
                SendBuffer[6] = value2;
                SendBuffer[7] = (byte)(0x100 - ((byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6])));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(50);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 8);
                }
                catch
                {
                    MessageBox.Show("UART timeout!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    SelectUart.Close();
                    return false;
                }
                int checmsum = 0x100 - ((byte)(ReceiveBuffer[3] + ReceiveBuffer[4] + ReceiveBuffer[5] + ReceiveBuffer[6]));
                if (ReceiveBuffer[2] == 0xFF)
                {
                    MessageBox.Show("IIC write fail!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    SelectUart.Close();
                    return false;
                }
                else if ((((byte)checmsum) != ReceiveBuffer[7]) || ReceiveBuffer[2] == 0xFE)
                {
                    MessageBox.Show("Communication error!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    SelectUart.Close();
                    return false;
                }
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select port!", "Error");
                ReadAll.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                SelectUart.Close();
                return false;
            }
            ReadAll.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            return true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Write_Register((byte)Convert.ToInt32(SlaveAddress.Text, 16), (byte)Convert.ToInt32(SUBAddress.Text, 16), (byte)Convert.ToInt32(RegValue.Text, 16));
        }

        private void SlaveAddress_TextChanged(object sender, EventArgs e)
        {
            int addr;
            try
            {
                addr = Convert.ToInt32(SlaveAddress.Text, 16);
                if (addr % 2 == 1 && addr > 16)
                {
                    addr = (addr / 2) * 2;
                    SlaveAddress.Text = addr.ToString("X2");
                }
            }
            catch
            {
                SlaveAddress.Text = "BA";
                return;
            }
        }

        private void ReadAll_Click(object sender, EventArgs e)
        {
            ReadAll.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            if (radioButton1.Checked)
            {
                int i, j, retry = 0;
                uint ReadNumber = 0;
                uint WriteNumber = 0;
                bool result;
                int HidHandle = CreateFile(HIDdevicePathName,
                                        GENERIC_READ | GENERIC_WRITE,
                                        FILE_SHARE_READ | FILE_SHARE_WRITE,
                                        0,
                                        OPEN_EXISTING,
                                        0,
                                        0);
                SendBuffer[0] = 0;
                SendBuffer[1] = 0;
                SendBuffer[2] = (byte)Convert.ToInt32(SlaveAddress.Text, 16);
                SendBuffer[3] = 0;
                SendBuffer[4] = 0;
                SendBuffer[5] = 32;
                for (j = 0; j < 8; j++)
                {
                    SendBuffer[3] = (byte)(32 * j);
                    result = WriteFile(HidHandle, SendBuffer, 65, ref WriteNumber, IntPtr.Zero);
                    do
                    {
                        result = ReadFile(HidHandle, ReceiveBuffer, 65, ref ReadNumber, IntPtr.Zero);
                        Thread.Sleep(10);
                        retry++;
                        if (retry > 10)
                        {
                            break;
                        }
                    }
                    while (!result);
                    if (ReceiveBuffer[1] == 0xFF)
                    {
                        MessageBox.Show("IIC read fail!", "Error");
                        ReadAll.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = true;
                        button4.Enabled = true;
                        CloseHandle(HidHandle);
                        return;
                    }
                    else
                    {
                        for (i = 0; i < 32; i++)
                        {
                            int val = ReceiveBuffer[6 + i];
                            RegData.CurrentCell = RegData[i % 16 + 1, (i + j * 32) / 16];
                            RegData.CurrentCell.Value = val.ToString("X2");

                        }
                    }
                    Thread.Sleep(10);
                }
                CloseHandle(HidHandle);
            }
            else if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 5000;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 3;
                SendBuffer[3] = (byte)Convert.ToInt32(SlaveAddress.Text, 16);
                SendBuffer[4] = 0;
                SendBuffer[5] = Convert.ToByte(textBox3.Text, 10);
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - ((byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6])));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(2000);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 256 + 7);
                }
                catch
                {
                    MessageBox.Show("Timeout!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    SelectUart.Close();
                    return;
                }
                int i, checksum = ReceiveBuffer[3] + ReceiveBuffer[4] + ReceiveBuffer[5];
                for (i = 0; i < 256; i++)
                {
                    checksum += ReceiveBuffer[6 + i];
                }
                checksum = 0x100 - (byte)checksum;
                if (ReceiveBuffer[2] == 0xFF)
                {
                    MessageBox.Show("IIC read fail!", "Error");
                }
                else if ((((byte)checksum) != ReceiveBuffer[6 + 256]) || ReceiveBuffer[2] == 0xFE)
                {
                    MessageBox.Show("Communication error!", "Error");
                }
                else
                {
                    for (i = 0; i < 256; i++)
                    {
                        int val = ReceiveBuffer[6 + i];
                        RegData.CurrentCell = RegData[i % 16 + 1, i / 16];
                        RegData.CurrentCell.Value = val.ToString("X2");

                    }
                }
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select port!", "Error");
            }
            ReadAll.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            SUBAddress.Text = "FF";
            RegValue.Text = RegData[16, 15].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReadAll.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            if (radioButton1.Checked)
            {
                int i, j, retry = 0;
                uint ReadNumber = 0;
                uint WriteNumber = 0;
                bool result;
                int HidHandle = CreateFile(HIDdevicePathName,
                                        GENERIC_READ | GENERIC_WRITE,
                                        FILE_SHARE_READ | FILE_SHARE_WRITE,
                                        0,
                                        OPEN_EXISTING,
                                        0,
                                        0);
                SendBuffer[0] = 0;
                SendBuffer[1] = 1;
                SendBuffer[2] = (byte)Convert.ToInt32(SlaveAddress.Text, 16);
                SendBuffer[3] = 0;
                SendBuffer[4] = 0;
                SendBuffer[5] = 16;
                for (j = 0; j < 16; j++)
                {
                    SendBuffer[3] = (byte)(16 * j);
                    for (i = 0; i < 16; i++)
                    {
                        SendBuffer[6 + i] = (byte)Convert.ToInt32(RegData[(i + j * 16) % 16 + 1, (i + j * 16) / 16].Value.ToString(), 16);
                    }
                    result = WriteFile(HidHandle, SendBuffer, 65, ref WriteNumber, IntPtr.Zero);
                    do
                    {
                        result = ReadFile(HidHandle, ReceiveBuffer, 65, ref ReadNumber, IntPtr.Zero);
                        Thread.Sleep(10);
                        retry++;
                        if (retry > 10)
                        {
                            break;
                        }
                    }
                    while (!result);
                    if (ReceiveBuffer[1] == 0xFF)
                    {
                        MessageBox.Show("IIC write fail!", "Error");
                        ReadAll.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = true;
                        button4.Enabled = true;
                        CloseHandle(HidHandle);
                        return;
                    }
                    Thread.Sleep(10);
                }
                CloseHandle(HidHandle);
            }
            else if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 5000;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 2;
                SendBuffer[3] = (byte)Convert.ToInt32(SlaveAddress.Text, 16);
                SendBuffer[4] = 0x00;
                SendBuffer[5] = Convert.ToByte(textBox3.Text, 10);
                int i, checksum = SendBuffer[3] + SendBuffer[4] + SendBuffer[5];
                for (i = 0; i < 256; i++)
                {
                    SendBuffer[6 + i] = (byte)Convert.ToInt32(RegData[i % 16 + 1, i / 16].Value.ToString(), 16);
                    checksum += SendBuffer[6 + i];
                }
                SendBuffer[256 + 6] = (byte)(0x100 - checksum);
                SelectUart.Write(SendBuffer, 0, 256 + 7);
                int addr = Convert.ToInt32(SlaveAddress.Text, 16);
                if (addr >= 0xA0 && addr < 0xB0)
                {
                    Thread.Sleep(3000);
                }
                else
                {
                    Thread.Sleep(2000);
                }
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 8);
                }
                catch
                {
                    MessageBox.Show("Timeout!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    SelectUart.Close();
                    return;
                }
                int checmsum = 0x100 - ((byte)(ReceiveBuffer[3] + ReceiveBuffer[4] + ReceiveBuffer[5] + ReceiveBuffer[6]));
                if (ReceiveBuffer[2] == 0xFF)
                {
                    MessageBox.Show("IIC write fail!", "Error");
                }
                else if ((((byte)checmsum) != ReceiveBuffer[7]) || ReceiveBuffer[2] == 0xFE)
                {
                    MessageBox.Show("Communication error!", "Error");
                }
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select port!", "Error");
            }
            ReadAll.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string fName;
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.Filter = "bin file|*.bin";
            FileDialog.RestoreDirectory = true;
            FileDialog.FilterIndex = 1;
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                fName = FileDialog.FileName;
                FileStream fs = new FileStream(fName, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                byte[] write_buffer = new byte[256];
                br.Read(write_buffer, 0, 256);
                for (int i = 0; i < 256; i++)
                {
                    int val = write_buffer[i];
                    string temp = val.ToString("X2");
                    RegData[i % 16 + 1, i / 16].Value = temp;
                }
                br.Close();
                fs.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string fName;
            SaveFileDialog FileDialog = new SaveFileDialog();
            FileDialog.Filter = "bin file|*.bin";
            FileDialog.RestoreDirectory = true;
            FileDialog.FilterIndex = 1;
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                fName = FileDialog.FileName;
                FileStream fs = new FileStream(fName, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                byte[] write_buffer = new byte[256];
                for (int i = 0; i < 256; i++)
                {
                    write_buffer[i] = (byte)Convert.ToInt32(RegData[i % 16 + 1, i / 16].Value.ToString(), 16);
                }
                bw.Write(write_buffer);
                bw.Flush();
                bw.Close();
                fs.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            byte val = (byte)Convert.ToInt32(RegValue.Text, 16);
            val++;
            RegValue.Text = val.ToString("X2");
            Write_Register((byte)Convert.ToInt32(SlaveAddress.Text, 16), (byte)Convert.ToInt32(SUBAddress.Text, 16), (byte)Convert.ToInt32(RegValue.Text, 16));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            byte val = (byte)Convert.ToInt32(RegValue.Text, 16);
            val--;
            RegValue.Text = val.ToString("X2");
            Write_Register((byte)Convert.ToInt32(SlaveAddress.Text, 16), (byte)Convert.ToInt32(SUBAddress.Text, 16), (byte)Convert.ToInt32(RegValue.Text, 16));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            byte val = (byte)Convert.ToInt32(RegValue.Text, 16);
            val += 16;
            RegValue.Text = val.ToString("X2");
            Write_Register((byte)Convert.ToInt32(SlaveAddress.Text, 16), (byte)Convert.ToInt32(SUBAddress.Text, 16), (byte)Convert.ToInt32(RegValue.Text, 16));
        }

        private void button10_Click(object sender, EventArgs e)
        {
            byte val = (byte)Convert.ToInt32(RegValue.Text, 16);
            val -= 16;
            RegValue.Text = val.ToString("X2");
            Write_Register((byte)Convert.ToInt32(SlaveAddress.Text, 16), (byte)Convert.ToInt32(SUBAddress.Text, 16), (byte)Convert.ToInt32(RegValue.Text, 16));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string fName;
            int lineNo = 0;
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.Filter = "script file|*.scr";
            FileDialog.RestoreDirectory = true;
            FileDialog.FilterIndex = 1;
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                string line, message = "";
                byte address = 0, subaddr = 0, value = 0, value1 = 0, delay = 0;
                bool result = false;
                fName = FileDialog.FileName;
                //FileStream fs = new FileStream(fName, FileMode.Open);
                System.IO.StreamReader file = new System.IO.StreamReader(fName);
                while ((line = file.ReadLine()) != null)
                {
                    string temp, temp1;
                    if (lineNo == 0)
                    {
                        if (line.Length < 10)
                        {
                            MessageBox.Show("No device address!", "Error");
                            break;
                        }
                        temp = line.Substring(0, 7);
                        if (temp != "ADDRESS")
                        {
                            MessageBox.Show("No device address!", "Error");
                            break;
                        }
                        temp = line.Substring(8, 2);
                        try
                        {
                            address = (byte)Convert.ToInt32(temp, 16);
                        }
                        catch
                        {
                            MessageBox.Show("Error line " + lineNo.ToString(), "Error");
                            break;
                        }
                        if (address == 0)
                        {
                            MessageBox.Show("device address error!", "Error");
                            break;
                        }
                        else
                        {
                            if (!DisplayMessage)
                            {
                                this.Width += 250;
                                DisplayMessage = true;
                                button13.Text = "<<<";
                            }
                            message = "DEVICE ADDRESS === ";
                            message += address.ToString("X2");
                            message += "\r\n";
                            textBox2.Text = message;
                        }
                    }
                    else
                    {
                        if (line.Length < 7)
                        {
                            MessageBox.Show("Error line " + lineNo.ToString(), "Error");
                            break;
                        }
                        temp = line.Substring(0, 5);
                        if (temp == "READ ")
                        {
                            temp = line.Substring(5, 2);
                            try
                            {
                                subaddr = (byte)Convert.ToInt32(temp, 16);
                            }
                            catch
                            {
                                MessageBox.Show("Error line " + lineNo.ToString(), "Error");
                                break;
                            }
                            result = Read_Register(address, subaddr, ref value);
                            if (!result)
                            {
                                break;
                            }
                            //Thread.Sleep(20);
                            temp = "READ <=== ";
                            temp += subaddr.ToString("X2");
                            temp += " VALUE ";
                            temp += value.ToString("X2");
                            temp += "\r\n";
                            message += temp;
                            textBox2.Text = message;
                        }
                        else if (temp == "WRITE")
                        {
                            temp = line.Substring(6, 2);
                            try
                            {
                                subaddr = (byte)Convert.ToInt32(temp, 16);
                            }
                            catch
                            {
                                MessageBox.Show("Error line " + lineNo.ToString(), "Error");
                                break;
                            }

                            if (line.Length == 10)
                            {
                                temp = line.Substring(9, 1);
                            }
                            else
                            {
                                temp1 = line.Substring(10, 1);
                                if (temp1 == " ")
                                {
                                    temp = line.Substring(9, 1);
                                }
                                else
                                {
                                    temp = line.Substring(9, 2);
                                }
                            }
                            try
                            {
                                value = (byte)Convert.ToInt32(temp, 16);
                            }
                            catch
                            {
                                MessageBox.Show("Error line " + lineNo.ToString(), "Error");
                                break;
                            }
                            result = Write_Register(address, subaddr, value);
                            if (!result)
                            {
                                break;
                            }
                            //Thread.Sleep(20);
                            temp = "WRITE ===> ";
                            temp += subaddr.ToString("X2");
                            temp += " VALUE ";
                            temp += value.ToString("X2");
                            temp += "\r\n";
                            message += temp;
                            textBox2.Text = message;
                        }
                        else if (temp == "WRTWO")
                        {
                            temp = line.Substring(6, 2);
                            try
                            {
                                subaddr = (byte)Convert.ToInt32(temp, 16);
                            }
                            catch
                            {
                                MessageBox.Show("Error line " + lineNo.ToString(), "Error");
                                break;
                            }

                            temp = line.Substring(9, 2);
                            try
                            {
                                value = (byte)Convert.ToInt32(temp, 16);
                            }
                            catch
                            {
                                MessageBox.Show("Error line " + lineNo.ToString(), "Error");
                                break;
                            }

                            temp = line.Substring(11, 2);
                            try
                            {
                                value1 = (byte)Convert.ToInt32(temp, 16);
                            }
                            catch
                            {
                                MessageBox.Show("Error line " + lineNo.ToString(), "Error");
                                break;
                            }

                            result = Write_Two_Register(address, subaddr, value, value1);
                            if (!result)
                            {
                                break;
                            }
                            temp = "WRITE TWO ===> ";
                            temp += subaddr.ToString("X2");
                            temp += " VALUE ";
                            temp += value.ToString("X2");
                            temp += value1.ToString("X2");
                            temp += "\r\n";
                            message += temp;
                            textBox2.Text = message;
                        }
                        else if (temp == "DELAY")
                        {
                            temp = line.Substring(6, 2);
                            try
                            {
                                delay = (byte)Convert.ToInt32(temp, 10);
                            }
                            catch
                            {
                                MessageBox.Show("Error line " + lineNo.ToString(), "Error");
                                break;
                            }
                            Thread.Sleep(delay);

                            temp = "DELAY === ";
                            temp += delay.ToString("D3");
                            temp += "ms\r\n";
                            message += temp;
                            textBox2.Text = message;
                        }
                        else if (temp == "ADDRE")
                        {
                            temp = line.Substring(8, 2);
                            try
                            {
                                address = (byte)Convert.ToInt32(temp, 16);
                            }
                            catch
                            {
                                MessageBox.Show("Error line " + lineNo.ToString(), "Error");
                                break;
                            }
                            temp = "DEVICE ADDRESS === ";
                            temp += address.ToString("X2");
                            temp += "\r\n";
                            message += temp;
                            textBox2.Text = message;
                        }
                        else
                        {
                            MessageBox.Show("Error line " + lineNo.ToString(), "Error");
                            break;
                        }
                    }
                    lineNo++;
                    textBox2.Refresh();
                    textBox2.Focus();
                    textBox2.Select(textBox2.TextLength, 0);
                    textBox2.ScrollToCaret();
                }
                file.Close();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MessageBoxButtons messButton = MessageBoxButtons.YesNo;
            DialogResult dr = MessageBox.Show("新硬件版本？（新版本直接存入EEPROOM）", "保存配置", messButton);

            if (dr == DialogResult.Yes)
            {
                if (radioButton1.Checked)
                {
                    byte[] data_buffer = new byte[21];
                    data_buffer[0] = 0xA5;
                    data_buffer[1] = (byte)Convert.ToInt32(RegData[0x07 + 1, 0x05].Value.ToString(), 16);   // 0x57
                    data_buffer[2] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x0C].Value.ToString(), 16);   // 0xC8
                    data_buffer[3] = (byte)Convert.ToInt32(RegData[0x09 + 1, 0x0C].Value.ToString(), 16);   // 0xC9
                    data_buffer[4] = (byte)Convert.ToInt32(RegData[0x0A + 1, 0x0C].Value.ToString(), 16);   // 0xCA
                    data_buffer[5] = (byte)Convert.ToInt32(RegData[0x0B + 1, 0x0C].Value.ToString(), 16);   // 0xCB
                    data_buffer[6] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x01].Value.ToString(), 16);   // 0x18
                    data_buffer[7] = (byte)Convert.ToInt32(RegData[0x07 + 1, 0x04].Value.ToString(), 16);   // 0x47
                    data_buffer[8] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x04].Value.ToString(), 16);   // 0x48
                    data_buffer[9] = (byte)Convert.ToInt32(RegData[0x09 + 1, 0x04].Value.ToString(), 16);   // 0x49
                    data_buffer[10] = (byte)Convert.ToInt32(RegData[0x0A + 1, 0x04].Value.ToString(), 16);   // 0x4A
                    data_buffer[11] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x05].Value.ToString(), 16);   // 0x58
                    data_buffer[12] = (byte)Convert.ToInt32(RegData[0x09 + 1, 0x05].Value.ToString(), 16);   // 0x59
                    data_buffer[13] = (byte)Convert.ToInt32(RegData[0x0A + 1, 0x05].Value.ToString(), 16);   // 0x5A
                    data_buffer[14] = (byte)Convert.ToInt32(RegData[0x0B + 1, 0x05].Value.ToString(), 16);   // 0x5B
                    data_buffer[15] = (byte)Convert.ToInt32(RegData[0x0C + 1, 0x05].Value.ToString(), 16);   // 0x5C
                    data_buffer[16] = (byte)Convert.ToInt32(RegData[0x0D + 1, 0x07].Value.ToString(), 16);   // 0x7D
                    data_buffer[17] = (byte)Convert.ToInt32(RegData[0x0F + 1, 0x0D].Value.ToString(), 16);   // 0xDF
                    data_buffer[18] = (byte)Convert.ToInt32(RegData[0x07 + 1, 0x0E].Value.ToString(), 16);   // 0xE7
                    data_buffer[19] = (byte)Convert.ToInt32(RegData[0x0D + 1, 0x00].Value.ToString(), 16);   // 0x0D
                    data_buffer[20] = Convert.ToByte(textBox3.Text, 10);
                    for (byte i = 0; i < 21; i++)
                    {
                        Write_Register(0xA0, i, data_buffer[i]);
                    }
                    MessageBox.Show("Write config success!", "Note");
                }
                else
                {
                    MessageBox.Show("Only USBHID tool function!", "Error");
                }
            }
            else
            {
                string fName;
                SaveFileDialog FileDialog = new SaveFileDialog();
                FileDialog.Filter = "Motorola S19 file|*.s19";
                FileDialog.RestoreDirectory = true;
                FileDialog.FilterIndex = 1;
                if (FileDialog.ShowDialog() == DialogResult.OK)
                {
                    fName = FileDialog.FileName;
                    FileStream fs = new FileStream(fName, FileMode.Create);
                    byte[] head = System.Text.Encoding.Default.GetBytes("S1234000");
                    fs.Write(head, 0, head.Length);
                    byte[] data_buffer = new byte[33];
                    data_buffer[0] = 0xA5;
                    data_buffer[1] = (byte)Convert.ToInt32(RegData[0x07 + 1, 0x05].Value.ToString(), 16);   // 0x57
                    data_buffer[2] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x0C].Value.ToString(), 16);   // 0xC8
                    data_buffer[3] = (byte)Convert.ToInt32(RegData[0x09 + 1, 0x0C].Value.ToString(), 16);   // 0xC9
                    data_buffer[4] = (byte)Convert.ToInt32(RegData[0x0A + 1, 0x0C].Value.ToString(), 16);   // 0xCA
                    data_buffer[5] = (byte)Convert.ToInt32(RegData[0x0B + 1, 0x0C].Value.ToString(), 16);   // 0xCB
                    data_buffer[6] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x01].Value.ToString(), 16);   // 0x18
                    data_buffer[7] = (byte)Convert.ToInt32(RegData[0x07 + 1, 0x04].Value.ToString(), 16);   // 0x47
                    data_buffer[8] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x04].Value.ToString(), 16);   // 0x48
                    data_buffer[9] = (byte)Convert.ToInt32(RegData[0x09 + 1, 0x04].Value.ToString(), 16);   // 0x49
                    data_buffer[10] = (byte)Convert.ToInt32(RegData[0x0A + 1, 0x04].Value.ToString(), 16);   // 0x4A
                    data_buffer[11] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x05].Value.ToString(), 16);   // 0x58
                    data_buffer[12] = (byte)Convert.ToInt32(RegData[0x09 + 1, 0x05].Value.ToString(), 16);   // 0x59
                    data_buffer[13] = (byte)Convert.ToInt32(RegData[0x0A + 1, 0x05].Value.ToString(), 16);   // 0x5A
                    data_buffer[14] = (byte)Convert.ToInt32(RegData[0x0B + 1, 0x05].Value.ToString(), 16);   // 0x5B
                    data_buffer[15] = (byte)Convert.ToInt32(RegData[0x0C + 1, 0x05].Value.ToString(), 16);   // 0x5C
                    data_buffer[16] = (byte)Convert.ToInt32(RegData[0x0D + 1, 0x07].Value.ToString(), 16);   // 0x7D
                    data_buffer[17] = (byte)Convert.ToInt32(RegData[0x0F + 1, 0x0D].Value.ToString(), 16);   // 0xDF
                    data_buffer[18] = (byte)Convert.ToInt32(RegData[0x07 + 1, 0x0E].Value.ToString(), 16);   // 0xE7
                    data_buffer[19] = (byte)Convert.ToInt32(RegData[0x0D + 1, 0x00].Value.ToString(), 16);   // 0x0D
                    data_buffer[20] = Convert.ToByte(textBox3.Text, 10);
                    data_buffer[32] = 0x63;
                    for (byte i = 0; i < 32; i++)
                    {
                        data_buffer[32] += data_buffer[i];
                    }
                    byte value;
                    value = 0xFF;
                    value -= data_buffer[32];
                    data_buffer[32] = value;

                    byte[] write_buffer = new byte[67];

                    for (byte i = 0; i < 33; i++)
                    {
                        value = data_buffer[i];
                        value >>= 4;
                        if (value > 9)
                        {
                            value += 0x37;
                            write_buffer[i * 2] = value;
                        }
                        else
                        {
                            value += 0x30;
                            write_buffer[i * 2] = value;
                        }
                        value = data_buffer[i];
                        value &= 0x0F;
                        if (value > 9)
                        {
                            value += 0x37;
                            write_buffer[i * 2 + 1] = value;
                        }
                        else
                        {
                            value += 0x30;
                            write_buffer[i * 2 + 1] = value;
                        }
                    }
                    write_buffer[66] = 0;
                    fs.Write(write_buffer, 0, write_buffer.Length - 1);
                    byte[] end = System.Text.Encoding.Default.GetBytes("\r\nS9030000FC\r\n");
                    fs.Write(end, 0, end.Length);
                    fs.Flush();
                    fs.Close();
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (DisplayMessage)
            {
                this.Width -= 250;
                DisplayMessage = false;
                button13.Text = ">>>";
            }
            else
            {
                this.Width += 250;
                DisplayMessage = true;
                button13.Text = "<<<";
            }
        }

        private void Value_MouseWheel(object sender, MouseEventArgs e)
        {
            byte val = (byte)Convert.ToInt32(RegValue.Text, 16);
            if (e.Delta > 0)
                val++;
            else
                val--;
            RegValue.Text = val.ToString("X2");
            Write_Register((byte)Convert.ToInt32(SlaveAddress.Text, 16), (byte)Convert.ToInt32(SUBAddress.Text, 16), (byte)Convert.ToInt32(RegValue.Text, 16));
        }

        private void Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x0d)
            {
                Write_Register((byte)Convert.ToInt32(SlaveAddress.Text, 16), (byte)Convert.ToInt32(SUBAddress.Text, 16), (byte)Convert.ToInt32(RegValue.Text, 16));
            }
        }

        private void Value_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                byte val = (byte)Convert.ToInt32(RegValue.Text, 16);
                val--;
                RegValue.Text = val.ToString("X2");
                Write_Register((byte)Convert.ToInt32(SlaveAddress.Text, 16), (byte)Convert.ToInt32(SUBAddress.Text, 16), (byte)Convert.ToInt32(RegValue.Text, 16));
            }
        }

        private void Value_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                byte val = (byte)Convert.ToInt32(RegValue.Text, 16);
                val++;
                RegValue.Text = val.ToString("X2");
                Write_Register((byte)Convert.ToInt32(SlaveAddress.Text, 16), (byte)Convert.ToInt32(SUBAddress.Text, 16), (byte)Convert.ToInt32(RegValue.Text, 16));
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

				 private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                byte val = (byte)Convert.ToInt32(textBox3.Text, 10);
                val--;
                if (val == 255)
                	val = 99;
                textBox3.Text = Convert.ToString(val, 10);
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                byte val = (byte)Convert.ToInt32(textBox3.Text, 10);
                val++;
                if (val == 100)
                	val = 0;
                textBox3.Text = Convert.ToString(val, 10);
            }
        }
        
        private void textBox3_MouseWheel(object sender, MouseEventArgs e)
        {
            byte val = (byte)Convert.ToInt32(textBox3.Text, 10);
            if (e.Delta > 0)
            {
                val++;
                if (val == 100)
                	val = 0;
            }
            else
            {
                val--;
                if (val == 255)
                	val = 99;
            }
            textBox3.Text = Convert.ToString(val, 10);
        }
        
        private void I2CTool_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 5000;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 6;
                SendBuffer[3] = 0;
                SendBuffer[4] = 0;
                SendBuffer[5] = Convert.ToByte(textBox3.Text, 10);
                SendBuffer[6] = (byte)Convert.ToInt32(RegData[0x07 + 1, 0x05].Value.ToString(), 16);   // 0x57
                SendBuffer[7] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x0C].Value.ToString(), 16);   // 0xC8
                SendBuffer[8] = (byte)Convert.ToInt32(RegData[0x09 + 1, 0x0C].Value.ToString(), 16);   // 0xC9
                SendBuffer[9] = (byte)Convert.ToInt32(RegData[0x0A + 1, 0x0C].Value.ToString(), 16);   // 0xCA
                SendBuffer[10] = (byte)Convert.ToInt32(RegData[0x0B + 1, 0x0C].Value.ToString(), 16);   // 0xCB
                SendBuffer[11] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x01].Value.ToString(), 16);   // 0x18
                SendBuffer[12] = (byte)Convert.ToInt32(RegData[0x07 + 1, 0x04].Value.ToString(), 16);   // 0x47
                SendBuffer[13] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x04].Value.ToString(), 16);   // 0x48
                SendBuffer[14] = (byte)Convert.ToInt32(RegData[0x09 + 1, 0x04].Value.ToString(), 16);   // 0x49
                SendBuffer[15] = (byte)Convert.ToInt32(RegData[0x0A + 1, 0x04].Value.ToString(), 16);   // 0x4A
                SendBuffer[16] = (byte)Convert.ToInt32(RegData[0x08 + 1, 0x05].Value.ToString(), 16);   // 0x58
                SendBuffer[17] = (byte)Convert.ToInt32(RegData[0x09 + 1, 0x05].Value.ToString(), 16);   // 0x59
                SendBuffer[18] = (byte)Convert.ToInt32(RegData[0x0A + 1, 0x05].Value.ToString(), 16);   // 0x5A
                SendBuffer[19] = (byte)Convert.ToInt32(RegData[0x0B + 1, 0x05].Value.ToString(), 16);   // 0x5B
                SendBuffer[20] = (byte)Convert.ToInt32(RegData[0x0C + 1, 0x05].Value.ToString(), 16);   // 0x5C
                SendBuffer[21] = (byte)Convert.ToInt32(RegData[0x0D + 1, 0x07].Value.ToString(), 16);   // 0x7D
                SendBuffer[22] = (byte)Convert.ToInt32(RegData[0x0F + 1, 0x0D].Value.ToString(), 16);   // 0xDF
                SendBuffer[23] = (byte)Convert.ToInt32(RegData[0x07 + 1, 0x0E].Value.ToString(), 16);   // 0xE7
                SendBuffer[24] = (byte)Convert.ToInt32(RegData[0x0D + 1, 0x00].Value.ToString(), 16);   // 0x0D
                SendBuffer[25] = Convert.ToByte(textBox3.Text, 10);
                int i, checksum = SendBuffer[5];
                for (i = 0; i < 20; i++)
                {
                    checksum += SendBuffer[6 + i];
                }
                SendBuffer[26] = (byte)(0x100 - (byte)checksum);
                SelectUart.Write(SendBuffer, 0, 27);
                Thread.Sleep(2000);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 27);
                    if (ReceiveBuffer[2] == 0xFE)
                    {
                        MessageBox.Show("Communication Error!", "Error");
                        SelectUart.Close();
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("Timeout!", "Error");
                    SelectUart.Close();
                    return;
                }
                MessageBox.Show("Write config success!", "Note");
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 5000;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 4;
                SendBuffer[3] = 0;
                SendBuffer[4] = 0;
                SendBuffer[5] = Convert.ToByte(textBox3.Text, 10);
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 8);
                    if (ReceiveBuffer[2] == 0xFE)
                    {
                        MessageBox.Show("Communication Error!", "Error");
                    }
                    else
                    {
                        if (ReceiveBuffer[4] == 0)
                        {
                            button21.BackColor = Color.Red;
                        }
                        else
                        {
                            button21.BackColor = Color.Green;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Timeout!", "Error");
                    SelectUart.Close();
                    return;
                }
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 5000;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 4;
                SendBuffer[3] = 1;
                SendBuffer[4] = 0;
                SendBuffer[5] = Convert.ToByte(textBox3.Text, 10);
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 8);
                    if (ReceiveBuffer[2] == 0xFE)
                    {
                        MessageBox.Show("Communication Error!", "Error");
                    }
                    else
                    {
                        if (ReceiveBuffer[4] == 1)
                        {
                            button23.BackColor = Color.Red;
                        }
                        else
                        {
                            button23.BackColor = Color.Green;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Timeout!", "Error");
                    SelectUart.Close();
                    return;
                }
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 5000;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 4;
                SendBuffer[3] = 2;
                SendBuffer[4] = 0;
                SendBuffer[5] = Convert.ToByte(textBox3.Text, 10);
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 8);
                    if (ReceiveBuffer[2] == 0xFE)
                    {
                        MessageBox.Show("Communication Error!", "Error");
                    }
                    else
                    {
                        if (ReceiveBuffer[4] == 0)
                        {
                            button22.BackColor = Color.Red;
                        }
                        else
                        {
                            button22.BackColor = Color.Green;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Timeout!", "Error");
                    SelectUart.Close();
                    return;
                }
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("警告：特殊功能，只能连接一台机器", "修改机器号", messButton);
            if (dr == DialogResult.OK)
            {
                if (radioButton2.Checked)
                {
                    try
                    {
                        string serialName = cbSerial.SelectedItem.ToString();
                        SelectUart.PortName = serialName;
                        if (checkBox1.Checked)
		                    {
		                        SelectUart.BaudRate = 9600;
		                    }
		                    else
		                    {
		                        SelectUart.BaudRate = 115200;
		                    }
                        SelectUart.DataBits = 8;
                        SelectUart.StopBits = StopBits.One;
                        SelectUart.Parity = Parity.None;
                        SelectUart.ReadTimeout = 5000;
                        if (SelectUart.IsOpen == true)
                        {
                            SelectUart.Close();
                        }
                        SelectUart.Open();
                    }
                    catch
                    {
                        MessageBox.Show("Can not open serial port!", "Error");
                        ReadAll.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = true;
                        button4.Enabled = true;
                        return;
                    }
                    SendBuffer[0] = 0xFF;
                    SendBuffer[1] = 0x55;
                    SendBuffer[2] = 7;
                    SendBuffer[3] = 0;
                    SendBuffer[4] = 0;
                    SendBuffer[5] = 0;
                    SendBuffer[6] = Convert.ToByte(textBox3.Text, 10);
                    SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                    SelectUart.Write(SendBuffer, 0, 8);
                    Thread.Sleep(200);
                    try
                    {
                        SelectUart.Read(ReceiveBuffer, 0, 8);
                    }
                    catch
                    {
                        MessageBox.Show("Timeout!", "Error");
                        SelectUart.Close();
                        return;
                    }
                    SelectUart.Close();
                }
                else
                {
                    MessageBox.Show("Please select uart port!", "Error");
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            ReadAll.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            if (radioButton1.Checked)
            {
                int retry = 0;
                uint ReadNumber = 0;
                uint WriteNumber = 0;
                bool result;
                int HidHandle = CreateFile(HIDdevicePathName,
                                        GENERIC_READ | GENERIC_WRITE,
                                        FILE_SHARE_READ | FILE_SHARE_WRITE,
                                        0,
                                        OPEN_EXISTING,
                                        0,
                                        0);
                SendBuffer[0] = 0;
                SendBuffer[1] = 1;
                SendBuffer[2] = 0xE0;
                SendBuffer[3] = 0x26;
                SendBuffer[4] = 0;
                SendBuffer[5] = 8;
                SendBuffer[6] = 0x38;
                SendBuffer[7] = 0x50;
                SendBuffer[8] = 0x6D;
                SendBuffer[9] = 0x00;
                SendBuffer[10] = 0x00;
                SendBuffer[11] = 0x00;
                SendBuffer[12] = 0x00;
                SendBuffer[13] = 0x02;
                result = WriteFile(HidHandle, SendBuffer, 65, ref WriteNumber, IntPtr.Zero);
                do
                {
                    result = ReadFile(HidHandle, ReceiveBuffer, 65, ref ReadNumber, IntPtr.Zero);
                    Thread.Sleep(10);
                    retry++;
                    if (retry > 10)
                    {
                        break;
                    }
                }
                while (!result);
                if (ReceiveBuffer[1] == 0xFF)
                {
                    MessageBox.Show("IIC write fail!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    CloseHandle(HidHandle);
                    return;
                }

                Thread.Sleep(100);
                SendBuffer[6] = 0x38;
                SendBuffer[7] = 0x64;
                SendBuffer[8] = 0x1C;
                SendBuffer[9] = 0x00;
                SendBuffer[10] = 0x00;
                SendBuffer[11] = 0x00;
                SendBuffer[12] = 0x00;
                SendBuffer[13] = 0x01;
                result = WriteFile(HidHandle, SendBuffer, 65, ref WriteNumber, IntPtr.Zero);
                do
                {
                    result = ReadFile(HidHandle, ReceiveBuffer, 65, ref ReadNumber, IntPtr.Zero);
                    Thread.Sleep(10);
                    retry++;
                    if (retry > 10)
                    {
                        break;
                    }
                }
                while (!result);
                if (ReceiveBuffer[1] == 0xFF)
                {
                    MessageBox.Show("IIC write fail!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    CloseHandle(HidHandle);
                    return;
                }
                CloseHandle(HidHandle);
            }
            else
            {
                MessageBox.Show("Only USBHID tool function!", "Error");
            }
            ReadAll.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            byte val;
            try
            {
                val = Convert.ToByte(textBox4.Text, 10);
            }
            catch
            {
                textBox4.Text = "0";
                return;
            }
            
            if (val > 100)
            {
                textBox4.Text = "0";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            byte val;
            try
            {
                val = Convert.ToByte(textBox3.Text, 10);
            }
            catch
            {
                textBox4.Text = "0";
                return;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 5000;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 4;
                SendBuffer[3] = 3;
                SendBuffer[4] = Convert.ToByte(textBox4.Text, 10);
                SendBuffer[5] = Convert.ToByte(textBox3.Text, 10);
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 8);
                }
                catch
                {
                    MessageBox.Show("Timeout!", "Error");
                    SelectUart.Close();
                    return;
                }
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 5000;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 4;
                SendBuffer[3] = 0x80;
                SendBuffer[4] = 0;
                SendBuffer[5] = Convert.ToByte(textBox3.Text, 10);
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 8);
                    if (ReceiveBuffer[4] == 0)
                    {
                        button21.BackColor = Color.Red;
                    }
                    else
                    {
                        button21.BackColor = Color.Green;
                    }
                    if (ReceiveBuffer[5] == 1)
                    {
                        button22.BackColor = Color.Red;
                    }
                    else
                    {
                        button22.BackColor = Color.Green;
                    }
                    if (ReceiveBuffer[6] == 0)
                    {
                        button23.BackColor = Color.Red;
                    }
                    else
                    {
                        button23.BackColor = Color.Green;
                    }
                }
                catch
                {
                    MessageBox.Show("Timeout!", "Error");
                    SelectUart.Close();
                    return;
                }
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 0x80;
                SendBuffer[3] = 0x80;
                SendBuffer[4] = 0;
                SendBuffer[5] = 0;
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(500);
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 0x80;
                SendBuffer[3] = 0x88;
                SendBuffer[4] = 0;
                SendBuffer[5] = 0;
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(500);
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 0x80;
                SendBuffer[3] = 0x82;
                SendBuffer[4] = 0;
                SendBuffer[5] = 0;
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(500);
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 0x80;
                SendBuffer[3] = 0x83;
                SendBuffer[4] = 0;
                SendBuffer[5] = 0;
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(500);
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 0x80;
                SendBuffer[3] = 0x84;
                SendBuffer[4] = 0;
                SendBuffer[5] = 0;
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(500);
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 0x80;
                SendBuffer[3] = 0x85;
                SendBuffer[4] = 0;
                SendBuffer[5] = 0;
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(500);
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 0x80;
                SendBuffer[3] = 0x86;
                SendBuffer[4] = 0;
                SendBuffer[5] = 0;
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(500);
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 0x80;
                SendBuffer[3] = 0x87;
                SendBuffer[4] = 0;
                SendBuffer[5] = 0;
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(500);
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;
                    if (checkBox1.Checked)
                    {
                        SelectUart.BaudRate = 9600;
                    }
                    else
                    {
                        SelectUart.BaudRate = 115200;
                    }
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    ReadAll.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    return;
                }
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 0x80;
                SendBuffer[3] = 0x81;
                SendBuffer[4] = 0;
                SendBuffer[5] = 0;
                SendBuffer[6] = 0;
                SendBuffer[7] = (byte)(0x100 - (byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(500);
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(200);
                SelectUart.Close();
            }
            else
            {
                MessageBox.Show("Please select uart port!", "Error");
            }
        }
    }
}
