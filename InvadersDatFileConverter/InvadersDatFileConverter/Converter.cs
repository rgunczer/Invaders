using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Script.Serialization;

namespace InvadersDatFileConverter
{
    public sealed class Converter
    {
        private string _savePathRoot;

        public Converter(string savePathRoot)
        {
            _savePathRoot = savePathRoot;
        }

        public string ConvertEnemy(List<byte> sourceData, string fileName)
        {            
            byte b;
            List<byte> convertedData = new List<byte>(sourceData.Count);
                        
            for (int pos = 0; pos < sourceData.Count; ++pos)
            {
                b = sourceData[pos];

                if (b != 0 && b != 200 && b != 255)
                {
                    convertedData.Add(b);
                }
                else if (b == 200)
                {
                    convertedData.Add(2);
                }
                else
                {
                    convertedData.Add(0);
                }
            }

            string fullPath = Path.Combine(_savePathRoot, fileName);
            writeToJson(convertedData, fullPath);
            return fullPath;
        }

        public string ConvertPlayer(List<byte> sourceData, string fileName)
        {
            byte b;            
            List<byte> convertedData = new List<byte>(sourceData.Count);
            
            for (int i = 0; i < sourceData.Count; ++i)
            {
                b = sourceData[i];

                if (b == 15)
                {
                    convertedData.Add(1);
                }
                else if (b == 200)
                {
                    convertedData.Add(2);
                }
                else
                {
                    convertedData.Add(0);
                }
            }

            string fullPath = Path.Combine(_savePathRoot, fileName);
            writeToJson(convertedData, fullPath);
            return fullPath;
        }

        public string ConvertPlayerLive(List<byte> sourceData, string fileName)
        {
            byte b;            
            List<byte> convertedData = new List<byte>(sourceData.Count);
            
            for (int i = 0; i < sourceData.Count; ++i)
            {
                b = sourceData[i];

                if (b == 15)
                {
                    convertedData.Add(1);
                }
                else if (b == 200)
                {
                    convertedData.Add(2);
                }
                else
                {
                    convertedData.Add(0);
                }
            }

            string fullPath = Path.Combine(_savePathRoot, fileName);
            writeToJson(convertedData, fullPath);
            return fullPath;
        }

        public string ConvertTitle(List<byte> sourceData, string fileName)
        {
            byte b;            
            List<byte> convertedData = new List<byte>(sourceData.Count);

            for (int i = 0; i < sourceData.Count; ++i)
            {
                b = sourceData[i];
                
                if (b == 11)
                {
                    convertedData.Add(1);
                }
                else if (b == 200)
                {                                
                    convertedData.Add(2);
                }
                else
                {
                    convertedData.Add(0);
                }

            }

            string fullPath = Path.Combine(_savePathRoot, fileName);
            writeToJson(convertedData, fullPath);
            return fullPath;
        }

        private void writeToJson(List<byte> convertedData, string fullPath)
        {
            var v = new { data = string.Join("", convertedData) };

            string json = new JavaScriptSerializer().Serialize(v);            
            File.WriteAllText(fullPath, json);
        }

    }
}
