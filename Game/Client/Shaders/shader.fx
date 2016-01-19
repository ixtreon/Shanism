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
	float2 p = TextureCoordinate;		//convert from -1,1 to 0,1, reverse y
	p.x = (p.x + 1) / 2;
	p.y = (1 - p.y) / 2;

	float4 c = tex2D(TextureSampler, p);

	float2 cp = p - CENTER;
	float dist = length(cp * TexSize);
	if (dist > SightRange)
		return SHADOW;

	float stepSz = min(1.0 / TexSize.x, 1.0 / TexSize.y) * 2;
	float nSteps = length(cp) / stepSz;
	float2 step = cp / nSteps;

	float alpha = c.a;

	float2 curPt = cp;
	float4 nc;

	for (int i = 0; i < nSteps; i++)
	{
		curPt -= step;
		nc = tex2Dlod(TextureSampler, float4(curPt + CENTER, 0, 0));
		if (nc.a > 0.1)
			alpha = max(alpha, nc.a);
	}

	if (alpha > 0.1 && c.a < 0.1)
		return SHADOW;

	float ratio = (dist / SightRange);
	//ratio = ratio * ratio;

	return SHADOW * ratio + NONE * (1 - ratio);
}
//
//////////////////////////////////////////////////////////////////////////////////
//// Vertex Shader
//////////////////////////////////////////////////////////////////////////////////
//float4 ColorVertexShader(float4 position)
//{
//	float4 output;
//
//	float4 worldPosition = mul(position, World);
//	float4 viewPosition = mul(worldPosition, View);
//	output = mul(viewPosition, Projection);
//
//	return output;
//}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_5_0 PixelShaderFunction();
		//VertexShader = compile vs_4_0_level_9_3 ColorVertexShader();
	}
}