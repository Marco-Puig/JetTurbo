#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

struct appdata
{
    float4 vertex : POSITION;				
    float4 uv : TEXCOORD0;
    float3 normal : NORMAL;
};

struct v2f
{
    float4 pos : SV_POSITION;
    float3 worldNormal : NORMAL;
    float2 uv : TEXCOORD0;
    float3 viewDir : TEXCOORD1;
    float3 worldPos : POSITION1;
    /* Creates an extra field to the struct for use in the shadowing process */
    SHADOW_COORDS(2)
};

sampler2D _MainTex;
float4 _MainTex_ST;

v2f vert (appdata v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.worldNormal = UnityObjectToWorldNormal(v.normal);
    o.worldPos = mul(unity_ObjectToWorld, v.vertex);
    o.viewDir = WorldSpaceViewDir(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    /* Assignes the shadow coordinate to the fragment */
    TRANSFER_SHADOW(o)
    return o;
}

/* Properties */
float4 _Color;
float4 _AmbientColor;
float4 _SpecularColor;
float _Glossiness;
float _Smoothness;
float4 _RimColor;
float _RimBlend;
float _RimThreshold;	

float4 frag (v2f i) : SV_Target
{
    float3 normal = normalize(i.worldNormal);
    float3 viewDir = normalize(i.viewDir);

    float3 lightDir = _WorldSpaceLightPos0;
    #if defined(POINT) || defined(SPOT)
	    lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
    #endif   
    
    UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos);

    /* Calculate illumination from directional light. */
    float NdotL = dot(_WorldSpaceLightPos0, normal);

    /* Sample the shadow map and get a value in range [0, 1] where 0 is in the shadow, and 1 is not. */
    float shadowValue = attenuation;//SHADOW_ATTENUATION(i);
    /* Clamp the intensity into unlit and lit and interpolate them using the _Smoothness value */
    float lightIntensity = smoothstep(0, _Smoothness, NdotL * shadowValue) * 0.75;	
    /* Multiply by the main directional light's intensity and color. */
    float4 light = lightIntensity * _LightColor0;

    /* Calculate specular lighting */
    float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
    float NdotH = dot(normal, halfVector);
    float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
    float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
    float4 specular = specularIntensitySmooth * _SpecularColor;				

    /* Calculate rim lighting */
    float rimDot = 1 - dot(viewDir, normal);
    /* Rim should only appear on the lit parts of the surface so we multiply it by NdotL, raised to a power to smoothly blend it. */
    float rawRimIntensity = rimDot * pow(NdotL, _RimThreshold);
    float rimIntensity = smoothstep(_RimBlend - 0.01, _RimBlend + 0.01, rawRimIntensity);
    float4 rim = rimIntensity * _RimColor;

    /* Sample the main texture */
    float4 sample = tex2D(_MainTex, i.uv);
    /* Combine everything */
    return (light + rim + specular + _AmbientColor) * attenuation * _Color * sample;
}