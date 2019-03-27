using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarsTupHelper
{
#pragma warning disable 0169
#pragma warning disable 0414

    public class StringParser : IDisposable
    {
        private string source_ = null;
        private int index_ = 0;
        private int length_ = 0;

        public StringParser(string source)
        {
            source_ = source;
            index_ = 0;
            length_ = source.Length;
        }

        public void Dispose()
        {
            //
        }
    }

#pragma warning restore 0414
#pragma warning restore 0169

}
