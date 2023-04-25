using System;

namespace Dada.Commander
{
    [Flags]
    public enum CommandFlags
    {
        //you can customize it
        noFlags = 1,
        main = 2,
        help = 4,
        admin = 8,




        //you always should have "all" flag that contains all bytes of another flags
        all = 255
    }
}
