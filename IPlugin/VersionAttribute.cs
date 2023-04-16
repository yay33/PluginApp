using System;
using System.Collections.Generic;
using System.Text;

namespace PluginInterface
{
    public class VersionAttribute : Attribute
    {
        public int Major { get; private set; }
        public int Minor { get; private set; }
        public VersionAttribute(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }

    }
}
