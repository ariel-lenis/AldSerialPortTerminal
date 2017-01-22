using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Management;
using System.Windows;

namespace ALDSerialPort
{
    public class ALDSerialPort
    {
        SerialPort serial;

        public SerialPort Serial
        {
            get { return serial; }
        }

        public event SerialErrorReceivedEventHandler ErrorReceived;
        public event SerialDataReceivedEventHandler DataReceived;

        public bool Kbhit()
        {
            if (!serial.IsOpen) return false;
            return serial.BytesToRead > 0;
        }
        public void ReadAllJunk()
        {
            if (!serial.IsOpen) return;
            while (serial.BytesToRead > 0)
                serial.ReadByte();
        }

        public static List<SerialPreviousInformation> EnumeratePorts()
        {
            string targetCaptionProperty = "Caption";

            SerialConfiguration c;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
            List<string> Descriptions = new List<string>();
            foreach (ManagementObject queryObj in searcher.Get())
                if (queryObj.ContainsProperty(targetCaptionProperty) && queryObj[targetCaptionProperty] != null && queryObj[targetCaptionProperty].ToString().Contains("(COM"))
                    Descriptions.Add(queryObj[targetCaptionProperty].ToString());

            List<SerialPreviousInformation> list = SerialPort.GetPortNames().ToList().ConvertAll(x => new SerialPreviousInformation() { Port = x });
            string information;
            int previdx;
            foreach (var i in list)
            {
                information = "";
                previdx = int.MaxValue;
                foreach (var j in Descriptions.Where(x => x.Contains(i.Port)))
                {

                    int idx = j.IndexOf(i.Port);
                    if (idx == j.Length - 1 || !(j[idx + 1] >= '0' && j[idx + 1] <= '9') && idx < previdx)
                    {
                        information = j;
                        previdx = idx;
                    }
                }
                i.Description = information;
            }
            return list;
        }
        public ALDSerialPort(TotalConfiguration config)
        {
            serial = new SerialPort(config.COM, (int)config.Configuration.ConfigBitsPerSecond, config.Configuration.ConfigParity, (int)config.Configuration.ConfigDataBits, config.Configuration.ConfigStopBits);
            serial.ErrorReceived += new SerialErrorReceivedEventHandler(serial_ErrorReceived);
            serial.DataReceived += new SerialDataReceivedEventHandler(serial_DataReceived);
            serial.Encoding = Encoding.GetEncoding("Windows-1252");
        }
        void serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (DataReceived != null)
                DataReceived(this, e);
        }
        void serial_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (ErrorReceived != null)
                ErrorReceived(this, e);
        }
        public void OpenPort()
        {
            serial.Open();
            serial.Encoding = System.Text.ASCIIEncoding.ASCII;
        }
        public void ClosePort()
        {

            serial.ErrorReceived -= new SerialErrorReceivedEventHandler(serial_ErrorReceived);
            serial.DataReceived -= new SerialDataReceivedEventHandler(serial_DataReceived);

            serial.DiscardInBuffer();
            serial.DiscardOutBuffer();
            var stream = serial.BaseStream;
            stream.Flush();
            stream.Close();
            stream.Dispose();

            serial.Close();

            serial.Dispose();

            serial = null;
            stream = null;
            GC.Collect();

        }
        public bool IsOpen
        {
            get { return serial != null && serial.IsOpen; }
        }
        public bool SendData(byte[] data)
        {
            try
            {
                serial.Write(data, 0, data.Length);
                return true;
            }
            catch { return false; }
        }
        public bool SendData(string data)
        {
            try
            {
                serial.Write(data);
                return true;
            }
            catch { return false; }
        }
    }
}
