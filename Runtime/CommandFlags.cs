using System;

namespace Dada.Commander
{
    [Flags]
    public enum CommandFlags
    {
        // default flag
        noFlags = 1,



        //[Examples] you can customize it and add new
        main = 2,
        hidedForHelp = 4,
        admin = 8,




        //you always should have "all" flag that contains all bytes of another flags
        all = 255
    }
}
