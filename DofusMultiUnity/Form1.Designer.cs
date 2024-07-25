
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace DofusMultiUnity
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            contextMenuStrip1 = new ContextMenuStrip(components);
            renommerToolStripMenuItem = new ToolStripMenuItem();
            fermerToolStripMenuItem = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            timer1 = new Timer(components);
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { renommerToolStripMenuItem, fermerToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(134, 48);
            // 
            // renommerToolStripMenuItem
            // 
            renommerToolStripMenuItem.Name = "renommerToolStripMenuItem";
            renommerToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            renommerToolStripMenuItem.Text = "Renommer";
            renommerToolStripMenuItem.Click += renameToolStripMenuItem_Click;
            // 
            // fermerToolStripMenuItem
            // 
            fermerToolStripMenuItem.Name = "fermerToolStripMenuItem";
            fermerToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            fermerToolStripMenuItem.Text = "Fermer";
            fermerToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            tabControl1.ContextMenuStrip = contextMenuStrip1;
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.HotTrack = true;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(1584, 861);
            tabControl1.TabIndex = 1;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            tabControl1.DragOver += tabControl1_DragOver;
            tabControl1.MouseDown += tabControl1_MouseDown;
            tabControl1.MouseMove += tabControl1_MouseMove;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1584, 861);
            Controls.Add(tabControl1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Multicompte Unity";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            Resize += Form1_Resize;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        [AccessedThroughProperty("tabControl1")]
        private TabControl tabControl1;
        private Timer timer1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem fermerToolStripMenuItem;
        private ToolStripMenuItem renommerToolStripMenuItem;
    }
}

