using System.Runtime;

namespace textured_raycast.maze.Utils
{
    public class DeltaVariable
    {
        double[] lastVars;

        public DeltaVariable(double[] start)
        {
            lastVars = start;
        }

    public double[] GetDelta(double[] absolute) {
            double[] delta = new double[lastVars.Length];
            for(int i = 0; i < lastVars.Length; i++) {
        delta[i] = absolute[i] - lastVars[i];
        lastVars[i] = absolute[i];
        }
        return delta;
    }
    }
}
