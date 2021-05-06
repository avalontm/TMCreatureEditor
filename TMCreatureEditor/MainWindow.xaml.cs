using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

        TMCreature _creature;

        public TMCreature creature
        {
            get { return _creature; }
            set { _creature = value;
                OnPropertyChanged("creature");
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

                // Texturas //
                texture1.Source = creature.dirs[0].textures[0].ToImage();
                texture2.Source = creature.dirs[0].textures[1].ToImage();
                texture3.Source = creature.dirs[0].textures[2].ToImage();
                texture4.Source = creature.dirs[0].textures[3].ToImage();

                // Mascaras //
                mask1.Source = creature.dirs[0].masks[0].ToImage();
                mask2.Source = creature.dirs[0].masks[1].ToImage();
                mask3.Source = creature.dirs[0].masks[2].ToImage();
                mask4.Source = creature.dirs[0].masks[3].ToImage();
            }
        }

        void onSave(object sender, RoutedEventArgs e)
        {

        }

        void onExit(object sender, RoutedEventArgs e)
        {

        }

    }
}
