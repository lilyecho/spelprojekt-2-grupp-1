using System;

public enum Materialcomposition {
    
    None,
    Dirt,
    Gras,
    Wood,
    Stone
}

[Flags]
public enum CharacterActivity
{
    Sneak=1,
    Walk=2,
    Run=4,
    Jump=8,
    Grab=16
}
