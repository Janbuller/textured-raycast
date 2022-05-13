namespace textured_raycast.maze.input
{
    interface ILowLevelInput {
        KeyState GetKey(Keys key);
        void Init();
    }
}
