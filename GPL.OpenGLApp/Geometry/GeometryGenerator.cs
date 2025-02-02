﻿using OpenTK.Mathematics;

namespace GPL.OpenGLApp.Geometry;

internal static class GeometryGenerator
{
    public static (float[] vertices, uint[] indices) GenerateSimpleSphere(int quality, float diameter = 1f)
    {
        var vertices = new List<float>();
        var indices = new List<uint>();

        var stacks = quality;
        var slices = quality * 2;

        for (int i = 0; i <= stacks; i++)
        {
            float phi = MathF.PI * i / stacks;
            for (int j = 0; j <= slices; j++)
            {
                float theta = MathF.Tau * j / slices;

                float x = MathF.Sin(phi) * MathF.Cos(theta);
                float y = MathF.Cos(phi);
                float z = MathF.Sin(phi) * MathF.Sin(theta);

                var radius = diameter / 2f;
                vertices.AddRange([x * radius, y * radius, z * radius]);
            }
        }

        for (int i = 0; i < stacks; i++)
        {
            for (int j = 0; j < slices; j++)
            {
                uint first = (uint)(i * (slices + 1) + j);
                uint second = (uint)(first + slices + 1);

                indices.Add(first);
                indices.Add(first + 1);
                indices.Add(second);

                indices.Add(second);
                indices.Add(first + 1);
                indices.Add(second + 1);
            }
        }

        return ([.. vertices], [.. indices]);
    }

    public static (float[] vertices, uint[] indices) GenerateSphere(int quality, float diameter = 1f)
    {
        var vertices = new List<float>();
        var indices = new List<uint>();

        var stacks = quality;
        var slices = quality * 2;

        for (int i = 0; i <= stacks; i++)
        {
            float phi = MathF.PI * i / stacks;
            for (int j = 0; j <= slices; j++)
            {
                float theta = MathF.Tau * j / slices;

                float x = MathF.Sin(phi) * MathF.Cos(theta);
                float y = MathF.Cos(phi);
                float z = MathF.Sin(phi) * MathF.Sin(theta);

                var norm = new Vector3(x, y, z).Normalized();
                float u = (float)j / slices;
                float v = (float)i / stacks;

                float r = 1f;
                float g = 1f;
                float b = 1f;

                var radius = diameter / 2f;
                vertices.AddRange([x * radius, y * radius, z * radius, r, g, b, u, v, norm.X, norm.Y, norm.Z]);
            }
        }

        for (int i = 0; i < stacks; i++)
        {
            for (int j = 0; j < slices; j++)
            {
                uint first = (uint)(i * (slices + 1) + j);
                uint second = (uint)(first + slices + 1);

                indices.Add(first);
                indices.Add(first + 1);
                indices.Add(second);

                indices.Add(second);
                indices.Add(first + 1);
                indices.Add(second + 1);
            }
        }

        return ([.. vertices], [.. indices]);
    }
}
