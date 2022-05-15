namespace textured_raycast.maze.input
{
    // An interface, holding the two functions needed for lower-level
    // input.
    interface ILowLevelInput {
	// Inits the input controller
        void Init();
	// Gets the keystate for a specific key.
        KeyState GetKey(Keys key);
    }
}
