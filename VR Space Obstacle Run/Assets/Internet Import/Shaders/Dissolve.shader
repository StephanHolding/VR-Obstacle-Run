
Shader "Custom/BurnShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Metallic("Metallic", 2D) = "black" {}
		_Glossiness("Metal Intensity", Range(0,1)) = 0.0
		_NormalMap("Normal", 2D) = "bump" {}

		_EmissionMap("Emission", 2D) = "black" {}
		_EmissionIntensity("Intensity", Range(0,5)) = 1.0
		_EmissionPulseSpeed("Pulse Speed", Range(0, 10)) = 1.0

		_DissolvePercentage("DissolvePercentage", Range(0,1)) = 0.0
		_BurnSize("Burn Size", Range(0.0, 1.0)) = 0.15
		_BurnRamp("Burn Ramp (RGB)", 2D) = "white" {}

		//Gradient Variables
		_Octaves("Octaves", Float) = 8.0
		_Frequency("Frequency", Float) = 1.0
		_Amplitude("Amplitude", Float) = 1.0
		_Lacunarity("Lacunarity", Float) = 1.92
		_Offset("Offset", Vector) = (0.0, 0.0, 0.0, 0.0)
	}

		CGINCLUDE
			//
			//	FAST32_hash
			//	A very fast hashing function.  Requires 32bit support.
			void FAST32_hash_3D(float3 gridcell, out float4 lowz_hash_0, out float4 lowz_hash_1, out float4 lowz_hash_2, out float4 highz_hash_0, out float4 highz_hash_1, out float4 highz_hash_2) //generates 3 random numbers for each of the 8 cell corners
		{
			//gridcell is assumed to be an integer coordinate
			const float2 OFFSET = float2(50.0, 161.0);
			const float DOMAIN = 69.0;
			const float3 SOMELARGEFLOATS = float3(635.298681, 682.357502, 668.926525);
			const float3 ZINC = float3(48.500388, 65.294118, 63.934599);

			//	truncate the domain
			gridcell.xyz = gridcell.xyz - floor(gridcell.xyz * (1.0 / DOMAIN)) * DOMAIN;
			float3 gridcell_inc1 = step(gridcell, float3(DOMAIN - 1.5, DOMAIN - 1.5, DOMAIN - 1.5)) * (gridcell + 1.0);

			//	calculate the noise
			float4 P = float4(gridcell.xy, gridcell_inc1.xy) + OFFSET.xyxy;
			P *= P;
			P = P.xzxz * P.yyww;
			float3 lowz_mod = float3(1.0 / (SOMELARGEFLOATS.xyz + gridcell.zzz * ZINC.xyz));
			float3 highz_mod = float3(1.0 / (SOMELARGEFLOATS.xyz + gridcell_inc1.zzz * ZINC.xyz));
			lowz_hash_0 = frac(P * lowz_mod.xxxx);
			highz_hash_0 = frac(P * highz_mod.xxxx);
			lowz_hash_1 = frac(P * lowz_mod.yyyy);
			highz_hash_1 = frac(P * highz_mod.yyyy);
			lowz_hash_2 = frac(P * lowz_mod.zzzz);
			highz_hash_2 = frac(P * highz_mod.zzzz);
		}

		//
		//	Interpolation functions
		//	( smoothly increase from 0.0 to 1.0 as x increases linearly from 0.0 to 1.0 )
		float3 Interpolation_C2(float3 x)
		{
			return x * x * x * (x * (x * 6.0 - 15.0) + 10.0);
		}

		//
		//	Perlin Noise 3D  ( gradient noise )
		//	Return value range of -1.0->1.0
		float Perlin3D(float3 P)
		{
			//	establish our grid cell and unit position
			float3 Pi = floor(P);
			float3 Pf = P - Pi;
			float3 Pf_min1 = Pf - 1.0;

			//
			//	classic noise.
			//	requires 3 random values per point.  with an efficent hash function will run faster than improved noise
			//

			//	calculate the hash.
			//	( various hashing methods listed in order of speed )
			float4 hashx0, hashy0, hashz0, hashx1, hashy1, hashz1;
			FAST32_hash_3D(Pi, hashx0, hashy0, hashz0, hashx1, hashy1, hashz1);

			//	calculate the gradients
			float4 grad_x0 = hashx0 - 0.49999;
			float4 grad_y0 = hashy0 - 0.49999;
			float4 grad_z0 = hashz0 - 0.49999;
			float4 grad_x1 = hashx1 - 0.49999;
			float4 grad_y1 = hashy1 - 0.49999;
			float4 grad_z1 = hashz1 - 0.49999;
			float4 grad_results_0 = rsqrt(grad_x0 * grad_x0 + grad_y0 * grad_y0 + grad_z0 * grad_z0) * (float2(Pf.x, Pf_min1.x).xyxy * grad_x0 + float2(Pf.y, Pf_min1.y).xxyy * grad_y0 + Pf.zzzz * grad_z0);
			float4 grad_results_1 = rsqrt(grad_x1 * grad_x1 + grad_y1 * grad_y1 + grad_z1 * grad_z1) * (float2(Pf.x, Pf_min1.x).xyxy * grad_x1 + float2(Pf.y, Pf_min1.y).xxyy * grad_y1 + Pf_min1.zzzz * grad_z1);

			//	Classic Perlin Interpolation
			float3 blend = Interpolation_C2(Pf);
			float4 res0 = lerp(grad_results_0, grad_results_1, blend.z);
			float2 res1 = lerp(res0.xy, res0.zw, blend.y);
			float final = lerp(res1.x, res1.y, blend.x);
			//final *= 1.1547005383792515290182975610039;		//	(optionally) scale things to a strict -1.0->1.0 range    *= 1.0/sqrt(0.75)
			return final;
		}

		float PerlinNormal(float3 p, int octaves, float3 offset, float frequency, float amplitude, float lacunarity, float persistence)
		{
			float sum = 0;
			for (int i = 0; i < octaves; i++)
			{
				float h = 0;
				h = Perlin3D((p + offset) * frequency);
				sum += h * amplitude;
				frequency *= lacunarity;
				amplitude *= persistence;
			}
			return sum;
		}

		ENDCG


			SubShader
		{
			Cull Off

			Tags{ "Queue" = "Transparent" "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM

			#pragma surface surf Standard fullforwardshadows

			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _Metallic;
			sampler2D _NormalMap;

			sampler2D _EmissionMap;
			float _EmissionIntensity;
			float _EmissionPulseSpeed;

			sampler2D _DissolveTex;
			fixed4 _Color;
			float _Scale;
			float _DissolvePercentage;

			sampler2D _BurnRamp;
			float _BurnSize;
			half _Glossiness;

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_NormalMap;
				float3 worldPos;
				float3 worldNormal; INTERNAL_DATA
			};

			fixed _Octaves;
			float _Frequency;
			float _Amplitude;
			float3 _Offset;
			float _Lacunarity;
			float _Persistence = 0.1;

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				float3 correctWorldNormal = WorldNormalVector(IN, float3(0, 0, 1));
				float2 uv = IN.worldPos.xz;

				if (abs(correctWorldNormal.x) > 0.5) uv = IN.worldPos.yz;
				if (abs(correctWorldNormal.z) > 0.5) uv = IN.worldPos.xy;

				float gradient = PerlinNormal(IN.worldPos, _Octaves, _Offset, _Frequency, _Amplitude, _Lacunarity, _Persistence);

				gradient += 1;
				_DissolvePercentage = _DissolvePercentage * 2;

				clip(gradient - _DissolvePercentage);

				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb * _Color;

				fixed4 m = tex2D(_Metallic, IN.uv_MainTex);
				o.Metallic = m.rgb;

				fixed4 g = tex2D(_Metallic, IN.uv_MainTex);
				o.Smoothness = g.rgb * _Glossiness;

				fixed3 n = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
				o.Normal = n;

				fixed4 e = tex2D(_EmissionMap, IN.uv_MainTex);
				o.Emission = e.rgb * clamp(sin(_EmissionIntensity + (_Time * _EmissionPulseSpeed)), 0, 5);

				half test = gradient - _DissolvePercentage;
				if (test < _BurnSize && _DissolvePercentage > 0 && _DissolvePercentage < 2) {
					o.Emission = tex2D(_BurnRamp, float2(test *(1 / _BurnSize), 0));
					o.Albedo *= o.Emission;
				}

				o.Alpha = c.a * (gradient - _DissolvePercentage);
			}

			ENDCG
		}
			FallBack "Diffuse"
}