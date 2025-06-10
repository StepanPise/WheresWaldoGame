using System;
using Microsoft.Maui.Graphics;

namespace MauiApp1.Models
{
    public class Smiley
    {
        public string FaceImage { get; set; }
        public string HatImage { get; set; }
        public string AccessoryImage { get; set; }
        public string BackgroundImage { get; set; }

        public bool IsWaldo { get; set; }

        public Smiley(string faceImage, string hatImage, string accessoryImage, bool isWaldo, string backgroundImage)
        {
            FaceImage = faceImage;
            HatImage = hatImage;
            AccessoryImage = accessoryImage;
            IsWaldo = isWaldo;
            BackgroundImage = backgroundImage;
        }
    }
}
