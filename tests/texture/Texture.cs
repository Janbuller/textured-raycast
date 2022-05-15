using Xunit;
using textured_raycast.maze.texture;
using System.Collections.Generic;

namespace tests.texture;

public class Texture_Test
{
    [Fact]
    void Constructor_GetPixel()
    {
        var t1 = new Texture(new List<TexColor> {
		new TexColor(255, 255, 255), new TexColor(255, 0, 0),
		new TexColor(0, 255, 0), new TexColor(0, 0, 255) }, 2, 2);

        Assert.True(t1.getPixel(1, 0) == new TexColor(255, 0, 0));
    }

    [Fact]
    void DrawTexture_NoAlpha()
    {
        var t1 = new Texture(new List<TexColor> {
		new TexColor(255, 255, 255), new TexColor(255, 0, 0),
		new TexColor(0, 255, 0), new TexColor(0, 0, 255) }, 2, 2);

        var t2 = new Texture(new List<TexColor> {new TexColor(255, 255, 255)}, 1, 1);

        t1.DrawTexture(t2, 1, 0);

        Assert.True(t1.getPixel(1, 0) == new TexColor(255, 255, 255));
    }

    [Fact]
    void DrawTexture_WithAlpha()
    {
        var t1 = new Texture(new List<TexColor> {
		new TexColor(255, 255, 255), new TexColor(255, 0, 0),
		new TexColor(0, 255, 0), new TexColor(0, 0, 255) }, 2, 2);

        var t2 = new Texture(new List<TexColor> {new TexColor(255, 255, 255)}, 1, 1);

        t1.DrawTexture(t2, 1, 0, new TexColor(255, 255, 255));

        Assert.True(t1.getPixel(1, 0) == new TexColor(255, 0, 0));
    }
}
