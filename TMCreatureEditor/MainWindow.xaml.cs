using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        void onLoaded(object sender, RoutedEventArgs e)
        {
            creature = new TMCreature();
        }

        void onUnloaded(object sender, RoutedEventArgs e)
        {

        }

        void onNew(object sender, RoutedEventArgs e)
        {

        }

        void onOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TMF files (*.tmc)|*.tmc|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string _file = openFileDialog.FileName;

                if (!File.Exists(_file))
                {
                    return;
                }

                creature = TMCreature.Load(_file);

                if (creature == null)
                {
                    return;
                }

                Title = $"{creature.name} - {_file}";

                onLoadCreatureDir();

                onLoadDirSprites();
            }
        }

        void onLoadDirSprites()
        {
            sprites = new ObservableCollection<TMSprite>();
            int index = 0;
            foreach (var texture in creature.dirs[DirIndex].textures)
            {
                index++;
                TMSprite sprite = new TMSprite();
                sprite.title = $"sprite_{index}";
                sprite.image = texture.ToImage();

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
            texture1.Source = creature.dirs[DirIndex].textures[0].ToImage();
            texture2.Source = creature.dirs[DirIndex].textures[1].ToImage();
            texture3.Source = creature.dirs[DirIndex].textures[2].ToImage();
            texture4.Source = creature.dirs[DirIndex].textures[3].ToImage();

            // Mascaras //
            mask1.Source = creature.dirs[DirIndex].masks[0].ToImage();
            mask2.Source = creature.dirs[DirIndex].masks[1].ToImage();
            mask3.Source = creature.dirs[DirIndex].masks[2].ToImage();
            mask4.Source = creature.dirs[DirIndex].masks[3].ToImage();
        }

        void onSave(object sender, RoutedEventArgs e)
        {

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

        }
    }
}
