using Xunit;
using textured_raycast.maze.math;

namespace tests.math;

public class Vector2i_Test
{
    [Fact]
    public void EqualityOperator_IsEqual()
    {
        var v1 = new Vector2i(2, 5);
        var v2 = new Vector2i(2, 5);

        Assert.True(v1 == v2);
    }

    [Fact]
    public void EqualityOperator_IsUnequal_LHS()
    {
        var v1 = new Vector2i(2, 5);
        var v2 = new Vector2i(1, 5);

        Assert.False(v1 == v2);
    }

    [Fact]
    public void EqualityOperator_IsUnequal_RHS()
    {
        var v1 = new Vector2i(2, 5);
        var v2 = new Vector2i(2, 6);

        Assert.False(v1 == v2);
    }

    [Fact]
    public void InequalityOperator_IsEqual()
    {
        var v1 = new Vector2i(2, 5);
        var v2 = new Vector2i(2, 5);

        Assert.False(v1 != v2);
    }

    [Fact]
    public void InequalityOperator_IsUnequal_LHS()
    {
        var v1 = new Vector2i(2, 5);
        var v2 = new Vector2i(1, 5);

        Assert.True(v1 != v2);
    }

    [Fact]
    public void InequalityOperator_IsUnequal_RHS()
    {
        var v1 = new Vector2i(2, 5);
        var v2 = new Vector2i(2, 6);

        Assert.True(v1 != v2);
    }

    [Fact]
    public void AdditionOperator_Integer_Positive()
    {
        var v1 = new Vector2i(5, 3);
        var v2 = new Vector2i(7, 4);

        var res = new Vector2i(5 + 7, 3 + 4);

        Assert.True((v1 + v2) == res);
    }

    [Fact]
    public void AdditionOperator_Integer_Negative()
    {
        var v1 = new Vector2i(-5, 3);
        var v2 = new Vector2i(-7, -4);

        var res = new Vector2i(-5 + -7, 3 + -4);

        Assert.True((v1 + v2) == res);
    }

    [Fact]
    public void SubtractionOperator_Integer_Positive()
    {
        var v1 = new Vector2i(5, 3);
        var v2 = new Vector2i(7, 4);

        var res = new Vector2i(5 - 7, 3 - 4);

        Assert.True((v1 - v2) == res);
    }

    [Fact]
    public void SubtractionOperator_Integer_Negative()
    {
        var v1 = new Vector2i(-5, 3);
        var v2 = new Vector2i(-7, -4);

        var res = new Vector2i(-5 - -7, 3 - -4);

        Assert.True((v1 - v2) == res);
    }

    [Fact]
    public void MultiplicationOperator_Integer_Positive()
    {
        var v1 = new Vector2i(5, 3);
        var v2 = new Vector2i(7, 4);

        var res = new Vector2i(5 * 7, 3 * 4);

        Assert.True((v1 * v2) == res);
    }

    [Fact]
    public void MultiplicationOperator_Integer_Negative()
    {
        var v1 = new Vector2i(-5, 3);
        var v2 = new Vector2i(-7, -4);

        var res = new Vector2i(-5 * -7, 3 * -4);

        Assert.True((v1 * v2) == res);
    }

    [Fact]
    public void DivisionOperator_Integer_Positive()
    {
        var v1 = new Vector2i(5, 3);
        var v2 = 55;

        var res = new Vector2i(5 / 55, 3 / 55);

        Assert.True((v1 / v2) == res);
    }

    [Fact]
    public void DivisionOperator_Integer_Negative()
    {
        var v1 = new Vector2i(-5, 3);
        var v2 = -55;

        var res = new Vector2i(-5 / -55, 3 / -55);

        Assert.True((v1 / v2) == res);
    }
}
