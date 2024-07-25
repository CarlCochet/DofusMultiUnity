using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace DofusMultiUnity.Components
{
    internal static class Core
    {
		public static Point DragStartPosition = Point.Empty;
		private static readonly Random Random = new();

		public static long Transform_Intptr(int loWord, int hiWord)
		{
			return checked(hiWord * 65536L) | (loWord & 0xFFFFL);
		}

		public static void InitTabs(TabControl tabControl)
		{
			try
			{
				if (Operators.ConditionalCompareObjectEqual(Param("path"), "", TextCompare: false))
				{
					Reset();
				}
				tabControl.TabPages.Clear();
				foreach (var process in Struct.ProcessList)
				{
					process.Release();
				}
				Struct.ProcessList.Clear();
			}
			catch (Exception ex)
			{
				MessageBox.Show($@"[Error] Release processes - {ex.Message}");
			}
			try
			{
				var processes = Process.GetProcesses();
				foreach (var process in processes)
				{
					if (Operators.CompareString(process.ProcessName, "Dofus", TextCompare: false) != 0)
						continue;
							
					var tabPage = new TabPage
					{
						Text = @"Dofus",
						ToolTipText = process.Id.ToString()
					};
					var panel = new Panel { Dock = DockStyle.Fill };
					var subprocess = default(Struct.SubProcess);
					subprocess.Process = process;
					subprocess.Panel = panel;
					subprocess.Tab = tabPage;
					tabControl.TabPages.Add(tabPage);
					tabPage.Controls.Add(panel);
					Struct.ProcessList.Add(subprocess);
				}
				foreach (var process in Struct.ProcessList)
				{
					process.Rec(@bool: true);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($@"[Error] Catch processes - {ex.Message}");
			}
			try
			{
				foreach (var process in Struct.ProcessList)
				{
					process.Resize();
					process.Rename();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($@"[Error] Resize processes - {ex.Message}");
			}
	    }

		public static void RenameTab(TabControl tabControl)
		{
			if (tabControl.Controls.Count == 0) 
				return;
			if (tabControl.SelectedTab == null)
				return;
			
			var text = Interaction.InputBox("Insert new tab name");
			tabControl.SelectedTab.Text = text;
		}

		public static void CloseTab(TabControl tabControl)
		{
			if (tabControl.Controls.Count == 0) 
				return;
			
			foreach (var process in Struct.ProcessList)
			{
				if (!IsSelectedTab(tabControl, process))
					continue;
				if (tabControl.SelectedTab == null)
					return;
				
				process.Process.Kill();
				tabControl.TabPages.Remove(tabControl.SelectedTab);
			}
		}

		public static void CloseTabs(IntPtr handle)
		{
			try
			{
				Api.UnregisterHotKey(handle, HotKey.SwitchHotkey);
				Api.UnregisterHotKey(handle, HotKey.RefreshHotkey);
				foreach (var process in Struct.ProcessList)
				{
					process.Process.Kill();
				}
				ProjectData.EndApp();
			}
			catch (Exception ex)
			{
				MessageBox.Show($@"[Error] Close processes - {ex.Message}");
			}
		}

		public static void ResizeProcesses()
		{
			try
			{
				foreach (var process in Struct.ProcessList)
				{
					process.Resize();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($@"[Error] Resize processes - {ex.Message}");
			}
		}

		public static async Task FollowMainProcess(TabControl tabControl)
		{
			try
			{
				var mouseButtons = Control.MouseButtons;
				if (mouseButtons != MouseButtons.XButton2)
					return;
						
				if (tabControl.SelectedTab != null)
					Struct.MovePosition = tabControl.SelectedTab.PointToClient(Control.MousePosition);
				
				var proc = Api.WindowFromPoint(Cursor.Position);
				foreach (var process in Struct.ProcessList)
				{
					process.Follow(proc);
					await Task.Delay(Random.Next(100, 400));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				MessageBox.Show($@"[Error] Follow main process - {ex.Message}");
			}
		}

		public static async Task DoubleClick(TabControl tabControl)
		{
			var mouseButtons = Control.MouseButtons;
			if (mouseButtons != MouseButtons.XButton1)
				return;
			
			if (tabControl.SelectedTab != null)
				Struct.MovePosition = tabControl.SelectedTab.PointToClient(Control.MousePosition);

			foreach (var process in Struct.ProcessList)
			{
				if (!IsSelectedTab(tabControl, process))
					continue;
				await process.DoubleClick();
			}
		}

		public static TabPage HoverTab(TabControl tabControl)
		{
			var num = tabControl.TabCount - 1;
			for (var i = 0; i <= num; i++)
			{
				if (tabControl.GetTabRect(i).Contains(tabControl.PointToClient(Cursor.Position)))
					return tabControl.TabPages[i];
			}
			return null;
		}

		public static void SwapTabPages(TabControl tabControl, TabPage tp1, TabPage tp2)
		{
			var index = tabControl.TabPages.IndexOf(tp1);
			var index2 = tabControl.TabPages.IndexOf(tp2);
			tabControl.TabPages[index] = tp2;
			tabControl.TabPages[index2] = tp1;
		}

		private static bool IsSelectedTab(TabControl tabControl, Struct.SubProcess process)
		{
			if (tabControl.SelectedTab == null)
				return false;
			return tabControl.SelectedTab.Text == process.Process.MainWindowTitle.Split(" - ")[0];
		}
		
		private static void Reset()
		{
			try
			{
				WParam("winOpen", "1");
				WParam("path", "");
				WParam("Alt", "0");
				WParam("Ctrl", "0");
				WParam("Windows", "0");
				WParam("hotkey", "F3");
			}
			catch (Exception ex)
			{
				MessageBox.Show($@"[Error] Reset shortcuts - {ex.Message}");
			}
		}
		
		private static void WParam(string par, string val)
		{
			try
			{
				Registry.SetValue("HKEY_CURRENT_USER\\DofusMultiUnity\\Settings", par, val);
			}
			catch (Exception ex)
			{
				MessageBox.Show($@"[Error] Set hotkeys - {ex.Message}");
			}
		}
		
		private static object Param(string param)
		{
			var result = default(object);
			try
			{
				var text = Conversions.ToString(Registry.GetValue("HKEY_CURRENT_USER\\DofusMultiUnity\\Settings", param, ""));
				if (Operators.CompareString(text, "1", TextCompare: false) == 0)
				{
					result = CheckState.Checked;
					return result;
				}
				if (Operators.CompareString(text, "0", TextCompare: false) == 0)
				{
					result = CheckState.Unchecked;
					return result;
				}
				result = text;
				return result;
			}
			catch (Exception ex)
			{
				MessageBox.Show($@"[Error] Get hotkey - {ex.Message}");
				return result;
			}
		}
	}
}
