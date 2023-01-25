﻿using Material.Components.Maui.Converters;
using Microsoft.Maui.Animations;
using System.ComponentModel;
using System.Windows.Input;
using Topten.RichTextKit;

namespace Material.Components.Maui;

public partial class Chip
    : SKTouchCanvasView,
        IView,
        IForegroundElement,
        IOutlineElement,
        IBackgroundElement,
        IIconElement,
        IElevationElement,
        IShapeElement,
        IStateLayerElement,
        IRippleElement,
        ITextElement
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
            ControlState.Normal => this.IsChecked ? "normal:actived" : "normal",
            ControlState.Hovered => this.IsChecked ? "hovered:actived" : "hovered",
            ControlState.Pressed => this.IsChecked ? "pressed:actived" : "pressed",
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

    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(IIconElement.Icon),
        typeof(IconKind),
        typeof(IIconElement),
        IconKind.None,
        propertyChanged: OnIconChanged
    );

    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IIconElement.IconSource),
        typeof(SKPicture),
        typeof(IIconElement),
        null,
        propertyChanged: OnIconChanged
    );

    private static void OnIconChanged(BindableObject bo, object oldValue, object newValue)
    {
        var chip = bo as Chip;
        if (
            oldValue is IconKind.None
            || newValue is IconKind.None
            || oldValue is null
            || newValue is null
        )
        {
            chip.SendInvalidateMeasure();
        }
        else
        {
            chip.InvalidateSurface();
        }
    }

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

    [AutoBindable(OnChanged = nameof(OnIsCheckedChanged))]
    private readonly bool isChecked;

    [AutoBindable(OnChanged = nameof(OnHasCloseIconChanged))]
    private readonly bool hasCloseIcon;

    [AutoBindable(OnChanged = nameof(OnPropertyChanged))]
    private readonly Color iconColor;

    [AutoBindable]
    private readonly ICommand command;

    [AutoBindable]
    private readonly object commandParameter;

    public event EventHandler<CheckedChangedEventArgs> ChangedChanged;

    private void OnIsCheckedChanged()
    {
        this.ChangeVisualState();
        this.CheckedChanged?.Invoke(this, new CheckedChangedEventArgs(this.IsChecked));
    }

    private void OnHasCloseIconChanged()
    {
        this.SendInvalidateMeasure();
        this.OnPropertyChanged();
    }

    public event EventHandler<CheckedChangedEventArgs> CheckedChanged;
    public event EventHandler Closed;

    internal float ChangingPercent { get; private set; } = 1f;
    private readonly ChipDrawable drawable;
    private IAnimationManager animationManager;

    public Chip()
    {
        this.Clicked += (sender, e) =>
        {
            if (this.HasCloseIcon && e.Location.X >= this.CanvasSize.Width - 34)
            {
                this.Closed?.Invoke(this, e);
                if (this.Parent is Layout parent)
                {
                    parent.Remove(this);
                }
            }
            else
            {
                this.Command?.Execute(this.CommandParameter ?? e);
            }
        };
        this.drawable = new ChipDrawable(this);
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
        var iconWidth =
            (this.HasCloseIcon ? 18d : 0d)
            + (this.Icon != IconKind.None || this.IconSource != null ? 18d : 0d);
        this.InternalText.MaxWidth = (float)(maxWidth - 32d - iconWidth);
        this.InternalText.MaxHeight = (float)(maxHeight - this.Margin.VerticalThickness);
        var width =
            this.Margin.HorizontalThickness
            + Math.Max(
                this.MinimumWidthRequest,
                this.WidthRequest is -1
                    ? Math.Min(maxWidth, this.InternalText.MeasuredWidth + 32d + iconWidth)
                    : this.WidthRequest
            );
        var height =
            this.Margin.VerticalThickness
            + Math.Max(
                this.MinimumHeightRequest,
                this.HeightRequest != -1 ? Math.Min(maxHeight, 32d) : this.HeightRequest
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

    protected override void OnParentChanged()
    {
        base.OnParentChanged();
    }
}
