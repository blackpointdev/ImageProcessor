﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ImageProcessor.ImageUtil;
using ImageProcessor.GaussianBlur;
using ImageProcessor.Brightness;

namespace ImageProcessor
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap img;
        private GaussianBlurEffect gauss;
        private BrightnessEffect brightness;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                // Update interface
                ImageLabel.Height = 0;
                saveMenuBtn.IsEnabled = true;
                GaussianButton.IsEnabled = true;
                BrigtnessButton.IsEnabled = true;

                // Load image to bitmap
                img = new Bitmap(dialog.FileName);

                // Creating effects objects
                gauss = new GaussianBlurEffect(ref img);
                brightness = new BrightnessEffect(ref img);


                // Setting display image
                CachedImage.Source = new IPImage(img).BitmapToImageSource();
            }
        }

        private void SaveMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG image (*.jpg)|*.jpg|PNG image(*.png)|*.png|All files (*.*)|*.*";
            dialog.Title = "Save as";

            if (dialog.ShowDialog() == true)
            {
                string path = dialog.FileName;
                img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GaussianButton_Click(object sender, RoutedEventArgs e)
        {
            float sigma = (float)SigmaSlider.Value;
            uint kernel = (uint)KernelSlider.Value;

            gauss.CalculateKernel(kernel, sigma);
            gauss.ApplyEffect();

            // Updating displayed image
            CachedImage.Source = new IPImage(img).BitmapToImageSource();
        }

        private void BrigtnessButton_Click(object sender, RoutedEventArgs e)
        {
            brightness.ApplyEffect((int)BrigtnessSlider.Value);

            // Updating displayed image
            CachedImage.Source = new IPImage(img).BitmapToImageSource();
        }
    }
}
