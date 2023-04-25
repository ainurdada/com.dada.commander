using System;

[Flags]
public enum CommandFlags
{
    noFlags = 1,
    main = 2,
    help = 4,
    admin = 8,
    all = 255
}
