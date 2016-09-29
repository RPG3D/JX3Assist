using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    public class DatabaseReader
    {
        public int? numberValue = null;
        public bool? boolValue = true;
        public int? GetIntFromDatabase()
        {
            return numberValue;
        }

        public bool? GetBoolFromDatabase()
        {
            return boolValue;
        }

    }
}