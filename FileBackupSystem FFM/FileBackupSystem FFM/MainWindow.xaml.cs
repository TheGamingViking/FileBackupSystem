﻿using System;
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

namespace FileBackupSystem_FFM
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

        private void btn_schedule_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings();
            settingsWindow.Show();
        }

        private void txtBox_filepathInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (txtBox_filepathInput.Text != "" || txtBox_filepathInput.Text != " ")
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Content = txtBox_filepathInput.Text;
                listBox.Items.Add(checkbox);
            }
            txtBox_filepathInput.Clear();
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btn_history_Click(object sender, RoutedEventArgs e)
        {
            History historyWindow = new History();
            historyWindow.Show();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btn_closeToSleep_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_runBackup_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Unchecked folders will be removed from list of folders to back-up.\nAre you sure you wish to back-up selected folders?", "Confirm backup", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                List<CheckBox> toRemove = new List<CheckBox>();
                foreach (CheckBox item in listBox.Items)
                {
                    if (!(bool)item.IsChecked)
                    {
                        toRemove.Add(item);
                    }
                }
                foreach (CheckBox item in toRemove)
                {
                    listBox.Items.Remove(item);
                }
                toRemove.Clear();
            }
            else
            {

            }
        }
    }
}
