using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using MessageBox = System.Windows.Forms.MessageBox;

namespace DofusMultiUnity.Components
{
	internal static class Struct
    {
		public struct SubProcess
		{
			public Process Process;
			public Panel Panel;
			public TabPage Tab;

            public void Resize()
			{
				try
				{
					var width = SystemInformation.FrameBorderSize.Width;
					var height = SystemInformation.FrameBorderSize.Height;
					var num = SystemInformation.FrameBorderSize.Width * 2;
					var num2 = SystemInformation.FrameBorderSize.Height * 2;
					Api.SetWindowPos(Process.MainWindowHandle, 0, -width, -height, Panel.ClientRectangle.Width + num, Panel.ClientRectangle.Height + num2, 16);
				}
				catch (Exception ex)
				{
					MessageBox.Show($@"[Error] Resize process - {ex.Message}");
				}
			}

			public void Rename()
			{
				var textComponents = Process.MainWindowTitle.Split(" - ");
				if (textComponents.Length == 0)
				{
					Tab.Text = "";
					return;
				}
				Tab.Text = Process.MainWindowTitle.Split(" - ")[0];
			}

			public void Follow(IntPtr proc)
			{
				try
				{
					if (proc == Process.Id) 
						return;
					
					var num = checked((int)Core.Transform_Intptr(MovePosition.X, MovePosition.Y));
					Api.SendMessage(Process.MainWindowHandle, 513, 0, num);
					Api.SendMessage(Process.MainWindowHandle, 514, 0, num);
				}
				catch (Exception ex)
				{
					MessageBox.Show($@"[Error] Follow main process - {ex.Message}");
				}
			}
			
			public async Task DoubleClick()
			{
				try
				{
					var num = checked((int)Core.Transform_Intptr(MovePosition.X, MovePosition.Y));
					Api.SendMessage(Process.MainWindowHandle, 513, 0, num);
					Api.SendMessage(Process.MainWindowHandle, 514, 0, num);
					await Task.Delay(100);
					Api.SendMessage(Process.MainWindowHandle, 513, 0, num);
					Api.SendMessage(Process.MainWindowHandle, 514, 0, num);
				}
				catch (Exception ex)
				{
					MessageBox.Show($@"[Error] Double click - {ex.Message}");
				}
			}

			public void Release()
			{
				try
				{
					Api.SetParent(Process.MainWindowHandle, Api.GetDesktopWindow());
				}
				catch (Exception ex)
				{
					MessageBox.Show($@"[Error] Release process - {ex.Message}"); 
				}
			}

			public void Rec(bool @bool)
			{
				try
				{
					if (@bool)
					{
						var num = checked((int)Api.GetWindowLong(Process.MainWindowHandle, -16L));
						num = num & -12582913 & -8388609;
						Api.SetWindowLong(Process.MainWindowHandle, -16L, num);
					}
					Api.SetParent(Process.MainWindowHandle, Panel.Handle);
				}
				catch (Exception ex)
				{
					MessageBox.Show($@"[Error] Set process parent - {ex.Message}"); 
				}
			}
		}

		public static Point MovePosition;
		public static readonly List<SubProcess> ProcessList = [];
	}
}
