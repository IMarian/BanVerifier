﻿namespace BANProtocolVerfier
{
    partial class Form1
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
            this.txtProtocol = new System.Windows.Forms.TextBox();
            this.loadBtn = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.analyzeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtProtocol
            // 
            this.txtProtocol.Location = new System.Drawing.Point(12, 12);
            this.txtProtocol.Multiline = true;
            this.txtProtocol.Name = "txtProtocol";
            this.txtProtocol.ReadOnly = true;
            this.txtProtocol.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProtocol.Size = new System.Drawing.Size(610, 658);
            this.txtProtocol.TabIndex = 0;
            // 
            // loadBtn
            // 
            this.loadBtn.Location = new System.Drawing.Point(100, 676);
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(135, 23);
            this.loadBtn.TabIndex = 1;
            this.loadBtn.Text = "Load protocol";
            this.loadBtn.UseVisualStyleBackColor = true;
            this.loadBtn.Click += new System.EventHandler(this.loadBtn_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(241, 676);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(135, 23);
            this.btnVerify.TabIndex = 2;
            this.btnVerify.Text = "Interpret/Reload protocol";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // analyzeButton
            // 
            this.analyzeButton.Location = new System.Drawing.Point(382, 676);
            this.analyzeButton.Name = "analyzeButton";
            this.analyzeButton.Size = new System.Drawing.Size(135, 23);
            this.analyzeButton.TabIndex = 4;
            this.analyzeButton.Text = "Analyze protocol";
            this.analyzeButton.UseVisualStyleBackColor = true;
            this.analyzeButton.Click += new System.EventHandler(this.analyzeButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 711);
            this.Controls.Add(this.analyzeButton);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.loadBtn);
            this.Controls.Add(this.txtProtocol);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtProtocol;
        private System.Windows.Forms.Button loadBtn;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button analyzeButton;
    }
}

