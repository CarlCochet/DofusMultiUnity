using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using DofusMultiUnity.Components;

namespace DofusMultiUnity
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Resize += Form1_Resize;
            Load += Form1_Load;
            HotkeyActive += Start;
            FormClosing += Form1_FormClosing;
        }

        public delegate void HotkeyActiveEventHandler(int id);
        private HotkeyActiveEventHandler _hotkeyActiveEvent;

        public event HotkeyActiveEventHandler HotkeyActive
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add => _hotkeyActiveEvent = (HotkeyActiveEventHandler)Delegate.Combine(_hotkeyActiveEvent, value);
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove => _hotkeyActiveEvent = (HotkeyActiveEventHandler)Delegate.Remove(_hotkeyActiveEvent, value);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Core.CloseTabs(Handle);
        }

        private void Start(int id)
        {
            switch (id)
            {
                case HotKey.SwitchHotkey:
                    tabControl1.SelectedIndex = (tabControl1.SelectedIndex + 1) % tabControl1.TabCount;
                    break;
                case HotKey.RefreshHotkey:
                    foreach (var process in Struct.ProcessList)
                    {
                        process.Rename();
                    }
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Core.InitTabs(tabControl1);
            Api.RegisterHotKey(Handle, HotKey.RefreshHotkey, HotKey.FsModifiers.None, Keys.F5);
            Api.RegisterHotKey(Handle, HotKey.SwitchHotkey, HotKey.FsModifiers.None, Keys.F3);
            tabControl1.AllowDrop = true;
            timer1.Start();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 786)
            {
                switch (m.WParam.ToInt32())
                {
                    case HotKey.SwitchHotkey:
                        _hotkeyActiveEvent?.Invoke(HotKey.SwitchHotkey);
                        break;
                    case HotKey.RefreshHotkey:
                        _hotkeyActiveEvent?.Invoke(HotKey.RefreshHotkey);
                        break;
                }
            }
            base.WndProc(ref m);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Core.ResizeProcesses();
            Api.RegisterHotKey(Handle, 1, HotKey.FsModifiers.None, Keys.F5);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Core.FollowMainProcess(tabControl1);
            Core.DoubleClick(tabControl1);
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            Core.DragStartPosition = new Point(e.X, e.Y);
        }

        private void tabControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var rectangle = new Rectangle(Core.DragStartPosition, Size.Empty);
            rectangle.Inflate(SystemInformation.DragSize);
            var tabPage = Core.HoverTab(tabControl1);

            if (tabPage != null && !rectangle.Contains(e.X, e.Y))
                tabControl1.DoDragDrop(tabPage, DragDropEffects.All);

            Core.DragStartPosition = Point.Empty;
        }

        private void tabControl1_DragOver(object sender, DragEventArgs e)
        {
            var tabPage = Core.HoverTab(tabControl1);
            if (tabPage == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            if (e.Data == null)
                return;
            if (!e.Data.GetDataPresent(typeof(TabPage)))
                return;

            e.Effect = DragDropEffects.Move;
            var tabPage2 = (TabPage)e.Data.GetData(typeof(TabPage));

            if (tabPage == tabPage2)
                return;

            var tabRect = tabControl1.GetTabRect(tabControl1.TabPages.IndexOf(tabPage));
            tabRect.Inflate(-3, -3);
            var tabControl = tabControl1;
            var p = new Point(e.X, e.Y);

            if (!tabRect.Contains(tabControl.PointToClient(p)))
                return;

            Core.SwapTabPages(tabControl, tabPage2, tabPage);
            tabControl1.SelectedTab = tabPage2;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Core.ResizeProcesses();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Core.CloseTab(tabControl1);
            Core.InitTabs(tabControl1);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Core.RenameTab(tabControl1);
        }
    }
}
