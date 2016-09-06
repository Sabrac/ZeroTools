// Decompiled with JetBrains decompiler
// Type: Conquer.ObjectManager
// Assembly: Conquer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50F7D947-6602-4FD8-9121-B7A03984980A
// Assembly location: C:\Users\hongha1412\Downloads\C3Tool_V1.2\Conquer.exe

using Conquer.C3;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Conquer
{
  public class ObjectManager : Form
  {
    private IContainer components = (IContainer) null;
    private Manager Parent;
    private ComboBox BlendModeComboBox;
    private Label label1;
    private TextBox ScaleXTextBox;
    private Label ScaleLabel;
    private TextBox ScaleYTextBox;
    private TextBox ScaleZTextBox;
    private TextBox PositionZTextBox;
    private TextBox PositionYTextBox;
    private Label PositionLabel;
    private TextBox PositionXTextBox;
    private Button ApplyButton;
    private Button ExportButton;
    private TextBox RotateZTextBox;
    private TextBox RotateYTextBox;
    private Label label2;
    private TextBox RotateXTextBox;

    public ObjectManager(Manager parent)
    {
      this.InitializeComponent();
      this.Parent = parent;
      if (parent.SelectedObject != null)
      {
        this.BlendModeComboBox.SelectedIndex = parent.SelectedObject.BlendMode.Name == "BlendState.NonPremultiplied" ? 0 : 1;
      }
      else
      {
        if (parent.SelectedModel == null || parent.SelectedModel.PhysicalObjects.Count <= 0)
          return;
        this.BlendModeComboBox.SelectedIndex = parent.SelectedModel.PhysicalObjects.FirstOrDefault<Physics>().BlendMode.Name == "BlendState.NonPremultiplied" ? 0 : 1;
      }
    }

    private void BlendModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.Parent.SelectedObject != null)
      {
        this.Parent.SelectedObject.BlendMode = this.BlendModeComboBox.SelectedIndex == 0 ? BlendState.NonPremultiplied : BlendState.Additive;
      }
      else
      {
        if (this.Parent.SelectedModel == null)
          return;
        foreach (Physics physicalObject in this.Parent.SelectedModel.PhysicalObjects)
          physicalObject.BlendMode = this.BlendModeComboBox.SelectedIndex == 0 ? BlendState.NonPremultiplied : BlendState.Additive;
      }
    }

    private void ApplyButton_Click(object sender, EventArgs e)
    {
      float result1;
      float.TryParse(this.ScaleXTextBox.Text, out result1);
      float result2;
      float.TryParse(this.ScaleYTextBox.Text, out result2);
      float result3;
      float.TryParse(this.ScaleZTextBox.Text, out result3);
      float result4;
      float.TryParse(this.PositionXTextBox.Text, out result4);
      float result5;
      float.TryParse(this.PositionYTextBox.Text, out result5);
      float result6;
      float.TryParse(this.PositionZTextBox.Text, out result6);
      float result7;
      float.TryParse(this.RotateXTextBox.Text, out result7);
      float result8;
      float.TryParse(this.RotateYTextBox.Text, out result8);
      float result9;
      float.TryParse(this.RotateZTextBox.Text, out result9);
      Vector3 scale = new Vector3(result1, result2, result3);
      Vector3 shift = new Vector3(result4, result5, result6);
      Matrix rotate = Matrix.CreateRotationX(MathHelper.ToRadians(result8)) * Matrix.CreateRotationY(MathHelper.ToRadians(result7)) * Matrix.CreateRotationZ(MathHelper.ToRadians(result9));
      if (this.Parent.SelectedModel != null)
      {
        foreach (Physics physicalObject in this.Parent.SelectedModel.PhysicalObjects)
          this.ApplyModification(physicalObject, scale, shift, rotate);
      }
      else
      {
        if (this.Parent.SelectedObject == null)
          return;
        this.ApplyModification(this.Parent.SelectedObject, scale, shift, rotate);
      }
    }

    private void ApplyModification(Physics phyObj, Vector3 scale, Vector3 shift, Matrix rotate)
    {
      foreach (Vertex normalVertex in phyObj.NormalVertexes)
        normalVertex.Position = normalVertex.Position * scale + shift;
      foreach (Vertex alphaVertex in phyObj.AlphaVertexes)
        alphaVertex.Position = alphaVertex.Position * scale + shift;
      if (phyObj.Motion == null)
        return;
      foreach (KeyFrame keyFrame in phyObj.Motion.KeyFrames)
      {
        for (int index = 0; index < keyFrame.Transforms.Count; ++index)
          keyFrame.Transforms[index] *= rotate;
      }
    }

    private void ExportButton_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      if (this.Parent.SelectedModel == null || saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.Parent.SelectedModel.WriteModel(new BinaryWriter((Stream) File.Create(saveFileDialog.FileName)));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.BlendModeComboBox = new ComboBox();
      this.label1 = new Label();
      this.ScaleXTextBox = new TextBox();
      this.ScaleLabel = new Label();
      this.ScaleYTextBox = new TextBox();
      this.ScaleZTextBox = new TextBox();
      this.PositionZTextBox = new TextBox();
      this.PositionYTextBox = new TextBox();
      this.PositionLabel = new Label();
      this.PositionXTextBox = new TextBox();
      this.ApplyButton = new Button();
      this.ExportButton = new Button();
      this.RotateZTextBox = new TextBox();
      this.RotateYTextBox = new TextBox();
      this.label2 = new Label();
      this.RotateXTextBox = new TextBox();
      this.SuspendLayout();
      this.BlendModeComboBox.FormattingEnabled = true;
      this.BlendModeComboBox.Items.AddRange(new object[2]
      {
        (object) "NonPremultiplied",
        (object) "Additive"
      });
      this.BlendModeComboBox.Location = new System.Drawing.Point(121, 7);
      this.BlendModeComboBox.Name = "BlendModeComboBox";
      this.BlendModeComboBox.Size = new Size(177, 28);
      this.BlendModeComboBox.TabIndex = 0;
      this.BlendModeComboBox.SelectedIndexChanged += new EventHandler(this.BlendModeComboBox_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(94, 20);
      this.label1.TabIndex = 1;
      this.label1.Text = "Blend Mode";
      this.ScaleXTextBox.Location = new System.Drawing.Point(121, 44);
      this.ScaleXTextBox.Name = "ScaleXTextBox";
      this.ScaleXTextBox.Size = new Size(55, 26);
      this.ScaleXTextBox.TabIndex = 6;
      this.ScaleXTextBox.Text = "1.0";
      this.ScaleXTextBox.TextAlign = HorizontalAlignment.Right;
      this.ScaleLabel.AutoSize = true;
      this.ScaleLabel.Location = new System.Drawing.Point(16, 47);
      this.ScaleLabel.Name = "ScaleLabel";
      this.ScaleLabel.Size = new Size(53, 20);
      this.ScaleLabel.TabIndex = 7;
      this.ScaleLabel.Text = "Scale:";
      this.ScaleYTextBox.Location = new System.Drawing.Point(182, 44);
      this.ScaleYTextBox.Name = "ScaleYTextBox";
      this.ScaleYTextBox.Size = new Size(55, 26);
      this.ScaleYTextBox.TabIndex = 9;
      this.ScaleYTextBox.Text = "1.0";
      this.ScaleYTextBox.TextAlign = HorizontalAlignment.Right;
      this.ScaleZTextBox.Location = new System.Drawing.Point(243, 44);
      this.ScaleZTextBox.Name = "ScaleZTextBox";
      this.ScaleZTextBox.Size = new Size(55, 26);
      this.ScaleZTextBox.TabIndex = 10;
      this.ScaleZTextBox.Text = "1.0";
      this.ScaleZTextBox.TextAlign = HorizontalAlignment.Right;
      this.PositionZTextBox.Location = new System.Drawing.Point(243, 76);
      this.PositionZTextBox.Name = "PositionZTextBox";
      this.PositionZTextBox.Size = new Size(55, 26);
      this.PositionZTextBox.TabIndex = 14;
      this.PositionZTextBox.Text = "0";
      this.PositionZTextBox.TextAlign = HorizontalAlignment.Right;
      this.PositionYTextBox.Location = new System.Drawing.Point(182, 76);
      this.PositionYTextBox.Name = "PositionYTextBox";
      this.PositionYTextBox.Size = new Size(55, 26);
      this.PositionYTextBox.TabIndex = 13;
      this.PositionYTextBox.Text = "0";
      this.PositionYTextBox.TextAlign = HorizontalAlignment.Right;
      this.PositionLabel.AutoSize = true;
      this.PositionLabel.Location = new System.Drawing.Point(16, 79);
      this.PositionLabel.Name = "PositionLabel";
      this.PositionLabel.Size = new Size(69, 20);
      this.PositionLabel.TabIndex = 12;
      this.PositionLabel.Text = "Position:";
      this.PositionXTextBox.Location = new System.Drawing.Point(121, 76);
      this.PositionXTextBox.Name = "PositionXTextBox";
      this.PositionXTextBox.Size = new Size(55, 26);
      this.PositionXTextBox.TabIndex = 11;
      this.PositionXTextBox.Text = "0";
      this.PositionXTextBox.TextAlign = HorizontalAlignment.Right;
      this.ApplyButton.Location = new System.Drawing.Point(168, 140);
      this.ApplyButton.Name = "ApplyButton";
      this.ApplyButton.Size = new Size(130, 40);
      this.ApplyButton.TabIndex = 15;
      this.ApplyButton.Text = "Apply";
      this.ApplyButton.UseVisualStyleBackColor = true;
      this.ApplyButton.Click += new EventHandler(this.ApplyButton_Click);
      this.ExportButton.Location = new System.Drawing.Point(20, 140);
      this.ExportButton.Name = "ExportButton";
      this.ExportButton.Size = new Size(130, 40);
      this.ExportButton.TabIndex = 16;
      this.ExportButton.Text = "Export";
      this.ExportButton.UseVisualStyleBackColor = true;
      this.ExportButton.Click += new EventHandler(this.ExportButton_Click);
      this.RotateZTextBox.Location = new System.Drawing.Point(243, 108);
      this.RotateZTextBox.Name = "RotateZTextBox";
      this.RotateZTextBox.Size = new Size(55, 26);
      this.RotateZTextBox.TabIndex = 20;
      this.RotateZTextBox.Text = "0";
      this.RotateZTextBox.TextAlign = HorizontalAlignment.Right;
      this.RotateYTextBox.Location = new System.Drawing.Point(182, 108);
      this.RotateYTextBox.Name = "RotateYTextBox";
      this.RotateYTextBox.Size = new Size(55, 26);
      this.RotateYTextBox.TabIndex = 19;
      this.RotateYTextBox.Text = "0";
      this.RotateYTextBox.TextAlign = HorizontalAlignment.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(16, 111);
      this.label2.Name = "label2";
      this.label2.Size = new Size(74, 20);
      this.label2.TabIndex = 18;
      this.label2.Text = "Rotation:";
      this.RotateXTextBox.Location = new System.Drawing.Point(121, 108);
      this.RotateXTextBox.Name = "RotateXTextBox";
      this.RotateXTextBox.Size = new Size(55, 26);
      this.RotateXTextBox.TabIndex = 17;
      this.RotateXTextBox.Text = "0";
      this.RotateXTextBox.TextAlign = HorizontalAlignment.Right;
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(310, 187);
      this.Controls.Add((Control) this.RotateZTextBox);
      this.Controls.Add((Control) this.RotateYTextBox);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.RotateXTextBox);
      this.Controls.Add((Control) this.ExportButton);
      this.Controls.Add((Control) this.ApplyButton);
      this.Controls.Add((Control) this.PositionZTextBox);
      this.Controls.Add((Control) this.PositionYTextBox);
      this.Controls.Add((Control) this.PositionLabel);
      this.Controls.Add((Control) this.PositionXTextBox);
      this.Controls.Add((Control) this.ScaleZTextBox);
      this.Controls.Add((Control) this.ScaleYTextBox);
      this.Controls.Add((Control) this.ScaleLabel);
      this.Controls.Add((Control) this.ScaleXTextBox);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.BlendModeComboBox);
      this.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = "ObjectManager";
      this.Text = "ObjectManager";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
