﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GardenWindowsService
{
    public partial class GardenWindowsService : ServiceBase
    {
        private Process process;
        private const string eventSource = "GardenWindows";

        public GardenWindowsService()
        {
            InitializeComponent();

            if (!EventLog.SourceExists(eventSource))
                EventLog.CreateEventSource(eventSource, "Application");
            EventLog.WriteEntry(eventSource, "Service Initializing", EventLogEntryType.Information, 0);
        }

        protected override void OnStart(string[] args)
        {
            process = new Process
            {
                StartInfo =
                {
                    FileName = "garden-windows.exe",
                    Arguments = "--listenNetwork=tcp -listenAddr=0.0.0.0:9241 -containerGraceTime=1h -containerizerURL=http://localhost:1788",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                }
            };
            process.EnableRaisingEvents = true;
            process.Exited += process_Exited;

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => EventLog.WriteEntry(eventSource, e.Data, EventLogEntryType.Information, 0);
            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => EventLog.WriteEntry(eventSource, e.Data, EventLogEntryType.Warning, 0);

            EventLog.WriteEntry(eventSource, "Starting", EventLogEntryType.Information, 0);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

        void process_Exited(object sender, EventArgs e)
        {
            EventLog.WriteEntry(eventSource, "Exiting", EventLogEntryType.Error, 0);
            this.ExitCode = 0XDEAD;
            System.Environment.Exit(this.ExitCode);
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (!process.HasExited)
            {
                process.Kill();
            }
        }
    }
}
