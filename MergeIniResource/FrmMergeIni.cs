using MergeIniResource.EnumType;
using MergeIniResource.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MergeIniResource
{
    public partial class FrmMergeIni : Form
    {
        public FrmMergeIni()
        {
            InitializeComponent();
            txtIni.AllowDrop = true;
        }

        private void txtFolder_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void txtFolder_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            string s = "";

            foreach (string File in FileList)
                s = s + " " + File;
            txtIni.Text = s;
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Directory.Exists(txtIni.Text))
            {
                string[] files = Directory.GetFiles(txtIni.Text);
                List<IniObject> lsIni = new List<IniObject>();

                foreach (String file in files)
                {
                    SetStatus("Reading file: " + file);
                    if (!file.EndsWith("ini"))
                    {
                        continue;
                    }

                    IniObject ini = new IniObject(file);
                    ini = ReadFile(ini);
                    if (ini.count > 0)
                    {
                        lsIni.Add(ini);
                    }
                    else
                    {
                        WriteLog("[Warning] ", Color.Orange);
                        WriteLineLog(String.Format("[File: {0}] - Does not have valid data.", file));
                    }
                }

                if (lsIni.Count > 0)
                {
                    ExportFile(lsIni);
                }
                else
                {
                    WriteLog("[Warning] ", Color.Orange);
                    WriteLineLog("Does not have ini file to export");
                }
            }
            else
            {
                WriteLog("[Error] ", Color.Red);
                WriteLineLog("Ini folder is not exists.");
                SetStatus("Complete!");
            }
        }

        private void ExportFile(List<IniObject> lsIni)
        {
            StringBuilder sb = new StringBuilder();

            foreach (IniObject ini in lsIni)
            {
                foreach (GraphicObject graphic in ini.lsGraphic.Values)
                {
                    sb.Append(graphic.id).Append("=").Append(graphic.value).AppendLine();
                }
            }

            if (sb.Length > 0)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Ini Files |*.ini|All Files |*.*";
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        using (StreamWriter outfile = new StreamWriter(sfd.FileName))
                        {
                            outfile.Write(sb.ToString());
                        }
                        SetStatus("Complete!");
                    }
                });
            }
            else
            {
                WriteLog("[Warning] ", Color.Orange);
                WriteLineLog("Result string is null or empty.");
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync();
            }
            else
            {
                WriteLog("[Error] ", Color.Red);
                WriteLineLog("Thread is running.");
            }
        }

        private void SetStatus(string content)
        {
            lblStatus.Invoke(new Action(() =>
                {
                    if (content.Length >= 120)
                    {
                        lblStatus.Text = content.Substring(0, 120) + "...";
                    }
                    else
                    {
                        lblStatus.Text = content;
                    }
                }
            ));
        }

        private void WriteLog(string content, Color? textColor = null)
        {
            txtLog.Invoke(new Action(() =>
                {
                    if (textColor == null)
                    {
                        txtLog.AppendText(content);
                    }
                    else
                    {
                        txtLog.SelectionStart = txtLog.TextLength;
                        txtLog.SelectionLength = 0;

                        txtLog.SelectionColor = (Color) textColor;
                        txtLog.AppendText(content);
                        txtLog.SelectionColor = txtLog.ForeColor;
                    }
                    txtLog.SelectionStart = txtLog.Text.Length;
                    txtLog.ScrollToCaret();
                }
            ));
        }

        private void WriteLineLog(string content, Color? textColor = null)
        {
            WriteLog(content, textColor);
            WriteLog(Environment.NewLine);
        }

        private IniObject ReadFile(IniObject iniFile)
        {
            string regexString = @"[0-9]+=[a-zA-Z0-9\/\\]+.(dds|c3)";
            using (FileStream fs = File.Open(iniFile.name, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (Regex.IsMatch(line, regexString, RegexOptions.IgnoreCase))
                    {
                        string[] part = line.Split('=');
                        if (part.Length != 2)
                        {
                            WriteLog("[Error] ", Color.Red);
                            WriteLineLog(String.Format("[File: {0}][Data: {1}] - Line does not contain 2 parts"));
                        }
                        else
                        {
                            GraphicObject graphic = new GraphicObject(part[0].ToString(), part[1].ToString());
                            if (line.EndsWith("dds") || line.EndsWith("DDS"))
                            {
                                graphic.type = FileType.DDS;
                            }
                            else if (line.EndsWith("c3") || line.EndsWith("C3"))
                            {
                                graphic.type = FileType.C3;
                            }
                            else
                            {
                                graphic.type = FileType.OTHER;
                            }

                            if (iniFile.lsGraphic.ContainsKey(graphic.id))
                            {
                                string tmpId = graphic.id;
                                int number = 1;
                                while (iniFile.lsGraphic.ContainsKey(tmpId))
                                {
                                    tmpId = graphic.id + "_" + (number++);
                                }
                                iniFile.lsGraphic.Add(tmpId, graphic);
                            }
                            else
                            {
                                iniFile.lsGraphic.Add(graphic.id, graphic);
                            }
                        }
                    }
                    else
                    {
                        //WriteLog("[Warning] ", Color.Orange);
                        //WriteLineLog(String.Format("[File: {0}][Data: {1}] - Line format not valid.", iniFile.name, line));
                    }
                }
                return iniFile;
            }
        }
    }
}
