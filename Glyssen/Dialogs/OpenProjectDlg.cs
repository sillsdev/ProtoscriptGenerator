﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Glyssen.Properties;
using Glyssen.Shared;
using Glyssen.Utilities;
using GlyssenEngine.Paratext;
using GlyssenFileBasedPersistence;
using L10NSharp;
using Paratext.Data;
using PtxUtils;
using SIL.Reporting;
using static System.String;

namespace Glyssen.Dialogs
{
	public partial class OpenProjectDlg : FormWithPersistedSettings
	{
		private bool m_gridSettingsChanged;

		public enum ProjectType
		{
			ExistingProject,
			TextReleaseBundle,
			ParatextProject,
		}

		public OpenProjectDlg(string currentProjectFilePath)
		{
			InitializeComponent();

			m_listExistingProjects.GetParatextProjects = GetParatextProjects;

			if (currentProjectFilePath != null)
			{
				m_listExistingProjects.SelectedProject = currentProjectFilePath;
				SelectedProject = currentProjectFilePath;
				m_listExistingProjects.AddReadOnlyProject(currentProjectFilePath);
			}
			else
				m_btnOk.Enabled = false;
		}

		[DefaultValue(ProjectType.ExistingProject)]
		public ProjectType Type { get; private set; }

		public string SelectedProject { get; private set; }
		/// <summary>
		/// For Paratext projects, use this instead of the SelectedProject (name) to load the project.
		/// </summary>
		public string ParatextProjectId => m_listExistingProjects.GetIdentifierForParatextProject;

		private void OpenProjectDlg_Load(object sender, EventArgs e)
		{
			TileFormLocation();
		}

