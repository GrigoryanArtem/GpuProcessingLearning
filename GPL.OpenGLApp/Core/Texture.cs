using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace GPL.OpenGLApp.Core;
internal class Texture
{
    private TextureUnit _unit;

    public Texture()
    {
        byte[] data = [255, 255, 255, 255];
        Handle = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, Handle);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 1, 1, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    private Texture(string path)
    {
        var image = ImageResult.FromStream(File.OpenRead(path),
            ColorComponents.RedGreenBlueAlpha);

        Handle = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, Handle);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.NearestMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    public int Handle { get; }

    public void Bind(TextureUnit unit)
    {
        _unit = unit;

        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }

    public void Unbind()
    {
        GL.ActiveTexture(_unit);
        GL.BindTexture(TextureTarget.Texture2D, 0);
    }

    public static Texture Load(string path)
        => new (path);
}
