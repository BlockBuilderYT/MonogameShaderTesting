#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

float2 RedOffset;
float2 GreenOffset;
float2 BlueOffset;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 screenPos = (input.TextureCoordinates - float2(0.5, 0.5)) * 2.0;
	
    float distFromCenter = length(screenPos);
	
    float2 redPos = input.TextureCoordinates + RedOffset * distFromCenter;
    float2 greenPos = input.TextureCoordinates + GreenOffset * distFromCenter;
    float2 bluePos = input.TextureCoordinates + BlueOffset * distFromCenter;
	
    float r = tex2D(SpriteTextureSampler, redPos).r;
    float g = tex2D(SpriteTextureSampler, greenPos).g;
    float b = tex2D(SpriteTextureSampler, bluePos).b;
	
    return float4(r, g, b, 1.0) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};