using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace IICTool
{
    public partial class I2CTool : Form
    {
        public SerialPort SelectUart;
        public Byte[] SendBuffer = new Byte[256 + 7];
        public Byte[] ReceiveBuffer = new Byte[256 + 7];

        public I2CTool()
        {
            InitializeComponent();
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
            RegData.SelectionChanged += new EventHandler(RegData_SelectionChanged);
        }

        private string fName;

        private void comboBox1_click(object sender, EventArgs e)
        {
            int i;

            cbSerial.Items.Clear();
            string[] portList = System.IO.Ports.SerialPort.GetPortNames();
            for (i = 0; i < portList.Length; i++)
            {
                string name = portList[i];
                cbSerial.Items.Add(name);
            }
            if (i == 0)
            {
                MessageBox.Show("No serial port!", "Error");
                return;
            }
            else
            {
                cbSerial.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!SelectUart.IsOpen)
            {
                try
                {
                    string serialName = cbSerial.SelectedItem.ToString();
                    SelectUart.PortName = serialName;

                    SelectUart.BaudRate = 115200;
                    SelectUart.DataBits = 8;
                    SelectUart.StopBits = StopBits.One;
                    SelectUart.Parity = Parity.None;
                    SelectUart.ReadTimeout = 1000;
                    cbSerial.Enabled = false;
                    if (SelectUart.IsOpen == true)
                    {
                        SelectUart.Close();
                    }
                    SelectUart.Open();
                    button1.Text = "DisConnect";
                }
                catch
                {
                    MessageBox.Show("Can not open serial port!", "Error");
                    return;
                }
            }
            else
            {
                cbSerial.Enabled = true;
                SelectUart.Close();
                button1.Text = "Connect";
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

            if ((i&0x01) == 0x01)
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
            Bitcheck_change();
        }

        private void cbBIT6_CheckedChanged(object sender, EventArgs e)
        {
            Bitcheck_change();
        }

        private void cbBIT5_CheckedChanged(object sender, EventArgs e)
        {
            Bitcheck_change();
        }

        private void cbBIT4_CheckedChanged(object sender, EventArgs e)
        {
            Bitcheck_change();
        }

        private void cbBIT3_CheckedChanged(object sender, EventArgs e)
        {
            Bitcheck_change();
        }

        private void cbBIT2_CheckedChanged(object sender, EventArgs e)
        {
            Bitcheck_change();
        }

        private void cbBIT1_CheckedChanged(object sender, EventArgs e)
        {
            Bitcheck_change();
        }

        private void cbBIT0_CheckedChanged(object sender, EventArgs e)
        {
            Bitcheck_change();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cbSerial.Enabled)
            {
                MessageBox.Show("Please connnect serial port!", "Warning");
                return;
            }
            else
            {
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 1;
                SendBuffer[3] = (Byte)Convert.ToInt32(SlaveAddress.Text, 16);
                SendBuffer[4] = (Byte)Convert.ToInt32(SUBAddress.Text, 16);
                SendBuffer[5] = 0;
                SendBuffer[6] = 0;
                SendBuffer[7] = (Byte)(0x100 - (Byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6]));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(20);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 8);
                }
                catch 
                {
                    MessageBox.Show("Timeout!", "Error");
                    return;
                }
                int checmsum = 0x100 - ((Byte)(ReceiveBuffer[3] + ReceiveBuffer[4] + ReceiveBuffer[5] + ReceiveBuffer[6]));
                if (((Byte)checmsum) == ReceiveBuffer[7])
                {
                    int val = ReceiveBuffer[6];
                    RegValue.Text = val.ToString("X2");
                }
                else
                {
                    MessageBox.Show("Read fail!", "Error");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cbSerial.Enabled)
            {
                MessageBox.Show("Please connnect serial port!", "Warning");
                return;
            }
            else
            {
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 0;
                SendBuffer[3] = (Byte)Convert.ToInt32(SlaveAddress.Text, 16);
                SendBuffer[4] = (Byte)Convert.ToInt32(SUBAddress.Text, 16);
                SendBuffer[5] = 0x80;
                SendBuffer[6] = (Byte)Convert.ToInt32(RegValue.Text, 16);
                SendBuffer[7] = (Byte)(0x100 - ((Byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6])));
                SelectUart.Write(SendBuffer, 0, 8);
            }
        }

        private void SlaveAddress_TextChanged(object sender, EventArgs e)
        {
            int addr;
            try
            {
                addr = Convert.ToInt32(SlaveAddress.Text, 16);
            }
            catch
            {
                SlaveAddress.Text = "BA";
                return;
            }
        }

        private void ReadAll_Click(object sender, EventArgs e)
        {
            if (cbSerial.Enabled)
            {
                MessageBox.Show("Please connnect serial port!", "Warning");
                return;
            }
            else
            {
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 3;
                SendBuffer[3] = (Byte)Convert.ToInt32(SlaveAddress.Text, 16);
                SendBuffer[4] = 0;
                SendBuffer[5] = 0;
                SendBuffer[6] = 0;
                SendBuffer[7] = (Byte)(0x100 - ((Byte)(SendBuffer[3] + SendBuffer[4] + SendBuffer[5] + SendBuffer[6])));
                SelectUart.Write(SendBuffer, 0, 8);
                Thread.Sleep(1000);
                try
                {
                    SelectUart.Read(ReceiveBuffer, 0, 256 + 7);
                }
                catch
                {
                    MessageBox.Show("Timeout!", "Error");
                    return;
                }
                int i, checksum = ReceiveBuffer[3] + ReceiveBuffer[4] + ReceiveBuffer[5];
                for (i = 0;i < 256; i++)
                {
                    checksum += ReceiveBuffer[6 + i];
                }
                checksum = 0x100 - (Byte)checksum;
                if ((Byte)checksum == ReceiveBuffer[6 + 256])
                {
                    for (i = 0; i < 256; i++)
                    {
                        int val = ReceiveBuffer[6 + i];
                        RegData.CurrentCell = RegData[i%16 + 1, i/16];
                        RegData.CurrentCell.Value = val.ToString("X2");

                    }
                }
                else
                {
                    MessageBox.Show("Read fail!", "Error");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cbSerial.Enabled)
            {
                MessageBox.Show("Please connnect serial port!", "Warning");
                return;
            }
            else
            {
                SendBuffer[0] = 0xFF;
                SendBuffer[1] = 0x55;
                SendBuffer[2] = 2;
                SendBuffer[3] = (Byte)Convert.ToInt32(SlaveAddress.Text, 16);
                SendBuffer[4] = 0x00;
                SendBuffer[5] = 0x80;
                int checksum = SendBuffer[3] + SendBuffer[4] + SendBuffer[5];
                for (int i = 0; i < 256; i++)
                {
                    SendBuffer[6 + i] = (Byte)Convert.ToInt32(RegData[i%16 + 1, i/16].Value.ToString(), 16);
                    checksum += SendBuffer[6 + i];
                }
                SendBuffer[256 + 6] = (Byte)(0x100 - checksum);
                SelectUart.Write(SendBuffer, 0, 256 + 7);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileDialog = new OpenFileDialog();
            FileDialog.Filter = "bin file|*.bin";
            FileDialog.RestoreDirectory = true;
            FileDialog.FilterIndex = 1;
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                fName = FileDialog.FileName;
                FileStream fs = new FileStream(fName, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                Byte[] write_buffer = new Byte[256];
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
            SaveFileDialog FileDialog = new SaveFileDialog();
            FileDialog.Filter = "bin file|*.bin";
            FileDialog.RestoreDirectory = true;
            FileDialog.FilterIndex = 1;
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                fName = FileDialog.FileName;
                FileStream fs = new FileStream(fName, FileMode.OpenOrCreate);
                BinaryWriter bw = new BinaryWriter(fs);
                Byte[] write_buffer = new Byte[256];
                for (int i = 0; i < 256; i++)
                {
                    write_buffer[i] = (Byte)Convert.ToInt32(RegData[i % 16 + 1, i / 16].Value.ToString(), 16);
                }
                bw.Write(write_buffer);
                bw.Flush();
                bw.Close();
                fs.Close();
            }
        }
    }
}