		private void m_linkTextReleaseBundle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			using (var dlg = new SelectProjectDlg())
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					SelectedProject = dlg.FileName;
					Type = Path.GetExtension(SelectedProject) == ProjectRepository.kProjectFileExtension ?
						ProjectType.ExistingProject : ProjectType.TextReleaseBundle;
					DialogResult = DialogResult.OK;
					Close();
				}
			}
		}

		// Note: This method is very similar to the method by the same name in HearThis' ChooseProject dialog. If improvements
		// are made here, they should also be made there if applicable.
		// TODO: Move this into libpalaso
		private IEnumerable<ScrText> GetParatextProjects()
		{
			List<ScrText> paratextProjects = null;
			try
			{
				paratextProjects = ScrTextCollection.ScrTexts(IncludeProjects.AccessibleScripture).ToList();
				var loadErrors = Program.CompatibleParatextProjectLoadErrors.ToList();
				VerifyProjectPropertiesAreAccessible(paratextProjects, loadErrors);
				if (loadErrors.Any())
				{
					StringBuilder sb = new StringBuilder(Format(LocalizationManager.GetString("DialogBoxes.OpenProjectDlg.ParatextProjectLoadErrors",
						"The following {0} project load errors occurred:", "Param 0: \"Paratext\" (product name)"), ParatextScrTextWrapper.kParatextProgramName));
					foreach (var errMsgInfo in loadErrors)
					{
						sb.Append("\n\n");
						try
						{
							switch (errMsgInfo.Reason)
							{
								case UnsupportedReason.UnknownType:
									AppendVersionIncompatibilityMessage(sb, errMsgInfo);
									sb.AppendFormat(LocalizationManager.GetString("DialogBoxes.OpenProjectDlg.ParatextProjectLoadError.UnknownProjectType",
											"This project has a project type ({0}) that is not supported.", "Param 0: Paratext project type"),
										errMsgInfo.ProjecType);
									break;

								case UnsupportedReason.CannotUpgrade:
									// Glyssen is newer than project version
									AppendVersionIncompatibilityMessage(sb, errMsgInfo);
									sb.AppendFormat(LocalizationManager.GetString("DialogBoxes.OpenProjectDlg.ParatextProjectLoadError.ProjectOutdated",
											"The project administrator needs to update it by opening it with {0} {1} or later. " +
											"Alternatively, you might be able to revert to an older version of {2}.",
											"Param 0: \"Paratext\" (product name); " +
											"Param 1: Paratext version number; " +
											"Param 2: \"Glyssen\" (product name)"),
										ParatextScrTextWrapper.kParatextProgramName,
										ParatextInfo.MinSupportedParatextDataVersion,
										GlyssenInfo.Product);
									break;

								case UnsupportedReason.FutureVersion:
									// Project version is newer than Glyssen
									AppendVersionIncompatibilityMessage(sb, errMsgInfo);
									sb.AppendFormat(LocalizationManager.GetString("DialogBoxes.OpenProjectDlg.ParatextProjectLoadError.GlyssenVersionOutdated",
											"To read this project, a version of {0} compatible with {1} {2} is required.",
											"Param 0: \"Glyssen\" (product name); " +
											"Param 1: \"Paratext\" (product name); " +
											"Param 2: Paratext version number"),
										GlyssenInfo.Product,
										ParatextScrTextWrapper.kParatextProgramName,
										ScrTextCollection.ScrTexts(IncludeProjects.Everything).First(
											p => p.Name == errMsgInfo.ProjectName).Settings.MinParatextDataVersion);
									break;

								case UnsupportedReason.Corrupted:
								case UnsupportedReason.Unspecified:
									sb.AppendFormat(LocalizationManager.GetString("DialogBoxes.OpenProjectDlg.ParatextProjectLoadError.Generic",
											"Project: {0}\nError message: {1}", "Param 0: Paratext project name; Param 1: error details"),
										errMsgInfo.ProjectName, errMsgInfo.Exception.Message);
									break;

								default:
									throw errMsgInfo.Exception;
							}
						}
						catch (Exception e)
						{
							ErrorReport.ReportNonFatalException(e);
						}
					}
					MessageBox.Show(sb.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
			catch (Exception err)
			{
				NotifyUserOfParatextProblem(Format(LocalizationManager.GetString("DialogBoxes.OpenProjectDlg.CantAccessParatext",
						"There was a problem accessing {0} data files.",
						"Param: \"Paratext\" (product name)"),
						ParatextScrTextWrapper.kParatextProgramName),
					Format(LocalizationManager.GetString("DialogBoxes.OpenProjectDlg.ParatextError", "The error was: {0}"), err.Message));
				paratextProjects = new List<ScrText>(0);
			}
			// ENHANCE (PG-63): Implement something like this if we decide to give the user the option of manually
			// specifying the location of Paratext data files if the program isn’t actually installed.
			// to select the location of Paratext data files.
			//if (paratextProjects.Any())
			//{
			//	_lblNoParatextProjectsInFolder.Visible = false;
			//	_lblNoParatextProjects.Visible = false;
			//}
			//else
			//{
			//	if (ParatextInfo.IsParatextInstalled)
			//		_lblNoParatextProjects.Visible = true;
			//	else
			//	{
			//		_lblNoParatextProjectsInFolder.Visible = _tableLayoutPanelParatextProjectsFolder.Visible;
			//	}
			//}
			return paratextProjects;
		}

		/// <summary>
		/// Verify that the ScrText's basic properties that we care about are accessible.
		/// </summary>
		/// <param name="paratextProjects">List of ScrTexts. Any whose basic properties result in
		/// errors will be purged from the list.</param>
		/// <param name="loadErrors">List of load error messages (to which this method will append
		/// if needed)</param>
		private static void VerifyProjectPropertiesAreAccessible(List<ScrText> paratextProjects, List<ErrorMessageInfo> loadErrors)
		{
			for (var index = 0; index < paratextProjects.Count; index++)
			{
				var scrText = paratextProjects[index];
				string name = "???";
				Enum<Paratext.Data.ProjectType> projectType = Paratext.Data.ProjectType.Standard;
				try
				{
					// Note: the properties we access here should reflect (more or less) the
					// properties we access in ParatextProjectProxy's constructor.
					name = scrText.Name;
					projectType = scrText.Settings.TranslationInfo.Type;

					if (scrText.DisplayLanguageName == null)
					{
						throw new NullReferenceException(Format(NullProjectPropertyErrorMessage,
							nameof(scrText.DisplayLanguageName), name));
					}
					if (scrText.Language == null)
					{
						throw new NullReferenceException(Format(NullProjectPropertyErrorMessage,
							nameof(scrText.Language), name));
					}
				}
				catch (Exception e)
				{
					loadErrors.Add(new ErrorMessageInfo(e, name, projectType));
					paratextProjects.RemoveAt(index--);
				}
			}
		}

		private static string NullProjectPropertyErrorMessage => LocalizationManager.GetString(
			"DialogBoxes.OpenProjectDlg.NullParatextProjectProperty",
			"Null {0} property for Paratext project {1}",
			"Param 0: property name (from Paratext code);" +
			"Param 1: Unique Paratext project identifier (if accessible)");

		private static void AppendVersionIncompatibilityMessage(StringBuilder sb, ErrorMessageInfo errMsgInfo)
		{
			sb.AppendFormat(LocalizationManager.GetString("DialogBoxes.OpenProjectDlg.ParatextProjectLoadError.VersionIncompatibility",
					"Project {0} is not compatible with this version of {1}.", "Param 0: Paratext project name; Param 1: \"Glyssen\""),
				errMsgInfo.ProjectName, GlyssenInfo.Product).Append(' ');
		}

		private void NotifyUserOfParatextProblem(string message, params string[] additionalInfo)
		{
			additionalInfo.Aggregate(message, (current, s) => current + Environment.NewLine + s);

			var result = ErrorReport.NotifyUserOfProblem(new ShowAlwaysPolicy(),
				LocalizationManager.GetString("Common.Quit", "Quit"), ErrorResult.Abort, message);

			if (result == ErrorResult.Abort)
				Application.Exit();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (Settings.Default.OpenProjectDlgGridSettings != null &&
				//Don't use settings based on old number of columns
				Settings.Default.OpenProjectDlgGridSettings.Columns.Length == m_listExistingProjects.GridSettings.Columns.Length)
			{
				m_listExistingProjects.GridSettings = Settings.Default.OpenProjectDlgGridSettings;
			}

			if (m_listExistingProjects.SelectedProject != null)
			{
				m_listExistingProjects.ScrollToSelected();
				m_btnOk.Enabled = true;
			}

			m_gridSettingsChanged = false;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (m_gridSettingsChanged)
				Settings.Default.OpenProjectDlgGridSettings = m_listExistingProjects.GridSettings;
			base.OnClosing(e);
		}

		private void HandleSelectedProjectChanged(object sender, EventArgs e)
		{
			SelectedProject = m_listExistingProjects.SelectedProject;
			Type = Path.GetExtension(SelectedProject) == ProjectRepository.kProjectFileExtension ?
				ProjectType.ExistingProject : ProjectType.ParatextProject;
			m_btnOk.Enabled = SelectedProject != null;
		}

		private void HandleExistingProjectsDoubleClick(object sender, EventArgs e)
		{
			if (m_btnOk.Enabled)
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		private void HandleShowHiddenProjectsCheckedChanged(object sender, EventArgs e)
		{
			m_listExistingProjects.IncludeHiddenProjects = m_chkShowInactiveProjects.Checked;
		}

		private void HandleExistingProjectsListLoaded(object sender, EventArgs e)
		{
			m_chkShowInactiveProjects.Visible = m_listExistingProjects.HiddenProjectsExist;
		}

		private void HandleProjectListSorted(object sender, EventArgs e)
		{
			m_gridSettingsChanged = true;
		}

		private void HandleColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			if (e.Column.AutoSizeMode == DataGridViewAutoSizeColumnMode.Fill)
				return;

			m_gridSettingsChanged = true;
		}

		private void m_listExistingProjects_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
		{
			m_gridSettingsChanged = true;
		}
	}
}
