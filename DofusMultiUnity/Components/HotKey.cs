using System;

namespace DofusMultiUnity.Components
{
	internal static class HotKey
    {
		[Flags]
		public enum FsModifiers
		{
			None = 0,
			Alt = 1,
			Control = 2,
			Shift = 4,
			Windows = 8,
			AltCtrl = 3,
			AltShift = 5,
			AltWindows = 9,
			CtrlShift = 6,
			CtrlWindows = 10,
			ShiftWindows = 12,
			NoRepeat = 0x4000
		}

		public const int WmHotkey = 786;
		public const int SwitchHotkey = 444719;
		public const int RefreshHotkey = 1;
	}
}
