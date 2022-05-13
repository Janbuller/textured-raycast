using Xunit;
using textured_raycast.maze.math;

namespace tests.math;

public class Vector2d_Test
{
    [Fact]
    public void EqualityOperator_IsEqual()
    {
        var v1 = new Vector2d(2.47, 5);
        var v2 = new Vector2d(2.47, 5);

        Assert.True(v1 == v2);
    }

    [Fact]
    public void EqualityOperator_IsUnequal_LHS()
    {
        var v1 = new Vector2d(2.22, 5);
        var v2 = new Vector2d(1.22, 5);

        Assert.False(v1 == v2);
    }

    [Fact]
    public void EqualityOperator_IsUnequal_RHS()
    {
        var v1 = new Vector2d(2, 5.63);
        var v2 = new Vector2d(2, 6.63);

        Assert.False(v1 == v2);
    }

    [Fact]
    public void InequalityOperator_IsEqual()
    {
        var v1 = new Vector2d(2.47, 5);
        var v2 = new Vector2d(2.47, 5);

        Assert.False(v1 != v2);
    }

    [Fact]
    public void InequalityOperator_IsUnequal_LHS()
    {
        var v1 = new Vector2d(2.22, 5);
        var v2 = new Vector2d(1.22, 5);

        Assert.True(v1 != v2);
    }

    [Fact]
    public void InequalityOperator_IsUnequal_RHS()
    {
        var v1 = new Vector2d(2, 5.63);
        var v2 = new Vector2d(2, 6.63);

        Assert.True(v1 != v2);
    }

    [Fact]
    public void AdditionOperator_Integer_Positive()
    {
        var v1 = new Vector2d(5, 3);
        var v2 = new Vector2d(7, 4);

        var res = new Vector2d(5 + 7, 3 + 4);

        Assert.True((v1 + v2) == res);
    }

    [Fact]
    public void AdditionOperator_Float_Positive()
    {
        var v1 = new Vector2d(9.234, 1.232);
        var v2 = new Vector2d(8.426, 9.918);

        var res = new Vector2d(9.234 + 8.426, 1.232 + 9.918);

        Assert.True((v1 + v2) == res);
    }

    [Fact]
    public void AdditionOperator_Integer_Negative()
    {
        var v1 = new Vector2d(-5, 3);
        var v2 = new Vector2d(-7, -4);

        var res = new Vector2d(-5 + -7, 3 + -4);

        Assert.True((v1 + v2) == res);
    }

    [Fact]
    public void AdditionOperator_Float_Negative()
    {
        var v1 = new Vector2d(-9.234, -1.232);
        var v2 = new Vector2d(8.426, -9.918);

        var res = new Vector2d(-9.234 + 8.426, -1.232 + -9.918);

        Assert.True((v1 + v2) == res);
    }

    [Fact]
    public void SubtractionOperator_Integer_Positive()
    {
        var v1 = new Vector2d(5, 3);
        var v2 = new Vector2d(7, 4);

        var res = new Vector2d(5 - 7, 3 - 4);

        Assert.True((v1 - v2) == res);
    }

    [Fact]
    public void SubtractionOperator_Float_Positive()
    {
        var v1 = new Vector2d(9.234, 1.232);
        var v2 = new Vector2d(8.426, 9.918);

        var res = new Vector2d(9.234 - 8.426, 1.232 - 9.918);

        Assert.True((v1 - v2) == res);
    }

    [Fact]
    public void SubtractionOperator_Integer_Negative()
    {
        var v1 = new Vector2d(-5, 3);
        var v2 = new Vector2d(-7, -4);

        var res = new Vector2d(-5 - -7, 3 - -4);

        Assert.True((v1 - v2) == res);
    }

    [Fact]
    public void SubtractionOperator_Float_Negative()
    {
        var v1 = new Vector2d(-9.234, -1.232);
        var v2 = new Vector2d(8.426, -9.918);

        var res = new Vector2d(-9.234 - 8.426, -1.232 - -9.918);

        Assert.True((v1 - v2) == res);
    }

    [Fact]
    public void MultiplicationOperator_Integer_Positive()
    {
        var v1 = new Vector2d(5, 3);
        var v2 = new Vector2d(7, 4);

        var res = new Vector2d(5 * 7, 3 * 4);

        Assert.True((v1 * v2) == res);
    }

    [Fact]
    public void MultiplicationOperator_Float_Positive()
    {
        var v1 = new Vector2d(9.234, 1.232);
        var v2 = new Vector2d(8.426, 9.918);

        var res = new Vector2d(9.234 * 8.426, 1.232 * 9.918);

        Assert.True((v1 * v2) == res);
    }

    [Fact]
    public void MultiplicationOperator_Integer_Negative()
    {
        var v1 = new Vector2d(-5, 3);
        var v2 = new Vector2d(-7, -4);

        var res = new Vector2d(-5 * -7, 3 * -4);

        Assert.True((v1 * v2) == res);
    }

    [Fact]
    public void MultiplicationOperator_Float_Negative()
    {
        var v1 = new Vector2d(-9.234, -1.232);
        var v2 = new Vector2d(8.426, -9.918);

        var res = new Vector2d(-9.234 * 8.426, -1.232 * -9.918);

        Assert.True((v1 * v2) == res);
    }

    [Fact]
    public void DivisionOperator_Integer_Positive()
    {
        var v1 = new Vector2d(5, 3);
        var v2 = 55;

        var res = new Vector2d(5 / 55.0, 3 / 55.0);

        Assert.True((v1 / v2) == res);
    }

    [Fact]
    public void DivisionOperator_Float_Positive()
    {
        var v1 = new Vector2d(9.234, 1.232);
        var v2 = 9.37;

        var res = new Vector2d(9.234 / 9.37, 1.232 / 9.37);

        Assert.True((v1 / v2) == res);
    }

    [Fact]
    public void DivisionOperator_Integer_Negative()
    {
        var v1 = new Vector2d(-5, 3);
        var v2 = -55;

        var res = new Vector2d(-5 / -55.0, 3 / -55.0);

        Assert.True((v1 / v2) == res);
    }

    [Fact]
    public void DivisionOperator_Float_Negative()
    {
        var v1 = new Vector2d(-9.234, -1.232);
        var v2 = -9.37;

        var res = new Vector2d(-9.234 / -9.37, -1.232 / -9.37);

        Assert.True((v1 / v2) == res);
    }

    [Fact]
    public void Flooring_Positive()
    {
        var v1 = new Vector2d(5.23523, 24352.223523);

        var res = new Vector2d(5, 24352);

        Assert.True(v1.Floor() == res);
    }

    [Fact]
    public void Flooring_Negative()
    {
        var v1 = new Vector2d(-5.23523, -24352.223523);

        var res = new Vector2d(-6, -24353);

        Assert.True(v1.Floor() == res);
    }

    [Fact]
    public void DistTo_Positive() {
        var v1 = new Vector2d(5.3, 3.7);
        var v2 = new Vector2d(8.6, 8.9);

        var res = 6.158733636065128;

        Assert.True(v1.DistTo(v2) == res);
    }

    [Fact]
    public void DistTo_Negative() {
        var v1 = new Vector2d(-5.3, -3.7);
        var v2 = new Vector2d(-8.6, -8.9);

        var res = 6.158733636065128;

        Assert.True(v1.DistTo(v2) == res);
    }

    [Fact]
    public void Normalize_Positive() {
        var v1 = new Vector2d(1, 1);
        v1.Normalize();

        var res = new Vector2d(0.7071067811865475, 0.7071067811865475);

        Assert.True(v1 == res);
    }
}
