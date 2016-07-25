using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Generals_Manager
{
    public partial class MapsWindow : Window
    {
        private bool mUploadingData;
        private BackgroundWorker workerDownload;
        private BackgroundWorker workerUpload;

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            List<string> badList = new List<string>();
            List<string> ingameList = new List<string>();
            foreach (CheckBoxListViewItem item in listMaps.Items.SourceCollection)
            {
                if (item.IsBad)
                {
                    badList.Add(item.Text);
                }
                if (item.IsChecked)
                {
                    ingameList.Add(item.Text);
                }
            }
            lblStatus.Text = "Uploading data...";
            mUploadingData = true;

            workerUpload.RunWorkerAsync(new List<string>[] { badList, ingameList });
        }

        private void checkBoxItem_CheckChanged(object sender, RoutedEventArgs e)
        {
            UpdateInGameMapsCountStatus();
        }

        private void chkShowCheckedOnly_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (chkShowCheckedOnly.IsChecked.HasValue)
            {
                listMaps.Items.Filter = (x) =>
                {
                    CheckBoxListViewItem item = x as CheckBoxListViewItem;
                    return item.IsChecked == chkShowCheckedOnly.IsChecked;
                };
            }
            else
            {
                listMaps.Items.Filter = null;
            }
        }

        private void toggleBad_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBoxListViewItem item in listMaps.SelectedItems)
            {
                item.IsBad = !item.IsBad;
            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                listMaps.Items.Filter = null;
            }
            else
            {
                listMaps.Items.Filter = (x) =>
                {
                    CheckBoxListViewItem item = x as CheckBoxListViewItem;
                    return (item.Text.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                };
            }
        }

        private void UpdateInGameMapsCountStatus()
        {
            int checkedCount = listMaps.Items.SourceCollection.Cast<CheckBoxListViewItem>().Count(x => x.IsChecked);

            lblStatus.Text = String.Format("Maps in-game: {0}", checkedCount);
            lblStatus.Foreground = new SolidColorBrush(checkedCount < 100 ? Colors.Black : Colors.Red);
        }

        private void workerDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            string[][] downloadedData = WebUtils.DownloadAll();
            var ingameMaps = downloadedData[0];
            var badMaps = downloadedData[1];

            var mapNames = Directory.GetDirectories(ALL_MAPS_PATH, "*", System.IO.SearchOption.TopDirectoryOnly);

            var maps = new Map[mapNames.Length];
            for (int i = 0; i < maps.Length; i++)
            {
                maps[i].Name = Path.GetFileName(mapNames[i]);
                maps[i].InGame = ingameMaps.Contains(maps[i].Name);
                maps[i].Bad = badMaps.Contains(maps[i].Name);
            }
            e.Result = maps;
        }

        private void workerDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ObservableCollection<CheckBoxListViewItem> items = new ObservableCollection<CheckBoxListViewItem>();
            foreach (Map map in e.Result as Map[])
            {
                items.Add(new CheckBoxListViewItem(map));
            }
            listMaps.ItemsSource = items;
            UpdateInGameMapsCountStatus();
        }

        private void workerUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            var pair = (List<string>[])e.Argument;
            WebUtils.UploadAll(pair[0], pair[1]);
        }

        private void workerUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mUploadingData = false;
            workerSync.RunWorkerAsync();
            lblStatus.Text = "Finished uploading";
        }
    }
}