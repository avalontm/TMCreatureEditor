using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMCreatureEditor.Enums;
using TMCreatureEditor.Helpers;
using TMCreatureEditor.Models;
using TMFormat.Formats;
using TMFormat.Helpers;

namespace TMCreatureEditor
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Propiedades

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        ObservableCollection<TMLook> _sprites;

        public ObservableCollection<TMLook> sprites
        {
            get { return _sprites; }
            set
            {
                _sprites = value;
                OnPropertyChanged("sprites");
            }
        }

        ObservableCollection<TMLoot> _loots;

        public ObservableCollection<TMLoot> loots
        {
            get { return _loots; }
            set
            {
                _loots = value;
                OnPropertyChanged("loots");
            }
        }

        TMCreature _creature;

        public TMCreature creature
        {
            get { return _creature; }
            set { _creature = value;
                OnPropertyChanged("creature");
            }
        }

        int _dirIndex;

        public int DirIndex
        {
            get { return _dirIndex; }
            set
            {
                _dirIndex = value;
                OnPropertyChanged("DirIndex");
            }
        }

        int _spriteIndex;

        public int SpriteIndex
        {
            get { return _spriteIndex; }
            set
            {
                _spriteIndex = value;
                OnPropertyChanged("SpriteIndex");
            }
        }

        string _fileCreature;

        public string FileCreature
        {
            get { return _fileCreature; }
            set
            {
                _fileCreature = value;
                OnPropertyChanged("FileCreature");
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        void onLoaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(App.file))
            {
                FileCreature = App.file;
                creature = TMCreature.Load(FileCreature);

                if (creature == null)
                {
                    MessageBox.Show(this, "No se pudo cargar el archivo.\nFormato desconocido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Title = $"{creature.name} - [{FileCreature}]";
                onLoadCreature();
                return;
            }
            onNew(sender, e);
        }

        void onUnloaded(object sender, RoutedEventArgs e)
        {

        }

        void onNew(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(FileCreature))
            {
                var result = MessageBox.Show(this,"¿Desea crear una nueva creatura?" , "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            FileCreature = string.Empty;
            creature = new TMCreature();
            creature.name = "creatura";
            Title = $"{creature.name} - [sin guardar]";
            onLoadCreature();
        }

        void onOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TMF files (*.tmc)|*.tmc|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                FileCreature = openFileDialog.FileName;

                if (!File.Exists(FileCreature))
                {
                    return;
                }

                creature = TMCreature.Load(FileCreature);

                if (creature == null)
                {
                    MessageBox.Show(this, "No se pudo cargar el archivo.\nFormato desconocido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Title = $"{creature.name} - [{FileCreature}]";
                onLoadCreature();
            }
        }

        void onLoadCreature()
        {
            DirIndex = 0;
            SpriteIndex = 0;
            onLoadCreatureDir();
            onLoadDirSprites();
            onLoadLoots();
        }

        void onLoadDirSprites()
        {
            sprites = new ObservableCollection<TMLook>();
            int index = 0;
            foreach (var spr in creature.dirs[DirIndex].sprites)
            {
                index++;
                TMLook sprite = new TMLook();
                sprite.title = $"sprite_{index}";
                sprite.image = spr.textures[0].ToImage();

                sprites.Add(sprite);
            }

            lstSprites.ItemsSource = sprites;
        }

        async void onLoadCreatureDir()
        {
            if (creature == null)
            {
                return;
            }

            gridWait.Visibility = Visibility.Visible;
            await Task.Delay(10);

            // Texturas //
            texture1.Source = creature.dirs[DirIndex].sprites[SpriteIndex].textures[0].ToImage();
            texture2.Source = creature.dirs[DirIndex].sprites[SpriteIndex].textures[1].ToImage();
            texture3.Source = creature.dirs[DirIndex].sprites[SpriteIndex].textures[2].ToImage();
            texture4.Source = creature.dirs[DirIndex].sprites[SpriteIndex].textures[3].ToImage();

            // Mascaras //
            mask1.Source = creature.dirs[DirIndex].sprites[SpriteIndex].masks[0].ToImage();
            mask2.Source = creature.dirs[DirIndex].sprites[SpriteIndex].masks[1].ToImage();
            mask3.Source = creature.dirs[DirIndex].sprites[SpriteIndex].masks[2].ToImage();
            mask4.Source = creature.dirs[DirIndex].sprites[SpriteIndex].masks[3].ToImage();

            gridWait.Visibility = Visibility.Hidden;
            await Task.Delay(10);
            Debug.WriteLine($"[onLoadCreatureDir] {DirIndex} => {SpriteIndex}");
        }

        void onSave(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(FileCreature))
            {
                onSaveFile();
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TMC file (*.tmc)|*.tmc";

            if (saveFileDialog.ShowDialog() == true)
            {
                FileCreature = saveFileDialog.FileName;
                onSaveFile();
            }
        }

        void onSaveFile()
        {
            bool result = creature.SaveToFile(FileCreature);

            if (result)
            {
                MessageBox.Show(this, "El archivo se a guardado correctamente.", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(this, "El archivo no se ha podido guardar.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void onExit(object sender, RoutedEventArgs e)
        {

        }

        void onDirSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmbBox = sender as ComboBox;
            DirIndex = cmbBox.SelectedIndex;
            onLoadCreatureDir();
        }

        void onSelectSpriteChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSprites.SelectedIndex >= 0)
            {
                SpriteIndex = lstSprites.SelectedIndex;
                onLoadCreatureDir();
            }
        }

        void onImportTexture1(object sender, MouseButtonEventArgs e)
        {
            onImportTextures(texture1, SlootEnum.Texture, 0);
        }

        void onImportTexture2(object sender, MouseButtonEventArgs e)
        {
            onImportTextures(texture2, SlootEnum.Texture, 1);
        }

        void onImportTexture3(object sender, MouseButtonEventArgs e)
        {
            onImportTextures(texture3, SlootEnum.Texture, 2);
        }

        void onImportTexture4(object sender, MouseButtonEventArgs e)
        {
            onImportTextures(texture4, SlootEnum.Texture, 3);
        }

        void onImportMask1(object sender, MouseButtonEventArgs e)
        {
            onImportTextures(mask1, SlootEnum.Mask, 0);
        }

        void onImportMask2(object sender, MouseButtonEventArgs e)
        {
            onImportTextures(mask2, SlootEnum.Mask, 1);
        }

        void onImportMask3(object sender, MouseButtonEventArgs e)
        {
            onImportTextures(mask3, SlootEnum.Mask, 2);
        }

        void onImportMask4(object sender, MouseButtonEventArgs e)
        {
            onImportTextures(mask4, SlootEnum.Mask, 3);
        }

        void onImportTextures(Image source, SlootEnum sloot, int index)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "images files (*.png, *.bmp)|*.png; *.bmp;";

            if (openFileDialog.ShowDialog() == true)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    onLoadTextureFromFile(source, sloot, index, openFileDialog.FileName);
                }
            }
        }

        void onLoadTextureFromFile(Image source, SlootEnum sloot, int index, string file)
        {
            byte[] _bytes = TMImageHelper.FromFile(file, true);

            switch (sloot)
            {
                case SlootEnum.Texture:
                    {
                        creature.dirs[DirIndex].sprites[SpriteIndex].textures[index] = _bytes;
                        if (creature.dirs[DirIndex].sprites[SpriteIndex].textures[index] != null)
                        {
                            source.Source = creature.dirs[DirIndex].sprites[SpriteIndex].textures[index].ToImage();
                        }
                    }
                    break;
                case SlootEnum.Mask:
                    {
                        creature.dirs[DirIndex].sprites[SpriteIndex].masks[index] = _bytes;
                        if (creature.dirs[DirIndex].sprites[SpriteIndex].masks[index] != null)
                        {
                            source.Source = creature.dirs[DirIndex].sprites[SpriteIndex].masks[index].ToImage();
                        }
                    }
                    break;
            }
        }

        void onLootNew(object sender, RoutedEventArgs e)
        {
            creature.loots.Add(new TMCreatureLoot());
            onLoadLoots();
        }

        void onLoadLoots()
        {
            if (loots == null)
            {
                loots = new ObservableCollection<TMLoot>();
            }

            loots.Clear();

            foreach (var loot in creature.loots)
            {
                loots.Add(new TMLoot() { id = loot.id, units = loot.count, drop = loot.probability });
            }

            lstLoot.ItemsSource = loots;
        }

        void onLootDelete(object sender, RoutedEventArgs e)
        {
            TMLoot loot = (sender as Button).DataContext as TMLoot;

            if (loot != null)
            {
                var result = MessageBox.Show(this, "¿Desea eliminar este loot?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                int index = loots.IndexOf(loot);

                creature.loots.RemoveAt(index);
                onLoadLoots();
            }
        }

        void onLootSave(object sender, RoutedEventArgs e)
        {
            TMLoot loot = (sender as Button).DataContext as TMLoot;

            if (loot != null)
            {
                int index = loots.IndexOf(loot);

                creature.loots[index].id = loot.id;
                creature.loots[index].count = loot.units;
                creature.loots[index].probability = loot.drop;
                onLoadLoots();
            }
        }

        void onTextureDrop(object sender, DragEventArgs e)
        {
            Border border = sender as Border;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                string file = files[0];
                string ext = System.IO.Path.GetExtension(file);

                if (ext == ".png" || ext == ".bmp")
                {
                    switch (int.Parse(border.Tag.ToString()))
                    {
                        case 1:
                            onLoadTextureFromFile(texture1, SlootEnum.Texture, 0, file);
                            break;
                        case 2:
                            onLoadTextureFromFile(texture2, SlootEnum.Texture, 1, file);
                            break;
                        case 3:
                            onLoadTextureFromFile(texture3, SlootEnum.Texture, 2, file);
                            break;
                        case 4:
                            onLoadTextureFromFile(texture4, SlootEnum.Texture, 3, file);
                            break;
                    }
                }

            }
        }

        void onMaskDrop(object sender, DragEventArgs e)
        {
            Border border = sender as Border;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                string file = files[0];
                string ext = System.IO.Path.GetExtension(file);

                if (ext == ".png" || ext == ".bmp")
                {
                    switch (int.Parse(border.Tag.ToString()))
                    {
                        case 1:
                            onLoadTextureFromFile(mask1, SlootEnum.Mask, 0, file);
                            break;
                        case 2:
                            onLoadTextureFromFile(mask2, SlootEnum.Mask, 1, file);
                            break;
                        case 3:
                            onLoadTextureFromFile(mask3, SlootEnum.Mask, 2, file);
                            break;
                        case 4:
                            onLoadTextureFromFile(mask4, SlootEnum.Mask, 3, file);
                            break;
                    }
                }

            }
        }
        void onTextureDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
        }

        void onDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        void onDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        void onFileAssosiate(object sender, RoutedEventArgs e)
        {
            /*
            if (!FileAssociation.IsAssociated(".tmc"))
            {
               
            }
            */
            string myExe = Process.GetCurrentProcess().MainModule.FileName;
            FileAssociation.Associate(".tmc", "tmcfile", "tmc File", myExe, myExe);
        }
    }
}
