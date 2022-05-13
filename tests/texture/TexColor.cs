using Xunit;
using textured_raycast.maze.texture;

namespace tests.texture;

public class TexColor_Test
{
    [Fact]
    void Constructor_ClampedProperties()
    {
        TexColor WithinRange = new TexColor(127, 98, 12);
        bool Assertion1 = WithinRange.R == 127 &&
            WithinRange.G == 98 &&
            WithinRange.B == 12;

        TexColor OutsideRange = new TexColor(12874, 123, 477);
        bool Assertion2 = OutsideRange.R == 255 &&
            OutsideRange.G == 123 &&
            OutsideRange.B == 255;

        Assert.True(Assertion1 && Assertion2);
    }

    [Fact]
    void Constructor_RealProperties()
    {
        TexColor WithinRange = new TexColor(127, 98, 12);
        bool Assertion1 = WithinRange.realR == 127 &&
            WithinRange.realG == 98 &&
            WithinRange.realB == 12;

        TexColor OutsideRange = new TexColor(12874, 123, 477);
        bool Assertion2 = OutsideRange.realR == 12874 &&
            OutsideRange.realG == 123 &&
            OutsideRange.realB == 477;

        Assert.True(Assertion1 && Assertion2);
    }

    [Fact]
    void Constructor_Clamped()
    {
        TexColor Clamped = new TexColor(2550, 255, 0, 2550);
        Assert.True(Clamped.R == 255 && Clamped.G == 25 && Clamped.B == 0);
    }

    [Fact]
    void EqualityOperator()
    {
        var tc1 = new TexColor(13, 34, 127);
        var tc2 = new TexColor(13, 34, 127);

        bool Assertion = tc1 == tc2;

        tc1 = new TexColor(235, 12421, 23874);
        tc2 = new TexColor(235, 12421, 23874);

        Assertion = Assertion && (tc1 == tc2);

        tc1 = new TexColor(235, 12421, 23874);
        tc2 = new TexColor(23, 8277, 23874);

        Assertion = Assertion && !(tc1 == tc2);

        Assert.True(Assertion);
    }

    [Fact]
    void InequalityOperator()
    {
        var tc1 = new TexColor(13, 34, 127);
        var tc2 = new TexColor(13, 34, 127);

        bool Assertion = !(tc1 != tc2);

        tc1 = new TexColor(235, 12421, 23874);
        tc2 = new TexColor(235, 12421, 23874);

        Assertion = Assertion && !(tc1 != tc2);

        tc1 = new TexColor(235, 12421, 23874);
        tc2 = new TexColor(23, 8277, 23874);

        Assertion = Assertion && (tc1 != tc2);

        Assert.True(Assertion);
    }

    [Fact]
    void HSVColorSetter()
    {
        var tc1 = new TexColor();
        tc1.setColorHSV(0, 1.0f, 1.0f);

        var res = new TexColor(255, 0, 0);
        bool Assertion = tc1 == res;

        tc1.setColorHSV(140, 1.0f, 1.0f);
        res = new TexColor(0, 255, 85);
        Assertion = Assertion && (tc1 == res);

        tc1.setColorHSV(140, 0.5f, 1.0f);
        res = new TexColor(128, 255, 170);
        Assertion = Assertion && (tc1 == res);

        tc1.setColorHSV(140, 1.0f, 0.5f);
        res = new TexColor(0, 128, 42);
        Assertion = Assertion && (tc1 == res);

        Assert.True(Assertion);
    }

    [Fact]
    void BrightnessGetter()
    {
        var tc1 = new TexColor();
        tc1.setColorHSV(140, 1.0f, 0.5f);

        var tc1_brightness = TexColor.getBrightness(tc1);
        var res = 94;

	Assert.True(tc1_brightness == res);
    }

    [Fact]
    void MultiplicationOperator() {
        var tc1 = new TexColor(24, 23, 110);
        tc1 *= 2;

        var res = new TexColor(48, 46, 220);

        Assert.True(tc1 == res);
    }

    [Fact]
    void AdditionOperator() {
        var tc1 = new TexColor(234, 172, 255);
        var tc2 = new TexColor(55, 22, 12);

        var res = new TexColor(289, 194, 267);

        Assert.True(tc1 + tc2 == res);
    }
}
