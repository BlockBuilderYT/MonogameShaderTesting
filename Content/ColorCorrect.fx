#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

float4 RedMask =	float4(1.0, 0.0, 0.0, 0.0);
float4 GreenMask =	float4(0.0, 1.0, 0.0, 0.0);
float4 BlueMask =	float4(0.0, 0.0, 1.0, 0.0);
float4 AlphaMask =	float4(0.0, 0.0, 0.0, 1.0);

float4 Gray =		float4(0.5, 0.5, 0.5, 0.5);

float Saturation = 1.0;
float Contrast = 1.0;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 cleanColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	
    float4 convolvedColor = cleanColor.r * RedMask + cleanColor.g * GreenMask + cleanColor.b * BlueMask + cleanColor.a * AlphaMask;
	
    float4 grayscale = dot(convolvedColor, float4(0.2126, 0.7152, 0.0722, 0.0));
	
    return (convolvedColor * Saturation + grayscale * (1 - Saturation)) * Contrast + Gray * (1 - Contrast);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};