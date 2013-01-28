﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using GitCommands;
using ResourceManager.Translation;

namespace GitUI
{
    public sealed partial class FormBranchSmall : GitModuleForm
    {
        readonly string _startPoint;
        private readonly TranslationString _noRevisionSelected =
            new TranslationString("Select 1 revision to create the branch on.");
        private readonly TranslationString _branchNameIsEmpty =
            new TranslationString("Enter branch name.");
        private readonly TranslationString _branchNameIsNotValud =
            new TranslationString("“{0}” is not valid branch name.");

        public FormBranchSmall(GitUICommands aCommands, string startPoint = null)
            : base(aCommands)
        {
            _startPoint = startPoint;
            InitializeComponent();
            Translate();
        }

        public GitRevision Revision { get; set; }

        private void OkClick(object sender, EventArgs e)
        {
            var branchName = BranchNameTextBox.Text.Trim();

            if (branchName.IsNullOrWhiteSpace())
            {// ""
                MessageBox.Show(_branchNameIsEmpty.Text, Text);
                DialogResult = DialogResult.None;
                return;
            }
            if (!Module.CheckBranchFormat(branchName))
            {// invalid branch name
                MessageBox.Show(string.Format(_branchNameIsNotValud.Text, branchName), Text);
                DialogResult = DialogResult.None;
                return;
            }
            try
            {
                if (Revision == null && _startPoint == null)
                {
                    MessageBox.Show(this, _noRevisionSelected.Text, Text);
                    return;
                }
                var branchCmd = GitCommandHelpers.BranchCmd(branchName, _startPoint ?? Revision.Guid,
                                                                  CheckoutAfterCreate.Checked);
                FormProcess.ShowDialog(this, branchCmd);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
    }
}