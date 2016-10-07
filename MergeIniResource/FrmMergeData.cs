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
    public partial class FrmMergeData : Form
    {
        BackgroundWorker bw;

        public FrmMergeData()
        {
            InitializeComponent();
        }

        private void txt_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            string s = "";

            foreach (string File in FileList)
            {
                // s = s + " " + File;
                s = File;
            }

            if (!s.Substring(s.LastIndexOf(".")).Trim().Equals("ini")) {
                MessageBox.Show("File's format is invalid");
                return;
            }
            ((TextBox)sender).Text = s;
        }

        private void txt_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            this.bw = new BackgroundWorker();
            this.bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
            this.bw.DoWork += new DoWorkEventHandler(this.Worker_DoWork);
            this.bw.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            string source = txtSourceFile.Text;
            string target = txtTargetFile.Text;
            if (!source.Substring(source.LastIndexOf(".")).Trim().Equals("ini") ||
                !target.Substring(target.LastIndexOf(".")).Trim().Equals("ini"))
            {
                MessageBox.Show("File's format is invalid");
                return;
            }

            IniObject sourceIni = new IniObject(source);
            IniObject targetIni = new IniObject(target);
            sourceIni = ReadFile(sourceIni);
            targetIni = ReadFile(targetIni);

            foreach (KeyValuePair<String, GraphicObject> data in sourceIni.lsGraphic)
            {
                if (targetIni.lsGraphic.ContainsKey(data.Key))
                {
                    sourceIni.lsGraphic[data.Key] = targetIni.lsGraphic[data.Key];
                    WriteLineLog("Found - " + data.Key);
                }
            }

            List<IniObject> lsIni = new List<IniObject>();
            lsIni.Add(sourceIni);
            ExportFile(lsIni);

            MessageBox.Show("Done !");
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
                    }
                });
            }
            else
            {
                WriteLog("[Warning] ", Color.Orange);
                WriteLineLog("Result string is null or empty.");
            }
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

        private string ReadFile(string file)
        {
            StringBuilder sb = new StringBuilder();

            using (FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }

            return sb.ToString();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void WriteLineLog(string content, Color? textColor = null)
        {
            WriteLog(content, textColor);
            WriteLog(Environment.NewLine);
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

                    txtLog.SelectionColor = (Color)textColor;
                    txtLog.AppendText(content);
                    txtLog.SelectionColor = txtLog.ForeColor;
                }
                txtLog.SelectionStart = txtLog.Text.Length;
                txtLog.ScrollToCaret();
            }
            ));
        }
    }
}
