using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintTool.Shell.Tests.Utils
{
    public class FileComparer
    {
        public static bool Compare(byte[] sourceFile, byte[] descinationFile,bool compareBodyBytes=true)
        {
            if (sourceFile.Length != descinationFile.Length)
                return false;

            if (compareBodyBytes)
            {
                for (int i = 0; i < sourceFile.Length; i++)
                {
                    if (descinationFile[i] != sourceFile[i])
                        return false;
                }
            }

            return true;
        }
    }
}
