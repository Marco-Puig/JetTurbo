Shader "Bytesized/Toon"
{
	Properties
	{
	    /* Color used for the material. Leave white if none */
		_Color("Color", Color) = (1,1,1,1)
		/* Texture used for the material */
		_MainTex("Main Texture", 2D) = "white" {}
		/* Ambient color to add to the light calculation */
		[HDR] _AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		/* Color to apply on the specular lighting stage */
		[HDR] _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		/* The size of the specular reflection */
		_Glossiness("Glossiness", Float) = 32
		/* The color used in the rim lighting stage */
		[HDR] _RimColor("Rim Color", Color) = (1,1,1,1)
		/* How much should the material be affected by rim lighting */
		_RimBlend("Rim Blend", Range(0, 1)) = 0.5
		/* Controls how smoothly the rim blends with other unlit parts */
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
		/* Controls the color transition between shadowed surfaces and non shadowed surfaces */
		_Smoothness("Smoothness", Range(0, 0.5)) = 0.025
	}
	SubShader
	{
		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			
			/* We include the lighting code for the base pass */
			#include "ToonPass.cginc"
			
			ENDCG
		}
		
		Pass {
			Tags
			{
				"LightMode" = "ForwardAdd"
			}

            Blend One One
            ZWrite Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdadd_fullshadows

            /* We include the lighting code for the add pass */
			#include "ToonPass.cginc"

			ENDCG
		}

		/* Support for casting shadows */
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}