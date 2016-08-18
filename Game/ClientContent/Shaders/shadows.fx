//------------------------------ TEXTURE PROPERTIES ----------------------------
sampler s0;

float4x4 Projection;
float time;

struct VSInput
{
	float4 Color	: COLOR0;
	float2 TexCoord : TEXCOORD0;
	float4 Position : SV_Position0;
};

struct PSInput
{
	float4 Color    : COLOR0;
	float2 TexCoord : TEXCOORD0;
	float4 Position : POSITION;
};

//////////////////////////////////////////////////////////////////////////////////
//// Vertex Shader
//////////////////////////////////////////////////////////////////////////////////
PSInput VS(VSInput i)
{
	PSInput o;

	float4 pos = mul(i.Position, Projection);
	//pos.xy += i.TexCoord * 25;

	o.Color = i.Color;
	o.Position = pos;
	o.TexCoord = i.TexCoord;

	return o;
}

//////////////////////////////////////////////////////////////////////////////////
//// Pixel Shader
//////////////////////////////////////////////////////////////////////////////////
float4 PS(PSInput o) : SV_TARGET0
{
	float4 texC = tex2D(s0, o.TexCoord);

	float4 c = o.Color * texC;

	return float4(c.xyz, max(0.5, c.w));
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 VS();
		//GeometryShader = compile gs_4_0 GS();
		PixelShader = compile ps_4_0 PS();
	}
}