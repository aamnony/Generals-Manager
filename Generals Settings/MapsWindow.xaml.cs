using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;

namespace Generals_Manager
{
    /// <summary>
    /// Interaction logic for MapsWindow.xaml
    /// </summary>
    public partial class MapsWindow : Window
    {
        private readonly string ALL_MAPS_PATH = Directory.GetCurrentDirectory().Replace('/', '\\') + "\\Maps"; // Stupid M$...

        public MapsWindow()
        {
            InitializeComponent();
            WebRequest.DefaultWebProxy = null;

            workerDownload = new BackgroundWorker();
            workerDownload.DoWork += workerDownload_DoWork;
            workerDownload.RunWorkerCompleted += workerDownload_RunWorkerCompleted;
            workerUpload = new BackgroundWorker();
            workerUpload.DoWork += workerUpload_DoWork;
            workerUpload.RunWorkerCompleted += workerUpload_RunWorkerCompleted;

            workerSync = new BackgroundWorker();
            workerSync.DoWork += workerSync_DoWork;
            workerSync.ProgressChanged += workerSync_ProgressChanged;
            workerSync.WorkerReportsProgress = true;

            chkShowCheckedOnly.IsChecked = null;
            workerDownload.RunWorkerAsync();
            workerSync.RunWorkerAsync();
        }
    }
}