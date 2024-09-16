using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace SceneTransitionSystem
{
    /// <summary>
    /// An attribute used to indicate that a SceneTransitionSystem effect should not have a small preview available.
    /// </summary>
    public class STSNoSmallPreviewAttribute : Attribute
    {
    }

    /// <summary>
    /// Disables the big preview functionality for a scene transition effect.
    /// </summary>
    public class STSNoBigPreviewAttribute : Attribute
    {
    }

    /// <summary>
    /// Custom attribute used to indicate that a field should be treated as an animation curve.
    /// </summary>
    public class STSAnimationCurveAttribute : Attribute
    {
    }

    /// <summary>
    /// Attribute to specify a name for an STS effect.
    /// </summary>
    public class STSEffectNameAttribute : Attribute
    {
        /// <summary>
        /// Represents the name of the effect as a string.
        /// </summary>
        public string EffectName;

        /// <summary>
        /// Attribute to define the name of an effect in the Scene Transition System.
        /// </summary>
        public STSEffectNameAttribute(string sEffectName)
        {
            this.EffectName = sEffectName;
        }
    }

    /// <summary>
    /// The STSTintPrimaryAttribute class is used to designate that a specific effect type
    /// requires a primary tint attribute. This attribute is intended to be used in the
    /// Scene Transition System to add tint customization to effects.
    /// </summary>
    public class STSTintPrimaryAttribute : Attribute
    {
        /// <summary>
        /// Represents the entitlement or description for various scene transition attributes,
        /// particularly used within the context of tinting attributes such as primary tint.
        /// </summary>
        public string Entitlement = "Tint Primary";

        /// <summary>
        /// Attribute that indicates the primary tint used for a scene transition.
        /// </summary>
        public STSTintPrimaryAttribute()
        {
        }

        /// <summary>
        /// Attribute used to specify that the primary tint should be applied to the annotated target.
        /// </summary>
        public STSTintPrimaryAttribute(string sEntitlement)
        {
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// Attribute class used to designate secondary tinting for scene transitions.
    /// </summary>
    public class STSTintSecondaryAttribute : Attribute
    {
        /// <summary>
        /// Represents the entitlement description associated with the secondary tint in an STSTintSecondaryAttribute.
        /// </summary>
        public string Entitlement = "Tint Secondary";

        /// <summary>
        /// Defines an attribute to specify a secondary tint color for scene transitions.
        /// </summary>
        /// <remarks>
        /// The secondary tint is represented by the `Entitlement` property, which can be set
        /// through the constructor.
        /// </remarks>
        public STSTintSecondaryAttribute()
        {
        }

        /// <summary>
        /// Attribute to define a secondary tint for scene transitions.
        /// </summary>
        public STSTintSecondaryAttribute(string sEntitlement)
        {
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// An attribute used to mark a primary texture for a scene transition effect in the Scene Transition System.
    /// </summary>
    public class STSTexturePrimaryAttribute : Attribute
    {
        /// <summary>
        /// The <c>Entitlement</c> variable signifies the entitlement or designation specific to a texture attribute in the Scene Transition System.
        /// This variable is instrumental in customization and identification of primary texture usage within the system's attribute classes.
        /// </summary>
        public string Entitlement = "Texture Primary";

        /// <summary>
        /// Defines an attribute for specifying a primary texture in a Scene Transition System effect.
        /// </summary>
        public STSTexturePrimaryAttribute()
        {
        }

        /// <summary>
        /// Represents a primary texture attribute for scene transition effects.
        /// </summary>
        public STSTexturePrimaryAttribute(string sEntitlement)
        {
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// The STSTextureSecondaryAttribute class is used for defining a secondary texture attribute for scene transitions in the Scene Transition System (STS).
    /// </summary>
    public class STSTextureSecondaryAttribute : Attribute
    {
        /// <summary>
        /// Represents the entitlement string used for the secondary texture
        /// in the Scene Transition System.
        /// </summary>
        public string Entitlement = "Texture Secondary";

        /// <summary>
        /// Represents a secondary texture attribute in the scene transition system.
        /// </summary>
        public STSTextureSecondaryAttribute()
        {
        }

        /// <summary>
        /// Attribute to define a secondary texture property.
        /// </summary>
        public STSTextureSecondaryAttribute(string sEntitlement)
        {
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// Represents an attribute for a parameter in the Scene Transition System (STS).
    /// This attribute can be used to specify the entitlement of the parameter,
    /// whether it should be displayed as a slider, and its minimum and maximum values if applicable.
    /// </summary>
    public class STSParameterOneAttribute : Attribute
    {
        /// <summary>
        /// Represents an entitlement string for the STSParameterOneAttribute class.
        /// </summary>
        public string Entitlement = "Parameter One";

        /// <summary>
        /// Indicates whether the parameter should be represented by a slider with a range,
        /// allowing users to adjust its value between a minimum and maximum limit.
        /// </summary>
        public bool Slider = false;

        /// <summary>
        /// Represents the minimum value for a parameter in the Scene Transition System.
        /// </summary>
        public int Min;

        /// <summary>
        /// Represents the maximum value for a parameter within the Scene Transition System.
        /// </summary>
        public int Max;

        /// <summary>
        /// Represents a custom attribute to define a parameter with an entitlement of "Parameter One".
        /// </summary>
        /// <remarks>
        /// This attribute can be used to designate a specific parameter, optionally with slider settings that include minimum and maximum values.
        /// </remarks>
        public STSParameterOneAttribute()
        {
        }

        /// <summary>
        /// Specifies a customizable parameter for scene transitions with optional slider functionality, minimum, and maximum values.
        /// </summary>
        public STSParameterOneAttribute(string sEntitlement, int sMin, int sMax)
        {
            this.Slider = true;
            this.Entitlement = sEntitlement;
            this.Min = sMin;
            this.Max = sMax;
        }

        /// <summary>
        /// An attribute class used to specify a first parameter in a scene transition system effect.
        /// </summary>
        /// <remarks>
        /// This attribute can be used to define a custom entitlement for the parameter, and optionally
        /// configure it as a slider with a specified minimum and maximum value.
        /// </remarks>
        public STSParameterOneAttribute(string sEntitlement)
        {
            this.Slider = false;
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// The STSParameterTwoAttribute class is an attribute used in the SceneTransitionSystem namespace
    /// to define metadata for scene transition effects. It allows specifying an entitlement or name
    /// for the attribute and optionally configures it as a slider with minimum and maximum values.
    /// </summary>
    public class STSParameterTwoAttribute : Attribute
    {
        /// <summary>
        /// Represents a descriptive title or label, often used to identify or name a specific parameter or feature in the UI.
        /// </summary>
        public string Entitlement = "Parameter Two";

        /// <summary>
        /// Indicates whether the parameter should be represented as a slider.
        /// </summary>
        public bool Slider = false;

        /// <summary>
        /// The minimum value for the parameter in the scene transition effect.
        /// </summary>
        public int Min;

        /// <summary>
        /// Represents the maximum value for the parameter.
        /// </summary>
        public int Max;

        /// <summary>
        /// Represents an attribute for applying a secondary parameter with optional slider UI and entitlement.
        /// </summary>
        public STSParameterTwoAttribute()
        {
        }

        /// <summary>
        /// Attribute to define parameters for a custom transition effect with additional configurations like sliders and value ranges.
        /// </summary>
        public STSParameterTwoAttribute(string sEntitlement, int sMin, int sMax)
        {
            this.Slider = true;
            this.Entitlement = sEntitlement;
            this.Min = sMin;
            this.Max = sMax;
        }

        /// <summary>
        /// Attribute representing the second parameter for a scene transition effect.
        /// </summary>
        public STSParameterTwoAttribute(string sEntitlement)
        {
            this.Slider = false;
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// Defines an attribute for a parameter in the Scene Transition System (STSEffectType).
    /// </summary>
    public class STSParameterThreeAttribute : Attribute
    {
        /// <summary>
        /// The entitlement string used within the STSParameterThreeAttribute class.
        /// Represents a description or label for what the third parameter is intended to control or signify.
        /// </summary>
        public string Entitlement = "Parameter Three";

        /// This boolean variable determines whether a slider control is used for the parameter.
        /// When set to true, the parameter will be represented with a slider for user input.
        /// When set to false, the parameter will be represented without a slider.
        public bool Slider = false;

        /// <summary>
        /// The minimum value for the parameter in the STSParameterThreeAttribute class.
        /// </summary>
        public int Min;

        /// <summary>
        /// Represents the maximum value for the third parameter of an STS effect.
        /// </summary>
        public int Max;

        /// <summary>
        /// Attribute to define a customizable parameter for a scene transition system.
        /// Provides an option to specify a slider with minimum and maximum values.
        /// </summary>
        public STSParameterThreeAttribute()
        {
        }

        /// <summary>
        /// Custom attribute used for defining parameters in the scene transition system.
        /// This attribute is specifically for "Parameter Three" and allows configuration
        /// of its properties such as entitlement, minimum value, and maximum value.
        /// </summary>
        public STSParameterThreeAttribute(string sEntitlement, int sMin, int sMax)
        {
            this.Slider = true;
            this.Entitlement = sEntitlement;
            this.Min = sMin;
            this.Max = sMax;
        }

        /// <summary>
        /// An attribute used to denote a third configurable parameter in an effect.
        /// </summary>
        /// <remarks>
        /// This attribute allows for the specification of a third parameter, which can be configured with optional minimum and maximum values if a slider is required.
        /// </remarks>
        public STSParameterThreeAttribute(string sEntitlement)
        {
            this.Slider = false;
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// An attribute used to indicate an offset entitlement for scene transition effects.
    /// </summary>
    public class STSOffsetAttribute : Attribute
    {
        /// <summary>
        /// Represents the entitlement attribute for the STSOffsetAttribute class.
        /// This variable holds a string value that serves as the label or identifier
        /// for the offset attribute in the scene transition system.
        /// It is utilized to customize the display and behavior of the offset property
        /// in the Unity editor.
        /// </summary>
        public string Entitlement = "Offset";

        /// <summary>
        /// An attribute to specify entitlement for an offset in the Scene Transition System (STS).
        /// </summary>
        public STSOffsetAttribute()
        {
        }

        /// <summary>
        /// Represents an attribute used to define an offset property in the scene transition system.
        /// </summary>
        public STSOffsetAttribute(string sEntitlement)
        {
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// An attribute used to mark a field or property for special handling within
    /// the Scene Transition System. Specifically, it denotes that the element
    /// is associated with a "Two Cross" transition effect.
    /// </summary>
    public class STSTwoCrossAttribute : Attribute
    {
        /// <summary>
        /// Represents the entitlement related to the STSTwoCrossAttribute.
        /// </summary>
        public string Entitlement = "Orientation";

        /// <summary>
        /// Represents an attribute that specifies a two-orientation cross, used for defining the orientation
        /// (Vertical or Horizontal) in a scene transition system.
        /// </summary>
        public STSTwoCrossAttribute()
        {
        }

        /// <summary>
        /// Specifies an orientation for a 2-cross attribute.
        /// </summary>
        /// <remarks>
        /// This attribute is used to define a horizontal or vertical orientation.
        /// Default entitlement is "Orientation".
        /// </remarks>
        public STSTwoCrossAttribute(string sEntitlement)
        {
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// An attribute class used to designate effects that involve a four-cross pattern
    /// in the Scene Transition System.
    /// </summary>
    public class STSFourCrossAttribute : Attribute
    {
        /// <summary>
        /// Represents the entitlement string associated with an STSFourCrossAttribute.
        /// This is used in the Scene Transition System to specify additional metadata or descriptive information
        /// for effects that utilize the four-cross transition.
        /// </summary>
        public string Entitlement = "Four cross";

        /// <summary>
        /// Represents an attribute for defining a four cross effect in the Scene Transition System.
        /// </summary>
        public STSFourCrossAttribute()
        {
        }

        /// <summary>
        /// Attribute to specify four cross entitlement for scene transitions.
        /// </summary>
        /// <remarks>
        /// This attribute can be used to tag properties or fields that involve
        /// transitions in four directions (Top, Bottom, Right, Left).
        /// </remarks>
        public STSFourCrossAttribute(string sEntitlement)
        {
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// The STSFiveCrossAttribute class is a custom attribute utilized within the SceneTransitionSystem.
    /// This attribute signifies that the associated effect type has a "Five cross" entitlement.
    /// </summary>
    public class STSFiveCrossAttribute : Attribute
    {
        /// <summary>
        /// The <c>Entitlement</c> field represents a descriptive string associated with the
        /// <c>STSFiveCrossAttribute</c> class, typically used to provide a label or identifier
        /// for the attribute when applied to an element in the Scene Transition System.
        /// </summary>
        public string Entitlement = "Five cross";

        /// <summary>
        /// Represents an attribute that denotes a five-cross configuration.
        /// </summary>
        public STSFiveCrossAttribute()
        {
        }

        /// <summary>
        /// Represents an attribute for the "Five cross" effect in the Scene Transition System.
        /// </summary>
        public STSFiveCrossAttribute(string sEntitlement)
        {
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// Attribute to mark effects with the "Eight cross" entitlement in the Scene Transition System.
    /// </summary>
    public class STSEightCrossAttribute : Attribute
    {
        /// <summary>
        /// Represents the entitlement description for an STSEightCrossAttribute.
        /// </summary>
        public string Entitlement = "Eight cross";

        /// <summary>
        /// Attribute indicating an "Eight Cross" effect in the Scene Transition System.
        /// </summary>
        public STSEightCrossAttribute()
        {
        }

        /// <summary>
        /// An attribute representing an eight-cross effect in the scene transition system.
        /// </summary>
        public STSEightCrossAttribute(string sEntitlement)
        {
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// An attribute to specify that an effect should be rendered in a clockwise manner.
    /// </summary>
    public class STSClockwiseAttribute : Attribute
    {
        /// <summary>
        /// The Entitlement variable represents the directional attribute of a scene transition effect.
        /// It is primarily used in the context of attribute classes within the SceneTransitionSystem to
        /// denote the direction of a transition, such as "Clockwise".
        /// The Entitlement variable can be set to either "Clockwise" or "Counterclockwise".
        /// </summary>
        public string Entitlement = "Clockwise";

        /// <summary>
        /// An attribute used to denote that an effect operates in a clockwise direction.
        /// It can be applied to classes to specify that they should execute their functionality in a clockwise manner.
        /// </summary>
        public STSClockwiseAttribute()
        {
        }

        /// <summary>
        /// The STSClockwiseAttribute class is an attribute that indicates the direction
        /// of a specific effect or animation as clockwise. It contains an entitlement field
        /// that can be customized via constructors to specify the desired direction.
        /// </summary>
        public STSClockwiseAttribute(string sEntitlement)
        {
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// This attribute is used to designate a class or method that pertains to the "Nine cross" effect
    /// in the Scene Transition System.
    /// </summary>
    public class STSNineCrossAttribute : Attribute
    {
        /// <summary>
        /// Represents the entitlement associated with the STSNineCrossAttribute,
        /// used to specify or override the nine cross textual description in the
        /// Scene Transition System.
        /// </summary>
        /// <remarks>
        /// The Entitlement property can be used to provide a custom textual representation
        /// for the nine cross when applied in a scene transition effect.
        /// </remarks>
        public string Entitlement = "Nine cross";

        /// An attribute that signifies the use of a "Nine cross" pattern.
        /// This attribute can be used to denote an effect or transition that
        /// involves nine distinct sections or areas (typically: Top, Bottom,
        /// Right, Left, Center, TopLeft, TopRight, BottomLeft, BottomRight).
        /// The default entitlement is "Nine cross".
        /// Constructors:
        /// STSNineCrossAttribute() - Initializes with the default entitlement.
        /// STSNineCrossAttribute(string sEntitlement) - Initializes with a
        /// specified entitlement.
        public STSNineCrossAttribute()
        {
        }

        /// <summary>
        /// Represents a custom attribute for an effect with a "Nine cross" pattern.
        /// This attribute allows customization of the effect by specifying an entitlement string.
        /// </summary>
        public STSNineCrossAttribute(string sEntitlement)
        {
            this.Entitlement = sEntitlement;
        }
    }

    /// <summary>
    /// An enumeration representing the four possible directions: Top, Bottom, Right, and Left.
    /// </summary>
    public enum STSFourCross : int
    {
        /// <summary>
        /// Represents the top position for a four-way cross effect.
        /// </summary>
        Top,

        /// <summary>
        /// Represents the bottom position in a four-way directional cross.
        /// </summary>
        Bottom,

        /// <summary>
        /// Represents the right direction in a four-directional cross.
        /// </summary>
        Right,

        /// <summary>
        /// Represents the 'Left' position in the STSFourCross enum, indicating an orientation or
        /// direction towards the left side in a four-point cross configuration.
        /// </summary>
        Left,
    }

    /// <summary>
    /// Specifies the directions for the five-cross transition in the Scene Transition System.
    /// </summary>
    public enum STSFiveCross : int
    {
        /// <summary>
        /// Represents the top position in a five-cross configuration for scene transitions.
        /// </summary>
        Top,

        /// <summary>
        /// Represents the bottom position in a five-directional cross pattern.
        /// </summary>
        Bottom,

        /// <summary>
        /// The Right position for the STSFiveCross enumeration.
        /// Indicates an effect starting point or orientation towards the right side.
        /// </summary>
        Right,

        /// <summary>
        /// Enum member representing the left direction in the STSFiveCross enumeration.
        /// </summary>
        Left,

        /// <summary>
        /// Represents the center position in the STSFiveCross enumeration.
        /// </summary>
        Center,
    }

    /// <summary>
    /// Specifies the eight cardinal and intercardinal directions.
    /// </summary>
    public enum STSEightCross : int
    {
        /// <summary>
        /// Represents the top position in the 8-directional cross enumeration.
        /// </summary>
        Top,

        /// <summary>
        /// Represents the bottom direction in an eight-way cross.
        /// </summary>
        Bottom,

        /// <summary>
        /// Represents the right alignment/position within an 8-way directional control in the SceneTransitionSystem namespace.
        /// </summary>
        Right,

        /// <summary>
        /// Represents the left direction in an eight-directional cross.
        /// </summary>
        Left,

        /// <summary>
        /// Represents the top-left direction in the STSEightCross enum, which is used for scene transitions.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Represents the top-right position in an eight-directional cross.
        /// </summary>
        TopRight,

        /// <summary>
        /// Represents the direction at the bottom-left corner in the coordinate system.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Represents the bottom-right position in an eight-way cross configuration.
        /// </summary>
        BottomRight,
    }

    /// <summary>
    /// Specifies the direction of rotation for certain effects in the Scene Transition System (STS).
    /// </summary>
    public enum STSClockwise : int
    {
        /// <summary>
        /// Represents a direction of movement in a clockwise manner.
        /// </summary>
        Clockwise,

        /// <summary>
        /// Represents the counterclockwise direction for animations or transitions.
        /// </summary>
        Counterclockwise
    }

    /// <summary>
    /// Represents the types of two-cross scene transition effects.
    /// </summary>
    public enum STSTwoCross : int
    {
        /// <summary>
        /// Indicates the vertical orientation for the transition effect.
        /// </summary>
        Vertical,

        /// <summary>
        /// Represents a horizontal orientation or direction in the STSTwoCross enum.
        /// </summary>
        Horizontal
    }

    /// <summary>
    /// Enum representing the nine possible crossing directions in a 3x3 grid system.
    /// </summary>
    public enum STSNineCross : int
    {
        /// <summary>
        /// Represents the top position in a 3x3 grid system.
        /// </summary>
        Top,

        /// <summary>
        /// Represents the bottom position in a 3x3 grid layout for scene transitions.
        /// </summary>
        Bottom,

        /// <summary>
        /// Represents the right position in a nine-directional cross pattern.
        /// </summary>
        Right,

        /// <summary>
        /// Represents the left position in a 3x3 grid within the Scene Transition System.
        /// It is used to specify the leftmost region in various transition effects.
        /// </summary>
        Left,

        /// <summary>
        /// The center position within a 3x3 grid.
        /// </summary>
        Center,

        /// <summary>
        /// Represents the top-left position in a nine-cross directional layout.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Represents the top-right position in a 3x3 grid for the `STSNineCross` enumeration.
        /// </summary>
        TopRight,

        /// <summary>
        /// Represents the bottom-left position in a 3x3 grid.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Represents the bottom-right position in a 3x3 grid for scene transitions.
        /// </summary>
        BottomRight,
    }

    /// <summary>
    /// Represents the base class for all scene transition effects in the SceneTransitionSystem namespace.
    /// </summary>
    [Serializable]
    public class STSEffectBase
    {
        //[SerializeField]
        /// <summary>
        /// Name of the visual or transitional effect.
        /// </summary>
        public string EffectName = "";

        /// <summary>
        /// Represents an AnimationCurve used to define the behavior of an animation effect
        /// over time within the Scene Transition System. The curve determines the value
        /// of a property at different points in time, allowing for complex animations.
        /// </summary>
        public AnimationCurve Curve;

        /// <summary>
        /// Represents the primary tint color to be used in various scene transition effects.
        /// </summary>
        /// <remarks>
        /// This color is applied to specific visual elements during transitions.
        /// It is utilized by different transition effects to achieve the desired visual outcome.
        /// Example effects employing this variable include circle drawings, fade animations, and more.
        /// </remarks>
        /// <value>
        /// The default value is Color.black.
        /// </value>
        public Color TintPrimary = Color.black;

        /// <summary>
        /// Represents the secondary tint color used in the transition effect.
        /// This color can be used for blending, gradient transitions, or other visual effects
        /// during scene transitions, providing a secondary color reference to the primary tint color.
        /// </summary>
        public Color TintSecondary = Color.black;

        /// <summary>
        /// Represents the primary texture used in the scene transition effect. This texture
        /// is applied as the main visual element during the transition and can be customized
        /// to fit various transition styles.
        /// </summary>
        public Texture2D TexturePrimary = null;

        /// <summary>
        /// Represents the secondary texture used in the scene transition effect.
        /// This texture provides an additional layer of customization and visual variation
        /// alongside the primary texture.
        /// </summary>
        public Texture2D TextureSecondary = null;

        /// <summary>
        /// Represents the position offset for the effect.
        /// This field is used to adjust the position of the effect within the scene.
        /// </summary>
        public Vector2 Offset;

        /// <summary>
        /// Specifies the type of cross transition effect to be used in the scene transition system.
        /// </summary>
        /// <remarks>
        /// This variable determines whether the cross transition effect will be applied vertically or horizontally.
        /// </remarks>
        public STSTwoCross TwoCross;

        /// <summary>
        /// Represents a four-way cross type effect used in scene transitions.
        /// </summary>
        public STSFourCross FourCross;

        /// <summary>
        /// Represents one of the five possible directions for a cross effect in scene transitions.
        /// </summary>
        /// <remarks>
        /// The FiveCross variable determines the direction or position used for certain scene transition effects.
        /// Possible values include:
        /// - Top
        /// - Bottom
        /// - Right
        /// - Left
        /// - Center
        /// </remarks>
        public STSFiveCross FiveCross;

        /// <summary>
        /// Represents the directional transitions available in the Scene Transition System.
        /// This variable determines which transitional direction (top, bottom, right, left, etc.)
        /// will be used for the animation effect.
        /// </summary>
        public STSEightCross EightCross;

        /// <summary>
        /// Represents the nine-cross transition effect setting in the Scene Transition System.
        /// </summary>
        /// <remarks>
        /// NineCross is a member of the STSEffectBase class and indicates a transition effect characterized
        /// by nine different positions: Top, Bottom, Right, Left, Center, TopLeft, TopRight, BottomLeft, and BottomRight.
        /// This effect can be used to create complex scene transitions by specifying the entry and exit locations
        /// of the transition effect.
        /// </remarks>
        public STSNineCross NineCross;

        /// <summary>
        /// Represents the direction of the effect's animation.
        /// </summary>
        /// <remarks>
        /// This variable indicates whether the animation effect should proceed in a clockwise or counterclockwise direction.
        /// </remarks>
        public STSClockwise Clockwise;

        /// <summary>
        /// Represents a customizable integer parameter for various graphical effects
        /// within the Scene Transition System. This parameter can influence different
        /// aspects of visual transitions, such as intensity, duration, or other effect-specific
        /// attributes.
        /// </summary>
        public int ParameterOne;

        /// <summary>
        /// Represents the number of columns in a grid system used by the scene transition effect.
        /// </summary>
        public int ParameterTwo;

        /// <summary>
        /// Represents an integer parameter used in the scene transition effect.
        /// This parameter is utilized within various scene transition computations
        /// and visual effect adjustments.
        /// </summary>
        public int ParameterThree;

        /// <summary>
        /// Represents the duration of the STS effect in seconds.
        /// </summary>
        public float Duration = 1.0F;

        /// <summary>
        /// Represents the percentage (from 0.0 to 1.0) that indicates the progress of the effect within the Scene Transition System.
        /// </summary>
        public float Purcent = 0.0F;

        /// <summary>
        /// Represents the current value on the animation curve evaluated at the percentage of the transition.
        /// </summary>
        /// <remarks>
        /// This variable is used to determine the interpolated value on the animation curve based on the current transition percentage.
        /// </remarks>
        public float CurvePurcent = 0.0F;

        // Pseudo private... pblic but not in the inspector
        /// <summary>
        /// Represents the direction of the animation progression for a scene transition effect.
        /// 0 indicates an unknown direction.
        /// 1 indicates that the animation progresses from 0.0F to 1.0F percentage.
        /// -1 indicates that the animation progresses from 1.0F to 0.0F percentage.
        /// </summary>
        public int Direction = 0; // 0 is unknow; -1 go from 1.0F to 0.0F purcent; 1 go from 0.0F to 1.0F purcent

        /// <summary>
        /// Represents the percentage of animation completion ranging from 0.0 (start) to 1.0 (complete).
        /// </summary>
        public float AnimPurcent = 0.0F;

        /// <summary>
        /// Indicates whether the animation is currently playing.
        /// </summary>
        public bool AnimIsPlaying = false;

        /// <summary>
        /// Indicates whether the animation has finished playing.
        /// </summary>
        public bool AnimIsFinished = false;


        /// <summary>
        /// Represents the previous color state before the transition effect begins.
        /// </summary>
        public Color OldColor;

        /// <summary>
        /// The duration of the color transition effect.
        /// Used to indicate how long the transition between colors should last.
        /// </summary>
        public float ColorDuration = 0.0F;

        /// <summary>
        /// Represents the percentage completion of a color transition in an effect animation.
        /// A value of 0.0F indicates that the transition has just started, while a value of 1.0F indicates that the transition is complete.
        /// </summary>
        public float ColorPurcent = 0.0F;

        /// <summary>
        /// Indicates whether a color transition effect is currently playing.
        /// </summary>
        public bool ColorIsPlaying = false;

        /// <summary>
        /// Indicates whether the color transition effect has completed.
        /// </summary>
        public bool ColorIsFinished = false;
    }

    /// <summary>
    /// Represents a type of effect in the Scene Transition System.
    /// </summary>
    [Serializable]
    public class STSEffectType : STSEffectBase
    {
        /// <summary>
        /// List to store GUI content for different types of scene transition effects.
        /// </summary>
        public static List<GUIContent> kEffectContentList = new List<GUIContent>();

        /// <summary>
        /// A list containing the types of effects used in the Scene Transition System (STS).
        /// </summary>
        public static List<Type> kEffectTypeList = new List<Type>();

        /// <summary>
        /// A static list that contains the names of all effects available in the
        /// Scene Transition System (STS).
        /// Intended to be used for effect selection and referencing.
        /// </summary>
        public static List<string> kEffectNameList = new List<string>();

        /// <summary>
        /// The default effect type used for scene transitions within the Scene Transition System (STS).
        /// It initializes with a 'Fade basic' effect, black color, and a duration of 1.0 seconds.
        /// </summary>
        public static STSEffectType Default = new STSEffectType(STSEffectFade.K_FADE_NAME, Color.black, 1.0F);

        /// <summary>
        /// A static instance of the <see cref="STSEffectType"/> class with predefined settings for quick default effects.
        /// </summary>
        public static STSEffectType QuickDefault = new STSEffectType(STSEffectFade.K_FADE_NAME, Color.black, 0.50F);

        /// <summary>
        /// A predefined effect type in the scene transition system representing a "flash" effect.
        /// </summary>
        public static STSEffectType Flash = new STSEffectType(STSEffectFade.K_FADE_NAME, Color.white, 0.50F);

        /// <summary>
        /// Represents the base class for scene transition effects, providing common attributes and methods for manipulating
        /// various effect parameters such as color, texture, and curves used in animations.
        /// </summary>
        public STSEffectType()
        {
            Curve = AnimationCurve.Linear(0.0F, 0.0F, 1.0F, 1.0F);
        }

        /// <summary>
        /// Retrieves an instance of the associated effect based on the effect name.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="STSEffect"/>. If the effect name is not found in the list of known effects,
        /// a default effect of type <see cref="STSEffectFade"/> is returned.
        /// </returns>
        public STSEffect GetEffect()
        {
            STSEffect rReturn = null;
            int tIndex = STSEffectType.kEffectNameList.IndexOf(EffectName);
            if (tIndex < 0 || tIndex >= STSEffectType.kEffectNameList.Count())
            {
                rReturn = new STSEffectFade();
            }
            else
            {
                Type tEffectType = STSEffectType.kEffectTypeList[tIndex];
                rReturn = (STSEffect)Activator.CreateInstance(tEffectType);
                // Copy Param
                rReturn.CopyFrom(this);
            }

            return rReturn;
        }

        /// <summary>
        /// Represents a class for scene transition effects in the SceneTransitionSystem namespace.
        /// </summary>
        public STSEffectType(string sString, Color sColor, float sDuration)
        {
            EffectName = sString;
            TintPrimary = sColor;
            Duration = sDuration;
        }

        /// <summary>
        /// Represents a type of scene transition effect within the Scene Transition System (STS).
        /// </summary>
        public STSEffectType(STSEffectType sObject)
        {
            CopyFrom(sObject);
        }

        /// <summary>
        /// Creates a duplicate of the current <see cref="STSEffectType"/> object.
        /// </summary>
        /// <returns>A new <see cref="STSEffectType"/> object that is a copy of the current instance.</returns>
        public STSEffectType Dupplicate()
        {
            STSEffectType tCopy = new STSEffectType(this);
            return tCopy;
        }

        /// <summary>
        /// Copies the properties from another STSEffectType instance to the current instance.
        /// </summary>
        /// <param name="sObject">The STSEffectType object from which to copy the properties.</param>
        public void CopyFrom(STSEffectType sObject)
        {
            EffectName = sObject.EffectName;
            TintPrimary = sObject.TintPrimary;
            TintSecondary = sObject.TintSecondary;
            TexturePrimary = sObject.TexturePrimary;
            TextureSecondary = sObject.TextureSecondary;

            ParameterOne = sObject.ParameterOne;
            ParameterTwo = sObject.ParameterTwo;
            ParameterThree = sObject.ParameterThree;

            TwoCross = sObject.TwoCross;
            FourCross = sObject.FourCross;
            FiveCross = sObject.FiveCross;
            EightCross = sObject.EightCross;
            NineCross = sObject.NineCross;

            Clockwise = sObject.Clockwise;

            Duration = sObject.Duration;
            Purcent = sObject.Purcent;
            if (sObject.Curve != null)
            {
                Curve = new AnimationCurve(sObject.Curve.keys);
            }
            else
            {
                Curve = AnimationCurve.Linear(0.0F, 0.0F, 1.0F, 1.0F);
            }
        }

        /// <summary>
        /// Represents a scene transition effect type in the Scene Transition System.
        /// </summary>
        static STSEffectType()
        {
            Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            Type[] tAllNWDTypes = (from System.Type type in tAllTypes
                where type.IsSubclassOf(typeof(STSEffect))
                select type).ToArray();
            // reccord all the effect type
            foreach (Type tType in tAllNWDTypes)
            {
                string tEntitlement = tType.Name;
                if (tType.GetCustomAttributes(typeof(STSEffectNameAttribute), true).Length > 0)
                {
                    foreach (STSEffectNameAttribute tReference in tType.GetCustomAttributes(typeof(STSEffectNameAttribute), true))
                    {
                        tEntitlement = tReference.EffectName;
                    }
                }

                if (string.IsNullOrEmpty(tEntitlement))
                {
                    tEntitlement = tType.Name;
                }

                //MethodInfo tMethodInfo = tType.GetMethod("EffectName", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                //if (tMethodInfo != null)
                //{
                //    tEntitlement = tMethodInfo.Invoke(null, null) as string;
                //}
                kEffectContentList.Add(new GUIContent(tEntitlement));
                kEffectTypeList.Add(tType);
                kEffectNameList.Add(tEntitlement);
            }

            // Add Default
            kEffectNameList.Insert(0, "");
            kEffectTypeList.Insert(0, typeof(STSEffectFade));
            kEffectContentList.Insert(0, new GUIContent("Default"));
        }

        /// <summary>
        /// Defines explicit conversion from a UnityEngine.Object to an STSEffectType object.
        /// </summary>
        /// <param name="v">The UnityEngine.Object to be converted.</param>
        /// <returns>The converted STSEffectType object.</returns>
        /// <exception cref="NotImplementedException">Thrown when the method is not implemented.</exception>
        public static explicit operator STSEffectType(UnityEngine.Object v)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Represents a scene transition effect within the Scene Transition System.
    /// </summary>
    [STSEffectNameAttribute("Default effect")]
    // *** Remove some parameters in inspector
    // No remove
    // ***
    public class STSEffect : STSEffectType
    {
        /// <summary>
        /// Estimates the percentage value based on an animation curve.
        /// </summary>
        /// <remarks>
        /// This method assigns the evaluated value of the animation curve at the current percentage to the CurvePurcent field.
        /// </remarks>
        public void EstimateCurvePurcent()
        {
            CurvePurcent = Curve.Evaluate(Purcent);
        }

        /// <summary>
        /// Estimates the animation percentage (AnimPurcent) based on the elapsed time and the effect's duration.
        /// Updates the Purcent field based on AnimPurcent and the direction of the animation.
        /// </summary>
        public void EstimatePurcent()
        {
            if (AnimPurcent < 1.0F || AnimPurcent >= 0.0F)
            {
                AnimPurcent += (Time.deltaTime) / Duration;
                if (AnimPurcent > 1.0F)
                {
                    AnimPurcent = 1.0F;
                    //AnimIsPlaying = false;
                    AnimIsFinished = true;
                }

                if (AnimPurcent < 0.0F)
                {
                    // IMPOSSIBLE
                    AnimPurcent = 0.0F;
                    //AnimIsPlaying = false;
                    AnimIsFinished = true;
                }
            }

            switch (Direction)
            {
                case 0:
                    // no direction do nothing ... on error
                    break;
                case 1:
                    Purcent = AnimPurcent;
                    break;
                case -1:
                    Purcent = 1 - AnimPurcent;
                    break;
            }
            //Debug.Log("STSEffect EstimatePurcent() => AnimPurcent = " + AnimPurcent.ToString("F3"));
            //Debug.Log("STSEffect EstimatePurcent() => Purcent = " + Purcent.ToString("F3"));
        }

        /// <summary>
        /// Initiates the start of the effect entering transition with specified parameters. Handles the preparation and color transition, ensuring
        /// the state is set to play the effect animation.
        /// </summary>
        /// <param name="sRect">The rectangular region where the effect is applied.</param>
        /// <param name="sOldColor">The initial color before the transition effect starts.</param>
        /// <param name="sInterEffectDelay">The delay before the effect starts transitioning.</param>
        /// <param name="sEffectMoreInfos">Additional information related to the transition effect.</param>
        public void StartEffectEnter(Rect sRect, Color sOldColor, float sInterEffectDelay, STSEffectMoreInfos sEffectMoreInfos)
        {
            PrepareEffectEnter(sRect);
            ColorIsPlaying = false;
            if (sInterEffectDelay > 0)
            {
                // I need do to a color transitionpublic float 
                ColorPurcent = 0.0F;
                ColorDuration = sInterEffectDelay;
                OldColor = sOldColor;
                ColorIsPlaying = true;
                ColorIsFinished = false;
            }
            else
            {
                ColorIsFinished = true;
            }

            AnimPurcent = 0.0F;
            Direction = -1;
            AnimIsPlaying = true;
            AnimIsFinished = false;
        }

        /// <summary>
        /// Initiates the exit animation effect for a scene transition.
        /// </summary>
        /// <param name="sRect">The dimensions of the effect area.</param>
        /// <param name="sEffectMoreInfos">Additional information related to the effect.</param>
        public void StartEffectExit(Rect sRect, STSEffectMoreInfos sEffectMoreInfos)
        {
            PrepareEffectExit(sRect);
            AnimPurcent = 0.0F;
            Direction = 1;
            AnimIsPlaying = true;
            AnimIsFinished = false;
        }

        /// <summary>
        /// Pauses the current effect animation.
        /// </summary>
        /// <remarks>
        /// This method stops the ongoing animation but does not reset its progress.
        /// It can be resumed from the same point at a later time.
        /// </remarks>
        public void PauseEffect()
        {
            AnimIsPlaying = false;
        }

        /// <summary>
        /// Stops the current effect, setting its animation percentage to 100%,
        /// marking it as finished, and stopping any ongoing animation.
        /// </summary>
        public void StopEffect()
        {
            AnimPurcent = 1.0F;
            AnimIsPlaying = false;
            AnimIsFinished = true;
        }

        /// Resets the transition effect to its initial state.
        /// This method sets the animation percentage to zero and resets the direction,
        /// playing, and finished status flags to their default values. Use this method
        /// to restart or reset the effect before beginning a new transition.
        /// /
        public void ResetEffect()
        {
            AnimPurcent = 0.0F;
            Direction = 0;
            AnimIsPlaying = false;
            AnimIsFinished = false;
        }

        /// <summary>
        /// Draws the master effect, handling both color transition and animation.
        /// </summary>
        /// <param name="sRect">The rectangle area to draw within.</param>
        public void DrawMaster(Rect sRect)
        {
            //if (Event.current.type.Equals(EventType.Repaint))
            //{
            // Do drawing
            if (ColorIsFinished == false)
            {
                ColorPurcent += (Time.deltaTime) / ColorDuration;
                Color tColor = Color.Lerp(OldColor, TintPrimary, ColorPurcent);
                STSDrawQuad.DrawRect(sRect, tColor);
                if (ColorPurcent >= 1)
                {
                    ColorIsPlaying = false;
                    ColorIsFinished = true;
                }
            }
            else
            {
                if (AnimIsPlaying == true) // play animation
                {
                    // estimate purcent
                    EstimatePurcent();
                    //EstimateCurvePurcent();
                    Draw(sRect);
                }
            }
            //}
        }

        /// <summary>
        /// Prepares the effect's initial state before it starts entering.
        /// </summary>
        /// <param name="sRect">The rectangle area where the effect will be drawn.</param>
        public virtual void PrepareEffectEnter(Rect sRect)
        {
            // Prepare your datas to draw
        }

        /// <summary>
        /// Prepares the effect for exiting the scene transition. This method is intended to be overridden
        /// by derived classes to set up the necessary data for rendering the exit effect.
        /// </summary>
        /// <param name="sRect">The rectangle defining the area of the screen for the exit effect.</param>
        public virtual void PrepareEffectExit(Rect sRect)
        {
            // Prepare your datas to draw
        }

        /// <summary>
        /// Draws the effect within the specified rectangle.
        /// </summary>
        /// <param name="sRect">The rectangle area where the effect will be drawn.</param>
        public virtual void Draw(Rect sRect)
        {
            // Do drawing with purcent
        }
    }
}