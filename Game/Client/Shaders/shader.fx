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

float2 TexSize;
float SightRange;


const float4 NONE = float4(0, 0, 0, 0);
const float4 SHADOW = float4(0, 0, 0, 0.5);

const float2 CENTER = float2(0.5, 0.5);


float2 getPointOffset(float2 pt, float ratio)
{
	return (pt - CENTER) * CENTER + CENTER;
}

//------------------------ PIXEL SHADER ----------------------------------------
// This pixel shader will simply look up the color of the texture at the
// requested point
float4 PixelShaderFunction(float2 TextureCoordinate : TEXCOORD0) : COLOR0
{
	return float4(1, 0, 0, 0);

	//float2 p = TextureCoordinate;		//convert from -1,1 to 0,1, reverse y
	//p.x = (p.x + 1) / 2;
	//p.y = (1 - p.y) / 2;

	//float4 c = tex2D(TextureSampler, p);

	//float2 cp = p - CENTER;
	//float dist = length(cp * TexSize);
	//if (dist > SightRange)
	//	return SHADOW;

	//return NONE + (SHADOW - NONE) * dist / SightRange * dist / SightRange;
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

	return o;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_3 PixelShaderFunction();
		VertexShader = compile vs_4_0_level_9_3 ColorVertexShader();
	}
}