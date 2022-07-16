Shader "Show Lasers/Show Laser Shader"
{
	Properties
	{
		_Color("Color", Color) = (1,0,0,0.8)

		// Properties for the color change on either side
		_IsRectangular("Mesh Type", int) = 0 // 1=rect, 0=other
		_LeftOffset("Left-Hand Beam Offset", float) = 0
		_LeftColor("Left-Hand Beam Color", Color) = (1,1,1,1)
		_RightOffset("Right-Hand Beam Offset", float) = 0
		_RightColor("Right-Hand Beam Color", Color) = (1,1,1,1)

		// Properties for control of the distortion
		_MainTex("Texture (Custom)", 2D) = "black" {} // R = Albedo (main texture), GB = Flow Field, A= Change Offset
		_NoiseScale("Noise Scale", float) = 1
		_TimeScale("Time Scale", float) = 1
		_DistortionStrength("Distortion Strength", float) = 1

		// Adjustments
		_FadeOut("Fade (0=none)", float) = 1 // fade. Zero means none, otherwise pow factor
		_Gamma("Gamma adjustment", float) = 1
		_GeneralTransparency("General Transparency", float) = 1

		// Scaling (added automatically)
		_ForwardScale("Forward Scale", float) = 1
		_RightScale("Right Scale", float) = 1
	}

		SubShader
		{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"  }
			LOD 100

			ZWrite Off

			BlendOp Add
			Blend SrcAlpha One

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#pragma multi_compile_fog

				#pragma multi_compile __ LASER_TEXTURE

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float2 uv2 : TEXCOORD1;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float2 uv2 : TEXCOORD1;
					float4 vertex : SV_POSITION;
					float3 worldPos : TEXCOORD2;
					UNITY_FOG_COORDS(3) 
				};


				float4 _Color;

				int _IsRectangular;
				float _LeftOffset;
				float4 _LeftColor;
				float _RightOffset;
				float4 _RightColor;

				sampler2D _MainTex;
				float4 _MainTex_ST; 

				float _NoiseScale;
				float _TimeScale;
				float _DistortionStrength;

				float _FadeOut;
				float _Gamma;
				float _GeneralTransparency;

				float _ForwardScale;
				float _RightScale;

				#if defined(LASER_TEXTURE)

					float3 createDistortionParams(float2 uv, float2 flowVector, float tiling, float time, float phaseOffset) {
						float progress = frac(time + phaseOffset);
						float3 uvw;
						uvw.xy = uv - flowVector * progress;
						uvw.xy *= tiling;
						uvw.xy += phaseOffset;
						uvw.xy += (time - progress) * float2(0.125, 0.25); // use different values for U and V 
						uvw.z = 1 - abs(1 - 2 * progress);
						return uvw;
					}

					fixed4 getTexture(v2f i) {

						float2 simulatedUV = float2(i.uv.x * _RightScale, i.uv.y * _ForwardScale) * _NoiseScale;

						float2 flow = (tex2D(_MainTex, simulatedUV).gb * 2 - 1)  * _DistortionStrength / 8;

						// The noise used comes from the alpha channel 
						float noise = tex2D(_MainTex, simulatedUV).a *_NoiseScale;

						float time = _Time.y * _TimeScale / 3 + noise;

						float3 uvwA = createDistortionParams(simulatedUV, flow, _NoiseScale, time, 0);
						float3 uvwB = createDistortionParams(simulatedUV, flow, _NoiseScale, time, 0.5);

						return (tex2D(_MainTex, uvwA.xy) * uvwA.z + tex2D(_MainTex, uvwB.xy) * uvwB.z).r;
					}

				#else

					fixed4 getTexture(v2f i) {
						return .7; // Used instead of 1 to have bloom compatibility with texture. Can be offset by gamma value if required.
					}

				#endif


				fixed4 getColor(float2 p) {

					float2 a1 = float2(_RightOffset, 0);
					float2 b1 = float2(_RightOffset * _IsRectangular, 1);

					float2 a2 = float2(1 - _LeftOffset, 0);
					float2 b2 = float2((1 - _LeftOffset) * _IsRectangular, 1);

					// calculate line offset for current pixel
					float d1 = (p.x - a1.x)*(b1.y - a1.y) - (p.y - a1.y)*(b1.x - a1.x);
					float d2 = (p.x - a2.x)*(b2.y - a2.y) - (p.y - a2.y)*(b2.x - a2.x);

					return  lerp(_LeftColor, lerp(_Color, _RightColor, step(0, d2)), step(0, d1));
				}

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex); 
					o.uv = TRANSFORM_TEX(v.uv, _MainTex); 
					o.uv2 = TRANSFORM_TEX(v.uv2, _MainTex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					UNITY_TRANSFER_FOG(o, o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// get base color (left, center, right)
					float4 actualColor = getColor(i.uv2);

					// overlay chosen texture 
					actualColor *= getTexture(i);

					// Gamma (allows > 1 for values, which requires HDR mode, and is also required for Bloom)
					actualColor *= _Gamma;

					// Apply fade out towards the far side of the element
					actualColor.a = lerp(actualColor.a, step(_FadeOut, 0) * actualColor.a, pow(max(0, 1 - i.uv2.y), 1.0001 - _FadeOut));

					// Apply general fade for animations
					actualColor.a = lerp(0, actualColor.a, _GeneralTransparency);

					// apply fog
					UNITY_APPLY_FOG(i.fogCoord, actualColor);

					// saturate and return
					return float4(saturate(actualColor.rgb), actualColor.a);
				}
				
				ENDCG

			}
		}
}
