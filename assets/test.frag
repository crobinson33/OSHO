uniform sampler2D texture;
void main()
{
    vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);

    gl_FragColor = vec4(pixel.r - 0.2, pixel.g + 0.5, pixel.b + 0.3, 0.5*pixel.a);
}