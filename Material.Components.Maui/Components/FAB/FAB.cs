﻿using Material.Components.Maui.Converters;
using Microsoft.Maui.Animations;
using System.ComponentModel;
using Topten.RichTextKit;

namespace Material.Components.Maui;

public partial class FAB
    : SKTouchCanvasView,
        IView,
        IIconElement,
        ITextElement,
        ITouchElement,
        IBackgroundElement,
        IForegroundElement,
        IElevationElement,
        IShapeElement,
        IRippleElement
{
    #region interface
    #region IView
    private bool isVisualStateChanging;
    private ControlState controlState = ControlState.Normal;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ControlState ControlState
    {
        get => this.controlState;
        set
        {
            this.controlState = value;
            this.ChangeVisualState();
        }
    }

    protected override void ChangeVisualState()
    {
        this.isVisualStateChanging = true;
        var state = this.ControlState switch
        {
            ControlState.Normal => "normal",
            ControlState.Hovered => "hovered",
            ControlState.Pressed => "pressed",
            ControlState.Disabled => "disabled",
            _ => "normal",
        };
        VisualStateManager.GoToState(this, state);
        this.isVisualStateChanging = false;

        if (!this.IsFocused)
            this.InvalidateSurface();
    }

    public void OnPropertyChanged()
    {
        if (this.Handler != null && !this.isVisualStateChanging)
        {
            this.InvalidateSurface();
        }
    }
    #endregion

    #region ITextElement
    public static readonly BindableProperty TextProperty = TextElement.TextProperty;
    public static readonly BindableProperty FontFamilyProperty = TextElement.FontFamilyProperty;
    public static readonly BindableProperty FontSizeProperty = TextElement.FontSizeProperty;
    public static readonly BindableProperty FontWeightProperty = TextElement.FontWeightProperty;
    public static readonly BindableProperty FontItalicProperty = TextElement.FontItalicProperty;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TextBlock InternalText { get; set; } = new();

    [EditorBrowsable(EditorBrowsableState.Never)]
    public TextStyle TextStyle { get; set; } = FontMapper.DefaultStyle.Modify();
    public string Text
    {
        get => (string)this.GetValue(TextProperty);
        set => this.SetValue(TextProperty, value);
    }
    public string FontFamily
    {
        get => (string)this.GetValue(FontFamilyProperty);
        set => this.SetValue(FontFamilyProperty, value);
    }
    public float FontSize
    {
        get => (float)this.GetValue(FontSizeProperty);
        set => this.SetValue(FontSizeProperty, value);
    }
    public int FontWeight
    {
        get => (int)this.GetValue(FontWeightProperty);
        set => this.SetValue(FontWeightProperty, value);
    }
    public bool FontItalic
    {
        get => (bool)this.GetValue(FontItalicProperty);
        set => this.SetValue(FontItalicProperty, value);
    }

    void ITextElement.OnChanged()
    {
        var oldSize = this.DesiredSize;
        this.SendInvalidateMeasure();
        if (oldSize == this.DesiredSize)
        {
            this.OnPropertyChanged();
        }
    }
    #endregion

    #region IIconElement
    public static readonly BindableProperty IconProperty = IconElement.IconProperty;
    public static readonly BindableProperty IconSourceProperty = IconElement.IconSourceProperty;
    public IconKind Icon
    {
        get => (IconKind)this.GetValue(IconProperty);
        set => this.SetValue(IconProperty, value);
    }

    [TypeConverter(typeof(IconSourceConverter))]
    public SKPicture IconSource
    {
        get => (SKPicture)this.GetValue(IconSourceProperty);
        set => this.SetValue(IconSourceProperty, value);
    }
    #endregion

    #region IForegroundElement
    public static readonly BindableProperty ForegroundColorProperty =
        ForegroundElement.ForegroundColorProperty;
    public static readonly BindableProperty ForegroundOpacityProperty =
        ForegroundElement.ForegroundOpacityProperty;
    public Color ForegroundColor
    {
        get => (Color)this.GetValue(ForegroundColorProperty);
        set => this.SetValue(ForegroundColorProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float ForegroundOpacity
    {
        get => (float)this.GetValue(ForegroundOpacityProperty);
        set => this.SetValue(ForegroundOpacityProperty, value);
    }
    #endregion

    #region IBackgroundElement
    public static readonly BindableProperty BackgroundColourProperty =
        BackgroundElement.BackgroundColourProperty;
    public static readonly BindableProperty BackgroundOpacityProperty =
        BackgroundElement.BackgroundOpacityProperty;
    public Color BackgroundColour
    {
        get => (Color)this.GetValue(BackgroundColourProperty);
        set => this.SetValue(BackgroundColourProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float BackgroundOpacity
    {
        get => (float)this.GetValue(BackgroundOpacityProperty);
        set => this.SetValue(BackgroundOpacityProperty, value);
    }
    #endregion

    #region IOutlineElement
    public static readonly BindableProperty OutlineColorProperty =
        OutlineElement.OutlineColorProperty;
    public static readonly BindableProperty OutlineWidthProperty =
        OutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineOpacityProperty =
        OutlineElement.OutlineOpacityProperty;
    public Color OutlineColor
    {
        get => (Color)this.GetValue(OutlineColorProperty);
        set => this.SetValue(OutlineColorProperty, value);
    }
    public int OutlineWidth
    {
        get => (int)this.GetValue(OutlineWidthProperty);
        set => this.SetValue(OutlineWidthProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float OutlineOpacity
    {
        get => (float)this.GetValue(OutlineOpacityProperty);
        set => this.SetValue(OutlineOpacityProperty, value);
    }
    #endregion

    #region IElevationElement
    public static readonly BindableProperty ElevationProperty = ElevationElement.ElevationProperty;
    public Elevation Elevation
    {
        get => (Elevation)this.GetValue(ElevationProperty);
        set => this.SetValue(ElevationProperty, value);
    }
    #endregion

    #region IShapeElement
    public static readonly BindableProperty ShapeProperty = ShapeElement.ShapeProperty;
    public Shape Shape
    {
        get => (Shape)this.GetValue(ShapeProperty);
        set => this.SetValue(ShapeProperty, value);
    }
    #endregion

    #region IStateLayerElement
    public static readonly BindableProperty StateLayerColorProperty =
        StateLayerElement.StateLayerColorProperty;
    public static readonly BindableProperty StateLayerOpacityProperty =
        StateLayerElement.StateLayerOpacityProperty;
    public Color StateLayerColor
    {
        get => (Color)this.GetValue(StateLayerColorProperty);
        set => this.SetValue(StateLayerColorProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float StateLayerOpacity
    {
        get => (float)this.GetValue(StateLayerOpacityProperty);
        set => this.SetValue(StateLayerOpacityProperty, value);
    }
    #endregion

    #region IRippleElement
    public static readonly BindableProperty RippleColorProperty = RippleElement.RippleColorProperty;
    public Color RippleColor
    {
        get => (Color)this.GetValue(RippleColorProperty);
        set => this.SetValue(RippleColorProperty, value);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float RippleSize { get; private set; } = 0f;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public float RipplePercent { get; set; } = 0f;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public SKPoint TouchPoint { get; set; } = new SKPoint(-1, -1);
    #endregion
    #endregion

    [AutoBindable]
    private readonly FABType fABType;

    [AutoBindable(OnChanged = nameof(OnIsExtendedChanged))]
    private readonly bool isExtended;

    public event EventHandler ExtendedChanged;

    private void OnIsExtendedChanged()
    {
        this.SendInvalidateMeasure();
        this.ExtendedChanged?.Invoke(this, EventArgs.Empty);
    }

    private readonly FABDrawable drawable;
    private IAnimationManager animationManager;

    public FAB()
    {
        this.drawable = new FABDrawable(this);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void StartRippleEffect()
    {
        this.animationManager ??=
            this.Handler.MauiContext?.Services.GetRequiredService<IAnimationManager>();
        var start = 0f;
        var end = 1f;

        this.animationManager?.Add(
            new Microsoft.Maui.Animations.Animation(
                callback: (progress) =>
                {
                    this.RipplePercent = start.Lerp(end, progress);
                    this.InvalidateSurface();
                },
                duration: 0.35f,
                easing: Easing.SinInOut,
                finished: () =>
                {
                    if (this.ControlState != ControlState.Pressed)
                    {
                        this.RipplePercent = 0f;
                        this.InvalidateSurface();
                    }
                }
            )
        );
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        this.RippleSize = e.Info.Rect.GetRippleSize(this.TouchPoint);
        this.drawable.Draw(e.Surface.Canvas, e.Info.Rect);
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var maxWidth = Math.Min(
            Math.Min(widthConstraint, this.MaximumWidthRequest),
            this.WidthRequest != -1 ? this.WidthRequest : double.PositiveInfinity
        );
        var maxHeight = Math.Min(
            Math.Min(heightConstraint, this.MaximumHeightRequest),
            this.HeightRequest != -1 ? this.HeightRequest : double.PositiveInfinity
        );
        var defaultSize =
            this.FABType is FABType.Default
                ? 56d
                : this.FABType is FABType.Small
                    ? 40d
                    : 96d;
        var blankWidth =
            this.FABType is FABType.Default
                ? 16d
                : this.FABType is FABType.Small
                    ? 8d
                    : 30d;
        this.InternalText.MaxWidth = (float)(maxWidth - blankWidth - defaultSize);
        this.InternalText.MaxHeight = (float)defaultSize;
        var width =
            this.Margin.HorizontalThickness
            + Math.Max(
                this.MinimumWidthRequest,
                this.WidthRequest is -1
                    ? Math.Min(
                        maxWidth,
                        defaultSize
                            + (this.IsExtended ? this.InternalText.MeasuredWidth + blankWidth : 0d)
                    )
                    : this.WidthRequest
            );
        var height =
            this.Margin.VerticalThickness
            + Math.Max(
                this.MinimumHeightRequest,
                this.HeightRequest is -1 ? Math.Min(maxHeight, defaultSize) : this.HeightRequest
            );
        var result = new Size(width, height);
        this.DesiredSize = result;
        return result;
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName is "IsEnabled")
        {
            this.ControlState = this.IsEnabled ? ControlState.Normal : ControlState.Disabled;
        }
    }
}
