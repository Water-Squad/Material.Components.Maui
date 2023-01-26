﻿using Material.Components.Maui.Converters;
using Microsoft.Maui.Animations;
using System.ComponentModel;
using IButton = Material.Components.Maui.Core.IButton;

namespace Material.Components.Maui;

public partial class IconButton : SKTouchCanvasView, IButton
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

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    public readonly int iconSize;

    private readonly IconButtonDrawable drawable;
    private IAnimationManager animationManager;

    public IconButton()
    {
        this.Clicked += (sender, e) => this.Command?.Execute(this.CommandParameter ?? e);
        this.drawable = new IconButtonDrawable(this);
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

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(IsEnabled))
        {
            this.ControlState = this.IsEnabled ? ControlState.Normal : ControlState.Disabled;
        }
    }
}
