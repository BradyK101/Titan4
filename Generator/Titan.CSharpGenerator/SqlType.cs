using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan.CSharpGenerator
{
    public enum SqlType
    {
        Cursor = 3000,
        Boolean = 16,
        Byte = -6,
        Short = 5,
        Integer = 4,
        Long = -5,
        Float = 7,
        Double = 8,
        NVarChar = -9,
        VarChar = 12,
        NClob = 2011,
        Clob = 2005,
        Blob = 2004,
        DateTime = 91,
        Char = -27,
        //Time = 92,
        //Timestamp = 93,
        //BIGDECIMAL = 2 
        Decimal = 1,
    }
}
