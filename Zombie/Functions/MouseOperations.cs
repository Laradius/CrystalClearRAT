using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using WindowsInput;

namespace Zombie.Functions
{
    public static class MouseOperations
    {
        private static InputSimulator inputSimulator = new InputSimulator();

        enum SystemMetric
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1,
        }

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(SystemMetric smIndex);

        private static int CalculateAbsoluteCoordinateX(int x)
        {
            return (x * 65536) / GetSystemMetrics(SystemMetric.SM_CXSCREEN);
        }

        private static int CalculateAbsoluteCoordinateY(int y)
        {
            return (y * 65536) / GetSystemMetrics(SystemMetric.SM_CYSCREEN);
        }

        public static void DoubleLeftClickOnPoint(int x, int y)
        {
            inputSimulator.Mouse.MoveMouseTo(CalculateAbsoluteCoordinateX(x), CalculateAbsoluteCoordinateY(y));
            inputSimulator.Mouse.LeftButtonDoubleClick();
        }


        public static MousePoint ConvertScreenPointToCurrent(int x, int oldMaxX, int y, int oldMaxY)
        {
            int newMaxX = Screen.PrimaryScreen.Bounds.Width;
            int newMax = Screen.PrimaryScreen.Bounds.Width;

            int convertedX = x * Screen.PrimaryScreen.Bounds.Width / (oldMaxX);
            int convertedY = y * Screen.PrimaryScreen.Bounds.Height / (oldMaxY);

            return new MousePoint(convertedX, convertedY);

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MousePoint
    {
        public int X;
        public int Y;

        public MousePoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}