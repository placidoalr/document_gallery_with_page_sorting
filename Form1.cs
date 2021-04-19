﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace document_gallery_with_page_sorting
{
    public partial class Form1 : Form
    {
        // selected image index, from the listview
        private int SelectedImageIndex = 0;
        private List<Image> LoadedImages { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadImagesFromFolder(string[] paths)
        {
            LoadedImages = new List<Image>();
            foreach(var path in paths)
            {
                var tempImage = Image.FromFile(path);
                LoadedImages.Add(tempImage);
            }
            if(LoadedImages.Count > 0)
            {
                label1.Text = $"( 1 / {LoadedImages.Count} )";
            }
        }

        private void imageList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (imageList.SelectedIndices.Count > 0)
            {
                var selectedIndex = imageList.SelectedIndices[0];
                Image selectedImg = LoadedImages[selectedIndex];
                selectedImage.Image = selectedImg;
                SelectedImageIndex = selectedIndex;
                label1.Text = $"( {selectedIndex + 1} / {LoadedImages.Count} )";
            }
        }

        private void button_navigation(object sender, EventArgs e)
        {
            var clickedButton = sender as Button;
            if (clickedButton.Name.Equals("button1"))
            {
                if (SelectedImageIndex > 0)
                {
                    SelectedImageIndex -= 1;
                    label1.Text = $"( {SelectedImageIndex + 1} / {LoadedImages.Count} )";
                    Image selectedImg = LoadedImages[SelectedImageIndex];
                    selectedImage.Image = selectedImg;
                    SelectTheClickedItem(imageList, SelectedImageIndex);
                }

            } else
            {
                if (SelectedImageIndex < (LoadedImages.Count - 1 ))
                {
                    SelectedImageIndex += 1;
                    label1.Text = $"( {SelectedImageIndex + 1} / {LoadedImages.Count} )";
                    Image selectedImg = LoadedImages[SelectedImageIndex];
                    selectedImage.Image = selectedImg;
                    SelectTheClickedItem(imageList, SelectedImageIndex);
                }
            }
        }

        private void SelectTheClickedItem(ListView list, int index)
        {
            for(int item = 0; item < list.Items.Count; item++)
            {
                if(item == index)
                {
                    list.Items[item].Selected = true;
                } else
                {
                    list.Items[item].Selected = false;
                }
            }
            
        }

        private void selectDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                // selected directory
                var selectedDirectory = folderBrowser.SelectedPath;
                // images paths from selected directory
                var imagePaths = Directory.GetFiles(selectedDirectory);
                // loading images from images paths
                LoadImagesFromFolder(imagePaths);

                PreiewImages();

            }
            
        }

        public void PreiewImages()
        {

            // initializing images list
            ImageList images = new ImageList();
            images.ImageSize = new Size(130, 180);

            foreach (var image in LoadedImages)
            {
                images.Images.Add(image);
            }

            imageList.Items.Clear();

            // setting our listview with the imagelist
            imageList.LargeImageList = images;

            for (int itemIndex = 0; itemIndex < LoadedImages.Count; itemIndex++)
            {
                imageList.Items.Add(new ListViewItem($"Página {itemIndex}", itemIndex ));
            }
        }

        // UP or Before [ ^ ]
        private void button4_Click(object sender, EventArgs e)
        {
            if (LoadedImages.Count > 0 && SelectedImageIndex >= 1)
            {
                var destino = SelectedImageIndex - 1;
                var imagemFinal = LoadedImages[SelectedImageIndex - 1];

                LoadedImages.Insert(destino, LoadedImages[SelectedImageIndex]);
                LoadedImages.RemoveAt(SelectedImageIndex);
                LoadedImages.Insert(SelectedImageIndex, imagemFinal);
                LoadedImages.RemoveAt(destino + 2);

                PreiewImages();

                SelectedImageIndex -= 1;
                label1.Text = $"( {SelectedImageIndex + 1} / {LoadedImages.Count} )";
                Image selectedImg = LoadedImages[SelectedImageIndex];
                selectedImage.Image = selectedImg;
                SelectTheClickedItem(imageList, SelectedImageIndex);
            }
        }
        
        //  DOWN or After [ v ] 
        private void button5_Click_1(object sender, EventArgs e)
        {
            if (SelectedImageIndex + 1 < LoadedImages.Count)
            {
                var destino = SelectedImageIndex + 1;
                var imagemFinal = LoadedImages[SelectedImageIndex + 1];

                LoadedImages.RemoveAt(destino);
                LoadedImages.Insert(destino, LoadedImages[SelectedImageIndex]);
                LoadedImages.Insert(SelectedImageIndex, imagemFinal);
                LoadedImages.RemoveAt(destino + 1);

                PreiewImages();

                SelectedImageIndex += 1;
                label1.Text = $"( {SelectedImageIndex + 1} / {LoadedImages.Count} )";
                Image selectedImg = LoadedImages[SelectedImageIndex];
                selectedImage.Image = selectedImg;
                SelectTheClickedItem(imageList, SelectedImageIndex);
            }
        }
    }
}
