﻿using System;
using Glyssen.Controls;
using SIL.Windows.Forms.Widgets.BetterGrid;

namespace Glyssen.Dialogs
{
	partial class VoiceActorAssignmentDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VoiceActorAssignmentDlg));
			this.m_btnAssignActor = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.m_contextMenuCharacters = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.m_tmiCreateNewGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.m_contextMenuCharacterGroups = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.m_assignActorToGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.m_unAssignActorFromGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.m_splitGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.l10NSharpExtender1 = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.m_btnUpdateGroup = new System.Windows.Forms.Button();
			this.m_lblCharacterGroups = new System.Windows.Forms.Label();
			this.m_lblVoiceActors = new System.Windows.Forms.Label();
			this.m_linkClose = new System.Windows.Forms.LinkLabel();
			this.m_linkEdit = new System.Windows.Forms.LinkLabel();
			this.m_helpIcon = new System.Windows.Forms.PictureBox();
			this.m_contextMenuVoiceActors = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.m_assignActorToGroupToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.m_editActorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.m_deleteActorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.m_characterGroupGrid = new Glyssen.Controls.AutoGrid();
			this.GroupNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CharacterIds = new Glyssen.Controls.DataGridViewListBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CharStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EstimatedHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_voiceActorGrid = new Glyssen.Controls.VoiceActorInformationGrid();
			this.m_saveStatus = new Glyssen.Controls.SaveStatus();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.m_contextMenuCharacters.SuspendLayout();
			this.m_contextMenuCharacterGroups.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_helpIcon)).BeginInit();
			this.m_contextMenuVoiceActors.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_characterGroupGrid)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_btnAssignActor
			// 
			this.m_btnAssignActor.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_btnAssignActor, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_btnAssignActor, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_btnAssignActor, "DialogBoxes.VoiceActorAssignmentDlg.AssignActorButton");
			this.m_btnAssignActor.Location = new System.Drawing.Point(438, 134);
			this.m_btnAssignActor.Name = "m_btnAssignActor";
			this.m_btnAssignActor.Size = new System.Drawing.Size(48, 36);
			this.m_btnAssignActor.TabIndex = 2;
			this.m_btnAssignActor.Text = "Assign\r\n<<";
			this.m_btnAssignActor.UseVisualStyleBackColor = true;
			this.m_btnAssignActor.Click += new System.EventHandler(this.m_btnAssignActor_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label1, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label1, "DialogBoxes.VoiceActorAssignmentDlg.AssignActors");
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(199, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Assign Voice Actors to Character Groups";
			// 
			// m_contextMenuCharacters
			// 
			this.m_contextMenuCharacters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_tmiCreateNewGroup});
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_contextMenuCharacters, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_contextMenuCharacters, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.m_contextMenuCharacters, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this.m_contextMenuCharacters, "DialogBoxes.VoiceActorAssignmentDlg.contextMenuStrip1");
			this.m_contextMenuCharacters.Name = "m_contextMenuCharacters";
			this.m_contextMenuCharacters.Size = new System.Drawing.Size(297, 26);
			this.m_contextMenuCharacters.Opening += new System.ComponentModel.CancelEventHandler(this.m_contextMenuCharacters_Opening);
			// 
			// m_tmiCreateNewGroup
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_tmiCreateNewGroup, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_tmiCreateNewGroup, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.m_tmiCreateNewGroup, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this.m_tmiCreateNewGroup, "DialogBoxes.VoiceActorAssignmentDlg.createANewGroupWithTheseCharactersToolStripMe" +
        "nuItem");
			this.m_tmiCreateNewGroup.Name = "m_tmiCreateNewGroup";
			this.m_tmiCreateNewGroup.Size = new System.Drawing.Size(296, 22);
			this.m_tmiCreateNewGroup.Text = "Create a New Group with these Characters";
			this.m_tmiCreateNewGroup.Click += new System.EventHandler(this.m_tmiCreateNewGroup_Click);
			// 
			// m_contextMenuCharacterGroups
			// 
			this.m_contextMenuCharacterGroups.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_assignActorToGroupToolStripMenuItem,
            this.m_unAssignActorFromGroupToolStripMenuItem,
            this.m_splitGroupToolStripMenuItem});
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_contextMenuCharacterGroups, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_contextMenuCharacterGroups, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_contextMenuCharacterGroups, "DialogBoxes.VoiceActorAssignmentDlg.contextMenuStrip1");
			this.m_contextMenuCharacterGroups.Name = "m_contextMenuCharacterGroups";
			this.m_contextMenuCharacterGroups.Size = new System.Drawing.Size(286, 70);
			this.m_contextMenuCharacterGroups.Opening += new System.ComponentModel.CancelEventHandler(this.m_contextMenuCharacterGroups_Opening);
			// 
			// m_assignActorToGroupToolStripMenuItem
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_assignActorToGroupToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_assignActorToGroupToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_assignActorToGroupToolStripMenuItem, "DialogBoxes.VoiceActorAssignmentDlg.ContextMenus.AssignSelectedActorToSelectedGro" +
        "up");
			this.m_assignActorToGroupToolStripMenuItem.Name = "m_assignActorToGroupToolStripMenuItem";
			this.m_assignActorToGroupToolStripMenuItem.Size = new System.Drawing.Size(285, 22);
			this.m_assignActorToGroupToolStripMenuItem.Text = "Assign Selected Actor to Selected Group";
			this.m_assignActorToGroupToolStripMenuItem.Click += new System.EventHandler(this.m_assignActorToGroupToolStripMenuItem_Click);
			// 
			// m_unAssignActorFromGroupToolStripMenuItem
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_unAssignActorFromGroupToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_unAssignActorFromGroupToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.m_unAssignActorFromGroupToolStripMenuItem, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this.m_unAssignActorFromGroupToolStripMenuItem, "DialogBoxes.VoiceActorAssignmentDlg.unAssignActorFromGroupToolStripMenuItem");
			this.m_unAssignActorFromGroupToolStripMenuItem.Name = "m_unAssignActorFromGroupToolStripMenuItem";
			this.m_unAssignActorFromGroupToolStripMenuItem.Size = new System.Drawing.Size(285, 22);
			this.m_unAssignActorFromGroupToolStripMenuItem.Text = "Un-Assign Actor from Selected Group";
			this.m_unAssignActorFromGroupToolStripMenuItem.Click += new System.EventHandler(this.m_unAssignActorFromGroupToolStripMenuItem_Click);
			// 
			// m_splitGroupToolStripMenuItem
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_splitGroupToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_splitGroupToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_splitGroupToolStripMenuItem, "DialogBoxes.VoiceActorAssignmentDlg.ContextMenus.SplitSelectedGroup");
			this.m_splitGroupToolStripMenuItem.Name = "m_splitGroupToolStripMenuItem";
			this.m_splitGroupToolStripMenuItem.Size = new System.Drawing.Size(285, 22);
			this.m_splitGroupToolStripMenuItem.Text = "Split Selected Group";
			this.m_splitGroupToolStripMenuItem.Click += new System.EventHandler(this.m_splitGroupToolStripMenuItem_Click);
			// 
			// l10NSharpExtender1
			// 
			this.l10NSharpExtender1.LocalizationManagerId = "Glyssen";
			this.l10NSharpExtender1.PrefixForNewItems = "DialogBoxes.VoiceActorAssignmentDlg";
			// 
			// m_btnUpdateGroup
			// 
			this.m_btnUpdateGroup.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_btnUpdateGroup, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_btnUpdateGroup, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_btnUpdateGroup, "DialogBoxes.VoiceActorAssignmentDlg.UpdateGroup");
			this.m_btnUpdateGroup.Location = new System.Drawing.Point(0, 3);
			this.m_btnUpdateGroup.Name = "m_btnUpdateGroup";
			this.m_btnUpdateGroup.Size = new System.Drawing.Size(185, 23);
			this.m_btnUpdateGroup.TabIndex = 4;
			this.m_btnUpdateGroup.Text = "Update/Edit the Character Group(s)";
			this.m_btnUpdateGroup.UseVisualStyleBackColor = true;
			// 
			// m_lblCharacterGroups
			// 
			this.m_lblCharacterGroups.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.m_lblCharacterGroups.AutoSize = true;
			this.m_lblCharacterGroups.ForeColor = System.Drawing.Color.White;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_lblCharacterGroups, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_lblCharacterGroups, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_lblCharacterGroups, "DialogBoxes.VoiceActorAssignmentDlg.CharacterGroupsTitle");
			this.m_lblCharacterGroups.Location = new System.Drawing.Point(164, 3);
			this.m_lblCharacterGroups.Name = "m_lblCharacterGroups";
			this.m_lblCharacterGroups.Size = new System.Drawing.Size(90, 13);
			this.m_lblCharacterGroups.TabIndex = 1;
			this.m_lblCharacterGroups.Text = "Character Groups";
			// 
			// m_lblVoiceActors
			// 
			this.m_lblVoiceActors.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.m_lblVoiceActors.AutoSize = true;
			this.m_lblVoiceActors.ForeColor = System.Drawing.Color.White;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_lblVoiceActors, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_lblVoiceActors, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_lblVoiceActors, "DialogBoxes.VoiceActorAssignmentDlg.VoiceActorsTitle");
			this.m_lblVoiceActors.Location = new System.Drawing.Point(146, 3);
			this.m_lblVoiceActors.Name = "m_lblVoiceActors";
			this.m_lblVoiceActors.Size = new System.Drawing.Size(67, 13);
			this.m_lblVoiceActors.TabIndex = 2;
			this.m_lblVoiceActors.Text = "Voice Actors";
			// 
			// m_linkClose
			// 
			this.m_linkClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_linkClose.AutoSize = true;
			this.m_linkClose.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_linkClose, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_linkClose, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_linkClose, "DialogBoxes.VoiceActorAssignmentDlg.Close");
			this.m_linkClose.Location = new System.Drawing.Point(326, 3);
			this.m_linkClose.Name = "m_linkClose";
			this.m_linkClose.Size = new System.Drawing.Size(33, 13);
			this.m_linkClose.TabIndex = 5;
			this.m_linkClose.TabStop = true;
			this.m_linkClose.Text = "Close";
			this.m_linkClose.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.m_linkClose_LinkClicked);
			// 
			// m_linkEdit
			// 
			this.m_linkEdit.AutoSize = true;
			this.m_linkEdit.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_linkEdit, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_linkEdit, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_linkEdit, "DialogBoxes.VoiceActorAssignmentDlg.EditVoiceActors");
			this.m_linkEdit.Location = new System.Drawing.Point(3, 3);
			this.m_linkEdit.Name = "m_linkEdit";
			this.m_linkEdit.Size = new System.Drawing.Size(88, 13);
			this.m_linkEdit.TabIndex = 6;
			this.m_linkEdit.TabStop = true;
			this.m_linkEdit.Text = "Edit Voice Actors";
			this.m_linkEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.m_linkEdit_LinkClicked);
			// 
			// m_helpIcon
			// 
			this.m_helpIcon.Cursor = System.Windows.Forms.Cursors.Hand;
			this.m_helpIcon.Image = global::Glyssen.Properties.Resources.helpSmall;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_helpIcon, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_helpIcon, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_helpIcon, "DialogBoxes.VoiceActorAssignmentDlg.pictureBox1");
			this.m_helpIcon.Location = new System.Drawing.Point(216, 9);
			this.m_helpIcon.Name = "m_helpIcon";
			this.m_helpIcon.Size = new System.Drawing.Size(19, 19);
			this.m_helpIcon.TabIndex = 7;
			this.m_helpIcon.TabStop = false;
			this.m_helpIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.m_helpIcon_MouseClick);
			this.m_helpIcon.MouseLeave += new System.EventHandler(this.m_helpIcon_MouseLeave);
			// 
			// m_contextMenuVoiceActors
			// 
			this.m_contextMenuVoiceActors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_assignActorToGroupToolStripMenuItem2,
            this.m_editActorToolStripMenuItem,
            this.m_deleteActorToolStripMenuItem});
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_contextMenuVoiceActors, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_contextMenuVoiceActors, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_contextMenuVoiceActors, "DialogBoxes.VoiceActorAssignmentDlg.contextMenuStrip1");
			this.m_contextMenuVoiceActors.Name = "m_contextMenuVoiceActors";
			this.m_contextMenuVoiceActors.Size = new System.Drawing.Size(318, 70);
			this.m_contextMenuVoiceActors.Opening += new System.ComponentModel.CancelEventHandler(this.m_contextMenuVoiceActors_Opening);
			// 
			// m_assignActorToGroupToolStripMenuItem2
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_assignActorToGroupToolStripMenuItem2, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_assignActorToGroupToolStripMenuItem2, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_assignActorToGroupToolStripMenuItem2, "DialogBoxes.VoiceActorAssignmentDlg.ContextMenus.AssignSelectedActorToSelectedGro" +
        "up");
			this.m_assignActorToGroupToolStripMenuItem2.Name = "m_assignActorToGroupToolStripMenuItem2";
			this.m_assignActorToGroupToolStripMenuItem2.Size = new System.Drawing.Size(317, 22);
			this.m_assignActorToGroupToolStripMenuItem2.Text = "Assign Selected Voice Actor to Selected Group";
			this.m_assignActorToGroupToolStripMenuItem2.Click += new System.EventHandler(this.m_assignActorToGroupToolStripMenuItem_Click);
			// 
			// m_editActorToolStripMenuItem
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_editActorToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_editActorToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_editActorToolStripMenuItem, "DialogBoxes.VoiceActorAssignmentDlg.ContextMenus.EditSelectedActor");
			this.m_editActorToolStripMenuItem.Name = "m_editActorToolStripMenuItem";
			this.m_editActorToolStripMenuItem.Size = new System.Drawing.Size(317, 22);
			this.m_editActorToolStripMenuItem.Text = "Edit Selected Voice Actor";
			this.m_editActorToolStripMenuItem.Click += new System.EventHandler(this.m_editActorToolStripMenuItem_Click);
			// 
			// m_deleteActorToolStripMenuItem
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_deleteActorToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_deleteActorToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.m_deleteActorToolStripMenuItem, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this.m_deleteActorToolStripMenuItem, "DialogBoxes.VoiceActorAssignmentDlg.deleteSelectedVoiceActorToolStripMenuItem");
			this.m_deleteActorToolStripMenuItem.Name = "m_deleteActorToolStripMenuItem";
			this.m_deleteActorToolStripMenuItem.Size = new System.Drawing.Size(317, 22);
			this.m_deleteActorToolStripMenuItem.Text = "Delete Selected Voice Actor";
			this.m_deleteActorToolStripMenuItem.Click += new System.EventHandler(this.m_deleteActorToolStripMenuItem_Click);
			// 
			// m_characterGroupGrid
			// 
			this.m_characterGroupGrid.AllowDrop = true;
			this.m_characterGroupGrid.AllowUserToAddRows = false;
			this.m_characterGroupGrid.AllowUserToDeleteRows = false;
			this.m_characterGroupGrid.AllowUserToOrderColumns = true;
			this.m_characterGroupGrid.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
			this.m_characterGroupGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.m_characterGroupGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_characterGroupGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.m_characterGroupGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_characterGroupGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(73)))), ((int)(((byte)(108)))));
			this.m_characterGroupGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_characterGroupGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_characterGroupGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.m_characterGroupGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GroupNumber,
            this.CharacterIds,
            this.Column3,
            this.CharStatus,
            this.EstimatedHours,
            this.Column5});
			this.m_characterGroupGrid.ContextMenuStrip = this.m_contextMenuCharacterGroups;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F);
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.NullValue = null;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_characterGroupGrid.DefaultCellStyle = dataGridViewCellStyle4;
			this.m_characterGroupGrid.DrawTextBoxEditControlBorder = false;
			this.m_characterGroupGrid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.m_characterGroupGrid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this.m_characterGroupGrid.GridColor = System.Drawing.Color.Black;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_characterGroupGrid, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_characterGroupGrid, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.m_characterGroupGrid, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this.m_characterGroupGrid, "DialogBoxes.VoiceActorAssignmentDlg.betterGrid1");
			this.m_characterGroupGrid.Location = new System.Drawing.Point(0, 20);
			this.m_characterGroupGrid.Margin = new System.Windows.Forms.Padding(0);
			this.m_characterGroupGrid.MultiSelect = false;
			this.m_characterGroupGrid.Name = "m_characterGroupGrid";
			this.m_characterGroupGrid.PaintHeaderAcrossFullGridWidth = true;
			this.m_characterGroupGrid.ParentLayersAffected = 0;
			this.m_characterGroupGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_characterGroupGrid.RowHeadersVisible = false;
			this.m_characterGroupGrid.RowHeadersWidth = 22;
			this.m_characterGroupGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.m_characterGroupGrid.SelectedCellBackColor = System.Drawing.Color.Empty;
			this.m_characterGroupGrid.SelectedCellForeColor = System.Drawing.Color.Empty;
			this.m_characterGroupGrid.SelectedRowBackColor = System.Drawing.Color.Empty;
			this.m_characterGroupGrid.SelectedRowForeColor = System.Drawing.Color.Empty;
			this.m_characterGroupGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_characterGroupGrid.ShowWaterMarkWhenDirty = false;
			this.m_characterGroupGrid.Size = new System.Drawing.Size(418, 352);
			this.m_characterGroupGrid.TabIndex = 6;
			this.m_characterGroupGrid.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this.m_characterGroupGrid.WaterMark = "!";
			this.m_characterGroupGrid.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_characterGroupGrid_CellLeave);
			this.m_characterGroupGrid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_characterGroupGrid_CellMouseDoubleClick);
			this.m_characterGroupGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_characterGroupGrid_CellMouseDown);
			this.m_characterGroupGrid.SelectionChanged += new System.EventHandler(this.m_characterGroupGrid_SelectionChanged);
			this.m_characterGroupGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.m_characterGroupGrid_DragDrop);
			this.m_characterGroupGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.m_characterGroupGrid_DragOver);
			this.m_characterGroupGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_characterGroupGrid_KeyDown);
			this.m_characterGroupGrid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_characterGroupGrid_MouseMove);
			// 
			// GroupNumber
			// 
			this.GroupNumber.DataPropertyName = "GroupNumber";
			this.GroupNumber.FillWeight = 18.8621F;
			this.GroupNumber.HeaderText = "_L10N_:DialogBoxes.VoiceActorAssignmentDlg.GroupNumber!Group #";
			this.GroupNumber.Name = "GroupNumber";
			// 
			// CharacterIds
			// 
			this.CharacterIds.ContextMenuStrip = this.m_contextMenuCharacters;
			this.CharacterIds.DataPropertyName = "CharacterIds";
			this.CharacterIds.FillWeight = 150.8968F;
			this.CharacterIds.HeaderText = "_L10N_:DialogBoxes.VoiceActorAssignmentDlg.Characters!Characters";
			this.CharacterIds.Name = "CharacterIds";
			this.CharacterIds.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			// 
			// Column3
			// 
			this.Column3.DataPropertyName = "AttributesDisplay";
			this.Column3.FillWeight = 75.44839F;
			this.Column3.HeaderText = "_L10N_:DialogBoxes.VoiceActorAssignmentDlg.Attributes!Attributes";
			this.Column3.Name = "Column3";
			// 
			// CharStatus
			// 
			this.CharStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.CharStatus.DataPropertyName = "StatusDisplay";
			this.CharStatus.FillWeight = 115.4822F;
			this.CharStatus.HeaderText = "_L10N_:DialogBoxes.VoiceActorAssignmentDlg.Status!Status";
			this.CharStatus.Name = "CharStatus";
			this.CharStatus.Visible = false;
			this.CharStatus.Width = 348;
			// 
			// EstimatedHours
			// 
			this.EstimatedHours.DataPropertyName = "EstimatedHours";
			dataGridViewCellStyle3.Format = "N2";
			dataGridViewCellStyle3.NullValue = null;
			this.EstimatedHours.DefaultCellStyle = dataGridViewCellStyle3;
			this.EstimatedHours.FillWeight = 37.7242F;
			this.EstimatedHours.HeaderText = "_L10N_:DialogBoxes.VoiceActorAssignmentDlg.Hours!Hours";
			this.EstimatedHours.Name = "EstimatedHours";
			// 
			// Column5
			// 
			this.Column5.DataPropertyName = "VoiceActorAssignedName";
			this.Column5.FillWeight = 56.5863F;
			this.Column5.HeaderText = "_L10N_:DialogBoxes.VoiceActorAssignmentDlg.ActorAssigned!Actor Assigned";
			this.Column5.Name = "Column5";
			// 
			// m_voiceActorGrid
			// 
			this.m_voiceActorGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_voiceActorGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(73)))), ((int)(((byte)(108)))));
			this.m_voiceActorGrid.CharacterGroupsWithAssignedActors = null;
			this.m_voiceActorGrid.ContextMenuStrip = this.m_contextMenuVoiceActors;
			this.m_voiceActorGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_voiceActorGrid, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_voiceActorGrid, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.m_voiceActorGrid, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this.m_voiceActorGrid, "DialogBoxes.VoiceActorAssignmentDlg.VoiceActorInformationGrid");
			this.m_voiceActorGrid.Location = new System.Drawing.Point(0, 20);
			this.m_voiceActorGrid.Margin = new System.Windows.Forms.Padding(0);
			this.m_voiceActorGrid.Name = "m_voiceActorGrid";
			this.m_voiceActorGrid.ReadOnly = false;
			this.m_voiceActorGrid.Size = new System.Drawing.Size(359, 352);
			this.m_voiceActorGrid.TabIndex = 1;
			this.m_voiceActorGrid.Leave += new System.EventHandler(this.m_voiceActorGrid_Leave);
			// 
			// m_saveStatus
			// 
			this.m_saveStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_saveStatus.AutoSize = true;
			this.m_saveStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_saveStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(73)))), ((int)(((byte)(108)))));
			this.m_saveStatus.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(73)))), ((int)(((byte)(108)))));
			this.m_saveStatus.ForeColor = System.Drawing.Color.White;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.m_saveStatus, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.m_saveStatus, null);
			this.l10NSharpExtender1.SetLocalizingId(this.m_saveStatus, "DialogBoxes.VoiceActorAssignmentDlg.SaveStatus");
			this.m_saveStatus.Location = new System.Drawing.Point(321, 3);
			this.m_saveStatus.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.m_saveStatus.Name = "m_saveStatus";
			this.m_saveStatus.Size = new System.Drawing.Size(97, 13);
			this.m_saveStatus.TabIndex = 6;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(73)))), ((int)(((byte)(108)))));
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.m_characterGroupGrid, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.m_lblCharacterGroups, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(418, 405);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.m_saveStatus);
			this.panel3.Controls.Add(this.m_btnUpdateGroup);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 372);
			this.panel3.Margin = new System.Windows.Forms.Padding(0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(418, 33);
			this.panel3.TabIndex = 7;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.m_voiceActorGrid, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.m_lblVoiceActors, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(359, 405);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.m_linkEdit);
			this.panel2.Controls.Add(this.m_linkClose);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 372);
			this.panel2.Margin = new System.Windows.Forms.Padding(0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(359, 33);
			this.panel2.TabIndex = 3;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(15, 25);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
			this.splitContainer1.Panel1.Margin = new System.Windows.Forms.Padding(3);
			this.splitContainer1.Panel1MinSize = 300;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
			this.splitContainer1.Panel2.Margin = new System.Windows.Forms.Padding(3);
			this.splitContainer1.Panel2MinSize = 200;
			this.splitContainer1.Size = new System.Drawing.Size(832, 405);
			this.splitContainer1.SplitterDistance = 418;
			this.splitContainer1.SplitterWidth = 55;
			this.splitContainer1.TabIndex = 0;
			this.splitContainer1.TabStop = false;
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
			this.splitContainer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_MouseUp);
			// 
			// VoiceActorAssignmentDlg
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(73)))), ((int)(((byte)(108)))));
			this.ClientSize = new System.Drawing.Size(859, 442);
			this.Controls.Add(this.m_helpIcon);
			this.Controls.Add(this.m_btnAssignActor);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this, null);
			this.l10NSharpExtender1.SetLocalizationComment(this, null);
			this.l10NSharpExtender1.SetLocalizingId(this, "DialogBoxes.VoiceActorAssignmentDlg.WindowTitle");
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(875, 480);
			this.Name = "VoiceActorAssignmentDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Voice Actor Assignment";
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.VoiceActorAssignmentDlg_KeyUp);
			this.m_contextMenuCharacters.ResumeLayout(false);
			this.m_contextMenuCharacterGroups.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_helpIcon)).EndInit();
			this.m_contextMenuVoiceActors.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_characterGroupGrid)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Controls.VoiceActorInformationGrid m_voiceActorGrid;
		private System.Windows.Forms.Button m_btnAssignActor;
		private System.Windows.Forms.Label label1;
		private L10NSharp.UI.L10NSharpExtender l10NSharpExtender1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button m_btnUpdateGroup;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label m_lblCharacterGroups;
		private System.Windows.Forms.Label m_lblVoiceActors;
		private System.Windows.Forms.LinkLabel m_linkClose;
		private Controls.SaveStatus m_saveStatus;
		private System.Windows.Forms.LinkLabel m_linkEdit;
		private System.Windows.Forms.PictureBox m_helpIcon;
		private System.Windows.Forms.ToolTip m_toolTip;
		private System.Windows.Forms.ContextMenuStrip m_contextMenuCharacterGroups;
		private System.Windows.Forms.ToolStripMenuItem m_assignActorToGroupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem m_unAssignActorFromGroupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem m_splitGroupToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip m_contextMenuVoiceActors;
		private System.Windows.Forms.ToolStripMenuItem m_assignActorToGroupToolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem m_editActorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem m_deleteActorToolStripMenuItem;
		private AutoGrid m_characterGroupGrid;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.ContextMenuStrip m_contextMenuCharacters;
		private System.Windows.Forms.ToolStripMenuItem m_tmiCreateNewGroup;
		private System.Windows.Forms.DataGridViewTextBoxColumn GroupNumber;
		private DataGridViewListBoxColumn CharacterIds;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
		private System.Windows.Forms.DataGridViewTextBoxColumn CharStatus;
		private System.Windows.Forms.DataGridViewTextBoxColumn EstimatedHours;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column5;

	}
}