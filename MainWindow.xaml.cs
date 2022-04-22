using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;


namespace folder_maker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void ListBoxFolders_Select(object sender, SelectionChangedEventArgs e)
        {
            if (listFolders.SelectedItems.Count != 0)
            {
                folderName.Text = listFolders.SelectedItems[0].ToString();

            }
        }
        private void ButtonSaveFolder_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(folderName.Text) && !listFolders.Items.Contains(folderName.Text))
            {
                listFolders.Items.Add(folderName.Text);
                folderName.Clear();

                // Remove entry being edited
                if(listFolders.SelectedItems.Count != 0)
                {
                    listFolders.Items.Remove(listFolders.SelectedItems[0]);
                }
            }
        }

        private void ButtonRemoveFolder_Click(object sender, RoutedEventArgs e)
        {
            while (listFolders.SelectedItems.Count > 0)
            {
                listFolders.Items.Remove(listFolders.SelectedItems[0]);
            }
        }

        private void ButtonClearFolders_Click(object sender, RoutedEventArgs e)
        {
            listFolders.Items.Clear();
        }

        private void ButtonImportFolders_Click(object sender, RoutedEventArgs e)
        {
            // Get data from file
            System.Windows.Forms.OpenFileDialog selectFileDialog = new();
            selectFileDialog.Filter = "CSV Files(*.csv)| *.csv";
            if (selectFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Read file
                var lines = File.ReadLines(selectFileDialog.FileName);
                foreach (string line in lines)
                {
                    // Remove any quotes
                    string folderName = line.Replace("\"", "");

                    // Add if it doesn't already exist
                    if (!listFolders.Items.Contains(folderName))
                    {
                        listFolders.Items.Add(folderName);
                    }
                }
            }

        }

        private void ButtonSelectLocation_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog selectFolderDialog = new();
            if (selectFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                createFolderPath.Text = selectFolderDialog.SelectedPath;
            }
        }

        private void ButtonCreateFolders_Click(object sender, RoutedEventArgs e)
        {
            string basePath = createFolderPath.Text;

            // Check if folder has been selected
            if (string.IsNullOrWhiteSpace(basePath))
            {
                MessageBox.Show("Please select a folder location");
                return;
            }

            // Check if there is anything to create
            int numOfFolders = listFolders.Items.Count;
            if (numOfFolders == 0)
            {
                MessageBox.Show("No folders to create");
                return;
            }

            // Loop through and create folders
            foreach (var folder in listFolders.Items)
            {
                string folderCreatePath = basePath + "\\" + folder.ToString();

                if (Directory.Exists(folderCreatePath))
                {
                    Console.WriteLine("That path exists already.");
                    continue;
                }

                // Try to create the directory.
                Directory.CreateDirectory(folderCreatePath);
            }

            MessageBox.Show("Folders created");
            listFolders.Items.Clear();
            createFolderPath.Text = null;
        }


    }
}
