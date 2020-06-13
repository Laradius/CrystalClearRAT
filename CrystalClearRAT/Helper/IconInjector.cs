using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClearRAT.Helper
{
    class IconInjector
    {

        private static readonly ushort _reserved = 0;
        private static readonly ushort _iconType = 1;

        private static readonly uint RT_ICON = 3;
        private static readonly uint RT_GROUP_ICON = 14;

        public class Icon
        {

            public byte Width { get; private set; }
            public byte Height { get; private set; }
            public byte Colors { get; private set; }

            public uint Size { get; private set; }

            public uint Offset { get; private set; }

            public ushort ColorPlanes { get; private set; }

            public ushort BitsPerPixel { get; private set; }

            public byte[] Data { get; set; }

            public Icon(byte width, byte height, byte colors, uint size, uint offset, ushort colorPlanes, ushort bitsPerPixel)
            {
                Width = width;
                Height = height;
                Colors = colors;
                Size = size;
                Offset = offset;
                ColorPlanes = colorPlanes;
                BitsPerPixel = bitsPerPixel;
            }

        }

        public class Icons : List<Icon>
        {
            public byte[] ToGroupData(int startindex = 1)
            {
                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write(_reserved);
                    writer.Write(_iconType);
                    writer.Write((ushort)this.Count);

                    for (int i = 0; i < this.Count; i++)
                    {
                        Icon icon = this[i];
                        writer.Write(icon.Width);
                        writer.Write(icon.Height);
                        writer.Write(icon.Colors);

                        writer.Write((byte)_reserved);
                        writer.Write(icon.ColorPlanes);

                        writer.Write(icon.BitsPerPixel);

                        writer.Write(icon.Size);

                        writer.Write((ushort)(startindex + i));
                    }

                    return ms.ToArray();
                }
            }
        }

        public static class IconReader
        {

            public static Icons Read(Stream input)
            {
                Icons icons = new Icons();

                using (BinaryReader reader = new BinaryReader(input))
                {
                    reader.ReadUInt16();
                    var type = reader.ReadUInt16();
                    if (type != _iconType)
                    {
                        throw new InvalidOperationException("The selected file isn't an .ico file.");
                    }
                    var num_of_images = reader.ReadUInt16();

                    for (var i = 0; i < num_of_images; i++)
                    {
                        var width = reader.ReadByte();
                        var height = reader.ReadByte();
                        var colors = reader.ReadByte();
                        reader.ReadByte();

                        var colorPlanes = reader.ReadUInt16();

                        var bitsPerPixel = reader.ReadUInt16();

                        var size = reader.ReadUInt32();

                        var offset = reader.ReadUInt32();


                        icons.Add(new Icon(width, height, colors, size, offset, colorPlanes, bitsPerPixel));



                    }

                    foreach (var icon in icons)
                    {
                        if (reader.BaseStream.Position < icon.Offset)
                        {
                            var irrelevantData = (int)(icon.Offset - reader.BaseStream.Position);
                            reader.ReadBytes(irrelevantData);
                        }

                        var data = reader.ReadBytes((int)icon.Size);

                        icon.Data = data;
                    }

                }
                return icons;
            }

        }


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int UpdateResource(IntPtr hUpdate, uint lpType, ushort lpName, ushort wLanguage, byte[] lpData, uint cbData);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr BeginUpdateResource(string pFileName, [MarshalAs(UnmanagedType.Bool)] bool bDeleteExistingResources);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        public static void ChangeIcon(string exeFilePath, string iconFilePath)
        {
            using (FileStream fs = new FileStream(iconFilePath, FileMode.Open, FileAccess.Read))
            {

                var reader = IconReader.Read(fs);
                ChangeIcon(exeFilePath, reader);
            }
        }

        private static void ChangeIcon(string exeFilePath, Icons icons)
        {

            IntPtr handleExe = BeginUpdateResource(exeFilePath, false);

            ushort startindex = 1;
            ushort index = startindex;
            int ret;
            foreach (var icon in icons)
            {
                ret = UpdateResource(handleExe, RT_ICON, index, 0, icon.Data, icon.Size);

                index++;
            }

            var groupdata = icons.ToGroupData();

            ret = UpdateResource(handleExe, RT_GROUP_ICON, startindex, 0, groupdata, (uint)groupdata.Length);
            if (ret == 1)
            {
                EndUpdateResource(handleExe, false);

            }

        }
    }
}
