namespace GPL.OpenGLApp.Geometry;

public class Pyramid : BaseGeometryObject
{
    private static readonly float[] VERTICES =
	[ 
		//     COORDINATES     /        COLORS          /    TexCoord   /        NORMALS       
		-0.5f, 0.0f,  0.5f,     0.83f, 0.70f, 0.44f,     0.0f, 0.0f,      0.0f, -1.0f, 0.0f,   // Bottom side
		-0.5f, 0.0f, -0.5f,     0.83f, 0.70f, 0.44f,     0.0f, 5.0f,      0.0f, -1.0f, 0.0f,   // Bottom side
		 0.5f, 0.0f, -0.5f,     0.83f, 0.70f, 0.44f,     5.0f, 5.0f,      0.0f, -1.0f, 0.0f,   // Bottom side
		 0.5f, 0.0f,  0.5f,     0.83f, 0.70f, 0.44f,     5.0f, 0.0f,      0.0f, -1.0f, 0.0f,   // Bottom side

		-0.5f, 0.0f,  0.5f,     0.83f, 0.70f, 0.44f,     0.0f, 0.0f,     -0.8f, 0.5f,  0.0f,   // Left Side
		-0.5f, 0.0f, -0.5f,     0.83f, 0.70f, 0.44f,     5.0f, 0.0f,     -0.8f, 0.5f,  0.0f,   // Left Side
		 0.0f, 0.8f,  0.0f,     0.92f, 0.86f, 0.76f,     2.5f, 5.0f,     -0.8f, 0.5f,  0.0f,   // Left Side
																							   
		-0.5f, 0.0f, -0.5f,     0.83f, 0.70f, 0.44f,     5.0f, 0.0f,      0.0f, 0.5f, -0.8f,   // Non-facing side
		 0.5f, 0.0f, -0.5f,     0.83f, 0.70f, 0.44f,     0.0f, 0.0f,      0.0f, 0.5f, -0.8f,   // Non-facing side
		 0.0f, 0.8f,  0.0f,     0.92f, 0.86f, 0.76f,     2.5f, 5.0f,      0.0f, 0.5f, -0.8f,   // Non-facing side
																							   
		 0.5f, 0.0f, -0.5f,     0.83f, 0.70f, 0.44f,     0.0f, 0.0f,      0.8f, 0.5f,  0.0f,   // Right side
		 0.5f, 0.0f,  0.5f,     0.83f, 0.70f, 0.44f,     5.0f, 0.0f,      0.8f, 0.5f,  0.0f,   // Right side
		 0.0f, 0.8f,  0.0f,     0.92f, 0.86f, 0.76f,     2.5f, 5.0f,      0.8f, 0.5f,  0.0f,   // Right side
																							   
		 0.5f, 0.0f,  0.5f,     0.83f, 0.70f, 0.44f,     5.0f, 0.0f,      0.0f, 0.5f,  0.8f,   // Facing side
		-0.5f, 0.0f,  0.5f,     0.83f, 0.70f, 0.44f,     0.0f, 0.0f,      0.0f, 0.5f,  0.8f,   // Facing side
		 0.0f, 0.8f,  0.0f,     0.92f, 0.86f, 0.76f,     2.5f, 5.0f,      0.0f, 0.5f,  0.8f    // Facing side
	];

    private static readonly uint[]  INDICES =
    [
		0, 1, 2,
		0, 2, 3, 
		4, 6, 5, 
		7, 9, 8,
		10, 12, 11, 
		13, 15, 14 
	];

    public Pyramid() : base(VERTICES, INDICES) { }
}
