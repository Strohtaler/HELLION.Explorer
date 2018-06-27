﻿using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using HELLION.DataStructures.UI;
using HELLION.DataStructures.Utilities;
using Newtonsoft.Json.Linq;

namespace HELLION.Explorer
{
    public partial class JsonDataViewForm : Form
    {
        //Create style for highlighting
        //TextStyle brownStyle = new TextStyle(Brushes.Brown, null, FontStyle.Regular);

        /// <summary>
        /// Property to get/set the _isDirty bool.
        /// </summary>
        public bool IsDirty
        {
            get => _isDirty;
            private set
            {
                _isDirty = value;
                // Enable or disable the Apply Changes menu option.
                applyChangesToolStripMenuItem.Enabled = value;
                // Update the form name
                RefreshJsonDataViewFormTitleText();
            }
        }

        /// <summary>
        /// Field that determines whether the text has been changed.
        /// </summary>
        private bool _isDirty = false;

        /// <summary>
        /// Stores a copy of the unmodified text - updated after the apply changes operation.
        /// </summary>
        private string AppliedText = null;

        private string FormTitleText = null;

        public Json_TN SourceNode { get; } = null;

        public JsonDataViewForm()
        {
            InitializeComponent();
            Icon = HellionExplorerProgram.MainForm.Icon;
            fastColoredTextBox1.Language = Language.JS;
            applyChangesToolStripMenuItem.Enabled = false;
        }

        public JsonDataViewForm(Json_TN passedSourceNode) : this()
        {
            SourceNode = passedSourceNode ?? throw new NullReferenceException("passedSourceNode was null.");
            FormTitleText = passedSourceNode.FullPath;
            Text = FormTitleText;
            AppliedText = passedSourceNode.JData.ToString();
            fastColoredTextBox1.Text = AppliedText;
            // Required as setting the FastColouredTextBox triggers the _isDirty
            IsDirty = false;
        }

        private void RefreshJsonDataViewFormTitleText()
        {
            if (IsDirty) Text = FormTitleText + "*";
            else Text = FormTitleText;
        }

        // This really needs to happen before form closing
        private void JsonDataViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDirty)
            {
                // Unsaved changes, prompt the user to apply them before closing the window.
                DialogResult result = MessageBox.Show("Do you want to apply changes to the main file?",
                    "Un-Applied Changes Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                switch (result)
                {
                    case DialogResult.Cancel:
                        return;

                    case DialogResult.Yes:
                        MessageBox.Show("User selected to apply changes.", "NonImplemented Notice",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
            // Remove the current JsonDataViewForm from the jsonDataViews list
            HellionExplorerProgram.jsonDataViews.Remove(this);
            GC.Collect();
        }

        /// <summary>
        /// Handles setting folding markers on the FastColoredTextBox and setting the _isDirty bool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fastColoredTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDirty = true;
            Debug.Print("Text Changed called");

            //clear previous highlighting
            //e.ChangedRange.ClearStyle(brownStyle);
            //highlight tags
            //e.ChangedRange.SetStyle(brownStyle, "<[^>]+>");

            //clear folding markers of changed range
            e.ChangedRange.ClearFoldingMarkers();
            //set folding markers
            e.ChangedRange.SetFoldingMarkers("{", "}");
            e.ChangedRange.SetFoldingMarkers(Regex.Escape(@"["), Regex.Escape(@"]"));

            //e.ChangedRange.SetFoldingMarkers(@"#region\b", @"#endregion\b");


            Refresh_toolStripStatusLabelSerialisatonStatus();


        }

        #region menuStrip1

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.ShowFindDialog();
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.ShowReplaceDialog();
        }

        private void applyChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ApplyChanges()) IsDirty = false;
        }

        #endregion

        /// <summary>
        /// De-serialises the text from the FastColoredTextBox and replaces the
        /// source JToken with the results if the serialisation was successful.
        /// </summary>
        /// <returns></returns>
        private bool ApplyChanges()
        {



            // AppliedText = fastColoredTextBox1.Text;
                

            return false;
        }


        private bool DoesTextSerialise => StringExtensions.TryParseJson(fastColoredTextBox1.Text, out JToken tmp);


        private void Refresh_toolStripStatusLabelSerialisatonStatus()
        {

           toolStripStatusLabelSerialisatonStatus.Text = 
                String.Format("Passes Serialisation: {0}", DoesTextSerialise);

        }


    }
}
