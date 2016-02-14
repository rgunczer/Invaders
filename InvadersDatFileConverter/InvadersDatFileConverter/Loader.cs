using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InvadersDatFileConverter
{
    public sealed class Loader
    {
        private string _pathRoot;

        public Loader(string pathRoot)
        {
            _pathRoot = pathRoot;
        }

        public List<byte> LoadTitle(string fileName)
        {
            string fullPath = Path.Combine(_pathRoot, fileName);
            List<byte> list = new List<byte>();

            using (BinaryReader b = new BinaryReader(File.Open(fullPath, FileMode.Open)))
            {
                int pos = 0;
                int length = (int)b.BaseStream.Length;
                while (pos < length)
                {
                    byte b1 = b.ReadByte();
                    byte b2 = b.ReadByte();
                    pos += sizeof(byte) * 2;

                    list.Add(b1);
                }
            }

            return list;
        }

        public List<byte> LoadEnemy(string fileName)
        {
            string fullPath = Path.Combine(_pathRoot, fileName);
            List<byte> list = new List<byte>();
            bool ignore = true;

            using (BinaryReader b = new BinaryReader(File.Open(fullPath, FileMode.Open)))
            {
                int pos = 0;
                int length = (int)b.BaseStream.Length;                
                while (pos < length)
                {
                    byte b1 = b.ReadByte();
                    byte b2 = b.ReadByte();
                    pos += sizeof(byte) * 2;

                    if (ignore)
                    {
                        if (b1 == 255)
                        {
                            ignore = false;
                        }
                    }
                    else
                    {
                        list.Add(b1);
                    }
                }
            }

            return list;
        }

        public List<byte> LoadPlayerLive(string fileName)
        {
            string fullPath = Path.Combine(_pathRoot, fileName);
            List<byte> list = new List<byte>();

            using (BinaryReader b = new BinaryReader(File.Open(fullPath, FileMode.Open)))
            {
                int pos = 0;
                int length = (int)b.BaseStream.Length;
                while (pos < length)
                {
                    byte b1 = b.ReadByte();
                    byte b2 = b.ReadByte();
                    pos += sizeof(byte) * 2;

                    list.Add(b1);
                }
            }

            return list;
        }

        public List<byte> LoadPlayer(string fileName)
        {
            string fullPath = Path.Combine(_pathRoot, fileName);
            List<byte> list = new List<byte>();

            using (BinaryReader b = new BinaryReader(File.Open(fullPath, FileMode.Open)))
            {
                int pos = 0;
                int length = (int)b.BaseStream.Length;
                while (pos < length)
                {
                    byte b1 = b.ReadByte();
                    byte b2 = b.ReadByte();
                    pos += sizeof(byte) * 2;

                    if (b1 == 255)
                    {
                        break;
                    }

                    list.Add(b1);
                }
            }

            return list;
        }


    }
}
