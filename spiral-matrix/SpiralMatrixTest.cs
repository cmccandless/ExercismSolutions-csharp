﻿// This file was auto-generated based on version 1.0.0 of the canonical data.

using Xunit;

public class SpiralMatrixTest
{
    [Fact]
    public void Empty_spiral()
    {
        var spiral = new int[,] { };
        Assert.Equal(spiral, SpiralMatrix.GetMatrix(0));
    }

    [Fact]
    public void Trivial_spiral()
    {
        var spiral = new int[,] 
        { 
            { 1 }
        };

        Assert.Equal(spiral, SpiralMatrix.GetMatrix(1));
    }

    [Fact]
    public void Spiral_of_size_2()
    {
        var spiral = new int[,] 
        { 
            { 1, 2 }, 
            { 4, 3 }
        };

        Assert.Equal(spiral, SpiralMatrix.GetMatrix(2));
    }

    [Fact]
    public void Spiral_of_size_3()
    {
        var spiral = new int[,] 
        { 
            { 1, 2, 3 }, 
            { 8, 9, 4 }, 
            { 7, 6, 5 }
        };

        Assert.Equal(spiral, SpiralMatrix.GetMatrix(3));
    }

    [Fact]
    public void Spiral_of_size_4()
    {
        var spiral = new int[,] 
        { 
            { 1, 2, 3, 4 }, 
            { 12, 13, 14, 5 }, 
            { 11, 16, 15, 6 }, 
            { 10, 9, 8, 7}
        };

        Assert.Equal(spiral, SpiralMatrix.GetMatrix(4));
    }

    [Fact]
    public void Spiral_of_size_5()
    {
        var spiral = new int[,] 
        { 
            { 1, 2, 3, 4, 5 }, 
            { 16, 17, 18, 19, 6 }, 
            { 15, 24, 25, 20, 7 }, 
            { 14, 23, 22, 21, 8 }, 
            { 13, 12, 11, 10, 9 }
        };

        Assert.Equal(spiral, SpiralMatrix.GetMatrix(5));
    }
}