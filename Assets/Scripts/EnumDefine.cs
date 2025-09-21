using System;

[Flags]
public enum EGameObjectState
{
    Hidden = 1 << 0,
    Invisible = 1 << 1,
    Minimized = 1 << 2,
    Visible = 1 << 3
}