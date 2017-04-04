using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace AW.Common.WebToolkit
{
    public static class TextToImage
    {
        public static byte[] EMailToImage(string email)
        {
            return EMailToImage(email, "verdana", 13, Color.Black, Color.White, FontStyle.Regular, 3,
                Color.LightGray, true, true);
        }

        public static SizeF MeasureString(string text, Font f)
        {
            using (var bmp = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    return g.MeasureString(text, f);
                }
            }
        }

        public static byte[] EMailToImage(string email, string fontName, int fontSize, Color fontColor,
            Color bgColor, FontStyle fontStyle, int dropShadowLevel, Color shadowColor, bool antiAlias, bool rectangle)
        {
            using (var font = new Font(new FontFamily(fontName), fontSize, fontStyle, GraphicsUnit.Pixel))
            {
                var textSize = MeasureString(email, font);
                var width = ((int)textSize.Width) + 5;
                var height = ((int)textSize.Height) + 3;

                var rectF = new RectangleF(0, 0, width, height);
                using (var pic = new Bitmap(width, height))
                {
                    using (var graphics = Graphics.FromImage(pic))
                    {
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.High;
                        if (antiAlias) graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                        using (var fgBrush = new SolidBrush(fontColor))
                        {
                            using (var bgBrush = new SolidBrush(bgColor))
                            {
                                if (rectangle)
                                {
                                    graphics.FillRectangle(bgBrush, rectF);
                                }
                                else
                                {
                                    graphics.FillRectangle(new SolidBrush(Color.White), rectF);
                                    graphics.FillEllipse(bgBrush, rectF);
                                }

                                graphics.DrawRectangle(new Pen(Color.LightGray), new Rectangle(0, 0, width - 1, height - 1));

                                using (var format = new StringFormat())
                                {
                                    format.FormatFlags = StringFormatFlags.NoWrap;
                                    format.Alignment = StringAlignment.Center;

                                    if (dropShadowLevel > 0)
                                    {
                                        switch (dropShadowLevel)
                                        {
                                            case 1:
                                                rectF.Offset(-1, -1);
                                                graphics.DrawString(email, font, new SolidBrush(shadowColor), rectF,
                                                             format);
                                                rectF.Offset(+1, +1);
                                                break;

                                            case 2:
                                                rectF.Offset(+1, -1);
                                                graphics.DrawString(email, font, new SolidBrush(shadowColor), rectF,
                                                             format);
                                                rectF.Offset(-1, +1);
                                                break;

                                            case 3:
                                                rectF.Offset(-1, +1);
                                                graphics.DrawString(email, font, new SolidBrush(shadowColor), rectF,
                                                             format);
                                                rectF.Offset(+1, -1);
                                                break;

                                            case 4:
                                                rectF.Offset(+1, +1);
                                                graphics.DrawString(email, font, new SolidBrush(shadowColor), rectF,
                                                             format);
                                                rectF.Offset(-1, -1);
                                                break;
                                        }
                                    }

                                    graphics.DrawString(email, font, fgBrush, rectF, format);

                                    using (var memory = new MemoryStream())
                                    {
                                        pic.Save(memory, ImageFormat.Png);
                                        return memory.ToArray();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}