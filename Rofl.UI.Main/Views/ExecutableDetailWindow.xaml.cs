﻿using Microsoft.WindowsAPICodePack.Dialogs;
using Rofl.Executables;
using Rofl.Executables.Models;
using Rofl.Executables.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace Rofl.UI.Main.Views
{
    /// <summary>
    /// Interaction logic for ExecutableDetailWindow.xaml
    /// </summary>
    public partial class ExecutableDetailWindow : Window
    {
        private LeagueExecutable _executable;
        private bool _isEditMode;

        public ExecutableDetailWindow()
        {
            InitializeComponent();
            _isEditMode = false;
        }

        public ExecutableDetailWindow(LeagueExecutable executable)
        {
            if (executable == null) { throw new ArgumentNullException(nameof(executable)); }

            InitializeComponent();

            LoadLeagueExecutable(executable);
            _isEditMode = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.MinWidth = this.ActualWidth;
            this.MinHeight = this.ActualHeight;
            this.MaxHeight = this.ActualHeight;
        }

        private void LoadLeagueExecutable(LeagueExecutable executable)
        {
            if (executable == null) { throw new ArgumentNullException(nameof(executable)); }

            _executable = executable;
            TargetTextBox.Text = executable.TargetPath;
            NameTextBox.Text = executable.Name;
            StartPathTextBox.Text = executable.StartFolder;
            PatchTextBox.Text = executable.PatchNumber;
            LaunchArgsTextBox.Text = executable.LaunchArguments;
            ModifiedDateTextBox.Text = executable.ModifiedDate.ToString("o", CultureInfo.InvariantCulture);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.DataContext is ExecutableManager context)) { return; }

            try
            {
                _executable.Name = NameTextBox.Text;
                _executable.TargetPath = TargetTextBox.Text;
                _executable.LaunchArguments = LaunchArgsTextBox.Text;

                if (_isEditMode)
                {
                    this.Close();
                }
                else
                {
                    context.AddExecutable(_executable);
                }
                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show
                    (
                        (TryFindResource("ExecutableSaveNullText") as String) +
                        $"\n{ex.ToString()}",
                        TryFindResource("ExecutableSaveNullTitle") as String,
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation
                    );
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TargetButton_Click(object sender, RoutedEventArgs e)
        {
            using (var folderDialog = new CommonOpenFileDialog())
            {
                folderDialog.Title = TryFindResource("ExecutableSelectDialogText") as String;
                folderDialog.IsFolderPicker = false;
                folderDialog.AddToMostRecentlyUsedList = false;
                folderDialog.AllowNonFileSystemItems = false;
                folderDialog.EnsureFileExists = true;
                folderDialog.EnsurePathExists = true;
                folderDialog.EnsureReadOnly = false;
                folderDialog.EnsureValidNames = true;
                folderDialog.Multiselect = false;
                folderDialog.ShowPlacesList = true;

                folderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                folderDialog.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var selectedExe = folderDialog.FileName;

                    if (ExeTools.CheckExecutableFile(selectedExe))
                    {
                        var newExe = ExeTools.CreateNewLeagueExecutable(selectedExe);
                        LoadLeagueExecutable(newExe);
                    }
                    else
                    {
                        var msgBoxResult = MessageBox.Show
                            (
                                TryFindResource("ExecutableSelectInvalidErrorText") as String,
                                TryFindResource("ExecutableSelectInvalidErrorTitle") as String,
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation
                            );

                        if(msgBoxResult == MessageBoxResult.OK)
                        {
                            TargetButton_Click(null, null);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }

    }
}