using System;
using SkiaSharp;


namespace Watch.Services.CanvasDrawing
{
    public class CanvasDrawing : ICanvasDrawing
    {
        private SKPaint face = new SKPaint()
        {
            TextSize = 50,
            TextAlign = SKTextAlign.Center,
        };

        private SKPaint background = new SKPaint()
        {
            Color = SKColors.Black
        };

        private SKPaint hands = new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            StrokeCap = SKStrokeCap.Round,
        };

        public void DrawClock(SKColor handsColor, SKColor faceColor, SKCanvas canvas, DateTime time, int heigh, int width)
        {
            DrawBackground(canvas, heigh, width);
            DrawFace(canvas, heigh, width, faceColor);
            DrawHands(canvas, time, handsColor, heigh, width);
        }

        private void DrawBackground(SKCanvas canvas, int heigh, int width)
        {
            float radius = width / 8 * 3;
            canvas.DrawCircle(new SKPoint() { X = width / 2, Y = heigh / 2 }, radius * 1.2f, background);
        }

        private void DrawFace(SKCanvas canvas, int heigh, int width, SKColor faceColor)
        {
            face.Color = faceColor;
            float radius = width / 8 * 3;
            canvas.DrawText("12", width / 2, heigh / 2 - radius + face.TextSize / 2, face);
            canvas.DrawText("6", width / 2, heigh / 2 + radius + face.TextSize / 2, face);
            canvas.DrawText("3", width / 2 + radius, heigh / 2 + face.TextSize / 2, face);
            canvas.DrawText("9", width / 2 - radius, heigh / 2 + face.TextSize / 2, face);
        }

        private void DrawHands(SKCanvas canvas, DateTime dateTime, SKColor handsColor, int heigh, int width)
        {
            hands.Color = handsColor;
            canvas.Translate(width / 2f, heigh / 2f);
            canvas.Scale(Math.Min(width / 200f, heigh / 200f));

            // Hours hand
            hands.StrokeWidth = 6;
            canvas.Save();
            canvas.RotateDegrees(30 * dateTime.Hour + dateTime.Minute / 2f);
            canvas.DrawLine(0, 0, 0, -50, hands);
            canvas.Restore();

            // Minute hand
            hands.StrokeWidth = 4;
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Minute + dateTime.Second / 10f);
            canvas.DrawLine(0, 0, 0, -70, hands);
            canvas.Restore();

            // Second hand
            hands.StrokeWidth = 2;
            canvas.Save();
            canvas.RotateDegrees(6 * dateTime.Second);
            canvas.DrawLine(0, 10, 0, -80, hands);
            canvas.Restore();
        }
    }
}
