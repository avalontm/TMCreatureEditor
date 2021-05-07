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

        ObservableCollection<TMSprite> _sprites;

        public ObservableCollection<TMSprite> sprites
        {
            get { return _sprites; }
            set
            {
                _sprites = value;
                OnPropertyChanged("sprites");
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
            onNew(sender, e);
        }

        void onUnloaded(object sender, RoutedEventArgs e)
        {

        }

        void onNew(object sender, RoutedEventArgs e)
        {
            creature = new TMCreature();
            creature.name = "creatura";
            Title = $"{creature.name} - [sin guardar]";
            onLoadCreatureDir();
            onLoadDirSprites();
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

                Title = $"{creature.name} - {FileCreature}";
                DirIndex = 0;
                SpriteIndex = 0;
                onLoadCreatureDir();
                onLoadDirSprites();
            }
        }

        void onLoadDirSprites()
        {
            sprites = new ObservableCollection<TMSprite>();
            int index = 0;
            foreach (var spr in creature.dirs[DirIndex].sprites)
            {
                index++;
                TMSprite sprite = new TMSprite();
                sprite.title = $"sprite_{index}";
                sprite.image = spr.textures[0].ToImage();

                sprites.Add(sprite);
            }

            lstSprites.ItemsSource = sprites;
        }

        void onLoadCreatureDir()
        {
            if (creature == null)
            {
                return;
            }

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

        void onSelectSpriteCahnged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSprites.SelectedIndex >= 0)
            {
                SpriteIndex = lstSprites.SelectedIndex;
            }
        }

        void onImportTexture(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "images files (*.png)|*.png|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    Debug.WriteLine($"[onImportTexture] {openFileDialog.FileName}");
                    byte[] _bytes = TMImageHelper.FromFile(openFileDialog.FileName);
                    Debug.WriteLine($"[TEXTURE] {_bytes.Length}");
                    creature.dirs[DirIndex].sprites[SpriteIndex].textures[0] = _bytes;
                   
                    texture1.Source = creature.dirs[DirIndex].sprites[SpriteIndex].textures[0].ToImage();
                }
            }

        }
    }
}
