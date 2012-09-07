float4x4 View;
float4x4 Projection;
float3 Position;

texture Texture;
sampler2D TextureSampler = sampler_state {
	texture = <Texture>;
};

float2 Size;
float3 CameraUp; 
float3 CameraSide; 

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Color: COLOR0;
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color: COLOR0;
	float2 UV : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float3 position = Position;

	float2 offset = float2(
		(input.UV.x - 0.5f) * 2.0f,
		-(input.UV.y - 0.5f) * 2.0f
	);

	position += offset.x * Size.x * CameraSide + offset.y * Size.y * CameraUp;

	output.Position = mul(float4(position, 1), mul(View, Projection));

	output.UV = input.UV;

	output.Color = input.Color;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(TextureSampler, input.UV) * input.Color;
	clip(color.a - 0.5f);

	return color;
}


technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
