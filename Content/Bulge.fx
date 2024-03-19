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

float2 Center;
float BulgeExponent;
float BulgeStrength;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 screenPos = input.TextureCoordinates - Center;
	
    float bulgeStrength = 1.0 + (pow(length(screenPos), BulgeExponent) * BulgeStrength);
	
    float2 pos = screenPos * bulgeStrength + Center;
	
    if (pos.x < 0)
        return float4(0.0, 0.0, 0.0, 1.0);
    if (pos.x > 1)
        return float4(0.0, 0.0, 0.0, 1.0);
    if (pos.y < 0)
        return float4(0.0, 0.0, 0.0, 1.0);
    if (pos.y > 1)
        return float4(0.0, 0.0, 0.0, 1.0);
	
    return tex2D(SpriteTextureSampler, pos) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};