// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "J4F/Unlit/AlphaMask" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
    _AlphaTex ("Alpha mask (A)", 2D) = "white" {}
}

SubShader {
   Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	Pass {
		ZTest Always Cull Off ZWrite Off
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform half _RampOffset;

fixed4 frag (v2f_img i) : SV_Target
{
	fixed4 original = tex2D(_MainTex, i.uv);
	fixed4 alphalayer = tex2D(_AlphaTex, i.uv);
	
	if(alphalayer.a < 0.1){
		original.r = 0;
		original.g = 0;
		original.b = 0;
    	original.a = alphalayer.a;
    }
	return original;
}
ENDCG

	}
}

Fallback off


}