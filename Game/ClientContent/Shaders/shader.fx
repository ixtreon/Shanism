//------------------------------ TEXTURE PROPERTIES ----------------------------
// This is the texture that SpriteBatch will try to set before drawing
texture ScreenTexture;

// Our sampler for the texture, which is just going to be pretty simple
sampler TextureSampler = sampler_state
{
	Texture = <ScreenTexture>;

	AddressU = WRAP;
	AddressV = WRAP;
};

float2 TexSize = float2(100, 100);
float SightRange = 500;


static const float4 NONE = float4(0, 0, 0, 0);
static const float4 SHADOW = float4(0, 0, 0, 0.7);

static const float2 CENTER = float2(0.5, 0.5);


float2 getPointOffset(float2 pt, float ratio)
{
	return (pt - CENTER) * CENTER + CENTER;
}


//////////////////////////////////////////////////////////////////////////////////
//// Pixel Shader
//////////////////////////////////////////////////////////////////////////////////

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 tint : COLOR0, 
	float2 texCoord : TEXCOORD0) : SV_TARGET0
{

	float2 cp = texCoord - CENTER;
	float dist = length(cp * TexSize);
	if (dist > SightRange)
		return SHADOW;

	return NONE + (SHADOW - NONE) * dist / SightRange * dist / SightRange;
}


//////////////////////////////////////////////////////////////////////////////////
//// Vertex Shader
//////////////////////////////////////////////////////////////////////////////////

struct VertexShaderInput
{
	float4 Color: COLOR0;
	float2 TexCoord : TEXCOORD0;
	float4 Position : SV_Position0;
};

VertexShaderInput ColorVertexShader(VertexShaderInput i)
{
	VertexShaderInput o;

	o.Color = i.Color;
	o.TexCoord = i.TexCoord;
	o.Position = i.Position;

	return i;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
		//VertexShader = compile vs_4_0_level_9_3 ColorVertexShader();
	}
}