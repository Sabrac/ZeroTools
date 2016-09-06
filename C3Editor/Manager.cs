// Decompiled with JetBrains decompiler
// Type: Conquer.Manager
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Conquer.C3;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Conquer
{
  public class Manager : Form
  {
    private IContainer components = (IContainer) null;
    public GameModel SelectedModel;
    public Physics SelectedObject;
    private TreeView ModelTreeView;
    private Button HideButton;
    private Button ToggleNodeButton;
    private Button DeleteButton;
    private Button ModifyButton;

    public Manager()
    {
      this.InitializeComponent();
    }

    public void AddPhysicalObject(string name, Physics phy)
    {
      this.ModelTreeView.Invoke((MethodInvoker)delegate
      {
        if (!this.ModelTreeView.Nodes.ContainsKey(name))
          this.ModelTreeView.Nodes.Add(name, name);
        this.ModelTreeView.Nodes[this.ModelTreeView.Nodes.IndexOfKey(name)].Nodes.Add(name, phy.Name);
        this.ModelTreeView.ExpandAll();
      });
      this.SelectedObject = phy;
    }

    public void AddShape(string name, Shape shape)
    {
      this.ModelTreeView.Invoke((MethodInvoker)delegate
      {
        if (!this.ModelTreeView.Nodes.ContainsKey(name))
          this.ModelTreeView.Nodes.Add(name, name);
        this.ModelTreeView.Nodes[this.ModelTreeView.Nodes.IndexOfKey(name)].Nodes.Add(name, shape.Name);
        this.ModelTreeView.ExpandAll();
      });
    }

    private void HideButton_Click(object sender, EventArgs e)
    {
      this.Hide();
    }

    private void ModelTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
      this.SelectedModel = Settings.Scene.Models.Where<GameModel>((Func<GameModel, bool>) (I => I.Name == this.ModelTreeView.SelectedNode.Name)).FirstOrDefault<GameModel>();
      this.SelectedObject = this.SelectedModel.PhysicalObjects.Where<Physics>((Func<Physics, bool>) (I => I.Name == this.ModelTreeView.SelectedNode.Text)).FirstOrDefault<Physics>();
      if (this.SelectedObject == null)
        return;
      this.ToggleNodeButton.Text = this.SelectedObject.IsShown ? "Hide" : "Show";
    }

    private void ModelTreeView_DoubleClick(object sender, EventArgs e)
    {
      int num = (int) new ObjectManager(this).ShowDialog();
    }

    private void ToggleNodeButton_Click(object sender, EventArgs e)
    {
      this.SelectedModel = Settings.Scene.Models.Where<GameModel>((Func<GameModel, bool>) (I => I.Name == this.ModelTreeView.SelectedNode.Name)).FirstOrDefault<GameModel>();
      this.SelectedObject = this.SelectedModel.PhysicalObjects.Where<Physics>((Func<Physics, bool>) (I => I.Name == this.ModelTreeView.SelectedNode.Text)).FirstOrDefault<Physics>();
      if (this.SelectedObject != null)
      {
        this.SelectedObject.IsShown = !this.SelectedObject.IsShown;
      }
      else
      {
        if (this.SelectedModel == null)
          return;
        foreach (Physics physicalObject in this.SelectedModel.PhysicalObjects)
          physicalObject.IsShown = !physicalObject.IsShown;
      }
    }

    private void DeleteButton_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.SelectedObject != null)
        {
          this.SelectedModel.PhysicalObjects.Remove(this.SelectedObject);
          this.ModelTreeView.SelectedNode.Remove();
        }
        else
        {
          if (this.SelectedModel == null)
            return;
          Settings.Scene.Models.Remove(this.SelectedModel);
          this.ModelTreeView.SelectedNode.Remove();
        }
      }
      catch
      {
      }
    }

    private void ModifyButton_Click(object sender, EventArgs e)
    {
      int num = (int) new ObjectManager(this).ShowDialog();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.ModelTreeView = new TreeView();
      this.HideButton = new Button();
      this.ToggleNodeButton = new Button();
      this.DeleteButton = new Button();
      this.ModifyButton = new Button();
      this.SuspendLayout();
      this.ModelTreeView.Location = new Point(13, 14);
      this.ModelTreeView.Margin = new Padding(4, 5, 4, 5);
      this.ModelTreeView.Name = "ModelTreeView";
      this.ModelTreeView.Size = new Size(323, 346);
      this.ModelTreeView.TabIndex = 0;
      this.ModelTreeView.AfterSelect += new TreeViewEventHandler(this.ModelTreeView_AfterSelect);
      this.ModelTreeView.DoubleClick += new EventHandler(this.ModelTreeView_DoubleClick);
      this.HideButton.Location = new Point(262, 365);
      this.HideButton.Margin = new Padding(4, 5, 4, 5);
      this.HideButton.Name = "HideButton";
      this.HideButton.Size = new Size(74, 32);
      this.HideButton.TabIndex = 1;
      this.HideButton.Text = "Close";
      this.HideButton.UseVisualStyleBackColor = true;
      this.HideButton.Click += new EventHandler(this.HideButton_Click);
      this.ToggleNodeButton.Location = new Point(179, 365);
      this.ToggleNodeButton.Margin = new Padding(4, 5, 4, 5);
      this.ToggleNodeButton.Name = "ToggleNodeButton";
      this.ToggleNodeButton.Size = new Size(74, 32);
      this.ToggleNodeButton.TabIndex = 2;
      this.ToggleNodeButton.Text = "Hide";
      this.ToggleNodeButton.UseVisualStyleBackColor = true;
      this.ToggleNodeButton.Click += new EventHandler(this.ToggleNodeButton_Click);
      this.DeleteButton.Location = new Point(96, 365);
      this.DeleteButton.Margin = new Padding(4, 5, 4, 5);
      this.DeleteButton.Name = "DeleteButton";
      this.DeleteButton.Size = new Size(74, 32);
      this.DeleteButton.TabIndex = 3;
      this.DeleteButton.Text = "Delete";
      this.DeleteButton.UseVisualStyleBackColor = true;
      this.DeleteButton.Click += new EventHandler(this.DeleteButton_Click);
      this.ModifyButton.Location = new Point(13, 365);
      this.ModifyButton.Margin = new Padding(4, 5, 4, 5);
      this.ModifyButton.Name = "ModifyButton";
      this.ModifyButton.Size = new Size(74, 32);
      this.ModifyButton.TabIndex = 4;
      this.ModifyButton.Text = "Modify";
      this.ModifyButton.UseVisualStyleBackColor = true;
      this.ModifyButton.Click += new EventHandler(this.ModifyButton_Click);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoValidate = AutoValidate.EnablePreventFocusChange;
      this.ClientSize = new Size(349, 405);
      this.ControlBox = false;
      this.Controls.Add((Control) this.ModifyButton);
      this.Controls.Add((Control) this.DeleteButton);
      this.Controls.Add((Control) this.ToggleNodeButton);
      this.Controls.Add((Control) this.HideButton);
      this.Controls.Add((Control) this.ModelTreeView);
      this.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Margin = new Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.Name = "Manager";
      this.Text = "Manager";
      this.ResumeLayout(false);
    }
  }
}
