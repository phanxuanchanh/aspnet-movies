using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Common.SystemInformation
{
    public class SystemInfo
    {
        private string osName;
        private string memoryUsed;
        private string totalMemory;
        private string ipAddress;
        private string extenalIpAddress;
        private string ffmpegPath;
        private string[] path_EnvironmentVariable;

        public string OSName { get { return osName; } }
        public string MemoryUsed { get { return memoryUsed; } }
        public string TotalMemory { get { return totalMemory; } }
        public string IpAddress { get { return ipAddress; } }
        public string ExtenalIpAddress { get { return extenalIpAddress; } }
        public string FfmpegPath { get { return (ffmpegPath == null) ? "null" : ffmpegPath; } }
        public string[] Path_EnvironmentVariable { get { return path_EnvironmentVariable; } }

        public SystemInfo()
        {
            osName = GetOSName();
            memoryUsed = GetMemoryUsed(MemorySizeInfo.MB) + " MB";
            totalMemory = GetTotalMemory(MemorySizeInfo.MB) + " MB";
            ipAddress = GetIPAddress();
            extenalIpAddress = GetExtenalIPAddress();
            ffmpegPath = GetFfmpegPath();
            path_EnvironmentVariable = GetPath_EnvironmentVariable();
        }

        public enum MemorySizeInfo { Byte, KB, MB, GB };

        private double GetMemoryUsed(MemorySizeInfo memorySizeInfo = MemorySizeInfo.MB)
        {
            Process process = Process.GetCurrentProcess();
            double memory = process.PrivateMemorySize64;
            process.Dispose();
            process = null;
            if (memorySizeInfo == MemorySizeInfo.Byte)
                return memory;
            else if (memorySizeInfo == MemorySizeInfo.KB)
                return memory / (double)1024;
            else if (memorySizeInfo == MemorySizeInfo.MB)
                return memory / (double)(1024 * 1024);
            else
                return memory / (double)(1024 * 1024 * 1024);
        }

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

        private double GetTotalMemory(MemorySizeInfo memorySizeInfo = MemorySizeInfo.MB)
        {
            long memory;
            GetPhysicallyInstalledSystemMemory(out memory);
            if (memorySizeInfo == MemorySizeInfo.Byte)
                return memory * 1024;
            else if (memorySizeInfo == MemorySizeInfo.KB)
                return memory;
            else if (memorySizeInfo == MemorySizeInfo.MB)
                return memory / (double)1024;
            else
                return memory / (double)(1024 * 1024);
        }

        private string GetOSName()
        {
            return System.Environment.OSVersion.VersionString;
        }

        private string GetFfmpegPath()
        {
            return System.Environment.GetEnvironmentVariable("ffmpeg");
        }

        private string[] GetPath_EnvironmentVariable()
        {
            return System.Environment.GetEnvironmentVariable("PATH").Split(';');
        }

        private string GetIPAddress()
        {
            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }

        private string GetExtenalIPAddress()
        {
            using(WebClient webClient = new WebClient())
            {
                string raw = webClient.DownloadString("http://checkip.dyndns.org");
                return Regex.Replace(raw, "<.*?>", String.Empty)
                .Replace("Current IP Check", String.Empty)
                .Replace("Current IP Address: ", String.Empty);
            }
        }
    }
}
