//=============================================================================
// 	[GLOBALS]
//=============================================================================

float4x4 World;
float4x4 Projection;
Texture2D DiffuseTexture;

//=============================================================================
// 	[FUNCTIONS]
//=============================================================================

float4 VertexShaderFunction(float4 input : POSITION0 ) : POSITION0
{
    float4 output = mul(input, World);    
    output = mul(output, Projection);

    return output;
}

float4 PixelShaderFunction(float4 input : POSITION0 ) : COLOR0
{
    return float4(1, 1, 1, 1);
}

//=============================================================================
//	[TECHNIQUES]
//=============================================================================

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader  = compile ps_2_0 PixelShaderFunction();
    }
}

sampler2D DiffuseSampler = sampler_state
{
    Texture = <DiffuseTexture>;
};

struct VertexPositionTexture
{
    float4 Position : POSITION0;
    float2 UV       : TEXCOORD0;
};

VertexPositionTexture TexturedVertexShader(VertexPositionTexture input)
{
    VertexPositionTexture output;
 
    output.Position = mul(input.Position, World);   
    output.Position = mul(output.Position, Projection);
    output.UV       = input.UV;
 
    return output;
}
 
float4 TexturedPixelShader(VertexPositionTexture input) : COLOR0
{
    return tex2D(DiffuseSampler, input.UV);
}
 
technique DefaultEffect
{
    Pass
    {
        VertexShader = compile vs_2_0 TexturedVertexShader();
        PixelShader  = compile ps_2_0 TexturedPixelShader();
    }
}
