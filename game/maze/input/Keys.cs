namespace textured_raycast.maze.input
{
    // An enum of all the supported keys.
    enum Keys
    {
        K_W,
        K_A,
        K_S,
        K_D,
        K_E,

        K_1,
        K_2,
        K_3,

        K_SHIFT,
        K_LCTRL,
        K_RCTRL,

        K_UP,
        K_DOWN,
        K_LEFT,
        K_RIGHT,

        K_ESC,

        UNKNOWN,
    }

    // An enum of supported keygroups
    enum KeyGroups
    {
        KG_UP,
        KG_DOWN,
        KG_LEFT,
        KG_RIGHT,
    }

    // An enum of all the supported keystates
    enum KeyState
    {
        KEY_UP,
        KEY_DOWN,
        KEY_HELD,
    }
}
