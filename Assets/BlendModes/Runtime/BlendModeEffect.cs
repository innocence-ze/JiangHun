// Copyright 2014-2018 Elringus (Artyom Sovetnikov). All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace BlendModes
{
    /// <summary>
    /// Applies blend mode effect to an object with a supported component.
    /// </summary>
    /// <remarks>
    /// Supported components are the components, for which a <see cref="ComponentExtension"/> is implemented.
    /// </remarks>
    [AddComponentMenu("Effects/Blend Mode"), ExecuteInEditMode, DisallowMultipleComponent]
    public sealed class BlendModeEffect : MonoBehaviour
    {
        /// <summary>
        /// Current shader family of the object's render material.
        /// </summary>
        public string ShaderFamily { get { return shaderFamily; } set { SetShaderFamily(value); } }
        /// <summary>
        /// Current blend mode.
        /// </summary>
        public BlendMode BlendMode { get { return blendMode; } set { SetBlendMode(value); } }
        /// <summary>
        /// Current render mode.
        /// </summary>
        public RenderMode RenderMode { get { return renderMode; } set { SetRenderMode(value); } }
        /// <summary>
        /// Current mask mode.
        /// </summary>
        public MaskMode MaskMode { get { return maskMode; } set { SetMaskMode(value); } }
        /// <summary>
        /// Current mask behaviour.
        /// </summary>
        public MaskBehaviour MaskBehaviour { get { return maskBehaviour; } set { SetMaskBehaviour(value); } }
        /// <summary>
        /// Color used to blend with the object's texture.
        /// Has effect only when <see cref="RenderMode.TextureWithSelf"/> is used.
        /// </summary>
        public Color OverlayColor { get { return overlayColor; } set { SetOverlayColor(value); } }
        /// <summary>
        /// Texture used to blend with the object's texture.
        /// Has effect only when <see cref="RenderMode.TextureWithSelf"/> is used.
        /// </summary>
        public Texture OverlayTexture { get { return overlayTexture; } set { SetOverlayTexture(value); } }
        /// <summary>
        /// Current UV offset of the <see cref="OverlayTexture"/>.
        /// Has effect only when <see cref="RenderMode.TextureWithSelf"/> is used.
        /// </summary>
        public Vector2 OverlayTextureOffset { get { return overlayTextureOffset; } set { SetOverlayTextureOffset(value); } }
        /// <summary>
        /// Current UV scale of the <see cref="OverlayTexture"/>.
        /// Has effect only when <see cref="RenderMode.TextureWithSelf"/> is used.
        /// </summary>
        public Vector2 OverlayTextureScale { get { return overlayTextureScale; } set { SetOverlayTextureScale(value); } }
        /// <summary>
        /// Whether `Framebuffer` optimization is currently enabled.
        /// Has effect only when <see cref="RenderMode.SelfWithScreen"/> is used.
        /// </summary>
        public bool FramebufferEnabled { get { return framebufferEnabled; } set { SetFramebufferEnabled(value); } }
        /// <summary>
        /// Whether `Unified Grab` optimization is currently enabled.
        /// Has effect only when <see cref="RenderMode.SelfWithScreen"/> is used.
        /// </summary>
        public bool UnifiedGrabEnabled { get { return unifiedGrabEnabled; } set { SetUnifiedGrabEnabled(value); } }
        /// <summary>
        /// Whether to share material instances with the same shader family and blend mode type.
        /// </summary>
        public bool ShareMaterial { get { return shareMaterial; } set { SetShareMaterial(value); } }
        /// <summary>
        /// Whether a <see cref="ComponentExtension"/> for the current object is initialized.
        /// </summary>
        public bool IsComponentExtensionValid { get { return componentExtension && componentExtension.IsValidFor(gameObject); } }

        [SerializeField] private string shaderFamily = null;
        [SerializeField] private BlendMode blendMode = default(BlendMode);
        [SerializeField] private RenderMode renderMode = default(RenderMode);
        [SerializeField] private MaskMode maskMode = default(MaskMode);
        [SerializeField] private MaskBehaviour maskBehaviour = default(MaskBehaviour);
        [SerializeField] private Color overlayColor = Color.white;
        [SerializeField] private Texture overlayTexture = null;
        [SerializeField] private Vector2 overlayTextureOffset = Vector2.zero;
        [SerializeField] private Vector2 overlayTextureScale = Vector2.one;
        [SerializeField] private bool framebufferEnabled = false;
        [SerializeField] private bool unifiedGrabEnabled = false;
        [SerializeField] private bool shareMaterial = false;
        [SerializeField] private ComponentExtension componentExtension = null;

        private const int defaultStencilId = 1;
        private static ShaderResources ShaderResources { get { return cachedShaderResources ?? (cachedShaderResources = ShaderResources.Load()); } }
        private static ShaderResources cachedShaderResources;

        private List<Material> transientMaterials = new List<Material>();
        private bool isMaterialDirty = true;

        /// <summary>
        /// Whether a shader family with the provided name is installed and supported by the <see cref="ComponentExtension"/>.
        /// </summary>
        public bool IsShaderFamilySupported (string shaderFamily)
        {
            if (!componentExtension || !ShaderResources) return false;
            return componentExtension.SupportedShaderFamilies.Contains(shaderFamily) && ShaderResources.GetShaderFamilies().Contains(shaderFamily);
        }

        /// <summary>
        /// Gets currently used component extension.
        /// </summary>
        /// <typeparam name="TComponent">Type of the component extension to get.</typeparam>
        public TComponent GetComponentExtension<TComponent> () where TComponent : ComponentExtension
        {
            return componentExtension as TComponent;
        }

        /// <summary>
        /// Sets current shader family of the object's render material.
        /// </summary>
        public void SetShaderFamily (string shaderFamily)
        {
            if (this.shaderFamily == shaderFamily) return;
            this.shaderFamily = shaderFamily;
            SetMaterialDirty();
        }

        /// <summary>
        /// Sets current blend mode.
        /// </summary>
        public void SetBlendMode (BlendMode blendMode)
        {
            if (this.blendMode == blendMode) return;
            this.blendMode = blendMode;
            SetMaterialDirty();
        }

        /// <summary>
        /// Sets current render mode.
        /// </summary>
        public void SetRenderMode (RenderMode renderMode)
        {
            if (this.renderMode == renderMode) return;
            this.renderMode = renderMode;
            SetMaterialDirty();
        }

        /// <summary>
        /// Sets current mask mode.
        /// </summary>
        public void SetMaskMode (MaskMode maskMode)
        {
            if (this.maskMode == maskMode) return;
            this.maskMode = maskMode;
            SetMaterialDirty();
        }

        /// <summary>
        /// Sets current mask behaviour.
        /// </summary>
        public void SetMaskBehaviour (MaskBehaviour maskBehaviour)
        {
            if (this.maskBehaviour == maskBehaviour) return;
            this.maskBehaviour = maskBehaviour;
            SetMaterialDirty();
        }

        /// <summary>
        /// Sets color to blend with the object's texture.
        /// Has effect only when <see cref="RenderMode.TextureWithSelf"/> is used.
        /// </summary>
        public void SetOverlayColor (Color color)
        {
            if (overlayColor == color) return;
            overlayColor = color;
            SetMaterialDirty();
        }

        /// <summary>
        /// Sets texture to blend with the object's texture.
        /// Has effect only when <see cref="RenderMode.TextureWithSelf"/> is used.
        /// </summary>
        public void SetOverlayTexture (Texture texture)
        {
            if (overlayTexture == texture) return;
            overlayTexture = texture;
            SetMaterialDirty();
        }

        /// <summary>
        /// Sets current UV offset of the <see cref="OverlayTexture"/>.
        /// Has effect only when <see cref="RenderMode.TextureWithSelf"/> is used.
        /// </summary>
        public void SetOverlayTextureOffset (Vector2 offset)
        {
            if (overlayTextureOffset == offset) return;
            overlayTextureOffset = offset;
            SetMaterialDirty();
        }

        /// <summary>
        /// Sets current scale of the <see cref="OverlayTexture"/>.
        /// Has effect only when <see cref="RenderMode.TextureWithSelf"/> is used.
        /// </summary>
        public void SetOverlayTextureScale (Vector2 scale)
        {
            if (overlayTextureScale == scale) return;
            overlayTextureScale = scale;
            SetMaterialDirty();
        }

        /// <summary>
        /// Sets whether `Framebuffer` optimization is currently enabled.
        /// Has effect only when <see cref="RenderMode.SelfWithScreen"/> is used.
        /// </summary>
        public void SetFramebufferEnabled (bool enabled)
        {
            if (framebufferEnabled == enabled) return;
            framebufferEnabled = enabled;
            SetMaterialDirty();
        }

        /// <summary>
        /// Sets whether `Unified Grab` optimization is currently enabled.
        /// Has effect only when <see cref="RenderMode.SelfWithScreen"/> is used.
        /// </summary>
        public void SetUnifiedGrabEnabled (bool enabled)
        {
            if (unifiedGrabEnabled == enabled) return;
            unifiedGrabEnabled = enabled;
            SetMaterialDirty();
        }

        /// <summary>
        /// Sets whether to share material instances with the same shader family and blend mode type.
        /// </summary>
        public void SetShareMaterial (bool value)
        {
            if (shareMaterial == value) return;
            shareMaterial = value;
            SetMaterialProperties(true);
        }

        /// <summary>
        /// Forces to apply material properties on next update.
        /// </summary>
        public void SetMaterialDirty ()
        {
            isMaterialDirty = true;
        }

        /// <summary>
        /// Initializes the attached component extension.
        /// </summary>
        public void InitializeComponentExtension ()
        {
            var forceCreateMaterial = false;
            if (!componentExtension || !componentExtension.IsValidFor(gameObject))
            {
                componentExtension = ComponentExtension.CreateForObject(gameObject);
                forceCreateMaterial = true;
            }
            if (!componentExtension) return; // No supported component types found on the extended object.
            componentExtension.OnEffectEnabled();
            if (string.IsNullOrEmpty(ShaderFamily))
                ShaderFamily = componentExtension.DefaultShaderFamily;
            SetMaterialProperties(forceCreateMaterial);
        }

        private void SetMaterialProperties (bool forceCreateMaterial = false)
        {
            if (!isActiveAndEnabled || !componentExtension || !ShaderResources) return;
            if (!IsShaderFamilySupported(ShaderFamily)) return;

            var maskEnabled = MaskMode != MaskMode.Disabled;
            var shaderName = ShaderUtilities.BuildShaderName(ShaderFamily, RenderMode, maskEnabled, FramebufferEnabled, UnifiedGrabEnabled);
            if (!ShaderResources.ShaderExists(shaderName)) return;

            var materials = componentExtension.GetRenderMaterials();
            var shareMaterial = ShareMaterial && componentExtension.AllowMaterialSharing();
            if (materials == null || materials.Length == 0) materials = new Material[] { null };
            for (int i = 0; i < materials.Length; i++)
            {
                if (shareMaterial)
                {
                    var shader = ShaderResources.GetShaderByName(shaderName);
                    var sharedMaterialCreated = false;
                    materials[i] = MaterialCache.GetSharedFor(shader, this, out sharedMaterialCreated);
                    if (sharedMaterialCreated) componentExtension.OnEffectMaterialCreated(materials[i]);
                }
                else if (forceCreateMaterial || !materials[i] || materials[i].shader.name != shaderName)
                {
                    var shader = ShaderResources.GetShaderByName(shaderName);
                    materials[i] = CreateTransientMaterial(shader);
                    componentExtension.OnEffectMaterialCreated(materials[i]);
                }

                ShaderUtilities.SelectBlendModeKeyword(materials[i], BlendMode);

                var propertyBlock = shareMaterial ? componentExtension.GetPropertyBlock(i) : null;
                if (RenderMode == RenderMode.TextureWithSelf) SetOverlayMaterialProperties(materials[i], propertyBlock);
                if (maskEnabled) SetMaskMaterialProperties(materials[i], propertyBlock);
                if (propertyBlock != null) componentExtension.SetPropertyBlock(propertyBlock, i);
            }

            componentExtension.ApplyShaderProperties(materials);
            componentExtension.SetRenderMaterials(materials);

            isMaterialDirty = false;
        }

        private void SetOverlayMaterialProperties(Material material, MaterialPropertyBlock block)
        {
            if (block != null)
            {
                block.SetColor(ShaderUtilities.OverlayColorPropertyId, OverlayColor);
                block.SetTexture(ShaderUtilities.OverlayTexturePropertyId, !OverlayTexture ? Texture2D.whiteTexture : OverlayTexture); // Can't set null here.
                block.SetVector(ShaderUtilities.OverlayTextureSTPropertyId, new Vector4(OverlayTextureScale.x, OverlayTextureScale.y, OverlayTextureOffset.x, OverlayTextureOffset.y));
            }
            else
            {
                material.SetColor(ShaderUtilities.OverlayColorPropertyId, OverlayColor);
                material.SetTexture(ShaderUtilities.OverlayTexturePropertyId, OverlayTexture);
                material.SetTextureOffset(ShaderUtilities.OverlayTexturePropertyId, OverlayTextureOffset);
                material.SetTextureScale(ShaderUtilities.OverlayTexturePropertyId, OverlayTextureScale);
            }
        }

        private void SetMaskMaterialProperties (Material material, MaterialPropertyBlock block)
        {
            var blendStencilComp = MaskMode == MaskMode.NothingButMask ? (int)CompareFunction.Equal : (int)CompareFunction.NotEqual;
            var normalStencilComp = MaskBehaviour == MaskBehaviour.Cutout ? (int)CompareFunction.Never : MaskMode == MaskMode.NothingButMask ? (int)CompareFunction.NotEqual : (int)CompareFunction.Equal;

            if (block != null)
            {
                block.SetFloat(ShaderUtilities.BlendStencilCompPropertyId, blendStencilComp);
                block.SetFloat(ShaderUtilities.NormalStencilCompPropertyId, normalStencilComp);
                block.SetFloat(ShaderUtilities.StencilIdPropertyId, defaultStencilId);
            }
            else
            {
                material.SetFloat(ShaderUtilities.BlendStencilCompPropertyId, blendStencilComp);
                material.SetFloat(ShaderUtilities.NormalStencilCompPropertyId, normalStencilComp);
                material.SetFloat(ShaderUtilities.StencilIdPropertyId, defaultStencilId);
            }
        }

        private Material CreateTransientMaterial (Shader shader)
        {
            var material = new Material(shader);
            material.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
            transientMaterials.Add(material);
            return material;
        }

        private void DestroyTransientMaterials ()
        {
            for (int i = 0; i < transientMaterials.Count; i++)
            {
                var material = transientMaterials[i];
                if (!material) return;
                if (Application.isPlaying) Destroy(material);
                else DestroyImmediate(material);
            }
            transientMaterials.Clear();
        }

        private void Reset ()
        {
            InitializeComponentExtension();
        }

        private void Awake ()
        {
            InitializeComponentExtension();
        }

        private void OnEnable ()
        {
            InitializeComponentExtension();
        }

        private void OnDisable ()
        {
            if (componentExtension)
                componentExtension.OnEffectDisabled();
        }

        private void OnDestroy ()
        {
            DestroyTransientMaterials();
        }

        private void OnValidate ()
        {
            // Update material properties when changing serialized fields with editor GUI.
            SetMaterialProperties();
        }

        private void OnDidApplyAnimationProperties ()
        {
            // Update material properties when changing serialized fields with Unity animation.
            SetMaterialDirty();
        }

        private void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            if (componentExtension)
                componentExtension.OnEffectRenderImage(source, destination);
        }

        private void Update ()
        {
            if (isMaterialDirty) SetMaterialProperties();
        }
    }
}
