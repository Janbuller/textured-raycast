using Xunit;
using textured_raycast.maze.math;

namespace tests.math;

public class Matrix2x2d_Test
{
    [Fact]
    public void Constructr_getE()
    {
        var v1 = new Matrix2x2d(new double[] { 5.2, 2.3,
					       3.7, 5.6});

        Assert.True(v1.getE(0, 1) == 3.7 &&
		    v1.getE(1, 1) == 5.6);
    }

    [Fact]
    public void getE_setE()
    {
        var v1 = new Matrix2x2d(new double[] { 5.2, 2.3,
					       3.7, 5.6});

        v1.setE(1, 1, 222.54);

        Assert.True(v1.getE(0, 1) == 3.7 &&
		    v1.getE(1, 1) == 222.54);
    }

    [Fact]
    public void VectorMultiplication_RHS()
    {
        var v1 = new Matrix2x2d(new double[] { 5.2, 2.3,
					       3.7, 5.6});

        var v2 = new Vector2d(2.55, 1.32);

        var res = new Vector2d(16.296, 16.826999999999998);

        Assert.True(v1 * v2 == res);
    }

    [Fact]
    public void VectorMultiplication_LHS()
    {
        var v1 = new Matrix2x2d(new double[] { 5.2, 2.3,
					       3.7, 5.6});

        var v2 = new Vector2d(2.55, 1.32);

        var res = new Vector2d(16.296, 16.826999999999998);

        Assert.True(v2 * v1 == res);
    }

    [Fact]
    public void ScalarMultiplication_RHS()
    {
        var v1 = new Matrix2x2d(new double[] { 5.2, 2.3,
					       3.7, 5.6});

        var v2 = 2;

        var res = new Matrix2x2d(new double[] { 10.4, 4.6,
						7.4, 11.2});

        Assert.True((v1*v2).getE(0,0) == res.getE(0,0) &&
		    (v1*v2).getE(0,1) == res.getE(0,1) &&
		    (v1*v2).getE(1,0) == res.getE(1,0) &&
		    (v1*v2).getE(1,1) == res.getE(1,1));
    }

    [Fact]
    public void ScalarMultiplication_LHS()
    {
        var v1 = new Matrix2x2d(new double[] { 5.2, 2.3,
					       3.7, 5.6});

        var v2 = 2;

        var res = new Matrix2x2d(new double[] { 10.4, 4.6,
						7.4, 11.2});

        Assert.True((v2*v1).getE(0,0) == res.getE(0,0) &&
		    (v2*v1).getE(0,1) == res.getE(0,1) &&
		    (v2*v1).getE(1,0) == res.getE(1,0) &&
		    (v2*v1).getE(1,1) == res.getE(1,1));
    }

    [Fact]
    public void InverseMatrix()
    {
        var v1 = new Matrix2x2d(new double[] { 5.2, 2.3,
					       3.7, 5.6});
	double determinant = 1/(v1.getE(0,0) * v1.getE(1,1) - v1.getE(1,0) * v1.getE(0,1));
	
	Matrix2x2d invMat = new Matrix2x2d(new double[] { v1.getE(1,1), -v1.getE(1,0),
							  -v1.getE(0, 1), v1.getE(0,0)});
	var res = invMat * determinant;

        v1 = v1.getInverse();

        Assert.True(v1.getE(0,0) == res.getE(0,0) &&
		    v1.getE(0,1) == res.getE(0,1) &&
		    v1.getE(1,0) == res.getE(1,0) &&
		    v1.getE(1,1) == res.getE(1,1));
    }
}
