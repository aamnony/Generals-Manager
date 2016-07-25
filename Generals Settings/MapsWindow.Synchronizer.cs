using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace Generals_Manager
{
    public partial class MapsWindow : Window
    {
        private static string GAME_FOLDER = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            @"Command and Conquer Generals Zero Hour Data\Maps");

        private BackgroundWorker workerSync;

        private void AddMaps(List<string> onlineMaps)
        {
            for (int i = 0; i < onlineMaps.Count; i++)
            {
                var src = Path.Combine(ALL_MAPS_PATH, onlineMaps[i]);
                var dst = Path.Combine(GAME_FOLDER, onlineMaps[i]);
                if (!Directory.Exists(dst))
                {
                    try
                    {
                        FileSystem.CopyDirectory(src, dst);
                        workerSync.ReportProgress(-1, onlineMaps[i] + " added");
                    }
                    catch (Exception e)
                    {
                        workerSync.ReportProgress(-1, e.Message);
                    }
                }
                else
                {
                    workerSync.ReportProgress(-1);
                }
            }
        }

        private void AppendLog(string s)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                txtLog.AppendText(DateTime.Now.ToString("HH:mm:ss.fff >> ") + s + Environment.NewLine);
            }
        }

        private void RemoveMaps(string[] offlineMaps, List<string> onlineMaps)
        {
            for (int i = 0; i < offlineMaps.Length; i++)
            {
                var shortMap = Path.GetFileName(offlineMaps[i]);
                if (!onlineMaps.Contains(shortMap))
                {
                    try
                    {
                        File.SetAttributes(offlineMaps[i], FileAttributes.Normal); // Stupid Windows...
                        FileSystem.DeleteDirectory(offlineMaps[i], DeleteDirectoryOption.DeleteAllContents);
                        workerSync.ReportProgress(-1, shortMap + " removed");
                    }
                    catch (Exception e)
                    {
                        workerSync.ReportProgress(-1, e.Message);
                    }
                }
                else
                {
                    workerSync.ReportProgress(-1);
                }
            }
        }

        private void workerSync_DoWork(object sender, DoWorkEventArgs e)
        {
            var onlineMaps = new List<string>(WebUtils.DownloadInGameMaps());

            var offlineMaps = Directory.GetDirectories(GAME_FOLDER, "*", System.IO.SearchOption.TopDirectoryOnly);
            workerSync.ReportProgress(offlineMaps.Length + onlineMaps.Count);

            // Delete unnecessary maps.
            RemoveMaps(offlineMaps, onlineMaps);

            // Move needed maps to game folder.
            AddMaps(onlineMaps);
        }

        private void workerSync_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            AppendLog(e.UserState as string);
            if (e.ProgressPercentage > 0)
            {
                progressBar1.Maximum = e.ProgressPercentage;
                progressBar1.Value = 0;
            }
            else
            {
                progressBar1.Value++;
            }
        }

        private void workerSync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AppendLog("Done");
        }
    }
}