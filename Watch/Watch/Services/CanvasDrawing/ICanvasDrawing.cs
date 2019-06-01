using System;
using SkiaSharp;


namespace Watch.Services.CanvasDrawing
{
    public interface ICanvasDrawing
    {
        void DrawClock(SKColor handsColor, SKColor faceColor, SKCanvas canvas, DateTime time, int heigh, int width);
    }
}
