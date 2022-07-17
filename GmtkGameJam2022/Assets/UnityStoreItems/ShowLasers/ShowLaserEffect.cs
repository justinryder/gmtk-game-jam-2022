using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace sourcenity {

    [ExecuteInEditMode]
    public class ShowLaserEffect : MonoBehaviour {

        /** Enumerations **/

        public enum ScLaserEffectType {
            Plane,
            Fan,
            Cone
        };

        public enum ScLaserEffectVisual {
            Plain,
            Textured,
            CustomMaterial
        };

        public enum ScLaserEffectVisualTextured {
            Clouds,
            Smoke,
            Custom
        };

        public enum ScLaserEffectAlignment {
            Left,
            Center
        };

        [Header("Lifecycle")]
        [Tooltip("Effect will only render if set to true.")]
        public bool show = true;

        [Header("Main")]
        [Tooltip("Main type of effect which determines overall visual appearance.")]
        public ScLaserEffectType type = ScLaserEffectType.Plane;

        [Tooltip(
            "Determines where the center of the element is placed, i.e. in which direction it grows when changing width.")]
        public ScLaserEffectAlignment alignment = ScLaserEffectAlignment.Left;

        [Tooltip("Main size parameter. Width is in units if type is 'plane', and in degrees in all other cases.")]
        public float width = 1f;

        [Tooltip("The length (along the Z axis) of the entire laser.")]
        public float length = 10f;

        [DependsOn("type", ScLaserEffectType.Cone)]
        [Tooltip("For cones only: Controls the radius of the target circle")]
        public float radius = 0.5f;

        [Tooltip("Main (center) color. May include transparency.")]
        public Color mainColor = Color.red;

        [Range(-1.0f, 1.0f)]
        [Tooltip(
            "Fill allows building up each individual element in the laser effect from 0 to 1 at the same time. Negative values fill from the other side.")]
        public float fill = 1f;

        [Header("Multiple Segments")]
        [Tooltip("Number of individual segments to show. Each segment has its own border.")]
        [FormerlySerializedAs("numberOfElements")]
        public int numberOfSegments = 1;

        [DependsOn("numberOfSegments", 1, DependsOnAttribute.ComparisonType.GreaterThan)]
        [Tooltip(
            "If there are multiple elements, specifies the gap between them. The gap is in units for the 'plane' type, and in degrees for all other types.")]
        [FormerlySerializedAs("gapBetweenElements")]
        public float gapBetweenSegments = 0;

        [Header("Border Configuration")]
        [Range(0f, 1.0f)]
        [Tooltip("Width (units for planes, degrees for other types) of the left border of the effect.")]
        public float leftSideOffset = 0;

        [DependsOn("leftSideOffset", 0f, DependsOnAttribute.ComparisonType.NonEquality)]
        [Tooltip("Color of the left border of the effect.")]
        public Color leftSideColor = Color.white;

        [Range(0f, 1.0f)]
        [Tooltip("Width (units for planes, degrees for other types) of the right border of the effect.")]
        public float rightSideOffset = 0;

        [DependsOn("rightSideOffset", 0f, DependsOnAttribute.ComparisonType.NonEquality)]
        [Tooltip("Color of the right border of the effect.")]
        public Color rightSideColor = Color.white;

        [Header("Visualization")]
        [Tooltip("How the inner part of the laser beam is shown graphically.")]
        public ScLaserEffectVisual visualization = ScLaserEffectVisual.Plain;

        [DependsOn("visualization", ScLaserEffectVisual.Textured)]
        [Tooltip("Texture to display.")]
        public ScLaserEffectVisualTextured textureType = ScLaserEffectVisualTextured.Clouds;

        [DependsOn("textureType", ScLaserEffectVisualTextured.Custom)]
        [Tooltip(
            "A custom texture to be used by the Show Lasers shader. Have a look at the documentation for instructions on how to prepare the texture.")]
        public Texture2D customTexture;

        [DependsOn("visualization", ScLaserEffectVisual.Textured)]
        [Tooltip("Determines repetition of the underlying noise. Higher values mean more repetition.")]
        public float noiseScaling = 1f;

        [DependsOn("visualization", ScLaserEffectVisual.Textured)]
        [Tooltip("Determines how fast the texture changes, i.e. how fast it is animated.")]
        public float timeScaling = 1f;

        [DependsOn("visualization", ScLaserEffectVisual.Textured)]
        [Tooltip("Determines how distorted the texture will be. A value of 0 uses the original texture.")]
        public float distortionStrength = 1f;

        [Header("Adjustments")]
        [Tooltip(
            "Controls whether the effect fades out towards its Z end (along the 'length' axis). 0 means no fade-out; 1 means total fadeout")]
        [Range(0.0f, 1.0f)]
        public float fadeOutValue = 0f;

        [Tooltip(
            "Gamma slider, that is for increasing or decreasing lightness. mostly to be used in combination with Bloom.")]
        public float gammaValue = 1f;

        [Tooltip(
            "Overall transparency in addition to color transparency. Useful for fading the entire effect in and out.")]
        public float generalTransparency = 1f;


        /** Private fields **/

        private readonly int MAX_FACES = 64;

        private float prevWidth;
        private float prevGap;
        private int prevNumberOfElements;

        private bool prevShow;
        private Shadow shadow = null;

        private Mesh cachedMesh = null;
        private Material cachedMaterial = null;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private Texture2D cloudsTexture;
        private Texture2D smokeTexture;

        /** Lifecycle methods **/

        /// <summary>
        /// Initialize the script. 
        /// </summary>
        private void Start() {

            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null) {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }

            meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null) {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            meshRenderer.receiveShadows = false;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

            cloudsTexture = Resources.Load("laser_tex_clouds") as Texture2D;
            smokeTexture = Resources.Load("laser_tex_smoke") as Texture2D;

            RefreshMesh();
            RefreshMaterial();

            prevShow = show;
            ApplyMeshAndMaterialToGameObject();
            shadow = CloneValues();
        }


        /// <summary>
        /// This method ensures that there are no illegal value combinations when changing parameters using Unity's editor.
        /// </summary>
        private void OnValidate() {

            // Gap must always be positive.
            if (gapBetweenSegments< 0) {
                gapBetweenSegments = 0;
            }

            // Ensure that width in combination with elements and gaps is not too high (>360)
            float fullWidth = GetFullWidth();
            if (!type.Equals(ScLaserEffectType.Plane) && (fullWidth < -360 || fullWidth > 360)) {
                float direction = fullWidth < 0 ? -1 : 1;
                if (prevWidth != width) {
                    width = (360 * direction - (numberOfSegments - 1) * gapBetweenSegments*direction) / numberOfSegments;
                } else if (prevGap != gapBetweenSegments) {
                    gapBetweenSegments = (360 - (numberOfSegments * width * direction)) / (numberOfSegments - 1);
                } else if (prevNumberOfElements != numberOfSegments) {
                    numberOfSegments = Math.Max(1,
                        (int) ((360 + gapBetweenSegments) /
                               (width * direction + gapBetweenSegments)));
                }
            }

            // After width has been calculated, number of elements might be below 1, which is not allowed.
            if (numberOfSegments < 1) {
                numberOfSegments = 1;
            }

            // Prevent fade out to become negative, which may lead to NaNs in the shader
            if (fadeOutValue < 0) {
                fadeOutValue = 0;
            }

            prevWidth = width;
            prevGap = gapBetweenSegments;
            prevNumberOfElements = numberOfSegments;

            // prevent custom, thus showing of the custom texture field.
            if (!visualization.Equals(ScLaserEffectVisual.Textured)) {
                textureType = ScLaserEffectVisualTextured.Clouds;
            }
        }

        /// <summary>
        /// Run once per frame; determines whether updates to the mesh or the material are required,
        /// and whether to actually show the mesh (otherwise, we disable it entirely).
        /// </summary>
        private void Update() {

            bool hasChanged = false;

            if (IsMeshBasedValuesChanged()) {
                RefreshMesh();
                hasChanged = true;
            }

            if (IsMaterialRelevantValuesChanged()) {
                RefreshMaterial();
                hasChanged = true;
            }

            if (hasChanged) {
                shadow = CloneValues();
            }

            if (hasChanged || (show && !prevShow) || (!show && prevShow)) {
                ApplyMeshAndMaterialToGameObject();
                prevShow = show;
            }
        }

        /** Application Methods **/

        private void RefreshMesh() {

            // Generate one submesh for each element
            CombineInstance[] instances = new CombineInstance[numberOfSegments];

            // Start and end are width for rects, and angles for arches and cones
            float fullWidth = GetFullWidth();
            float start = GetStartForEffect();
            float direction = width < 0 ? -1 : 1;
            if (alignment == ScLaserEffectAlignment.Center) {
                start -= fullWidth / 2;
            }

            for (int i = 0; i < numberOfSegments; i++) {

                CombineInstance inst = new CombineInstance();
                inst.subMeshIndex = 0;
                inst.mesh = GenerateElement(start);
                instances[i] = inst;

                start += width + (gapBetweenSegments * direction);
            }

            if (cachedMesh == null) {
                cachedMesh = new Mesh();
            }

            cachedMesh.Clear();
            cachedMesh.name = "Show Laser Effect";
            cachedMesh.CombineMeshes(instances, true, false);
        }

        private void RefreshMaterial() {

            if (!visualization.Equals(ScLaserEffectVisual.CustomMaterial)) {
                if (cachedMaterial == null) {
                    cachedMaterial = new Material(Shader.Find("Show Lasers/Show Laser Shader"));
                }

                Texture2D textureToUse = null;

                // Add new keyword, if required.
                if (visualization.Equals(ScLaserEffectVisual.Plain)) {
                    cachedMaterial.DisableKeyword("LASER_TEXTURE");
                    textureToUse = null;
                } else {
                    cachedMaterial.EnableKeyword("LASER_TEXTURE");
                    switch (textureType) {
                        case ScLaserEffectVisualTextured.Clouds:
                            textureToUse = cloudsTexture;
                            break;

                        case ScLaserEffectVisualTextured.Smoke:
                            textureToUse = smokeTexture;
                            break;

                        case ScLaserEffectVisualTextured.Custom:
                            textureToUse = customTexture;
                            break;
                    }
                }

                cachedMaterial.SetColor("_Color", mainColor);

                cachedMaterial.SetInt("_IsRectangular", type.Equals(ScLaserEffectType.Plane) ? 1 : 0);
                cachedMaterial.SetFloat("_LeftOffset", leftSideOffset);
                cachedMaterial.SetColor("_LeftColor", leftSideOffset != 0 ? leftSideColor : mainColor);
                cachedMaterial.SetFloat("_RightOffset", rightSideOffset);
                cachedMaterial.SetColor("_RightColor", rightSideOffset != 0 ? rightSideColor : mainColor);


                cachedMaterial.SetFloat("_NoiseScale", noiseScaling);
                cachedMaterial.SetFloat("_TimeScale", timeScaling);
                cachedMaterial.SetFloat("_DistortionStrength", distortionStrength);
                cachedMaterial.SetFloat("_FadeOut", fadeOutValue);
                cachedMaterial.SetFloat("_Gamma", gammaValue);
                cachedMaterial.SetFloat("_GeneralTransparency", generalTransparency);

                cachedMaterial.SetTexture("_MainTex", textureToUse);

                cachedMaterial.SetFloat("_ForwardScale", transform.localScale.z);
                cachedMaterial.SetFloat("_RightScale", transform.localScale.x);
            }
        }

        private void ApplyMeshAndMaterialToGameObject() {

            if (show) {

                meshFilter.sharedMesh = cachedMesh;

                if (!visualization.Equals(ScLaserEffectVisual.CustomMaterial)) {
                    meshRenderer.sharedMaterial = cachedMaterial;
                }

            } else {

                meshFilter.sharedMesh = null;
                if (!visualization.Equals(ScLaserEffectVisual.CustomMaterial)) {
                    meshRenderer.sharedMaterial = null;
                }
            }
        }

        /** Helper methods **/

        /// <summary>
        /// Returns the start x point (right direction) for the entire effect, in units (for rectangles) or
        /// angles (for arches and cones).
        /// </summary>
        /// <returns></returns>
        private float GetStartForEffect() {
            switch (type) {
                case ScLaserEffectType.Fan:
                case ScLaserEffectType.Cone:
                    return 90;
                case ScLaserEffectType.Plane:
                    return 0;
            }

            return 0;
        }

        /// <summary>
        /// Returns the width, or circumference, in world units for a single elements within the effect-
        /// </summary>
        /// <returns></returns>
        private float GetWidthPerElement() {

            switch (type) {
                case ScLaserEffectType.Fan:
                    return (2 * (float) Math.PI * length) * (width / 360);
                case ScLaserEffectType.Cone:
                    return (2 * (float) Math.PI * radius * radius) * (width / 360);
                case ScLaserEffectType.Plane:
                    return width;
            }

            return 0;
        }

        /// <summary>
        /// Returns the full width (including all elements and their gaps) for the effect.
        /// </summary>
        /// <returns></returns>
        private float GetFullWidth() {
            return (numberOfSegments * width + (numberOfSegments - 1) * (width<0? -1 : 1) *gapBetweenSegments);
        }


        /** Mesh Generation **/

        /// <summary>
        /// Generates the mesh for a single element in the effect.
        /// </summary>
        /// <param name="startPosition"></param>
        /// <returns></returns>
        private Mesh GenerateElement(float startPosition) {

            switch (type) {
                case ScLaserEffectType.Plane: {
                        return GenerateRectangle(startPosition, width, length);
                    }

                case ScLaserEffectType.Fan: {
                        return GenerateArc(startPosition, startPosition + width, length);
                    }

                case ScLaserEffectType.Cone: {
                        return GenerateCone(startPosition, startPosition + width, radius, length);
                    }
            }

            return null;

        }

        private Mesh GenerateRectangle(float rectStart, float rectWidth, float rectLength) {

            Vector3[] vertices = new Vector3[8];
            Vector3[] normals = new Vector3[8];
            Vector2[] uv = new Vector2[8];
            Vector2[] uv2 = new Vector2[8];
            int[] tris = new int[12];

            // Seen from the top, top:
            // 0    1
            // 2    3
            vertices[0] = new Vector3(rectStart, 0, 0);
            vertices[1] = new Vector3(rectStart + (rectWidth * fill), 0, 0);
            vertices[2] = new Vector3(rectStart, 0, rectLength);
            vertices[3] = new Vector3(rectStart + (rectWidth * fill), 0, rectLength);

            // Seen from the top, bottom:
            // 4    5
            // 6    7
            vertices[4] = new Vector3(rectStart, 0, 0);
            vertices[5] = new Vector3(rectStart + (rectWidth * fill), 0, 0);
            vertices[6] = new Vector3(rectStart, 0, rectLength);
            vertices[7] = new Vector3(rectStart + (rectWidth * fill), 0, rectLength);

            normals[0] = Vector3.up;
            normals[1] = Vector3.up;
            normals[2] = Vector3.up;
            normals[3] = Vector3.up;

            normals[4] = Vector3.down;
            normals[5] = Vector3.down;
            normals[6] = Vector3.down;
            normals[7] = Vector3.down;

            // Unity uses a clockwise winding order for determining front-facing polygons.
            // Top, this is the upper-left triangle
            tris[0] = 0;
            tris[1] = 1;
            tris[2] = 2;

            // Top, lower-right
            tris[3] = 1;
            tris[4] = 3;
            tris[5] = 2;

            // bottom, upper-left as seen from above (reversed order)
            tris[6] = 6;
            tris[7] = 5;
            tris[8] = 4;

            // bottom, lower-right as seen from above (reversed order)
            tris[9] = 6;
            tris[10] = 7;
            tris[11] = 5;

            // UV1 coordinates are world space
            uv[0] = new Vector2(vertices[0].x, vertices[0].z);
            uv[1] = new Vector2(vertices[1].x, vertices[1].z);
            uv[2] = new Vector2(vertices[2].x, vertices[2].z);
            uv[3] = new Vector2(vertices[3].x, vertices[3].z);

            uv[4] = new Vector2(vertices[4].x, vertices[4].z);
            uv[5] = new Vector2(vertices[5].x, vertices[5].z);
            uv[6] = new Vector2(vertices[6].x, vertices[6].z);
            uv[7] = new Vector2(vertices[7].x, vertices[7].z);

            // UV1 coordinates are 0-1 based
            uv2[0] = new Vector3(1, 1); 
            uv2[1] = new Vector3(0, 1); 
            uv2[2] = new Vector2(1, 0); 
            uv2[3] = new Vector2(0, 0); 

            uv2[4] = new Vector3(1, 1); 
            uv2[5] = new Vector3(0, 1); 
            uv2[6] = new Vector2(1, 0); 
            uv2[7] = new Vector2(0, 0); 

            Mesh mesh = new Mesh {
                name = "Laser Plane",
                vertices = vertices,
                triangles = tris,
                normals = normals,
                uv = uv,
                uv2 = uv2
            };

            mesh.RecalculateBounds();

            return mesh;
        }

        private Mesh GenerateArc(float fromAngle, float toAngle, float beamLength) {

            float fullAngle = toAngle - fromAngle;

            // Adjust by fill
            if (fill > 0) {
                fullAngle *= fill;
            } else {
                fullAngle *= -fill;
            }

            int sideFaces = Mathf.Max(1, (int) Mathf.Abs(MAX_FACES * (fullAngle / 360)));
            int sideFaceVertices = sideFaces + 1;

            int vertAmount = sideFaceVertices * 4; // front and back, inner and outer

            Vector3[] vertices = new Vector3[vertAmount];
            Vector3[] normals = new Vector3[vertAmount];
            Vector2[] uv = new Vector2[vertAmount]; // UV coordinates use world space, used for applying the main texture
            Vector2[] uv2 = new Vector2[vertAmount]; // UV2 coordinates use 0 to 1 space, used for determining the edge points
            int[] tris = new int[vertAmount * 3];

            // Generate zero verts, i.e. at the projection point
            for (int i = 0; i < sideFaceVertices; i++) {
                vertices[i] = vertices[i + sideFaceVertices * 2] + new Vector3(0, 0, 0);
                normals[i] = Vector3.up;
                normals[i + sideFaceVertices * 2] = Vector3.down;

                uv[i] = uv[i + sideFaceVertices * 2] = new Vector2(0, 0);
                uv2[i] = uv2[i + sideFaceVertices * 2] = new Vector2(0, 1);
            }

            // Generate verts at end position, from left to right as seen from above
            for (int i = 0; i < sideFaceVertices; i++) {
                int index = i + sideFaceVertices;
                float currentAngle = fromAngle + ((i / (float) sideFaces) * fullAngle);

                float theta = currentAngle * Mathf.Deg2Rad; // convert to radians

                float x = beamLength * Mathf.Cos(theta);
                float y = beamLength * Mathf.Sin(theta);
                vertices[index] = vertices[index + sideFaceVertices * 2] = new Vector3(x, 0, y);

                normals[index] = Vector3.up;
                normals[index + sideFaceVertices * 2] = Vector3.down;

                float uvX = (currentAngle - fromAngle) / fullAngle;

                uv[index] = uv[index + sideFaceVertices * 2] = new Vector2(x, y);
                uv2[index] = uv2[index + sideFaceVertices * 2] = new Vector2(uvX, 0);
            }

            // Triangles facing upwards: First and second quarter of vertices
            // i.e. 0-3-2 for the above example
            int tri = 0;
            for (int i = 0; i < sideFaceVertices - 1; i++) {
                tris[tri] = i;
                tris[tri + 1] = i + sideFaceVertices + 1;
                tris[tri + 2] = i + sideFaceVertices;
                tri += 3;
            }

            for (int i = sideFaceVertices * 2; i < (sideFaceVertices * 3) - 1; i++) {
                tris[tri] = i;
                tris[tri + 1] = i + sideFaceVertices;
                tris[tri + 2] = i + sideFaceVertices + 1;
                tri += 3;
            }

            Mesh mesh = new Mesh {
                name = "Laser Fan",
                vertices = vertices,
                triangles = tris,
                normals = normals,
                uv = uv,
                uv2 = uv2
            };

            mesh.RecalculateBounds();

            return mesh;
        }

        private Mesh GenerateCone(float fromAngle, float toAngle, float outerRadius, float beamLength) {

            float fullAngle = toAngle - fromAngle;

            // Adjust by fill
            fullAngle *= fill;

            int sideFaces = Mathf.Max(1, (int) Mathf.Abs(MAX_FACES * (fullAngle / 360)));
            int sideFaceVertices = sideFaces + 1;

            int vertAmount = sideFaceVertices * 4; // front and back, inner and outer

            Vector3[] vertices = new Vector3[vertAmount];
            Vector3[] normals = new Vector3[vertAmount];
            Vector2[] uv = new Vector2[vertAmount];
            Vector2[] uv2 = new Vector2[vertAmount];
            int[] tris = new int[vertAmount * 3];

            // Generate verts at start position (multiple ones, for correct normals)
            for (int i = 0; i < sideFaceVertices; i++) {
                vertices[i] = vertices[i + sideFaceVertices * 2] + new Vector3(0, 0, 0);
                normals[i] =
                    Vector3.up; // TODO isn't this WRONG? Shouldn't the normal be into the direction of the opening cone?
                normals[i + sideFaceVertices * 2] = Vector3.down;

                uv[i] = uv[i + sideFaceVertices * 2] = new Vector2(0, 0);
                uv2[i] = uv2[i + sideFaceVertices * 2] = new Vector2(0, 1);
            }

            // Generate verts at end position, from left to right as seen from above
            for (int i = 0; i < sideFaceVertices; i++) {
                int index = i + sideFaceVertices;
                float currentAngle = fromAngle + ((i / (float) sideFaces) * fullAngle);
                float theta = currentAngle * Mathf.PI / 180; // convert to radians

                float x = outerRadius * Mathf.Cos(theta);
                float y = outerRadius * Mathf.Sin(theta);
                vertices[index] = vertices[index + sideFaceVertices * 2] = new Vector3(x, y, beamLength);

                normals[index] = Vector3.up;
                normals[index + sideFaceVertices * 2] = Vector3.down;

                float uvX = (currentAngle - fromAngle) / fullAngle;

                uv[index] = uv[index + sideFaceVertices * 2] = new Vector2(x, y);
                uv2[index] = uv2[index + sideFaceVertices * 2] = new Vector2(uvX, 0);
            }

            // Triangles facing upwards: First and second quarter of vertices
            // i.e. 0-3-2 for the above example
            int tri = 0;
            for (int i = 0; i < sideFaceVertices - 1; i++) {
                tris[tri] = i;
                tris[tri + 1] = i + sideFaceVertices + 1;
                tris[tri + 2] = i + sideFaceVertices;
                tri += 3;
            }

            for (int i = sideFaceVertices * 2; i < (sideFaceVertices * 3) - 1; i++) {
                tris[tri] = i;
                tris[tri + 1] = i + sideFaceVertices;
                tris[tri + 2] = i + sideFaceVertices + 1;
                tri += 3;
            }

            Mesh mesh = new Mesh {
                name = "Laser Cone",
                vertices = vertices,
                triangles = tris,
                normals = normals,
                uv = uv,
                uv2 = uv2
            };

            mesh.RecalculateBounds();
            return mesh;
        }


        /** Methods for determining changes **/

        [Serializable]
        private class Shadow {
            public ScLaserEffectType type;
            public ScLaserEffectAlignment alignment;
            public float width;
            public float length;
            public float radius;
            public Color mainColor;
            public float fill;
            public int numberOfElements;
            public float gapBetweenElements;
            public float leftSideOffset;
            public Color leftSideColor;
            public float rightSideOffset;
            public Color rightSideColor;
            public ScLaserEffectVisual visualization;
            public ScLaserEffectVisualTextured textureType;
            public Texture2D customTexture;
            public float noiseScaling;
            public float timeScaling;
            public float distortionStrength;
            public float fadeOutValue;
            public float gammaValue;
            public float generalTransparency;
        }

        private Shadow CloneValues() {

            Shadow clone = new Shadow();
            clone.type = type;
            clone.alignment = alignment;
            clone.width = width;
            clone.length = length;
            clone.radius = radius;
            clone.numberOfElements = numberOfSegments;
            clone.fill = fill;
            clone.gapBetweenElements = gapBetweenSegments;
            clone.mainColor = mainColor;
            clone.leftSideOffset = leftSideOffset;
            clone.rightSideOffset = rightSideOffset;
            clone.leftSideColor = leftSideColor;
            clone.rightSideColor = rightSideColor;
            clone.visualization = visualization;
            clone.textureType = textureType;
            clone.customTexture = customTexture;
            clone.noiseScaling = noiseScaling;
            clone.timeScaling = timeScaling;
            clone.distortionStrength = distortionStrength;
            clone.fadeOutValue = fadeOutValue;
            clone.gammaValue = gammaValue;
            clone.generalTransparency = generalTransparency;
            return clone;
        }

        private bool IsMeshBasedValuesChanged() {

            return (shadow.type != type ||
                    shadow.alignment != alignment ||
                    shadow.width != width ||
                    shadow.length != length ||
                    shadow.radius != radius ||
                    shadow.numberOfElements != numberOfSegments ||
                    shadow.fill != fill ||
                    shadow.gapBetweenElements != gapBetweenSegments);
        }

        private bool IsMaterialRelevantValuesChanged() {
            return (shadow.mainColor != mainColor ||
                    shadow.leftSideOffset != leftSideOffset ||
                    shadow.rightSideOffset != rightSideOffset ||
                    shadow.leftSideColor != leftSideColor ||
                    shadow.rightSideColor != rightSideColor ||
                    shadow.visualization != visualization ||
                    shadow.textureType != textureType ||
                    shadow.customTexture != customTexture ||
                    shadow.noiseScaling != noiseScaling ||
                    shadow.timeScaling != timeScaling ||
                    shadow.distortionStrength != distortionStrength ||
                    shadow.fadeOutValue != fadeOutValue ||
                    shadow.gammaValue != gammaValue ||
                    shadow.generalTransparency != generalTransparency);
        }

    }

}