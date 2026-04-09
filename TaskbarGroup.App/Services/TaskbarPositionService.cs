using System.Runtime.InteropServices;
using System.Windows;

namespace TaskbarGroup.App.Services;

public class TaskbarPositionService
{
    public Point CalculatePopupTopLeft(Size popupSize, double margin = 8)
    {
        var workArea = SystemParameters.WorkArea;

        if (!TryGetTaskbarRect(out var rect))
        {
            return new Point(workArea.Right - popupSize.Width - margin, workArea.Bottom - popupSize.Height - margin);
        }

        var width = popupSize.Width;
        var height = popupSize.Height;

        // taskbar bottom
        if (rect.Top > 0 && rect.Left == 0)
        {
            return new Point(rect.Right - width - margin, rect.Top - height - margin);
        }

        // taskbar top
        if (rect.Top == 0 && rect.Left == 0 && rect.Bottom < workArea.Bottom)
        {
            return new Point(rect.Right - width - margin, rect.Bottom + margin);
        }

        // taskbar left
        if (rect.Left == 0 && rect.Right < workArea.Right)
        {
            return new Point(rect.Right + margin, rect.Bottom - height - margin);
        }

        // taskbar right
        return new Point(rect.Left - width - margin, rect.Bottom - height - margin);
    }

    private static bool TryGetTaskbarRect(out RECT rect)
    {
        var appBar = new APPBARDATA();
        appBar.cbSize = (uint)Marshal.SizeOf(appBar);
        var result = SHAppBarMessage(ABM_GETTASKBARPOS, ref appBar);
        rect = appBar.rc;
        return result != IntPtr.Zero;
    }

    private const int ABM_GETTASKBARPOS = 0x00000005;

    [DllImport("shell32.dll")]
    private static extern IntPtr SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

    [StructLayout(LayoutKind.Sequential)]
    private struct APPBARDATA
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public uint uEdge;
        public RECT rc;
        public int lParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
