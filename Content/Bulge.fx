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

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 screenPos = input.TextureCoordinates - float2(0.5, 0.5);
	
    float bulgeStrength = 1.0 + (pow(length(screenPos), 2) * -1);
    //float bulgeStrength = 1.0 + (pow(length(screenPos), -1) * 0.125);
	
    float2 pos = screenPos * bulgeStrength + float2(0.5, 0.5);
	
	/// Let me be clear, LET IT CRUST
	/// FR FR
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