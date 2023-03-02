using Microsoft.Maui.Graphics;
using System.Diagnostics;
using SkiaSharp;
using Microsoft.Maui.Controls;
using System.Reflection;
using System.IO;
using Microsoft.Maui.Devices;

namespace MauiAppAndroidDrawImage.Controls;


public class ViewCtrl : ScrollView
{
    private GraphicsView graphics_view_ = null;
    private ViewDrawable drawable_ = null;
    private StackLayout layout_ = null;
    private float density_ = (float)DeviceDisplay.Current.MainDisplayInfo.Density;

    public ViewCtrl()
    {
        graphics_view_ = new GraphicsView();
        drawable_ = new ViewDrawable();
        graphics_view_.Drawable = drawable_;
        layout_ = new StackLayout();
        Content = layout_;
        layout_.Children.Add(graphics_view_);
#pragma warning disable CS0618 // 类型或成员已过时
        this.VerticalOptions = LayoutOptions.FillAndExpand;
        this.HorizontalOptions = LayoutOptions.FillAndExpand;
    }

    protected override void OnSizeAllocated(double width, double height)
    {

        if(width < height)
            Render(false);
        else
            Render(true);
    }

    public void Render(bool isHor = false)
    {
        if (drawable_ != null && graphics_view_ != null)
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("MauiAppAndroidDrawImage.Resources.Images.dotnet_bot.png"))
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                memoryStream.Position = 0;
                var bitmap = SKBitmap.Decode(memoryStream);
                var image = SKImage.FromBitmap(bitmap);
                stream.Position = 0;
                drawable_.SetData(stream);                
                if (density_ > 1)
                {
                    graphics_view_.WidthRequest = image.Width / density_;
                    graphics_view_.HeightRequest = image.Height / density_;
                    graphics_view_.Invalidate();
                }
                else
                {
                    graphics_view_.WidthRequest = image.Width;
                    graphics_view_.HeightRequest = image.Height;
                    graphics_view_.Invalidate();
                }
            }
        }
    }
}
